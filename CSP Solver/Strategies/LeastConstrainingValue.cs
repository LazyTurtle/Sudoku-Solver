using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class LeastConstrainingValue : DomainValueSelectionStragety
    {
        public override IEnumerable<object> getOrderedDomainValues(Variable variable, Assignment assignment, ConstraintSatisfactionProblem csp)
        {
            throw new NotImplementedException();
        }
    }
}
