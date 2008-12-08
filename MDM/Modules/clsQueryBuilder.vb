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

Public Class QueryBuilder
    Dim Columns(), Values() As String
    Dim Added As Boolean = False

    Public Sub Add(ByVal Column As String, ByVal Value As String)
        Dim Position As Integer

        If Added Then
            Position = Columns.Length
            ReDim Preserve Columns(Position)
            ReDim Preserve Values(Position)
        Else
            ReDim Columns(0)
            ReDim Values(0)

            Position = 0
        End If

        Columns(Position) = Column
        Values(Position) = Value

        Added = True
    End Sub

    Public Function GetINSERT(ByVal Table As String) As String
        Dim I As Integer
        Dim qInto As String = ""
        Dim qValues As String = ""

        For I = 0 To (Columns.Length - 1)
            If I > 0 Then
                qInto &= ", "
                qValues &= ", "
            End If

            qInto &= "`" & Columns(I) & "`"

            If Values(I) Is Nothing Then
                qValues &= "NULL"
            Else
                qValues &= "'" & Values(I).Replace("\", "\\").Replace("'", "\'") & "'"
            End If
        Next

        Return String.Format("INSERT INTO `{0}` ({1}) VALUES ({2});", Table, qInto, qValues)
    End Function

    Public Sub RunINSERT(ByVal Table As String)
        ExecuteQuery(GetINSERT(Table))
    End Sub

    Public Function GetUPDATE(ByVal Table As String, ByVal EntryID As Integer, Optional ByVal EntryField As String = "entry") As String
        Dim I As Integer
        Dim qSet As String = ""

        For I = 0 To (Columns.Length - 1)
            If I > 0 Then
                qSet &= ", "
            End If

            If Values(I) Is Nothing Then
                qSet &= "`" & Columns(I) & "` = NULL"
            Else
                qSet &= "`" & Columns(I) & "` = '" & Values(I).Replace("\", "\\").Replace("'", "\'") & "'"
            End If
        Next

        Return String.Format("UPDATE `{0}` SET {1} WHERE `{2}` = {3} LIMIT 1;", Table, qSet, EntryField, EntryID)
    End Function

    Public Sub RunUPDATE(ByVal Table As String, ByVal EntryID As Integer, Optional ByVal EntryField As String = "entry")
        ExecuteQuery(GetUPDATE(Table, EntryID, EntryField), True, Table, EntryID, EntryField)
    End Sub
End Class
