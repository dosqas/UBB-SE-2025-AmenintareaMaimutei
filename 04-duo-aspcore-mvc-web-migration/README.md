# Subgroup 927/1 - Software Engineering Team (UBB-SE-2025)

## 📱 Duolingo for Other Things

Welcome to the **Subgroup 927/1** team repository! This project is being developed as part of the **Software Engineering Course 2024-2025** at UBB by subgroup 927/1. Our teams have taken over and are continuing the work on the **Duolingo for Other Things** app, originally developed by subgroup 923/2. The app is built using **C# + .NET with WinUI/ASP.NET Core MVC** for the frontend, **Entity Framework** for database operations, and **SQL Server** as the database, designed to help users learn new skills in a fun, interactive way.

You can find the other team's original repository here:
- [NewFolder](https://github.com/vodaioan03/UBB-SE-2025-League-Right)

---

**Latest Update**: We're migrating our WinUI desktop application to ASP.NET Core MVC while maintaining all existing functionality. Below are the key architectural changes:

## 🚀 Current Migration Work

### 🔧 Architectural Changes
- **New ASP.NET Core MVC Web App** (Model-View-Controller) replacing WinUI frontend
- **Service Layer Integration**:
  - Shared service layer between desktop and web via class library
  - API now exposes services instead of raw repositories
  - Service proxy pattern implemented for cross-platform consistency

### 🔒 Security Enhancements
- All routes now require authentication
- Role-based authorization for admin/user features
- JWT token validation for API calls

### 🛠️ Technical Improvements
- **Dependency Injection**:
  - Full DI implementation across all layers
  - Auto-wiring of services and repositories
- **Caching Strategy**:
  - Memory caching for frequent operations
  - Automatic cache invalidation on data changes
- **Client-Side Stack**:
  - Standard Bootstrap/jQuery foundation
  - Additional frameworks pending team approval

### 📊 Progress Tracking
- CRUD operations generated for 2 core entities (Courses, Modules)
- Sequence diagrams created for premium course enrollment flow
- Deployment pipeline configured for seminar #6 demo

---

## 🚀 Features

### 🔹 **Admin Features**  
- **Authentication & Access**:  
  - Admins can securely log in and log out.  
- **Course Management**:  
  - Create, modify, or remove courses.  
  - Set courses as free or premium with prices for premium ones.  
  - Assign difficulty levels and timer durations, with extra coin rewards for time-based completion.  
  - Assign topics to courses with no limit on number.  
- **Module Management**:  
  - Set the number of modules per course and define their order.  
  - Designate a bonus module and set a coin unlock cost.  
- **Coin Rewards Configuration**:  
  - Define coin rewards for course completion, daily app starts, module interactions, and timer-based completion.  
- **Search & Filters**:  
  - Admins can define search parameters based on titles, topics, and other course criteria.  
- **Security & Integrity**:  
  - Control coin transactions, course enrollments, module order, and reward distributions.  

### 🔹 **User Features**  
- **Course Enrollment**:  
  - Enroll in free or premium courses; premium courses require coins.  
- **Module Completion**:  
  - Mark modules as completed once reviewed. A course is completed when all modules (except bonus ones) are finished.  
- **Progress Tracking**:  
  - Track progress through courses based on completed modules.  
- **Search & Filters**:  
  - Search and filter courses by title, enrollment status, type, and topics.  
  - Multiple filters can be applied at once for refined search results.  

### 🔹 **Reward System**  
- **Coin Economy**:  
  - Earn coins for course completion, daily app starts, image interactions, and finishing courses within the timer.  
- **Bonus Rewards**:  
  - Extra coins for completing courses within the time limit and interacting with specific images.  

### 🔹 **Exercise Types**  
- **Multiple Choice**:  
  - Test knowledge with multiple-choice questions.  
- **Fill in the Blanks**:  
  - Practice sentence completion with contextual clues.  
- **Association Exercises**:  
  - Match related items to reinforce learning.  
- **Flashcards**:  
  - Timed flashcards with difficulty-based timers (Easy: 15s, Normal: 30s, Hard: 45s) and immediate feedback.  

### 🔹 **Learning Roadmap**  
- **Structured Learning Paths**:  
  - Organized by difficulty levels (Easy, Normal, Hard) with section-based progress tracking.  
  - Quiz previews and completion tracking.  

### 🔹 **User Interface**  
- **WinUI 3 Design**:  
  - Modern, responsive layout with visual feedback and progress indicators.  
  - Difficulty-based styling for a tailored user experience.  
  - Accessibility support for all users.

---

## 🛠️ Tech Stack  
- **Frontend**: WinUI 3 (Desktop) / ASP.NET Core MVC (Web)
- **Backend**: .NET (C#)  
- **Database**: SQL Server  
- **ORM**: Entity Framework Core  

---

## 📅 Development Process

1. **Migration to ASP.NET Core MVC**:
   We are migrating our existing solution to **ASP.NET Core MVC** (Model–View–Controller). This migration only affects the GUI, keeping the core business logic intact. The new MVC project will be added alongside the existing solution.

2. **Architecture Options & Integration**:
   For integrating with our existing API and service layer, we will use the following architecture strategy: **Class Library Sharing** - We move the service and proxy repository to a class library referenced by both the desktop and web apps.

3. **UI & Security Enhancements**:
   * All functionalities will be protected against unauthorized access.
   * A **dependency injection** framework will handle all object instantiations.

---

🎯 Thank you for following our journey! We're excited to continue enhancing the **Duolingo for Other Things** app and bring you a more seamless experience. Stay tuned for more updates!
