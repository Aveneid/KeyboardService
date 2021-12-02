<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Logs
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.LogsBox = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        '
        'LogsBox
        '
        Me.LogsBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LogsBox.Location = New System.Drawing.Point(0, 0)
        Me.LogsBox.Name = "LogsBox"
        Me.LogsBox.ReadOnly = True
        Me.LogsBox.Size = New System.Drawing.Size(274, 374)
        Me.LogsBox.TabIndex = 0
        Me.LogsBox.Text = ""
        '
        'Logs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 374)
        Me.Controls.Add(Me.LogsBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(290, 413)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(290, 413)
        Me.Name = "Logs"
        Me.Text = "Logs"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LogsBox As System.Windows.Forms.RichTextBox
End Class
