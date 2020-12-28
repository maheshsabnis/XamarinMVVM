using MVVM_XamarinApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_XamarinApp.Services
{
    public class ProductService
    {
        string url;
        HttpClient client;

        public ProductService()
        {
            url = "https://apiapptrainingnewapp.azurewebsites.net/api/Products";
            client = new HttpClient();
        }

        public async Task<List<ProductInfo>> GetData()
        {
            List<ProductInfo> products = new List<ProductInfo>();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductInfo>>(content);
            }

            return products;
        }

        public async Task<ProductInfo> PostProduct(ProductInfo product)
        {
            
            string json = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;

            response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                string respData = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<ProductInfo>(respData);
            }
            return product;
        }
    }
}
