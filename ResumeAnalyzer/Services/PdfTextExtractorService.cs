using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using System.Text;

namespace ResumeAnalyzer.Services;

public class PdfTextExtractorService : IPdfTextExtractorService
{
    private readonly ILogger<PdfTextExtractorService> _logger;

    public PdfTextExtractorService(ILogger<PdfTextExtractorService> logger)
    {
        _logger = logger;
    }

    public string ExtractText(Stream pdfStream)
    {
        var text = new StringBuilder();

        // Primary: direct text extraction via PdfPig
        try
        {
            using var document = PdfDocument.Open(pdfStream);
            foreach (var page in document.GetPages())
            {
                var pageText = page.Text;
                if (!string.IsNullOrWhiteSpace(pageText))
                {
                    text.AppendLine(pageText);
                }
            }

            if (text.Length > 0)
            {
                return text.ToString().Trim();
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Direct PDF text extraction failed");
        }

        // Fallback: OCR via Tesseract for image-based PDFs
        _logger.LogInformation("Falling back to OCR for image-based PDF");
        try
        {
            text.Clear();
            pdfStream.Position = 0;

            using var engine = new Tesseract.TesseractEngine(@"./tessdata", "eng", Tesseract.EngineMode.Default);
            using var document = PdfDocument.Open(pdfStream);

            foreach (var page in document.GetPages())
            {
                // Extract images embedded in the page for OCR
                foreach (var image in page.GetImages())
                {
                    var imageBytes = image.RawBytes.ToArray();
                    using var pix = Tesseract.Pix.LoadFromMemory(imageBytes);
                    using var ocrPage = engine.Process(pix);
                    var ocrText = ocrPage.GetText();
                    if (!string.IsNullOrWhiteSpace(ocrText))
                    {
                        text.AppendLine(ocrText);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "OCR extraction failed");
        }

        return text.ToString().Trim();
    }
}
