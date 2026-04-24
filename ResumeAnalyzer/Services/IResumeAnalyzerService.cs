namespace ResumeAnalyzer.Services;

public interface IResumeAnalyzerService
{
    Task<string> AnalyzeResumeAsync(string resumeText, string? jobDescription = null);
}
