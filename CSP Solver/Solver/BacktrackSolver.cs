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
            InferenceResults<Tval> preliminaryResults= inferenceStrategy.Infer(csp);

            if (preliminaryResults.IsAssignmentConsistent())
            {
                foreach (Variable<Tval> variable in csp.GetVariables())
                {
                    GD.Print(variable.ToString());
                }
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
                //Console.WriteLine("-------------");
                //foreach (var v in csp.GetVariables()) Console.WriteLine("["+v.ToString());
                inference.RestoreOldDomains();
                //Console.WriteLine("-------------");
                //foreach (var v in csp.GetVariables()) Console.WriteLine("{"+v.ToString());
            }

            return null;
        }

        private void OnVariableAssigned(Variable<Tval> variable, Tval value)
        {
            GD.Print("Variable assigned: " + variable.ToString());
            GD.Print("Selected Value: " + value.ToString());
            VariableAssignmentEventArgs<Tval> args = new VariableAssignmentEventArgs<Tval>();
            args.Variable = variable;
            args.AssignedValue = value;
            VariableAssigned(this, args);
        }

        private void OnAssignmentRemoved(Variable<Tval> variable, Tval value)
        {
            GD.Print("Variable removed: " + variable.ToString()+" - "+value);
            VariableAssignmentEventArgs<Tval> args = new VariableAssignmentEventArgs<Tval>();
            args.Variable = variable;
            AssignmentRemoved(this, args);
        }

        public override void ClearEvents()
        {
            base.ClearEvents();
            VariableAssigned = null;
            AssignmentRemoved = null;
        }

    }
}
