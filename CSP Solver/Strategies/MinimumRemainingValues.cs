using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class MinimumRemainingValues : VariableSelectionStrategy
    {
        public override Variable selectUnassignedVariable(ConstraintSatisfactionProblem csp, Assignment assignment)
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

    class MinimumRemainingValues<Tval> : VariableSelectionStrategy<Tval>
    {
        public override Variable<Tval> SelectUnassignedVariable(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> assignment)
        {
            HashSet<Variable<Tval>> unassigned_variables = new HashSet<Variable<Tval>>();
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
