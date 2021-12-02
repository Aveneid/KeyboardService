Public Class Logs
    Private Sub Logs_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Sub Log(text As String)
        LogsBox.AppendText("(" & TimeOfDay.ToString("HH:mm:ss") & ") ")
        LogsBox.AppendText(text)
        LogsBox.AppendText(vbNewLine)
    End Sub
End Class