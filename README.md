# AI Resume Analyzer

An intelligent web-based Resume Analysis System developed using **ASP.NET Core MVC**, **MySQL**, and **AI-based Resume Matching Techniques**.
The system helps recruiters automatically analyze resumes, extract skills, compare them with job requirements, and identify top candidates efficiently.

---

# 📌 Project Overview

The **AI Resume Analyzer** automates the manual resume screening process by using:

* Resume Parsing
* Skill Extraction
* Match Percentage Calculation
* AI-based Suggestions
* Candidate Ranking
* Admin Dashboard
* AI Chatbot Support

The system supports PDF and DOCX resume uploads and compares candidate skills with job descriptions to generate accurate match scores.

---

# 🚀 Features

## 👤 User Module

* User Registration & Login
* Upload Resume (PDF/DOCX)
* Resume Analysis
* Skill Extraction
* Match Percentage Calculation
* Resume History
* AI Suggestions for Improvement

## 🛠️ Admin Module

* Admin Login
* Manage Jobs
* View Uploaded Resumes
* Manage Candidates
* View Top Candidates
* Candidate Score Analysis

## 🤖 AI Chatbot

* Project-specific chatbot
* Answers queries related to:

  * Resume upload
  * Match calculation
  * Skills
  * Project workflow
  * Top candidates

---

# 🧠 Technologies Used

| Technology          | Purpose                   |
| ------------------- | ------------------------- |
| ASP.NET Core MVC    | Web Application Framework |
| C#                  | Backend Programming       |
| MySQL               | Database                  |
| Bootstrap 5         | UI Design                 |
| iText7              | PDF Parsing               |
| DocX                | DOCX Parsing              |
| OpenRouter API      | AI Chatbot                |
| HTML/CSS/JavaScript | Frontend                  |

---

# 📂 Project Structure

```plaintext
ResumeAnalyzer/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── ResumeController.cs
│   └── HomeController.cs
│
├── Models/
│
├── Services/
│   ├── ResumeParserService.cs
│   ├── MatchingService.cs
│   └── ChatbotService.cs
│
├── Views/
│
├── wwwroot/
│
└── appsettings.json
```

---

# ⚙️ System Workflow

1. User registers/login
2. Uploads resume
3. System extracts text from resume
4. Skills are identified
5. Skills are compared with job requirements
6. Match percentage is calculated
7. Suggestions are generated
8. Admin views top candidates

---

# 🧮 Match Percentage Logic

The system compares:

* Resume skills
* Job-required skills

### Formula Used

```plaintext
Match % = (Matched Skills / Total Job Skills) × 100
```

### Example

Job Skills:

```plaintext
C#, ASP.NET, SQL, Bootstrap
```

Resume Skills:

```plaintext
C#, SQL, HTML
```

Matched Skills:

```plaintext
C#, SQL
```

Calculation:

```plaintext
(2 / 4) × 100 = 50%
```

---

# 🗄️ Database Tables

## Users Table

| Column   | Type    |
| -------- | ------- |
| user_id  | int     |
| name     | varchar |
| email    | varchar |
| password | varchar |

---

## Jobs Table

| Column | Type    |
| ------ | ------- |
| job_id | int     |
| title  | varchar |
| skills | text    |

---

## Results Table

| Column           | Type |
| ---------------- | ---- |
| result_id        | int  |
| user_id          | int  |
| match_percentage | int  |
| suggestions      | text |

---

# 🤖 AI Chatbot Integration

The chatbot uses:

* OpenRouter API
* GPT-based conversational AI

### Capabilities

* Resume guidance
* Project explanation
* Skill suggestions
* Candidate analysis help

---

# 🔐 Security Features

* Session-based authentication
* Admin authorization
* File validation
* Secure database connectivity

---

# 🎯 Advantages

* Reduces manual HR work
* Fast resume screening
* Accurate candidate ranking
* Smart skill analysis
* Easy-to-use interface

---

# 📈 Future Enhancements

* Real AI/ML Resume Scoring
* NLP-based Skill Detection
* Email Notifications
* Video Interview Analysis
* Cloud Deployment
* ATS Integration

---

# 🖥️ Screens Included

* Home Page
* Login/Register
* Resume Upload
* Analysis Result
* Admin Dashboard
* Top Candidates
* AI Chatbot

---

# ▶️ How to Run Project

## 1️⃣ Clone Repository

```bash
git clone <repository-link>
```

## 2️⃣ Open in Visual Studio

Open:

```plaintext
ResumeAnalyzer.sln
```

---

## 3️⃣ Configure Database

Update:

```json
appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=resumeanalyzer;uid=root;pwd=;"
}
```

---

## 4️⃣ Install Packages

```bash
Install-Package MySql.Data
Install-Package Newtonsoft.Json
Install-Package Xceed.Words.NET
Install-Package itext7
```

---

## 5️⃣ Run Project

Press:

```plaintext
Ctrl + F5
```

---

# 📚 Learning Outcomes

This project demonstrates:

* ASP.NET MVC Architecture
* Database Integration
* Resume Parsing
* AI Chatbot Integration
* Skill Matching Algorithms
* Web Application Development

---

# 👩‍💻 Developed By

**Saishwari Korade**
AI Resume Analyzer Project
Deep Learning / AI Based Final Year Project

---

# 📄 License

This project is developed for educational and academic purposes.
