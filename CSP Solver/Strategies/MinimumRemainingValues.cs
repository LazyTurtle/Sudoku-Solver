using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class MinimumRemainingValues<Tval> : VariableSelectionStrategy<Tval>
    {
        public override Variable<Tval> SelectUnassignedVariable(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> assignment)
        {
            HashSet<Variable<Tval>> unassigned_variables = new HashSet<Variable<Tval>>(csp.GetVariables());
            unassigned_variables.ExceptWith(assignment.GetAssignedVariables());

            Variable<Tval> variable = unassigned_variables.First();
            foreach (Variable<Tval> v in unassigned_variables)
            {
                variable = (variable.GetDomain().Size() < v.GetDomain().Size()) ? variable : v;
            }
            return variable;
        }
    }
}
