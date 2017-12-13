using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroRuleEngine
{
    public class Appointment:ABC
    {
        public string Flow { get; set; }
        public string BE { get; set; }
        public string ContainerNumber { get; set; }
        public string BookingNumber { get; set; }
        public bool HandleFalse(int a)
        {
            return true;
        }
        private decimal Charges = 0;
        public List<string> messages = new List<string>();
        public string AddMessage(string msg)
        {
            messages.Add(msg);
            return msg;
        }
        public void SetCharges(string a)
        {
            Charges = decimal.Parse(a);
            
        }


        string ABC.getMessage()
        {
            return messages[0];
        }
        public bool CheckLength()
        {
            return true;
        }
        public bool AppointMentMaxDays()
        {
            return true;
        }
        public DateTime Date { get; set; }
        public DateTime TimeSlotFrom { get; set; }
        public DateTime TimeSlotTo { get; set; }
        public string TruckType { get; set; }
        public string DriverContact { get; set; }
        public string DriverName { get; set; }
        public string NotificationType { get; set; }
        public bool AlreadyBooked { get; set; }


    }
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
