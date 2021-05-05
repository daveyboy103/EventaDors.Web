using System;
using EventaDors.Entities.Classes;

namespace EventaDors.WebApplication.Models
{
    public class Journey
    {
        public Journey()
        {
            EventDate = DateTime.Today;
            YourStory = "Tell us a bit more about your event...";
        }
        public User User { get; set; }
        public DateTime? EventDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PostalCode { get; set; }
        public string ContactNumber { get; set; }
        public string PartnerEmail { get; set; }
        public bool InformPartner { get; set; }
        public string NextPage { get; set; }
        public string YourStory { get; set; }

        public bool IsEmpty =>
            string.IsNullOrEmpty(Title) &&
            string.IsNullOrEmpty(FirstName) &&
            string.IsNullOrEmpty(Surname) &&
            string.IsNullOrEmpty(PostalCode) &&
            string.IsNullOrEmpty(ContactNumber) &&
            !EventDate.HasValue &&
            string.IsNullOrEmpty(PartnerEmail);
    }
}