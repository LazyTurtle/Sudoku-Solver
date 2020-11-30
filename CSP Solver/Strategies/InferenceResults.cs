using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver.Strategies
{
    public class InferenceResults
    {
        private bool isConsistent = true;
        private Dictionary<Variable, Domain> storedDomains;

        public InferenceResults()
        {
            storedDomains = new Dictionary<Variable, Domain>();
        }

        public void storeDomainForVariable(Variable variable, Domain domain)
        {
            if (variable == null) throw new ArgumentNullException("variable");
            if (domain == null) throw new ArgumentNullException("domain");
            if (storedDomains.ContainsKey(variable))
            {
                storedDomains[variable] = domain;
            }
            else
            {
                storedDomains.Add(variable, domain);
            }
        }

        public void restoreOldDomains()
        { 
            foreach (var pair in storedDomains)
            {
                pair.Key.setDomain(pair.Value);
            }
        }

        public void inconsistencyFound()
        {
            isConsistent = false;
        }

        public bool isAssignmentConsistent()
        {
            return isConsistent;
        }
    }
    public class InferenceResults<Tval>
    {
        private bool isConsistent = true;
        private Dictionary<Variable<Tval>, Domain<Tval>> storedDomains;

        public InferenceResults()
        {
            storedDomains = new Dictionary<Variable<Tval>, Domain<Tval>>();
        }

        public void StoreDomainForVariable(Variable<Tval> variable, Domain<Tval> domain)
        {
            if (variable == null) throw new ArgumentNullException("variable");
            if (domain == null) throw new ArgumentNullException("domain");
            if (storedDomains.ContainsKey(variable))
            {
                storedDomains[variable] = domain;
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

        public bool IsAssignmentConsistent()
        {
            return isConsistent;
        }
    }
}
