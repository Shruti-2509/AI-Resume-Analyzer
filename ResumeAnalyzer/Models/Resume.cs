namespace ResumeAnalyzer.Models
{
    public class Resume
    {
        public int resume_id { get; set; }
        public int user_id { get; set; }
        public string file_path { get; set; }
        public string extracted_text { get; set; }
    }
}