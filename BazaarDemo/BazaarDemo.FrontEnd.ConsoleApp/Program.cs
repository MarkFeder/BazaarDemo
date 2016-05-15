using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using BazaarDemo.BackEnd.Domain.Models;
using System.Net;
using BazaarDemo.FrontEnd.ConsoleApp.BazaarService;

namespace BazaarDemo.FrontEnd.ConsoleApp
{
    public class Program
    {
        #region Properties

        private static HttpClient client;
        // Change port number if doesn't fit with your server
        private static int port = 5369;
        private static string baseAddress = null;

        private static BazaarService.IBazaarService BazaarService { get; set; }

        #endregion

        public static void Main(string[] args)
        {
            SetUpClient();

            // BazaarService task

            Task0();

            // OData tasks

            Task1();

            Task2();

            Console.ReadLine();
        }

        #region Tasks
        private static void Task0()
        {
            Console.WriteLine("The number of products in Bazaar is : " + BazaarService.TotalProductsInBazaar());

            BazaarService = null;
        }

        private static void Task1()
        {
            // Read Metadata

            var resp1 = Task.Run(() => client.GetAsync(baseAddress + "api/$metadata").Result.Content.ReadAsStringAsync()).Result;

            Console.WriteLine((resp1 == null ? null : resp1));
        }

        private static void Task2()
        {
            // Post a ProductModel

            ProductModel product = new ProductModel()
            {
                ProductName = "NewProductToInsert"
            };

            HttpStatusCode statusCode;

            var resp2 = Task.Run(() => client.PostAsJsonAsync(baseAddress + "api/Products", product));
            statusCode = resp2.Result.StatusCode;

            var savedProduct = resp2.Result.Content.ReadAsAsync<ProductModel>().Result;

            Console.WriteLine((savedProduct == null ? null : "[ProductId: " + savedProduct.ProductId + ", ProductName: " + savedProduct.ProductName + "]"));
        }

        #endregion

        #region Client

        private static void SetUpClient()
        {
            baseAddress = "http://localhost:" + port + "/";

            // Client configuration
            client = new HttpClient();

            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion
    }
}
