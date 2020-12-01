using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.CSP_Solver.Constraints;

namespace SudokuSolver.CSP_Solver
{
    public class ConstraintSatisfactionProblem
    {
        private HashSet<Variable> variables;
        private HashSet<Constraint> constraints;

        public ConstraintSatisfactionProblem(IEnumerable<Variable> variables, IEnumerable<Constraint> constraints)
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

    public class ConstraintSatisfactionProblem<Tval>
    {
        private ImmutableArray<Variable<Tval>> Variables;
        private ImmutableArray<Constraint<Tval>> Constraints;
        private ImmutableDictionary<Variable<Tval>, List<Variable<Tval>>> NeighboursTable;
        private List<Tuple<Variable<Tval>, Variable<Tval>>> arcs;

        public ConstraintSatisfactionProblem(IEnumerable<Variable<Tval>> variables, IEnumerable<Constraint<Tval>> constraints = null)
        {
            if (variables == null) throw new ArgumentNullException("variables");
            Variables = ImmutableArray.Create(variables.ToArray());
            Console.WriteLine("Variabili: "+Variables.Length);
            Constraints = (constraints != null) ? ImmutableArray.Create(constraints.ToArray()) : ImmutableArray.Create<Constraint<Tval>>();
            Console.WriteLine("Constraints: " + Constraints.Length);
            NeighboursTable = CreateNeighboursArcs(Constraints).ToImmutableDictionary();
            Console.WriteLine("NeighboursTable: " + NeighboursTable.Count());
            arcs = CreateListOfArcs();
            Console.WriteLine("Arcs: " + arcs.Count());
        }

        // This is only used for binary constraints
        private Dictionary<Variable<Tval>, List<Variable<Tval>>> CreateNeighboursArcs(ImmutableArray<Constraint<Tval>> constraints)
        {
            Dictionary<Variable<Tval>, List<Variable<Tval>>> neighbours = new Dictionary<Variable<Tval>, List<Variable<Tval>>>(Variables.Length);

            List<Variable<Tval>> neighbourList;

            foreach (Constraint<Tval> constraint in constraints)
            {
                if(constraint is BinaryConstraint<Tval>)
                {
                    if (neighbours.TryGetValue(constraint.ElementAt(0), out neighbourList))
                    {
                        neighbourList.Add(constraint.ElementAt(1));
                    }
                    else
                    {
                        neighbours.Add(constraint.ElementAt(0), new List<Variable<Tval>>() { constraint.ElementAt(1) });
                    }

                    if (neighbours.TryGetValue(constraint.ElementAt(1), out neighbourList))
                    {
                        neighbourList.Add(constraint.ElementAt(0));
                    }
                    else
                    {
                        neighbours.Add(constraint.ElementAt(1), new List<Variable<Tval>>() { constraint.ElementAt(0) });
                    }
                }
            }

            return neighbours;
        }

        private List<Tuple<Variable<Tval>, Variable<Tval>>> CreateListOfArcs()
        {
            List<Tuple<Variable<Tval>, Variable<Tval>>> list = new List<Tuple<Variable<Tval>, Variable<Tval>>>(Constraints.Length);
            foreach (Constraint<Tval> constraint in Constraints)
            {
                list.Add(new Tuple<Variable<Tval>, Variable<Tval>>(constraint.ElementAt(0), constraint.ElementAt(1)));
            }
            return list;
        }

        public IEnumerable<Variable<Tval>> GetVariables()
        {
            return Variables;
        }

        public IEnumerable<Constraint<Tval>> GetConstraints()
        {
            return Constraints;
        }

        public List<Variable<Tval>> GetArcsOf(Variable<Tval> variable)
        {
            List<Variable<Tval>> list;
            NeighboursTable.TryGetValue(variable, out list);
            return list;
        }

        public List<Tuple<Variable<Tval>, Variable<Tval>>> GetArcs()
        {
            return arcs;
        }
    }
}
