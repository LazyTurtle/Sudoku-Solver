using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver.CSP_Solver
{
    public class Domain
    {
        private HashSet<object> values;
        
        public Domain(IEnumerable<object> values = null)
        {
            if (values == null) throw new ArgumentNullException("values");
            this.values = new HashSet<object>(values);
        }


        public void update(Domain domain)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            values = new HashSet<object>(domain.getValues());
        }

        public void addValue(object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            values.Add(value);
        }

        public void removeValue(object value)
        {
            if (value == null) throw new ArgumentNullException("value");
            values.Remove(value);
        }

        public IEnumerable<object> getValues()
        {
            return values;
        }

        public int size()
        {
            return values.Count;
        }
    }

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
