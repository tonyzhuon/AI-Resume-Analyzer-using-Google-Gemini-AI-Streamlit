namespace ResumeAnalyzer.Services;

public interface IPdfTextExtractorService
{
    string ExtractText(Stream pdfStream);
}
