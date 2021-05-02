using System;
using EventaDors.Entities.Classes;

namespace EventaDors.WebApplication.Models
{
    public class Journey
    {
        public Journey()
        {
            EventDate = DateTime.Today;
        }
        public User User { get; set; }
        public DateTime EventDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PostalCode { get; set; }
        public string ContactNumber { get; set; }
    }
}