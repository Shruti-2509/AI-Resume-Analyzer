using Xceed.Words.NET;

namespace ResumeAnalyzer.Utils
{
    public class DocParser
    {
        public static string ExtractText(string path)
        {
            using (DocX doc = DocX.Load(path))
            {
                return doc.Text;
            }
        }
    }
}