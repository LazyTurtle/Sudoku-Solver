using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public class Assignment<Tval>
    {
        private Dictionary<Variable<Tval>, Tval> assignments;

        public Assignment(Dictionary<Variable<Tval>, Tval> initialAssignment = null)
        {
            assignments = initialAssignment ?? new Dictionary<Variable<Tval>, Tval>();
        }

        public Assignment(int numberOfVariables)
        {
            assignments = new Dictionary<Variable<Tval>, Tval>(numberOfVariables);
        }

        public bool IsComplete(IEnumerable<Variable<Tval>> variables)
        {
            return variables.Count() == assignments.Keys.Count;
        }

        public bool IsConsistent(IEnumerable<Constraint<Tval>> constraints)
        {
            int constraintsPerSlot = constraints.Count() / 4;
            List<Constraint<Tval>> list = constraints.ToList();

            return (constraints.Any(c => c.IsViolated(this))) ? false : true;
        }

        private bool ConsistencyInRange(List<Constraint<Tval>> constraints, int min, int max)
        {
            bool consistent = true;
            for (int i = min; i < max; i++)
            {
                consistent = !constraints[i].IsViolated(this);
                if (!consistent)
                    break;
            }
            return consistent;
        }

        public void Assign(Variable<Tval> variable, Tval value)
        {
            if (assignments.ContainsKey(variable))
            {
                assignments[variable] = value;
            }
            else
            {
                assignments.Add(variable, value);
            }
        }

        public HashSet<Variable<Tval>> GetAssignedVariables()
        {
            return assignments.Keys.ToHashSet();
        }

        public bool RemoveAssignment(Variable<Tval> variable)
        {
            return assignments.Remove(variable);
        }

        public Tval ValueOf(Variable<Tval> variable)
        {
            assignments.TryGetValue(variable, out Tval value);
            return value;
        }

        public bool HasBeenAssigned(Variable<Tval> variable)
        {
            return assignments.ContainsKey(variable);
        }
    }
}
