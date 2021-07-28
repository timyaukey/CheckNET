
Public Class CategoryEditorForm
    Private mblnSaved As Boolean

    Public Function blnShowDialog(ByVal objTransElem As StringTransElement, ByVal blnNew As Boolean) As Boolean
        mblnSaved = False
        txtName.Text = objTransElem.Value1
        Dim strCatTypeCode As String = ""
        If objTransElem.ExtraValues.ContainsKey(CategoryTranslator.TypeKey) Then
            strCatTypeCode = objTransElem.ExtraValues(CategoryTranslator.TypeKey)
        End If
        cboType.Items.Clear()
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeSales)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeReturns)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeCOGS)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeOperatingExpenses)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeOfficeExpense)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypePayroll)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeRentInc)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeRentExp)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeOtherIncome)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeOtherExpense)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeTaxes)
        AddComboItem(cboType, strCatTypeCode, CategoryTranslator.TypeDepreciation)
        Me.ShowDialog()
        If mblnSaved Then
            objTransElem.Value1 = txtName.Text
            objTransElem.ExtraValues.Clear()
            If Not cboType.SelectedItem Is Nothing Then
                objTransElem.ExtraValues.Add(CategoryTranslator.TypeKey, DirectCast(cboType.SelectedItem, ComboItem).strKey)
            End If
        End If
        Return mblnSaved
    End Function

    Private Sub AddComboItem(ByVal cbo As ComboBox, ByVal strCurrentValueKey As String, ByVal strCBKey As String)
        Dim objItem As ComboItem = New ComboItem(strCBKey, CategoryTranslator.TranslateType(strCBKey))
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