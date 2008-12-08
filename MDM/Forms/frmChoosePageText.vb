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