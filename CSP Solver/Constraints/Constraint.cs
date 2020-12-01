using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public abstract class Constraint<Tval>
    {
        protected ImmutableArray<Variable<Tval>> scope;

        public abstract bool IsSatisfied(Assignment<Tval> assignment);
        public abstract bool IsViolated(Assignment<Tval> assignment);
        public virtual Variable<Tval> ElementAt(int index)
        {
            return scope.ElementAt(index);
        }
    }
}
