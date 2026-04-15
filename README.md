# Spreadsheet Application

## Overview
This project is a spreadsheet application inspired by Microsoft Excel. It replicates core spreadsheet functionality such as cell referencing, formula evaluation, formatting, and editing operations.

The application was developed in C# using WinForms, with an emphasis on clean architecture and strong object-oriented design. Key principles such as decoupling and high cohesion were prioritized to ensure maintainability and scalability.

Several design patterns were implemented, including:
- Command Pattern (undo/redo functionality)
- Factory Pattern
- Observer Pattern (cell updates and dependencies)
- Singleton Pattern

---

## Features
- Cell referencing (e.g., `=A1`)
- Arithmetic expressions and formulas
- Copy & paste cells
- Undo / Redo functionality
- Save and load spreadsheet files
- Change cell background color

---

## Getting Started

### Option 1: Run Executable
1. Clone the repository
2. Navigate to:
   ```
   Spreadsheet-Clone\Spreadsheet-Application\SpreadsheetUI\bin\Debug\net8.0-windows
   ```
3. Run:
   ```
   HW7.exe
   ```

### Option 2: Run via Visual Studio
1. Open:
   ```
   Spreadsheet-Clone\Spreadsheet-Application\SpreadsheetUI\HW7.sln
   ```
2. Build and run the solution

---

## Usage

### Editing Cells
- Click on a cell and enter a value or expression.

### Cell Referencing
- Use `=` followed by a cell reference:
  ```
  =A1
  ```

### Formulas
- Supports arithmetic expressions:
  ```
  =20*3 + 45/2 * A2
  ```

**Notes:**
- Self-references will result in an error
- Referencing an empty/undefined cell evaluates to `0`

---

### Changing Cell Color
1. Select one or more cells  
2. Click:
   ```
   Cell → Change Background Color
   ```

---

### Save and Load
- Use the **File** menu at the top of the application

---

### Undo / Redo
- Use the **Edit** menu to undo or redo actions

---

## Design Highlights
- Modular architecture using object-oriented principles
- Clear separation between UI and logic
- Efficient dependency tracking for cell updates
- Extensible design for adding new features
