using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Solver
{
    public class SolutionSearchCompleateEventArgs<Tval> : EventArgs
    {
        public bool SolutionFound { set; get; }
        public Assignment<Tval> Solution { set; get; }
    }
    public abstract class Solver<Tval>
    {
        public delegate void SolutionSearchCompleateEventHandler(object source, SolutionSearchCompleateEventArgs<Tval> eventArgs);

        public event SolutionSearchCompleateEventHandler SolutionSearchCompleate;
        public abstract Assignment<Tval> Solve(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> initialAssignment = null);

        protected virtual void OnSolutionFound(Assignment<Tval> solution)
        {
            SolutionSearchCompleateEventArgs<Tval> args = new SolutionSearchCompleateEventArgs<Tval>();
            args.Solution = solution;
            args.SolutionFound = true;
            SolutionSearchCompleate(this, args);
        }

        protected virtual void OnNoSolutionFound()
        {
            SolutionSearchCompleateEventArgs<Tval> args = new SolutionSearchCompleateEventArgs<Tval>();
            args.SolutionFound = false;
            SolutionSearchCompleate(this, args);
        }

        public virtual void ClearEvents()
        {
            SolutionSearchCompleate = null;
        }
    }
}
