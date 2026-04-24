using Microsoft.AspNetCore.Mvc;
using ResumeAnalyzer.Models;
using ResumeAnalyzer.Services;

namespace ResumeAnalyzer.Controllers;

public class ResumeController : Controller
{
    private readonly IPdfTextExtractorService _pdfExtractor;
    private readonly IResumeAnalyzerService _resumeAnalyzer;
    private readonly ILogger<ResumeController> _logger;

    public ResumeController(
        IPdfTextExtractorService pdfExtractor,
        IResumeAnalyzerService resumeAnalyzer,
        ILogger<ResumeController> logger)
    {
        _pdfExtractor = pdfExtractor;
        _resumeAnalyzer = resumeAnalyzer;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ResumeAnalysisViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
    public async Task<IActionResult> Analyze(ResumeAnalysisViewModel model)
    {
        if (model.Resume == null || model.Resume.Length == 0)
        {
            model.ErrorMessage = "Please upload a resume in PDF format.";
            return View("Index", model);
        }

        var extension = Path.GetExtension(model.Resume.FileName).ToLowerInvariant();
        if (extension != ".pdf")
        {
            model.ErrorMessage = "Only PDF files are accepted.";
            return View("Index", model);
        }

        try
        {
            // Extract text from the uploaded PDF
            using var stream = model.Resume.OpenReadStream();
            var resumeText = _pdfExtractor.ExtractText(stream);

            if (string.IsNullOrWhiteSpace(resumeText))
            {
                model.ErrorMessage = "Could not extract text from the uploaded PDF. The file may be empty or corrupted.";
                return View("Index", model);
            }

            // Analyze resume with Gemini AI
            model.AnalysisResult = await _resumeAnalyzer.AnalyzeResumeAsync(resumeText, model.JobDescription);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Resume analysis failed");
            model.ErrorMessage = $"Analysis failed: {ex.Message}";
        }

        return View("Index", model);
    }
}
