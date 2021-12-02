Public Class SelectCOM

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MainWindow.selectedCOM = ComPortsList.SelectedItem
        MainWindow.ToolStripStatusLabel1.Text = "Selected port: " & ComPortsList.SelectedItem
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ComPortsList.Items.Clear()
        For Each port As String In My.Computer.Ports.SerialPortNames
            ComPortsList.Items.Add(port)
        Next
        If ComPortsList.Items.Count > 0 Then
            ComPortsList.SelectedIndex = 0
        End If
    End Sub

    Private Sub SelectCOM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComPortsList.Items.Clear()
        For Each port As String In My.Computer.Ports.SerialPortNames
            ComPortsList.Items.Add(port)
        Next
        If ComPortsList.Items.Count > 0 Then
            ComPortsList.SelectedIndex = 0
        End If
    End Sub
End Class