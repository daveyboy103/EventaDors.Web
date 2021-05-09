using System;
using System.Data;

namespace EventaDors.Entities.Classes
{
    public class 
        Journey
    {
        public Journey()
        {
            EventDate = DateTime.Today;
            YourStory = "Tell us a bit more about your event...";
        }

        public Journey(IDataReader dr)
        {
            while (dr.Read())
            {
                if (dr["EventDate"] != DBNull.Value)
                    EventDate = dr.GetDateTime(dr.GetOrdinal("EventDate")); 
                
                if (dr["Created"] != DBNull.Value)
                    Created = dr.GetDateTime(dr.GetOrdinal("Created"));
                
                if (dr["Title"] != DBNull.Value)
                    Title = dr.GetString(dr.GetOrdinal("Title"));  
                
                if (dr["Password"] != DBNull.Value)
                    Password = dr.GetString(dr.GetOrdinal("Password"));  
                
                if (dr["EmailAddress"] != DBNull.Value)
                    Email = dr.GetString(dr.GetOrdinal("EmailAddress"));                
                
                if (dr["FirstName"] != DBNull.Value)
                    FirstName = dr.GetString(dr.GetOrdinal("FirstName"));
                
                if (dr["Surname"] != DBNull.Value)
                    Surname = dr.GetString(dr.GetOrdinal("Surname"));
                
                if (dr["PostalCode"] != DBNull.Value)
                    PostalCode = dr.GetString(dr.GetOrdinal("PostalCode"));
                
                if (dr["ContactNumber"] != DBNull.Value)
                    ContactNumber = dr.GetString(dr.GetOrdinal("ContactNumber"));
                
                if (dr["PartnerEmail"] != DBNull.Value)
                    PartnerEmail = dr.GetString(dr.GetOrdinal("PartnerEmail"));
                
                if (dr["InformPartner"] != DBNull.Value)
                    InformPartner = dr.GetBoolean(dr.GetOrdinal("InformPartner"));
                
                if (dr["CurrentPage"] != DBNull.Value)
                    CurrentPage = dr.GetString(dr.GetOrdinal("CurrentPage"));
                
                if (dr["YourStory"] != DBNull.Value)
                    YourStory = dr.GetString(dr.GetOrdinal("YourStory"));
                
                if (dr["Registered"] != DBNull.Value)
                    Registered = dr.GetBoolean(dr.GetOrdinal("Registered"));                
                
                if (dr["Completed"] != DBNull.Value)
                    Completed = dr.GetDateTime(dr.GetOrdinal("Completed"));
            }
        }
        public User User { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime? EventDate { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string PostalCode { get; set; }
        public string ContactNumber { get; set; }
        public string PartnerEmail { get; set; }
        public bool InformPartner { get; set; }
        public string CurrentPage { get; set; }
        public string YourStory { get; set; }
        public bool Registered { get; set; }
        
        public DateTime Created { get; set; }

        public bool IsEmpty =>
            string.IsNullOrEmpty(Title) &&
            string.IsNullOrEmpty(FirstName) &&
            string.IsNullOrEmpty(Surname) &&
            string.IsNullOrEmpty(PostalCode) &&
            string.IsNullOrEmpty(ContactNumber) &&
            !EventDate.HasValue &&
            string.IsNullOrEmpty(PartnerEmail);

        public DateTime? Completed { get; set; }
    }
}