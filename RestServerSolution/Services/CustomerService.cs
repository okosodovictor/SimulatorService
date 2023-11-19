using System;
using RestServerSolution.Models;
using RestServerSolution.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace RestServerSolution.Services
{
	public static class CustomerService
	{
        private static List<Customer> customers = new List<Customer>();
        private static Dictionary<int, Customer> customerMap = new Dictionary<int, Customer>();

        public static bool HasCustomer(int id)
        {
            if (customerMap.ContainsKey(id))
            {
                return true;
            }

            return false;
        }

        public static void Initialize()
        {
            var customers = Helper.LoadCustomers();
            if (customers.Any())
            {
                foreach (var oldCustomer in customers)
                {
                    customerMap.Add(oldCustomer.Id, oldCustomer);
                }

                AddCustomers(customers);
            }
        }

        public static void AddCustomers(List<Customer> newCustomers)
		{
            foreach (var newCustomer in newCustomers)
            {
                int index = 0;
                while (index < customers.Count && new CustomerComparerHelper().Compare(customers[index], newCustomer) < 0)
                {
                    index++;
                }

                customers.Insert(index, newCustomer);
                if (!HasCustomer(newCustomer.Id))
                {
                    customerMap.Add(newCustomer.Id, newCustomer);
                    Helper.PersistCustomer(newCustomer);
                }
            }
        }

        public static List<Customer> GetCustomers()
        {
            return customers;
        }
    }
}

