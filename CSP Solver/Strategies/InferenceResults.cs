using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public class InferenceResults<Tval>
    {
        private bool isConsistent = true;
        private Dictionary<Variable<Tval>, Domain<Tval>> storedDomains;
        private List<Variable<Tval>> variablesWithEmptyDomain;

        public InferenceResults()
        {
            storedDomains = new Dictionary<Variable<Tval>, Domain<Tval>>();
            variablesWithEmptyDomain = new List<Variable<Tval>>();
        }

        public void StoreDomainForVariable(Variable<Tval> variable, Domain<Tval> domain)
        {
            if (variable == null) throw new ArgumentNullException("variable");
            if (domain == null) throw new ArgumentNullException("domain");
            if (storedDomains.ContainsKey(variable))
            {
                storedDomains[variable].GetValues().UnionWith(domain.GetValues());
            }
            else
            {
                storedDomains.Add(variable, domain);
            }
        }

        public void RestoreOldDomains()
        { 
            foreach (var pair in storedDomains)
            {
                pair.Key.UpdateDomain(pair.Value);
            }
        }

        public void InconsistencyFound()
        {
            isConsistent = false;
        }

        public void InconsistencyFound(Variable<Tval> variable)
        {
            InconsistencyFound();
            AddInconsistentVariable(variable);
        }

        public bool IsAssignmentConsistent()
        {
            return isConsistent;
        }

        public void AddInconsistentVariable(Variable<Tval> variable)
        {
            variablesWithEmptyDomain.Add(variable);
        }

        public IEnumerable<Variable<Tval>> InconsistentVariables()
        {
            return variablesWithEmptyDomain;
        }
    }
}
