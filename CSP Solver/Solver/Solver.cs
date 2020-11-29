using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Solver
{
    public abstract class Solver
    {
        public abstract Assignment solve(ConstrainSatisfactionProblem csp, Assignment initial_assignment = null);
    }
}
