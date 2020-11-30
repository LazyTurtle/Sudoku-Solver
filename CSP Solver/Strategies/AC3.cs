using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.CSP_Solver.Constraints;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public class AC3 : InferenceStrategy
    {
        public override InferenceResults infer(ConstraintSatisfactionProblem csp, Variable variable, object value)
        {
            InferenceResults results = new InferenceResults();

            Queue<Tuple<Variable, Variable>> arcs = CreateListOfArcs<BinaryNotEquals>(csp);
            while (arcs.Count > 0)
            {
                Tuple<Variable, Variable> arc = arcs.Dequeue();
                
                Variable X = arc.Item1, Y = arc.Item2;
                if (revise(csp, X, Y, results))
                {
                    if (X.getDomain().getValues().Count() == 0)
                    {
                        results.inconsistencyFound();
                        return results;
                    }
                    foreach (Variable neighbour in csp.neighboursOf(X))
                    {
                        if (neighbour != Y)
                            arcs.Enqueue(new Tuple<Variable, Variable>(neighbour,X));
                    }
                }
            }

            return results;
        }

        private Queue<Tuple<Variable, Variable>> CreateListOfArcs<Tconstraint>(ConstraintSatisfactionProblem csp)
        {
            Queue<Tuple<Variable, Variable>> arcs = new Queue<Tuple<Variable, Variable>>();
            foreach (Constraint c in csp.getConstraints())
            {
                // TODO: AC3 only uses binary constraints, not only binarynotequals
                if (c is Tconstraint)
                    arcs.Enqueue(new Tuple<Variable, Variable>(c.ElementAt(0), c.ElementAt(1)));
            }
            return arcs;
        }

        private bool revise(ConstraintSatisfactionProblem csp, Variable variableX, Variable variableY, InferenceResults results)
        {
            bool revised = false;
            Assignment assignment = new Assignment();
            Domain oldDomain = new Domain(variableX.getDomain().getValues());
            List<object> domain_values = new List<object>(oldDomain.getValues());
            foreach(object valueX in domain_values)
            {
                assignment.assign(variableX, valueX);
                bool satisfiable = false;
                
                foreach(object valueY in variableY.getDomain().getValues())
                {
                    assignment.assign(variableY, valueY);
                    if (assignment.IsConsistent(csp.getConstraints()))
                    {
                        satisfiable = true;
                        break;
                    }
                }
                if (!satisfiable)
                {
                    variableX.getDomain().removeValue(valueX);
                    revised = true;
                }
            }
            if (revised)
                results.storeDomainForVariable(variableX, oldDomain);

            return revised;
        }
    }

    public class AC3<Tval> : InferenceStrategy<Tval>
    {
        public override InferenceResults<Tval> Infer(ConstraintSatisfactionProblem<Tval> csp, Variable<Tval> variable, Tval value)
        {
            /*
             * InferenceResults results = new InferenceResults();

            Queue<Tuple<Variable, Variable>> arcs = CreateListOfArcs<BinaryNotEquals>(csp);
            while (arcs.Count > 0)
            {
                Tuple<Variable, Variable> arc = arcs.Dequeue();

                Variable X = arc.Item1, Y = arc.Item2;
                if (revise(csp, X, Y, results))
                {
                    if (X.getDomain().getValues().Count() == 0)
                    {
                        results.inconsistencyFound();
                        return results;
                    }
                    foreach (Variable neighbour in csp.neighboursOf(X))
                    {
                        if (neighbour != Y)
                            arcs.Enqueue(new Tuple<Variable, Variable>(neighbour, X));
                    }
                }
            }

            return results;*/
            throw new NotImplementedException();
        }

        private Queue<Tuple<Variable, Variable>> CreateListOfArcs<Tconstraint>(ConstraintSatisfactionProblem csp)
        {
            Queue<Tuple<Variable, Variable>> arcs = new Queue<Tuple<Variable, Variable>>();
            foreach (Constraint c in csp.getConstraints())
            {
                // TODO: AC3 only uses binary constraints, not only binarynotequals
                if (c is Tconstraint)
                    arcs.Enqueue(new Tuple<Variable, Variable>(c.ElementAt(0), c.ElementAt(1)));
            }
            return arcs;
        }

        private bool revise(ConstraintSatisfactionProblem csp, Variable variableX, Variable variableY, InferenceResults results)
        {
            bool revised = false;
            Assignment assignment = new Assignment();
            Domain oldDomain = new Domain(variableX.getDomain().getValues());
            List<object> domain_values = new List<object>(oldDomain.getValues());
            foreach (object valueX in domain_values)
            {
                assignment.assign(variableX, valueX);
                bool satisfiable = false;

                foreach (object valueY in variableY.getDomain().getValues())
                {
                    assignment.assign(variableY, valueY);
                    if (assignment.IsConsistent(csp.getConstraints()))
                    {
                        satisfiable = true;
                        break;
                    }
                }
                if (!satisfiable)
                {
                    variableX.getDomain().removeValue(valueX);
                    revised = true;
                }
            }
            if (revised)
                results.storeDomainForVariable(variableX, oldDomain);

            return revised;
        }
    }
}
