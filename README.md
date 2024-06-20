# SlidelyFormApp

# Introduction

Welcome to the SlidelyFormApp! This application allows you to manage form submissions with functionalities including creating, editing, deleting, and viewing submissions. This guide will help you navigate and use the application effectively.

# Installation

  Prerequisites:
  
  Make sure you have .NET Framework installed on your system.
  Ensure your server is set up and running, as the application interacts with the server to store and retrieve data.
  
  Download:
  Download the SlidelyFormApp executable from the provided link or repository.
  
# Run the Application:

Double-click the executable file to launch the application or press f5.
Using SlidelyFormApp
# Main form
When you first launch the application, you will be presented with the main interface, where you can navigate between different functionalities:

Create Submission: Open the CreateForm to submit new data.
View Submissions: Open the ViewForm to view, edit, or delete existing submissions.

# Create Submission
Open CreateForm:

Click on the "Create Submission" button to open the form.
Fill Out the Form:

Enter your Name, Email, Phone number, and GitHub link in the respective text fields.
The stopwatch time will be displayed in the label.
Start/Stop Stopwatch:

Click the "Toggle Stopwatch" button to start the stopwatch. Click again to stop it.
Submit the Form:

Click the "Submit" button to send your data to the server.

# View Submissions
Open ViewForm:

Click on the "View Submissions" button to open the view form.
Navigation:

Use the "Previous" and "Next" buttons to navigate through the submissions.
Alternatively, use the keyboard shortcuts:
Ctrl + P for Previous.
Ctrl + N for Next.
Search Submissions:

Enter an email in the search box and click the "Search" button to find submissions with that email. You will have too add the entire email for it work

# Edit Submission:

Click the "Edit" button to open the selected submission in the CreateForm for editing.
Make the necessary changes and click "Submit" to save the updates.
You will have to open view form again the see the changes you made.

# Delete Submission:

Click the "Delete" button to remove the current submission.
Alternatively, use the keyboard shortcut Ctrl + D to delete the current submission.
Additional Features
Keyboard Shortcuts:
Ctrl + S to submit the form (in CreateForm).
Ctrl + T to toggle the stopwatch (in CreateForm).
Ctrl + U to edit the current submission (in ViewForm).
Server Interaction
The application communicates with a server to store and retrieve submissions. Ensure your server is running and accessible at the configured URL (default: http://localhost:3000).

# API Endpoints
POST /submit: To submit a new form.
GET /read?index=<index>: To read a specific submission.
PUT /edit/<index>: To update a specific submission.
DELETE /delete/<index>: To delete a specific submission.
GET /search?email=<email>: To search for submissions by email.

# Troubleshooting
No Submissions Found: Ensure the server is running and reachable. Check if the db.json file is correctly set up and not empty.
Server Errors: Check the server logs for any errors and ensure all endpoints are working correctly.
For further assistance, refer to the project repository or contact support.







