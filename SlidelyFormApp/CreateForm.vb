Imports System.Net.Http

Public Class CreateForm
    Private stopwatch As Stopwatch = New Stopwatch()
    Private isEditing As Boolean = False
    Private editingIndex As Integer = -1
    Private WithEvents timer As New Timer()

    Public Property UpdatedSubmission As Submission

    Public Sub New(Optional index As Integer = -1)
        InitializeComponent()

        Me.BackColor = Color.LightGray
        Me.ForeColor = Color.DarkBlue
        Me.Font = New Font("Arial", 10, FontStyle.Regular)

        lblStopwatchTime.BackColor = Color.White
        lblStopwatchTime.ForeColor = Color.Black
        lblStopwatchTime.Font = New Font("Arial", 10, FontStyle.Regular)

        btnToggleStopwatch.BackColor = Color.LightGoldenrodYellow
        btnToggleStopwatch.ForeColor = Color.Black
        btnToggleStopwatch.Font = New Font("Arial", 10, FontStyle.Regular)
        btnToggleStopwatch.FlatStyle = FlatStyle.Standard

        btnSubmit.BackColor = Color.LightBlue
        btnSubmit.ForeColor = Color.Black
        btnSubmit.Font = New Font("Arial", 10, FontStyle.Regular)
        btnSubmit.FlatStyle = FlatStyle.Standard

        ' Set timer interval to 1 second (1000 milliseconds)
        timer.Interval = 1000

        If index >= 0 Then
            isEditing = True
            editingIndex = index
            LoadSubmissionForEdit()
        End If
    End Sub

    Private Sub LoadSubmissionForEdit()
        Dim submission = ViewForm.Submissions(editingIndex)
        txtName.Text = submission.Name
        txtEmail.Text = submission.Email
        txtPhone.Text = submission.Phone
        txtGitHubLink.Text = submission.github_link
        lblStopwatchTime.Text = submission.stopwatch_time
    End Sub

    Private Sub btnToggleStopwatch_Click(sender As Object, e As EventArgs) Handles btnToggleStopwatch.Click
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            timer.Stop()
        Else
            stopwatch.Start()
            timer.Start()
        End If
    End Sub

    Private Sub timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
        lblStopwatchTime.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim name As String = txtName.Text
        Dim email As String = txtEmail.Text
        Dim phone As String = txtPhone.Text
        Dim gitHubLink As String = txtGitHubLink.Text
        Dim stopwatchTime As String = lblStopwatchTime.Text

        If isEditing Then
            UpdateFormDataOnServer(name, email, phone, gitHubLink, stopwatchTime, editingIndex)
        Else
            SendFormDataToServer(name, email, phone, gitHubLink, stopwatchTime)
        End If
    End Sub

    Private Async Sub SendFormDataToServer(name As String, email As String, phone As String, gitHubLink As String, stopwatchTime As String)
        Try
            Using client As New HttpClient()
                Dim formData As New Dictionary(Of String, String) From {
                    {"name", name},
                    {"email", email},
                    {"phone", phone},
                    {"github_link", gitHubLink},
                    {"stopwatch_time", stopwatchTime}
                }

                Dim content As New FormUrlEncodedContent(formData)
                Dim response As HttpResponseMessage = Await client.PostAsync("http://localhost:3000/submit", content)

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Form submitted successfully!")
                Else
                    Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                    MessageBox.Show("Failed to submit form: " & response.StatusCode & " - " & responseBody)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error submitting form: " & ex.Message)
        End Try
    End Sub

    Private Async Sub UpdateFormDataOnServer(name As String, email As String, phone As String, gitHubLink As String, stopwatchTime As String, index As Integer)
        Try
            Using client As New HttpClient()
                Dim formData As New Dictionary(Of String, String) From {
                    {"name", name},
                    {"email", email},
                    {"phone", phone},
                    {"github_link", gitHubLink},
                    {"stopwatch_time", stopwatchTime},
                    {"index", index.ToString()}
                }

                Dim content As New FormUrlEncodedContent(formData)
                Dim response As HttpResponseMessage = Await client.PutAsync($"http://localhost:3000/edit/{index}", content)

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Form updated successfully!")
                Else
                    Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                    MessageBox.Show("Failed to update form: " & response.StatusCode & " - " & responseBody)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error updating form: " & ex.Message)
        End Try
    End Sub

    Private Sub CreateForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.S Then
            btnSubmit.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.T Then
            btnToggleStopwatch.PerformClick()
        End If
    End Sub

    Private Sub CreateForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub
End Class
