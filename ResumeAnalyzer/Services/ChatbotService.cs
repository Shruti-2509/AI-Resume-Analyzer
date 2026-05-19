using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace ResumeAnalyzer.Services
{
    public class ChatbotService
    {
        public static async Task<string> GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                // ✅ OpenRouter API Key
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        "YOUR_OPENROUTER_API_KEY"
                    );

                //YOUR_OPENROUTER_API_KEY:sk - or - v1 - 7154d5fd992f8ca37b6e8cbccddca0b889bd5845e1ee4cdc52819e115b01164d

                // ✅ Required Headers
                client.DefaultRequestHeaders.Add(
                    "HTTP-Referer",
                    "http://localhost:5000"
                );

                client.DefaultRequestHeaders.Add(
                    "X-Title",
                    "Resume Analyzer"
                );

                // ✅ Request Body
                var body = new
                {
                    model = "openai/gpt-3.5-turbo",

                    messages = new object[]
{
    // ✅ SYSTEM PROMPT
    new
    {
        role = "system",
        content = @"You are an AI assistant for the AI Resume Analyzer project.

This website is built using ASP.NET Core MVC and MySQL.

Features of the project:
- User Registration and Login
- Admin Login
- Resume Upload in PDF and DOCX
- Resume Parsing using iText7 and DocX
- AI Resume Analysis
- Skill Extraction
- Job Matching System
- Match Percentage Calculation
- Suggestions for Missing Skills
- Admin Dashboard
- Manage Jobs
- Manage Candidates
- Top Candidates
- Resume History

Your responsibilities:
- Help users use the website
- Guide users about resume upload
- Explain job matching
- Suggest improvements for resumes
- Explain missing skills
- Help users improve resume score

Answer ONLY according to this project.
Give short, professional and helpful answers.

If user asks unrelated questions, say:
'I am designed only for the Resume Analyzer project.'"
    },

    // ✅ USER MESSAGE
    new
    {
        role = "user",
        content = message
    }
}
                };

                var json = JsonConvert.SerializeObject(body);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                // ✅ API Call
                var response = await client.PostAsync(
                    "https://openrouter.ai/api/v1/chat/completions",
                    content
                );

                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine(result);

                if (!response.IsSuccessStatusCode)
                {
                    return "API Error: " + result;
                }

                dynamic data = JsonConvert.DeserializeObject(result);

                return data.choices[0].message.content.ToString();
            }
        }
    }
}