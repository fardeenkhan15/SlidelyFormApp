Imports Newtonsoft.Json
Imports System.Net.Http


Public Class ViewForm
    Private currentIndex As Integer = 0
    Public Shared Submissions As List(Of Submission)
    Private WithEvents txtSearchEmail As New TextBox()
    Private WithEvents btnSearch As New Button()

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()


        txtSearchEmail.Location = New Point(20, Me.ClientSize.Height - 60)
        txtSearchEmail.Size = New Size(200, 20)
        Me.Controls.Add(txtSearchEmail)

        btnSearch.Location = New Point(230, Me.ClientSize.Height - 60)
        btnSearch.Size = New Size(75, 20)
        btnSearch.Text = "Search"
        Me.Controls.Add(btnSearch)

        ' Set form properties
        Me.BackColor = Color.White
        Me.ForeColor = Color.Black

        Me.Font = New Font("Arial", 10, FontStyle.Regular)

        ' Style search controls
        txtSearchEmail.BackColor = Color.White
        txtSearchEmail.ForeColor = Color.Black
        txtSearchEmail.Font = New Font("Arial", 10, FontStyle.Regular)

        btnSearch.BackColor = Color.DarkBlue
        btnSearch.ForeColor = Color.White
        btnSearch.Font = New Font("Arial", 10, FontStyle.Bold)
        btnSearch.FlatStyle = FlatStyle.Flat
        btnSearch.FlatAppearance.BorderSize = 0 ' Remove border

        ' Style navigation buttons
        btnPrevious.BackColor = Color.LightGoldenrodYellow
        btnPrevious.ForeColor = Color.Black
        btnPrevious.Font = New Font("Arial", 10, FontStyle.Regular)
        btnPrevious.FlatStyle = FlatStyle.Standard

        btnDelete.BackColor = Color.Red
        btnDelete.ForeColor = Color.Black
        btnDelete.Font = New Font("Arial", 10, FontStyle.Regular)
        btnDelete.FlatStyle = FlatStyle.Standard


        btnEdit.BackColor = Color.LightGreen
        btnEdit.ForeColor = Color.Black
        btnEdit.Font = New Font("Arial", 10, FontStyle.Regular)
        btnEdit.FlatStyle = FlatStyle.Standard


        btnNext.BackColor = Color.LightBlue
        btnNext.ForeColor = Color.Black
        btnNext.Font = New Font("Arial", 10, FontStyle.Regular)
        btnNext.FlatStyle = FlatStyle.Standard

        ' Add any initialization after the InitializeComponent() call.
        Submissions = New List(Of Submission)()
    End Sub

    Private Async Sub ViewForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadSubmissionsFromServer()
        DisplaySubmission()
    End Sub

    Private Async Function LoadSubmissionsFromServer() As Task
        Using client As New HttpClient()
            Try
                Dim index As Integer = 0
                Dim moreSubmissions As Boolean = True

                While moreSubmissions
                    Try
                        Dim response = Await client.GetStringAsync($"http://localhost:3000/read?index={index}")
                        Dim submission = JsonConvert.DeserializeObject(Of Submission)(response)
                        If submission IsNot Nothing Then
                            submissions.Add(submission)
                            index += 1
                        Else
                            moreSubmissions = False
                        End If
                    Catch ex As HttpRequestException
                        moreSubmissions = False
                    End Try
                End While

                If submissions.Count = 0 Then
                    MessageBox.Show("No submissions found.")
                End If
            Catch ex As Exception
                MessageBox.Show("Failed to load submissions from server: " & ex.Message)
            End Try
        End Using
    End Function

    Private Async Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim email As String = txtSearchEmail.Text.Trim()

        If String.IsNullOrEmpty(email) Then
            MessageBox.Show("Please enter an email to search.")
            Return
        End If

        Try
            Using client As New HttpClient()
                Dim response = Await client.GetAsync($"http://localhost:3000/search?email={email}")

                If response.IsSuccessStatusCode Then
                    Dim responseData = Await response.Content.ReadAsStringAsync()
                    Dim foundSubmissions As List(Of Submission) = JsonConvert.DeserializeObject(Of List(Of Submission))(responseData)

                    If foundSubmissions.Count > 0 Then
                        Submissions.Clear()
                        Submissions.AddRange(foundSubmissions)
                        currentIndex = 0
                        DisplaySubmission()
                    Else
                        MessageBox.Show("No submissions found with the provided email.")
                    End If
                Else
                    MessageBox.Show($"Error searching submissions: {response.StatusCode}")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Failed to search submissions: {ex.Message}")
        End Try
    End Sub


    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If currentIndex > 0 Then
            currentIndex -= 1
            DisplaySubmission()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentIndex < submissions.Count - 1 Then
            currentIndex += 1
            DisplaySubmission()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        DeleteSubmissionFromServer(currentIndex)
    End Sub

    Private Async Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Dim editForm As New CreateForm(currentIndex)
        If editForm.ShowDialog() = DialogResult.OK Then
            ' Update submission in the list
            Submissions(currentIndex) = editForm.UpdatedSubmission

            ' Refresh UI
            DisplaySubmission()
        End If
    End Sub

    Private Sub DisplaySubmission()
        If Submissions.Count > 0 AndAlso currentIndex >= 0 AndAlso currentIndex < Submissions.Count Then
            Dim submission = Submissions(currentIndex)
            txtName.Text = submission.name
            txtEmail.Text = submission.email
            txtPhone.Text = submission.phone
            txtGithubLink.Text = submission.github_link
            txtStopwatchTime.Text = submission.stopwatch_time
        Else
            ClearFormFields()
        End If
    End Sub

    Private Sub ClearFormFields()
        txtName.Clear()
        txtEmail.Clear()
        txtPhone.Clear()
        txtGithubLink.Clear()
        txtStopwatchTime.Clear()
    End Sub
    Private Async Sub DeleteSubmissionFromServer(index As Integer)
        Try
            Using client As New HttpClient()
                Dim response As HttpResponseMessage = Await client.DeleteAsync($"http://localhost:3000/delete/{index}")

                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Submission deleted successfully!")
                    submissions.RemoveAt(index)
                    If currentIndex >= submissions.Count Then
                        currentIndex = submissions.Count - 1
                    End If
                    DisplaySubmission()
                Else
                    MessageBox.Show("Failed to delete submission: " & response.StatusCode)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error deleting submission: " & ex.Message)
        End Try
    End Sub


    Private Sub ViewForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.P Then
            btnPrevious.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            btnNext.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.D Then
            btnDelete.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.U Then
            btnEdit.PerformClick()
        End If
    End Sub
    Private Sub CreateForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub
End Class


