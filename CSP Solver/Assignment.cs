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
        private int numberOfTasks = 5;

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
            return !constraints.Any(c => c.IsViolated(this));
        }

        // there is no more need for this parallelized consistency check
        public async Task<bool> IsConsistentParallelAsync(IEnumerable<Constraint<Tval>> constraints)
        {
            int constraintsPerTask = (int)Math.Ceiling((constraints.Count() / (float)numberOfTasks));
            List<Task<bool>> tasks = new List<Task<bool>>(numberOfTasks);
            List<Constraint<Tval>> constraintList = constraints.ToList();
            for (int i = 0; i < numberOfTasks; ++i)
            {
                // this local variable is necessary since the lambda expression
                // is not syncronized with the for outside its scope
                // without this the i inside the lambda body is a random value.
                // there should be a different way to give lambda expression
                // arguments but I only found funcs and actions
                // which are unnecessarily convoluted and a pain to implement
                int local = i;
                tasks.Add(Task.Run(() =>
                {
                    return ConsistencyInRange(constraintList, local * constraintsPerTask, (local + 1) * constraintsPerTask);
                }));
            }
            bool[] tasksResult = await Task.WhenAll(tasks);
            return !tasksResult.Any(r => r == false);
        }

        private bool ConsistencyInRange(List<Constraint<Tval>> constraints, int min, int max)
        {
            bool consistent = true;
            min = (min >= 0) ? min : 0;
            max = (max < constraints.Count) ? max : constraints.Count;
            for (int i = min; i < max; i++)
            {
                consistent = constraints[i].IsSatisfied(this);
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
