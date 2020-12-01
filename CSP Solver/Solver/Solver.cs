using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Solver
{
    public abstract class Solver
    {
        public abstract Assignment solve(ConstraintSatisfactionProblem csp, Assignment initial_assignment = null);
    }

    public abstract class Solver<Tval>
    {
        public delegate void SolutionFoundEventHandler(object source, EventArgs eventArgs);
        public delegate void NoSolutionFoundEventHandler(object source, EventArgs eventArgs);

        public event SolutionFoundEventHandler SolutionFound;
        public event NoSolutionFoundEventHandler NoSolutionFound;
        public abstract Assignment<Tval> Solve(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> initialAssignment = null);

        protected virtual void OnSolutionFound(Assignment<Tval> solution)
        {
            SolutionFound(this, EventArgs.Empty);
        }

        protected virtual void OnNoSolutionFound()
        {
            NoSolutionFound(this, EventArgs.Empty);
        }

        internal void ClearEvents()
        {
            SolutionFound = null;
            NoSolutionFound = null;
        }
    }
}
