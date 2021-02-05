Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class MenuElementRegister
    Inherits MenuElementBase

    Private HostUI As IHostUI
    Private ReadOnly RegisterClickHandler As Action(Of Object, RegisterEventArgs)
    Public MenuItemControl As ToolStripMenuItem

    Public Sub New(ByVal hostUI_ As IHostUI, ByVal title_ As String, ByVal sortCode_ As Integer,
                   ByVal regClickHandler_ As Action(Of Object, RegisterEventArgs), ByVal pluginPath_ As String)
        MyBase.New(title_, sortCode_, pluginPath_)
        HostUI = hostUI_
        RegisterClickHandler = regClickHandler_
    End Sub

    Public Overrides Sub CreateUIElement(ByVal mnuParent As ToolStripMenuItem)
        Dim mnuNewItem As ToolStripMenuItem = New ToolStripMenuItem()
        mnuNewItem.Text = Me.Title
        AddHandler mnuNewItem.Click, AddressOf ClickHandler
        mnuParent.DropDownItems.Add(mnuNewItem)
        MenuItemControl = mnuNewItem
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Dim objReg As Register = HostUI.objGetCurrentRegister()
        If objReg Is Nothing Then
            HostUI.ErrorMessageBox("This option is not available, because I don't know which register" &
                " or account to use. The register form for the desired register must be the current window.")
            Exit Sub
        End If
        Dim objRegArgs As RegisterEventArgs = New RegisterEventArgs()
        objRegArgs.objReg = objReg
        RegisterClickHandler(sender, objRegArgs)
    End Sub
End Class

Public Class RegisterEventArgs
    Inherits EventArgs

    Public objReg As Register
End Class
