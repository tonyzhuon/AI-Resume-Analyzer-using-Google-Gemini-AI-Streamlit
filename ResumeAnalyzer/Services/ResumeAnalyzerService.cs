using Mscc.GenerativeAI;

namespace ResumeAnalyzer.Services;

public class ResumeAnalyzerService : IResumeAnalyzerService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ResumeAnalyzerService> _logger;

    public ResumeAnalyzerService(IConfiguration configuration, ILogger<ResumeAnalyzerService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> AnalyzeResumeAsync(string resumeText, string? jobDescription = null)
    {
        if (string.IsNullOrWhiteSpace(resumeText))
        {
            return "Resume text is required for analysis.";
        }

        var apiKey = _configuration["GeminiAI:ApiKey"];
        var modelName = _configuration["GeminiAI:Model"] ?? "gemini-1.5-flash";

        var googleAi = new GoogleAI(apiKey);
        var model = googleAi.GenerativeModel(model: modelName);

        var prompt = $@"
You are an experienced HR with Technical Experience in the field of any one job role from Data Science, Data Analyst, DevOPS, Machine Learning Engineer, Prompt Engineer, AI Engineer, Full Stack Web Development, Big Data Engineering, Marketing Analyst, Human Resource Manager, Software Developer your task is to review the provided resume.
Please share your professional evaluation on whether the candidate's profile aligns with the role. Also mention Skills he already has and suggest some skills to improve his resume, also suggest some courses he might take to improve the skills. Highlight the strengths and weaknesses.

Resume:
{resumeText}";

        if (!string.IsNullOrWhiteSpace(jobDescription))
        {
            prompt += $@"

Additionally, compare this resume to the following job description:

Job Description:
{jobDescription}

Highlight the strengths and weaknesses of the applicant in relation to the specified job requirements.";
        }

        _logger.LogInformation("Sending resume analysis request to Gemini AI");
        var response = await model.GenerateContent(prompt);

        return response?.Text?.Trim() ?? "No analysis was returned from the AI model.";
    }
}
