using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        List<Product> productList = new List<Product>();
        HttpClient client = new HttpClient();

        public ProductsController()
        {
            var product1 = new Product(1, "Meja", 300000);
            var product2 = new Product(2, "Lemari", 1200000);
            var product3 = new Product(3, "Kursi", 100000);
            productList = new List<Product>() { product1, product2, product3 };

            //client.DefaultRequestHeaders.Clear();
            //client.BaseAddress = new Uri("https://localhost:44349");
            //var response = client.GetAsync("/users-api").Result;
            //var token = response.Content.ReadAsStringAsync().Result;
            //client.DefaultRequestHeaders.Add("Authorization", token); //HttpContext.Session.GetString("Token"));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(productList);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = productList.Where(p => p.Id == id);
            return Ok(product);
        }
    }
}
