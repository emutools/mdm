Option Explicit On
Option Strict On

Imports MySql.Data.MySqlClient
Imports System.IO

Public Class frmManageMain

#Region " Defines & Functions "
    Public DefaultValues(2) As Collection
    Public Linker As Boolean = False

    Public Info As MaskedTextBox = Nothing
    Public DisplayField As MaskedTextBox = Nothing
    Public Male As Boolean
    Public List As ListBox = Nothing

    Private ClearValues(2) As Collection
    Private LinkerTable As String
    Private EntryEditing As Integer
    Private Adding As Boolean

    Private Enum Type
        Textbox
        MaskedTextBox
        CheckedListBox
        ComboBox
        Numric
        CheckBox
        RadioBox
    End Enum

    Private Structure Field
        Public FieldType As Type
        Public Container As Control
        Public Name, Text As String
        Public Tag As Object
        Public Value As Decimal
        Public Checked, CheckedList() As Boolean
    End Structure

    Private Function GetStringNoNull(ByVal Text As String) As String
        Return Text
    End Function

    Public Sub DefaultRecursive(ByVal vCollection As Collection, ByVal Container As System.Windows.Forms.Control)
        Dim I, X As Integer
        Dim NewField As Field = New Field

        For I = 0 To (Container.Controls.Count - 1)
            With Container
                NewField.Name = .Controls(I).Name
                NewField.Tag = .Controls(I).Tag
                NewField.Text = .Controls(I).Text
                NewField.Container = Container

                Select Case True
                    Case TypeOf (.Controls(I)) Is TextBox
                        NewField.FieldType = Type.Textbox
                        vCollection.Add(NewField)
                    Case TypeOf (.Controls(I)) Is MaskedTextBox
                        NewField.FieldType = Type.MaskedTextBox
                        vCollection.Add(NewField)
                    Case TypeOf (.Controls(I)) Is ComboBox
                        NewField.FieldType = Type.ComboBox
                        NewField.Value = CType(.Controls(I), ComboBox).SelectedIndex
                        vCollection.Add(NewField)
                    Case TypeOf (.Controls(I)) Is CheckedListBox
                        NewField.FieldType = Type.CheckedListBox
                        ReDim NewField.CheckedList(CType(.Controls(I), CheckedListBox).Items.Count - 1)

                        For X = 0 To (CType(.Controls(I), CheckedListBox).Items.Count - 1)
                            NewField.CheckedList(X) = CType(.Controls(I), CheckedListBox).GetItemChecked(X)
                        Next

                        vCollection.Add(NewField)
                    Case TypeOf (.Controls(I)) Is NumericUpDown
                        NewField.FieldType = Type.Numric
                        NewField.Value = CType(.Controls(I), NumericUpDown).Value
                        vCollection.Add(NewField)
                    Case TypeOf (.Controls(I)) Is CheckBox
                        NewField.FieldType = Type.CheckBox
                        NewField.Checked = CType(.Controls(I), CheckBox).Checked
                        vCollection.Add(NewField)
                    Case TypeOf (.Controls(I)) Is RadioButton
                        NewField.FieldType = Type.RadioBox
                        NewField.Checked = CType(.Controls(I), RadioButton).Checked
                        vCollection.Add(NewField)
                    Case Else
                        DefaultRecursive(vCollection, .Controls(I))
                End Select
            End With
        Next
    End Sub

    Private Sub RestoreDefaults(ByVal vCollection As Collection, Optional ByVal AddSubclass As Boolean = False)
        Dim I, X, Max, Subclass As Integer
        Dim Done As Boolean = False
        Dim eField As Field
        Dim eControl As Control

        Max = (vCollection.Count - 1)
        I = 1
        While I <= Max
Iteration:
            eField = CType(vCollection(I), Field)
            eControl = eField.Container.Controls(eField.Name)

            eControl.Tag = eField.Tag
            eControl.Text = eField.Text

            If eField.Name <> "cmbItemSubclass" Then
                With eField
                    Select Case .FieldType
                        Case Type.Textbox
                        Case Type.MaskedTextBox
                        Case Type.ComboBox
                            CType(eControl, ComboBox).SelectedIndex = CInt(.Value)
                        Case Type.CheckedListBox
                            Updating = True
                            For X = 0 To (.CheckedList.Length - 1)
                                CType(eControl, CheckedListBox).SetItemChecked(X, .CheckedList(X))
                            Next
                            Updating = True
                        Case Type.Numric
                            CType(eControl, NumericUpDown).Value = .Value
                        Case Type.CheckBox
                            CType(eControl, CheckBox).Checked = .Checked
                        Case Type.RadioBox
                            CType(eControl, RadioButton).Checked = .Checked
                    End Select
                End With
            Else
                Subclass = I
            End If

            I += 1
        End While

        If AddSubclass And Not Done Then
            Done = True
            Max = Subclass
            I = Subclass
            GoTo Iteration
        End If
    End Sub

    Private Sub UpdateDPS(ByVal MinField As Control, ByVal MaxField As Control, ByVal SpeedField As MaskedTextBox, ByVal DPSLabel As Label, ByVal Text As String)
        Try
            Dim Min As Integer = CInt(IIf(MinField.Text = "", 0, MinField.Text))
            Dim Max As Integer = CInt(IIf(MaxField.Text = "", 0, MaxField.Text))
            Dim Speed As Integer = CInt(IIf(SpeedField.Text = "", 0, SpeedField.Text))

            DPSLabel.Text = Text & ": " & Math.Round(((Min + Max) / 2) * (1000 / Speed))
        Catch e As Exception
            DPSLabel.Text = Text & ": NaN"
        End Try
    End Sub

    Private Function GetItemName(ByVal ID As Long) As String
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Temp As String

        Query = New MySqlCommand("SELECT `name` FROM `item_template` WHERE `entry` = " & ID & " LIMIT 1;", Connection)
        Reader = Query.ExecuteReader()

        If Reader.HasRows Then
            Reader.Read()
            Temp = Reader.GetString(0)
            Reader.Close()

            Return Temp
        End If

        If Reader.IsClosed = False Then
            Reader.Close()
        End If

        Return Nothing
    End Function

    Private Function GetNPCName(ByVal ID As Long) As String
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Temp As String

        Query = New MySqlCommand("SELECT `name` FROM `creature_template` WHERE `entry` = " & ID & " LIMIT 1;", Connection)
        Reader = Query.ExecuteReader()

        If Reader.HasRows Then
            Reader.Read()
            Temp = Reader.GetString(0)
            Reader.Close()

            Return Temp
        End If

        Return Nothing
    End Function

    Private Function GetGOName(ByVal ID As Long) As String
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Temp As String

        Query = New MySqlCommand("SELECT `name` FROM `gameobject_template` WHERE `entry` = " & ID & " LIMIT 1;", Connection)
        Reader = Query.ExecuteReader()

        If Reader.HasRows Then
            Reader.Read()
            Temp = Reader.GetString(0)
            Reader.Close()

            Return Temp
        End If

        Return Nothing
    End Function

    Private Function GetQuestName(ByVal ID As Long) As String
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Temp As String

        Query = New MySqlCommand("SELECT `Title` FROM `quest_template` WHERE `entry` = " & ID & " LIMIT 1;", Connection)
        Reader = Query.ExecuteReader()

        If Reader.HasRows Then
            Reader.Read()
            Temp = Reader.GetString(0)
            Reader.Close()

            Return Temp
        End If

        Return Nothing
    End Function

    Private Function GetItemID(ByVal ItemName As String) As Integer
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim ID As Integer

        Query = New MySqlCommand("SELECT `entry` FROM `item_template` WHERE `name`=" & ItemName & ";", Connection)
        Reader = Query.ExecuteReader()

        If Reader.HasRows Then
            Reader.Read()
            ID = CInt(Reader.GetString(0))
            Reader.Close()

            Return ID
        End If

        Return Nothing
    End Function
#End Region

#Region " Global Form Functions "

    Private Sub frmManageMain_Exit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.FormClosing
        Application.Exit()
    End Sub

    Private Sub frmManageMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tabNPCFlags.VerticalScroll.Enabled = True
        ' get absolute directory
        Dim fullPath = Path.GetFullPath(Directory.GetCurrentDirectory())
        Dim dataPath As String = fullPath + "\data\"
        ' if we dont have our data directory, disable items right away
        If Directory.Exists(dataPath) = False Then
            MsgBox("Data Directory was not found, please make sure it is there and that files can be loaded", MsgBoxStyle.Critical)
            mtbItemBonus.Enabled = False
            btnItemChooseBonus.Enabled = False
            mtbItemSpellID.Enabled = False
            btnItemSpell.Enabled = False
            If radLLFishing.Checked Then
                mtbLLID.Enabled = False
                btnLLChoose.Enabled = False
            End If
            cmbQuestZoneID.Enabled = False
        Else
            ' check to see if necessary data files are located
            ' if not, then disable forms that use them
            If File.Exists(dataPath + "bonuses.dat") = False Then
                MsgBox("BONUS.DAT not found, Socket Bonus chooser disabled", MsgBoxStyle.Critical)
                mtbItemBonus.Enabled = False
                btnItemChooseBonus.Enabled = False
            ElseIf File.Exists(dataPath + "spells.dat") = False Then
                MsgBox("SPELLS.DAT not found, Spell Chooser disabled", MsgBoxStyle.Critical)
                mtbItemSpellID.Enabled = False
                btnItemSpell.Enabled = False
            ElseIf File.Exists(dataPath + "spells.dat") = False Then
                MsgBox("ZONES.DAT not found, Fishing Loot Editing disabled", MsgBoxStyle.Critical)
                cmbQuestZoneID.Enabled = False
                If radLLFishing.Checked Then
                    mtbLLID.Enabled = False
                    btnLLChoose.Enabled = False
                End If
            End If
        End If

        ClearValues(0) = New Collection
        DefaultValues(0) = New Collection
        DefaultRecursive(ClearValues(0), grpNPC)
        DefaultRecursive(DefaultValues(0), grpNPC)

        ClearValues(1) = New Collection
        DefaultValues(1) = New Collection
        DefaultRecursive(ClearValues(1), grpItems)
        DefaultRecursive(DefaultValues(1), grpItems)

        ClearValues(2) = New Collection
        DefaultValues(2) = New Collection
        DefaultRecursive(ClearValues(2), grpQuest)
        DefaultRecursive(DefaultValues(2), grpQuest)

        LQUnediting()
        LNUnediting()
        LLUnediting()

        For I = 1 To 8
            tapItemStats.Controls("rabItemStats" & I).Tag = New Object
            tapItemStats.Controls("rabItemStats" & I).Tag = "0|0"
        Next I

        For I = 1 To 5
            tapItemWeapon.Controls("rabItemWeapon" & I).Tag = New Object
            tapItemWeapon.Controls("rabItemWeapon" & I).Tag = "0|0|0"
        Next I

        For I = 1 To 5
            tapItemSpells.Controls("rabItemSpell" & I).Tag = New Object
            tapItemSpells.Controls("rabItemSpell" & I).Tag = "0|0|0|0|0|0"
        Next I
        cmbItemClass.SelectedIndex = 0
        cmbItemQuality.SelectedIndex = 0
        cmbItemBonding.SelectedIndex = 0
        cmbItemFactions.SelectedIndex = 0
        cmbItemSet.SelectedIndex = 0
        rabItemStats1.Checked = True
        rabItemWeapon1.Checked = True
        rabItemSpell1.Checked = True
        cmbItemInventory.SelectedIndex = 0
        cmbItemFactionStanding.SelectedIndex = 0
        cmbItemSheath.SelectedIndex = 0
        clbItemClasses.SetItemChecked(0, True)
        clbItemRaces.SetItemChecked(0, True)
        cmbItemSocket1.SelectedIndex = 0
        cmbItemSocket2.SelectedIndex = 0
        cmbItemSocket3.SelectedIndex = 0
        cmbItemTrigger.SelectedIndex = 0
        cmbItemSkill.SelectedIndex = 0
        clbItemClasses.SetItemChecked(0, True)
        clbItemRaces.SetItemChecked(0, True)

        cmbQuestProffesion.SelectedIndex = 0
        cmbQuestSort.SelectedIndex = 0
        cmbQuestType.SelectedIndex = 0
        cmbQuestRepRew1.SelectedIndex = 0
        cmbQuestRepRew2.SelectedIndex = 0
        cmbQuestRepRew3.SelectedIndex = 0
        cmbQuestRepRew4.SelectedIndex = 0
        cmbQuestRepRew5.SelectedIndex = 0
        cmbMinRepFaction.SelectedIndex = 0
        cmbMaxRepFaction.SelectedIndex = 0
        If cmbQuestZoneID.Enabled = False Then
            FillWithZones(cmbQuestZoneID)
        End If
        clbQuestClasses.SetItemChecked(0, True)
        clbQuestRaces.SetItemChecked(0, True)
        clbQuestFlags.SetItemChecked(0, True)
        clbQuestSpecialFlags.SetItemChecked(0, True)
    End Sub

    Public Function GetValue(ByVal Text As String) As Decimal
        Text = Text.Replace(" ", "")

        If (Text = ".") Or (Text = "") Then
            Return 0
        Else
            Return CDec(Text)
        End If
    End Function

    Public Function CallSearch(ByVal SearchForm As String) As String
        Me.Enabled = False

        If SearchForm = "Item" Then
            frmChooseItem.Show(Me)
        ElseIf SearchForm = "NPC" Then
            frmChooseNPC.Show(Me)
        ElseIf SearchForm = "Spell" Then
            frmChooseSpell.Show(Me)
        ElseIf SearchForm = "Quest" Then
            frmChooseQuest.Show(Me)
        ElseIf SearchForm = "Bonus" Then
            frmChooseBonus.Show(Me)
        ElseIf SearchForm = "Page" Then
            frmChoosePageText.Show(Me)
        ElseIf SearchForm = "GO" Then
            frmChooseGO.Show(Me)
        End If

        Return Nothing
    End Function

    Public Function CallSearch(ByVal SearchForm As String, ByVal Field As MaskedTextBox) As String
        Me.Enabled = False

        If SearchForm = "Item" Then
            frmChooseItem.Field = Field
            frmChooseItem.Show(Me)
        ElseIf SearchForm = "NPC" Then
            frmChooseNPC.Field = Field
            frmChooseNPC.Show(Me)
        ElseIf SearchForm = "Spell" Then
            frmChooseSpell.Field = Field
            frmChooseSpell.Show(Me)
        ElseIf SearchForm = "Quest" Then
            frmChooseQuest.Field = Field
            frmChooseQuest.Show(Me)
        ElseIf SearchForm = "Bonus" Then
            frmChooseBonus.Field = Field
            frmChooseBonus.Show(Me)
        ElseIf SearchForm = "Page" Then
            frmChoosePageText.Field = Field
            frmChoosePageText.Show(Me)
        ElseIf SearchForm = "GO" Then
            frmChooseGO.Field = Field
            frmChooseGO.Show(Me)
        End If

        Return Nothing
    End Function

    Public Sub CallSearchDisplay(ByVal SearchForm As String, ByVal Field As MaskedTextBox)
        If SearchForm = "Item" Then
            frmChooseItem.DisplayField = Field
            frmChooseItem.Show(Me)
        End If
        If SearchForm = "NPC" Then
            frmChooseNPC.DisplayField = Field
            frmChooseNPC.Show(Me)
        End If
    End Sub
#End Region

#Region " Home Tab "
    Private Sub btnLog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLog.Click
        Me.Enabled = False
        frmLogging.Show()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.mangosdbmanager.wordpress.com/")
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("http://www.mangosproject.org/")
    End Sub
#End Region

#Region " Item Tab "

    Private Sub btnItemPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemPage.Click
        CallSearch("Page", mtbItemPage)
    End Sub

    Private Sub cmbItemSubclass_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        tlpTip.ToolTipIcon = ToolTipIcon.Info
        tlpTip.ToolTipTitle = "Note:"
        tlpTip.UseFading = True
        tlpTip.UseAnimation = True
        tlpTip.Show("To have a weapon/armor working perfectly change ""Inventory Type"" in ""Miscellaneous"" tab to the correct value.", cmbItemSubclass, 5000)
    End Sub

    Private Sub btnChooseItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseItem.Click
        CallSearch("Item")
    End Sub

    Private Sub btnItemsClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemsClear.Click
        RestoreDefaults(ClearValues(1), True)
    End Sub

    Private Sub btnItemsDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemsDefaults.Click
        RestoreDefaults(DefaultValues(1), True)
    End Sub

    Private Sub cmbItemClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemClass.SelectedIndexChanged
        FillSubclasses(cmbItemClass.SelectedIndex)
    End Sub

    Private Sub btnItemDisplay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemDisplay.Click
        CallSearchDisplay("Item", mtbItemDisplay)
    End Sub

    Private Sub btnItemChooseBonus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemChooseBonus.Click
        CallSearch("Bonus", mtbItemBonus)
    End Sub

#Region " StatsTab "
    Private Sub rabItemStats_CheckedChanged(ByVal Button As RadioButton)
        If Button.Checked = True Then
            If Button.Checked Then
                cmbItemStats.SelectedIndex = GetIndex(CInt(CStr(Button.Tag).Split(CChar("|"))(0)), cmbItemStats)
                nudItemStat.Value = CDec(CStr(Button.Tag).Split(CChar("|"))(1))
            End If
        End If
    End Sub

    Private Sub cmbItemStats_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemStats.Leave
        Dim I As Integer
        Dim StatsRadio As RadioButton

        For I = 1 To 8
            StatsRadio = CType(tapItemStats.Controls("rabItemStats" & I), RadioButton)
            If StatsRadio.Checked = True Then
                StatsRadio.Tag = GetNumberFromIndex(cmbItemStats) & "|" & CStr(StatsRadio.Tag).Split(CChar("|"))(1)
            End If
        Next
    End Sub

    Private Sub nudItemStat_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudItemStat.Leave
        Dim I As Integer
        Dim StatsRadio As RadioButton

        For I = 1 To 8
            StatsRadio = CType(tapItemStats.Controls("rabItemStats" & I), RadioButton)
            If StatsRadio.Checked = True Then
                StatsRadio.Tag = CStr(StatsRadio.Tag).Split(CChar("|"))(0) & "|" & nudItemStat.Text
            End If
        Next
    End Sub

    Private Sub rabItemStats1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats1.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats1)
    End Sub
    Private Sub rabItemStats2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats2.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats2)
    End Sub
    Private Sub rabItemStats3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats3.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats3)
    End Sub
    Private Sub rabItemStats4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats4.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats4)
    End Sub
    Private Sub rabItemStats5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats5.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats5)
    End Sub
    Private Sub rabItemStats6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats6.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats6)
    End Sub
    Private Sub rabItemStats7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats7.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats7)
    End Sub
    Private Sub rabItemStats8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemStats8.CheckedChanged
        rabItemStats_CheckedChanged(rabItemStats8)
    End Sub

#End Region

#Region " Weapon Tab "
    Private Sub mtbItemDamageSpeed_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemDamageMin.TextChanged, mtbItemDamageMax.TextChanged, mtbItemSpeed.TextChanged
        UpdateDPS(mtbItemDamageMin, mtbItemDamageMax, mtbItemSpeed, lblItemDPS, "DPS")
    End Sub

    Private Sub rabItemWeapon_CheckedChanged(ByVal Button As RadioButton)
        If Button.Checked = True Then
            If Button.Checked Then
                cmbItemDamageType.SelectedIndex = CInt(CStr(Button.Tag).Split(CChar("|"))(0))
                mtbItemDamageMin.Text = CStr(Button.Tag).Split(CChar("|"))(1)
                mtbItemDamageMax.Text = CStr(Button.Tag).Split(CChar("|"))(2)
            End If
        End If
    End Sub

    Private Sub cmbItemDamageType_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemDamageType.Leave
        Dim I As Integer
        Dim WeaponRadio As RadioButton

        For I = 1 To 5
            WeaponRadio = CType(tapItemWeapon.Controls("rabItemWeapon" & I), RadioButton)
            If WeaponRadio.Checked = True Then
                WeaponRadio.Tag = cmbItemDamageType.SelectedIndex & "|" & CStr(WeaponRadio.Tag).Split(CChar("|"))(1) & "|" & CStr(WeaponRadio.Tag).Split(CChar("|"))(2)
            End If
        Next
    End Sub

    Private Sub mtbItemDamageMin_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemDamageMin.Leave
        Dim I As Integer
        Dim WeaponRadio As RadioButton

        For I = 1 To 5
            WeaponRadio = CType(tapItemWeapon.Controls("rabItemWeapon" & I), RadioButton)
            If WeaponRadio.Checked = True Then
                WeaponRadio.Tag = CStr(WeaponRadio.Tag).Split(CChar("|"))(0) & "|" & mtbItemDamageMin.Text & "|" & CStr(WeaponRadio.Tag).Split(CChar("|"))(2)
            End If
        Next
    End Sub

    Private Sub mtbItemDamageMax_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemDamageMax.Leave
        Dim I As Integer
        Dim WeaponRadio As RadioButton

        For I = 1 To 5
            WeaponRadio = CType(tapItemWeapon.Controls("rabItemWeapon" & I), RadioButton)
            If WeaponRadio.Checked = True Then
                WeaponRadio.Tag = CStr(WeaponRadio.Tag).Split(CChar("|"))(0) & "|" & CStr(WeaponRadio.Tag).Split(CChar("|"))(1) & "|" & mtbItemDamageMax.Text
            End If
        Next
    End Sub

    Private Sub rabItemWeapon1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemWeapon1.CheckedChanged
        rabItemWeapon_CheckedChanged(rabItemWeapon1)
    End Sub
    Private Sub rabItemWeapon2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemWeapon2.CheckedChanged
        rabItemWeapon_CheckedChanged(rabItemWeapon2)
    End Sub
    Private Sub rabItemWeapon3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemWeapon3.CheckedChanged
        rabItemWeapon_CheckedChanged(rabItemWeapon3)
    End Sub
    Private Sub rabItemWeapon4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemWeapon4.CheckedChanged
        rabItemWeapon_CheckedChanged(rabItemWeapon4)
    End Sub
    Private Sub rabItemWeapon5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemWeapon5.CheckedChanged
        rabItemWeapon_CheckedChanged(rabItemWeapon5)
    End Sub
#End Region

#Region " Spells Tab "
    Private Sub btnItemSpell_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemSpell.Click
        CallSearch("Spell", mtbItemSpellID)
    End Sub

    Private Sub rabItemSpell_CheckedChanged(ByVal Button As RadioButton)
        If Button.Checked = True Then
            If Button.Checked And Button.Tag IsNot Nothing Then
                mtbItemSpellID.Text = CStr(Button.Tag).Split(CChar("|"))(0)
                cmbItemTrigger.SelectedIndex = CInt(CStr(Button.Tag).Split(CChar("|"))(1))
                nudItemCharges.Value = CDec(IIf(CDec(CStr(Button.Tag).Split(CChar("|"))(2)) > -1, CDec(CStr(Button.Tag).Split(CChar("|"))(2)), 0))
                mtbItemPPMRate.Text = CStr(Button.Tag).Split(CChar("|"))(3)
                mtbItemCooldown.Text = CStr(Button.Tag).Split(CChar("|"))(4)
                mtbItemCategory.Text = CStr(Button.Tag).Split(CChar("|"))(5)
                mtbItemCategoryCooldown.Text = CStr(Button.Tag).Split(CChar("|"))(6)
            End If
        End If
    End Sub

    Private Sub mtbItemSpellID_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemSpellID.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub cmbItemTrigger_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItemTrigger.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub nudItemCharges_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudItemCharges.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub mtbItemPPMRate_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemPPMRate.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub mtbItemCooldown_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemCooldown.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub mtbItemCategory_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemCategory.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub mtbItemCategoryCooldown_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles mtbItemCategoryCooldown.Leave
        Dim I As Integer
        Dim SpellRadio As RadioButton

        For I = 1 To 5
            SpellRadio = CType(tapItemSpells.Controls("rabItemSpell" & I), RadioButton)
            If SpellRadio.Checked = True Then
                SpellRadio.Tag = CStr(mtbItemSpellID.Text & "|" & cmbItemTrigger.SelectedIndex & "|" & nudItemCharges.Value & "|" & mtbItemPPMRate.Text & "|" & mtbItemCooldown.Text & "|" & mtbItemCategory.Text & "|" & mtbItemCategoryCooldown.Text)
            End If
        Next
    End Sub

    Private Sub rabItemSpell1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemSpell1.CheckedChanged
        rabItemSpell_CheckedChanged(rabItemSpell1)
    End Sub
    Private Sub rabItemSpell2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemSpell2.CheckedChanged
        rabItemSpell_CheckedChanged(rabItemSpell2)
    End Sub
    Private Sub rabItemSpell3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemSpell3.CheckedChanged
        rabItemSpell_CheckedChanged(rabItemSpell3)
    End Sub
    Private Sub rabItemSpell4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemSpell4.CheckedChanged
        rabItemSpell_CheckedChanged(rabItemSpell4)
    End Sub
    Private Sub rabItemSpell5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rabItemSpell5.CheckedChanged
        rabItemSpell_CheckedChanged(rabItemSpell5)
    End Sub
#End Region

    Private Sub lblItemClasses_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblItemClasses.MouseClick
        If clbItemClasses.Visible = True Then
            clbItemClasses.Visible = False
        Else
            clbItemClasses.Visible = True
        End If
    End Sub

    Private Sub lblItemRaces_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblItemRaces.MouseClick
        If clbItemRaces.Visible = True Then
            clbItemRaces.Visible = False
        Else
            clbItemRaces.Visible = True
        End If
    End Sub

    Private Sub tapItemReq_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tapItemReq.MouseClick
        clbItemClasses.Visible = False
        clbItemRaces.Visible = False
    End Sub

    Private Sub btnItemDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemDelete.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If mtbItemEntry.Text <> "" Then
            Query = New MySqlCommand("SELECT `entry` FROM `item_template` WHERE `entry` = '" & mtbItemEntry.Text & "' LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                Reader.Close()

                If MsgBox("Are you sure you want to delete this item?" & vbCrLf & "Warning: You can't reverse this action.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Item Delete") = MsgBoxResult.Yes Then
                    ExecuteQuery("DELETE FROM `item_template` WHERE `entry` = '" & mtbItemEntry.Text & "';")
                    MsgBox("Item " & txtID.Text & " deleted successfully.", MsgBoxStyle.Information, "Item Deleted")
                    RestoreDefaults(ClearValues(1))
                End If
            Else
                Reader.Close()
                MsgBox("No item found with this entry ID.", MsgBoxStyle.Exclamation, "Item Delete")
            End If
        End If
    End Sub

    Private Sub clbItemClasses_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbItemClasses.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer
        Static Working As Boolean = False

        If Not Working Then
            Working = True
            If e.Index = 0 Then
                For I As Integer = 1 To (clbItemClasses.Items.Count - 1)
                    clbItemClasses.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.Index <> 0 And e.NewValue = CheckState.Unchecked Then
                clbItemClasses.SetItemChecked(0, False)
            End If

            Flags = GetFlags(clbItemClasses)
            Number = GetNumberFromIndex(clbItemClasses, e.Index)

            If e.NewValue = CheckState.Checked Then
                Flags += Number
            Else
                Flags -= Number
            End If

            If Flags = 1503 Then Flags = -1

            lblItemClasses.Text = CStr(Flags)
            Working = False
        End If
    End Sub

    Private Sub clbItemRaces_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbItemRaces.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer
        Static Working As Boolean = False

        If Not Working Then
            Working = True
            If e.Index = 0 Then
                For I As Integer = 1 To (clbItemRaces.Items.Count - 1)
                    clbItemRaces.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.Index = 1 Then
                If e.NewValue = CheckState.Unchecked Then clbItemRaces.SetItemChecked(0, False)

                For I As Integer = 2 To 6
                    clbItemRaces.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.Index = 7 Then
                If e.NewValue = CheckState.Unchecked Then clbItemRaces.SetItemChecked(0, False)

                For I As Integer = 8 To 12
                    clbItemRaces.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.NewValue = CheckState.Unchecked Then
                clbItemRaces.SetItemChecked(0, False)

                If e.Index > 7 Then
                    clbItemRaces.SetItemChecked(7, False)
                Else
                    clbItemRaces.SetItemChecked(1, False)
                End If
            End If

            Flags = GetFlags(clbItemRaces)
            Number = GetNumberFromIndex(clbItemRaces, e.Index)

            If e.NewValue = CheckState.Checked Then
                Flags += Number
            Else
                Flags -= Number
            End If

            If Flags = 1791 Then Flags = -1

            lblItemRaces.Text = CStr(Flags)
            Working = False
        End If
    End Sub

    Private Sub btnItemUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemUpdate.Click
        Dim Query As MySqlCommand
        Dim Reader As MySqlDataReader = Nothing
        Dim I As Integer
        'Dim Exception As System.Exception
        Dim Sen As Integer
        Dim SQLRunner As QueryBuilder

        'Try
        Sen = 0
        SQLRunner = New QueryBuilder
        SQLRunner.Add("class", CStr(cmbItemClass.SelectedIndex))
        SQLRunner.Add("subclass", CStr(cmbItemSubclass.SelectedIndex))
        SQLRunner.Add("name", GetStringNoNull(txtItemName.Text))
        SQLRunner.Add("displayid", CStr(IIf(mtbItemDisplay.Text <> "", mtbItemDisplay.Text, 0)))
        SQLRunner.Add("Quality", CStr(cmbItemQuality.SelectedIndex))
        SQLRunner.Add("Flags", lblItemFlags.Text)
        SQLRunner.Add("maxcount", CStr(nudUniqueCount.Value))
        SQLRunner.Add("Buyprice", CStr(glcItemBuy.GetValue()))
        SQLRunner.Add("Sellprice", CStr(glcItemSell.GetValue()))
        SQLRunner.Add("InventoryType", ComboGet(cmbItemInventory))
        SQLRunner.Add("AllowableClass", lblItemClasses.Text)
        SQLRunner.Add("AllowableRace", lblItemRaces.Text)
        SQLRunner.Add("ItemLevel", CStr(nudItemLevel.Value))
        SQLRunner.Add("RequiredLevel", CStr(nudItemReqLevel.Value))
        SQLRunner.Add("RequiredSkill", ComboGet(cmbItemSkill))
        SQLRunner.Add("RequiredSkillRank", CStr(nudItemSkillRank.Value))
        SQLRunner.Add("RequiredReputationFaction", ComboGet(cmbItemFactions))
        SQLRunner.Add("RequiredReputationRank", CStr(cmbItemFactionStanding.SelectedIndex))
        SQLRunner.Add("stackable", CStr(nudItemStack.Value))

        For I = 1 To 8
            SQLRunner.Add("stat_type" & I, CStr(tapItemStats.Controls("rabItemStats" & I).Tag).Split(CChar("|"))(0))
            SQLRunner.Add("stat_value" & I, CStr(tapItemStats.Controls("rabItemStats" & I).Tag).Split(CChar("|"))(1))
        Next

        For I = 1 To 5
            SQLRunner.Add("dmg_min" & I, CStr(tapItemWeapon.Controls("rabItemWeapon" & I).Tag).Split(CChar("|"))(1))
            SQLRunner.Add("dmg_max" & I, CStr(tapItemWeapon.Controls("rabItemWeapon" & I).Tag).Split(CChar("|"))(2))
            SQLRunner.Add("dmg_type" & I, CStr(tapItemWeapon.Controls("rabItemWeapon" & I).Tag).Split(CChar("|"))(0))
        Next

        SQLRunner.Add("armor", CStr(IIf(mtbItemArmor.Text <> "", mtbItemArmor.Text, 0)))
        SQLRunner.Add("holy_res", CStr(nudItemHoly.Value))
        SQLRunner.Add("fire_res", CStr(nudItemFire.Value))
        SQLRunner.Add("nature_res", CStr(nudItemNature.Value))
        SQLRunner.Add("frost_res", CStr(nudItemFrost.Value))
        SQLRunner.Add("shadow_res", CStr(nudItemShadow.Value))
        SQLRunner.Add("arcane_res", CStr(nudItemArcane.Value))
        SQLRunner.Add("delay", CStr(IIf(mtbItemSpeed.Text <> "", mtbItemSpeed.Text, 0)))
        SQLRunner.Add("RangedModRange", CStr(nudItemRange.Value))

        For I = 1 To 5
            SQLRunner.Add("spellid_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(0))
            SQLRunner.Add("spelltrigger_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(1))
            SQLRunner.Add("spellcharges_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(2))
            SQLRunner.Add("spellppmRate_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(3))
            SQLRunner.Add("spellcooldown_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(4))
            SQLRunner.Add("spellcategory_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(5))
            SQLRunner.Add("spellcategorycooldown_" & I, CStr(tapItemSpells.Controls("rabItemSpell" & I).Tag).Split(CChar("|"))(6))
        Next

        SQLRunner.Add("bonding", CStr(cmbItemBonding.SelectedIndex))
        SQLRunner.Add("description", txtItemText.Text)
        SQLRunner.Add("PageText", mtbItemPage.Text)
        SQLRunner.Add("startquest", CStr(IIf(mtbItemQuest.Text <> "", mtbItemQuest.Text, 0)))
        SQLRunner.Add("sheath", CStr(cmbItemSheath.SelectedIndex))
        SQLRunner.Add("block", CStr(IIf(mtbItemBlock.Text <> "", mtbItemBlock.Text, 0)))
        SQLRunner.Add("itemset", ComboGet(cmbItemSet))
        SQLRunner.Add("MaxDurability", CStr(nudItemDurability.Value))
        SQLRunner.Add("socketColor_1", CStr(GetNumberFromIndex(cmbItemSocket1)))
        SQLRunner.Add("socketColor_2", CStr(GetNumberFromIndex(cmbItemSocket2)))
        SQLRunner.Add("socketColor_3", CStr(GetNumberFromIndex(cmbItemSocket3)))
        SQLRunner.Add("socketBonus", mtbItemBonus.Text)
        SQLRunner.Add("ContainerSlots", CStr(nudItemSlots.Value))
        SQLRunner.Add("PageMaterial", CStr(cmbPageMaterial.SelectedIndex))
        SQLRunner.Add("LanguageID", ComboGet(cmbLanguageID))
        SQLRunner.Add("TotemCategory", ComboGet(cmbTotemCategory))
        SQLRunner.Add("Material", CStr(cmbObjMaterial.SelectedIndex))
        SQLRunner.Add("BagFamily", ComboGet(cmbBagFamily))
        SQLRunner.Add("RequiredDisenchantSkill", mtbReqDisenchantSkill.Text)
        SQLRunner.Add("FoodType", CStr(GetNumberFromIndex(cmbFoodType)))
        SQLRunner.Add("ScriptName", txtScriptName.Text)
        SQLRunner.Add("Duration", mtbDuration.Text)

        Sen = 1
        Query = New MySqlCommand("SELECT `entry` FROM `item_template` WHERE `entry` = '" & mtbItemEntry.Text & "' LIMIT 1;", Connection)
        Reader = Query.ExecuteReader()

        If Reader.HasRows And mtbItemEntry.Text <> "" Then
            Reader.Close()

            If MsgBox("An entry with this ID (" & mtbItemEntry.Text & ") is already used by other item." & vbCrLf & "Do you want to change this item details to the details you just entered?" & vbCrLf & "Warning: You can't reverse this action.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Item Update") = MsgBoxResult.Yes Then
                Sen = 2
                SQLRunner.RunUPDATE("item_template", CInt(mtbItemEntry.Text))
            End If
        Else
            Reader.Close()
            If mtbItemEntry.Text <> "" Then
                Sen = 4
                SQLRunner.Add("entry", mtbItemEntry.Text)
                SQLRunner.RunINSERT("item_template")
                MsgBox("Item created successfuly.", MsgBoxStyle.Information, "Item Editor")
            Else

                Sen = 6
                Query = New MySqlCommand("SELECT `entry` FROM `item_template` WHERE " & _
                                         "`entry` >= 200000 ORDER BY `entry` DESC LIMIT 1;", Connection)
                Reader = Query.ExecuteReader()

                If Reader.HasRows Then
                    Reader.Read()
                    ItemStartEntry = CInt(Reader.GetInt64(0)) + 1
                End If

                Reader.Close()

                Sen = 7
                SQLRunner.Add("entry", CStr(ItemStartEntry))
                SQLRunner.RunINSERT("item_template")
                mtbItemEntry.Text = CStr(ItemStartEntry)

                MsgBox("Item created successfuly as entry #" & ItemStartEntry & ".", MsgBoxStyle.Information, "Item Editor")
            End If
        End If
        'Catch Exception
        '    If Reader IsNot Nothing Then If Not Reader.IsClosed Then Reader.Close()

        '    Clipboard.Clear()
        '    Clipboard.SetText("(" & Sen & ") " & Exception.Message)
        '    MsgBox("Error! Item wasn't updated/added successfuly." & vbCrLf & "Please post this error at information thread (the error copied to the clipboard):" & vbCrLf & "(" & Sen & ") " & Exception.Message, MsgBoxStyle.Critical)
        'End Try
    End Sub

    Private Sub ItemFlags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblItemFlags.MouseClick
        If clbItemFlags.Visible = True Then
            clbItemFlags.Visible = False
        Else
            clbItemFlags.Visible = True
        End If
    End Sub

    Private Sub clbItemFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbItemFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbItemFlags.Items.Count - 1)
                clbItemFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbItemFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbItemFlags)
        Number = GetNumberFromIndex(clbItemFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblItemFlags.Text = CStr(Flags)
    End Sub

    Private Sub btnItemLootEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemLootEdit.Click
        If mtbItemEntry.Text <> "" Then
            radLLItems.Checked = True
            mtbLLID.Text = mtbItemEntry.Text
            tabMain.SelectTab(tabLootEdit)
            btnLLEdit_Click(Nothing, Nothing)
        Else
            MsgBox("You need to enter an entry ID first before editing the loot", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub cmbItemTrigger_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbItemTrigger.SelectedIndexChanged
        If cmbItemTrigger.SelectedIndex = 3 Then
            mtbItemPPMRate.Enabled = True
        Else
            mtbItemPPMRate.Enabled = False
        End If
    End Sub

    Private Sub btnItemExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemExport.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If (mtbItemEntry.Text = "") Then
            MsgBox("Please make sure the Entry ID textbox is filled before exporting data.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If My.Settings.log Then
            Query = New MySqlCommand("SELECT * FROM `item_template` WHERE `entry` = " & mtbItemEntry.Text & " LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                Reader.Read()

                If AddToLog(Export("item_template", CInt(mtbItemEntry.Text), Reader).GetUPDATE("item_template", CInt(mtbItemEntry.Text))) Then
                    MsgBox("Item exported." & vbCrLf & "Note: If you edited this item without clicking ""Update"", the changes won't be exported.", MsgBoxStyle.Information, "Export")
                End If
            Else
                MsgBox("No item found with this ID.", MsgBoxStyle.Critical, "Export")
            End If
            Reader.Close()
        Else
            MsgBox("Please enable logging and specify a log file before using Export.", MsgBoxStyle.Exclamation, "Export")
        End If
    End Sub

    Private Sub btnItemChooseQuest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnItemChooseQuest.Click
        CallSearch("Quest", mtbItemQuest)
    End Sub
#End Region

#Region " NPC Tab "

    Private Sub btnCreatureExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatureExport.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If (txtID.Text = "") Then
            MsgBox("Please make sure the Entry ID textbox is filled before exporting data.", MsgBoxStyle.Exclamation)
            Exit Sub
        Else
            If My.Settings.log Then
                Query = New MySqlCommand("SELECT * FROM `creature_template` WHERE `entry` = " & mtbNPCID.Text & " LIMIT 1;", Connection)
                Reader = Query.ExecuteReader()

                If Reader.HasRows Then
                    Reader.Read()

                    If AddToLog(Export("creature__template", CInt(mtbNPCID.Text), Reader).GetUPDATE("creature__template", CInt(mtbNPCID.Text))) Then
                        MsgBox("NPC exported." & vbCrLf & "Note: If you edited this NPC without clicking ""Update"", the changes won't be exported.", MsgBoxStyle.Information, "Export")
                    End If
                Else
                    MsgBox("No NPC found with this ID.", MsgBoxStyle.Critical, "Export")
                End If
                Reader.Close()
            Else
                MsgBox("Please enable logging and specify a log file before using Export.", MsgBoxStyle.Exclamation, "Export")
            End If
        End If
    End Sub

    Private Sub btnChooseNpc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseNpc.Click
        CallSearch("NPC")
    End Sub

    Private Sub mtbNPCDamageSpeed_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMinDmg.TextChanged, txtMaxDmg.TextChanged, mtbBaseAtkTime.TextChanged
        UpdateDPS(txtMinDmg, txtMaxDmg, mtbBaseAtkTime, Label25, "Melee DPS")
    End Sub

    Private Sub mtbNPCRDamageSpeed_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMinRngDmg.TextChanged, txtMaxRngDmg.TextChanged, mtbNPCRSpeed.TextChanged
        UpdateDPS(txtMinRngDmg, txtMaxRngDmg, mtbNPCRSpeed, Label24, "Ranged DPS")
    End Sub

    Private Sub btnTrainerSpellLookup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrainerSpellLookup.Click
        CallSearch("Spell", mtbTrainerSpell)
    End Sub

    Private Sub cmbTrainerType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbTrainerType.SelectedIndexChanged
        If clbNPCFlags.CheckedIndices.Contains(3) Then
            If cmbTrainerType.SelectedIndex = 0 Or cmbTrainerType.SelectedIndex = 3 Then
                cmbTrainerClass.Enabled = True
            Else
                cmbTrainerClass.Enabled = False
            End If

            If cmbTrainerType.SelectedIndex = 1 Then
                cmbTrainerRace.Enabled = True
            Else
                cmbTrainerRace.Enabled = False
            End If

            If cmbTrainerType.SelectedIndex = 2 Then
                mtbTrainerSpell.Enabled = True
            Else
                mtbTrainerSpell.Enabled = False
            End If
        Else
            cmbTrainerType.Enabled = False
            cmbTrainerClass.Enabled = False
            cmbTrainerRace.Enabled = False
            mtbTrainerSpell.Enabled = False
        End If
    End Sub

#Region " Flags "

    Private Sub Flags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblFlags.MouseClick
        If clbFlags.Visible = True Then
            clbFlags.Visible = False
        Else
            clbFlags.Visible = True
        End If
    End Sub

    Private Sub clbFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbFlags.Items.Count - 1)
                clbFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbFlags)
        Number = GetNumberFromIndex(clbFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblFlags.Text = CStr(Flags)
    End Sub

    Private Sub NPCFlags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblNpcFlag.MouseClick
        If clbNPCFlags.Visible = True Then
            clbNPCFlags.Visible = False
        Else
            clbNPCFlags.Visible = True
        End If
    End Sub

    Private Sub clbNPCFlag_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbNPCFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbNPCFlags.Items.Count - 1)
                clbNPCFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbNPCFlags.SetItemChecked(0, False)
        End If

        If e.Index = 3 And e.NewValue = CheckState.Checked Then
            cmbTrainerType.Enabled = True
            cmbTrainerClass.Enabled = True
        Else
            cmbTrainerType.Enabled = False
            cmbTrainerClass.Enabled = False
        End If

        Flags = GetFlags(clbNPCFlags)
        Number = GetNumberFromIndex(clbNPCFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblNpcFlag.Text = CStr(Flags)
    End Sub

    Private Sub DynamicFlags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblDynamicFlags.MouseClick
        If clbDynamicFlags.Visible = True Then
            clbDynamicFlags.Visible = False
        Else
            clbDynamicFlags.Visible = True
        End If
    End Sub

    Private Sub clbDynamicFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbDynamicFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbDynamicFlags.Items.Count - 1)
                clbDynamicFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbDynamicFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbDynamicFlags)
        Number = GetNumberFromIndex(clbDynamicFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblDynamicFlags.Text = CStr(Flags)
    End Sub

    Private Sub ExtraFlags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblExtraFlags.MouseClick
        If clbExtraFlags.Visible = True Then
            clbExtraFlags.Visible = False
        Else
            clbExtraFlags.Visible = True
        End If
    End Sub

    Private Sub clbExtraFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbExtraFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbExtraFlags.Items.Count - 1)
                clbExtraFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbExtraFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbExtraFlags)
        Number = GetNumberFromIndex(clbExtraFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblExtraFlags.Text = CStr(Flags)
    End Sub

    Private Sub AttributeFlags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblAttributeFlags.MouseClick
        If clbAttributeFlags.Visible = True Then
            clbAttributeFlags.Visible = False
        Else
            clbAttributeFlags.Visible = True
        End If
    End Sub

    Private Sub clbAttributeFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbAttributeFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbAttributeFlags.Items.Count - 1)
                clbAttributeFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbAttributeFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbAttributeFlags)
        Number = GetNumberFromIndex(clbAttributeFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblAttributeFlags.Text = CStr(Flags)
    End Sub

    Private Sub ImmuneFlags_Display(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblImmuneFlags.MouseClick
        If clbImmuneFlags.Visible = True Then
            clbImmuneFlags.Visible = False
        Else
            clbImmuneFlags.Visible = True
        End If
    End Sub

    Private Sub clbImmuneFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbImmuneFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbImmuneFlags.Items.Count - 1)
                clbImmuneFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbImmuneFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbImmuneFlags)
        Number = GetNumberFromIndex(clbImmuneFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblImmuneFlags.Text = CStr(Flags)
    End Sub

#End Region

    Private Sub btnNPCClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNPCClear.Click
        RestoreDefaults(ClearValues(0))
    End Sub

    Private Sub btnNPCDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNPCDefaults.Click
        RestoreDefaults(DefaultValues(0))
    End Sub

    Private Sub btnChooseMaleDisplayID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseMaleDisplayID.Click
        frmChooseNPC.Male = True
        CallSearchDisplay("NPC", cmbMaleID)
    End Sub

    Private Sub btnChooseFemaleDisplayID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseFemaleDisplayID.Click
        frmChooseNPC.Male = True
        CallSearchDisplay("NPC", cmbFemID)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If txtID.Text <> "" Then
            Query = New MySqlCommand("SELECT `entry` FROM `creature_template` WHERE `entry` = '" & txtID.Text & "' LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                Reader.Close()

                If MsgBox("Are you sure you want to delete this NPC?" & vbCrLf & "Warning: You can't reverse this action.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "NPC Delete") = MsgBoxResult.Yes Then
                    ExecuteQuery("DELETE FROM `creature_template` WHERE `entry` = '" & txtID.Text & "';")
                    MsgBox("NPC " & txtID.Text & " deleted successfully.", MsgBoxStyle.Information, "NPC Deleted")
                    RestoreDefaults(ClearValues(0))
                End If
            Else
                Reader.Close()
                MsgBox("No NPC found with this entry ID.", MsgBoxStyle.Exclamation, "NPC Delete")
            End If
        End If
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim Query As MySqlCommand
        Dim Reader As MySqlDataReader = Nothing
        Dim Exception As System.Exception
        Dim Sen As Integer = 0
        Dim SQLRunner1 As QueryBuilder

        If cmbMovementType.SelectedIndex = -1 Then
            cmbMovementType.SelectedIndex = 0
        End If

        Try
            Sen = 0
            SQLRunner1 = New QueryBuilder
            SQLRunner1.Add("name", GetStringNoNull(txtName.Text))
            SQLRunner1.Add("subname", GetStringNoNull(txtSubname.Text))
            SQLRunner1.Add("type", CStr(cmbType.SelectedIndex))
            SQLRunner1.Add("family", CStr(FamilyToIndex(cmbFamily.SelectedIndex)))
            SQLRunner1.Add("rank", CStr(cmbRank.SelectedIndex))
            SQLRunner1.Add("modelid_A", CStr(IIf(cmbMaleID.Text <> "", cmbMaleID.Text, 0)))
            SQLRunner1.Add("modelid_H", CStr(IIf(cmbFemID.Text <> "", cmbFemID.Text, 0)))
            SQLRunner1.Add("RacialLeader", CStr(IIf(chkLeader.Checked, 1, 0)))
            SQLRunner1.Add("armor", CStr(txtArmor.Text))
            SQLRunner1.Add("attackpower", CStr(txtAtkPwr.Text))
            SQLRunner1.Add("faction_A", ComboGet(cmbFaction))
            SQLRunner1.Add("faction_H", ComboGet(cmbFaction))
            SQLRunner1.Add("mingold", CStr(glcMin.GetValue()))
            SQLRunner1.Add("maxgold", CStr(glcMax.GetValue()))
            SQLRunner1.Add("speed", CStr(txtSpeed.Text))
            SQLRunner1.Add("minlevel", CStr(nudMinLvl.Value))
            SQLRunner1.Add("maxlevel", CStr(nudMaxLvl.Value))
            SQLRunner1.Add("minhealth", mtbMinHp.Text)
            SQLRunner1.Add("maxhealth", mtbMaxHp.Text)
            SQLRunner1.Add("minmana", mtbMinMana.Text)
            SQLRunner1.Add("maxmana", mtbMaxMana.Text)
            SQLRunner1.Add("scale", CStr(GetValue(txtScale.Text)))
            SQLRunner1.Add("baseattacktime", CStr(IIf(mtbBaseAtkTime.Text <> "", mtbBaseAtkTime.Text, 0)))
            SQLRunner1.Add("mindmg", CStr(GetValue(txtMinDmg.Text)))
            SQLRunner1.Add("maxdmg", CStr(GetValue(txtMaxDmg.Text)))
            SQLRunner1.Add("rangeattacktime", CStr(IIf(mtbRngAtkTime.Text <> "", mtbRngAtkTime.Text, 0)))
            SQLRunner1.Add("minrangedmg", CStr(GetValue(txtMinRngDmg.Text)))
            SQLRunner1.Add("maxrangedmg", CStr(GetValue(txtMaxRngDmg.Text)))
            SQLRunner1.Add("equipment_id", mtbEquip.Text)
            SQLRunner1.Add("dmgschool", CStr(cmbDamageSchool.SelectedIndex))
            SQLRunner1.Add("InhabitType", CStr(cmbInhabitType.SelectedIndex + 1))
            SQLRunner1.Add("MovementType", CStr(cmbMovementType.SelectedIndex))
            SQLRunner1.Add("npcflag", lblNpcFlag.Text)
            SQLRunner1.Add("dynamicflags", lblDynamicFlags.Text)
            SQLRunner1.Add("flag1", lblExtraFlags.Text)
            SQLRunner1.Add("flags", lblFlags.Text)
            SQLRunner1.Add("flags_extra", lblAttributeFlags.Text)
            SQLRunner1.Add("mechanic_immune_mask", lblImmuneFlags.Text)
            SQLRunner1.Add("lootid", mtbLootID.Text)
            SQLRunner1.Add("skinloot", mtbSkinningLootID.Text)
            SQLRunner1.Add("pickpocketloot", mtbPickpocketingLootID.Text)
            SQLRunner1.Add("trainer_type", CStr(cmbTrainerType.SelectedIndex))
            SQLRunner1.Add("trainer_spell", mtbTrainerSpell.Text)
            SQLRunner1.Add("class", ComboGet(cmbTrainerClass))
            SQLRunner1.Add("race", ComboGet(cmbTrainerRace))
            If chkRegenHealth.Checked = True Then
                SQLRunner1.Add("RegenHealth", "1")
            Else
                SQLRunner1.Add("RegenHealth", "0")
            End If

            Sen = 1
            Query = New MySqlCommand("SELECT `entry` FROM `creature_template` WHERE `entry` = '" & txtID.Text & "' LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows And txtID.Text <> "" Then
                Reader.Close()

                If MsgBox("An entry with this ID (" & txtID.Text & ") is already in use by another NPC." & vbCrLf & "Do you want to change this NPCs details to the details you just entered?" & vbCrLf & "Warning: You can't undo this action.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "NPC Update") = MsgBoxResult.Yes Then
                    Sen = 2
                    SQLRunner1.RunUPDATE("creature_template", CInt(txtID.Text))

                    MsgBox("NPC updated successfully.", MsgBoxStyle.Information, "DB Editor")
                End If
            Else
                Reader.Close()
                If txtID.Text <> "" Then
                    Sen = 4
                    SQLRunner1.Add("entry", txtID.Text)
                    SQLRunner1.RunINSERT("creature_template")

                    MsgBox("NPC created successfuly.", MsgBoxStyle.Information, "DB Editor")
                Else

                    Sen = 6
                    Query = New MySqlCommand("SELECT `entry` FROM `creature_template` WHERE " & _
                                             "`entry` >= 200000 ORDER BY `entry` DESC LIMIT 1;", Connection)
                    Reader = Query.ExecuteReader

                    If Reader.HasRows Then
                        Reader.Read()
                        NPCStartEntry = CInt(Reader.GetInt64(0)) + 1
                    End If

                    Reader.Close()

                    Sen = 7
                    SQLRunner1.Add("entry", CStr(NPCStartEntry))
                    SQLRunner1.RunINSERT("creature_template")

                    txtID.Text = CStr(NPCStartEntry)

                    MsgBox("NPC created successfuly as entry #" & NPCStartEntry & ".", MsgBoxStyle.Information, "DB Editor")
                End If
            End If
        Catch Exception
            If Reader IsNot Nothing Then If Not Reader.IsClosed Then Reader.Close()

            Clipboard.Clear()
            Clipboard.SetText("(" & Sen & ") " & Exception.Message)
            MsgBox("Error! NPC wasn't updated/added successfuly." & vbCrLf & "Please send this text to me (the error copied to the clipboard):" & vbCrLf & "(" & Sen & ") " & Exception.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnEditRegLoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditRegLoot.Click
        If mtbLootID.Text = "0" Then
            MsgBox("This creature does not have a regular loot table.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If mtbLootID.Text <> "" Then
            radLLCreature.Checked = True
            mtbLLID.Text = mtbLootID.Text
            tabMain.SelectTab(tabLootEdit)
            btnLLEdit_Click(Nothing, Nothing)
        Else
            MsgBox("You need to enter an entry ID first before editing the loot", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnEditSkinLoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditSkinLoot.Click
        If mtbSkinningLootID.Text = "0" Then
            MsgBox("This creature does not have a skinning loot table.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If mtbSkinningLootID.Text <> "" Then
            radLLSkinning.Checked = True
            mtbLLID.Text = mtbSkinningLootID.Text
            tabMain.SelectTab(tabLootEdit)
            btnLLEdit_Click(Nothing, Nothing)
        Else
            MsgBox("You need to enter an entry ID first before editing the loot", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnEditPickpocketLoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditPickpocketLoot.Click
        If mtbPickpocketingLootID.Text = "0" Then
            MsgBox("This creature does not have a pickpocket loot table.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If mtbPickpocketingLootID.Text <> "" Then
            radLLPickPocketing.Checked = True
            mtbLLID.Text = mtbPickpocketingLootID.Text
            tabMain.SelectTab(tabLootEdit)
            btnLLEdit_Click(Nothing, Nothing)
        Else
            MsgBox("You need to enter an entry ID first before editing the loot", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnCreatureLinkEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreatureLinkEdit.Click
        If txtID.Text <> "" Then
            mtbLNID.Text = txtID.Text
            tabMain.SelectTab(tabLink)
            tabLinker.SelectTab(tapLinkNPCs)
            btnLNEdit_Click(Nothing, Nothing)
            MsgBox("You need to enter an entry ID first before viewing linker information", MsgBoxStyle.Information)
        End If
    End Sub
#End Region

#Region " Quest Tab "
#Region " Neg. Checks "
    Private Sub cmbQuestZoneID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuestZoneID.SelectedIndexChanged
        tlpQuest.SetToolTip(cmbQuestZoneID, "If you have chosen a value from Quest Zone, Quest Sort is disabled, if you wish for it to be enabled, please set value to nothing")
        If cmbQuestZoneID.SelectedIndex <> 0 Then
            cmbQuestSort.Enabled = False
        Else
            cmbQuestSort.Enabled = True
        End If
    End Sub

    Private Sub cmbQuestSort_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuestSort.SelectedIndexChanged
        tlpQuest.SetToolTip(cmbQuestSort, "If you have chosen a value from Quest Sort, Quest Zone is disabled, if you wish for it to be enabled, please set value to nothing")
        If cmbQuestSort.SelectedIndex <> 0 Then
            cmbQuestZoneID.Enabled = False
        Else
            cmbQuestZoneID.Enabled = True
        End If
    End Sub

    Private Sub clbQuestClasses_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        tlpQuest.SetToolTip(clbQuestClasses, "If you have chosen a value from Class Requirement, Profession Requirement is disabled, if you wish for it to be enabled, please set value to nothing")
        If clbQuestClasses.SelectedIndex > 0 Then
            cmbQuestProffesion.Enabled = False
        Else
            cmbQuestProffesion.Enabled = True
        End If
    End Sub

    Private Sub cmbQuestProffesion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuestProffesion.SelectedIndexChanged
        tlpQuest.SetToolTip(cmbQuestProffesion, "If you have chosen a value from Profession Requirement, Class Requirement is disabled, if you wish for it to be enabled, please set value to nothing")
        If cmbQuestProffesion.SelectedIndex > 0 Then
            clbQuestClasses.Enabled = False
        Else
            clbQuestClasses.Enabled = True
        End If
    End Sub

    Private Sub glcQuestGoldReq_Load() Handles glcQuestGoldReq.ValueChanged
        tlpQuest.SetToolTip(glcQuestGoldReq, "If you have chosen a value from Gold Requirement, Gold Reward is disabled, if you wish for it to be enabled, please set value to nothing")
        If glcQuestGoldReq.GetValue > 0 Then
            glcQuestGoldRew.Enabled = False
        Else
            glcQuestGoldRew.Enabled = True
        End If
    End Sub

    Private Sub glcQuestGoldRew_Load() Handles glcQuestGoldRew.ValueChanged
        tlpQuest.SetToolTip(glcQuestGoldRew, "If you have chosen a value from Gold Reward, Gold Requirement is disabled, if you wish for it to be enabled, please set value to nothing")
        If glcQuestGoldRew.GetValue > 0 Then
            glcQuestGoldReq.Enabled = False
        Else
            glcQuestGoldReq.Enabled = True
        End If
    End Sub
#End Region
#Region " Form Controls "
    Private Sub btnQuestLinkEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestLinkEdit.Click
        If mtbQuestEntry.Text <> "" Then
            mtbLQID.Text = mtbQuestEntry.Text
            tabMain.SelectTab(tabLink)
            tabLinker.SelectTab(tapLinkQuests)
            btnLLEdit_Click(Nothing, Nothing)
        Else
            MsgBox("You need to enter an entry ID first before viewing linker information", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub btnQuestExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestExport.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If (mtbQuestEntry.Text = "") Then
            MsgBox("Please make sure the Entry ID textbox is filled before exporting data.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        If My.Settings.log Then
            Query = New MySqlCommand("SELECT * FROM `quest_template` WHERE `entry` = " & mtbQuestEntry.Text & " LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                Reader.Read()

                If AddToLog(Export("quest_template", CInt(mtbQuestEntry.Text), Reader).GetUPDATE("quest_template", CInt(mtbQuestEntry.Text))) Then
                    MsgBox("Quest exported." & vbCrLf & "Note: If you edited this quest without clicking ""Update"", the changes won't be exported.", MsgBoxStyle.Information, "Export")
                End If
            Else
                MsgBox("No quest found with this ID.", MsgBoxStyle.Critical, "Export")
            End If
            Reader.Close()
        Else
            MsgBox("Please enable logging and specify a log file before using Export.", MsgBoxStyle.Exclamation, "Export")
        End If
    End Sub

    Private Sub FlagsVisible_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles grpQuest.MouseEnter, tabQuestTab.MouseEnter, tapQuestTexts.MouseEnter, tapQuestObjER.MouseEnter, tapQuestObjIM.MouseEnter, tapQuestProvided.MouseEnter, tapQuestReq.MouseEnter, tapQuestRewardChoices.MouseEnter, tapQuestRewards.MouseEnter
        clbQuestFlags.Visible = False
        clbQuestSpecialFlags.Visible = False
    End Sub

    Private Sub tapQuestReq_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tapQuestReq.MouseEnter
        clbQuestClasses.Visible = False
        clbQuestRaces.Visible = False
    End Sub

    Private Sub btnChooseQuest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChooseQuest.Click
        CallSearch("Quest")
    End Sub

    Private Sub btnQuestsClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestsClear.Click
        RestoreDefaults(ClearValues(2))
    End Sub

    Private Sub btnQuestDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestDefaults.Click
        RestoreDefaults(DefaultValues(2))
    End Sub

    Private Sub btnQuestPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestPrev.Click
        CallSearch("Quest", mtbQuestPrevQ)
    End Sub

    Private Sub btnQuestNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestNext.Click
        CallSearch("Quest", mtbQuestNextQ)
    End Sub

    Private Sub btnQuestObj1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestObj1.Click
        CallSearch("Item", mtbQuestObj1)
    End Sub

    Private Sub btnQuestObj2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestObj2.Click
        CallSearch("Item", mtbQuestObj2)
    End Sub

    Private Sub btnQuestObj3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestObj3.Click
        CallSearch("Item", mtbQuestObj3)
    End Sub

    Private Sub btnQuestObj4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestObj4.Click
        CallSearch("Item", mtbQuestObj4)
    End Sub

    Private Sub btnQuestCast1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCast1.Click
        CallSearch("Spell", mtbQuestReqSpell1)
    End Sub

    Private Sub btnQuestCast2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCast2.Click
        CallSearch("Spell", mtbQuestReqSpell2)
    End Sub

    Private Sub btnQuestCast3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCast3.Click
        CallSearch("Spell", mtbQuestReqSpell3)
    End Sub

    Private Sub btnQuestCast4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCast4.Click
        CallSearch("Spell", mtbQuestReqSpell4)
    End Sub

    Private Sub btnQuestMob1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestMob1.Click
        CallSearch("NPC", mtbQuestMob1)
    End Sub

    Private Sub btnQuestMob2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestMob2.Click
        CallSearch("NPC", mtbQuestMob2)
    End Sub

    Private Sub btnQuestMob3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestMob3.Click
        CallSearch("NPC", mtbQuestMob3)
    End Sub

    Private Sub btnQuestMob4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestMob4.Click
        CallSearch("NPC", mtbQuestMob4)
    End Sub

    Private Sub btnQuestRew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestRew.Click
        CallSearch("Spell", mtbQuestSpellRew)
    End Sub

    Private Sub btnQuestCast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCast.Click
        CallSearch("Spell", mtbQuestCast)
    End Sub

    Private Sub btnQuestCho1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCho1.Click
        CallSearch("Item", mtbQuestItemChoice1)
    End Sub

    Private Sub btnQuestCho2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCho2.Click
        CallSearch("Item", mtbQuestItemChoice2)
    End Sub

    Private Sub btnQuestCho3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCho3.Click
        CallSearch("Item", mtbQuestItemChoice3)
    End Sub

    Private Sub btnQuestCho4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCho4.Click
        CallSearch("Item", mtbQuestItemChoice4)
    End Sub

    Private Sub btnQuestCho5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCho5.Click
        CallSearch("Item", mtbQuestItemChoice5)
    End Sub

    Private Sub btnQuestCho6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestCho6.Click
        CallSearch("Item", mtbQuestItemChoice6)
    End Sub

    Private Sub btnQuestRew1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestRew1.Click
        CallSearch("Item", mtbQuestIRew1)
    End Sub

    Private Sub btnQuestRew2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestRew2.Click
        CallSearch("Item", mtbQuestIRew2)
    End Sub

    Private Sub btnQuestRew3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestRew3.Click
        CallSearch("Item", mtbQuestIRew3)
    End Sub

    Private Sub btnQuestRew4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestRew4.Click
        CallSearch("Item", mtbQuestIRew4)
    End Sub

    Private Sub btnQuestDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestDelete.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand

        If mtbQuestEntry.Text <> "" Then
            Query = New MySqlCommand("SELECT `entry` FROM `quest_template` WHERE `entry` = '" & mtbQuestEntry.Text & "' LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows Then
                Reader.Close()

                If MsgBox("Are you sure you want to delete this quest?" & vbCrLf & "Warning: You can't reverse this action.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Quset Delete") = MsgBoxResult.Yes Then
                    ExecuteQuery("DELETE FROM `quest_template` WHERE `entry` = '" & mtbQuestEntry.Text & "';")
                End If
            Else
                Reader.Close()
                MsgBox("No quest found with this entry ID.", MsgBoxStyle.Exclamation, "Quest Delete")
            End If
        End If
    End Sub

    Private Sub lblQuestFlags_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuestFlags.Click
        If clbQuestFlags.Visible = True Then
            clbQuestFlags.Visible = False
        Else
            clbQuestFlags.Visible = True
        End If
    End Sub

    Private Sub lblQuestSpecialFlags_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuestSpecialFlags.Click
        If clbQuestSpecialFlags.Visible = True Then
            clbQuestSpecialFlags.Visible = False
        Else
            clbQuestSpecialFlags.Visible = True
        End If
    End Sub

    Private Sub lblQuestClasses_MouseClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuestClasses.Click
        If clbQuestClasses.Visible = True Then
            clbQuestClasses.Visible = False
        Else
            clbQuestClasses.Visible = True
        End If
    End Sub

    Private Sub lblQuestRaces_MouseClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuestRaces.Click
        If clbQuestRaces.Visible = True Then
            clbQuestRaces.Visible = False
        Else
            clbQuestRaces.Visible = True
        End If
    End Sub

    Private Sub clbQuestFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbQuestFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbQuestFlags.Items.Count - 1)
                clbQuestFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbQuestFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbQuestFlags)
        Number = GetNumberFromIndex(clbQuestFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblQuestFlags.Text = CStr(Flags)
    End Sub

    Private Sub clbQuestSpecialFlags_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbQuestSpecialFlags.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer

        If e.Index = 0 And e.NewValue = CheckState.Checked Then
            For I As Integer = 1 To (clbQuestSpecialFlags.Items.Count - 1)
                clbQuestSpecialFlags.SetItemChecked(I, False)
            Next
        ElseIf e.Index <> 0 And e.NewValue = CheckState.Checked Then
            clbQuestSpecialFlags.SetItemChecked(0, False)
        End If

        Flags = GetFlags(clbQuestSpecialFlags)
        Number = GetNumberFromIndex(clbQuestSpecialFlags, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        lblQuestSpecialFlags.Text = CStr(Flags)
    End Sub

    Private Sub clbQuestClasses_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbQuestClasses.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer
        Static Working As Boolean = False

        If Not Working And Not Updating Then
            Working = True

            If e.Index = 0 Then
                For I As Integer = 1 To (clbQuestClasses.Items.Count - 1)
                    clbQuestClasses.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.Index <> 0 And e.NewValue = CheckState.Unchecked Then
                clbQuestClasses.SetItemChecked(0, False)
            End If
            Working = False
        End If

        Flags = GetFlags(clbQuestClasses)
        Number = GetNumberFromIndex(clbQuestClasses, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        If Flags = 1503 Then Flags = 0

        lblQuestClasses.Text = CStr(Flags)
    End Sub

    Private Sub clbQuestRaces_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbQuestRaces.ItemCheck
        Dim Flags As Integer
        Dim Number As Integer
        Static Working As Boolean = False

        If Not Working And Not Updating Then
            Working = True
            If e.Index = 0 Then
                For I As Integer = 1 To (clbQuestRaces.Items.Count - 1)
                    clbQuestRaces.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.Index = 1 Then
                If e.NewValue = CheckState.Unchecked Then clbQuestRaces.SetItemChecked(0, False)

                For I As Integer = 2 To 6
                    clbQuestRaces.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.Index = 7 Then
                If e.NewValue = CheckState.Unchecked Then clbQuestRaces.SetItemChecked(0, False)

                For I As Integer = 8 To 12
                    clbQuestRaces.SetItemChecked(I, e.NewValue = CheckState.Checked)
                Next
            ElseIf e.NewValue = CheckState.Unchecked Then
                clbQuestRaces.SetItemChecked(0, False)

                If e.Index > 7 Then
                    clbQuestRaces.SetItemChecked(7, False)
                Else
                    clbQuestRaces.SetItemChecked(1, False)
                End If
            End If
            Working = False
        End If

        Flags = GetFlags(clbQuestRaces)
        Number = GetNumberFromIndex(clbQuestRaces, e.Index)

        If e.NewValue = CheckState.Checked Then
            Flags += Number
        Else
            Flags -= Number
        End If

        If Flags = 1791 Then Flags = 0

        lblQuestRaces.Text = CStr(Flags)
    End Sub

    Private Sub btnQuestUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestUpdate.Click
        Dim Query As MySqlCommand
        Dim Reader As MySqlDataReader = Nothing
        Dim Exception As System.Exception
        Dim Sen As Integer = 0
        Dim SQLRunner As QueryBuilder

        Try
            Sen = 0
            SQLRunner = New QueryBuilder
            If cmbQuestType.SelectedIndex = 3 Then
                SQLRunner.Add("ZoneOrSort", "0")
            Else
                If cmbQuestSort.Enabled = False Then
                    SQLRunner.Add("ZoneOrSort", ComboGet(cmbQuestZoneID))
                Else
                    SQLRunner.Add("ZoneOrSort", CStr(ToOppositeSign(GetNumberFromIndex(cmbQuestSort))))
                End If
            End If
            If clbQuestClasses.Enabled = False Then
                SQLRunner.Add("SkillOrClass", CStr(GetNumberFromIndex(cmbQuestProffesion)))
            Else
                SQLRunner.Add("SkillOrClass", CStr(ToOppositeSign(GetFlags(clbQuestClasses))))
            End If
            SQLRunner.Add("MinLevel", CStr(nudQuestMinLevel.Value))
            SQLRunner.Add("QuestLevel", CStr(nudQuestLevel.Value))
            SQLRunner.Add("Type", CStr(GetNumberFromIndex(cmbQuestType)))
            SQLRunner.Add("RequiredRaces", CStr(GetFlags(clbQuestRaces)))
            SQLRunner.Add("RequiredSkillValue", CStr(nudSkillVal.Value))
            SQLRunner.Add("RepObjectiveFaction", ComboGet(cmbQuestObjFaction))
            SQLRunner.Add("RepObjectiveValue", CStr(nudQuestObjFaction.Value))
            SQLRunner.Add("RequiredMinRepFaction", ComboGet(cmbMinRepFaction))
            SQLRunner.Add("RequiredMinRepValue", CStr(nudMinRepFaction.Value))
            SQLRunner.Add("RequiredMaxRepFaction", ComboGet(cmbMaxRepFaction))
            SQLRunner.Add("RequiredMaxRepValue", CStr(nudMaxRepFaction.Value))
            SQLRunner.Add("SuggestedPlayers", CStr(nudSuggestedPlayers.Value))
            SQLRunner.Add("LimitTime", CStr(nudQuestLimit.Value))
            SQLRunner.Add("QuestFlags", lblQuestFlags.Text)
            SQLRunner.Add("SpecialFlags", lblQuestSpecialFlags.Text)
            SQLRunner.Add("CharTitleId", CStr(GetNumberFromIndex(cmbCharTitleID)))
            SQLRunner.Add("PrevQuestId", mtbQuestPrevQ.Text)
            SQLRunner.Add("NextQuestId", mtbQuestNextQ.Text)
            SQLRunner.Add("SrcItemID", mtbQuestSource.Text)
            SQLRunner.Add("SrcItemCount", CStr(nudQuestSource.Value))
            SQLRunner.Add("SrcSpell", mtbQuestSSource.Text)
            SQLRunner.Add("Title", txtQuestTitle.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("Details", txtQuestDetails.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("Objectives", txtQuestObjectives.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("OfferRewardText", txtQuestCompText.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("RequestItemsText", txtQuestIncompText.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("EndText", txtQuestFinishText.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("ObjectiveText1", txtQuestObjText1.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("ObjectiveText2", txtQuestObjText1.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("ObjectiveText3", txtQuestObjText1.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("ObjectiveText4", txtQuestObjText1.Text.Replace(vbCrLf, "$B"))
            SQLRunner.Add("ReqItemId1", mtbQuestObj1.Text)
            SQLRunner.Add("ReqItemId2", mtbQuestObj2.Text)
            SQLRunner.Add("ReqItemId3", mtbQuestObj3.Text)
            SQLRunner.Add("ReqItemId4", mtbQuestObj4.Text)
            SQLRunner.Add("ReqItemCount1", CStr(nudQuestObj1.Value))
            SQLRunner.Add("ReqItemCount2", CStr(nudQuestObj2.Value))
            SQLRunner.Add("ReqItemCount3", CStr(nudQuestObj3.Value))
            SQLRunner.Add("ReqItemCount4", CStr(nudQuestObj4.Value))
            SQLRunner.Add("ReqCreatureOrGOId1", CStr(IIf(chkQuestMob1.Checked, mtbQuestMob1.Text, ToOppositeSign(CInt(mtbQuestMob1.Text)))))
            SQLRunner.Add("ReqCreatureOrGOId2", CStr(IIf(chkQuestMob2.Checked, mtbQuestMob2.Text, ToOppositeSign(CInt(mtbQuestMob2.Text)))))
            SQLRunner.Add("ReqCreatureOrGOId3", CStr(IIf(chkQuestMob3.Checked, mtbQuestMob3.Text, ToOppositeSign(CInt(mtbQuestMob3.Text)))))
            SQLRunner.Add("ReqCreatureOrGOId4", CStr(IIf(chkQuestMob4.Checked, mtbQuestMob4.Text, ToOppositeSign(CInt(mtbQuestMob4.Text)))))
            SQLRunner.Add("ReqCreatureOrGOCount1", CStr(nudQuestMob1.Value))
            SQLRunner.Add("ReqCreatureOrGOCount2", CStr(nudQuestMob2.Value))
            SQLRunner.Add("ReqCreatureOrGOCount3", CStr(nudQuestMob3.Value))
            SQLRunner.Add("ReqCreatureOrGOCount4", CStr(nudQuestMob4.Value))
            SQLRunner.Add("ReqSpellCast1", mtbQuestReqSpell1.Text)
            SQLRunner.Add("ReqSpellCast2", mtbQuestReqSpell2.Text)
            SQLRunner.Add("ReqSpellCast3", mtbQuestReqSpell3.Text)
            SQLRunner.Add("ReqSpellCast4", mtbQuestReqSpell4.Text)
            SQLRunner.Add("RewChoiceItemId1", mtbQuestItemChoice1.Text)
            SQLRunner.Add("RewChoiceItemId2", mtbQuestItemChoice2.Text)
            SQLRunner.Add("RewChoiceItemId3", mtbQuestItemChoice3.Text)
            SQLRunner.Add("RewChoiceItemId4", mtbQuestItemChoice4.Text)
            SQLRunner.Add("RewChoiceItemId5", mtbQuestItemChoice5.Text)
            SQLRunner.Add("RewChoiceItemId6", mtbQuestItemChoice6.Text)
            SQLRunner.Add("RewChoiceItemCount1", CStr(nudQuestItemChoice1.Value))
            SQLRunner.Add("RewChoiceItemCount2", CStr(nudQuestItemChoice2.Value))
            SQLRunner.Add("RewChoiceItemCount3", CStr(nudQuestItemChoice3.Value))
            SQLRunner.Add("RewChoiceItemCount4", CStr(nudQuestItemChoice4.Value))
            SQLRunner.Add("RewChoiceItemCount5", CStr(nudQuestItemChoice5.Value))
            SQLRunner.Add("RewChoiceItemCount6", CStr(nudQuestItemChoice6.Value))
            SQLRunner.Add("RewItemId1", mtbQuestIRew1.Text)
            SQLRunner.Add("RewItemId2", mtbQuestIRew2.Text)
            SQLRunner.Add("RewItemId3", mtbQuestIRew3.Text)
            SQLRunner.Add("RewItemId4", mtbQuestIRew4.Text)
            SQLRunner.Add("RewItemCount1", CStr(nudQuestIRew1.Value))
            SQLRunner.Add("RewItemCount2", CStr(nudQuestIRew2.Value))
            SQLRunner.Add("RewItemCount3", CStr(nudQuestIRew3.Value))
            SQLRunner.Add("RewItemCount4", CStr(nudQuestIRew4.Value))
            SQLRunner.Add("RewRepFaction1", ComboGet(cmbQuestRepRew1))
            SQLRunner.Add("RewRepFaction2", ComboGet(cmbQuestRepRew2))
            SQLRunner.Add("RewRepFaction3", ComboGet(cmbQuestRepRew3))
            SQLRunner.Add("RewRepFaction4", ComboGet(cmbQuestRepRew4))
            SQLRunner.Add("RewRepFaction5", ComboGet(cmbQuestRepRew5))
            SQLRunner.Add("RewRepValue1", CStr(nudQuestRepRew1.Value))
            SQLRunner.Add("RewRepValue2", CStr(nudQuestRepRew2.Value))
            SQLRunner.Add("RewRepValue3", CStr(nudQuestRepRew2.Value))
            SQLRunner.Add("RewRepValue4", CStr(nudQuestRepRew2.Value))
            SQLRunner.Add("RewRepValue5", CStr(nudQuestRepRew2.Value))
            If glcQuestGoldReq.Enabled = False Then
                SQLRunner.Add("RewOrReqMoney", CStr(glcQuestGoldRew.GetValue()))
            Else
                SQLRunner.Add("RewOrReqMoney", CStr(ToOppositeSign(glcQuestGoldReq.GetValue())))
            End If
            SQLRunner.Add("RewMoneyMaxLevel", CStr(glcQuestMaxLvl.GetValue))
            SQLRunner.Add("RewSpell", mtbQuestSpellRew.Text)
            SQLRunner.Add("RewSpellCast", mtbQuestCast.Text)

            Sen = 1
            Query = New MySqlCommand("SELECT `entry` FROM `quest_template` WHERE `entry` = '" & mtbQuestEntry.Text & "' LIMIT 1;", Connection)
            Reader = Query.ExecuteReader()

            If Reader.HasRows And mtbQuestEntry.Text <> "" Then
                Reader.Close()
                If MsgBox("An entry with this ID (" & mtbQuestEntry.Text & ") is already used by other quest." & vbCrLf & "Do you want to change this quest details to the details you just entered?" & vbCrLf & "Warning: You can't reverse this action.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Quest Update") = MsgBoxResult.Yes Then
                    Sen = 2
                    SQLRunner.RunUPDATE("quest_template", CInt(mtbQuestEntry.Text))
                    MsgBox("Quest: " & mtbQuestEntry.Text & " has been updated succesfully.", MsgBoxStyle.Information)
                End If
            Else
                If mtbQuestEntry.Text <> "" Then
                    Reader.Close()

                    Sen = 4
                    SQLRunner.Add("entry", mtbQuestEntry.Text)
                    SQLRunner.RunINSERT("quest_template")

                    MsgBox("Quest created successfuly.", MsgBoxStyle.Information, "Quest Editor")
                Else
                    Reader.Close()

                    Sen = 6
                    Query = New MySqlCommand("SELECT `entry` FROM `quest_template` WHERE " & _
                                             "`entry` >=100000 ORDER BY `entry` DESC LIMIT 1;", Connection)
                    Reader = Query.ExecuteReader()

                    If Reader.HasRows Then
                        Reader.Read()
                        QuestStartEntry = CInt(Reader.GetInt64(0)) + 1
                    End If

                    Reader.Close()

                    Sen = 7
                    SQLRunner.Add("entry", CStr(QuestStartEntry))
                    SQLRunner.RunINSERT("quest_template")
                    mtbQuestEntry.Text = CStr(QuestStartEntry)

                    MsgBox("Quest created successfuly as entry #" & QuestStartEntry & ".", MsgBoxStyle.Information, "Quest Editor")
                End If
            End If
        Catch Exception
            If Reader IsNot Nothing Then If Not Reader.IsClosed Then Reader.Close()

            Clipboard.Clear()
            Clipboard.SetText("(" & Sen & ") " & Exception.Message)
            MsgBox("Error! Quest wasn't updated/added successfuly." & vbCrLf & "Please post this error at information thread (the error copied to the clipboard):" & vbCrLf & "(" & Sen & ") " & Exception.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub btnQuestSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestSource.Click
        CallSearch("Item", mtbQuestSource)
    End Sub

    Private Sub btnQuestSSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnQuestSSource.Click
        CallSearch("Spell", mtbQuestSSource)
    End Sub

    Private Sub cmbQuestType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbQuestType.SelectedIndexChanged
        If cmbQuestType.SelectedIndex = 3 Then
            cmbQuestZoneID.Enabled = False
            cmbQuestSort.Enabled = False
        Else
            cmbQuestZoneID.Enabled = True
            cmbQuestSort.Enabled = True
        End If
    End Sub
#End Region
#End Region

#Region " Loot Tab "
#Region " Loot Global Functions "
    Private Sub LLUnediting()
        LLSubUnediting()

        grpLLMain.Enabled = True
        grpLLOf.Enabled = True
        grpLLContents.Enabled = False
        grpLLFinish.Enabled = False

        lsvLLContents.Items.Clear()
    End Sub

    Private Sub LLEditing()
        grpLLMain.Enabled = False
        grpLLOf.Enabled = False
        grpLLContents.Enabled = True
        grpLLEdit.Enabled = False
        grpLLFinish.Enabled = True
    End Sub

    Private Sub LLSubEditing()
        grpLLEdit.Enabled = True
        grpLLContents.Enabled = False
        grpLLFinish.Enabled = False
    End Sub

    Private Sub LLSubUnediting()
        grpLLEdit.Enabled = False
        grpLLContents.Enabled = True
        grpLLFinish.Enabled = True
        mtbLLItemID.Text = "0"
        nudLLMin.Value = 1
        nudLLMax.Value = 1
        txtLLChance.Text = "20%"
        btnLLDo.Text = "Add"
    End Sub
#End Region
#Region " Loot Form Functions"
    Private Sub radLLNPCsRads_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radLLCreature.CheckedChanged, radLLPickPocketing.CheckedChanged, radLLSkinning.CheckedChanged
        If CType(sender, RadioButton).Checked Then
            lblELLID.Text = "NPC ID:"
            btnLLChoose.Enabled = True
        End If
    End Sub

    Private Sub radLLGOsRads_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radLLGO.CheckedChanged
        If CType(sender, RadioButton).Checked Then
            lblELLID.Text = "GO ID:"
            btnLLChoose.Enabled = True
        End If
    End Sub

    Private Sub radLLZonesRads_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radLLFishing.CheckedChanged
        If CType(sender, RadioButton).Checked Then
            lblELLID.Text = "Zone ID:"
            btnLLChoose.Enabled = False
        End If
    End Sub

    Private Sub radLLItemsRads_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radLLItems.CheckedChanged, radLLProspecting.CheckedChanged
        If CType(sender, RadioButton).Checked Then
            lblELLID.Text = "Item ID:"
            btnLLChoose.Enabled = True
        End If
    End Sub

    Private Sub btnLLCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLCancel.Click
        LLUnediting()
    End Sub

    Private Sub btnLLEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLEdit.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim Item As ListViewItem
        Dim I As Integer
        Dim entryID As String = mtbLLID.Text

        If Trim(mtbLLID.Text) = "" Then Exit Sub

        Select Case True
            Case radLLCreature.Checked : LinkerTable = "creature_loot_template"
                Query = New MySqlCommand("SELECT `lootid` FROM `creature_template` WHERE `entry`='" & mtbLLID.Text & "';", Connection)
                Reader = Query.ExecuteReader()
                Reader.Read()
                entryID = Reader.GetString("lootid")
                Reader.Close()
            Case radLLFishing.Checked : LinkerTable = "fishing_loot_template"
            Case radLLGO.Checked : LinkerTable = "gameobject_loot_template"
            Case radLLItems.Checked : LinkerTable = "item_loot_template"
            Case radLLPickPocketing.Checked : LinkerTable = "pickpocketing_loot_template"
                Query = New MySqlCommand("SELECT `pickpocketloot` FROM `creature_template` WHERE `entry`='" & mtbLLID.Text & "';", Connection)
                Reader = Query.ExecuteReader()
                Reader.Read()
                entryID = Reader.GetString("pickpocketloot")
                Reader.Close()
            Case radLLProspecting.Checked : LinkerTable = "prospecting_loot_template"
            Case radLLSkinning.Checked : LinkerTable = "skinning_loot_template"
                Query = New MySqlCommand("SELECT `skinloot` FROM `creature_template` WHERE `entry`='" & mtbLLID.Text & "';", Connection)
                Reader = Query.ExecuteReader()
                Reader.Read()
                entryID = Reader.GetString("skinloot")
                Reader.Close()
        End Select

        Query = New MySqlCommand("SELECT `item`, `ChanceOrQuestChance`, `mincountOrRef`, `maxcount` FROM `" & LinkerTable & "` WHERE `entry` = " & entryID & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                Item = lsvLLContents.Items.Add(CStr(Reader.GetInt64("item")))
                Item.SubItems.Add("")
                Item.SubItems.Add(CStr(Reader.GetFloat("ChanceOrQuestChance")) & "%")
                Item.SubItems.Add(CStr(Reader.GetInt32("mincountOrRef")))
                Item.SubItems.Add(CStr(Reader.GetInt32("maxcount")))
            End While

            'For I = 0 To (lsvLLContents.Items.Count - 1)
            '    lsvLLContents.Items(I).SubItems(1).Text = GetItemName(CLng(lsvLLContents.Items(I).SubItems(0).Text))
            'Next
        ElseIf Reader.HasRows = False Then
            MsgBox("No loot found", MsgBoxStyle.Information, "Loot Editor")
        End If
        Reader.Close()

        For I = 0 To (lsvLLContents.Items.Count - 1)
            lsvLLContents.Items(I).SubItems(1).Text = GetItemName(CLng(lsvLLContents.Items(I).SubItems(0).Text))
        Next

        LLEditing()
    End Sub

    Private Sub btnLLRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLRemove.Click
        If lsvLLContents.SelectedItems.Count = 1 Then lsvLLContents.Items.RemoveAt(lsvLLContents.SelectedItems(0).Index)
    End Sub

    Private Sub btnLLAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLAdd.Click
        Adding = True
        btnLLDo.Text = "Add"
        LLSubEditing()
    End Sub

    Private Sub btnLLEditLoot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLEditLoot.Click
        Dim Item As ListViewItem
        If lsvLLContents.SelectedItems.Count = 0 Then Exit Sub

        Adding = False
        btnLLDo.Text = "Edit"
        LLSubEditing()

        Item = lsvLLContents.SelectedItems(0)
        mtbLLItemID.Text = Item.SubItems(0).Text
        txtLLChance.Text = Item.SubItems(2).Text
        nudLLMin.Value = CInt(Item.SubItems(3).Text)
        nudLLMax.Value = CInt(Item.SubItems(4).Text)
    End Sub

    Private Sub btnLLCancelEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLCancelEdit.Click
        LLSubUnediting()
    End Sub

    Private Sub txtLLChance_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLLChance.Leave
        Try
            Dim Text As String = txtLLChance.Text

            If (Trim(Text) = "") Then
                txtLLChance.Text = "0%"
            Else
                If (Text.Substring(Text.Length - 1, 1) = "%") Then
                    Text = Text.Substring(0, Text.Length - 1)
                End If

                Dim Test As Decimal = CDec(Text)
                txtLLChance.Text = CStr(Test) & "%"
            End If
        Catch ex As Exception
            MsgBox("Please check your input.", MsgBoxStyle.Exclamation, "Loot Editor")
            txtLLChance.Focus()
        End Try
    End Sub

    Private Sub btnLLDo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLDo.Click
        Dim Item As ListViewItem

        If mtbLLItemID.Text.Trim() = "" Then Exit Sub

        If Adding Then
            Item = lsvLLContents.Items.Add(mtbLLItemID.Text)
            Item.SubItems.Add(GetItemName(CLng(mtbLLItemID.Text)))
            Item.SubItems.Add(txtLLChance.Text)
            Item.SubItems.Add(CStr(nudLLMin.Value))
            Item.SubItems.Add(CStr(nudLLMax.Value))
        Else
            Item = lsvLLContents.SelectedItems(0)
            Item.SubItems(0).Text = mtbLLItemID.Text
            Item.SubItems(1).Text = GetItemName(CLng(mtbLLItemID.Text))
            Item.SubItems(2).Text = txtLLChance.Text
            Item.SubItems(3).Text = CStr(nudLLMin.Value)
            Item.SubItems(4).Text = CStr(nudLLMax.Value)
        End If

        LLSubUnediting()
    End Sub

    Private Sub btnLLChooseItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLChooseItem.Click
        Dim ItemChooser As frmChooseItem

        Me.Enabled = False
        ItemChooser = New frmChooseItem
        ItemChooser.Field = mtbLLItemID
        ItemChooser.Show(Me)
    End Sub

    Private Sub btnLLChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLChoose.Click
        Select Case True
            Case radLLCreature.Checked, radLLPickPocketing.Checked, radLLSkinning.Checked
                Dim NPCChooser As frmChooseNPC

                Me.Enabled = False
                NPCChooser = New frmChooseNPC
                NPCChooser.Field = mtbLLID
                NPCChooser.Show(Me)
            Case radLLItems.Checked, radLLProspecting.Checked
                Dim ItemChooser As frmChooseItem

                Me.Enabled = False
                ItemChooser = New frmChooseItem
                ItemChooser.Field = mtbLLID
                ItemChooser.Show(Me)
            Case radLLGO.Checked
                Dim GOChooser As frmChooseGO

                Me.Enabled = False
                GOChooser = New frmChooseGO
                GOChooser.Field = mtbLLID
                GOChooser.Show(Me)
            Case radLLFishing.Checked
                Dim ZoneChooser As frmChooseZone

                Me.Enabled = False
                ZoneChooser = New frmChooseZone
                ZoneChooser.Field = mtbLLID
                ZoneChooser.Show(Me)
        End Select
    End Sub

    Private Sub btnLLUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLLUpdate.Click
        Dim I As Integer
        Dim Items As ListViewItem.ListViewSubItemCollection
        Dim entryID As String

        If LinkerTable = "creature_loot_template" Then
            Dim Query As MySqlCommand
            Dim Reader As MySqlDataReader
            Query = New MySqlCommand("SELECT `lootid` FROM `creature_template` WHERE entry='" & mtbLLID.Text & "';", Connection)
            Reader = Query.ExecuteReader()
            Reader.Read()
            entryID = Reader.GetString("lootid")
            Reader.Close()
        ElseIf LinkerTable = "skinning_loot_template" Then
            Dim Query As MySqlCommand
            Dim Reader As MySqlDataReader
            Query = New MySqlCommand("SELECT * FROM `creature_template` WHERE entry='" & mtbLLID.Text & "';", Connection)
            Reader = Query.ExecuteReader()
            Reader.Read()
            entryID = Reader.GetString("skinloot")
            Reader.Close()
        ElseIf LinkerTable = "pickpocketing_loot_template" Then
            Dim Query As MySqlCommand
            Dim Reader As MySqlDataReader
            Query = New MySqlCommand("SELECT `pickpocketloot` FROM `creature_template` WHERE entry='" & mtbLLID.Text & "';", Connection)
            Reader = Query.ExecuteReader()
            Reader.Read()
            entryID = Reader.GetString("pickpocketloot")
            Reader.Close()
        Else
            entryID = mtbLLID.Text
        End If

        ExecuteQuery("DELETE FROM `" & LinkerTable & "` WHERE `entry` = " & entryID & ";")
        Try
            For I = 0 To (lsvLLContents.Items.Count - 1)
                Items = lsvLLContents.Items(I).SubItems
                ExecuteQuery("INSERT INTO `" & LinkerTable & "` (`entry`, `item`, `ChanceOrQuestChance`, `mincountOrRef`, `maxcount`) VALUES ('" & entryID & "', '" & Items(0).Text & "', '" & CStr(Items(2).Text.Substring(0, Items(2).Text.Length - 1)) & "', '" & Items(3).Text & "', '" & Items(4).Text & "');")
            Next

        Catch lootError As Exception
            MsgBox("An error has occurred, please report it to the devs: " & lootError.Message, MsgBoxStyle.Critical)

        Finally
            MsgBox("Loot information has been updated succesfully", MsgBoxStyle.Information, "Loot Editor")
            LLUnediting()
        End Try
    End Sub
#End Region
#End Region

#Region " Linker Tab "
#Region " Global Functions "
    Private Sub LQUnediting()
        grpLQMain.Enabled = True
        grpLQNStarters.Enabled = False
        grpLQNFinishers.Enabled = False
        grpLQGStarters.Enabled = False
        grpLQGFinishers.Enabled = False
        grpLQIStarters.Enabled = False
        grpLQFinish.Enabled = False

        lstLQStarters.Items.Clear()
        lstLQFinishers.Items.Clear()
        lstLQGStarters.Items.Clear()
        lstLQGFinishers.Items.Clear()
        lstLQIStarters.Items.Clear()
    End Sub

    Private Sub LQEditing()
        grpLQMain.Enabled = False
        grpLQNStarters.Enabled = True
        grpLQNFinishers.Enabled = True
        grpLQFinish.Enabled = True
        grpLQGStarters.Enabled = True
        grpLQGFinishers.Enabled = True
        grpLQIStarters.Enabled = True
    End Sub

    Private Sub LNUnediting()
        grpLNMain.Enabled = True
        grpLNVendor.Enabled = False
        grpLNStart.Enabled = False
        grpLNFinish.Enabled = False
        grpLNOk.Enabled = False
        mtbLNExtendedCost.Enabled = False

        nudLNRestockTime.Value = 1
        lsvLNVendors.Items.Clear()
        lstLNStart.Items.Clear()
        lstLNFinish.Items.Clear()
    End Sub

    Private Sub LNEditing()
        grpLNMain.Enabled = False
        grpLNVendor.Enabled = True
        grpLNStart.Enabled = True
        grpLNFinish.Enabled = True
        grpLNOk.Enabled = True

        mtbLNExtendedCost.Enabled = True
    End Sub
#End Region

#Region " Quest "
    Private Sub btnLQChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQChoose.Click
        CallSearch("Quest", mtbLQID)
    End Sub

    Private Sub btnLQEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQEdit.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim I As Integer = 0

        If Trim(mtbLQID.Text) = "" Then Exit Sub

        Query = New MySqlCommand("SELECT creature_questrelation.id, creature_template.name FROM " & _
                                 "creature_questrelation INNER JOIN creature_template ON " & _
                                 "creature_questrelation.id = creature_template.entry WHERE " & _
                                 "creature_questrelation.quest = " & mtbLQID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLQStarters.Items.Add(I)
                lstLQStarters.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        I = 0
        Query = New MySqlCommand("SELECT creature_involvedrelation.id, creature_template.name FROM " & _
                                 "creature_involvedrelation INNER JOIN creature_template ON " & _
                                 "creature_involvedrelation.id = creature_template.entry WHERE " & _
                                 "creature_involvedrelation.quest = " & mtbLQID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLQFinishers.Items.Add(I)
                lstLQFinishers.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        I = 0
        Query = New MySqlCommand("SELECT gameobject_questrelation.id, gameobject_template.name FROM " & _
                                 "gameobject_questrelation INNER JOIN gameobject_template ON " & _
                                 "gameobject_questrelation.id = gameobject_template.entry WHERE " & _
                                 "gameobject_questrelation.quest = " & mtbLQID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLQGStarters.Items.Add(I)
                lstLQGStarters.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        I = 0
        Query = New MySqlCommand("SELECT gameobject_involvedrelation.id, gameobject_template.name FROM " & _
                                 "gameobject_involvedrelation INNER JOIN gameobject_template ON " & _
                                 "gameobject_involvedrelation.id = gameobject_template.entry WHERE " & _
                                 "gameobject_involvedrelation.quest = " & mtbLQID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLQGFinishers.Items.Add(I)
                lstLQGFinishers.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        I = 0
        Query = New MySqlCommand("SELECT entry, name FROM item_template WHERE startquest = " & mtbLQID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLQIStarters.Items.Add(0)
                lstLQIStarters.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        LQEditing()
    End Sub

    Private Sub btnLQStartRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQStartRem.Click
        If lstLQStarters.SelectedIndex > -1 Then lstLQStarters.Items.RemoveAt(lstLQStarters.SelectedIndex)
    End Sub

    Private Sub btnLQFinishRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQFinishRem.Click
        If lstLQFinishers.SelectedIndex > -1 Then lstLQFinishers.Items.RemoveAt(lstLQFinishers.SelectedIndex)
    End Sub

    Private Sub btnLQStartAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQStartAdd.Click
        Me.Enabled = False
        frmChooseNPC.List = lstLQStarters
        frmChooseNPC.Show(Me)
    End Sub

    Private Sub btnLQFinishAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQFinishAdd.Click
        Me.Enabled = False
        frmChooseNPC.List = lstLQFinishers
        frmChooseNPC.Show(Me)
    End Sub

    Private Sub btnLQCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQCancel.Click
        LQUnediting()
    End Sub

    Private Sub btnLQUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQUpdate.Click
        Dim I As Integer
        Dim Temp As String

        ExecuteQuery("DELETE FROM `creature_questrelation` WHERE `quest` = " & mtbLQID.Text & ";")
        ExecuteQuery("DELETE FROM `creature_involvedrelation` WHERE `quest` = " & mtbLQID.Text & ";")

        ExecuteQuery("DELETE FROM `gameobject_questrelation` WHERE `quest` = " & mtbLQID.Text & ";")
        ExecuteQuery("DELETE FROM `gameobject_involvedrelation` WHERE `quest` = " & mtbLQID.Text & ";")

        ExecuteQuery("UPDATE `item_template` SET `startquest` = 0 WHERE `startquest` = '" & mtbLQID.Text & "';")

        For I = 0 To (lstLQStarters.Items.Count - 1)
            Temp = CStr(lstLQStarters.Items(I))
            ExecuteQuery("INSERT INTO `creature_questrelation` (`id`, `quest`) VALUES ('" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "', '" & mtbLQID.Text & "');")
        Next
        For I = 0 To (lstLQFinishers.Items.Count - 1)
            Temp = CStr(lstLQFinishers.Items(I))
            ExecuteQuery("INSERT INTO `creature_involvedrelation` (`id`, `quest`) VALUES ('" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "', '" & mtbLQID.Text & "');")
        Next
        For I = 0 To (lstLQGStarters.Items.Count - 1)
            Temp = CStr(lstLQGStarters.Items(I))
            ExecuteQuery("INSERT INTO `gameobject_questrelation` (`id`, `quest`) VALUES ('" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "', '" & mtbLQID.Text & "');")
        Next
        For I = 0 To (lstLQGFinishers.Items.Count - 1)
            Temp = CStr(lstLQGFinishers.Items(I))
            ExecuteQuery("INSERT INTO `gameobject_involvedrelation` (`id`, `quest`) VALUES ('" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "', '" & mtbLQID.Text & "');")
        Next
        For I = 0 To (lstLQIStarters.Items.Count - 1)
            Temp = CStr(lstLQIStarters.Items(I))
            ExecuteQuery("UPDATE `item_template` SET `startquest` = '" & mtbLQID.Text & "' WHERE `entry` = '" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "' LIMIT 1;")
        Next

        MsgBox("Data has been inputted succesfully", MsgBoxStyle.Information, "Linker")
        btnLQCancel_Click(Nothing, Nothing)
    End Sub

    Private Sub btnLQGStartAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQGStartAdd.Click
        Me.Enabled = False
        frmChooseGO.List = lstLQGStarters
        frmChooseGO.Show(Me)
    End Sub

    Private Sub btnLQGStartRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQGStartRem.Click
        If lstLQGStarters.SelectedIndex > -1 Then
            lstLQGStarters.Items.RemoveAt(lstLQGStarters.SelectedIndex)
        End If
    End Sub

    Private Sub btnLQGFinishAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQGFinishAdd.Click
        Me.Enabled = False
        frmChooseGO.List = lstLQGFinishers
        frmChooseGO.Show(Me)
    End Sub

    Private Sub btnLQGFinishRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQGFinishRem.Click
        If lstLQGFinishers.SelectedIndex > -1 Then
            lstLQGFinishers.Items.RemoveAt(lstLQGFinishers.SelectedIndex)
        End If
    End Sub

    Private Sub btnLQIStartAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQIStartAdd.Click
        Me.Enabled = False
        frmChooseItem.List = lstLQIStarters
        frmChooseItem.Show(Me)
    End Sub

    Private Sub btnLQIStartRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLQIStartRem.Click
        If lstLQIStarters.SelectedIndex > -1 Then
            lstLQIStarters.Items.RemoveAt(lstLQIStarters.SelectedIndex)
        End If
    End Sub
#End Region

#Region " NPC "
    Private Sub btnLNChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNChoose.Click
        Me.Enabled = False
        frmChooseNPC.Field = mtbLNID
        frmChooseNPC.Show(Me)
    End Sub

    Private Sub btnLNEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNEdit.Click
        Dim Reader As MySqlDataReader
        Dim Query As MySqlCommand
        Dim I As Integer = 0
        Dim Item As ListViewItem

        If Trim(mtbLNID.Text) = "" Then Exit Sub

        Query = New MySqlCommand("SELECT npc_vendor.item, item_template.name, " & _
                                 "npc_vendor.maxcount, npc_vendor.incrtime, npc_vendor.ExtendedCost " & _
                                 "FROM npc_vendor INNER JOIN item_template ON " & _
                                 "npc_vendor.item = item_template.entry WHERE " & _
                                 "npc_vendor.entry = " & mtbLNID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                Item = lsvLNVendors.Items.Add(CStr(Reader.GetInt64(0)))
                Item.SubItems.Add(CStr(Reader.GetString(1)))
                Item.SubItems.Add(CStr(Reader.GetInt32(2)))
                Item.SubItems.Add(CStr(Reader.GetInt32(3)))
                Item.SubItems.Add(CStr(Reader.GetInt32(4)))
            End While
        End If
        Reader.Close()

        Query = New MySqlCommand("SELECT creature_questrelation.quest, quest_template.Title FROM " & _
                                 "creature_questrelation INNER JOIN quest_template ON " & _
                                 "creature_questrelation.quest = quest_template.entry WHERE " & _
                                 "creature_questrelation.id = " & mtbLNID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLNStart.Items.Add(I)
                lstLNStart.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        I = 0
        Query = New MySqlCommand("SELECT creature_involvedrelation.quest, quest_template.Title FROM " & _
                                 "creature_involvedrelation INNER JOIN quest_template ON " & _
                                 "creature_involvedrelation.quest = quest_template.entry WHERE " & _
                                 "creature_involvedrelation.id = " & mtbLNID.Text & ";", Connection)
        Reader = Query.ExecuteReader()
        If Reader.HasRows Then
            While (Reader.Read())
                lstLNFinish.Items.Add(I)
                lstLNFinish.Items(I) = CStr(Reader.GetString(1)) + " [" + CStr(Reader.GetInt64(0)) + "]"
                I = I + 1
            End While
        End If
        Reader.Close()

        LNEditing()
    End Sub

    Private Sub btnLNSAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNSAdd.Click
        Me.Enabled = False
        frmChooseQuest.List = lstLNStart
        frmChooseQuest.Show(Me)
    End Sub

    Private Sub btnLNFAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNFAdd.Click
        Me.Enabled = False
        frmChooseQuest.List = lstLNFinish
        frmChooseQuest.Show(Me)
    End Sub

    Private Sub btnLNSRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNSRemove.Click
        If lstLNStart.SelectedIndex > -1 Then lstLNStart.Items.RemoveAt(lstLNStart.SelectedIndex)
    End Sub

    Private Sub btnLNFRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNFRemove.Click
        If lstLNFinish.SelectedIndex > -1 Then lstLNFinish.Items.RemoveAt(lstLNFinish.SelectedIndex)
    End Sub

    Private Sub btnLNCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNCancel.Click
        LNUnediting()
    End Sub

    Private Sub btnLNVRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNVRemove.Click
        If lsvLNVendors.SelectedItems.Count = 1 Then lsvLNVendors.Items.RemoveAt(lsvLNVendors.SelectedItems(0).Index)
    End Sub

    Private Sub btnLNVAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNVAdd.Click
        Me.Enabled = False
        frmChooseItem.Linker = True
        frmChooseItem.Show(Me)
    End Sub

    Private Sub btnLNUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLNUpdate.Click
        Dim I As Integer
        Dim Temp As String

        ExecuteQuery("DELETE FROM `creature_questrelation` WHERE `id` = " & mtbLNID.Text & ";")
        ExecuteQuery("DELETE FROM `creature_involvedrelation` WHERE `id` = " & mtbLNID.Text & ";")

        For I = 0 To (lstLNStart.Items.Count - 1)
            Temp = CStr(lstLNStart.Items(I))
            ExecuteQuery("INSERT INTO `creature_questrelation` (`id`, `quest`) VALUES ('" & mtbLNID.Text & "', '" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "');")
        Next
        For I = 0 To (lstLNFinish.Items.Count - 1)
            Temp = CStr(lstLNFinish.Items(I))
            ExecuteQuery("INSERT INTO `creature_involvedrelation` (`id`, `quest`) VALUES ('" & mtbLNID.Text & "', '" & Temp.Substring(Temp.IndexOf("[") + 1, Temp.IndexOf("]") - Temp.IndexOf("[") - 1) & "');")
        Next


        ExecuteQuery("DELETE FROM `npc_vendor` WHERE `entry` = " & mtbLNID.Text & ";")

        For I = 0 To (lsvLNVendors.Items.Count - 1)
            ExecuteQuery("INSERT INTO `npc_vendor` (`entry`, `item`, `maxcount`, `incrtime`, `ExtendedCost`) VALUES ('" & mtbLNID.Text & "', '" & lsvLNVendors.Items(I).SubItems(0).Text & "', '" & lsvLNVendors.Items(I).SubItems(2).Text & "', '" & lsvLNVendors.Items(I).SubItems(3).Text & "', '" & lsvLNVendors.Items(I).SubItems(4).Text & "');")
        Next

        MsgBox("Data has been inputted succesfully", MsgBoxStyle.Information, "Linker")
        LNUnediting()
    End Sub
#End Region
#End Region
End Class