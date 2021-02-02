using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SelectPdf;
using SparkAuto.Data;
using SparkAuto.Models;

namespace SparkAuto.Pages.Services
{
    public class InvoiceModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public InvoiceModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public ServiceHeader serviceHeader { get; set; }
        public List<ServiceDetails> serviceDetails { get; set; }

        public void OnGet(int serviceId)
        {
            serviceHeader = _db.ServiceHeader.Include(s => s.Car).Include(s => s.Car.ApplicationUser).FirstOrDefault(s => s.Id == serviceId);
            serviceDetails = _db.ServiceDetails.Where(s => s.ServiceHeaderId == serviceId).ToList();
            DdlPageSize = "A4";
        }

        [BindProperty]
        public string TxtUrl { get; set; }

        [BindProperty]
        public string DdlPageSize { get; set; }

        [BindProperty]
        public string TxtHTMLCode { get; set; }
        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "A1", Text = "A1" },
            new SelectListItem { Value = "A2", Text = "A2" },
            new SelectListItem { Value = "A3", Text = "A3" },
            new SelectListItem { Value = "A4", Text = "A4" },
            new SelectListItem { Value = "A5", Text = "A5" },
            new SelectListItem { Value = "Letter", Text = "Letter" },
            new SelectListItem { Value = "HalfLetter", Text = "HalfLetter" },
            new SelectListItem { Value = "Ledger", Text = "Ledger" },
            new SelectListItem { Value = "Legal", Text = "Legal" },
        };

        [BindProperty]
        public string DdlPageOrientation { get; set; }

        public List<SelectListItem> PageOrientations { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Portrait", Text = "Portrait" },
            new SelectListItem { Value = "Landscape", Text = "Landscape" },
        };

        [BindProperty]
        public string TxtWidth { get; set; }

        [BindProperty]
        public string TxtHeight { get; set; }



        public IActionResult OnPost()
        {
            // read parameters from the webpage
            PdfPageSize pageSize =
                (PdfPageSize)Enum.Parse(typeof(PdfPageSize), DdlPageSize, true);

            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                DdlPageOrientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = System.Convert.ToInt32(1024);
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                webPageHeight = System.Convert.ToInt32(TxtHeight);
            }
            catch { }

            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(TxtUrl);

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            return fileResult;

        }
    }
}
