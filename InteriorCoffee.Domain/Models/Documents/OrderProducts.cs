﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class OrderProducts
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<string> Category { get; set; }
        public string Dimensions { get; set; }
        public List<string> Materials { get; set; }
    }
}
