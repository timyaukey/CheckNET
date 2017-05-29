Imports CheckBookLib

Public Class CategoryEditorForm
    Private mblnSaved As Boolean

    Public Function blnShowDialog(ByVal objTransElem As StringTransElement, ByVal blnNew As Boolean) As Boolean
        mblnSaved = False
        txtName.Text = objTransElem.strValue1
        Dim strCatTypeCode As String = ""
        If objTransElem.colValues.ContainsKey(CategoryTranslator.strTypeKey) Then
            strCatTypeCode = objTransElem.colValues(CategoryTranslator.strTypeKey)
        End If
        cboType.Items.Clear()
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeSales, "Sales")
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeReturns, "Returns")
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeCOGS, "Cost of Goods Sold")
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeOperatingExpenses, "Operating Expenses")
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeOtherIncome, "Other Income")
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeOtherExpense, "Other Expenses")
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.strTypeTaxes, "Taxes")
        Me.ShowDialog()
        If mblnSaved Then
            objTransElem.strValue1 = txtName.Text
            objTransElem.colValues.Clear()
            If Not cboType.SelectedItem Is Nothing Then
                objTransElem.colValues.Add(CategoryTranslator.strTypeKey, DirectCast(cboType.SelectedItem, ComboItem).strKey)
            End If
        End If
        Return mblnSaved
    End Function

    Private Sub AddComboItem(ByVal cbo As ComboBox, ByVal strCurrentValueKey As String, ByVal strCBKey As String, ByVal strCBValue As String)
        Dim objItem As ComboItem = New ComboItem(strCBKey, strCBValue)
        cbo.Items.Add(objItem)
        If objItem.strKey = strCurrentValueKey Then
            cbo.SelectedItem = objItem
        End If
    End Sub

    Private Sub btnOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click
        mblnSaved = True
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        mblnSaved = False
        Me.Close()
    End Sub

    Private Class ComboItem
        Public ReadOnly strKey As String
        Public ReadOnly strValue As String

        Public Sub New(ByVal strKey_ As String, ByVal strValue_ As String)
            strKey = strKey_
            strValue = strValue_
        End Sub

        Public Overrides Function ToString() As String
            Return strValue
        End Function
    End Class
End Class