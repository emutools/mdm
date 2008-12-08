'Copyright © 2007-2008, vbCrLf, Emutools.org,
'
'This program is free software; you can redistribute it and/or modify
'it under the terms of the GNU General Public License as published by
'the Free Software Foundation; either version 3 of the License, or
'(at your option) any later version.

'This program is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
'GNU General Public License for more details.

'You should have received a copy of the GNU General Public License
'along with this program; if not, write to the Free Software
'Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
'
'Created By:             vbCrLf, Kalashnikov, DarkenedFate, JD Guzman
'Creation Date:          3/1/2008
'
'Modification Date:      12/8/2008
'
'Modified By:                JD Guzman

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmManager))
        Me.lblConnectionInfo = New System.Windows.Forms.Label
        Me.lblDBConnection = New System.Windows.Forms.Label
        Me.gbxDBInformation = New System.Windows.Forms.GroupBox
        Me.txtDatabase = New System.Windows.Forms.TextBox
        Me.lblDatabase = New System.Windows.Forms.Label
        Me.txtPass = New System.Windows.Forms.TextBox
        Me.lblPass = New System.Windows.Forms.Label
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.lblUser = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.lblPort = New System.Windows.Forms.Label
        Me.txtHost = New System.Windows.Forms.TextBox
        Me.lblHost = New System.Windows.Forms.Label
        Me.bttnConnect = New System.Windows.Forms.Button
        Me.cmbSavedConnections = New System.Windows.Forms.ComboBox
        Me.bttnAddConnection = New System.Windows.Forms.Button
        Me.bttnEditConnection = New System.Windows.Forms.Button
        Me.bttnRemoveConnection = New System.Windows.Forms.Button
        Me.gbxDBInformation.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblConnectionInfo
        '
        Me.lblConnectionInfo.AutoSize = True
        Me.lblConnectionInfo.Font = New System.Drawing.Font("Calibri", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConnectionInfo.Location = New System.Drawing.Point(12, 33)
        Me.lblConnectionInfo.Name = "lblConnectionInfo"
        Me.lblConnectionInfo.Size = New System.Drawing.Size(285, 17)
        Me.lblConnectionInfo.TabIndex = 1
        Me.lblConnectionInfo.Text = "Please enter database connection information:"
        '
        'lblDBConnection
        '
        Me.lblDBConnection.AutoSize = True
        Me.lblDBConnection.Font = New System.Drawing.Font("Calibri", 14.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle))
        Me.lblDBConnection.Location = New System.Drawing.Point(11, 9)
        Me.lblDBConnection.Name = "lblDBConnection"
        Me.lblDBConnection.Size = New System.Drawing.Size(177, 23)
        Me.lblDBConnection.TabIndex = 3
        Me.lblDBConnection.Text = "Database Connection"
        '
        'gbxDBInformation
        '
        Me.gbxDBInformation.Controls.Add(Me.txtDatabase)
        Me.gbxDBInformation.Controls.Add(Me.lblDatabase)
        Me.gbxDBInformation.Controls.Add(Me.txtPass)
        Me.gbxDBInformation.Controls.Add(Me.lblPass)
        Me.gbxDBInformation.Controls.Add(Me.txtUser)
        Me.gbxDBInformation.Controls.Add(Me.lblUser)
        Me.gbxDBInformation.Controls.Add(Me.txtPort)
        Me.gbxDBInformation.Controls.Add(Me.lblPort)
        Me.gbxDBInformation.Controls.Add(Me.txtHost)
        Me.gbxDBInformation.Controls.Add(Me.lblHost)
        Me.gbxDBInformation.Location = New System.Drawing.Point(15, 80)
        Me.gbxDBInformation.Name = "gbxDBInformation"
        Me.gbxDBInformation.Size = New System.Drawing.Size(316, 187)
        Me.gbxDBInformation.TabIndex = 4
        Me.gbxDBInformation.TabStop = False
        '
        'txtDatabase
        '
        Me.txtDatabase.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.txtDatabase.Location = New System.Drawing.Point(9, 156)
        Me.txtDatabase.Name = "txtDatabase"
        Me.txtDatabase.Size = New System.Drawing.Size(304, 22)
        Me.txtDatabase.TabIndex = 9
        '
        'lblDatabase
        '
        Me.lblDatabase.AutoSize = True
        Me.lblDatabase.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.lblDatabase.Location = New System.Drawing.Point(6, 139)
        Me.lblDatabase.Name = "lblDatabase"
        Me.lblDatabase.Size = New System.Drawing.Size(63, 14)
        Me.lblDatabase.TabIndex = 8
        Me.lblDatabase.Text = "Database:"
        '
        'txtPass
        '
        Me.txtPass.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.txtPass.Location = New System.Drawing.Point(9, 114)
        Me.txtPass.Name = "txtPass"
        Me.txtPass.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPass.Size = New System.Drawing.Size(304, 22)
        Me.txtPass.TabIndex = 7
        '
        'lblPass
        '
        Me.lblPass.AutoSize = True
        Me.lblPass.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.lblPass.Location = New System.Drawing.Point(6, 98)
        Me.lblPass.Name = "lblPass"
        Me.lblPass.Size = New System.Drawing.Size(62, 14)
        Me.lblPass.TabIndex = 6
        Me.lblPass.Text = "Password:"
        '
        'txtUser
        '
        Me.txtUser.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.txtUser.Location = New System.Drawing.Point(9, 74)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(304, 22)
        Me.txtUser.TabIndex = 5
        Me.txtUser.Text = "root"
        '
        'lblUser
        '
        Me.lblUser.AutoSize = True
        Me.lblUser.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.lblUser.Location = New System.Drawing.Point(6, 57)
        Me.lblUser.Name = "lblUser"
        Me.lblUser.Size = New System.Drawing.Size(66, 14)
        Me.lblUser.TabIndex = 4
        Me.lblUser.Text = "Username:"
        '
        'txtPort
        '
        Me.txtPort.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.txtPort.Location = New System.Drawing.Point(239, 34)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(71, 22)
        Me.txtPort.TabIndex = 3
        Me.txtPort.Text = "3306"
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.lblPort.Location = New System.Drawing.Point(236, 16)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(31, 14)
        Me.lblPort.TabIndex = 2
        Me.lblPort.Text = "Port:"
        '
        'txtHost
        '
        Me.txtHost.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.txtHost.Location = New System.Drawing.Point(9, 34)
        Me.txtHost.Name = "txtHost"
        Me.txtHost.Size = New System.Drawing.Size(224, 22)
        Me.txtHost.TabIndex = 1
        Me.txtHost.Text = "localhost"
        '
        'lblHost
        '
        Me.lblHost.AutoSize = True
        Me.lblHost.Font = New System.Drawing.Font("Calibri", 9.0!)
        Me.lblHost.Location = New System.Drawing.Point(6, 16)
        Me.lblHost.Name = "lblHost"
        Me.lblHost.Size = New System.Drawing.Size(35, 14)
        Me.lblHost.TabIndex = 0
        Me.lblHost.Text = "Host:"
        '
        'bttnConnect
        '
        Me.bttnConnect.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bttnConnect.Location = New System.Drawing.Point(15, 273)
        Me.bttnConnect.Name = "bttnConnect"
        Me.bttnConnect.Size = New System.Drawing.Size(316, 23)
        Me.bttnConnect.TabIndex = 5
        Me.bttnConnect.Text = "Connect"
        Me.bttnConnect.UseVisualStyleBackColor = True
        '
        'cmbSavedConnections
        '
        Me.cmbSavedConnections.FormattingEnabled = True
        Me.cmbSavedConnections.Location = New System.Drawing.Point(15, 53)
        Me.cmbSavedConnections.Name = "cmbSavedConnections"
        Me.cmbSavedConnections.Size = New System.Drawing.Size(154, 21)
        Me.cmbSavedConnections.TabIndex = 6
        '
        'bttnAddConnection
        '
        Me.bttnAddConnection.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bttnAddConnection.Location = New System.Drawing.Point(175, 52)
        Me.bttnAddConnection.Name = "bttnAddConnection"
        Me.bttnAddConnection.Size = New System.Drawing.Size(50, 23)
        Me.bttnAddConnection.TabIndex = 7
        Me.bttnAddConnection.Text = "Add"
        Me.bttnAddConnection.UseVisualStyleBackColor = True
        '
        'bttnEditConnection
        '
        Me.bttnEditConnection.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bttnEditConnection.Location = New System.Drawing.Point(226, 52)
        Me.bttnEditConnection.Name = "bttnEditConnection"
        Me.bttnEditConnection.Size = New System.Drawing.Size(50, 23)
        Me.bttnEditConnection.TabIndex = 8
        Me.bttnEditConnection.Text = "Edit"
        Me.bttnEditConnection.UseVisualStyleBackColor = True
        '
        'bttnRemoveConnection
        '
        Me.bttnRemoveConnection.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bttnRemoveConnection.Location = New System.Drawing.Point(277, 52)
        Me.bttnRemoveConnection.Name = "bttnRemoveConnection"
        Me.bttnRemoveConnection.Size = New System.Drawing.Size(50, 23)
        Me.bttnRemoveConnection.TabIndex = 9
        Me.bttnRemoveConnection.Text = "Delete"
        Me.bttnRemoveConnection.UseVisualStyleBackColor = True
        '
        'frmManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(346, 305)
        Me.Controls.Add(Me.bttnRemoveConnection)
        Me.Controls.Add(Me.bttnEditConnection)
        Me.Controls.Add(Me.bttnAddConnection)
        Me.Controls.Add(Me.cmbSavedConnections)
        Me.Controls.Add(Me.bttnConnect)
        Me.Controls.Add(Me.gbxDBInformation)
        Me.Controls.Add(Me.lblDBConnection)
        Me.Controls.Add(Me.lblConnectionInfo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmManager"
        Me.Text = "MaNGOS Database Manager"
        Me.gbxDBInformation.ResumeLayout(False)
        Me.gbxDBInformation.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblConnectionInfo As System.Windows.Forms.Label
    Friend WithEvents lblDBConnection As System.Windows.Forms.Label
    Friend WithEvents gbxDBInformation As System.Windows.Forms.GroupBox
    Friend WithEvents lblPort As System.Windows.Forms.Label
    Friend WithEvents txtHost As System.Windows.Forms.TextBox
    Friend WithEvents lblHost As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtDatabase As System.Windows.Forms.TextBox
    Friend WithEvents lblDatabase As System.Windows.Forms.Label
    Friend WithEvents txtPass As System.Windows.Forms.TextBox
    Friend WithEvents lblPass As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents lblUser As System.Windows.Forms.Label
    Friend WithEvents bttnConnect As System.Windows.Forms.Button
    Friend WithEvents cmbSavedConnections As System.Windows.Forms.ComboBox
    Friend WithEvents bttnAddConnection As System.Windows.Forms.Button
    Friend WithEvents bttnEditConnection As System.Windows.Forms.Button
    Friend WithEvents bttnRemoveConnection As System.Windows.Forms.Button

End Class
