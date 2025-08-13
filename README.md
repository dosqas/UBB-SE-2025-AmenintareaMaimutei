# 🎯 Hospital & Duo Apps

## 🎉 Introduction

This repository contains team projects developed for the **Software Engineering course (2nd semester, 2nd year)**. Over the semester, we worked in two main iterations to strengthen our programming and software design skills.

---

## 🔹 First Iteration – Design and Initial Hospital App

In the **first iteration**, our team of **6 students** wrote the project requirements, designed the use case diagram and class diagram, and created tasks for the development process. We then built a **Hospital Management app** using **WinUI 3** with the **MVVM UI pattern**, and **SQL Server** for data storage.

## ⚙️ Requirements & Features

### 🔧 Admin Features

* **Authentication & Access:** Login/logout functionality for admin users.
* **Hospital Data Management:** Manage doctor accounts, equipment, drugs, departments, rooms, shifts, and schedules. Validate data inputs such as doctor credentials and room capacities.
* **Shifts & Salaries:** Assign shifts and calculate doctor salaries based on schedules.
* **Full CRUD Operations:** Create, Read, Update, Delete functionalities for hospital records.

### 🔧 Patient Features

* **Submit Feedback:** Patients can write reviews (5–255 characters) and give star ratings (1–5). Input is validated to ensure proper formatting.

---

## 🔹 Second Iteration – Project Switch

After the first iteration, we **switched projects**: we handed over our Hospital app to another team and received their **Duolingo-style learning app**. This iteration focused on:

* **Code Refactoring:** Cleaning the code to remove business logic from the GUI.
* **Interface Usage:** Implementing interfaces to improve flexibility and maintainability.
* **Unit Testing:** Writing **Osherove-style mocked unit tests** with **100% coverage**.
* **Code Style Enforcement:** Using **StyleCop** to maintain consistent coding style throughout the project.

---

## 🔹 Third Iteration – First Merge

During the **third iteration**, our team grew to **13 members**. We established **tech and team leads**, and the **tech lead defined a coding style** to ensure consistency across the project. Key achievements in this phase included:

* **Code Homogenization:** Merged individual projects and standardized the folder structure and code organization.
* **Entity Framework Core:** Began using EF Core for database access.
* **API Setup:** Moved the repository to a **separate ASP.NET Core Web API project** running independently from the main application.

---

## 🔹 Fourth Iteration – ASP.NET Core MVC Migration

In the **fourth iteration**, we migrated our solution to **ASP.NET Core MVC**, introducing a **new web application**:

* **Class Library for Services:** Moved the service layer into a class library, connecting to the API through existing proxies.
* **Dependency Injection:** Integrated a DI framework for managing object instantiations across the project.
* **Sequence Diagrams:** Created sequence diagrams for key functionalities to document workflows and interactions.

This iteration focused on **refactoring the GUI**, separating concerns, and preparing the app for large-scale integration.

---

## 🔹 Final Iteration – Full Project Merge

In the **final iteration**, our team expanded to **28 members**, including:

* **Project Manager**
* **QA Lead**
* **Desktop Tech Lead**
* **Web Tech Lead**

Key accomplishments of this phase:

* **Single Solution Merge:** Consolidated all code into one unified solution.
* **Homogenized Structure:** Standardized project structure and coding style across the entire application.
* **Final Integration:** Ensured both desktop and web applications work seamlessly together with the shared service layer and API.

This final phase marks the completion of our **Hospital & Duo apps project**, showcasing collaborative development, software engineering best practices, and modern C#/.NET technologies.

---

## 🛠️ Tech Stack

* **Languages:** C#
* **Desktop UI:** WinUI 3 (MVVM)
* **Web UI:** ASP.NET Core MVC
* **API:** ASP.NET Core Web API
* **Database:** SQL Server, Entity Framework Core
* **Dependency Injection:** Microsoft.Extensions.DependencyInjection
* **Testing:** MSTest / xUnit (Osherove-style unit tests)
* **Code Style:** StyleCop
* **Version Control & Collaboration:** Git, GitHub

---

## 🏗️ Project Architecture

* **Desktop App:** MVVM pattern, communicates with Web API via proxies
* **Web App:** ASP.NET Core MVC, service layer in a class library
* **Web API:** Handles data access and business logic using EF Core
* **Database:** SQL Server for persistence
* **Dependency Injection:** Used to manage services across layers

---

## 🧩 Challenges & Lessons Learned
* **Large Team Collaboration:** Handling code merges with up to 28 members
* **Code Refactoring & Clean Architecture:** Moving business logic out of GUI, applying interfaces, enforcing coding style
* **Unit Testing Practices:** Writing mocked tests with 100% coverage
* **Design Patterns:** Applying MVVM, using DI, and structuring layered architecture

---

## Acknowledgements

* Special thanks to **Imre Zsigmond** and **Bogdan-Ionuț Flueras** for their guidance, support, and feedback throughout the course.
* Huge appreciation to all my **teammates**. It was a pleasure working with you, even though we faced challenges at times, especially as this was our first experience on a team project.
* Grateful for the collaborative spirit and learning opportunities that made this project both educational and enjoyable.

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## 💡 Contact

Questions, feedback, or ideas? Reach out anytime at [sebastian.soptelea@proton.me](mailto:sebastian.soptelea@proton.me).
