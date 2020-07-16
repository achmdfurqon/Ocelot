﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Product(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
