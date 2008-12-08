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
