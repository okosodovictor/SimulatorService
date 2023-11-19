using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;
using RestServerSolution.Models;

namespace SimulatorSolution;

class Program
{
    public static async Task Main(string[] args)
    {
        //Sample Test case provided.

        await SimulateWithCustomRequest();

        await SimulateWithRandomRequest();

        Console.ReadLine();
    }
    private static async Task SimulateWithCustomRequest()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://localhost:7075");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var customers1 = new List<Customer>
            {
                new Customer { Id = 3, LastName = "Aaaa", FirstName = "Aaaa", Age = 20 },
                new Customer { Id = 2, LastName = "Aaaa", FirstName = "Bbbb", Age = 56 },
                new Customer { Id = 5, LastName = "Cccc", FirstName = "Aaaa", Age = 32 },
                new Customer { Id = 1, LastName = "Cccc", FirstName = "Bbbb", Age = 50 },
                new Customer { Id = 4, LastName = "Dddd", FirstName = "Aaaa", Age = 70 }
            };

            Console.WriteLine();
            Console.WriteLine("About to post following Customers:");
            foreach (var customer in customers1)
            {
                Console.WriteLine($"ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}, Age: {customer.Age}");
            }

            Console.WriteLine();

            await PostCustomersAsync(client, "/Customers", customers1);

            var customers2 = new List<Customer>
            {
                new Customer { Id = 6, LastName = "Bbbb", FirstName = "Bbbb", Age = 26 },
            };

            Console.WriteLine();
            Console.WriteLine("About to post following Customers:");
            foreach (var customer in customers2)
            {
                Console.WriteLine($"ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}, Age: {customer.Age}");
            }

            Console.WriteLine();
            await PostCustomersAsync(client, "/Customers", customers2);


            var customers3 = new List<Customer>
            {
                new Customer { Id = 7, LastName = "Bbbb", FirstName = "Aaaa", Age = 28 },
            };

            Console.WriteLine();
            Console.WriteLine("About to post following Customers:");
            foreach (var customer in customers3)
            {
                Console.WriteLine($"ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}, Age: {customer.Age}");
            }

            await PostCustomersAsync(client, "/Customers", customers3);
        }
    }

    private static async Task SimulateWithRandomRequest()
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://localhost:7075");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            await PostRandomCustomersAsync(client, "/Customers");
        }
    }

    public static async Task PostRandomCustomersAsync(HttpClient httpClient, string url)
    {
        var random = new Random();

        var firstNames = new List<string> { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
        var lastNames = new List<string> { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

        var allCustomers = new List<Customer>();
        int totalCustomers = firstNames.Count;

        for (int i = 1; i <= totalCustomers; i++)
        {
            var customer = new Customer
            {
                Id = i,
                FirstName = firstNames[random.Next(firstNames.Count)],
                LastName = lastNames[random.Next(lastNames.Count)],
                Age = random.Next(19, 101)
            };

            allCustomers.Add(customer);
        }

        int batchSize = 2;
        // Sending customers in batches of 2
        for (int i = 0; i < totalCustomers; i += batchSize)
        {
            var customersBatch = allCustomers.Skip(i).Take(batchSize).ToList();

            Console.WriteLine();
            Console.WriteLine("About to post following Customers:");

            foreach (var customer in customersBatch)
            {
                Console.WriteLine($"ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}, Age: {customer.Age}");
            }

            await PostCustomersAsync(httpClient, url, customersBatch);
        }
    }

    private static async Task PostCustomersAsync(HttpClient httpClient, string url, List<Customer> customersBatch)
    {
        var response = await httpClient.PostAsJsonAsync(url, customersBatch);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("After Insert Customers:\n");
            await GetCustomersAsync(httpClient, url);
        }
        else
        {
            Console.WriteLine($"Failed: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Failed response content: {content}");
        }
    }

    public static async Task GetCustomersAsync(HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var customers = JsonConvert.DeserializeObject<List<Customer>>(content);

            if (customers!= null && customers.Count > 0)
            {
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.Id}, Name: {customer.FirstName} {customer.LastName}, Age: {customer.Age}");
                }
            }
        }
        else
        {
            Console.WriteLine($"GET request failed and status code is: {response.StatusCode}");
        }
    }
}

