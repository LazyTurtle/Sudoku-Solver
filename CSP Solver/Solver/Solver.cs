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
    public class AssignmentCheckEventArgs<Tval> : EventArgs
    {
        public bool IsConsistent { set; get; }
        public IEnumerable<Tval> InconsistentVariables { set; get; }
    }
    public abstract class Solver<Tval>
    {
        public delegate void SolutionSearchCompleateEventHandler(object source, SolutionSearchCompleateEventArgs<Tval> eventArgs);
        public delegate void AssignmentCheckEventHandler(object source, AssignmentCheckEventArgs<Tval> eventArgs);

        public event SolutionSearchCompleateEventHandler SolutionSearchCompleate;
        public event AssignmentCheckEventHandler AssignmentCheckCompleate;
        public abstract Assignment<Tval> Solve(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> initialAssignment = null);
        public abstract void CheckAssignment(ConstraintSatisfactionProblem<Tval> csp, Assignment<Tval> initial_assignment);

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

        protected virtual void OnAssignmentCheckComplete(bool isConsistent, IEnumerable<Tval> inconsistentVariables = null)
        {
            AssignmentCheckEventArgs<Tval> args = new AssignmentCheckEventArgs<Tval>();
            args.IsConsistent = isConsistent;
            args.InconsistentVariables = inconsistentVariables;
            AssignmentCheckCompleate(this, args);
        }

        public virtual void ClearEvents()
        {
            SolutionSearchCompleate = null;
            AssignmentCheckCompleate = null;
        }

    }
}
