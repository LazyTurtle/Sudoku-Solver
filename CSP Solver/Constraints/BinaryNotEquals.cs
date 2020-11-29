using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SudokuSolver.CSP_Solver;

namespace SudokuSolver.CSP_Solver.Constraints
{
    class BinaryNotEquals : Constraint
    {

        public BinaryNotEquals(Variable first, Variable second)
        {
            scope = new Variable[2] { first, second };
        }

        public override bool isSatisfied(Assignment assignment)
        {
            if (!assignment.haveBeenAssigned(scope.ElementAt(0)))
                return true;
            if (!assignment.haveBeenAssigned(scope.ElementAt(1)))
                return true;

            return !(assignment.valueOf(scope.ElementAt(0)).Equals(assignment.valueOf(scope.ElementAt(1))));
        }

        public override bool IsViolated(Assignment assignment)
        {
            return !isSatisfied(assignment);
        }

        public override Variable ElementAt(int index)
        {
            return scope.ElementAt(index);
        }
    }
}
