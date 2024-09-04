﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class Floor
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public List<DesignProducts> Products { get; set; }
        public List<DesignNonProducts> NonProducts { get; set; }
    }
}
