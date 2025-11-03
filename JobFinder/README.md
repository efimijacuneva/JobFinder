# 🚀 JobFinder - Professional Job Search Platform

[![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20MVC-5.2.9-blue.svg)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.4.4-green.svg)](https://docs.microsoft.com/en-us/ef/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.2.3-purple.svg)](https://getbootstrap.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

> A modern, full-featured job search and recruitment platform built with ASP.NET MVC Framework, designed to connect talented professionals with innovative companies.

## 📋 Table of Contents

- [✨ Features](#-features)
- [🛠️ Technology Stack](#️-technology-stack)
- [🚀 Quick Start](#-quick-start)
- [📖 Usage Guide](#-usage-guide)
- [🏗️ Architecture](#️-architecture)
- [🔧 Configuration](#-configuration)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

## ✨ Features

### 👥 **For Job Seekers**
- 🔍 **Advanced Job Search** - Browse jobs with filters by location, category, and keywords
- 📝 **Easy Application Process** - Apply with CV upload (PDF, DOC, DOCX)
- 📊 **Application Tracking** - Monitor application status and company responses
- 💼 **Job Details** - Comprehensive job information with company details
- 🔖 **Application History** - View all your applications in one place

### 🏢 **For Companies**
- 📢 **Job Posting** - Create detailed job listings with rich descriptions
- 👥 **Applicant Management** - Review and manage candidate applications
- 📄 **CV Download** - Access candidate resumes and documents
- 💬 **Response System** - Respond to applications with status updates and messages
- 📈 **Analytics Dashboard** - Track application statistics and insights

### 🔐 **Security & User Management**
- 🔑 **Role-Based Authentication** - Separate interfaces for Companies and Candidates
- 👤 **User Profiles** - Comprehensive profile management
- 🔒 **Secure File Uploads** - Validated and secure CV uploads
- 🛡️ **CSRF Protection** - Built-in security measures

## 🛠️ Technology Stack

| Component | Technology | Version |
|-----------|------------|---------|
| **Framework** | ASP.NET MVC | 5.2.9 |
| **Database** | Entity Framework | 6.4.4 |
| **Authentication** | ASP.NET Identity | 2.2.4 |
| **UI Framework** | Bootstrap | 5.2.3 |
| **Icons** | Font Awesome | 6.0.0 |
| **Database** | SQL Server LocalDB | - |
| **Language** | C# | .NET Framework 4.7.2 |

## 🚀 Quick Start

### Prerequisites

- [Visual Studio 2019](https://visualstudio.microsoft.com/) or later
- [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework) or later
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/efimijacuneva/JobFinder.git
   cd JobFinder
   ```

2. **Open in Visual Studio**
   ```bash
   # Open JobFinder.sln in Visual Studio
   # Or use command line:
   dotnet restore
   ```

3. **Database Setup**
   ```bash
   # In Package Manager Console:
   Update-Database
   ```

4. **Run the Application**
   ```bash
   # Press F5 in Visual Studio
   # Or use command line:
   dotnet run
   ```

5. **Access the Application**
   ```
   http://localhost:port
   ```

## 📖 Usage Guide

### 🏢 **For Companies**

#### Creating Job Posts
1. Register/Login as a Company
2. Navigate to "Post New Job"
3. Fill in job details:
   - Job Title
   - Description
   - Location
   - Category
   - Requirements
4. Submit and publish your job

#### Managing Applications
1. Go to "My Job Posts"
2. Click "View Applicants" for any job
3. Review candidate applications
4. Download CV files
5. Respond to candidates with status updates

### 👤 **For Job Seekers**

#### Browsing Jobs
1. Register/Login as a Candidate
2. Use the search filters to find relevant positions
3. Click "View Details" for comprehensive job information
4. Apply with your CV/resume

#### Tracking Applications
1. Go to "My Applications"
2. View application status and company responses
3. Download your submitted CV files
4. Track response history

## 🏗️ Architecture

### Project Structure
```
JobFinder/
├── 📁 Controllers/           # MVC Controllers
│   ├── AccountController.cs
│   ├── ApplicationsController.cs
│   ├── CompanyController.cs
│   └── JobPostsController.cs
├── 📁 Models/               # Entity Framework Models
│   ├── AccountViewModels.cs
│   ├── IdentityModels.cs
│   ├── JobApplication.cs
│   └── JobPost.cs
├── 📁 Views/                # Razor Views
│   ├── Account/
│   ├── Applications/
│   ├── Company/
│   ├── JobPosts/
│   └── Shared/
├── 📁 Content/              # CSS and static files
├── 📁 Scripts/              # JavaScript files
├── 📁 App_Data/            # File uploads (CVs)
└── 📁 Migrations/          # Database migrations
```

### Database Schema

| Entity | Description | Key Fields |
|--------|-------------|------------|
| **ApplicationUser** | User accounts | Id, UserName, Email, Role |
| **JobPost** | Job listings | Id, Title, Description, CompanyId |
| **JobApplication** | Applications | Id, CandidateId, JobPostId, Status |
| **Feedback** | Rating system | Id, Rating, Comment |
| **SavedJob** | Bookmarked jobs | Id, UserId, JobPostId |

## 🔧 Configuration

### File Upload Settings
```csharp
// In Controllers/ApplicationsController.cs
private const int MaxFileSize = 5 * 1024 * 1024; // 5MB
private readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx" };
```

### Database Connection
```xml
<!-- In Web.config -->
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Data Source=(LocalDb)\MSSQLLocalDB;..." />
</connectionStrings>
```

### Customization Options

#### Adding Job Categories
Edit the dropdown in `Views/JobPosts/Create.cshtml`:
```html
<select name="Category" class="form-control">
    <option value="Technology">Technology</option>
    <option value="Healthcare">Healthcare</option>
    <!-- Add more categories -->
</select>
```

#### Modifying Styling
Update `Content/Site.css` for custom styling:
```css
/* Custom styles for JobFinder */
.profile-header {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}
```

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Commit your changes**
   ```bash
   git commit -m 'Add amazing feature'
   ```
4. **Push to the branch**
   ```bash
   git push origin feature/amazing-feature
   ```
5. **Open a Pull Request**

### Development Guidelines

- Follow C# coding conventions
- Add comments for complex logic
- Test thoroughly before submitting
- Update documentation as needed

## 🐛 Troubleshooting

### Common Issues

| Issue | Solution |
|-------|----------|
| **Database Connection Error** | Ensure SQL Server LocalDB is installed and run `Update-Database` |
| **File Upload Fails** | Check file size (max 5MB) and type (PDF, DOC, DOCX) |
| **Build Errors** | Restore NuGet packages and clean/rebuild solution |
| **Authentication Issues** | Clear browser cache and cookies |

### Debug Mode
```bash
# Enable detailed error messages in Web.config
<customErrors mode="Off"/>
```

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [ASP.NET MVC](https://dotnet.microsoft.com/apps/aspnet) for the web framework
- [Bootstrap](https://getbootstrap.com/) for the UI components
- [Font Awesome](https://fontawesome.com/) for the icons
- [Entity Framework](https://docs.microsoft.com/en-us/ef/) for data access

## 📞 Support

- **Documentation**: Check this README and inline code comments
- **Issues**: Create an issue in the repository
- **Questions**: Open a discussion in the repository

---

<div align="center">

**Made with ❤️ using ASP.NET MVC**

[![GitHub stars](https://img.shields.io/github/stars/efimijacuneva/JobFinder?style=social)](https://github.com/efimijacuneva/JobFinder)
[![GitHub forks](https://img.shields.io/github/forks/efimijacuneva/JobFinder?style=social)](https://github.com/efimijacuneva/JobFinder)

</div> 
