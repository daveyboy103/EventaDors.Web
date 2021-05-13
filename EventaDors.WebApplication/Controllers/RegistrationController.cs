using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using EventaDors.DataManagement;
using EventaDors.Entities.Classes;
using EventaDors.Entities.Interfaces;
using EventaDors.WebApplication.Helpers;
using EventaDors.WebApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace EventaDors.WebApplication.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly Wrapper _wrapper;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(Wrapper wrapper, ILogger<RegistrationController> logger)
        {
            _wrapper = wrapper;
            _logger = logger;
        }
        // GET
        public IActionResult Index(Journey journey)
        {
            ModelState.ClearValidationState("Email");
            ModelState.ClearValidationState("Password");
            
            if (journey.Email == null)
            {
                journey.Email = (HttpContext.Session.Keys.Contains(Statics.EmailTempData) ? HttpContext.Session.GetString(Statics.EmailTempData) : string.Empty) ;;
            }
            
            return View(journey);
        }

        [HttpPost]
        public IActionResult Step1(Journey journey)
        {
            try
            {
                var password = journey.Password;

                journey = _wrapper.GetJourney(journey.Email);
                journey.Password = password;
                
                if (_wrapper.LoginUser(new User
                {
                    PrimaryEmail = journey.Email,
                    UserName = journey.Email,
                    CurrentPassword = journey.Password
                }))
                {
                    if(!HttpContext.Session.Keys.Contains(Statics.EmailTempData))
                        HttpContext.Session.SetString(Statics.EmailTempData, journey.Email);

                    if (journey.QuoteIdIdentity.HasValue)
                    {
                        return RedirectToAction("ProcessTemplate", "QuoteTemplate", new { quoteIdIdentity = journey.QuoteIdIdentity});
                    }

                    if (journey.Completed.HasValue)
                    {
                        if(!HttpContext.Session.Keys.Contains(Statics.EmailTempData))
                            HttpContext.Session.SetString(Statics.EmailTempData, journey.Email);
                        return RedirectToAction("Step3");
                    }
                
                    return View(journey);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.Log(LogLevel.Error, e.Message);
                Response.WriteAsync(e.Message);
                Response.WriteAsync(e.StackTrace);
            }

            return RedirectToAction("LoginFailed");
        }

        public IActionResult LoginFailed()
        {
            return View();
        }       
        
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public IActionResult Register(Journey journey)
        {
            var mode = Request.Query["mode"];

            if (mode.ToString() == "Log Out")
            {
                HttpContext.Session.Remove(Statics.EmailTempData);
                return RedirectToAction("Index", "Registration");
            }
       
            return View(journey);
        }
        
        [HttpPost]
        public IActionResult Step2(Journey journey, string YourStory)
        {
            if (journey.Email == null)
            {
                journey.Email = (HttpContext.Session.Keys.Contains(Statics.EmailTempData) ? HttpContext.Session.GetString(Statics.EmailTempData) : string.Empty) ;
            }

            journey.YourStory = YourStory;
            User u = _wrapper.CreateUser(journey.Email);
            
            if(!u.MetaData.ContainsKey(nameof(journey.Title)))
            {
                u.MetaData.Add(nameof(journey.Title), new MetaDataItem
                {
                    Name = nameof(journey.Title),
                    Value = journey.Title,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.Title)].Value = journey.Title;
            }

            if (!u.MetaData.ContainsKey(nameof(journey.FirstName)))
            {
                u.MetaData.Add(nameof(journey.FirstName), new MetaDataItem
                {
                    Name = nameof(journey.FirstName),
                    Value = journey.FirstName,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.FirstName)].Value = journey.FirstName;
            }

            if (!u.MetaData.ContainsKey(nameof(journey.Surname)))
            {
                u.MetaData.Add(nameof(journey.Surname), new MetaDataItem
                {
                    Name = nameof(journey.Surname),
                    Value = journey.Surname,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.Surname)].Value = journey.Surname;
            }

            if (!u.MetaData.ContainsKey(nameof(journey.PostalCode)))
            {
                u.MetaData.Add(nameof(journey.PostalCode), new MetaDataItem
                {
                    Name = nameof(journey.PostalCode),
                    Value = journey.PostalCode,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.PostalCode)].Value = journey.PostalCode;
            }

            if (!u.MetaData.ContainsKey(nameof(journey.ContactNumber)))
            {
                u.MetaData.Add(nameof(journey.ContactNumber), new MetaDataItem
                {
                    Name = nameof(journey.ContactNumber),
                    Value = journey.ContactNumber,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.ContactNumber)].Value = journey.ContactNumber;
            }

            if (!u.MetaData.ContainsKey(nameof(journey.PartnerEmail)))
            {
                u.MetaData.Add(nameof(journey.PartnerEmail), new MetaDataItem
                {
                    Name = nameof(journey.PartnerEmail),
                    Value = journey.PartnerEmail,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.PartnerEmail)].Value = journey.PartnerEmail;
            }

            if (!u.MetaData.ContainsKey(nameof(journey.InformPartner)))
            {
                u.MetaData.Add(nameof(journey.InformPartner), new MetaDataItem
                {
                    Name = nameof(journey.InformPartner),
                    Value = journey.InformPartner.ToString(),
                    Type = MetaDataType.Boolean
                });
            }
            else
            {
                u.MetaData[nameof(journey.InformPartner)].Value = journey.InformPartner.ToString();
            }

            if (!u.MetaData.ContainsKey(nameof(journey.YourStory)))
            {
                u.MetaData.Add(nameof(journey.YourStory), new MetaDataItem
                {
                    Name = nameof(journey.YourStory),
                    Value = journey.YourStory,
                    Type = MetaDataType.String
                });
            }
            else
            {
                u.MetaData[nameof(journey.YourStory)].Value = journey.YourStory.ToString();
            }

            _wrapper.UpdateUserMetaData(u);

            journey.Completed = DateTime.UtcNow;
            _wrapper.PutJourney(journey);
            return View();
        }
        
        public IActionResult Step3(Journey journey)
        {
            if (journey.Email == null)
            {
                journey.Email = (HttpContext.Session.Keys.Contains(Statics.EmailTempData) ? HttpContext.Session.GetString(Statics.EmailTempData) : string.Empty) ;
            }

            var user  = _wrapper.CreateUser(journey.Email);

            var templates = _wrapper.GetQuoteTemplates();

            var payloadWrapper = new EmailPayloadWrapper<IEnumerable<QuoteTemplate>>
            {
                Email = journey.Email,
                Payload = templates
            };
            
            return View(payloadWrapper);
        }
        
        [HttpPost]
        public IActionResult Step4(Journey journey)
        {
            return View();
        }

        public IActionResult Verify()
        {
            string g = Request.QueryString.ToString();

            User u = _wrapper.VerifyUser(new Guid(g.Replace("?", "")));
            
            if(u != null)
            {
                HttpContext.Session.SetString(Statics.EmailTempData, u.PrimaryEmail);
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
            _wrapper.PutJourney(journey);
            
            var link = new LinkWrapper
            {
                Text = "Click here to verify new account",
                Link = $"Verify/?{u.Uuid}"
            };
            
            return View(link);
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
    }
}