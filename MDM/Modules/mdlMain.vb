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

Module mdlMain

    'DB information is defined
    Public Host As String
    Public Username As String
    Public Password As String
    Public DB As String
    Public ItemStartEntry As Integer = 200000
    Public NPCStartEntry As Integer = 200000
    Public QuestStartEntry As Integer = 100000
    Public Port As Integer
    Public Updating As Boolean = False

    Public Connection As MySqlConnection

    Public Function ConnectToDB() As Boolean
        Try
            Connection = New MySqlConnection("server=" & Host & ";user id=" & Username & "; password=" & Password & "; port=" & Port & "; database=" & DB & "; pooling=false")
            Connection.Open()
        Catch
            Return False
        End Try

        Return True
    End Function

    Public Sub ComboChoose(ByVal ID As Integer, ByVal Combo As ComboBox)
        Dim Temp As Integer

        Temp = GetIndexMinus(ID, Combo)
        If Temp = -1 Then
            Combo.Text = CStr(ID)
        Else
            Combo.SelectedIndex = Temp
        End If
    End Sub

    Public Function ComboGet(ByVal Combo As ComboBox) As String
        If IsNumeric(Combo.Text) Then
            Return Combo.Text
        Else
            Return CStr(GetNumberFromIndex(Combo))
        End If
    End Function

    Public Function ToOppositeSign(ByVal Value As Integer) As Integer
        Return Value - (Value * 2)
    End Function

    Public Function ToOppositeSign(ByVal Value As Long) As Long
        Return Value - (Value * 2)
    End Function

    Public Sub ExecuteQuery(ByVal QueryString As String, Optional ByVal Update As Boolean = False, Optional ByVal Table As String = Nothing, Optional ByVal Entry As Integer = 0, Optional ByVal EntryField As String = "entry")
        Dim Query As MySqlCommand
        Dim Reader As MySqlDataReader
        Dim Data() As String = Nothing
        Dim I As Integer
        Dim ToLog As String = ""

        If Not My.Settings.dontrun Then
            If Update And My.Settings.log Then
                Query = New MySqlCommand("SELECT * FROM `" & Table & "` WHERE `" & EntryField & "` = " & Entry & " LIMIT 1;", Connection)
                Reader = Query.ExecuteReader()
                Reader.Read()

                ReDim Data(Reader.FieldCount - 1)
                For I = 0 To (Data.Length - 1)
                    If Reader.IsDBNull(I) Then
                        Data(I) = Nothing
                    Else
                        Data(I) = Reader.GetString(I)
                    End If
                Next

                Reader.Close()
            End If

            Query = New MySqlCommand(QueryString, Connection)
            Reader = Query.ExecuteReader()
            Reader.Close()

            If Update And My.Settings.log Then
                Query = New MySqlCommand("SELECT * FROM `" & Table & "` WHERE `" & EntryField & "` = " & Entry & " LIMIT 1;", Connection)
                Reader = Query.ExecuteReader()
                Reader.Read()

                For I = 0 To (Data.Length - 1)
                    If Reader.IsDBNull(I) Then
                        If Data(I) <> Nothing Then
                            If ToLog <> "" Then ToLog &= ", "
                            ToLog &= "`" & Reader.GetName(I) & "` = NULL"
                        End If
                    Else
                        If Data(I) <> Reader.GetString(I) Then
                            If ToLog <> "" Then ToLog &= ", "
                            ToLog &= "`" & Reader.GetName(I) & "` = '" & Reader.GetString(I).Replace("'", "\'") & "'"
                        End If
                    End If
                Next

                If ToLog <> "" Then
                    ToLog = "UPDATE `" & Table & "` SET " & ToLog & " WHERE `" & EntryField & "` = " & Entry & " LIMIT 1;"
                End If

                Reader.Close()
            End If
        End If

        If My.Settings.log Then
            If Not Update Then
                AddToLog(QueryString)
            ElseIf ToLog <> "" Then
                AddToLog(ToLog)
            Else
                If MsgBox("No changes has been made to table `" & Table & "`. Do you want to save this query to log anyway?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Update") = MsgBoxResult.Yes Then
                    AddToLog(QueryString)
                    MsgBox("Changes updated succesfully.", MsgBoxStyle.Information)
                End If
            End If
        End If
    End Sub

    Public Function AddToLog(ByVal Query As String) As Boolean
        Try
            My.Computer.FileSystem.WriteAllText(My.Settings.logfile, vbCrLf & Query, True)
            Return True
        Catch ex As Exception
            MsgBox("Error occured while writing to the log file. Check file path in ""Logging Settings"" window.", MsgBoxStyle.Critical, "Error")
            Return False
        End Try
    End Function

    Public Function Export(ByVal Table As String, ByVal Entry As Integer, ByRef Reader As MySqlDataReader) As QueryBuilder
        Dim I As Integer
        Dim Builder As QueryBuilder = New QueryBuilder

        Builder = New QueryBuilder
        For I = 0 To (Reader.FieldCount - 1)
            If Reader.IsDBNull(I) Then
                Builder.Add(Reader.GetName(I), Nothing)
            Else
                Builder.Add(Reader.GetName(I), Reader.GetString(I))
            End If
        Next
        Reader.Close()

        Return Builder
    End Function
End Module

