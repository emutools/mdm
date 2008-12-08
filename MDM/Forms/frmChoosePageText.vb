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

Imports MySql.Data.MySqlClient

Public Class frmChoosePageText
    Public Field As MaskedTextBox = Nothing

    Private Sub frmChoosePageText_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                bttnSearch_Click(Nothing, Nothing)
            Else
                btnChoose_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub btnChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnProceed.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If mtbPageID.Text = "" Then Exit Sub

        With frmManageMain
            Query = New MySqlCommand("SELECT `entry` FROM `page_text` WHERE `entry` = " & mtbPageID.Text & " LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                Reader.Close()
                Field.Text = mtbPageID.Text
                Me.Close()
            Else
                If Reader IsNot Nothing Then Reader.Close()
                MsgBox("No Page found with this ID.", MsgBoxStyle.Information, "Page Chooser")
            End If
        End With
    End Sub

    Private Sub frmPageChooser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub bttnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Text As String

        lstResults.Items.Clear()

        Query = New MySqlCommand("SELECT `entry`, `text` FROM `page_text` WHERE `text` LIKE '%" & txtName.Text.Trim.Replace("'", "\'") & "%'" & CStr(IIf(chkLimit.Checked, " LIMIT 100", "")) & ";", Connection)
        Reader = Query.ExecuteReader()
        While (Reader.Read())
            Text = Reader.GetString(1).Replace("$B", " ")
            If Text.Length > 50 Then
                Text = Text.Substring(0, 50) & "..."
            End If

            lstResults.Items.Add(Reader.GetInt64(0) & " - " & Text)
        End While
        If Reader IsNot Nothing Then Reader.Close()
    End Sub

    Private Sub lstResults_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResults.DoubleClick
        btnChoose_Click(Nothing, Nothing)
    End Sub

    Private Sub lstResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstResults.SelectedIndexChanged
        If lstResults.SelectedItem IsNot Nothing Then
            mtbPageID.Text = lstResults.SelectedItem.ToString.Substring(0, lstResults.SelectedItem.ToString.IndexOf(" - "))
        End If
    End Sub

    Private Sub frmChoosePageText_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class