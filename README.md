# Campus Connect - Team 404

## Introduction
Campus Connect, by Team 404, is a collaborative platform for students where they can share study materials, join groups/communities, schedule events, and post questions, ideas, or tips. The project aims to provide a centralized hub for students to collaborate, learn, and engage with one another. 

This project was created as part of a hackathon, with the goal of rapidly developing a large project using advanced architectures like **MVC**, **Entity Framework**, **Dependency Injection**, and hosting it online. Additionally, this project is intended to enhance our portfolios and CVs.

---

## Team Members & Roles
- **Kabelo Ntokozo Will Mndebele** – Main UI Developer, assisting Backend Developer, responsible for hosting the application.  
- **Joshua Ponquett** – Main Backend Developer, Documentation Specialist, responsible for GitHub setup.

---
## Technologies used:

- Visual studio
- Entity Framework
- SQlite
- Render (for hosting)
## Functional Requirements
1. **Login / Sign-Up**: Users can create an account and log in securely.  
2. **Profile Creation**: Users can customize their profiles with degree, skills, sports, profile photo, and other relevant information.  
3. **Posting Questions / Ideas / Tips**: Students can post questions, ideas, or tips and receive comments from other users.  
4. **Joining Groups or Communities**: Students can create or join groups/communities (admin approval required). Each group/community has:
   - Announcements section
   - Dedicated messaging page for discussion  
5. **Sharing Study Materials**: Students, lecturers, or admins can share study materials either through groups/communities or a dedicated sharing page.  
6. **Reporting Study Materials**: Users can flag inappropriate materials for admin review and potential deletion.  
7. **Scheduling Meetings / Events**: Students can RSVP to events, with the ability to remove RSVPs. Attendance is restricted after a certain date.

---

## Non-Functional Requirements
- **Availability**: Hosted online and accessible 24/7.  
- **Compatibility**: Responsive design for both mobile and desktop.  
- **Maintainability**: Built with **MVC**, **Entity Framework**, and **Dependency Injection** to simplify updates and maintenance.  
- **Scalability**: Currently limited due to SQLite; can be upgraded to cloud SQL/NoSQL servers with minimal code changes.  
- **Security**: Passwords are hashed, and authentication/authorization restricts unauthorized access to user and admin pages.

---

## Database
- **ERD (Entity Relationship Diagram)**:  
[View ERD](https://drive.google.com/file/d/1WInvtsE2RU9FikPB8F0FtBKo3DV_mGH2/view)  
- **Considerations**:
  - SQLite supports many reads simultaneously but limits concurrent inserts. For production, a queue system or cloud database is recommended.
  - Only group/community owners can post announcements. Expanding this would require modifying the database schema.  
  - Comments on posts are not currently implemented in the ERD.

---

## Project Architecture
- **MVC (Model-View-Controller)** for structured application design.  
- **Entity Framework** for database management.  
- **Dependency Injection** using repository + interface pattern for maintainable and testable code.  

---

## Getting Started
1. Clone the repository:
```bash
git clone https://github.com/VC-ST10405508/Team404_CampusConnect_Hackathon.git
