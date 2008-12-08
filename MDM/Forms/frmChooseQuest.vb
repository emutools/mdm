Option Explicit On
Option Strict On

Imports MySql.Data.MySqlClient

Public Class frmChooseQuest

    Public Field As MaskedTextBox = Nothing
    Public List As ListBox = Nothing

    Private Sub frmChooseQuest_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
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

        If mtbQuestID.Text = "" Then Exit Sub

        With frmManageMain
            Query = New MySqlCommand("SELECT " & _
                                        "`ZoneOrSort`, `SkillOrClass`, `MinLevel`, `QuestLevel`, `Type`, `RequiredRaces`, `RequiredSkillValue`, `RepObjectiveFaction`, `RepObjectiveValue`, `RequiredMinRepFaction`, `RequiredMinRepValue`, `RequiredMaxRepFaction`, `RequiredMaxRepValue`, " & _
                                        "`SuggestedPlayers`, `LimitTime`, `QuestFlags`, `SpecialFlags`, `CharTitleId`, `PrevQuestId`, `NextQuestId`, `ExclusiveGroup`, `NextQuestInChain`, `SrcItemId`, `SrcItemCount`, `SrcSpell`, `Title`, `Details`, `Objectives`, `OfferRewardText`, `RequestItemsText`, " & _
                                        "`EndText` ,`ObjectiveText1`, `ObjectiveText2`, `ObjectiveText3`, `ObjectiveText4`, `ReqItemId1`, `ReqItemId2`, `ReqItemId3`, `ReqItemId4`, `ReqItemCount1`, `ReqItemCount2`, `ReqItemCount3`, `ReqItemCount4`, `ReqCreatureOrGOId1`, `ReqCreatureOrGOId2`, `ReqCreatureOrGOId3`, `ReqCreatureOrGOId4`, " & _
                                        "`ReqCreatureOrGOCount1`, `ReqCreatureOrGOCount2`, `ReqCreatureOrGOCount3`, `ReqCreatureOrGOCount4`, `ReqSpellCast1`, `ReqSpellCast2`, `ReqSpellCast3`, `ReqSpellCast4`, `RewChoiceItemId1`, `RewChoiceItemId2`, `RewChoiceItemId3`, `RewChoiceItemId4`, `RewChoiceItemId5`, `RewChoiceItemId6`, " & _
                                        "`RewChoiceItemCount1`, `RewChoiceItemCount2`, `RewChoiceItemCount3`, `RewChoiceItemCount4`, `RewChoiceItemCount5`, `RewChoiceItemCount6`, `RewItemId1`, `RewItemId2`, `RewItemId3`, `RewItemId4`, `RewItemCount1`, `RewItemCount2`, `RewItemCount3`, `RewItemCount4`, `RewRepFaction1`, `RewRepFaction2`, `RewRepFaction3`, `RewRepFaction4`, `RewRepFaction5`, " & _
                                        "`RewRepValue1`, `RewRepValue2`, `RewRepValue3`, `RewRepValue4`, `RewRepValue5`, `RewOrReqMoney`, `RewMoneyMaxLevel`, `RewSpell`, `RewSpellCast`, `RewMailTemplateId`, `RewMailDelaySecs`, `IncompleteEmote`, `CompleteEmote` " & _
                                     " FROM `quest_template` WHERE `entry` = " & mtbQuestID.Text & ";", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                If Field IsNot Nothing Then
                    Reader.Close()
                    Field.Text = mtbQuestID.Text
                    Me.Close()
                ElseIf List IsNot Nothing Then
                    Reader.Read()
                    List.Items.Add(Reader.GetString("Title") & " [" & mtbQuestID.Text & "]")
                    Reader.Close()
                    Me.Close()
                Else
                    ' taken from ChrClasses.dbc - has to be negative for MaNGOS
                    Dim wowClass() As Integer = {-1, -2, -3, -4, -5, -7, -8, -9, -11}
                    ' taken from QuestSort.dbc - has to be negative for MaNGOS
                    Dim wowQuestSort() As Integer = {-1, -21, -23, -24, -25, -41, -61, -81, -82, -82, -101, -121, -141, -161, -162, -181, -182, -201, -221, -241, -261, -262, -263, -264, -284, -304, -324, -344, -364, -365, -366, -367, -368, -369, -370}

                    Reader.Read()
                    .mtbQuestEntry.Text = mtbQuestID.Text

                    If Array.IndexOf(wowQuestSort, Reader.GetInt32("ZoneOrSort")) = -1 Then
                        ' if it's not a sort, it's a zone
                        ComboChoose(Reader.GetInt32("ZoneOrSort"), .cmbQuestZoneID)
                        .cmbQuestSort.Enabled = False
                    Else
                        ' if it's not a zone, it's a sort
                        .cmbQuestSort.SelectedIndex = GetIndexNon(Reader.GetInt32("ZoneOrSort"), .cmbQuestSort)
                        .cmbQuestZoneID.Enabled = False
                    End If

                    If Array.IndexOf(wowClass, Reader.GetInt32("SkillOrClass")) = -1 Then
                        ' if it's not a class, it's a skill
                        .cmbQuestProffesion.SelectedIndex = GetIndexNon(Reader.GetInt32("SkillOrClass"), .cmbQuestProffesion)
                        .clbQuestClasses.Enabled = False
                    Else
                        ' else, it's a class, duhh =D
                        ChooseFlags(Reader.GetInt32("SkillOrClass"), .clbQuestClasses, True, True)
                        .cmbQuestProffesion.Enabled = False
                    End If

                    .nudQuestMinLevel.Value = Reader.GetInt32("MinLevel")
                    .nudQuestLevel.Value = Reader.GetInt32("QuestLevel")
                    .cmbQuestType.SelectedIndex = GetIndexNon(Reader.GetInt32("Type"), .cmbQuestType)
                    ChooseFlags(Reader.GetInt32("RequiredRaces"), .clbQuestRaces, True, True)
                    .nudSkillVal.Value = Reader.GetInt32("RequiredSkillValue")
                    ComboChoose(Reader.GetInt32("RepObjectiveFaction"), .cmbQuestObjFaction)
                    .nudQuestObjFaction.Value = Reader.GetInt64("RepObjectiveValue")

                    ComboChoose(Reader.GetInt32("RequiredMinRepFaction"), .cmbMinRepFaction)
                    .nudMinRepFaction.Value = Reader.GetInt32("RequiredMinRepValue")
                    ComboChoose(Reader.GetInt32("RequiredMaxRepFaction"), .cmbMaxRepFaction)
                    .nudMaxRepFaction.Value = Reader.GetInt32("RequiredMaxRepValue")

                    .nudSuggestedPlayers.Value = Reader.GetInt32("SuggestedPlayers")
                    .nudQuestLimit.Value = Reader.GetInt64("LimitTime")
                    ChooseFlags(Reader.GetInt32("QuestFlags"), .clbQuestFlags, False)
                    ChooseFlags(Reader.GetInt32("SpecialFlags"), .clbQuestSpecialFlags, True)
                    ComboChoose(Reader.GetInt32("CharTitleId"), .cmbCharTitleID)
                    .mtbQuestPrevQ.Text = CStr(Reader.GetInt64("PrevQuestId"))
                    .mtbQuestNextQ.Text = CStr(Reader.GetInt64("NextQuestId"))
                    .mtbQuestSource.Text = CStr(Reader.GetInt64("SrcItemID"))
                    .nudQuestSource.Value = Reader.GetInt64("SrcItemCount")
                    .mtbQuestSSource.Text = CStr(Reader.GetInt64("SrcSpell"))
                    .txtQuestTitle.Text = Reader.GetString("Title").Replace("$B", vbCrLf)
                    .txtQuestDetails.Text = Reader.GetString("Details").Replace("$B", vbCrLf)
                    .txtQuestObjectives.Text = Reader.GetString("Objectives").Replace("$B", vbCrLf)
                    Try
                        .txtQuestCompText.Text = Reader.GetString("OfferRewardText").Replace("$B", vbCrLf)
                    Catch f As Exception
                        .txtQuestCompText.Text = ""
                    End Try
                    Try
                        .txtQuestIncompText.Text = Reader.GetString("RequestItemsText").Replace("$B", vbCrLf)
                    Catch f As Exception
                        .txtQuestIncompText.Text = ""
                    End Try
                    Try
                        .txtQuestFinishText.Text = Reader.GetString("EndText").Replace("$B", vbCrLf)
                    Catch f As Exception
                        .txtQuestFinishText.Text = ""
                    End Try
                    .txtQuestObjText1.Text = Reader.GetString("ObjectiveText1")
                    .txtQuestObjText2.Text = Reader.GetString("ObjectiveText2")
                    .txtQuestObjText3.Text = Reader.GetString("ObjectiveText3")
                    .txtQuestObjText4.Text = Reader.GetString("ObjectiveText4")
                    .mtbQuestObj1.Text = Reader.GetString("ReqItemId1")
                    .mtbQuestObj2.Text = Reader.GetString("ReqItemId2")
                    .mtbQuestObj3.Text = Reader.GetString("ReqItemId3")
                    .mtbQuestObj4.Text = Reader.GetString("ReqItemId4")
                    .nudQuestObj1.Value = Reader.GetInt64("ReqItemCount1")
                    .nudQuestObj2.Value = Reader.GetInt64("ReqItemCount2")
                    .nudQuestObj3.Value = Reader.GetInt64("ReqItemCount3")
                    .nudQuestObj4.Value = Reader.GetInt64("ReqItemCount4")

                    If Reader.GetInt64("ReqCreatureOrGOId1") > 0 Then
                        .mtbQuestMob1.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId1"))
                        .chkQuestMob1.Checked = True
                    Else
                        .mtbQuestMob1.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId1"))
                        .chkQuestMob1.Checked = False
                    End If
                    If Reader.GetInt64("ReqCreatureOrGOId2") > 0 Then
                        .mtbQuestMob2.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId2"))
                        .chkQuestMob2.Checked = True
                    Else
                        .mtbQuestMob2.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId2"))
                        .chkQuestMob2.Checked = False
                    End If
                    If Reader.GetInt64("ReqCreatureOrGOId3") > 0 Then
                        .mtbQuestMob3.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId3"))
                        .chkQuestMob3.Checked = True
                    Else
                        .mtbQuestMob3.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId3"))
                        .chkQuestMob3.Checked = False
                    End If
                    If Reader.GetInt64("ReqCreatureOrGOId4") > 0 Then
                        .mtbQuestMob4.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId4"))
                        .chkQuestMob4.Checked = True
                    Else
                        .mtbQuestMob4.Text = CStr(Reader.GetInt64("ReqCreatureOrGOId4"))
                        .chkQuestMob4.Checked = False
                    End If

                    .nudQuestMob1.Value = Reader.GetInt64("ReqCreatureOrGOCount1")
                    .nudQuestMob2.Value = Reader.GetInt64("ReqCreatureOrGOCount2")
                    .nudQuestMob3.Value = Reader.GetInt64("ReqCreatureOrGOCount3")
                    .nudQuestMob4.Value = Reader.GetInt64("ReqCreatureOrGOCount4")
                    .mtbQuestReqSpell1.Text = CStr(Reader.GetInt32("ReqSpellCast1"))
                    .mtbQuestReqSpell2.Text = CStr(Reader.GetInt32("ReqSpellCast2"))
                    .mtbQuestReqSpell3.Text = CStr(Reader.GetInt32("ReqSpellCast3"))
                    .mtbQuestReqSpell4.Text = CStr(Reader.GetInt32("ReqSpellCast4"))
                    .mtbQuestItemChoice1.Text = CStr(Reader.GetInt64("RewChoiceItemId1"))
                    .mtbQuestItemChoice2.Text = CStr(Reader.GetInt64("RewChoiceItemId2"))
                    .mtbQuestItemChoice3.Text = CStr(Reader.GetInt64("RewChoiceItemId3"))
                    .mtbQuestItemChoice4.Text = CStr(Reader.GetInt64("RewChoiceItemId4"))
                    .mtbQuestItemChoice5.Text = CStr(Reader.GetInt64("RewChoiceItemId5"))
                    .mtbQuestItemChoice6.Text = CStr(Reader.GetInt64("RewChoiceItemId6"))
                    .nudQuestItemChoice1.Value = Reader.GetInt64("RewChoiceItemCount1")
                    .nudQuestItemChoice2.Value = Reader.GetInt64("RewChoiceItemCount2")
                    .nudQuestItemChoice3.Value = Reader.GetInt64("RewChoiceItemCount3")
                    .nudQuestItemChoice4.Value = Reader.GetInt64("RewChoiceItemCount4")
                    .nudQuestItemChoice5.Value = Reader.GetInt64("RewChoiceItemCount5")
                    .nudQuestItemChoice6.Value = Reader.GetInt64("RewChoiceItemCount6")
                    .mtbQuestIRew1.Text = CStr(Reader.GetInt64("RewItemId1"))
                    .mtbQuestIRew2.Text = CStr(Reader.GetInt64("RewItemId2"))
                    .mtbQuestIRew3.Text = CStr(Reader.GetInt64("RewItemId3"))
                    .mtbQuestIRew4.Text = CStr(Reader.GetInt64("RewItemId4"))
                    .nudQuestIRew1.Value = Reader.GetInt64("RewItemCount1")
                    .nudQuestIRew2.Value = Reader.GetInt64("RewItemCount2")
                    .nudQuestIRew3.Value = Reader.GetInt64("RewItemCount3")
                    .nudQuestIRew4.Value = Reader.GetInt64("RewItemCount4")
                    ComboChoose(Reader.GetInt32("RewRepFaction1"), .cmbQuestRepRew1)
                    ComboChoose(Reader.GetInt32("RewRepFaction2"), .cmbQuestRepRew2)
                    ComboChoose(Reader.GetInt32("RewRepFaction3"), .cmbQuestRepRew3)
                    ComboChoose(Reader.GetInt32("RewRepFaction4"), .cmbQuestRepRew4)
                    ComboChoose(Reader.GetInt32("RewRepFaction5"), .cmbQuestRepRew5)
                    .nudQuestRepRew1.Value = Reader.GetInt64("RewRepValue1")
                    .nudQuestRepRew2.Value = Reader.GetInt64("RewRepValue2")
                    .nudQuestRepRew3.Value = Reader.GetInt64("RewRepValue3")
                    .nudQuestRepRew4.Value = Reader.GetInt64("RewRepValue4")
                    .nudQuestRepRew5.Value = Reader.GetInt64("RewRepValue5")

                    If Reader.GetInt64("RewOrReqMoney") > 0 Then
                        .glcQuestGoldRew.SetValue(Reader.GetInt64("RewOrReqMoney"))
                    Else
                        .glcQuestGoldReq.SetValue(ToOppositeSign(Reader.GetInt64("RewOrReqMoney")))
                    End If
                    .glcQuestMaxLvl.SetValue(Reader.GetInt64("RewMoneyMaxLevel"))
                    .mtbQuestSpellRew.Text = CStr(Reader.GetInt64("RewSpell"))
                    .mtbQuestCast.Text = CStr(Reader.GetInt64("RewSpellCast"))

                    If Reader IsNot Nothing Then Reader.Close()

                    .DefaultValues(2).Clear()
                    .DefaultRecursive(.DefaultValues(2), .grpQuest)

                    Me.Close()
                End If
            Else
                If Reader IsNot Nothing Then Reader.Close()
                MsgBox("No quest found with this ID.", MsgBoxStyle.Information, "Quest Search")
            End If
        End With
    End Sub

    Private Sub frmQuestChooser_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        lstResults.Items.Clear()

        Query = New MySqlCommand("SELECT `entry`, `Title` FROM `quest_template` WHERE `Title` LIKE '%" & txtName.Text.Replace("'", "\'") & "%'" & CStr(IIf(chkLimit.Checked, " LIMIT 100", "")) & ";", Connection)
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
            mtbQuestID.Text = lstResults.SelectedItem.ToString.Substring(0, lstResults.SelectedItem.ToString.IndexOf(" - "))
        End If
    End Sub

    Private Sub frmChooseQuest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class