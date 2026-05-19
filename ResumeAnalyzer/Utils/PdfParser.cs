using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace ResumeAnalyzer.Utils
{
    public class PdfParser
    {
        public static string ExtractText(string path)
        {
            string text = "";

            using (PdfReader reader = new PdfReader(path))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                {
                    text += PdfTextExtractor.GetTextFromPage(pdf.GetPage(i));
                }
            }

            return text;
        }
    }
}