using System;
using SudokuSolver.CSP_Solver;
using SudokuSolver.CSP_Solver.Strategies;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace SudokuSolver.CSP_Solver.Solver
{

    public class VariableAssignmentEventArgs<Tval> : EventArgs
    {
        public Variable<Tval> Variable { get; set; }
        public Tval AssignedValue { get; set; }
    }
    public class BacktrackSolver<Tval> : Solver<Tval>
    {

        public delegate void AssignedVariableEventHandler(object source, VariableAssignmentEventArgs<Tval> eventArgs);
        public delegate void AssignmentRemovedEventHandler(object source, VariableAssignmentEventArgs<Tval> eventArgs);

        public event AssignedVariableEventHandler VariableAssigned;
        public event AssignmentRemovedEventHandler AssignmentRemoved;

        private InferenceStrategy<Tval> inferenceStrategy;
        private VariableSelectionStrategy<Tval> variableSelectionStrategy;
        private DomainValueSelectionStragety<Tval> domainValueSelectionStragety;


        public BacktrackSolver(InferenceStrategy<Tval> infs = null, VariableSelectionStrategy<Tval> vss = null, DomainValueSelectionStragety<Tval> dvss = null)
        {
            inferenceStrategy = infs ?? new AC3<Tval>();
            variableSelectionStrategy = vss ?? new MinimumRemainingValues<Tval>();
            domainValueSelectionStragety = dvss ?? new UnorderedDomainValues<Tval>();
        }

        public override Assignment<Tval> Solve(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> initialAssignment = null)
        {
            initialAssignment = initialAssignment ?? new Assignment<Tval>();
            InferenceResults<Tval> preliminaryResults = inferenceStrategy.Infer(csp);

            if (preliminaryResults.IsAssignmentConsistent())
            {
                Assignment<Tval> solution = Backtrack(csp, initialAssignment);

                if (solution != null)
                {
                    OnSolutionFound(solution);
                    return solution;
                }
            }

            OnNoSolutionFound();
            return null;
        }

        private Assignment<Tval> Backtrack(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> assignment)
        {
            if (assignment.IsComplete(csp.GetVariables()))
                return assignment;

            Variable<Tval> variable = variableSelectionStrategy.SelectUnassignedVariable(csp, assignment);
            foreach (Tval value in domainValueSelectionStragety.getOrderedDomainValues(variable, assignment, csp))
            {
                assignment.Assign(variable, value);
                OnVariableAssigned(variable, value);
                InferenceResults<Tval> inference = new InferenceResults<Tval>();
                inference.StoreDomainForVariable(variable,variable.GetDomain());
                variable.UpdateDomain(new Domain<Tval>(value));

                if (assignment.IsConsistent(csp.GetConstraints()))
                {
                    inference = inferenceStrategy.Infer(csp, variable, value, inference);
                    if (inference.IsAssignmentConsistent())
                    {
                        Assignment<Tval> result = Backtrack(csp, assignment);
                        if (result != null)
                            return result;
                    }
                }

                assignment.RemoveAssignment(variable);
                OnAssignmentRemoved(variable, value);
                inference.RestoreOldDomains();
            }

            return null;
        }

        private void OnVariableAssigned(Variable<Tval> variable, Tval value)
        {
            VariableAssignmentEventArgs<Tval> args = new VariableAssignmentEventArgs<Tval>
            {
                Variable = variable,
                AssignedValue = value
            };
            VariableAssigned(this, args);
        }

        private void OnAssignmentRemoved(Variable<Tval> variable, Tval value)
        {
            VariableAssignmentEventArgs<Tval> args = new VariableAssignmentEventArgs<Tval>
            {
                Variable = variable
            };
            AssignmentRemoved(this, args);
        }

        public override void ClearEvents()
        {
            base.ClearEvents();
            VariableAssigned = null;
            AssignmentRemoved = null;
        }

        // we assume that, if the variable has already been assigned,
        // the old value of the variable has been removed
        // by an inference at a previous step, and therefore it is not
        // present in the domain of its neighbours, but since we might
        // have another variable that has the same value in its neighbour
        // (the assignment was not consistent) there is going to be another
        // inference to rule out inconsistent domains.

        public override InferenceResults<Tval> UpdateVariable(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> assignment, Variable<Tval> variable, Tval value)
        {
            if (csp == null)
                throw new ArgumentNullException("csp");
            if (assignment == null)
                throw new ArgumentNullException("assignment");
            if (variable == null)
                throw new ArgumentNullException("variable");

            InferenceResults<Tval> removeInference = RemoveVariable(csp, assignment, variable);
            
            assignment.Assign(variable, value);
            variable.UpdateDomain(new Domain<Tval>(value));
            return inferenceStrategy.Infer(csp, variable, value, removeInference, false);
        }

        public override InferenceResults<Tval> RemoveVariable(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> assignment, Variable<Tval> variable, Domain<Tval> default_domain = null)
        {
            if (csp == null)
                throw new ArgumentNullException("csp");
            if (assignment == null)
                throw new ArgumentNullException("assignment");
            if (variable == null)
                throw new ArgumentNullException("variable");

            if (assignment.HasBeenAssigned(variable))
            {
                Tval oldValue = assignment.ValueOf(variable);
                assignment.RemoveAssignment(variable);
                List<Variable<Tval>> neighbours = csp.GetNeighboursOf(variable);
                foreach (Variable<Tval> v in neighbours)
                {
                    // to preserve consistency of the domains it is neccessary
                    // to add back the old value only when it's fitting
                    if (!assignment.HasBeenAssigned(v) || assignment.ValueOf(v).Equals(oldValue))
                        v.GetDomain().GetValues().Add(oldValue);
                }
                variable.UpdateDomain(default_domain ?? new Domain<Tval>());
            }
            return inferenceStrategy.Infer(csp, false);
            // there are difficult edge cases that an inference limited
            // on the neighbours can't solve
            // return inferenceStrategy.Infer(csp, variable, default(Tval), null, false);
        }
    }
}
