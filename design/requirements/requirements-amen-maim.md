# Requirements

## Admin Functionalities

### Authentication
- **Login:** As an admin, I want to log in to the Hospital Application using my username and password to access the admin version.
- **Logout:** As an admin, I want to log out and be redirected to the login page.

### Viewing Information
As an admin, I want to view:
- Doctor accounts
- Hospital equipment and consumables
- Doctor schedules and shifts
- Departments and rooms

### Adding Information
As an admin, I want to add:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules and shifts
- Departments and rooms

#### Validation Rules
- **Doctor:**
  - `UserID` must exist in the user record with the role "Doctor" and must not be already present in the doctors' record.
  - `DepartmentID` must correspond to an existing department.
  - `Experience` must be a non-negative float.
  - `Rating` is auto-calculated from consultation reviews, initially set to 0.
  - `License Number` must be a non-empty string.
- **Drugs:**
  - `Name`, `Administration`, and `Specification` must contain alphanumeric characters.
  - `Supply` must be a non-negative integer.
- **Equipment:**
  - `Name`, `Type`, and `Specifications` must contain alphanumeric characters.
  - `Stock` must be a non-negative integer.
- **Shifts:**
  - `Start Time` and `End Time` must be either 08:00 or 20:00.
- **Schedules:**
  - Must provide an existing `DoctorID` and `ShiftID`.
- **Departments:**
  - `Name` must contain alphanumeric characters.
- **Rooms:**
  - `Capacity` must be a non-negative integer.
  - `DepartmentID` must exist.
  - `EquipmentID` must either exist or not be specified.
- **Error Handling:**
  - If any validation fails, an error message will be displayed under the **Add** button.

### Modifying Information
As an admin, I want to modify:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules
- Departments and rooms

#### Validation Rules
- **Doctor:**
  - `UserID` must be an existing user ID.
  - `DepartmentID` must correspond to an existing department.
  - `Experience` must be a positive float.
  - `License Number` must be a non-empty string.
- **Drugs & Equipment:**
  - Fields must contain alphanumeric characters.
  - `Supply` and `Stock` must be non-negative integers.
- **Shifts & Schedules:**
  - `Start Time` and `End Time` must be either 08:00 or 20:00.
  - `DoctorID` and `ShiftID` must exist for schedules.
- **Departments & Rooms:**
  - `Name` must contain alphanumeric characters.
  - `Capacity` must be a non-negative integer.
  - `DepartmentID` and `EquipmentID` must exist or not be specified.
- **Error Handling:**
  - If validation fails, an error message will be displayed under the **Update** button.

### Deleting Information
As an admin, I want to delete:
- Doctor accounts
- Hospital equipment and drugs
- Doctor schedules and shifts
- Departments and rooms

#### Deletion Rules
- All related records should also be deleted.
- The provided `ID` must be an existing and valid ID (greater than 0).

### Viewing Doctor Details & Salary
As an admin, I want to see a list of all doctors along with their salaries, calculated based on scheduled shifts for the current month:

| Shift Type  | Duration  | Pay Rate |
|-------------|----------|---------|
| Type 0      | 12h (08:00 - 20:00) | $200/shift |
| Type 1      | 12h (20:00 - 08:00) | 1.2 × Type 0 |
| Type 2      | 24h (08:00 - 08:00 + On Call) | 1.5 × Type 1 |

## Patient Functionalities

### Providing Feedback
As a logged-in patient, after a consultation, I want to:
- Leave a written feedback (5-255 characters) in a text box.
- Rate the doctor on a scale of 1 to 5 stars by selecting a star (e.g., selecting star 3 fills stars 1 & 2 as well).
- Press the **Submit** button to send my feedback.

#### Validation Rules
- If validation fails, an error message will be displayed next to the respective field(s).