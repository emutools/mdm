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
