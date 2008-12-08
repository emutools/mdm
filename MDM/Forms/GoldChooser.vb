Option Explicit On
Option Strict On

Public Class GoldChooser
    Public Event ValueChanged()

    Public Sub SetValue(ByVal Copper As Long)
        nudGold.Value = CInt(Copper / 10000)
        nudSilver.Value = CInt((Copper Mod 10000) / 100)
        nudCopper.Value = CInt(Copper Mod 100)
    End Sub

    Public Function GetValue() As Long
        Return CLng((nudCopper.Value + (nudSilver.Value * 100) + (nudGold.Value * 10000)))
    End Function

    Private Sub nudGold_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudGold.ValueChanged
        RaiseEvent ValueChanged()
    End Sub

    Private Sub nudSilver_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSilver.ValueChanged
        RaiseEvent ValueChanged()
    End Sub

    Private Sub nudCopper_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCopper.ValueChanged
        RaiseEvent ValueChanged()
    End Sub
End Class
