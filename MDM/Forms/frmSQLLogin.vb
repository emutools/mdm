Option Strict On
Option Explicit On

Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions

Public Class frmManager

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtHost.KeyDown, txtPort.KeyDown, txtPass.KeyDown, txtUser.KeyDown, txtDatabase.KeyDown, cmbSavedConnections.KeyDown
        If e.KeyCode = Keys.Enter Then
            bttnConnect_Click(sender, e)
            e.Handled = True
        End If
    End Sub

    Private Sub bttnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnConnect.Click
        Me.Enabled = False

        Dim AllInformationInputted As Boolean = False
        If txtHost.Text = "" Then
            AllInformationInputted = False
        ElseIf txtUser.Text = "" Then
            AllInformationInputted = False
        ElseIf txtPass.Text = "" Then
            AllInformationInputted = False
        ElseIf txtPort.Text = "" Then
            AllInformationInputted = False
        ElseIf txtDatabase.Text = "" Then
            AllInformationInputted = False
        Else
            AllInformationInputted = True
        End If

        If AllInformationInputted = False Then
            MsgBox("Please make sure you have entered in all the information before continuing.", MsgBoxStyle.Critical)
            Me.Enabled = True
            If txtHost.Text = "" Then
                MsgBox("No host entered. Please enter a valid host IP.", MsgBoxStyle.Critical)
                txtHost.Focus()
            ElseIf txtUser.Text = "" Then
                MsgBox("No username entered. Please enter a valid username.", MsgBoxStyle.Critical)
                txtUser.Focus()
            ElseIf txtPass.Text = "" Then
                MsgBox("No password entered. Please enter a valid password.", MsgBoxStyle.Critical)
                txtPass.Focus()
            ElseIf txtPort.Text = "" Then
                MsgBox("No port entered. Please enter a valid port.", MsgBoxStyle.Critical)
                txtPort.Focus()
            ElseIf txtDatabase.Text = "" Then
                MsgBox("No database entered. Please enter a valid database.", MsgBoxStyle.Critical)
                txtDatabase.Focus()
            End If
            GoTo Here
        End If

        Username = txtUser.Text
        Password = txtPass.Text
        DB = txtDatabase.Text
        Host = txtHost.Text
        Port = CInt(txtPort.Text)

        Me.Enabled = False

        If Not ConnectToDB() Then
            MsgBox("Error while connecting, please check to see if the server is on & that the connection details are correct.", MsgBoxStyle.Critical, "Error")
            Me.Enabled = True
        Else
            MsgBox("Connection succesful!", MsgBoxStyle.Information)
            ' hide login form
            Me.Hide()
            ' show manager form
            frmManageMain.Show()
Here:
        End If
    End Sub

    Private Sub frmManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load user settings
        Dim FilePath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\MDMConnections.dat"

        If (File.Exists(FilePath)) = True Then
            LoadConfigData()
        Else
            MsgBox("You do not have any saved connections." + vbCrLf + "It is now possible to save various connection settings.", MsgBoxStyle.Information)

            Dim EncryptKey As String = My.Settings.GUID

            If (EncryptKey = Nothing Or EncryptKey = "") Then
                EncryptKey = Guid.NewGuid().ToString()
                My.Settings.GUID = EncryptKey
                My.Settings.Save()
            End If

            EnableControls()
            cmbSavedConnections.DataSource = Nothing
            cmbSavedConnections.Items.Clear()
            cmbSavedConnections.Enabled = False
            txtUser.Text = "root"
            txtPass.Text = ""
            txtHost.Text = "localhost"
            txtDatabase.Text = ""
            txtPort.Text = "3306"
        End If

    End Sub

    Private Sub EnableControls()
        txtDatabase.Enabled = True
        txtHost.Enabled = True
        txtPass.Enabled = True
        txtPort.Enabled = True
        txtUser.Enabled = True
    End Sub

    Private Sub DisableControls()
        txtDatabase.Enabled = False
        txtHost.Enabled = False
        txtPass.Enabled = False
        txtPort.Enabled = False
        txtUser.Enabled = False
    End Sub

    Private Sub LoadConfigData()
        Dim ConfigEncrypt As New ConfigEditor.ConfigEncryptor()
        Dim EncryptKey As String = My.Settings.GUID
        Dim DataPath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\"
        Dim ConfigFile As String = DataPath + "MDMConnections.dat"

        ConfigEncrypt.DecryptConfig(ConfigFile, DataPath, EncryptKey)

        Dim Reader As StreamReader = New StreamReader(DataPath + "MDMConnections.dat")
        Dim Config As ConfigEditor.ConfigEditor = New ConfigEditor.ConfigEditor(Reader)
        Reader.Close()
        Dim KeyList As IList(Of String) = Config.GetKeyList()

        ConfigEncrypt.EncryptConfig(ConfigFile, DataPath, EncryptKey)

        cmbSavedConnections.DataSource = KeyList
    End Sub

    Private Sub bttnAddConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnAddConnection.Click
        cmbSavedConnections.Enabled = True
        cmbSavedConnections.Focus()
        bttnConnect.Enabled = False
        bttnAddConnection.Enabled = False
        bttnRemoveConnection.Enabled = False
        bttnEditConnection.Text = "Save"
        ClearFields()
        EnableControls()
    End Sub

    Private Sub bttnEditConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnEditConnection.Click
        If (bttnEditConnection.Text = "Save") Then
            Dim FilePath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\MDMConnections.dat"

            If File.Exists(FilePath) Then
                SaveConfig()
            Else
                SaveNewConfig()
            End If
        ElseIf (bttnEditConnection.Text = "Edit") Then
            If (cmbSavedConnections.Text = "") Then
                MsgBox("You must choose a connection to edit or add a connection first.", MsgBoxStyle.Critical)
            Else
                bttnConnect.Enabled = False
                bttnAddConnection.Enabled = False
                bttnRemoveConnection.Enabled = False
                bttnEditConnection.Text = "Save"
                EnableControls()
            End If
        End If
    End Sub

    Private Sub SaveConfig()
        Me.Enabled = False

        Dim AllInformationInputted As Boolean = False

        If cmbSavedConnections.Text = "" Then
            AllInformationInputted = False
        ElseIf txtHost.Text = "" Then
            AllInformationInputted = False
        ElseIf txtUser.Text = "" Then
            AllInformationInputted = False
        ElseIf txtPass.Text = "" Then
            AllInformationInputted = False
        ElseIf txtPort.Text = "" Then
            AllInformationInputted = False
        ElseIf txtDatabase.Text = "" Then
            AllInformationInputted = False
        Else
            AllInformationInputted = True
        End If

        If AllInformationInputted = False Then
            MsgBox("Please make sure you have entered in all the information before continuing.", MsgBoxStyle.Critical)
            Me.Enabled = True
            If cmbSavedConnections.Text = "" Then
                MsgBox("No name has been entered for this connection.", MsgBoxStyle.Critical)
                cmbSavedConnections.Focus()
            ElseIf txtHost.Text = "" Then
                MsgBox("No host entered. Please enter a valid host IP.", MsgBoxStyle.Critical)
                txtHost.Focus()
            ElseIf txtUser.Text = "" Then
                MsgBox("No username entered. Please enter a valid username.", MsgBoxStyle.Critical)
                txtUser.Focus()
            ElseIf txtPass.Text = "" Then
                MsgBox("No password entered. Please enter a valid password.", MsgBoxStyle.Critical)
                txtPass.Focus()
            ElseIf txtPort.Text = "" Then
                MsgBox("No port entered. Please enter a valid port.", MsgBoxStyle.Critical)
                txtPort.Focus()
            ElseIf txtDatabase.Text = "" Then
                MsgBox("No database entered. Please enter a valid database.", MsgBoxStyle.Critical)
                txtDatabase.Focus()
            End If
            GoTo Here
        End If

        Dim ConfigEncrypt As New ConfigEditor.ConfigEncryptor()
        Dim EncryptKey As String = My.Settings.GUID
        Dim DataPath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\"
        Dim ConfigFile As String = DataPath + "MDMConnections.dat"

        ConfigEncrypt.DecryptConfig(ConfigFile, DataPath, EncryptKey)

        Dim Reader As StreamReader = New StreamReader(DataPath + "MDMConnections.dat")
        Dim Config As ConfigEditor.ConfigEditor = New ConfigEditor.ConfigEditor(Reader)
        Reader.Close()
        Dim Writer As StreamWriter = New StreamWriter(DataPath + "MDMConnections.dat")

        Config.AddKey(cmbSavedConnections.Text)
        Config.SetValue(cmbSavedConnections.Text, "Host", txtHost.Text)
        Config.SetValue(cmbSavedConnections.Text, "User", txtUser.Text)
        Config.SetValue(cmbSavedConnections.Text, "Pass", txtPass.Text)
        Config.SetValue(cmbSavedConnections.Text, "Port", txtPort.Text)
        Config.SetValue(cmbSavedConnections.Text, "Database", txtDatabase.Text)
        Config.SaveConfig(Writer)
        Writer.Close()

        ConfigEncrypt.EncryptConfig(ConfigFile, DataPath, EncryptKey)

        LoadConfigData()
        bttnConnect.Enabled = True
        bttnAddConnection.Enabled = True
        bttnRemoveConnection.Enabled = True
        bttnEditConnection.Text = "Edit"
Here:
        Me.Enabled = True
    End Sub

    Private Sub SaveNewConfig()
        Me.Enabled = False

        Dim AllInformationInputted As Boolean = False

        If cmbSavedConnections.Text = "" Then
            AllInformationInputted = False
        ElseIf txtHost.Text = "" Then
            AllInformationInputted = False
        ElseIf txtUser.Text = "" Then
            AllInformationInputted = False
        ElseIf txtPass.Text = "" Then
            AllInformationInputted = False
        ElseIf txtPort.Text = "" Then
            AllInformationInputted = False
        ElseIf txtDatabase.Text = "" Then
            AllInformationInputted = False
        Else
            AllInformationInputted = True
        End If

        If AllInformationInputted = False Then
            MsgBox("Please make sure you have entered in all the information before continuing.", MsgBoxStyle.Critical)
            Me.Enabled = True
            If cmbSavedConnections.Text = "" Then
                MsgBox("No name has been entered for this connection.", MsgBoxStyle.Critical)
                cmbSavedConnections.Focus()
            ElseIf txtHost.Text = "" Then
                MsgBox("No host entered. Please enter a valid host IP.", MsgBoxStyle.Critical)
                txtHost.Focus()
            ElseIf txtUser.Text = "" Then
                MsgBox("No username entered. Please enter a valid username.", MsgBoxStyle.Critical)
                txtUser.Focus()
            ElseIf txtPass.Text = "" Then
                MsgBox("No password entered. Please enter a valid password.", MsgBoxStyle.Critical)
                txtPass.Focus()
            ElseIf txtPort.Text = "" Then
                MsgBox("No port entered. Please enter a valid port.", MsgBoxStyle.Critical)
                txtPort.Focus()
            ElseIf txtDatabase.Text = "" Then
                MsgBox("No database entered. Please enter a valid database.", MsgBoxStyle.Critical)
                txtDatabase.Focus()
            End If
            GoTo Here
        End If

        Dim ConfigEncrypt As New ConfigEditor.ConfigEncryptor()
        Dim EncryptKey As String = My.Settings.GUID
        Dim DataPath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\"
        Dim ConfigFile As String = DataPath + "MDMConnections.dat"

        Dim Writer As StreamWriter = New StreamWriter(DataPath + "MDMConnections.dat")
        Dim Config As ConfigEditor.ConfigEditor = New ConfigEditor.ConfigEditor()

        Config.AddKey(cmbSavedConnections.Text)
        Config.SetValue(cmbSavedConnections.Text, "Host", txtHost.Text)
        Config.SetValue(cmbSavedConnections.Text, "User", txtUser.Text)
        Config.SetValue(cmbSavedConnections.Text, "Pass", txtPass.Text)
        Config.SetValue(cmbSavedConnections.Text, "Port", txtPort.Text)
        Config.SetValue(cmbSavedConnections.Text, "Database", txtDatabase.Text)
        Config.SaveConfig(Writer)
        Writer.Close()

        ConfigEncrypt.EncryptConfig(ConfigFile, DataPath, EncryptKey)

        LoadConfigData()
        bttnConnect.Enabled = True
        bttnAddConnection.Enabled = True
        bttnRemoveConnection.Enabled = True
        bttnEditConnection.Text = "Edit"
Here:
        Me.Enabled = True
    End Sub

    Private Sub bttnRemoveConnection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnRemoveConnection.Click
        If (cmbSavedConnections.Items.Count = 1) Then
            Dim DataPath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\"
            File.Delete(DataPath + "MDMConnections.dat")
            frmManager_Load(sender, e)
        Else
            Dim ConfigEncrypt As New ConfigEditor.ConfigEncryptor()
            Dim EncryptKey As String = My.Settings.GUID
            Dim DataPath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\"
            Dim ConfigFile As String = DataPath + "MDMConnections.dat"

            ConfigEncrypt.DecryptConfig(ConfigFile, DataPath, EncryptKey)

            Dim Reader As StreamReader = New StreamReader(DataPath + "MDMConnections.dat")
            Dim Config As ConfigEditor.ConfigEditor = New ConfigEditor.ConfigEditor(Reader)
            Reader.Close()

            Config.RemoveKey(cmbSavedConnections.Text)

            Dim Writer As StreamWriter = New StreamWriter(DataPath + "MDMConnections.dat")
            Config.SaveConfig(Writer)
            Writer.Close()

            ConfigEncrypt.EncryptConfig(ConfigFile, DataPath, EncryptKey)

            LoadConfigData()
        End If
    End Sub

    Private Sub cmbSavedConnections_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbSavedConnections.SelectedIndexChanged
        If (cmbSavedConnections.Text = "") Then
            Exit Sub
        Else
            Dim ConfigEncrypt As New ConfigEditor.ConfigEncryptor()
            Dim EncryptKey As String = My.Settings.GUID
            Dim DataPath As String = Path.GetFullPath(Directory.GetCurrentDirectory) + "\data\"
            Dim ConfigFile As String = DataPath + "MDMConnections.dat"

            ConfigEncrypt.DecryptConfig(ConfigFile, DataPath, EncryptKey)

            Dim Reader As StreamReader = New StreamReader(DataPath + "MDMConnections.dat")
            Dim Config As ConfigEditor.ConfigEditor = New ConfigEditor.ConfigEditor(Reader)
            Reader.Close()

            If (Config.KeyExists(cmbSavedConnections.Text)) Then
                txtHost.Text = Config.GetValue(cmbSavedConnections.Text, "Host")
                txtPort.Text = Config.GetValue(cmbSavedConnections.Text, "Port")
                txtUser.Text = Config.GetValue(cmbSavedConnections.Text, "User")
                txtPass.Text = Config.GetValue(cmbSavedConnections.Text, "Pass")
                txtDatabase.Text = Config.GetValue(cmbSavedConnections.Text, "Database")
            End If
            DisableControls()

            ConfigEncrypt.EncryptConfig(ConfigFile, DataPath, EncryptKey)
        End If
    End Sub

    Private Sub ClearFields()
        cmbSavedConnections.Text = ""
        txtUser.Text = "root"
        txtPass.Text = ""
        txtHost.Text = "localhost"
        txtDatabase.Text = ""
        txtPort.Text = "3306"
    End Sub
End Class
