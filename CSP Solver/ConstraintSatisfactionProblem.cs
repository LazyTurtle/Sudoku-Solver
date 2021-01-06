using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.CSP_Solver.Constraints;

namespace SudokuSolver.CSP_Solver
{
    public class ConstraintSatisfactionProblem<Tval>
    {
        private ImmutableArray<Variable<Tval>> Variables;
        private ImmutableArray<Constraint<Tval>> Constraints;
        private ImmutableDictionary<Variable<Tval>, List<Variable<Tval>>> NeighboursTable;
        private List<Tuple<Variable<Tval>, Variable<Tval>>> arcs;
        private ImmutableDictionary<Variable<Tval>,List<Tuple<Variable<Tval>,Variable<Tval>>>> VariableArcsTable;

        public ConstraintSatisfactionProblem() { }
        public ConstraintSatisfactionProblem(IEnumerable<Variable<Tval>> variables, IEnumerable<Constraint<Tval>> constraints = null)
        {
            if (variables == null) throw new ArgumentNullException("variables");
            Variables = ImmutableArray.Create(variables.ToArray());
            Console.WriteLine("Variables: "+Variables.Length);
            Constraints = (constraints != null) ? ImmutableArray.Create(constraints.ToArray()) : ImmutableArray.Create<Constraint<Tval>>();
            Console.WriteLine("Constraints: " + Constraints.Length);
            NeighboursTable = CreateNeighboursArcs(Constraints).ToImmutableDictionary();
            Console.WriteLine("NeighboursTable: " + NeighboursTable.Count());
            arcs = CreateListOfArcs();
            Console.WriteLine("Arcs: " + arcs.Count());
            VariableArcsTable = CreateVariableArcsTable(NeighboursTable).ToImmutableDictionary();
        }

        private Dictionary<Variable<Tval>, List<Tuple<Variable<Tval>, Variable<Tval>>>> CreateVariableArcsTable(ImmutableDictionary<Variable<Tval>, List<Variable<Tval>>> neighboursTable)
        {
            Dictionary<Variable<Tval>, List<Tuple<Variable<Tval>, Variable<Tval>>>> table = new Dictionary<Variable<Tval>, List<Tuple<Variable<Tval>, Variable<Tval>>>>(Variables.Length);
            foreach (Variable<Tval> variable in Variables)
            {
                if(neighboursTable.TryGetValue(variable, out var neighbours))
                {
                    List<Tuple<Variable<Tval>, Variable<Tval>>> list = new List<Tuple<Variable<Tval>, Variable<Tval>>>();
                    foreach (var neighbour in neighbours)
                    {
                        list.Add(new Tuple<Variable<Tval>, Variable<Tval>>(neighbour, variable));
                    }
                    table.Add(variable, list);
                }
            }

            return table;
        }

        // This is only used for binary constraints
        private Dictionary<Variable<Tval>, List<Variable<Tval>>> CreateNeighboursArcs(ImmutableArray<Constraint<Tval>> constraints)
        {
            Dictionary<Variable<Tval>, List<Variable<Tval>>> neighbours = new Dictionary<Variable<Tval>, List<Variable<Tval>>>(Variables.Length);

            foreach (Constraint<Tval> constraint in constraints)
            {
                if(constraint is BinaryConstraint<Tval>)
                {
                    if (neighbours.TryGetValue(constraint.ElementAt(0), out List<Variable<Tval>> neighbourList))
                    {
                        neighbourList.Add(constraint.ElementAt(1));
                    }
                    else
                    {
                        neighbours.Add(constraint.ElementAt(0), new List<Variable<Tval>>() { constraint.ElementAt(1) });
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

        public List<Variable<Tval>> GetNeighboursOf(Variable<Tval> variable)
        {
            List<Variable<Tval>> list;
            NeighboursTable.TryGetValue(variable, out list);
            return list;
        }

        public List<Tuple<Variable<Tval>, Variable<Tval>>> GetArcs()
        {
            return arcs;
        }
        public IEnumerable<Tuple<Variable<Tval>, Variable<Tval>>> GetArcsTowards(Variable<Tval> variable)
        {
            if(VariableArcsTable.TryGetValue(variable, out var list))
            {
                return list;
            }
            return null;
        }
    }
}
