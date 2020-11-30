using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class UnorderedDomainValues : DomainValueSelectionStragety
    {
        public override IEnumerable<object> getOrderedDomainValues(Variable variable, Assignment assignment, ConstraintSatisfactionProblem csp)
        {
            return  new List<object>(variable.getDomain().getValues());
        }
    }

    class UnorderedDomainValues<Tval> : DomainValueSelectionStragety<Tval>
    {
        public override IEnumerable<Tval> getOrderedDomainValues(Variable<Tval> variable, Assignment<Tval> assignment, ConstraintSatisfactionProblem<Tval> csp)
        {
            return variable.GetDomain().GetValues().ToList();
        }
    }
}
