using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public class Assignment
    {
        private Dictionary<Variable, object> assignments;

        public Assignment(Dictionary<Variable,object> initial_assignments = null)
        {
            assignments = initial_assignments ?? new Dictionary<Variable, object>();
        }
        
        public bool IsConsistent(IEnumerable<Constraint> constraints)
        {
            bool consistent = true;
            if (constraints.Any(c => c.IsViolated(this))) consistent = false;
            return consistent;
        }

        public IEnumerable<Variable> getAssignedVariables()
        {
            return assignments.Keys;
        }

        public bool haveBeenAssigned(Variable variable)
        {
            return assignments.ContainsKey(variable);
        }

        public bool isComplete(IEnumerable<Variable>variables)
        {
            return assignments.Count == variables.Count();
        }

        public void assign(Variable variable, object value)
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

        public void removeAssignment(Variable variable)
        {
            assignments.Remove(variable);
        }

        public object valueOf(Variable variable)
        {
            object value = null;
            assignments.TryGetValue(variable,out value);
            return value;
        }

        public override string ToString()
        {
            return "count: "+assignments.Count()+" and: "+ assignments.ToString();
        }
    }

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
            return (constraints.Any(c => c.IsViolated(this))) ? false : true;
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
    }
}
