# Requirements

## Admin Features

### Authentication & Access
- Admin users (authenticated via username and password) can log in to access the Hospital Application's admin panel.
- Admin users can log out and will be redirected to the login page.

### Viewing Information
Admin users can view:
- Doctor accounts
- Hospital equipment and consumables
- Doctor schedules and shifts
- Departments and rooms

### Adding Information
Admin users can add:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules and shifts
- Departments and rooms

#### Validation Rules for Adding Data:
- **Doctor Accounts:**
  - `UserID` must exist in the user records and have the role of Doctor.
  - `UserID` must not already be in the doctor records.
  - `DepartmentID` must correspond to an existing department.
  - `Years of Experience` must be a non-negative float.
  - `Rating` is auto-calculated based on consultation reviews, initially set to `0`.
  - `License Number` must be a non-empty string.
- **Drugs:**
  - `Name`, `Administration`, and `Specification` must contain alphanumeric characters.
  - `Supply` must be a non-negative integer.
- **Equipment:**
  - `Name`, `Type`, and `Specifications` must contain alphanumeric characters.
  - `Stock` must be a non-negative integer.
- **Shifts:**
  - `Month` must be an integer between `1-12`.
  - `Days` must be between `1-21`.
  - `Type` must be `0` (08:00 - 20:00), `1` (20:00 - 08:00), or `2` (08:00 - 08:00).
- **Schedules:**
  - Must provide valid `DoctorID` and `ShiftID`.
- **Departments:**
  - `Name` must contain alphanumeric characters.
- **Rooms:**
  - `Capacity` must be a non-negative integer.
  - `DepartmentID` must exist.
  - `EquipmentID` must either exist or not be specified.
- If any validation fails, an error message will be displayed next to the relevant field(s).

### Modifying Information
Admin users can modify:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules
- Departments and rooms

#### Validation Rules for Modifications:
- **Doctors:**
  - `DoctorID` must exist.
  - `UserID` must be an existing user ID.
  - `DepartmentID` must be an existing department ID.
  - `Years of Experience` must be a positive float.
  - `License Number` must be a non-empty string.
- **Drugs:**
  - `Name`, `Administration`, and `Specification` must contain alphanumeric characters.
  - `Supply` must be a non-negative integer.
- **Equipment:**
  - `Name`, `Type`, and `Specifications` must contain alphanumeric characters.
  - `Stock` must be a non-negative integer.
- **Shifts:**
  - `Month` must be an integer between `1-12`.
  - `Days` must be between `1-21`.
  - `Type` must be `0`, `1`, or `2`.
- **Schedules:**
  - `DoctorID` and `ShiftID` must be valid.
- **Departments:**
  - `Name` must contain alphanumeric characters.
- **Rooms:**
  - `Capacity` must be a non-negative integer.
  - `DepartmentID` must exist.
  - `EquipmentID` must either exist or not be specified.
- If any validation fails, an error message will be displayed next to the relevant field(s).

### Deleting Information
Admin users can delete:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules
- Departments and rooms
- Deleting an entity will also remove all related records.

### Viewing Doctor Salaries
- Admin users can view doctor details and salaries in a list on the dashboard.
- Salary is calculated as the total sum of pay rates for all scheduled shifts in the current month.
- **Pay Rates per Shift Type:**
  - **Type 0:** 12h (08:00 - 20:00) → `$200/shift`
  - **Type 1:** 12h (20:00 - 08:00) → `1.2 × Type 0`
  - **Type 2:** 24h (08:00 - 08:00 + On Call) → `1.5 × Type 1`

## Patient Features
### Submitting Feedback
- Logged-in patients can leave:
  - A written review in a textbox (between `5-255` characters).
  - A rating (1 to 5 stars).
  - Clicking a star fills all previous stars (e.g., selecting `3` fills `1` and `2`).
- Clicking "Submit" sends the feedback.
- If any validation fails, an error message appears next to the relevant field(s).