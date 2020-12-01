using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public abstract class InferenceStrategy<Tval>
    {
        abstract public InferenceResults<Tval> Infer(ConstraintSatisfactionProblem<Tval> csp, Variable<Tval> variable, Tval value);
        abstract public InferenceResults<Tval> Infer(ConstraintSatisfactionProblem<Tval> csp);
    }
}
