Option Strict On
Option Explicit On

Public Class CompanyEventMonitorForm

    Public Sub ShowModal(ByVal objHostUI As IHostUI, ByVal objEvents As List(Of String))
        lstEvents.Items.Clear()
        For Each strEvent As String In objEvents
            lstEvents.Items.Add(strEvent)
        Next
        Me.ShowDialog()
    End Sub
End Class