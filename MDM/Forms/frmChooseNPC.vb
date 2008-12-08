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

Public Class frmChooseNPC
    Public Field As MaskedTextBox = Nothing
    Public DisplayField As MaskedTextBox = Nothing
    Public Linker As Boolean = False
    Public List As ListBox
    Public Male As Boolean

    Private Sub frmChooseNPCs_Exit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub bttnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        lstResults.Items.Clear()
        Query = New MySqlCommand("SELECT `entry`, `name` FROM `creature_template` WHERE `name` LIKE '%" & txtName.Text.Replace("'", "\'") & "%'" & CStr(IIf(chkLimit.Checked, " LIMIT 100", "")) & ";", Connection)
        Reader = Query.ExecuteReader()
        While (Reader.Read())
            lstResults.Items.Add(Reader.GetInt64(0) & " - " & Reader.GetString(1))
        End While
        If Reader IsNot Nothing Then Reader.Close()
    End Sub

    Private Sub frmChooseNPC_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
        If e.KeyData = Keys.Enter Then
            If txtName.Focused Then
                bttnSearch_Click(Nothing, Nothing)
            Else
                bttnProceed_Click(Nothing, Nothing)
            End If
        End If
    End Sub
    Private Sub lstResults_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstResults.DoubleClick
        bttnProceed_Click(Nothing, Nothing)
    End Sub

    Private Sub lstNPCResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstResults.SelectedIndexChanged
        If lstResults.SelectedItem IsNot Nothing Then
            mtbNPCID.Text = lstResults.SelectedItem.ToString.Substring(0, lstResults.SelectedItem.ToString.IndexOf(" - "))
        End If
    End Sub

    Private Sub bttnProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnProceed.Click
        Dim Info As MaskedTextBox = Nothing
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If mtbNPCID.Text = "" Then Exit Sub


        With frmManageMain
            'Query = New MySqlCommand("SELECT `name`, `subname`, `type`, `family`, `rank`, `modelid_A`, " & _
            '                         "`modelid_H`, `RacialLeader`, `minlevel`, `maxlevel`, `faction_A`, " & _
            '                         "`minhealth`, `maxhealth`, `minmana`, `maxmana`, `scale`, `npcflag`, " & _
            '                         "`dmgschool`, `InhabitType`, `MovementType`, " & _
            '                         "`baseattacktime`, `mindmg`, `maxdmg`, `speed`, `rangeattacktime`, " & _
            '                         "`minrangedmg`, `maxrangedmg`, `equipment_id`, `armor`, " & _
            '                         "`attackpower`, `resistance1`, `resistance2`, `resistance3`, " & _
            '                         "`resistance4`, `resistance5`, `resistance6`, `mingold`, `maxgold` " & _
            '                         "FROM `creature_template` WHERE `entry` = " & mtbNPCID.Text & " " & _
            '                         "LIMIT 1;", Connection)
            Query = New MySqlCommand("SELECT * FROM `creature_template` WHERE `entry` = " & mtbNPCID.Text & _
                                     " LIMIT 1", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                If Field IsNot Nothing Then
                    Reader.Close()
                    Field.Text = mtbNPCID.Text
                    Me.Close()
                ElseIf Info IsNot Nothing Then
                    Reader.Close()
                    Info.Text = .txtID.Text
                ElseIf List IsNot Nothing Then
                    Reader.Read()
                    List.Items.Add(Reader.GetString("name") & " [" & mtbNPCID.Text & "]")
                    Reader.Close()
                ElseIf DisplayField IsNot Nothing Then
                    Reader.Read()
                    DisplayField.Text = CStr(Reader.GetInt64(CStr(IIf(Male, "modelid_A", "modelid_H"))))
                    Reader.Close()
                Else
                    Reader.Read()
                    .txtID.Text = mtbNPCID.Text
                    .txtName.Text = Reader.GetString("name")

                    Try
                        .txtSubname.Text = Reader.GetString("subname")
                    Catch f As Exception
                        MsgBox("Null data was found, replacing with a blank value.", MsgBoxStyle.Information)
                        .txtSubname.Text = ""
                    End Try

                    'Main Tab
                    .cmbType.SelectedIndex = Reader.GetInt32("type")
                    .cmbFamily.SelectedIndex = FamilyToIndex(Reader.GetInt32("family"))
                    .cmbRank.SelectedIndex = Reader.GetInt32("rank")
                    .cmbMaleID.Text = CStr(Reader.GetInt64("modelid_A"))
                    .cmbFemID.Text = CStr(Reader.GetInt64("modelid_H"))
                    .nudMinLvl.Value = Reader.GetInt64("minlevel")
                    .nudMaxLvl.Value = Reader.GetInt64("maxlevel")
                    .mtbMinMana.Text = CStr(Reader.GetInt64("minmana"))
                    .mtbMaxMana.Text = CStr(Reader.GetInt64("maxmana"))
                    .mtbMinHp.Text = CStr(Reader.GetInt64("minhealth"))
                    .mtbMaxHp.Text = CStr(Reader.GetInt64("maxhealth"))

                    'Attack Tab
                    .mtbBaseAtkTime.Text = CStr(Reader.GetInt64("baseattacktime"))
                    .txtMinDmg.Text = CStr(Reader.GetDecimal("mindmg"))
                    .txtMaxDmg.Text = CStr(Reader.GetDecimal("maxdmg"))
                    .mtbRngAtkTime.Text = CStr(Reader.GetInt64("rangeattacktime"))
                    .txtMinRngDmg.Text = CStr(Reader.GetDecimal("minrangedmg"))
                    .txtMaxRngDmg.Text = CStr(Reader.GetDecimal("maxrangedmg"))
                    .txtAtkPwr.Text = CStr(Reader.GetInt64("attackpower"))
                    .cmbDamageSchool.SelectedIndex = CInt(Reader.GetInt64("dmgschool"))

                    'Armor Tab
                    .txtArmor.Text = CStr(Reader.GetInt64("armor"))
                    .txtHolyRes.Text = CStr(Reader.GetInt64("resistance1")) ' holy
                    .txtFireRes.Text = CStr(Reader.GetInt64("resistance2")) ' fire
                    .txtNatureRes.Text = CStr(Reader.GetInt64("resistance3")) ' nature
                    .txtFrostRes.Text = CStr(Reader.GetInt64("resistance4")) ' frost
                    .txtShadowRes.Text = CStr(Reader.GetInt64("resistance5")) ' shadow
                    .txtArcaneRes.Text = CStr(Reader.GetInt64("resistance6")) ' arcane

                    'Flags Tab
                    ChooseFlags(Reader.GetInt32("unit_flags"), .clbFlags, False)
                    ChooseFlags(Reader.GetInt32("npcflag"), .clbNPCFlags, False)
                    ChooseFlags(Reader.GetInt32("dynamicflags"), .clbDynamicFlags, False)
                    ChooseFlags(Reader.GetInt32("type_flags"), .clbExtraFlags, False)
                    ChooseFlags(Reader.GetInt32("flags_extra"), .clbAttributeFlags, False)
                    ChooseFlags(Reader.GetInt32("mechanic_immune_mask"), .clbImmuneFlags, False)


                    'Loot Tab
                    .mtbLootID.Text = CStr(Reader.GetInt32("lootid"))
                    .mtbSkinningLootID.Text = CStr(Reader.GetInt32("skinloot"))
                    .mtbPickpocketingLootID.Text = CStr(Reader.GetInt32("pickpocketloot"))

                    'Trainer Tab
                    .cmbTrainerType.SelectedIndex = Reader.GetInt32("trainer_type")
                    .mtbTrainerSpell.Text = CStr(Reader.GetInt32("trainer_spell"))
                    ComboChoose(Reader.GetInt32("class"), .cmbTrainerClass)
                    ComboChoose(Reader.GetInt32("race"), .cmbTrainerRace)

                    'Misc Tab
                    ComboChoose(Reader.GetInt32("faction_A"), .cmbFaction)
                    .txtScale.Text = CStr(Reader.GetDecimal("scale"))
                    .txtSpeed.Text = CStr(Reader.GetInt64("speed"))
                    .cmbInhabitType.SelectedIndex = Reader.GetInt32("InhabitType") - 1
                    If Not Reader.IsDBNull(Reader.GetOrdinal("RacialLeader")) Then
                        .chkLeader.Checked = Reader.GetBoolean("RacialLeader")
                    Else
                        .chkLeader.Checked = False
                    End If
                    If Reader.GetInt16("RegenHealth") <> 1 Then
                        .chkRegenHealth.Checked = False
                    Else
                        .chkRegenHealth.Checked = True
                    End If

                    'Other Tab
                    .glcMax.SetValue(Reader.GetInt64("maxgold"))
                    .glcMin.SetValue(Reader.GetInt64("mingold"))
                    .cmbMovementType.SelectedIndex = Reader.GetInt32("MovementType")
                    .mtbEquip.Text = CStr(Reader.GetInt64("equipment_id"))

                    Reader.Close()

                    .DefaultValues(0).Clear()
                    .DefaultRecursive(.DefaultValues(0), .grpNPC)
                    frmManageMain.Enabled = True
                    Me.Close()

                End If
            Else
                Reader.Close()
                MsgBox("No NPC found with this ID.", MsgBoxStyle.Information, "NPC Search")
            End If
            Me.Close()
            Reader.Close()
        End With

    End Sub

    Private Sub frmChooseNPC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class