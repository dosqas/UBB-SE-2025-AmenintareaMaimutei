# Formatting guidelines
## Naming conventions
### Code
1. Classes:
   - every class must use ``PascalCase`` naming convention
2. Interfaces:
   - every interface must use ``IPascalCase`` naming convention
3. Structs:
   - every struct must use ``camelCase`` naming convention
4. Variables:
   - public member variables must use ``camelCase`` naming convention
   - private member variables must use ``camelCase`` naming convention
   - local variables, parameters must use ``camelCase`` naming convention
5. Properties
   - properties must use ``PascalCase`` naming convention
6. Constants
   - whenever possible to use ``const`` keyword before a variable, it must be used
### Files
  - filenames and directory names must use ```PascalCase```
  - filename must be the same as the main class/struct/interface in that file
  - in each file there is a single class/struct/interface
## Organization
   - ``using`` directives for namespace use always must be declared at the top of the file
   - modifiers occur in the following order: ``public protected internal private new abstract virtual override sealed static readonly extern unsafe volatile async``
   - Class member ordering:
     - Group class members in the following order: 
       - Nested classes, enums, delegates and events.
       - Static, const and readonly fields.
       - Fields and properties.
       - Constructors
       - Methods
     - Within each group, elements should be in the following order: 
       - ``private``
       - ``protected``
       - ``public``
## Whitespace rules
- a maximum of one statement per line
- a maximum of one assignment per statement
- braces should not be omitted
- code should not contain multiple blank lines in a row


