using System.ComponentModel.DataAnnotations;

namespace ResumeAnalyzer.Models;

public class ResumeAnalysisViewModel
{
    [Display(Name = "Upload your resume (PDF)")]
    public IFormFile? Resume { get; set; }

    [Display(Name = "Job Description")]
    public string? JobDescription { get; set; }

    public string? AnalysisResult { get; set; }

    public string? ErrorMessage { get; set; }
}
