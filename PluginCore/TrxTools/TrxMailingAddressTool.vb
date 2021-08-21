Option Strict On
Option Explicit On

Public Class TrxMailingAddressTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property Title As String Implements ITrxTool.Title
        Get
            Return "Show Mailing Address"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Title
    End Function

    Public Sub Run(objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run

        Dim colPayees As CBXmlNodeList
        Dim objPayee As CBXmlElement
        Dim strMsg As String
        Dim strPayee As String
        Dim strAddress2 As String
        Dim objNormalTrx As BankTrx

        objNormalTrx = objHostTrxToolUI.GetTrxCopy()
        If objNormalTrx Is Nothing Then
            Exit Sub
        End If

        colPayees = mobjHostUI.Company.FindPayeeMatches(objNormalTrx.Description)
        If colPayees.Length = 0 Then
            strMsg = "No matching memorized transactions."
        Else
            strMsg = "Matching Memorized Transaction(s):"
            For Each objPayee In colPayees
                strPayee = CStr(objPayee.GetAttribute("Output")) & vbCrLf & XMLMisc.GetChildText(objPayee, "Address1")
                strAddress2 = XMLMisc.GetChildText(objPayee, "Address2")
                If Len(strAddress2) > 0 Then
                    strPayee = strPayee & vbCrLf & strAddress2
                End If
                strPayee = strPayee & vbCrLf & XMLMisc.GetChildText(objPayee, "City") & " " &
                    XMLMisc.GetChildText(objPayee, "State") & " " & XMLMisc.GetChildText(objPayee, "Zip") & vbCrLf &
                    "Account #: " & XMLMisc.GetChildText(objPayee, "Account")
                strMsg = strMsg & vbCrLf & vbCrLf & strPayee
            Next objPayee
        End If
        mobjHostUI.InfoMessageBox(strMsg)

    End Sub
End Class
