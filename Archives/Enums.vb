' item columns ENUM, obsolete as of rev 46
Public Enum ItemColumn As Integer
    itemClass = 0
    SubClass = 1
    Name = 2
    DisplayId = 3
    Quality = 4
    BuyPrice = 7
    SellPrice = 8
    ItemLevel = 12
    InventoryType = 9
    MaxCount = 21
    Bonding = 104

    ContainerSlots = 23
    StatType_1 = 24
    StatValue_1 = 25
    StatType_2 = 26
    StatValue_2 = 27
    StatType_3 = 28
    StatValue_3 = 29
    StatType_4 = 30
    StatValue_4 = 31
    StatType_5 = 32
    StatValue_5 = 33
    StatType_6 = 34
    StatValue_6 = 35
    StatType_7 = 36
    StatValue_7 = 37
    StatType_8 = 38
    StatValue_8 = 39
    StatType_9 = 40
    StatValue_9 = 41

    DamageMin_1 = 44
    DamageMax_1 = 45
    DamageType_1 = 46
    DamageMin_2 = 47
    DamageMax_2 = 48
    DamageType_2 = 49
    DamageMin_3 = 50
    DamageMax_3 = 51
    DamageType_3 = 52
    DamageMin_4 = 53
    DamageMax_4 = 54
    DamageType_4 = 55
    DamageMin_5 = 56
    DamageMax_5 = 57
    DamageType_5 = 58
    Delay = 66
    Range = 68

    HolyRes = 60
    FireRes = 61
    NatureRes = 62
    FrostRes = 63
    ShadowRes = 64
    ArcaneRes = 65

    Armor = 59
    Block = 115

    SpellID_1 = 69
    SpellTrigger_1 = 70
    SpellCharges_1 = 71
    SpellCooldown_1 = 73
    SpellCategory_1 = 74
    SpellCategoryCooldown_1 = 75
    SpellID_2 = 76
    SpellTrigger_2 = 77
    SpellCharges_2 = 78
    SpellCooldown_2 = 80
    SpellCategory_2 = 81
    SpellCategoryCooldown_2 = 82
    SpellID_3 = 83
    SpellTrigger_3 = 84
    SpellCharges_3 = 85
    SpellCooldown_3 = 87
    SpellCategory_3 = 88
    SpellCategoryCooldown_3 = 89
    SpellID_4 = 90
    SpellTrigger_4 = 91
    SpellCharges_4 = 92
    SpellCooldown_4 = 94
    SpellCategory_4 = 95
    SpellCategoryCooldown_4 = 96
    SpellID_5 = 97
    SpellTrigger_5 = 98
    SpellCharges_5 = 100
    SpellCooldown_5 = 101
    SpellCategory_5 = 102
    SpellCategoryCooldown_5 = 103

    AllowableClass = 10
    AllowableRace = 11

    RequiredSkill = 14
    RequiredSkillRank = 15
    RequiredFaction = 19
    RequiredFactionStanding = 20
    RequiredLevel = 13

    StartsQuestID = 109
    Sheath = 112
    ItemSet = 116
    MaxDurability = 117
    Description = 105
    PageText = 106

    SocketColor_1 = 122
    SocketContent_1 = 123
    SocketColor_2 = 124
    SocketContent_2 = 125
    SocketColor_3 = 126
    SocketContent_3 = 127
    SocketBonus = 128

    ItemUnique = 5

End Enum

' creature columns ENUM, obsolete as of rev 46
Public Enum CreatureColumn As Integer
    MinHealth = 3
    MinMana = 5
    MaxHealth = 4
    MaxMana = 6

    Scale = 7
    NpcFlag = 8
    RunSpeed = 12
    Armor = 17

    BaseAtkTime = 9
    MinDmg = 10
    MaxDmg = 11
    AtkPwr = 18

    RngAtkTime = 13
    MinRngDmg = 14
    MaxRngDmg = 15

    EquipId = 16

    HolyRes = 19
    FireRes = 20
    NatureRes = 21
    FrostRes = 22
    ShadowRes = 23
    ArcaneRes = 24

    MinGold = 25
    MaxGold = 26
End Enum