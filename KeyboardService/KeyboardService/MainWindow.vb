Imports System.Threading
Imports System.IO
Imports System.Exception
Imports System.Windows.Forms
Public Class MainWindow

    Dim com As IO.Ports.SerialPort = Nothing
    Dim thrd As New Thread(AddressOf ReadSerial)
    Dim cancelling As Boolean = False

    Dim mainFS As New IO.FileStream("./config.cfg", IO.FileMode.OpenOrCreate,
                                    IO.FileAccess.ReadWrite, IO.FileShare.None)

    Dim readFS As New IO.StreamReader(mainFS)
    Dim writeFS As New IO.StreamWriter(mainFS)

    Dim keys() As String = {"A", "B", "C", "D", "E", "F", "G", "H"}

    Dim activeKey As Integer = -1
    Public selectedCOM As String = "None"
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        If thrd.IsAlive Then
            thrd.Abort()
        End If
        Me.Close()
    End Sub

    Private Sub SelectCOMToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectCOMToolStripMenuItem.Click
        SelectCOM.ShowDialog()

    End Sub

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolStripStatusLabel1.Text = "Selected port: " & selectedCOM
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.KeyPreview = True
        activeKey = 1
        Me.Enabled = False
    End Sub

    Private Sub ReadSerial()
        If selectedCOM IsNot "None" Then
            Try
                Dim incomingdata As String = Nothing
                com = My.Computer.Ports.OpenSerialPort(selectedCOM)
                com.BaudRate = CInt(9600)
                Do
                    If cancelling Then
                        Exit Do
                    Else
                        If com IsNot Nothing Then
                            incomingdata = com.ReadLine()
                            If incomingdata IsNot Nothing Then
                                If IsNumeric(incomingdata) Then
                                    SendKeys.Flush()
                                    SendKeys.SendWait(keys(incomingdata))
                                End If
                            End If
                            incomingdata = Nothing
                        End If
                        Thread.Sleep(10)
                    End If
                Loop
            Catch ex As Exception
                Dim exType = ex.GetType
                If exType = GetType(TimeoutException) Then
                    Logs.RichTextBox1.AppendText(TimeOfDay.ToString("hh:mm:ss") & "Error: read timeout")
                End If
                com.Close()
                com = Nothing
                ReadSerial()
            End Try
        End If
    End Sub
    Private Sub StartServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartServiceToolStripMenuItem.Click
        If thrd.ThreadState = ThreadState.Unstarted Or thrd.ThreadState = ThreadState.Stopped Then
            ' thrd = New Thread(AddressOf ReadSerial)
            thrd.IsBackground = True
            thrd.SetApartmentState(ApartmentState.STA)
            thrd.Start()
        End If
    End Sub

    Private Sub StopServiceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopServiceToolStripMenuItem.Click
        If thrd.IsAlive Then
            cancelling = True
        End If
        'thrd.Abort()

    End Sub

    Private Sub ToolStripStatusLabel2_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel2.Click
        Logs.Show()
        Logs.Left = Me.Right
        Logs.Top = Me.Top
    End Sub

    Private Sub MainWindow_Move(sender As Object, e As EventArgs) Handles Me.Move
        If Logs.IsHandleCreated Then
            Logs.Left = Me.Right
            Logs.Top = Me.Top
        End If
    End Sub

    Private Sub MainWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If Me.KeyPreview Then
            Dim pressedKeys As New List(Of String)
            Dim pressedKeysNames As New List(Of String)
            If e.Control Then
                pressedKeys.Add("^")
                pressedKeysNames.Add("Ctrl")
            End If
            If e.Shift Then
                pressedKeys.Add("+")
                pressedKeysNames.Add("Shift")
            End If
            If e.Alt Then
                pressedKeys.Add("%")
                pressedKeysNames.Add("ALt")
            End If
            If e.KeyCode >= Windows.Forms.Keys.A AndAlso Windows.Forms.Keys.Z Then
                pressedKeys.Add(e.KeyCode.ToString)
                pressedKeysNames.Add(e.KeyCode.ToString)
            End If
        End If
    End Sub
End Class
