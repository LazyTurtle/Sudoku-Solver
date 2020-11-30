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
}
