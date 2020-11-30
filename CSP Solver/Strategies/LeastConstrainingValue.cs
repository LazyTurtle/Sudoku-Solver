using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class LeastConstrainingValue<Tval> : DomainValueSelectionStragety<Tval>
    {
        public override IEnumerable<Tval> getOrderedDomainValues(Variable<Tval> variable, Assignment<Tval> assignment, ConstraintSatisfactionProblem<Tval> csp)
        {
            throw new NotImplementedException();
        }
    }
}
