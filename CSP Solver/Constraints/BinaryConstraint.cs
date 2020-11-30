using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Constraints
{
    public abstract class BinaryConstraint<Tval> : Constraint<Tval>
    {
        public virtual Tuple<Variable<Tval>,Variable<Tval>> GetVariables()
        {
            return new Tuple<Variable<Tval>, Variable<Tval>>(scope.ElementAt(0), scope.ElementAt(1));
        }
    }
}
