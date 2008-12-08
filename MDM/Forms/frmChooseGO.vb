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

Public Class frmChooseGO
    Public Field As MaskedTextBox = Nothing
    Public List As ListBox

    Private Sub frmGOChooser_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                btnSearch_Click(Nothing, Nothing)
            Else
                btnChoose_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub btnChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnProceed.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If mtbGOID.Text = "" Then Exit Sub

        With frmManageMain
            Query = New MySqlCommand("SELECT " & _
                                            "`name`" & _
                                    " FROM `gameobject_template` WHERE `entry` = " & mtbGOID.Text & " LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                If Field IsNot Nothing Then
                    Reader.Close()
                    Field.Text = mtbGOID.Text
                    Me.Close()
                ElseIf List IsNot Nothing Then
                    Reader.Read()
                    List.Items.Add(Reader.GetString("name") & " [" & mtbGOID.Text & "]")
                    Reader.Close()
                    Me.Close()
                Else
                    Reader.Read()
                    If Reader IsNot Nothing Then Reader.Close()
                    Me.Close()
                End If
            Else
                If Reader IsNot Nothing Then Reader.Close()
                MsgBox("No GO found with this ID.", MsgBoxStyle.Information, "Gameobject Search")
            End If
        End With
    End Sub

    Private Sub frmGOChooser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        lstResults.Items.Clear()
        Query = New MySqlCommand("SELECT `entry`, `name` FROM `gameobject_template` WHERE `name` LIKE '%" & txtName.Text.Replace("'", "\'") & "%'" & CStr(IIf(chkLimit.Checked, " LIMIT 100", "")) & ";", Connection)
        Reader = Query.ExecuteReader()
        While (Reader.Read())
            lstResults.Items.Add(Reader.GetInt64(0) & " - " & Reader.GetString(1))
        End While
        If Reader IsNot Nothing Then Reader.Close()
    End Sub

    Private Sub lstResults_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResults.DoubleClick
        btnChoose_Click(Nothing, Nothing)
    End Sub

    Private Sub lstResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstResults.SelectedIndexChanged
        If lstResults.SelectedItem IsNot Nothing Then
            mtbGOID.Text = lstResults.SelectedItem.ToString.Substring(0, lstResults.SelectedItem.ToString.IndexOf(" - "))
        End If
    End Sub

    Private Sub frmChooseGO_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                btnSearch_Click(Nothing, Nothing)
            Else
                btnChoose_Click(Nothing, Nothing)
            End If
        End If
    End Sub
    Private Sub frmChooseGO_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class