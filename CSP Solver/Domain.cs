using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public class Domain<Tval>
    {
        private HashSet<Tval> Values;

        public Domain(IEnumerable<Tval> values = null)
        {
            Values = (values == null) ? new HashSet<Tval>() : new HashSet<Tval>(values);
        }

        public Domain(Domain<Tval> domain)
        {
            Values = new HashSet<Tval>(domain.Values);
        }

        public HashSet<Tval> GetValues()
        {
            return Values;
        }

        public int Size()
        {
            return Values.Count();
        }

        public void RemoveValue(Tval value)
        {
            Values.Remove(value);
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder("values: ");
            foreach (Tval v in Values)
                s.Append( " " + v.ToString());
            return s.ToString();
        }
    }
}
