# Coverage Exclusions

This file documents the projects, classes and/or methods that have been intentionally excluded from code coverage.

We are using the `[ExcludeFromCodeCoverage]` attribute to mark classes or methods that are either:

- Not relevant for testing (e.g., purely UI logic with no meaningful backend interaction),
- Not feasible to test in a meaningful way (e.g., tightly coupled to platform-specific behavior),
- Automatically generated or boilerplate code (e.g., designer files).

This helps keep our coverage metrics meaningful and focused on testable, logic-driven code.

## Examples of Excluded Code

- **UI classes or Windows Forms / WPF / XAML-backed code**  
  These are typically excluded due to their dependency on visual components and event-based behavior.

- **App entry points or auto-generated methods**

- **Helpers or wrappers that just forward calls without logic**

All such exclusions are annotated directly in the code with `[ExcludeFromCodeCoverage]`.

Method-level exclusion is supported and used, and it follows the same rationale.

---

## List of Excluded Projects

| Project Name | Reason for Exclusion                                                                   |
|--------------|----------------------------------------------------------------------------------------|
| `Tests`      | This project contains only unit tests and does not include any production code logic.  |

## List of Excluded Classes

| Class Name                           | Reason for Exclusion                                                                                   |
|--------------------------------------|--------------------------------------------------------------------------------------------------------|
| `RealDispatcherTimer`                | Wrapper around `DispatcherTimer` from UWP/WinUI which cannot be properly mocked or tested. This class is tightly coupled to the Windows Runtime (WinRT), and leads to COM exceptions during testing. It provides no additional logic beyond forwarding calls. |
| `App`								   | The main entry point for the application. It is auto-generated and contains no logic that can be tested. |
| `MainPage`                           | The main page of the application. It is auto-generated and contains no logic that can be tested. |
| `MainWindow`						   | This is the root window of the application, mostly hosting UI elements and binding to view models. Like other UI components, it is tightly coupled to the visual layer and does not contain logic suitable for unit testing. |
| `CoursePage`						   | This page is tightly coupled to the WinUI framework, handling navigation, UI events, and data binding logic. It does not contain any logic that can be meaningfully unit tested without a UI testing framework. |
| `ModulePage`						   | Similar to CoursePage, ModulePage is responsible for rendering UI and initializing its view model based on navigation parameters. It is heavily dependent on the WinUI framework for page lifecycle and has no logic that is appropriate for unit testing. |
| `DataLink`						   | Provides a static method to create a SqlConnection using a hardcoded connection string. Contains no business logic and is tightly coupled to platform-specific and environment-specific configurations.|
| `CourseModelView`					   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `EnrollmentModelView`				   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `ModuleModelView`					   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `ProgressModelView`				   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `RewardModelView`					   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `TagModelView`					   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `UserWalletModelView` 			   | This class tightly couples SQL logic and database connections, making it unsuitable for unit testing. |
| `FakeCoinsRepository`				   | This class is a mock implementation of a repository for testing purposes. Is not intended for production use. |
| `AvailabilityColorConverter`		   | This class only maps a boolean to a visual brush (SolidColorBrush). This is not business logic and is not suitable for unit testing. |
| `CoinsRepository`					   | This class is a wrapper around the UserWalletModelView class and provides no additional logic. It is tightly coupled to the SQL database and cannot be tested in isolation. |
| `CourseRepository`				   | This class is a wrapper around the CourseModelView, ModuleModelView, EnrollmentModelView, TagModelView, ProgressModelView, RewardModelView classess and provides no additional logic. It is tightly coupled to the SQL database and cannot be tested in isolation. |
| `Enrollment`                         | The Enrollment class is a simple data model representing user course enrollments and doesn't contain business logic, so it doesn't require unit tests at this stage.|
| `CourseCompletion` 				   | The CourseCompletion class is a simple data model representing course completion status and doesn't contain business logic, so it doesn't require unit tests at this stage.|
| `User`							   | The User class is a simple data model representing user information and doesn't contain business logic, so it doesn't require unit tests at this stage.|
| `Course`                             | The Course class is a data model representing comprehensive course details including metadata, pricing, and completion metrics. While containing multiple properties, it remains a simple DTO without business logic, so it doesn't require unit tests at this stage. |

## List of Excluded Methods

| Method Name                              | Reason for Exclusion                                                                                   |
|------------------------------------------|--------------------------------------------------------------------------------------------------------|
| `CourseViewModel.InitializeTimersAndNotificationHelper`  | **Cannot test when timers are null in the constructor.** The method creates new instances of `DispatcherTimerService`, which contains a `RealDispatcherTimer`. Since `RealDispatcherTimer` wraps around a platform-specific `DispatcherTimer`, which we cannot mock due to its reliance on the Windows Runtime (WinRT), it leads to COM exceptions during testing. |
| `DispatcherTimerService.InitializeTimer` | This method creates a `RealDispatcherTimer`, which contains a `DispatcherTimer` from UWP/WinUI. This timer cannot be mocked in tests due to its dependency on WinRT, making the null branch untestable. It contains no business logic.          |

---

This file serves as a reference for what was excluded and why, especially during evaluation.
