Option Strict On
Option Explicit On

Public Class TrxMailingAddressTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ITrxTool.strTitle
        Get
            Return "Get Mailing Address"
        End Get
    End Property

    Public Sub Run(objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run

        Dim colPayees As VB6XmlNodeList
        Dim objPayee As VB6XmlElement
        Dim strMsg As String
        Dim strPayee As String
        Dim strAddress2 As String
        Dim objNormalTrx As NormalTrx

        objNormalTrx = objHostTrxToolUI.objGetTrxCopy()
        If objNormalTrx Is Nothing Then
            Exit Sub
        End If

        colPayees = mobjHostUI.objCompany.colFindPayeeMatches(objNormalTrx.strDescription)
        If colPayees.Length = 0 Then
            strMsg = "No matching memorized transactions."
        Else
            strMsg = "Matching Memorized Transaction(s):"
            For Each objPayee In colPayees
                strPayee = CStr(objPayee.GetAttribute("Output")) & vbCrLf & gstrGetXMLChildText(objPayee, "Address1")
                strAddress2 = gstrGetXMLChildText(objPayee, "Address2")
                If Len(strAddress2) > 0 Then
                    strPayee = strPayee & vbCrLf & strAddress2
                End If
                strPayee = strPayee & vbCrLf & gstrGetXMLChildText(objPayee, "City") & " " &
                    gstrGetXMLChildText(objPayee, "State") & " " & gstrGetXMLChildText(objPayee, "Zip") & vbCrLf &
                    "Account #: " & gstrGetXMLChildText(objPayee, "Account")
                strMsg = strMsg & vbCrLf & vbCrLf & strPayee
            Next objPayee
        End If
        mobjHostUI.InfoMessageBox(strMsg)

    End Sub
End Class
