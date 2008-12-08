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

Option Explicit On
Option Strict On

Public Class frmLogging
    Private Sub frmLogging_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub frmLogging_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkLog.Checked = My.Settings.log
        txtFile.Text = My.Settings.logfile
        chkDontRun.Checked = My.Settings.dontrun
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If sfdLogFile.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFile.Text = sfdLogFile.FileName
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If chkLog.Checked Then
            If My.Computer.FileSystem.FileExists(txtFile.Text) Then
                Select Case MsgBox("The log file you chose is already exists. Do you want to append the queries (Yes) or clear the file (No)?", MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel, "Log File")
                    Case MsgBoxResult.No
                        My.Computer.FileSystem.WriteAllText(txtFile.Text, String.Empty, False)
                    Case MsgBoxResult.Cancel
                        Exit Sub
                End Select
            Else
                Try
                    My.Computer.FileSystem.WriteAllText(txtFile.Text, String.Empty, False)
                Catch ex As Exception
                    MsgBox("Error occured. Please check the log file you chose.", MsgBoxStyle.Exclamation, "Error")
                    Exit Sub
                End Try
            End If
        End If

        My.Settings.log = chkLog.Checked
        My.Settings.logfile = txtFile.Text
        My.Settings.dontrun = chkDontRun.Checked

        My.Settings.Save()

        MsgBox("Settings saved succesfully. Closing.", MsgBoxStyle.Information)
        Me.Close()
    End Sub

    Private Sub btnLogView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogView.Click
        Dim fileReader As New System.IO.StreamReader(My.Settings.logfile)
        Try
            txtLogContents.Text = fileReader.ReadToEnd()
        Catch Ex As Exception
            MsgBox("An error has occurred: " & Ex.Message & vbCrLf & _
                   "(1) Check if file exists and is readable " & _
                   "(2) Report this error to the devs ", MsgBoxStyle.Exclamation)
        End Try
        fileReader.Close()
    End Sub

    Private Sub btnLogSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogSave.Click
        Dim fileWriter As New System.IO.StreamWriter(My.Settings.logfile)
        Try
            fileWriter.Write(txtLogContents.Text)
        Catch Ex As Exception
            MsgBox("An error has occurred: " & Ex.Message & vbCrLf & _
                   "(1) Check if file exists and can be writable to " & _
                   "(2) Report this error to the devs ", MsgBoxStyle.Exclamation)
        End Try
        fileWriter.Close()
    End Sub
End Class