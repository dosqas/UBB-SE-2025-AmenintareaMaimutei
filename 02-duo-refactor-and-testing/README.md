# Amenintarea Maimutei - Software Engineering Team (UBB-SE-2025)

## 📱 Duolingo for Other Things

Welcome to the **Amenintarea Maimutei** team repository! This project is being developed as part of the **Software Engineering Course 2024-2025** at UBB, Group 927, Team #4. Our team has taken over and is continuing the work on the **Duolingo for Other Things** app, originally developed by **team OnlyFeatures** (Group 923). The app is built using **C# + .NET with WinUI** for the frontend and **SQL Server** for the database, designed to help users learn new skills in a fun, interactive way.

You can find the original repository [here](https://github.com/alexzmmv/UBB-SEE-2025-OnlyFeatures).

The next stage of development, which involves half-merging the code, can be found in the updated repository at [this link](https://github.com/dosqas/UBB-SE-2025-927-1).

---

## 📌 Project Overview  
Our team is currently tasked with **refactoring the code** and writing **unit and integration tests** for the **Duolingo for Other Things** app. This involves:  
- **Code Refactoring**: Improving the structure and readability of the codebase to ensure better maintainability and scalability. We are using **StyleCop** to enforce coding standards and maintain consistency across the codebase.  
- **Testing**: Writing comprehensive **Osherove-style mocked unit tests** to ensure proper isolation and behavior of individual components. These tests are crucial for validating the app's functionality and reliability. Additionally, we are writing **integration tests** to verify the interactions between different parts of the system.

Our work on these tasks is crucial for preparing the app for its final integration and smooth operation. After completing this phase, the project will be ready for further development and final integration into the complete system.

---

## 🚀 Features  

### 🔹 **Admin Features**  
- **Authentication & Access**:  
  - Admins can securely log in and log out of the system.  
- **Course Management**:  
  - Admins can create, modify, or remove courses.  
  - Define each course as free or premium, and set the price for premium courses.  
  - Assign difficulty levels to courses.  
  - Configure the timer duration for each course and set extra coin rewards for completing courses within the time limit.  
  - Assign topics to courses; there is no fixed limit to the number of topics per course.  
- **Module Management**:  
  - Admins can set the number of modules per course.  
  - Establish the sequential order in which modules must be completed.  
  - Optionally designate one module as a bonus module and define its coin unlock cost.  
- **Coin Rewards Configuration**:  
  - Admins set coin rewards for course completion, daily application start, and module image interactions.  
  - Designate which images within modules provide one-time coin rewards.  
  - Define the extra coin reward for completing a course within the set timer.  
- **Search and Filter Parameters**:  
  - Admins define all underlying criteria (course titles, topics) to ensure effective searching and filtering.  
- **Security and Integrity**:  
  - Admins control all parameters related to coin transactions, course enrollment, module order, and reward distributions.

### 🔹 **User Features**  
- **Course Enrollment**:  
  - Users can enroll in both free and premium courses. Premium courses require users to spend coins to enroll.  
- **Module Completion**:  
  - Users can mark modules as completed once they've reviewed them. A course is completed when all modules (except bonus modules) are completed.  
- **Progress Tracking**:  
  - Users can track their progress through each course based on the modules they’ve completed.  
- **Search & Filter**:  
  - Users can search for courses by title and apply filters based on enrollment status, course type, or topics associated with the course.  
  - Multiple filters can be applied at once to narrow down course selection.  
  - The search functionality allows users to search for courses using substring matching.  

### 🔹 **Reward System**  
- **Coin Economy**:  
  - Users earn coins for completing courses, starting the app daily, interacting with images, and finishing courses within the timer duration.  
- **Bonus Rewards**:  
  - Extra coin rewards are given for finishing a course within the set time limit.  
  - Users can also earn coins by interacting with specific images within modules.  

---

## 🛠️ Tech Stack  
- **Frontend**: WinUI  
- **Backend**: .NET (C#)  
- **Database**: SQL Server  

---

## 👥 Team Members  
- [Suba Dániel](https://github.com/danisuba10)  
- [Szarics Iulia](https://github.com/iuliaszarics)   
- [Șerban Dragoș](https://github.com/dragos06)
- [Șoptelea Sebastian](https://github.com/dosqas) 
- [Tanasă Ștefan-Alexandru](https://github.com/Jevan2004) 
- [Terebenț Roxana](https://github.com/TereRoxy)   
- [Újfalusi Ábel](https://github.com/UjfalusiAbel)

---

## 📅 Development Process  

1. **Design and Development Phase**: The initial design and development of the application were completed by **team OnlyFeatures** (group 923). They created the **use case diagrams, class diagrams**, and modeled the requirements, all of which can be found in the **design** folder.

2. **Project Handover**: Our team has received the project and is now focusing on **unit and integration tests**, **code refactoring**, and applying **further refinements** to ensure the application is ready for final integration into the **Duolingo for Other Things** app.

3. **Final Integration**: After completing the refinements and tests, the project will be integrated into the larger **Duolingo for Other Things** app.

---

🎯 Thank you for following our journey! We're excited to continue enhancing the **Duolingo for Other Things** app and bring you a more seamless experience. Stay tuned for more updates!
