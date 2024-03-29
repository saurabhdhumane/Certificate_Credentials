//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Certificate_Credentials.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class interns_tbl
    {
        public int id { get; set; }

        [Display(Name = "Intern Name")]
        [Required(ErrorMessage = "Intern Name Is Required.")]
        public string internname { get; set; }

        [Display(Name ="Intern Email")]
        [Required(ErrorMessage = "Intern Email Is Required.")]
        [EmailAddress(ErrorMessage = "Invalid email Address.")]
        public string internemail { get; set; }

        [Display(Name ="Intern Registration No")]
        [Required(ErrorMessage = "Intern Registration No Is Required.")]
        public string internregistrationno { get; set; }

        [Display(Name = "Internship Start Date")]
        [Required(ErrorMessage = "Internship Start Date Is Required.")]
        public System.DateTime internshipstartdate { get; set; }

        [Display(Name = "Internship End Date")]
        [Required(ErrorMessage = "Internship End Date Is Required.")]
        public System.DateTime internshipenddate { get; set; }

        [Display(Name = "Internship Certificate No")]
        [Required(ErrorMessage = "Internship Certificate No Is Required.")]
        public string Internshipcertificateno { get; set; }

        [Display(Name = "Internship Domain")]
        [Required(ErrorMessage = "Internship Domain Is Required.")]
        public string Domain { get; set; }

        [Display(Name = "Internship Duration")]
        [Required(ErrorMessage = "Internship Duration Is Required.")]
        public int Duration { get; set; }

        [Display(Name = "Internship Certificate (Image)")]
        //[Required(ErrorMessage = "Internship Certificate Image Is Required.")]
        public byte[] ImageData { get; set; }

        [Display(Name = "Internship Certificate (PDF)")]
        //[Required(ErrorMessage = "Internship Certificate PDF Is Required.")]
        public byte[] PdfData { get; set; }
        public object CreationDate { get; internal set; }
    }
}
