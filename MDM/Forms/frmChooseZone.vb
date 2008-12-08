Public Class frmChooseZone

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

    Private Sub frmChooseZone_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                bttnSearch_Click(Nothing, Nothing)
            Else
                bttnProceed_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub frmChooseZone_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                bttnSearch_Click(Nothing, Nothing)
            Else
                bttnProceed_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub frmChooseZone_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub bttnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Search(My.Computer.FileSystem.ReadAllText("data\zones.dat").Split(vbCrLf.ToCharArray, System.StringSplitOptions.RemoveEmptyEntries), txtName.Text, lstResults, chkLimit.Checked)
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

    Private Sub frmChooseZone_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class