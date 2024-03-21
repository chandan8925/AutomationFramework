C# Project Details

A C# solution incorporates two projects: one utilizing Selenium for browser automation and another using NUnit as a testing framework with RestSharp for REST API calls.

Project Setup

Download the Project: Clone or download the project from Git.

Prerequisites: Ensure you have Google Chrome version 123 installed. Outdated versions may cause test failures.


Running the Tests
Open the Test Explorer: Locate the test explorer within your development environment. (This step may vary depending on your IDE)
Run Tests: Select the desired project or test suite within the test explorer and initiate the test run.

Technical Details
UI Automation:
Frameworks: Selenium
Languages: C# (Selenium)

API Automation:
Framework: NUnit
Languages: C#
Library: RestSharp


Benefit Dashboard Automation Project Overview
This project automates tests for the Benefit Dashboard application, covering both the user interface (UI) and application programming interface (API).


Project Structure:
Solution File: BenefitDashboardAutomationProject


Projects:
BenefitDashboardAPITest (8 test cases)
BenefitDashboardUITest (4 test cases)

Test Coverage:

API Tests (BenefitDashboardAPITest):
1) Create, update, and delete employees

2) Validate employee details

3) Verify expected error messages for bad requests and unauthorized access


UI Tests (BenefitDashboardUITest):
1) Add employee and verify benefit and net pay details calculation

2) Update employee details and confirm changes

3) Delete the employee and ensure the removal

4) Verify benefit page Is Displayed Or Not

Additional Features:
Page Object Model (POM): Provides a structured approach to UI testing.
Helper Methods: Reusable functionalities for generating random data, calculating benefits, and net pay.
Manual Bug Injection: Comments within the code highlight bugs added as part of a testing challenge.

Visual Summary: A screenshot of the test results is displayed below

![image](https://github.com/chandan8925/AutomationFramework/assets/14102123/f258baea-b268-4216-b0e3-4bf47833ad6f)

