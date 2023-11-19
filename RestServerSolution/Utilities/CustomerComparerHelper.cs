using System;
using System.Text;
using RestServerSolution.Models;

namespace RestServerSolution.Utilities
{
    public class CustomerComparerHelper : IComparer<Customer>
    {
        public int Compare(Customer? c1, Customer? c2)
        {
            int lastNameComparison = string.Compare(c1?.LastName, c2?.LastName, StringComparison.OrdinalIgnoreCase);

            if (lastNameComparison != 0)
            {
                return lastNameComparison;
            }

            return string.Compare(c1?.FirstName, c2?.FirstName, StringComparison.OrdinalIgnoreCase);
        }
    }
}

