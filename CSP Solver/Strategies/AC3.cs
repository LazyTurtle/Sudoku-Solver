using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.CSP_Solver.Constraints;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public class AC3<Tval> : InferenceStrategy<Tval>
    {
        public override InferenceResults<Tval> Infer(ConstraintSatisfactionProblem<Tval> csp, Variable<Tval> variable, Tval value)
        {
            Queue<Tuple<Variable<Tval>, Variable<Tval>>> queueOfArcs = new Queue<Tuple<Variable<Tval>, Variable<Tval>>>(csp.GetArcsOf(variable));
            return ReduceDomains(csp, queueOfArcs);
        }

        public override InferenceResults<Tval> Infer(ConstraintSatisfactionProblem<Tval> csp)
        {
            Queue<Tuple<Variable<Tval>, Variable<Tval>>> queueOfArcs = new Queue<Tuple<Variable<Tval>, Variable<Tval>>>(csp.GetArcs());
            return ReduceDomains(csp, queueOfArcs);
        }

        private InferenceResults<Tval> ReduceDomains(ConstraintSatisfactionProblem<Tval> csp, Queue<Tuple<Variable<Tval>, Variable<Tval>>> queueOfArcs)
        {
            InferenceResults<Tval> results = new InferenceResults<Tval>();
            while (queueOfArcs.Count > 0)
            {
                Tuple<Variable<Tval>, Variable<Tval>> arc = queueOfArcs.Dequeue();

                Variable<Tval> X = arc.Item1, Y = arc.Item2;
                if (Revise(csp, X, Y, results))
                {
                    if (X.GetDomain().Size() == 0)
                    {
                        results.InconsistencyFound();
                        return results;
                    }
                    foreach (Variable<Tval> neighbour in csp.GetNeighboursOf(X))
                    {
                        queueOfArcs.Enqueue(new Tuple<Variable<Tval>, Variable<Tval>>(neighbour, X));
                    }
                }
            }

            return results;
        }

        private bool Revise(ConstraintSatisfactionProblem<Tval> csp, Variable<Tval> variableX, Variable<Tval> variableY, InferenceResults<Tval> results)
        {
            bool revised = false;
            Assignment<Tval> assignment = new Assignment<Tval>();
            Domain<Tval> oldDomain = new Domain<Tval>(variableX.GetDomain());
            List<Tval> domain_values = new List<Tval>(oldDomain.GetValues());
            foreach (Tval valueX in domain_values)
            {
                assignment.Assign(variableX, valueX);
                bool satisfiable = false;

                foreach (Tval valueY in variableY.GetDomain().GetValues())
                {
                    assignment.Assign(variableY, valueY);
                    if (assignment.IsConsistent(csp.GetConstraints()))
                    {
                        satisfiable = true;
                        break;
                    }
                }
                if (!satisfiable)
                {
                    variableX.GetDomain().RemoveValue(valueX);
                    revised = true;
                }
            }
            if (revised)
                results.StoreDomainForVariable(variableX, oldDomain);

            return revised;
        }
    }
}