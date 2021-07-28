Option Strict On
Option Explicit On

Public Class BudgetTranslator
    Inherits SimpleStringTranslator

    Public Function TranslateKey(ByVal strKey As String) As String
        Dim strName As String
        TranslateKey = ""
        If strKey <> "" Then
            strName = Me.KeyToValue1(strKey)
            If strName = "" Then
                strName = "TmpBud#" & strKey
                Me.Add(New StringTransElement(Me, strKey, strName, strName))
            End If
            TranslateKey = strName
        End If
    End Function

End Class
