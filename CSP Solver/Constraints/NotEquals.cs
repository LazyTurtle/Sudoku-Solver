using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Constraints
{
    class NotEquals<Tval> : BinaryConstraint<Tval>
    {

        public NotEquals(Variable<Tval> variableX, Variable<Tval> variableY)
        {
            if (variableX == null) throw new ArgumentNullException("variableX");
            if (variableY == null) throw new ArgumentNullException("variableY");
            scope = ImmutableArray.Create<Variable<Tval>>(variableX, variableY);
        }
        public override bool IsSatisfied(Assignment<Tval> assignment)
        {
            return scope.ElementAt(0) != scope.ElementAt(1);
        }

        public override bool IsViolated(Assignment<Tval> assignment)
        {
            return !IsSatisfied(assignment);
        }
    }
}
