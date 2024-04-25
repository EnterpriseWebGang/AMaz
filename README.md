# AMaz - Article Magazine System

AMaz is a web-based Article Magazine System developed using .NET 6 MVC framework. It aims to facilitate the submission, review, and publication process of articles and images contributed by students in various faculties within a university setting. The system includes features to ensure smooth coordination between students, faculty marketing coordinators, and the university marketing manager.

## Key Features

- **User Roles**: 
  - Marketing Manager: Oversees the entire process.
  - Marketing Coordinator: Manages the process for their respective faculty.
  - Students: Contribute articles and images.
  - Administrator: Maintains system data.

- **Article Submission**:
  - Students can submit one or more articles as Word documents.
  - Ability to upload high-quality images.

- **Submission Management**:
  - Closure dates for new entries and final closure dates.
  - Terms and Conditions agreement required for submission.
  - Email notification to Faculty’s Marketing Coordinator upon submission.
  - 14-day window for Marketing Coordinators to comment on submissions.

- **Faculty Interaction**:
  - Marketing Coordinators can edit contributions and select articles for publication within their faculty.

- **University Marketing Manager Access**:
  - View all selected contributions.
  - Download selected contributions in a ZIP file after the final closure date.

- **Administrator Controls**:
  - Maintains system data, including closure dates for each academic year.

- **Guest Access**:
  - Guest accounts for each faculty to view selected reports.

- **Statistical Analysis**:
  - Provides statistical analysis, such as the number of contributions per faculty.

- **Responsive Interface**:
  - Suitable for all devices including mobile phones, tablets, and desktops.

## Getting Started

1. Ensure you have .NET 6 SDK installed. If not, you can download it from [here](https://dotnet.microsoft.com/download).
2. Clone the repository: `git clone https://github.com/EnterpriseWebGang/AMaz.git`
3. Open the solution in Visual Studio or your preferred IDE.
4. Set up the database according to the provided schema.
5. Update database connection strings in `appsettings.json`.
6. Run the application.

## Technologies Used

- .NET 6
- MVC Framework
- Entity Framework Core
- HTML/CSS/JavaScript
- Bootstrap (for responsive design)


## Acknowledgments

- Special thanks to [University Of Greenwich](https://greenwich.edu.vn/) for inspiration and requirements specification.
