using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class MinimumRemainingValues : VariableSelectionStrategy
    {
        public override Variable selectUnassignedVariable(ConstrainSatisfactionProblem csp, Assignment assignment)
        {
            HashSet<Variable> unassigned_variables = new HashSet<Variable>(csp.getVariables());
            unassigned_variables.ExceptWith(assignment.getAssignedVariables());

            Variable variable = unassigned_variables.First();
            foreach(Variable v in unassigned_variables)
            {
                variable = (variable.getDomain().size() < v.getDomain().size()) ? variable : v;
            }
            return variable;
        }
    }
}
