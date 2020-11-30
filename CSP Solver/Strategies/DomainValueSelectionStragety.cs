using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public abstract class DomainValueSelectionStragety
    {
        public abstract IEnumerable<object> getOrderedDomainValues(Variable variable, Assignment assignment, ConstraintSatisfactionProblem csp);
    }
    public abstract class DomainValueSelectionStragety<Tval>
    {
        public abstract IEnumerable<Tval> getOrderedDomainValues(Variable<Tval> variable, Assignment<Tval> assignment, ConstraintSatisfactionProblem<Tval> csp);
    }
}
