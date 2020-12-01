using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace SudokuSolver.CSP_Solver
{
    public class Variable
    {
        private Domain domain;

        public Variable(Domain domain)
        {
            if (domain==null) throw new ArgumentNullException("domain");
            this.domain = domain;
        }

        public void setDomain(Domain domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");

            domain.update(domain);
        }

        public Domain getDomain()
        {
            return domain;
        }
    }

    public class Variable<Tval>
    {
        private string Name;
        private Domain<Tval> Domain;
        public Variable(Domain<Tval> domain = null, string name = null)
        {
            Domain = domain ?? new Domain<Tval>();
            Name = name;
        }

        public void UpdateDomain(Domain<Tval> newDomain)
        {
            if (newDomain == null) throw new ArgumentNullException("newDomain");
            Domain = newDomain;
        }

        public Domain<Tval> GetDomain()
        {
            return Domain;
        }

        public override string ToString()
        {
            return Name + " " + Domain.ToString();
        }
    }
}
