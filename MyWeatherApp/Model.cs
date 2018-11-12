using System;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MyWeatherApp
{
    public class Model
    {
        private const string URI = "api.openweathermap.org/data/2.5/forecast";

        private const string APPID = "bbee93d67b25c3a25d873df876df5b23";
        
        private string _location;
        private int _daysAhead;
        
        
        HttpClient client = new HttpClient();

        public Model()
        {
            
        }


        async Task<Forecast> GetForecast(string location, int days)
        {
            if (location is null)
            {
                //запрос к сервису определения адреса по айпи
            }

            else
            {
                
            }
            
            Forecast forecast = null;
            string path = null;
            
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                forecast = await response.Content.ReadAsAsync<Forecast>();
            }
            
            return forecast;
        }

        async Task<Forecast> GetProductAsync(string path)
        {
            Forecast forecast = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                forecast = await response.Content.ReadAsAsync<Forecast>();
            }

            return forecast;
        }

        void MainMethod()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:64195/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Create a new product
                Forecast forecast = new Forecast()
                {
                    Location = "",
                    Temperature = ""
                };

                //0var url = await CreateProductAsync(forecast);
                //Console.WriteLine($"Created at {url}");

                // Get the product
                //forecast = await GetProductAsync(url.PathAndQuery);
                //ShowProduct(forecast);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}