using Certificate_Credentials.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;


namespace Certificate_Credentials.Controllers
{
    [Authorize]
    public class credentialsController : Controller
    {
        private credentials_DBEntities dbContext = new credentials_DBEntities();
        // GET: credentials
        public ActionResult Index(int? page, string searchTerm, string certificateNo, string registrationNo, string internName)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IQueryable<interns_tbl> internsQuery = dbContext.interns_tbl.OrderByDescending(i => i.id);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Filter by email, certificate number, registration number, and name
                internsQuery = internsQuery
                    .Where(i => i.internemail.Contains(searchTerm) ||
                                i.Internshipcertificateno.Contains(searchTerm) ||
                                i.internregistrationno.Contains(searchTerm) ||
                                i.internname.Contains(searchTerm));
            }
            else
            {
                // Additional filters for certificate number, registration number, and name
                if (!string.IsNullOrEmpty(certificateNo))
                {
                    internsQuery = internsQuery.Where(i => i.Internshipcertificateno.Contains(certificateNo));
                }

                if (!string.IsNullOrEmpty(registrationNo))
                {
                    internsQuery = internsQuery.Where(i => i.internregistrationno.Contains(registrationNo));
                }

                if (!string.IsNullOrEmpty(internName))
                {
                    internsQuery = internsQuery.Where(i => i.internname.Contains(internName));
                }
            }

            var interns = internsQuery.ToList();

            return View(interns.ToPagedList(pageNumber, pageSize));
        }





        [HttpGet]
        public ActionResult Create()
        {
            var intern = new interns_tbl
            {
                internregistrationno = GenerateUniqueRegistrationNumber(),
                Internshipcertificateno = GenerateUniqueCertificateNumber()
            };

            return View(intern);
        }

        private string GenerateUniqueRegistrationNumber()
        {
            // Implement your logic to generate a unique registration number (e.g., combining a prefix and a unique identifier)
            // Example: "REG-20220001"
            return "REG-" + DateTime.Now.Year + dbContext.interns_tbl.Count().ToString("D4");
        }

        private string GenerateUniqueCertificateNumber()
        {
            // Implement your logic to generate a unique certificate number (e.g., combining a prefix and a unique identifier)
            // Example: "CERT-20220001"
            return "CERT-" + DateTime.Now.Year + dbContext.interns_tbl.Count().ToString("D4");
        }


        [HttpPost]
        public ActionResult Create(interns_tbl intern, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Validate uniqueness of registration and certificate numbers
                    bool isRegistrationNoUnique = !dbContext.interns_tbl.Any(i => i.internregistrationno == intern.internregistrationno);
                    bool isCertificateNoUnique = !dbContext.interns_tbl.Any(i => i.Internshipcertificateno == intern.Internshipcertificateno);

                    if (!isRegistrationNoUnique)
                    {
                        ModelState.AddModelError("intern.internregistrationno", "Registration number must be unique.");
                    }

                    if (!isCertificateNoUnique)
                    {
                        ModelState.AddModelError("intern.Internshipcertificateno", "Certificate number must be unique.");
                    }

                    if (isRegistrationNoUnique && isCertificateNoUnique)
                    {
                        // Save intern to the database
                        intern.ImageData = ConvertToByteArray(imageFile);
                        intern.PdfData = ConvertToByteArray(pdfFile);

                        dbContext.interns_tbl.Add(intern);
                        dbContext.SaveChanges();

                       // var orderedCertificates = dbContext.interns_tbl.OrderByDescending(i => i.CreationDate).ToList();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return View(intern);
        }

        public ActionResult Edit(int id)
        {
            interns_tbl intern = dbContext.interns_tbl.Find(id);

            if (intern == null)
            {
                return HttpNotFound();
            }

            return View(intern);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(interns_tbl intern, HttpPostedFileBase imageFile, HttpPostedFileBase pdfFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Validate uniqueness of registration and certificate numbers
                    bool isRegistrationNoUnique = !dbContext.interns_tbl.Any(i => i.id != intern.id && i.internregistrationno == intern.internregistrationno);
                    bool isCertificateNoUnique = !dbContext.interns_tbl.Any(i => i.id != intern.id && i.Internshipcertificateno == intern.Internshipcertificateno);

                    if (!isRegistrationNoUnique)
                    {
                        ModelState.AddModelError("intern.internregistrationno", "Registration number must be unique.");
                    }

                    if (!isCertificateNoUnique)
                    {
                        ModelState.AddModelError("intern.Internshipcertificateno", "Certificate number must be unique.");
                    }

                    if (isRegistrationNoUnique && isCertificateNoUnique)
                    {
                        intern.ImageData = ConvertToByteArray(imageFile);
                        intern.PdfData = ConvertToByteArray(pdfFile);

                        dbContext.Entry(intern).State = System.Data.Entity.EntityState.Modified;
                        dbContext.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            return View(intern);
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            interns_tbl intern = dbContext.interns_tbl.Find(id);

            if (intern == null)
            {
                return HttpNotFound();
            }

            return View(intern);
        }


        public ActionResult Delete(int id)
        {
            interns_tbl intern = dbContext.interns_tbl.Find(id);

            if (intern == null)
            {
                return HttpNotFound();
            }

            return View(intern);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            interns_tbl intern = dbContext.interns_tbl.Find(id);

            if (intern == null)
            {
                return HttpNotFound();
            }

            try
            {
                dbContext.interns_tbl.Remove(intern);
                dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                return View(intern);
            }


        }

        // Helper method to convert HttpPostedFileBase to byte[]
        private byte[] ConvertToByteArray(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }

        public ActionResult GetPdf(int id)
        {
            var intern = dbContext.interns_tbl.Find(id);

            if (intern != null && intern.PdfData != null)
            {
                return File(intern.PdfData, "application/pdf");
            }

            return HttpNotFound();
        }


        public ActionResult ExportToExcel()
        {
            var interns = dbContext.interns_tbl.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Interns");

                // Add headers with style and set column width
                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Cell(1).Value = "Intern Name";
                headerRow.Cell(2).Value = "Intern Email";
                headerRow.Cell(3).Value = "Intern Registration No";
                headerRow.Cell(4).Value = "Internship Start Date";
                headerRow.Cell(5).Value = "Internship End Date";
                headerRow.Cell(6).Value = "Internship Certificate No";
                headerRow.Cell(7).Value = "Internship Domain";
                headerRow.Cell(8).Value = "Internship Duration";

                // Set column width for date columns
                worksheet.Column(4).Width = 15; // Adjust the width as needed for the start date column
                worksheet.Column(5).Width = 15; // Adjust the width as needed for the end date column

                // Add data with style
                for (int i = 0; i < interns.Count; i++)
                {
                    var intern = interns[i];
                    var dataRow = worksheet.Row(i + 2);
                    dataRow.Cell(1).Value = intern.internname;
                    dataRow.Cell(2).Value = intern.internemail;
                    dataRow.Cell(3).Value = intern.internregistrationno;
                    dataRow.Cell(4).Value = intern.internshipstartdate;
                    dataRow.Cell(5).Value = intern.internshipenddate;
                    dataRow.Cell(6).Value = intern.Internshipcertificateno;
                    dataRow.Cell(7).Value = intern.Domain;
                    dataRow.Cell(8).Value = intern.Duration;

                    // Add other properties...
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Interns.xlsx");
                }
            }
        }

        public ActionResult DownloadPdf(int id)
        {
            var intern = dbContext.interns_tbl.Find(id);

            if (intern != null && intern.PdfData != null)
            {
                // Set the file name for download
                string fileName = $"InternCertificate_{intern.internname}.pdf";

                // Return the PDF file as a downloadable attachment
                return File(intern.PdfData, "application/pdf", fileName);
            }

            return HttpNotFound();
        }




    }
}