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

            if (domain == null)
                domain = new Domain();

            domain.update(domain);
        }

        public Domain getDomain()
        {
            return domain;
        }
    }
}
