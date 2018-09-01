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
                MsgBox("Error: Could not find code " & strKey & " in budget " & "list. Have assigned it temporary budget name " & strName & ", which " & "you will probably want to edit to make this budget " & "permanent.", MsgBoxStyle.Information)
            End If
            strTranslateKey = strName
        End If
    End Function

End Class
