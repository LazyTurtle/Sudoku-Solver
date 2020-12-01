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
    public class BacktrackSolver : Solver
    {
        private InferenceStrategy inferenceStrategy;
        private VariableSelectionStrategy variableSelectionStrategy;
        private DomainValueSelectionStragety domainValueSelectionStragety;

        
        public BacktrackSolver(InferenceStrategy infs = null, VariableSelectionStrategy vss = null, DomainValueSelectionStragety dvss = null)
        {
            inferenceStrategy = infs ?? new AC3();
            variableSelectionStrategy = vss ?? new MinimumRemainingValues();
            domainValueSelectionStragety = dvss ?? new UnorderedDomainValues();

        }

        public Assignment backtrack(ConstraintSatisfactionProblem csp, Assignment assignment)
        {
            if (assignment.isComplete(csp.getVariables()))
                return assignment;

            Variable variable = variableSelectionStrategy.selectUnassignedVariable(csp, assignment);
            foreach(object value in domainValueSelectionStragety.getOrderedDomainValues(variable, assignment, csp))
            {
                assignment.assign(variable, value);
                int int_value = (int)value;
                Console.WriteLine("assigned: " + int_value);
                InferenceResults inference = new InferenceResults();
                if (assignment.IsConsistent(csp.getConstraints()))
                {
                    inference = inferenceStrategy.infer(csp, variable, value);
                    if (true)
                    {
                        Assignment result = backtrack(csp, assignment);
                        if (result != null)
                            return result;
                    }
                }
                assignment.removeAssignment(variable);
                inference.restoreOldDomains();
                Console.WriteLine("removed: " + value);
            }

            return null;
        }

        public override Assignment solve(ConstraintSatisfactionProblem csp, Assignment initial_assignment = null)
        {
            initial_assignment = initial_assignment ?? new Assignment();
            InferenceResults initialResults = inferenceStrategy.infer(csp, null, null);
            if(initialResults.isAssignmentConsistent())
                return backtrack(csp, initial_assignment);
            return null;
        }
    }

    public class BacktrackSolver<Tval> : Solver<Tval>
    {
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
                Console.WriteLine(variable.ToString());
                Console.WriteLine("assigned: " + value.ToString());
                InferenceResults<Tval> inference = new InferenceResults<Tval>();
                if (assignment.IsConsistent(csp.GetConstraints()))
                {
                    inference = inferenceStrategy.Infer(csp, variable, value);
                    if (true)
                    {
                        Assignment<Tval> result = Backtrack(csp, assignment);
                        if (result != null)
                            return result;
                    }
                }
                assignment.RemoveAssignment(variable);
                inference.RestoreOldDomains();
                Console.WriteLine("removed: " + value);
            }

            return null;
        }
    }
}
