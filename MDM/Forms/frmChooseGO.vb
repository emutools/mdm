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