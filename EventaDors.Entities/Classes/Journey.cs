using System;
using System.ComponentModel.DataAnnotations;
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
                
                if (dr["QuoteIdIdentity"] != DBNull.Value)
                    QuoteIdIdentity = dr.GetInt32(dr.GetOrdinal("QuoteIdIdentity"));
            }
        }

        public int? QuoteIdIdentity { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "* Email is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "* Password is required")]
        [Compare("RepeatPassword")]
        public string Password { get; set; }
        [Required]
        public DateTime? EventDate { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "*")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "*")]
        [Phone]
        public string ContactNumber { get; set; }
        [EmailAddress]
        public string PartnerEmail { get; set; }
        public bool InformPartner { get; set; }
        public string CurrentPage { get; set; }
        [Required(ErrorMessage = "*")]
        public string YourStory { get; set; }
        public bool Registered { get; set; }
        public DateTime Created { get; set; }
        
        public bool AgreeConditions { get; set; }

        public DateTime? Completed { get; set; }
        public string RepeatPassword { get; set; }
    }
}