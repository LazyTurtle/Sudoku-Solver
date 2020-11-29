using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public class ConstrainSatisfactionProblem
    {
        private HashSet<Variable> variables;
        private HashSet<Constraint> constraints;

        public ConstrainSatisfactionProblem(IEnumerable<Variable> variables, IEnumerable<Constraint> constraints)
        {
            if (variables == null) throw new ArgumentNullException("variables");
            if (constraints == null) throw new ArgumentNullException("constraints");

            this.variables = new HashSet<Variable>(variables);
            this.constraints = new HashSet<Constraint>(constraints);
        }

        internal IEnumerable<Constraint> getConstraints()
        {
            return constraints;
        }

        // TODO: the constraints do not change in this problem, I should create a structure to reduce this to O(1)
        public IEnumerable<Variable> neighboursOf(Variable variable)
        {
            List<Variable> neighbours = new List<Variable>();

            foreach(Constraint constraint in constraints)
            {
                if (constraint.ElementAt(0) == variable)
                    neighbours.Add(constraint.ElementAt(1));
            }

            return neighbours;
        }

        internal IEnumerable<Variable> getVariables()
        {
            return variables;
        }
    }
}
