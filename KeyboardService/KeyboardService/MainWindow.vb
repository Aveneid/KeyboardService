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
    Dim buttons As New List(Of Button)
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
        For Each b In Controls
            If TypeOf b Is Button Then
                If b IsNot Nothing Then
                    buttons.Add(b)
                    Logs.Log("# found button " & b.Name)
                End If
            End If
        Next
        For Each b In buttons
            AddHandler b.Click, AddressOf btnHandler
        Next
        sortButtons()

    End Sub
    Sub sortButtons()
        Dim tmp As New List(Of Button)
        Logs.Log("# order of buttons: ")
        Dim tString As String = ""
        For l = 0 To 7
            tString &= " " & buttons(l).Name
        Next
        Logs.Log(tString)

        For i = 1 To 8
            For j = 0 To 7
                If buttons(j).Name = "btn" & i Then
                    tmp.Add(buttons(j))
                End If
            Next
        Next

        buttons = tmp

        Logs.Log("# buttons order sorted")
        Logs.Log("# order of buttons: ")
        tString = ""
        For l = 0 To 7
            tString &= " " & buttons(l).Name
        Next

        Logs.Log(tString)
    End Sub

    Sub btnHandler(ByVal sender As System.Object, e As System.EventArgs)
        Me.KeyPreview = True
        activeKey = CInt(CType(sender, Button).Name.Substring(3))
        Logs.Log("# active key: " & activeKey)
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
                    Logs.Log("! Error: read timeout")
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
            If e.KeyCode.ToString IsNot Nothing Then
                If e.Control Then
                    pressedKeys.Add("^")
                    pressedKeysNames.Add("Ctrl")
                    Controls.Find("btn" & activeKey, False)(0).Text = "Ctrl"
                    Logs.Log("# key pressed: " & "Ctrl")
                End If
                If e.Shift Then
                    pressedKeys.Add("+")
                    pressedKeysNames.Add("Shift")
                    Controls.Find("btn" & activeKey, False)(0).Text = "Shift"
                    Logs.Log("# key pressed: " & "Shift")
                End If
                If e.Alt Then
                    pressedKeys.Add("%")
                    pressedKeysNames.Add("Alt")
                    Controls.Find("btn" & activeKey, False)(0).Text = "Alt"
                    Logs.Log("# key pressed: " & "Alt")
                End If
                If (e.KeyCode >= Windows.Forms.Keys.A AndAlso Windows.Forms.Keys.Z) Or (e.KeyCode >= Windows.Forms.Keys.D0 AndAlso e.KeyCode <= Windows.Forms.Keys.D9) Then
                    pressedKeys.Add(e.KeyCode.ToString)
                    pressedKeysNames.Add(e.KeyCode.ToString)
                    Controls.Find("btn" & activeKey, False)(0).Text = e.KeyCode.ToString
                    Logs.Log("# key pressed: " & e.KeyCode.ToString)
                End If
                Me.KeyPreview = False
                Me.Enabled = True
            End If

        End If
    End Sub
End Class
