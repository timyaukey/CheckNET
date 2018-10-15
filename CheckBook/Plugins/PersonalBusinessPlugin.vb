Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class PersonalBusinessPlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim objReg As Register = HostUI.objGetCurrentRegister()
            If objReg Is Nothing Then
                HostUI.ErrorMessageBox("Please click on the register window you wish to adjust.")
                Exit Sub
            End If
            Dim frmAdjust As AdjustPersonalBusinessForm = New AdjustPersonalBusinessForm
            frmAdjust.ShowModal(HostUI, objReg.objAccount)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Adjust Account For Personal Use"
    End Function

    Public Overrides Function SortCode() As Integer
        Return 110
    End Function
End Class
