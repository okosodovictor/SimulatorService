using System;
using RestServerSolution.Models;
using System.Text;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RestServerSolution.Utilities
{
	public static class Helper
	{
        private static string FullPath => Path.Combine(Directory.GetCurrentDirectory(), "customers.txt");

        public static void PersistCustomer(Customer customer)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FullPath, true, Encoding.UTF8))
                {
                    if (customer != null)
                    {
                        var result = customer.ToString();

                        writer.WriteLine(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving customer: {ex.Message}");
            }
        }

        public static List<Customer> LoadCustomers()
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                if (File.Exists(FullPath))
                {
                    string[] lines = File.ReadAllLines(FullPath);
                    foreach (var line in lines)
                    {
                        string[] items = line.Split(',');

                        if (items.Length == 4 && int.TryParse(items[0], out int id) && int.TryParse(items[3], out int age))
                        {
                            customers.Add(new Customer
                            {
                                Id = id,
                                FirstName = items[1],
                                LastName = items[2],
                                Age = age
                            });
                        }
                    }
                }

                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                return new List<Customer>();
            }
        }
    }
}

