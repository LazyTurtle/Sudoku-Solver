using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuSolver.CSP_Solver.Constraints;

namespace SudokuSolver.CSP_Solver.Strategies
{
    class AC3 : InferenceStrategy
    {
        public override InferenceResults infer(ConstrainSatisfactionProblem csp, Variable variable, object value)
        {
            InferenceResults results = new InferenceResults();
            /*results.storeDomainForVariable(variable, variable.getDomain());
            variable.setDomain(new Domain(new object[] { value }));*/

            
            Queue<Constraint> constraints_queue = new Queue<Constraint>(csp.getConstraints());
            while (constraints_queue.Count > 0)
            {
                Constraint constraint = constraints_queue.Dequeue();
                // TODO: AC3 only uses binary constraints, not only binarynotequals
                if (constraint is BinaryNotEquals)
                {
                    Variable X = constraint.ElementAt(0), Y = constraint.ElementAt(1);
                    if (revise(csp, X, Y, results))
                    {
                        if (X.getDomain().getValues().Count() == 0)
                        {
                            results.inconsistencyFound();
                            return results;
                        }
                        foreach(Variable neighbour in csp.neighboursOf(X))
                        {
                            if(neighbour != Y)
                            constraints_queue.Enqueue(new BinaryNotEquals(neighbour, X));
                        }
                    }
                }
            }

            return results;
        }

        private bool revise(ConstrainSatisfactionProblem csp, Variable variableX, Variable variableY, InferenceResults results)
        {
            bool revised = false;
            Assignment assignment = new Assignment();
            Domain oldDomain = new Domain(variableX.getDomain().getValues());
            List<object> domain_values = new List<object>(oldDomain.getValues());
            foreach(object valueX in domain_values)
            {
                assignment.assign(variableX, valueX);
                bool satisfiable = false;
                // TODO: optimize, could exit the cycle before checking them all
                foreach (object valueY in variableY.getDomain().getValues())
                {
                    assignment.assign(variableY, valueY);
                    if (assignment.IsConsistent(csp.getConstraints()))
                        satisfiable = true;
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
