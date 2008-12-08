<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLogging
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogging))
        Me.lblE = New System.Windows.Forms.Label
        Me.chkLog = New System.Windows.Forms.CheckBox
        Me.lblE1 = New System.Windows.Forms.Label
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.chkDontRun = New System.Windows.Forms.CheckBox
        Me.sfdLogFile = New System.Windows.Forms.SaveFileDialog
        Me.btnLogSave = New System.Windows.Forms.Button
        Me.btnLogView = New System.Windows.Forms.Button
        Me.txtLogContents = New System.Windows.Forms.TextBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblE
        '
        Me.lblE.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.lblE.Location = New System.Drawing.Point(9, 22)
        Me.lblE.Name = "lblE"
        Me.lblE.Size = New System.Drawing.Size(318, 35)
        Me.lblE.TabIndex = 0
        Me.lblE.Text = "Log feature allows you to create a text file with all the changes you made in the" & _
            " program."
        '
        'chkLog
        '
        Me.chkLog.AutoSize = True
        Me.chkLog.Location = New System.Drawing.Point(333, 21)
        Me.chkLog.Name = "chkLog"
        Me.chkLog.Size = New System.Drawing.Size(87, 17)
        Me.chkLog.TabIndex = 2
        Me.chkLog.Text = "Log Queries."
        Me.chkLog.UseVisualStyleBackColor = True
        '
        'lblE1
        '
        Me.lblE1.AutoSize = True
        Me.lblE1.Location = New System.Drawing.Point(12, 53)
        Me.lblE1.Name = "lblE1"
        Me.lblE1.Size = New System.Drawing.Size(27, 13)
        Me.lblE1.TabIndex = 3
        Me.lblE1.Text = "File:"
        '
        'txtFile
        '
        Me.txtFile.Location = New System.Drawing.Point(12, 69)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.Size = New System.Drawing.Size(315, 21)
        Me.txtFile.TabIndex = 4
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(333, 67)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(99, 23)
        Me.btnBrowse.TabIndex = 5
        Me.btnBrowse.Text = "Browse..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'chkDontRun
        '
        Me.chkDontRun.AutoSize = True
        Me.chkDontRun.Location = New System.Drawing.Point(333, 44)
        Me.chkDontRun.Name = "chkDontRun"
        Me.chkDontRun.Size = New System.Drawing.Size(202, 17)
        Me.chkDontRun.TabIndex = 6
        Me.chkDontRun.Text = "Don't make changes to the database"
        Me.chkDontRun.UseVisualStyleBackColor = True
        '
        'sfdLogFile
        '
        Me.sfdLogFile.Filter = "SQL File (*.sql)|*.sql|All files (*.*)|*.*"
        '
        'btnLogSave
        '
        Me.btnLogSave.Location = New System.Drawing.Point(271, 96)
        Me.btnLogSave.Name = "btnLogSave"
        Me.btnLogSave.Size = New System.Drawing.Size(254, 23)
        Me.btnLogSave.TabIndex = 8
        Me.btnLogSave.Text = "Save Data"
        Me.btnLogSave.UseVisualStyleBackColor = True
        '
        'btnLogView
        '
        Me.btnLogView.Location = New System.Drawing.Point(12, 96)
        Me.btnLogView.Name = "btnLogView"
        Me.btnLogView.Size = New System.Drawing.Size(254, 23)
        Me.btnLogView.TabIndex = 9
        Me.btnLogView.Text = "View / Refresh Data"
        Me.btnLogView.UseVisualStyleBackColor = True
        '
        'txtLogContents
        '
        Me.txtLogContents.Location = New System.Drawing.Point(15, 125)
        Me.txtLogContents.Multiline = True
        Me.txtLogContents.Name = "txtLogContents"
        Me.txtLogContents.Size = New System.Drawing.Size(510, 377)
        Me.txtLogContents.TabIndex = 10
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(438, 67)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(87, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "Proceed"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmLogging
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(537, 514)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtLogContents)
        Me.Controls.Add(Me.btnLogView)
        Me.Controls.Add(Me.btnLogSave)
        Me.Controls.Add(Me.chkDontRun)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtFile)
        Me.Controls.Add(Me.lblE1)
        Me.Controls.Add(Me.chkLog)
        Me.Controls.Add(Me.lblE)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogging"
        Me.Text = "Logging Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblE As System.Windows.Forms.Label
    Friend WithEvents chkLog As System.Windows.Forms.CheckBox
    Friend WithEvents lblE1 As System.Windows.Forms.Label
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents chkDontRun As System.Windows.Forms.CheckBox
    Friend WithEvents sfdLogFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnLogSave As System.Windows.Forms.Button
    Friend WithEvents btnLogView As System.Windows.Forms.Button
    Friend WithEvents txtLogContents As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
