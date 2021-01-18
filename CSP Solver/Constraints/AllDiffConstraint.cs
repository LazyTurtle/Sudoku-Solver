using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Constraints
{
    class AllDiffConstraint<Tval> : Constraint<Tval>
    {
        Dictionary<Tval, int> valueTable;

        AllDiffConstraint(IEnumerable<Variable<Tval>> variables)
        {
            if (variables == null)
                throw new ArgumentNullException("variables");

            scope = ImmutableArray.Create(variables.ToArray());
            valueTable = new Dictionary<Tval, int>(scope.Length);
        }

        public override bool IsSatisfied(Assignment<Tval> assignment)
        {
            return !IsViolated(assignment);
        }

        public override bool IsViolated(Assignment<Tval> assignment)
        {
            valueTable.Clear();

            foreach (var variable in scope)
            {
                if (assignment.HasBeenAssigned(variable))
                {
                    Tval Tvalue = assignment.ValueOf(variable);

                    if (valueTable.TryGetValue(Tvalue, out int count))
                    {
                        valueTable[Tvalue] = valueTable[Tvalue] + 1;
                    }
                    else
                    {
                        valueTable.Add(Tvalue, 1);
                    }
                    
                    
                }
            }

            return valueTable.Any(p => p.Value > 1);
        }
    }
}
