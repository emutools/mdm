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

Public Class frmChooseSpell
    Public Field As MaskedTextBox = Nothing

    Private Sub Search(ByVal Items() As String, ByVal Text As String, ByVal List As ListBox, ByVal Limit As Boolean)
        Dim I As Integer
        Dim Added As Integer = 0

        List.Items.Clear()

        For I = 0 To (Items.Length - 1)
            If Items(I).ToLower Like "*" & Text.ToLower & "*" Then
                List.Items.Add(Items(I))
                Added += 1
            End If

            If Added = 100 Then Exit For
        Next
    End Sub

    Private Sub frmChooseSpell_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                bttnSearch_Click(Nothing, Nothing)
            Else
                bttnProceed_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub frmChooseSpell_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                bttnSearch_Click(Nothing, Nothing)
            Else
                bttnProceed_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub frmChooseSpell_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub bttnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Search(My.Computer.FileSystem.ReadAllText("data\spells.dat").Split(vbCrLf.ToCharArray, System.StringSplitOptions.RemoveEmptyEntries), txtName.Text, lstResults, chkLimit.Checked)
    End Sub

    Private Sub Results_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResults.DoubleClick
        bttnProceed_Click(Nothing, Nothing)
    End Sub

    Private Sub lstResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstResults.SelectedIndexChanged
        Dim Temp As String

        If lstResults.SelectedItem IsNot Nothing Then
            Temp = lstResults.SelectedItem.ToString
            mtbSpellID.Text = Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1)
        End If
    End Sub

    Private Sub bttnProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnProceed.Click
        If mtbSpellID.Text = "" Then Exit Sub
        Field.Text = mtbSpellID.Text
        Me.Close()
    End Sub

    Private Sub frmChooseSpell_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class