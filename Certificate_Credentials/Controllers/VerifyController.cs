using Certificate_Credentials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Certificate_Credentials.Controllers
{
    public class VerifyController : Controller
    {
        // GET: Verify

        private readonly credentials_DBEntities dbContext = new credentials_DBEntities();

        public ActionResult Index(string certificateNo, string registrationNo)
        {
            IQueryable<interns_tbl> internsQuery = dbContext.interns_tbl.OrderByDescending(i => i.id);

            // Check if a valid certificate number is provided
            if (!string.IsNullOrEmpty(certificateNo))
            {
                // Check if certificate number has at least 8 characters
                if (certificateNo.Length < 8)
                {
                    ViewBag.ErrorMessage = "Enter complete certificate number (at least 8 characters).";
                    return View(new List<interns_tbl>());
                }

                internsQuery = internsQuery.Where(i => i.Internshipcertificateno.Contains(certificateNo));
            }
            else
            {
                // If certificate number is not provided, check for registration number
                if (!string.IsNullOrEmpty(registrationNo))
                {
                    if (registrationNo.Length < 8)
                    {
                        ViewBag.ErrorMessage = "Enter complete registration number (at least 8 characters).";
                        return View(new List<interns_tbl>());
                    }
                    internsQuery = internsQuery.Where(i => i.internregistrationno.Contains(registrationNo));
                }
                else
                {
                    // Neither certificate nor registration number provided, return an empty result
                    return View(new List<interns_tbl>());
                }
            }

            var intern = internsQuery.FirstOrDefault(); // Take the first result

            if (intern != null)
            {
                return View(new List<interns_tbl> { intern });
            }
            else
            {
                // No matching result found, return an empty result
                return View(new List<interns_tbl>());
            }
        }

    }
}