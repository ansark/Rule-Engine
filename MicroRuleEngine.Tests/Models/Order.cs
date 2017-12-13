﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroRuleEngine.Tests.Models
{
    public class Order
    {
        public Order()
        {
            this.Items = new List<Item>();
        }
        public int OrderId { get; set; }
        public Customer Customer { get; set; }
        public List<Item> Items { get; set; }
        public bool HasItem(string itemCode)
        {
            return this.Items.Any(x => x.ItemCode == itemCode);
        }
    }
    public class Item{
        public decimal Cost{get;set;}
        public string ItemCode{get;set;}
    }

    public class Customer
    {
        public string FirstName {get;set;}
        public string LastName { get; set; }
        public Country Country {get;set;}
    }
    public class Country{
        public string CountryCode{get;set;}
    }
}
