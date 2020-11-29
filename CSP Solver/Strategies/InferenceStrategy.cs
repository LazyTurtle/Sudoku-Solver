using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public abstract class InferenceStrategy
    {
        abstract public InferenceResults infer(ConstrainSatisfactionProblem csp, Variable variable, object value);
    }
}
