<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChooseSpell
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChooseSpell))
        Me.Label47 = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.lblBonusSearchTitle = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.lstResults = New System.Windows.Forms.ListBox
        Me.chkLimit = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.bttnSearch = New System.Windows.Forms.Button
        Me.bttnProceed = New System.Windows.Forms.Button
        Me.mtbSpellID = New System.Windows.Forms.MaskedTextBox
        Me.imgSearch = New System.Windows.Forms.PictureBox
        CType(Me.imgSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label47
        '
        Me.Label47.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.Location = New System.Drawing.Point(13, 74)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(51, 14)
        Me.Label47.TabIndex = 20
        Me.Label47.Text = "Results:"
        '
        'Label45
        '
        Me.Label45.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.Location = New System.Drawing.Point(13, 32)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(305, 14)
        Me.Label45.TabIndex = 18
        Me.Label45.Text = "Please enter the NAME of the spell you are looking for:"
        '
        'lblBonusSearchTitle
        '
        Me.lblBonusSearchTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblBonusSearchTitle.AutoSize = True
        Me.lblBonusSearchTitle.Font = New System.Drawing.Font("Calibri", 14.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lblBonusSearchTitle.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.lblBonusSearchTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblBonusSearchTitle.Name = "lblBonusSearchTitle"
        Me.lblBonusSearchTitle.Size = New System.Drawing.Size(106, 23)
        Me.lblBonusSearchTitle.TabIndex = 17
        Me.lblBonusSearchTitle.Text = "Spell Search"
        Me.lblBonusSearchTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtName
        '
        Me.txtName.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtName.Location = New System.Drawing.Point(16, 48)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(300, 22)
        Me.txtName.TabIndex = 25
        Me.txtName.Text = "Spell Name"
        '
        'lstResults
        '
        Me.lstResults.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstResults.FormattingEnabled = True
        Me.lstResults.ItemHeight = 14
        Me.lstResults.Location = New System.Drawing.Point(16, 90)
        Me.lstResults.Name = "lstResults"
        Me.lstResults.Size = New System.Drawing.Size(375, 144)
        Me.lstResults.TabIndex = 27
        '
        'chkLimit
        '
        Me.chkLimit.AutoSize = True
        Me.chkLimit.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLimit.Location = New System.Drawing.Point(16, 242)
        Me.chkLimit.Name = "chkLimit"
        Me.chkLimit.Size = New System.Drawing.Size(132, 18)
        Me.chkLimit.TabIndex = 28
        Me.chkLimit.Text = "Limit to 100 Results"
        Me.chkLimit.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(199, 243)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 14)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "Chosen ID:"
        '
        'bttnSearch
        '
        Me.bttnSearch.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bttnSearch.Location = New System.Drawing.Point(16, 269)
        Me.bttnSearch.Name = "bttnSearch"
        Me.bttnSearch.Size = New System.Drawing.Size(122, 33)
        Me.bttnSearch.TabIndex = 31
        Me.bttnSearch.Text = "Search"
        Me.bttnSearch.UseVisualStyleBackColor = True
        '
        'bttnProceed
        '
        Me.bttnProceed.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bttnProceed.Location = New System.Drawing.Point(269, 269)
        Me.bttnProceed.Name = "bttnProceed"
        Me.bttnProceed.Size = New System.Drawing.Size(122, 33)
        Me.bttnProceed.TabIndex = 32
        Me.bttnProceed.Text = "Proceed"
        Me.bttnProceed.UseVisualStyleBackColor = True
        '
        'mtbSpellID
        '
        Me.mtbSpellID.Font = New System.Drawing.Font("Calibri", 8.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mtbSpellID.Location = New System.Drawing.Point(270, 240)
        Me.mtbSpellID.Name = "mtbSpellID"
        Me.mtbSpellID.Size = New System.Drawing.Size(121, 22)
        Me.mtbSpellID.TabIndex = 33
        '
        'imgSearch
        '
        Me.imgSearch.Image = CType(resources.GetObject("imgSearch.Image"), System.Drawing.Image)
        Me.imgSearch.Location = New System.Drawing.Point(321, 12)
        Me.imgSearch.Name = "imgSearch"
        Me.imgSearch.Size = New System.Drawing.Size(70, 70)
        Me.imgSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.imgSearch.TabIndex = 24
        Me.imgSearch.TabStop = False
        '
        'frmChooseSpell
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(404, 311)
        Me.Controls.Add(Me.mtbSpellID)
        Me.Controls.Add(Me.bttnProceed)
        Me.Controls.Add(Me.bttnSearch)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkLimit)
        Me.Controls.Add(Me.lstResults)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.imgSearch)
        Me.Controls.Add(Me.Label47)
        Me.Controls.Add(Me.Label45)
        Me.Controls.Add(Me.lblBonusSearchTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(410, 335)
        Me.MinimumSize = New System.Drawing.Size(410, 335)
        Me.Name = "frmChooseSpell"
        Me.Text = "Spell Search"
        CType(Me.imgSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents imgSearch As System.Windows.Forms.PictureBox
    Friend WithEvents Label47 As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents lblBonusSearchTitle As System.Windows.Forms.Label
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents lstResults As System.Windows.Forms.ListBox
    Friend WithEvents chkLimit As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents bttnSearch As System.Windows.Forms.Button
    Friend WithEvents bttnProceed As System.Windows.Forms.Button
    Friend WithEvents mtbSpellID As System.Windows.Forms.MaskedTextBox
End Class
