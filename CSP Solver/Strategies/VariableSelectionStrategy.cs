using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public abstract class VariableSelectionStrategy<Tval>
    {
        public abstract Variable<Tval> SelectUnassignedVariable(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> assignment);
    }
}
