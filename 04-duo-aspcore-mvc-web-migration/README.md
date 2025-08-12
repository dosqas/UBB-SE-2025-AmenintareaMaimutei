# Subgroup 927/1 - Software Engineering Team (UBB-SE-2025)

## 📱 Duolingo for Other Things

Welcome to the **Subgroup 927/1** team repository! This project is being developed as part of the **Software Engineering Course 2024-2025** at UBB by subgroup 927/1. Our teams have taken over and are continuing the work on the **Duolingo for Other Things** app, originally developed by subgroup 923/2. The app is built using **C# + .NET with WinUI** for the frontend, **Entity Framework** for database operations, and **SQL Server** as the database, designed to help users learn new skills in a fun, interactive way.

You can find the original team repositories here:
- [Amenintarea Maimutei](https://github.com/dosqas/UBB-SE-2025-AmenintareaMaimutei-NewBase)
- [NewFolder](https://github.com/vodaioan03/UBB-SE-2025-League-Right)

---

## 📌 Project Overview  

Our team is currently tasked with **merging our solutions** for the **Duolingo for Other Things** app. This involves:

- **Solution Merging**: We are in the process of **half-merging** our solutions with another team, combining both codebases into a unified structure. The final merge will follow soon, but our current focus is on making sure the architecture is aligned. The structure must be consistent across all classes, ensuring that each component has a defined role. For example, if one team used a model as a service, while the other used a separate service, we will ensure there is a unified approach with separate services for all features.

- **Refactoring & Testing**: We are refactoring the codebase to improve maintainability and readability. During this phase, we are making sure that no business logic resides in the UI or data access layer, and that the **MVVM pattern** is respected. We are also writing comprehensive **Osherove-style mocked unit tests** to ensure proper isolation of components and **integration tests** to verify interactions between different system parts.

- **Entity Framework Core**: We are transitioning the data access layer to use **Entity Framework Core** with **Code First** and migrations to manage the database. This will allow us to maintain a single database across the entire project, eliminating the need for multiple databases. Raw SQL and stored procedures will no longer be used, and any file saving/loading will be handled outside of domain logic.

- **Web API Setup**: Our solution now includes an **ASP.NET Core Web API** project, which will be hosted separately from the main application. This API will serve as the communication layer for data access, and each team member will run the API locally. The main application will interact with this Web API instead of connecting directly to the database.

- **Code Quality & Standards**: We will continue to follow **StyleCop** rules for code readability, testability, and maintainability. The goal is to ensure that the code is clean, well-documented, and adheres to consistent formatting rules. All team members are expected to adhere to these guidelines, and code should be tested with appropriate unit and integration tests.

Our work on these tasks is crucial for preparing the app for its final integration and ensuring it operates smoothly. After completing this phase, the project will be ready for further development and final integration into the complete system.

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
- **Frontend**: WinUI 3  
- **Backend**: .NET (C#)  
- **Database**: SQL Server  
- **ORM**: Entity Framework Core  

---

## 📅 Development Process  

1. **Design and Initial Implementation**: The initial design, requirement modeling and initial implementations were developed by subgroup 923/2, laying the groundwork for the app’s core features.

2. **Project Handover and Merge Phase**: Our subgroup has received the projects and is currently focused on merging them. We are in the process of integrating the different solutions by refactoring the code, applying unit and integration tests, and making necessary adjustments to align the projects. This phase includes switching to **Entity Framework Core** for data access and implementing a unified API. 

3. **Final Integration**: Once the ongoing half-merge and refinements are completed, the project will be ready for final integration into the **Duolingo for Other Things** app. The final merge and polish will ensure the app is fully cohesive and ready for deployment.

---

🎯 Thank you for following our journey! We're excited to continue enhancing the **Duolingo for Other Things** app and bring you a more seamless experience. Stay tuned for more updates!
