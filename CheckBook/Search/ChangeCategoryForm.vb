Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class ChangeCategoryForm
	Inherits System.Windows.Forms.Form

    Private mobjCompany As Company
    Private mblnSuccess As Boolean
    Private mstrOldCatKey As String
	Private mstrNewCatKey As String

    Public Function blnGetCategories(ByVal objCompany As Company, ByRef strOldCatKey As String, ByRef strNewCatKey As String) As Boolean

        mobjCompany = objCompany
        gLoadComboFromStringTranslator(cboOldCategory, mobjCompany.objCategories, True)
        gLoadComboFromStringTranslator(cboNewCategory, mobjCompany.objCategories, True)
        mblnSuccess = False
        ShowDialog()
        strOldCatKey = mstrOldCatKey
        strNewCatKey = mstrNewCatKey
        blnGetCategories = mblnSuccess

    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
		Dim lngItemData As Integer
		
		If cboOldCategory.SelectedIndex < 0 Then
			MsgBox("Please select the old category.")
			Exit Sub
		End If
		
		If cboNewCategory.SelectedIndex < 0 Then
			MsgBox("Please select the new category.")
			Exit Sub
		End If
		
		If MsgBox("Are you sure you want to change all splits in the selected transactions " & "with the old category to the new category?", MsgBoxStyle.Question + MsgBoxStyle.OKCancel) <> MsgBoxResult.OK Then
			Exit Sub
		End If
		
        lngItemData = gintVB6GetItemData(cboOldCategory, cboOldCategory.SelectedIndex)
        mstrOldCatKey = mobjCompany.objCategories.strKey(lngItemData)

        lngItemData = gintVB6GetItemData(cboNewCategory, cboNewCategory.SelectedIndex)
        mstrNewCatKey = mobjCompany.objCategories.strKey(lngItemData)

        mblnSuccess = True
		Me.Close()
	End Sub
End Class