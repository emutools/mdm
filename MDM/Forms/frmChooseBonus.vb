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

Public Class frmChooseBonus
    Public Field As MaskedTextBox = Nothing

    Private Sub frmBonusChooser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub lstResults_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResults.DoubleClick
        bttnProceed_Click(Nothing, Nothing)
    End Sub

    Private Sub lstResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstResults.SelectedIndexChanged
        Dim Temp As String

        If lstResults.SelectedItem IsNot Nothing Then
            Temp = lstResults.Items(lstResults.SelectedIndex).ToString
            mtbBonusID.Text = Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1)
        End If
    End Sub

    Private Sub txtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        ListBonuses(lstResults, txtName.Text, chkLimit.Checked)
    End Sub

    Private Sub frmBonusChooser_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.Enter Then
            bttnProceed_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub frmBonusChooser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim I As Integer
        Dim Temp As String

        mtbBonusID.Text = Field.Text

        ListBonuses(lstResults, "", False)

        For I = 0 To (lstResults.Items.Count - 1)
            Temp = lstResults.Items(I).ToString
            If Field.Text = Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) Then
                txtName.Text = Temp.Substring(0, Temp.IndexOf(" ["))
                Exit Sub
            End If
        Next

        ListBonuses(lstResults, "", chkLimit.Checked)
    End Sub

    Private Sub chkLimit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLimit.CheckedChanged
        ListBonuses(lstResults, txtName.Text, chkLimit.Checked)
    End Sub

    Private Sub bttnProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnProceed.Click
        Field.Text = mtbBonusID.Text
        Me.Close()
    End Sub
End Class