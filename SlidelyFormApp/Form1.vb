Public Class Form1

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.LightGray
        Me.ForeColor = Color.DarkBlue
        Me.Font = New Font("Arial", 10, FontStyle.Regular)

        btnViewSubmissions.BackColor = Color.LightGoldenrodYellow
        btnViewSubmissions.ForeColor = Color.Black
        btnViewSubmissions.Font = New Font("Arial", 10, FontStyle.Regular)
        btnViewSubmissions.FlatStyle = FlatStyle.Standard

        btnCreateSubmission.BackColor = Color.LightBlue
        btnCreateSubmission.ForeColor = Color.Black
        btnCreateSubmission.Font = New Font("Arial", 10, FontStyle.Regular)
        btnCreateSubmission.FlatStyle = FlatStyle.Standard

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnViewSubmissions_Click(sender As Object, e As EventArgs) Handles btnViewSubmissions.Click
        Dim viewForm As New ViewForm()
        viewForm.Show()
    End Sub

    Private Sub btnCreateSubmission_Click(sender As Object, e As EventArgs) Handles btnCreateSubmission.Click
        Dim createForm As New CreateForm()
        createForm.Show()
    End Sub

    Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.N Then

            btnCreateSubmission.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.V Then
            btnViewSubmissions.PerformClick()
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub
End Class
