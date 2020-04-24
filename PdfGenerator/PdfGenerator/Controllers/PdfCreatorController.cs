using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using PdfGenerator.Models;
using PdfGenerator.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PdfGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private IConverter _converter;

        public PdfCreatorController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePDF(EstructuraDatosPDF datosPDF)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //Asignación de Logo
            datosPDF.InfoGeneralCotizacion.Logo = Path.Combine(Directory.GetCurrentDirectory(), "assets", "red.png");
            bool result = await CreatePDfAsync(datosPDF).ConfigureAwait(false);

            if (result)
                return Ok(@"C:\PDFCreator\" + datosPDF.InfoGeneralCotizacion.Referencia + ".pdf");

            return BadRequest();
        }

        private async Task<bool> CreatePDfAsync(EstructuraDatosPDF datosPDF)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = @"C:\PDFCreator\" + datosPDF.InfoGeneralCotizacion.Referencia + ".pdf"
            };

            var templateGenerator = new TemplateGenerator(datosPDF);
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateGenerator.GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            try
            {
                await Task.Run(() => _converter.Convert(pdf)).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}