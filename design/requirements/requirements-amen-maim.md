# Hospital Management System

A comprehensive hospital management application with admin and patient functionalities.

## Features

### Admin Functionalities

#### Authentication
- **Login:** Admins can log in using username/password to access admin features
- **Logout:** Securely log out and return to login page

#### Data Management
Admins can perform CRUD operations on:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules and shifts
- Departments and rooms

##### Adding Records
- **Doctors:**
  - Must have valid UserID (existing user with Doctor role)
  - Valid DepartmentID
  - Non-negative experience
  - Non-empty license number
  - Rating auto-calculated (starts at 0)

- **Drugs/Equipment:**
  - Alphanumeric fields
  - Non-negative supply/stock

- **Shifts:**
  - Start/End times: 08:00 or 20:00

- **Departments/Rooms:**
  - Valid capacity and references

##### Modifying Records
Same validations as adding, plus:
- Existing IDs must be valid
- Positive experience for doctors

##### Deleting Records
- Cascading deletes for related records
- Valid ID required (>0)

#### Doctor Salary Calculation
- View doctor details with monthly salary based on:
  - Type 0: 12h day shift ($100/hr)
  - Type 1: 12h night shift (1.2× day rate)
  - Type 2: 24h on-call (1.5× night rate)

### Patient Functionalities

#### Consultation Feedback
- Submit written feedback (5-255 chars)
- Star rating (1-5)
- Validation with error messages

## Validation Rules
All operations include comprehensive validation with appropriate error messages displayed to users.

## Technical Requirements
- Backend: C# .NET
- Frontend: WinUI
- Database: SQL Server
