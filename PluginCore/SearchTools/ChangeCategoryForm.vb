Option Strict Off
Option Explicit On


Friend Class ChangeCategoryForm
	Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mblnSuccess As Boolean
    Private mstrOldCatKey As String
	Private mstrNewCatKey As String

    Public Function blnGetCategories(ByVal objHostUI As IHostUI, ByRef strOldCatKey As String, ByRef strNewCatKey As String) As Boolean

        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany
        UITools.LoadComboFromStringTranslator(cboOldCategory, mobjCompany.objCategories, True)
        UITools.LoadComboFromStringTranslator(cboNewCategory, mobjCompany.objCategories, True)
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
            mobjHostUI.InfoMessageBox("Please select the old category.")
            Exit Sub
		End If
		
		If cboNewCategory.SelectedIndex < 0 Then
            mobjHostUI.InfoMessageBox("Please select the new category.")
            Exit Sub
		End If

        If mobjHostUI.OkCancelMessageBox("Are you sure you want to change all splits in the selected transactions " & "with the old category to the new category?") <> DialogResult.OK Then
            Exit Sub
        End If

        lngItemData = UITools.GetItemData(cboOldCategory, cboOldCategory.SelectedIndex)
        mstrOldCatKey = mobjCompany.objCategories.strKey(lngItemData)

        lngItemData = UITools.GetItemData(cboNewCategory, cboNewCategory.SelectedIndex)
        mstrNewCatKey = mobjCompany.objCategories.strKey(lngItemData)

        mblnSuccess = True
		Me.Close()
	End Sub
End Class