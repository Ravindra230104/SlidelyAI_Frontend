Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Diagnostics

Public Class ViewSubmissionsForm
    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGitHub As TextBox
    Private txtTime As TextBox
    Private btnPrevious As Button
    Private btnNext As Button
    Private btnDelete As Button
    Private btnEdit As Button
    Private txtSearchEmail As TextBox
    Private btnSearch As Button
    Private lblInstructions As Label

    Private currentSubmissionIndex As Integer = 0
    Private submissions As List(Of JObject) = New List(Of JObject)()
    Private baseUrl As String = "http://localhost:3000"

    Public Sub New()
        InitializeComponent()
        InitializeForm()
    End Sub

    Private Sub InitializeForm()
        Me.Text = "View Submissions"
        Me.Size = New Size(900, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        Dim borderPanel As New Panel()
        borderPanel.BorderStyle = BorderStyle.FixedSingle
        borderPanel.Dock = DockStyle.Fill
        Me.Controls.Add(borderPanel)

        lblInstructions = New Label()
        lblInstructions.Text = "Ravindra Sapkal, Slidely Task 2 - View Submissions"
        lblInstructions.Font = New Font(lblInstructions.Font.FontFamily, 18, FontStyle.Bold)
        lblInstructions.AutoSize = True
        lblInstructions.Location = New Point(50, 20)
        borderPanel.Controls.Add(lblInstructions)

        ' Initialize Name textbox
        txtName = New TextBox()
        txtName.Location = New Point(200, 90)
        txtName.Size = New Size(400, 40)
        txtName.Multiline = True
        txtName.ReadOnly = True
        txtName.BackColor = Color.LightGray
        borderPanel.Controls.Add(txtName)
        Dim lblName As New Label()
        lblName.Text = "Name:"
        lblName.Location = New Point(50, 90)
        lblName.Font = New Font(lblName.Font.FontFamily, 12)
        lblName.AutoSize = True
        borderPanel.Controls.Add(lblName)

        ' Initialize Email textbox
        txtEmail = New TextBox()
        txtEmail.Location = New Point(200, 140)
        txtEmail.Size = New Size(400, 40)
        txtEmail.Multiline = True
        txtEmail.ReadOnly = True
        txtEmail.BackColor = Color.LightGray
        borderPanel.Controls.Add(txtEmail)
        Dim lblEmail As New Label()
        lblEmail.Text = "Email:"
        lblEmail.Location = New Point(50, 140)
        lblEmail.Font = New Font(lblEmail.Font.FontFamily, 12)
        lblEmail.AutoSize = True
        borderPanel.Controls.Add(lblEmail)

        ' Initialize Phone textbox
        txtPhone = New TextBox()
        txtPhone.Location = New Point(200, 190)
        txtPhone.Size = New Size(400, 40)
        txtPhone.Multiline = True
        txtPhone.ReadOnly = True
        txtPhone.BackColor = Color.LightGray
        borderPanel.Controls.Add(txtPhone)
        Dim lblPhone As New Label()
        lblPhone.Text = "Phone:"
        lblPhone.Location = New Point(50, 190)
        lblPhone.Font = New Font(lblPhone.Font.FontFamily, 12)
        lblPhone.AutoSize = True
        borderPanel.Controls.Add(lblPhone)

        ' Initialize GitHub textbox
        txtGitHub = New TextBox()
        txtGitHub.Location = New Point(200, 240)
        txtGitHub.Size = New Size(400, 40)
        txtGitHub.Multiline = True
        txtGitHub.ReadOnly = True
        txtGitHub.BackColor = Color.LightGray
        borderPanel.Controls.Add(txtGitHub)
        Dim lblGitHub As New Label()
        lblGitHub.Text = "GitHub Link:"
        lblGitHub.Location = New Point(50, 240)
        lblGitHub.Font = New Font(lblGitHub.Font.FontFamily, 12)
        lblGitHub.AutoSize = True
        borderPanel.Controls.Add(lblGitHub)

        ' Initialize Time textbox
        txtTime = New TextBox()
        txtTime.Location = New Point(200, 290)
        txtTime.Size = New Size(400, 40)
        txtTime.Multiline = True
        txtTime.ReadOnly = True
        txtTime.BackColor = Color.LightGray
        borderPanel.Controls.Add(txtTime)
        Dim lblTime As New Label()
        lblTime.Text = " Time:"
        lblTime.Location = New Point(50, 290)
        lblTime.Font = New Font(lblTime.Font.FontFamily, 12)
        lblTime.AutoSize = True
        borderPanel.Controls.Add(lblTime)

        ' Initialize Previous button
        btnPrevious = New Button()
        btnPrevious.Text = "Previous (CTRL+P)"
        btnPrevious.Location = New Point(50, 400)
        btnPrevious.Size = New Size(160, 40)
        AddHandler btnPrevious.Click, AddressOf BtnPrevious_Click
        btnPrevious.BackColor = Color.LightYellow
        borderPanel.Controls.Add(btnPrevious)

        ' Initialize Next button
        btnNext = New Button()
        btnNext.Text = "Next (CTRL+N)"
        btnNext.Location = New Point(230, 400)
        btnNext.Size = New Size(160, 40)
        AddHandler btnNext.Click, AddressOf BtnNext_Click
        btnNext.BackColor = Color.LightBlue
        borderPanel.Controls.Add(btnNext)

        ' Initialize Delete button
        btnDelete = New Button()
        btnDelete.Text = "Delete"
        btnDelete.Location = New Point(410, 400)
        btnDelete.Size = New Size(160, 40)
        AddHandler btnDelete.Click, AddressOf BtnDelete_Click
        borderPanel.Controls.Add(btnDelete)

        ' Initialize Edit button
        btnEdit = New Button()
        btnEdit.Text = "Edit"
        btnEdit.Location = New Point(590, 400)
        btnEdit.Size = New Size(160, 40)
        AddHandler btnEdit.Click, AddressOf BtnEdit_Click
        btnEdit.BackColor = Color.LightGreen
        borderPanel.Controls.Add(btnEdit)


        ' Initialize Search Email textbox
        txtSearchEmail = New TextBox()
        txtSearchEmail.Location = New Point(200, 350)
        txtSearchEmail.Size = New Size(400, 40)
        borderPanel.Controls.Add(txtSearchEmail)
        Dim lblSearchEmail As New Label()
        lblSearchEmail.Text = "Search by Email:"
        lblSearchEmail.Location = New Point(50, 350)
        lblSearchEmail.Font = New Font(lblSearchEmail.Font.FontFamily, 12)
        lblSearchEmail.AutoSize = True
        borderPanel.Controls.Add(lblSearchEmail)

        ' Initialize Search button
        btnSearch = New Button()
        btnSearch.Text = "Search"
        btnSearch.Location = New Point(620, 350)
        btnSearch.Size = New Size(100, 30)
        AddHandler btnSearch.Click, AddressOf BtnSearch_Click
        borderPanel.Controls.Add(btnSearch)


        LoadSubmission(currentSubmissionIndex)
        AddHandler Me.KeyDown, AddressOf ViewSubmissionsForm_KeyDown

    End Sub

    Private Async Sub LoadSubmission(index As Integer)
        Try
            Using client As New HttpClient()
                Dim response = Await client.GetAsync($"{baseUrl}/read?index={index}")
                response.EnsureSuccessStatusCode()
                Dim responseBody = Await response.Content.ReadAsStringAsync()
                Dim submission = JsonConvert.DeserializeObject(Of JObject)(responseBody)
                If submission IsNot Nothing Then
                    txtName.Text = submission("name")?.ToString()
                    txtEmail.Text = submission("email")?.ToString()
                    txtPhone.Text = submission("phone")?.ToString()
                    txtGitHub.Text = submission("github")?.ToString()
                    txtTime.Text = submission("stopwatch_time")?.ToString()
                Else
                    MessageBox.Show("No more submissions.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}")
        End Try
    End Sub

    Private Sub BtnPrevious_Click(sender As Object, e As EventArgs)
        If currentSubmissionIndex > 0 Then
            currentSubmissionIndex -= 1
            LoadSubmission(currentSubmissionIndex)
        Else
            MessageBox.Show("This is the first submission.")
        End If
    End Sub

    Private Sub BtnNext_Click(sender As Object, e As EventArgs)
        currentSubmissionIndex += 1
        LoadSubmission(currentSubmissionIndex)
    End Sub

    Private Async Sub BtnDelete_Click(sender As Object, e As EventArgs)
        Try
            Using client As New HttpClient()
                Dim response = Await client.DeleteAsync($"{baseUrl}/delete?index={currentSubmissionIndex}")
                response.EnsureSuccessStatusCode()
                MessageBox.Show("Submission deleted successfully.")
                If currentSubmissionIndex > 0 Then
                    currentSubmissionIndex -= 1
                End If
                LoadSubmission(currentSubmissionIndex)
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}")
        End Try
    End Sub

    Private Async Sub BtnEdit_Click(sender As Object, e As EventArgs)
        Try
            Dim name = txtName.Text
            Dim email = txtEmail.Text
            Dim phone = txtPhone.Text
            Dim github = txtGitHub.Text
            Dim stopwatchTime = txtTime.Text

            Using client As New HttpClient()
                Dim content = New StringContent(JsonConvert.SerializeObject(New With {
                    Key .name = name,
                    Key .email = email,
                    Key .phone = phone,
                    Key .github = github,
                    Key .stopwatch_time = stopwatchTime
                }), Encoding.UTF8, "application/json")
                Dim response = Await client.PutAsync($"{baseUrl}/edit?index={currentSubmissionIndex}", content)
                response.EnsureSuccessStatusCode()
                MessageBox.Show("Submission edited successfully.")
                LoadSubmission(currentSubmissionIndex)
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}")
        End Try
    End Sub

    Private Async Sub BtnSearch_Click(sender As Object, e As EventArgs)
        Try
            Dim email = txtSearchEmail.Text

            Using client As New HttpClient()
                Dim response = Await client.GetAsync($"{baseUrl}/search?email={email}")
                response.EnsureSuccessStatusCode()
                Dim responseBody = Await response.Content.ReadAsStringAsync()


                If responseBody.StartsWith("[") Then
                    Dim submissions = JsonConvert.DeserializeObject(Of List(Of JObject))(responseBody)
                    If submissions.Count > 0 Then
                        Dim submission = submissions(0)
                        txtName.Text = submission("name")?.ToString()
                        txtEmail.Text = submission("email")?.ToString()
                        txtPhone.Text = submission("phone")?.ToString()
                        txtGitHub.Text = submission("github")?.ToString()
                        txtTime.Text = submission("stopwatch_time")?.ToString()
                    Else
                        MessageBox.Show("Email does not exist.")
                    End If
                ElseIf responseBody.StartsWith("{") Then
                    Dim submission = JsonConvert.DeserializeObject(Of JObject)(responseBody)
                    If submission IsNot Nothing Then
                        txtName.Text = submission("name")?.ToString()
                        txtEmail.Text = submission("email")?.ToString()
                        txtPhone.Text = submission("phone")?.ToString()
                        txtGitHub.Text = submission("github")?.ToString()
                        txtTime.Text = submission("stopwatch_time")?.ToString()
                    Else
                        MessageBox.Show("Email does not exist.")
                    End If
                Else
                    MessageBox.Show("Invalid response format.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"Error: {ex.Message}")
        End Try
    End Sub


    Private Sub ViewSubmissionsForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.P Then
            BtnPrevious_Click(Nothing, Nothing)
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            BtnNext_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If keyData = (Keys.Control Or Keys.P) Then
            btnPrevious.PerformClick()
            Return True
        ElseIf keyData = (Keys.Control Or Keys.N) Then
            btnNext.PerformClick()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class