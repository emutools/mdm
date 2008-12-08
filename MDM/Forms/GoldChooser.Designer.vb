<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GoldChooser
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.nudSilver = New System.Windows.Forms.NumericUpDown
        Me.nudCopper = New System.Windows.Forms.NumericUpDown
        Me.nudGold = New System.Windows.Forms.NumericUpDown
        Me.picBuySilver = New System.Windows.Forms.PictureBox
        Me.picBuyCopper = New System.Windows.Forms.PictureBox
        Me.picBuyGold = New System.Windows.Forms.PictureBox
        CType(Me.nudSilver, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCopper, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGold, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBuySilver, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBuyCopper, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picBuyGold, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'nudSilver
        '
        Me.nudSilver.Location = New System.Drawing.Point(121, 9)
        Me.nudSilver.Name = "nudSilver"
        Me.nudSilver.Size = New System.Drawing.Size(46, 21)
        Me.nudSilver.TabIndex = 80
        '
        'nudCopper
        '
        Me.nudCopper.Location = New System.Drawing.Point(204, 9)
        Me.nudCopper.Name = "nudCopper"
        Me.nudCopper.Size = New System.Drawing.Size(46, 21)
        Me.nudCopper.TabIndex = 78
        '
        'nudGold
        '
        Me.nudGold.Location = New System.Drawing.Point(33, 9)
        Me.nudGold.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudGold.Name = "nudGold"
        Me.nudGold.Size = New System.Drawing.Size(46, 21)
        Me.nudGold.TabIndex = 76
        '
        'picBuySilver
        '
        Me.picBuySilver.Image = Global.MDM.My.Resources.Resources.silver
        Me.picBuySilver.Location = New System.Drawing.Point(91, 3)
        Me.picBuySilver.Name = "picBuySilver"
        Me.picBuySilver.Size = New System.Drawing.Size(35, 32)
        Me.picBuySilver.TabIndex = 79
        Me.picBuySilver.TabStop = False
        '
        'picBuyCopper
        '
        Me.picBuyCopper.Image = Global.MDM.My.Resources.Resources.copper
        Me.picBuyCopper.Location = New System.Drawing.Point(174, 3)
        Me.picBuyCopper.Name = "picBuyCopper"
        Me.picBuyCopper.Size = New System.Drawing.Size(35, 32)
        Me.picBuyCopper.TabIndex = 77
        Me.picBuyCopper.TabStop = False
        '
        'picBuyGold
        '
        Me.picBuyGold.Image = Global.MDM.My.Resources.Resources.gold
        Me.picBuyGold.Location = New System.Drawing.Point(3, 3)
        Me.picBuyGold.Name = "picBuyGold"
        Me.picBuyGold.Size = New System.Drawing.Size(35, 32)
        Me.picBuyGold.TabIndex = 75
        Me.picBuyGold.TabStop = False
        '
        'GoldChooser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.nudSilver)
        Me.Controls.Add(Me.picBuySilver)
        Me.Controls.Add(Me.nudCopper)
        Me.Controls.Add(Me.picBuyCopper)
        Me.Controls.Add(Me.nudGold)
        Me.Controls.Add(Me.picBuyGold)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.Name = "GoldChooser"
        Me.Size = New System.Drawing.Size(269, 39)
        CType(Me.nudSilver, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCopper, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGold, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBuySilver, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBuyCopper, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picBuyGold, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents nudSilver As System.Windows.Forms.NumericUpDown
    Friend WithEvents picBuySilver As System.Windows.Forms.PictureBox
    Friend WithEvents nudCopper As System.Windows.Forms.NumericUpDown
    Friend WithEvents picBuyCopper As System.Windows.Forms.PictureBox
    Friend WithEvents nudGold As System.Windows.Forms.NumericUpDown
    Friend WithEvents picBuyGold As System.Windows.Forms.PictureBox

End Class
