Option Explicit On
Option Strict On

Imports MySql.Data.MySqlClient

Public Class frmChooseItem
    Public Field As MaskedTextBox = Nothing
    Public DisplayField As MaskedTextBox = Nothing
    Public Linker As Boolean = False
    Public List As ListBox

    ' Column Names & their ID according to their position in MaNGOS item_template table
    ' Implemented to prevent problems with structure changes & changes in position of 
    ' columns that are used in app
    ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' 
    ' Written in the order that it is used further in the code.
    ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' ' 

    Private Sub bttnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnSearch.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        lstResults.Items.Clear()
        Query = New MySqlCommand("SELECT `entry`, `name` FROM `item_template` WHERE `name` LIKE '%" & txtName.Text.Replace("'", "\'") & "%'" & CStr(IIf(chkLimit.Checked, " LIMIT 100", "")) & ";", Connection)
        Reader = Query.ExecuteReader()

        While (Reader.Read())
            lstResults.Items.Add(Reader.GetInt64(0) & " - " & Reader.GetString(1))
        End While

        If Reader IsNot Nothing Then Reader.Close()
    End Sub

    Private Sub frmChooseItem_EnterInfo(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown, lstResults.KeyDown
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
            mtbItemID.Text = lstResults.SelectedItem.ToString.Substring(0, lstResults.SelectedItem.ToString.IndexOf(" - "))
        End If
    End Sub

    Private Sub frmChooseItem_Exit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        frmManageMain.Enabled = True
    End Sub

    Private Sub bttnProceed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnProceed.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Item As ListViewItem

        If mtbItemID.Text = "" Then Exit Sub

        With frmManageMain
            'Select information for the Main part of the Item Editor form
            Query = New MySqlCommand("SELECT " & _
                                     "`class`, `subclass`, `name`, `displayid`, `Quality`, `Flags`, `BuyCount`, `BuyPrice`, `SellPrice`, `InventoryType`, `AllowableClass`, `AllowableRace`, `ItemLevel`, " & _
                                     "`RequiredLevel`, `RequiredSkill`, `RequiredSkillRank`, `requiredspell`, `requiredhonorrank`, `RequiredCityRank`, `RequiredReputationFaction`, `RequiredReputationRank`, `maxcount`, `stackable`, `ContainerSlots`, " & _
                                     "`stat_type1`, `stat_value1`, `stat_type2`, `stat_value2`, `stat_type3`, `stat_value3`, `stat_type4`, `stat_value4`, `stat_type5`, `stat_value5`, `stat_type6`, `stat_value6`, `stat_type7`, `stat_value7`, `stat_type8`, `stat_value8`, `stat_type9`, `stat_value9`, `stat_type10`, `stat_value10`, " & _
                                     "`dmg_min1`, `dmg_max1`, `dmg_type1`, `dmg_min2`, `dmg_max2`, `dmg_type2`, `dmg_min3`, `dmg_max3`, `dmg_type3`, `dmg_min4`, `dmg_max4`, `dmg_type4`, `dmg_min5`, `dmg_max5`, `dmg_type5`, " & _
                                     "`armor`, `holy_res`, `fire_res`, `nature_res`, `frost_res`, `shadow_res`, `arcane_res`, `delay`, `ammo_type`, `RangedModRange`, " & _
                                     "`spellid_1`, `spelltrigger_1`, `spellcharges_1`, `spellppmRate_1`, `spellcooldown_1`, `spellcategory_1`, `spellcategorycooldown_1`, `spellid_2`, `spelltrigger_2`, `spellcharges_2`, `spellppmRate_2`, `spellcooldown_2`, `spellcategory_2`, `spellcategorycooldown_2`, `spellid_3`, `spelltrigger_3`, `spellcharges_3`, `spellppmRate_3`, `spellcooldown_3`, `spellcategory_3`, `spellcategorycooldown_3`, `spellid_4`, `spelltrigger_4`, `spellcharges_4`, `spellppmRate_4`, `spellcooldown_4`, `spellcategory_4`, `spellcategorycooldown_4`, `spellid_5`, `spelltrigger_5`, `spellcharges_5`, `spellppmRate_5`, `spellcooldown_5`, `spellcategory_5`, `spellcategorycooldown_5`, " & _
                                     "`bonding`, `description`, `PageText`, `LanguageID`, `PageMaterial`, `startquest`, `lockid`, `Material`, `sheath`, `RandomProperty`, `RandomSuffix`, `block`, `itemset`, `MaxDurability`, `area`, `Map`, `BagFamily`, `TotemCategory`, `socketColor_1`, `socketContent_1`, `socketColor_2`, `socketContent_2`, `socketColor_3`, `socketContent_3`, `socketBonus`, `GemProperties`, `RequiredDisenchantSkill`, `ArmorDamageModifier`, `ScriptName`, `DisenchantID`, `FoodType`, `minMoneyLoot`, `maxMoneyLoot`, `Duration`" & _
                                     " FROM `item_template` WHERE `entry` = " & mtbItemID.Text & ";", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                If Field IsNot Nothing Then
                    Reader.Close()
                    Field.Text = mtbItemID.Text
                    Me.Close()
                ElseIf Linker Then
                    Reader.Read()
                    Item = frmManageMain.lsvLNVendors.Items.Add(mtbItemID.Text)
                    Item.SubItems.Add(Reader.GetString("name"))
                    Item.SubItems.Add(CStr(frmManageMain.nudLNMaxAmount.Value))
                    Item.SubItems.Add(CStr(frmManageMain.nudLNRestockTime.Value))
                    Item.SubItems.Add(frmManageMain.mtbLNExtendedCost.Text)
                    Reader.Close()
                    Me.Close()
                ElseIf List IsNot Nothing Then
                    Reader.Read()
                    List.Items.Add(Reader.GetString("name") & " [" & mtbItemID.Text & "]")
                    Reader.Close()
                    Me.Close()
                ElseIf DisplayField IsNot Nothing Then
                    Reader.Read()
                    DisplayField.Text = CStr(Reader.GetInt64("displayid"))
                    Reader.Close()
                    Me.Close()
                Else
                    Reader.Read()

                    .mtbItemEntry.Text = mtbItemID.Text
                    .cmbItemClass.SelectedIndex = Reader.GetInt32("class")
                    If .cmbItemSubclass.Items.Count > 0 Then
                        .cmbItemSubclass.SelectedIndex = Reader.GetInt32("subclass")
                    End If
                    .txtItemName.Text = Reader.GetString("name")
                    .mtbItemDisplay.Text = CStr(Reader.GetInt64("displayid"))
                    .cmbItemQuality.SelectedIndex = Reader.GetInt32("Quality")
                    .glcItemBuy.SetValue(Reader.GetInt64("BuyPrice"))
                    .glcItemSell.SetValue(Reader.GetInt64("SellPrice"))
                    ChooseFlags(Reader.GetInt32("Flags"), .clbItemFlags, False)
                    .nudItemLevel.Value = Reader.GetInt64("ItemLevel")
                    .nudItemSlots.Value = Reader.GetInt64("ContainerSlots")
                    .nudItemStack.Value = Reader.GetInt64("stackable")
                    .cmbItemBonding.SelectedIndex = Reader.GetInt32("bonding")

                    ' Stats Tab:
                    .rabItemStats1.Tag = Reader.GetInt64("stat_type1") & "|" & Reader.GetInt64("stat_value1")
                    .rabItemStats2.Tag = Reader.GetInt64("stat_type2") & "|" & Reader.GetInt64("stat_value2")
                    .rabItemStats3.Tag = Reader.GetInt64("stat_type3") & "|" & Reader.GetInt64("stat_value3")
                    .rabItemStats4.Tag = Reader.GetInt64("stat_type4") & "|" & Reader.GetInt64("stat_value4")
                    .rabItemStats5.Tag = Reader.GetInt64("stat_type5") & "|" & Reader.GetInt64("stat_value5")
                    .rabItemStats6.Tag = Reader.GetInt64("stat_type6") & "|" & Reader.GetInt64("stat_value6")
                    .rabItemStats7.Tag = Reader.GetInt64("stat_type7") & "|" & Reader.GetInt64("stat_value7")
                    .rabItemStats8.Tag = Reader.GetInt64("stat_type8") & "|" & Reader.GetInt64("stat_value8")
                    .rabItemStats2.Checked = True
                    .rabItemStats1.Checked = True

                    ' Weapon Tab:
                    .rabItemWeapon1.Tag = Reader.GetInt64("dmg_type1") & "|" & Reader.GetInt64("dmg_min1") & "|" & Reader.GetInt64("dmg_max1")
                    .rabItemWeapon2.Tag = Reader.GetInt64("dmg_type2") & "|" & Reader.GetInt64("dmg_min2") & "|" & Reader.GetInt64("dmg_max2")
                    .rabItemWeapon3.Tag = Reader.GetInt64("dmg_type3") & "|" & Reader.GetInt64("dmg_min3") & "|" & Reader.GetInt64("dmg_max3")
                    .rabItemWeapon4.Tag = Reader.GetInt64("dmg_type4") & "|" & Reader.GetInt64("dmg_min4") & "|" & Reader.GetInt64("dmg_max4")
                    .rabItemWeapon5.Tag = Reader.GetInt64("dmg_type5") & "|" & Reader.GetInt64("dmg_min5") & "|" & Reader.GetInt64("dmg_max5")
                    .rabItemWeapon2.Checked = True
                    .rabItemWeapon1.Checked = True
                    .mtbItemSpeed.Text = CStr(Reader.GetInt64("delay"))
                    .nudItemRange.Value = Reader.GetInt64("RangedModRange")

                    ' Armor Tab:
                    .nudItemHoly.Value = Reader.GetInt64("holy_res")
                    .nudItemFire.Value = Reader.GetInt64("fire_res")
                    .nudItemNature.Value = Reader.GetInt64("nature_res")
                    .nudItemFrost.Value = Reader.GetInt64("frost_res")
                    .nudItemShadow.Value = Reader.GetInt64("shadow_res")
                    .nudItemArcane.Value = Reader.GetInt64("arcane_res")
                    .mtbItemArmor.Text = CStr(Reader.GetInt64("armor"))
                    .mtbItemBlock.Text = CStr(Reader.GetInt64("block"))

                    ' Spells Tab:
                    .rabItemSpell1.Tag = CStr(CStr(IIf(Reader.GetInt64("spellid_1") = 0, 0, Reader.GetInt64("spellid_1"))) & "|" & Reader.GetInt64("spelltrigger_1") & "|" & Reader.GetInt64("spellcharges_1") & "|" & Reader.GetInt64("spellppmRate_1") & "|" & Reader.GetInt64("spellcooldown_1") & "|" & Reader.GetInt64("spellcategory_1") & "|" & Reader.GetInt64("spellcategorycooldown_1"))
                    .rabItemSpell2.Tag = CStr(CStr(IIf(Reader.GetInt64("spellid_2") = 0, 0, Reader.GetInt64("spellid_2"))) & "|" & Reader.GetInt64("spelltrigger_2") & "|" & Reader.GetInt64("spellcharges_2") & "|" & Reader.GetInt64("spellppmRate_2") & "|" & Reader.GetInt64("spellcooldown_2") & "|" & Reader.GetInt64("spellcategory_2") & "|" & Reader.GetInt64("spellcategorycooldown_2"))
                    .rabItemSpell3.Tag = CStr(CStr(IIf(Reader.GetInt64("spellid_3") = 0, 0, Reader.GetInt64("spellid_3"))) & "|" & Reader.GetInt64("spelltrigger_3") & "|" & Reader.GetInt64("spellcharges_3") & "|" & Reader.GetInt64("spellppmRate_3") & "|" & Reader.GetInt64("spellcooldown_3") & "|" & Reader.GetInt64("spellcategory_3") & "|" & Reader.GetInt64("spellcategorycooldown_3"))
                    .rabItemSpell4.Tag = CStr(CStr(IIf(Reader.GetInt64("spellid_4") = 0, 0, Reader.GetInt64("spellid_4"))) & "|" & Reader.GetInt64("spelltrigger_4") & "|" & Reader.GetInt64("spellcharges_4") & "|" & Reader.GetInt64("spellppmRate_4") & "|" & Reader.GetInt64("spellcooldown_4") & "|" & Reader.GetInt64("spellcategory_4") & "|" & Reader.GetInt64("spellcategorycooldown_4"))
                    .rabItemSpell5.Tag = CStr(CStr(IIf(Reader.GetInt64("spellid_5") = 0, 0, Reader.GetInt64("spellid_5"))) & "|" & Reader.GetInt64("spelltrigger_5") & "|" & Reader.GetInt64("spellcharges_5") & "|" & Reader.GetInt64("spellppmRate_5") & "|" & Reader.GetInt64("spellcooldown_5") & "|" & Reader.GetInt64("spellcategory_5") & "|" & Reader.GetInt64("spellcategorycooldown_5"))
                    .rabItemSpell2.Checked = True
                    .rabItemSpell1.Checked = True

                    ' Requirements Tab:
                    ChooseClasses(Reader.GetInt32("AllowableClass"))
                    ChooseRaces(Reader.GetInt32("AllowableRace"))
                    ComboChoose(Reader.GetInt32("RequiredSkill"), .cmbItemSkill)
                    .nudItemSkillRank.Value = Reader.GetInt64("RequiredSkillRank")
                    ComboChoose(Reader.GetInt32("RequiredReputationFaction"), .cmbItemFactions)
                    .cmbItemFactionStanding.SelectedIndex = Reader.GetInt32("RequiredReputationRank")
                    .nudItemReqLevel.Value = Reader.GetInt64("RequiredLevel")

                    ' Sockets Tab:
                    .cmbItemSocket1.SelectedIndex = GetIndex(Reader.GetInt32("socketColor_1"), .cmbItemSocket1)
                    .cmbItemSocket2.SelectedIndex = GetIndex(Reader.GetInt32("socketColor_2"), .cmbItemSocket2)
                    .cmbItemSocket3.SelectedIndex = GetIndex(Reader.GetInt32("socketColor_3"), .cmbItemSocket3)
                    .mtbItemBonus.Text = CStr(Reader.GetInt64("socketBonus"))

                    ' Text Tab:
                    .cmbPageMaterial.SelectedIndex = Reader.GetInt32("PageMaterial")
                    ComboChoose(Reader.GetInt32("LanguageID"), .cmbLanguageID)
                    .txtItemText.Text = Reader.GetString("description")
                    .mtbItemPage.Text = CStr(Reader.GetInt64("PageText"))

                    ' Misc Tab:
                    .cmbItemInventory.SelectedIndex = Reader.GetInt32("InventoryType")
                    .cmbItemSheath.SelectedIndex = Reader.GetInt32("sheath")
                    .cmbObjMaterial.SelectedIndex = Reader.GetInt32("Material")
                    ComboChoose(Reader.GetInt32("BagFamily"), .cmbBagFamily)
                    ComboChoose(Reader.GetInt32("TotemCategory"), .cmbTotemCategory)
                    ComboChoose(Reader.GetInt32("itemset"), .cmbItemSet)
                    .mtbItemQuest.Text = CStr(Reader.GetInt64("startquest"))
                    .nudItemDurability.Value = Reader.GetInt64("MaxDurability")

                    ' Extra's Tab:
                    .mtbReqDisenchantSkill.Text = CStr(Reader.GetInt32("RequiredDisenchantSkill"))
                    .cmbFoodType.SelectedIndex = Reader.GetInt32("FoodType")
                    .txtScriptName.Text = Reader.GetString("ScriptName")
                    .mtbDuration.Text = CStr(Reader.GetInt64("Duration"))

                    If Reader IsNot Nothing Then Reader.Close()

                    .DefaultValues(1).Clear()
                    .DefaultRecursive(.DefaultValues(1), .grpItems)

                    Me.Close()
                End If
            Else
                If Reader IsNot Nothing Then Reader.Close()
                MsgBox("No item found with this ID.", MsgBoxStyle.Information, "Item Search")
            End If
        End With
    End Sub

    Private Sub frmChooseItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class