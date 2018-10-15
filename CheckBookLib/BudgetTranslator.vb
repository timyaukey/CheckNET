Option Strict On
Option Explicit On

Public Class BudgetTranslator
    Inherits SimpleStringTranslator

    Public Function strTranslateKey(ByVal strKey As String) As String
        Dim strName As String
        strTranslateKey = ""
        If strKey <> "" Then
            strName = Me.strKeyToValue1(strKey)
            If strName = "" Then
                strName = "TmpBud#" & strKey
                Me.Add(New StringTransElement(Me, strKey, strName, strName))
            End If
            strTranslateKey = strName
        End If
    End Function

End Class
