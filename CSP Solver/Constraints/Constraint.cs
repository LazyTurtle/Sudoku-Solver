using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public abstract class Constraint
    {
        protected IReadOnlyCollection<Variable> scope;

        public abstract bool isSatisfied(Assignment assignment);
        public abstract bool IsViolated(Assignment assignment);
        public abstract Variable ElementAt(int index);
    }
}
