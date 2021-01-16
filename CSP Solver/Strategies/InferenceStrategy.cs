using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public abstract class InferenceStrategy<Tval>
    {
        abstract public Task<InferenceResults<Tval>> Infer(ConstraintSatisfactionProblem<Tval> csp, bool stopAtInconsistency = true);
        abstract public Task<InferenceResults<Tval>> Infer(ConstraintSatisfactionProblem<Tval> csp, Variable<Tval> variable, Tval value, InferenceResults<Tval> inference = null, bool stopAtInconsistency = true);
    }
}
