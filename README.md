## AI-Based Exam Proctoring System (Admin Side)
# Overview
This repository contains the admin side of an AI-Based Exam Proctoring System, designed to manage and monitor online examinations with enhanced security and anti-cheating measures. The admin side is built using .NET for the backend and SQL Server for the database, providing a robust interface for administrators to oversee exam sessions, manage users, and review cheating detection reports.
The desktop application, which handles client-side exam monitoring (including video, audio, and AI-based detection), is implemented in Python using libraries such as OpenCV, Dlib, and PyQt5. It is hosted in a separate repository (link to be added once available).
# Features

- Admin Dashboard: User-friendly interface for managing exams, students, and proctoring data.
- User Management: Create, update, and delete student and admin accounts.
- Exam Management: Schedule exams, assign students, and configure proctoring settings.
- Cheating Detection Reports: View detailed reports of detected cheating incidents, including face recognition, gaze tracking, head movement, object detection, and audio violations.
- Real-Time Monitoring: Receive real-time data from the desktop application regarding exam violations.
- Secure Authentication: Secure login system for administrators using email and password.
- Database Integration: SQL Server database for storing user data, exam details, and violation logs.

# Technology Stack

Backend: .NET (ASP.NET Core for API development)
Database: SQL Server
Frontend: (Specify your frontend technology, e.g., Razor Pages, Blazor, or a JavaScript framework like Angular/React)
AI Integration: Communicates with the desktop application for AI-based proctoring (face recognition, gaze detection, etc.)
APIs: RESTful APIs for communication between the admin side and the desktop application.

# Prerequisites
To set up and run the admin side of the project, ensure you have the following installed:

(1) .NET SDK (version 6.0 or later, depending on the project configuration)
(2) SQL Server (e.g., SQL Server Express 2019 or later)
(3) Visual Studio or another IDE compatible with .NET development

# Installation

(1) Clone the Repository:
- git clone https://github.com/FatmaAlaa28/AI-Based-Exam-Proctoring-System.git
- cd AI-Based-Exam-Proctoring-System

(2) Set Up the Database:
- Create a new SQL Server database.
- Update the connection string in the appsettings.json file to point to your SQL Server instance.
- Run the database migrations to set up the schema:
- dotnet ef database update

(3) Install Dependencies:
- Restore the .NET packages:
dotnet restore

(4) Run the Application:
- Build and run the project:
dotnet run
The application will be accessible at http://localhost:5000 (or the port specified in your configuration).

# Configuration
appsettings.json: Configure the SQL Server connection string, API endpoints, and other settings.
Environment Variables: Set up any required environment variables for sensitive data (e.g., API keys, admin credentials).
CORS Settings: If the desktop application communicates with the admin side, ensure CORS is configured to allow requests from the desktop app's server URL.

# Desktop Application (Student Side)
The client-side desktop application, which performs real-time exam monitoring using AI, is implemented in Python and is hosted in a separate repository. This application uses:

- OpenCV and Dlib for face detection and recognition.
- MTCNN for advanced face detection.
- PyQt5 for the user interface.
- TensorFlow for machine learning models.
- PyAudio for audio monitoring.

=> To integrate the desktop application with this admin side:

Ensure the desktop application is configured to send cheating detection data to the admin API endpoints (e.g., /api/detection/cheating).
Update the server URL in the desktop application to point to the admin side's API.

Desktop Repository:(https://github.com/FatmaAlaa28/Exam-Proctoring-Desktop-Application).
API Endpoints
The admin side provides RESTful APIs for communication with the desktop application. Key endpoints include:

POST /api/auth/login: Authenticate users and return exam links.
POST /api/detection/cheating: Receive cheating detection data from the desktop application.
(Add other relevant endpoints as implemented in your .NET project)

# Usage

- Admin Login: Log in using admin credentials to access the dashboard.
- Exam Setup: Create and configure exams, assign students, and set proctoring rules.
- Monitoring: During an exam, the desktop application sends real-time cheating detection data to the admin side.
- Review Reports: View and analyze cheating reports, including timestamps, confidence scores, and violation types.

# Contributing
Contributions are welcome! To contribute:
(1) Fork the repository.
(2) Create a new branch:
git checkout -b feature/your-feature
(3) Make your changes and commit:
git commit -m "Add your feature"
(4) Push to the branch:
git push origin feature/your-feature
(5) Open a pull request.


# License
This project is licensed under the MIT License. See the LICENSE file for details.
# Contact
For questions or support, contact Fatma Alaa or open an issue on this repository.
