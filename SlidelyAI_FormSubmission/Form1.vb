Public Class Form1
    Private btnViewSubmissions As Button
    Private btnCreateSubmission As Button
    Private lblInstructions As Label

    Public Sub New()
        InitializeComponent()
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        Me.Text = "Form Submissions App"
        Me.Size = New Size(600, 400)
        Me.StartPosition = FormStartPosition.CenterScreen

        lblInstructions = New Label()
        lblInstructions.Text = "Ravindra Sapkal, Slidely Task 2 - Slidely Form app"
        lblInstructions.Font = New Font(lblInstructions.Font.FontFamily, 18, FontStyle.Bold)
        lblInstructions.AutoSize = True
        lblInstructions.Location = New Point((Me.ClientSize.Width - lblInstructions.Width - 460) \ 2, (Me.ClientSize.Height - lblInstructions.Height) \ 2 - 100)
        Me.Controls.Add(lblInstructions)

        ' Initialize View Submissions button
        btnViewSubmissions = New Button()
        btnViewSubmissions.Text = "View Submissions"
        btnViewSubmissions.BackColor = Color.LightYellow
        btnViewSubmissions.Size = New Size(400, 100)
        btnViewSubmissions.Location = New Point((Me.ClientSize.Width - btnViewSubmissions.Width) \ 2, (Me.ClientSize.Height - btnViewSubmissions.Height) \ 2)
        AddHandler btnViewSubmissions.Click, AddressOf btnViewSubmissions_Click
        Me.Controls.Add(btnViewSubmissions)

        ' Initialize Create Submission button
        btnCreateSubmission = New Button()
        btnCreateSubmission.Text = "Create New Submission"
        btnCreateSubmission.BackColor = Color.LightBlue
        btnCreateSubmission.Size = New Size(400, 100)
        btnCreateSubmission.Location = New Point((Me.ClientSize.Width - btnCreateSubmission.Width) \ 2, btnViewSubmissions.Bottom + 20) ' Position the button below the first button with spacing
        AddHandler btnCreateSubmission.Click, AddressOf btnCreateSubmission_Click
        Me.Controls.Add(btnCreateSubmission)
    End Sub

    Private Sub btnViewSubmissions_Click(sender As Object, e As EventArgs)
        Dim viewForm As New ViewSubmissionsForm()
        viewForm.Show()
    End Sub

    Private Sub btnCreateSubmission_Click(sender As Object, e As EventArgs)
        Dim createForm As New CreateSubmissionForm()
        createForm.Show()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class

