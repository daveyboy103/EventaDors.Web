using System;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.Entities.Interfaces;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;

namespace EventaDors.WebApplication.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly Wrapper _wrapper;

        public RegistrationController(Wrapper wrapper)
        {
            _wrapper = wrapper;
        }
        // GET
        public IActionResult Index(Journey journey)
        {
            if (journey.Email == null)
            {
                journey.Email = TempDataHelper.Get(Statics.EmailTempData);
            }
            
            return View(journey);
        }

        [HttpPost]
        public IActionResult Step1(Journey journey)
        {
            TempDataHelper.Set(Statics.EmailTempData, journey.Email);

            if (_wrapper.LoginUser(new User
            {
                PrimaryEmail = journey.Email,
                UserName = journey.Email,
                CurrentPassword = journey.Password
            }))
            {
                _wrapper.PutJourney(journey);
                return View(journey);
            }
            
            return View(journey);
        }

        public IActionResult Register(Journey journey)
        {
            return View(journey);
        }
        
        
        [HttpPost]
        public IActionResult Step2(Journey journey)
        {
            if (journey.Email == null)
            {
                journey.Email = TempDataHelper.Get(Statics.EmailTempData);
            }

            User u = _wrapper.CreateUser(journey.Email);
            
            u.MetaData.Add(nameof(journey.Title), new MetaDataItem
            {
                Name = nameof(journey.Title), 
                Value = journey.Title, 
                Type = MetaDataType.String
            }); 
            
            u.MetaData.Add(nameof(journey.FirstName), new MetaDataItem
            {
                Name = nameof(journey.FirstName), 
                Value = journey.FirstName, 
                Type = MetaDataType.String
            });            
            
            u.MetaData.Add(nameof(journey.Surname), new MetaDataItem
            {
                Name = nameof(journey.Surname), 
                Value = journey.Surname, 
                Type = MetaDataType.String
            });            
            
            u.MetaData.Add(nameof(journey.PostalCode), new MetaDataItem
            {
                Name = nameof(journey.PostalCode), 
                Value = journey.PostalCode, 
                Type = MetaDataType.String
            });            
            
            u.MetaData.Add(nameof(journey.ContactNumber), new MetaDataItem
            {
                Name = nameof(journey.ContactNumber), 
                Value = journey.ContactNumber, 
                Type = MetaDataType.String
            });
                        
            u.MetaData.Add(nameof(journey.PartnerEmail), new MetaDataItem
            {
                Name = nameof(journey.PartnerEmail), 
                Value = journey.PartnerEmail, 
                Type = MetaDataType.String
            });            
            
            u.MetaData.Add(nameof(journey.InformPartner), new MetaDataItem
            {
                Name = nameof(journey.InformPartner), 
                Value = journey.InformPartner.ToString(), 
                Type = MetaDataType.Boolean
            });            
            
            u.MetaData.Add(nameof(journey.YourStory), new MetaDataItem
            {
                Name = nameof(journey.YourStory), 
                Value = journey.YourStory, 
                Type = MetaDataType.String
            });

            _wrapper.UpdateUserMetaData(u);
            
            _wrapper.PutJourney(journey);
            return View();
        }
        
        [HttpPost]
        public IActionResult Step3(Journey journey)
        {
            if (journey.Email == null)
            {
                journey.Email = TempDataHelper.Get(Statics.EmailTempData);
            }
            
            _wrapper.PutJourney(journey);
            return View();
        }
        
        [HttpPost]
        public IActionResult Step4(Journey journey)
        {
            return View();
        }

        public IActionResult Verify()
        {
            string g = Request.QueryString.ToString();

            if (_wrapper.VerifyUser(new Guid(g.Replace("?", ""))))
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult CompleteRegistration(Journey journey)
        {
            User u = _wrapper.RegisterUser(new User()
            {
                PrimaryEmail = journey.Email,
                CurrentPassword = journey.Password,
                UserName = journey.Email
            });

            var link = new LinkWrapper
            {
                Text = "Click here to verify new account",
                Link = $"https://localhost:5002/Registration/Verify/?{u.Uuid}"
            };
            
            return View(link);
        }
    }
}