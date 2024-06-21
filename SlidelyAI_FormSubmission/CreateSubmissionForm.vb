Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports System.Diagnostics

Public Class CreateSubmissionForm
    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGitHub As TextBox
    Private btnStopwatch As Button
    Private btnSubmit As Button
    Private stopwatch As Stopwatch
    Private lblInstructions As Label
    Private timerLabel As Label

    Public Sub New()
        InitializeComponent()

        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        Me.Text = "Create New Submission"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim borderPanel As New Panel()
        borderPanel.BorderStyle = BorderStyle.FixedSingle
        borderPanel.Dock = DockStyle.Fill
        Me.Controls.Add(borderPanel)

        lblInstructions = New Label()
        lblInstructions.Text = "Ravindra Sapkal, Slidely Task 2 - Create Submission"
        lblInstructions.Font = New Font(lblInstructions.Font.FontFamily, 18, FontStyle.Bold)
        lblInstructions.AutoSize = True
        lblInstructions.Location = New Point(50, 20)
        borderPanel.Controls.Add(lblInstructions)

        ' Initialize Name textbox
        txtName = New TextBox()
        txtName.Location = New Point(200, 90)
        txtName.Size = New Size(400, 40)
        txtName.Multiline = True
        borderPanel.Controls.Add(txtName)
        Dim lblName As New Label()
        lblName.Text = "Name:"
        lblName.Location = New Point(50, 90)
        lblName.Font = New Font(lblName.Font.FontFamily, 12)
        borderPanel.Controls.Add(lblName)

        ' Initialize Email textbox
        txtEmail = New TextBox()
        txtEmail.Location = New Point(200, 160)
        txtEmail.Size = New Size(400, 40)
        txtEmail.Multiline = True
        borderPanel.Controls.Add(txtEmail)
        Dim lblEmail As New Label()
        lblEmail.Text = "Email:"
        lblEmail.Location = New Point(50, 160)
        lblEmail.Font = New Font(lblEmail.Font.FontFamily, 12)
        borderPanel.Controls.Add(lblEmail)

        ' Initialize Phone textbox
        txtPhone = New TextBox()
        txtPhone.Location = New Point(200, 230)
        txtPhone.Size = New Size(400, 40)
        txtPhone.Multiline = True
        borderPanel.Controls.Add(txtPhone)
        Dim lblPhone As New Label()
        lblPhone.Text = "Phone Num:"
        lblPhone.Location = New Point(50, 230)
        lblPhone.Font = New Font(lblPhone.Font.FontFamily, 12)
        lblPhone.Size = New Size(120, 40)
        borderPanel.Controls.Add(lblPhone)

        ' Initialize GitHub textbox
        txtGitHub = New TextBox()
        txtGitHub.Location = New Point(200, 300)
        txtGitHub.Size = New Size(400, 60)
        txtGitHub.Multiline = True
        borderPanel.Controls.Add(txtGitHub)
        Dim lblGitHub As New Label()
        lblGitHub.Text = "GitHub Link For Task 2:"
        lblGitHub.Location = New Point(50, 300)
        lblGitHub.Font = New Font(lblGitHub.Font.FontFamily, 12)
        lblGitHub.Size = New Size(140, 40)
        lblGitHub.MaximumSize = New Size(140, 0)
        lblGitHub.AutoSize = True
        borderPanel.Controls.Add(lblGitHub)

        ' Initialize Stopwatch button
        btnStopwatch = New Button()
        btnStopwatch.Text = "TOGGLE STOPWATCH (CTRL+T)"
        btnStopwatch.BackColor = Color.LightYellow
        btnStopwatch.Size = New Size(260, 40)
        btnStopwatch.Location = New Point((Me.ClientSize.Width - 520) / 2, 380)
        AddHandler btnStopwatch.Click, AddressOf btnStopwatch_Click
        borderPanel.Controls.Add(btnStopwatch)

        ' Initialize Timer Label
        timerLabel = New Label()
        timerLabel.Text = "00:00:00"
        timerLabel.Font = New Font(timerLabel.Font.FontFamily, 14, FontStyle.Bold)
        timerLabel.AutoSize = True
        timerLabel.Location = New Point(btnStopwatch.Right + 10, btnStopwatch.Top + (btnStopwatch.Height - timerLabel.Height) \ 2)
        borderPanel.Controls.Add(timerLabel)

        ' Initialize Submit button
        btnSubmit = New Button()
        btnSubmit.Text = "SUBMIT (CTRL+S)"
        btnSubmit.BackColor = Color.LightBlue
        btnSubmit.Size = New Size(380, 40)
        btnSubmit.Location = New Point((Me.ClientSize.Width - 520) / 2, 440)
        AddHandler btnSubmit.Click, AddressOf btnSubmit_Click
        borderPanel.Controls.Add(btnSubmit)

        stopwatch = New Stopwatch()
    End Sub

    Private Sub btnStopwatch_Click(sender As Object, e As EventArgs)
        If stopwatch.IsRunning Then
            stopwatch.Stop()
            btnStopwatch.Text = "TOGGLE STOPWATCH"
        Else
            stopwatch.Start()
            btnStopwatch.Text = "STOP"
            Dim timer As New Timer()
            AddHandler timer.Tick, AddressOf UpdateTimerLabel
            timer.Interval = 1000
            timer.Start()
        End If
    End Sub

    Private Sub UpdateTimerLabel(sender As Object, e As EventArgs)
        timerLabel.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs)
        Dim name As String = txtName.Text
        Dim email As String = txtEmail.Text
        Dim phone As String = txtPhone.Text
        Dim gitHub As String = txtGitHub.Text
        Dim stopwatchTime As String = FormatStopwatchTime(stopwatch.Elapsed)

        SubmitForm(name, email, phone, gitHub, stopwatchTime)
    End Sub

    Private Function FormatStopwatchTime(elapsed As TimeSpan) As String
        Return elapsed.ToString("hh\:mm\:ss")
    End Function

    Private Async Sub SubmitForm(name As String, email As String, phone As String, gitHub As String, stopwatchTime As String)
        Dim client As New HttpClient()
        Dim submission As New Dictionary(Of String, String) From {
            {"name", name},
            {"email", email},
            {"phone", phone},
            {"github", gitHub},
            {"stopwatch_time", stopwatchTime}
        }
        Dim content As New StringContent(JsonConvert.SerializeObject(submission), Encoding.UTF8, "application/json")

        Dim response As HttpResponseMessage = Await client.PostAsync("http://localhost:3000/submit", content)

        If response.IsSuccessStatusCode Then
            MessageBox.Show("Submission successful!")
        Else
            MessageBox.Show("Submission failed.")
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.T) Then
            btnStopwatch.PerformClick()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.S) Then
            btnSubmit.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
End Class
