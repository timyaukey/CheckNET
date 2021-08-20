Option Strict Off
Option Explicit On

Imports System.ComponentModel

Friend Class PayeeListForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    'A deep clone of gdomTransTable.
    Private mdomNewTransTable As CBXmlDocument
    'This is the <Table> element that will be modified, mdomNewTransTable.documentElement.
    Private melmTransTable As CBXmlElement
    'Results of searching for all <Payee> in melmTransTable. Must be recreated
    'when <Payee> nodes are added or deleted in melmTransTable.
    Private mcolPayees As CBXmlNodeList
    'If a payee is displayed in the controls, this is the <Payee> element it came from.
    Private melmPayeeToSave As CBXmlElement
    'If a payee is displayed in the controls, this is it ListItem.
    Private mobjDisplayedPayee As System.Windows.Forms.ListViewItem
    'True iff Form_Activate event has fired.
    Private mblnActivated As Boolean
    'Ignore this number of selected item changed event.
    Private mintIgnoreSelectionEvents As Integer

    Public Sub ShowMe(ByVal objHostUI As IHostUI)
        Try

            mobjHostUI = objHostUI
            mobjCompany = mobjHostUI.objCompany
            LoadSharedDocument()
            UITools.LoadComboFromStringTranslator(cboCategory, mobjCompany.Categories, True)
            UITools.LoadComboFromStringTranslator(cboBudget, mobjCompany.Budgets, True)
            gLoadMatchNarrowingMethods(cboNarrowMethod)
            Me.ShowDialog()

            Exit Sub
        Catch ex As Exception
            Me.Close()
            NestedException(ex)
        End Try
    End Sub

    Private Sub LoadSharedDocument()
        mdomNewTransTable = mobjCompany.MemorizedTransXml.CloneNode(True)
        melmTransTable = mdomNewTransTable.DocumentElement
    End Sub

    Private Sub PayeeListForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        Try

            If Not mblnActivated Then
                mblnActivated = True
                ShowPayeeList()
            End If

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdSaveChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSaveChanges.Click
        Try

            If blnValidateAndCopyPayeeToXML() Then
                Exit Sub
            End If
            mobjCompany.MemorizedTransXml = mdomNewTransTable
            LoadSharedDocument()
            mobjCompany.MemorizedTransXml.Save(mobjCompany.MemorizedTransFilePath())
            mobjCompany.CreateMemorizedTransXmlUCS()
            mobjCompany.LoadMemorizedTransNew()
            Me.Close()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdDiscardChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDiscardChanges.Click
        Me.Close()
    End Sub

    Private Sub cmdNewPayee_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewPayee.Click
        Dim elmPayee As CBXmlElement
        Dim objNewItem As System.Windows.Forms.ListViewItem

        Try

            If blnValidateAndCopyPayeeToXML() Then
                Exit Sub
            End If
            elmPayee = mdomNewTransTable.CreateElement("Payee")
            elmPayee.SetAttribute("Output", "(new transaction - edit this name)")
            melmTransTable.AppendChild(elmPayee)
            mcolPayees = melmTransTable.SelectNodes("Payee")
            objNewItem = gobjCreatePayeeListItem(elmPayee, lvwPayees, mcolPayees.Length - 1)
            gSortPayeeListByName(lvwPayees)
            System.Windows.Forms.Application.DoEvents()
            SyncDisplay(objNewItem)
            ShowSelectedPayee()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdDeletePayee_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeletePayee.Click
        Try

            If melmPayeeToSave Is Nothing Then
                mobjHostUI.ErrorMessageBox("You must select a memorized transaction to delete.")
                Exit Sub
            End If

            melmTransTable.RemoveChild(melmPayeeToSave)
            ShowPayeeList()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub lvwPayees_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwPayees.ItemSelectionChanged
        If mintIgnoreSelectionEvents <= 0 Then
            If e.IsSelected Then
                SelectedItemChanged(e.Item)
            End If
        End If
        mintIgnoreSelectionEvents = mintIgnoreSelectionEvents - 1
    End Sub

    Private Sub SelectedItemChanged(ByVal Item As System.Windows.Forms.ListViewItem)
        Try

            If lvwPayees.FocusedItem Is Nothing Then
                Exit Sub
            End If
            If Not mobjDisplayedPayee Is Nothing Then
                If blnDisplayedPayeeInvalid() Then
                    'TO DO: Fix display bug. It shows both old and new selected items,
                    'for reasons I do not understand. The software works correctly, it
                    'just shows the extra selected item.
                    mintIgnoreSelectionEvents = 3 + lvwPayees.SelectedItems.Count
                    lvwPayees.SelectedItems.Clear()
                    mobjDisplayedPayee.Selected = True
                    Exit Sub
                End If
                CopyPayeeToXML()
            End If
            ShowSelectedPayee()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub ShowPayeeList()
        Dim elmPayee As CBXmlElement
        Dim intIndex As Short
        Dim objFirst As System.Windows.Forms.ListViewItem

        Try

            gInitPayeeList(lvwPayees)
            lvwPayees.Items.Clear()
            mcolPayees = melmTransTable.SelectNodes("Payee")
            For intIndex = 1 To mcolPayees.Length
                elmPayee = mcolPayees.Item(intIndex - 1)
                gobjCreatePayeeListItem(elmPayee, lvwPayees, intIndex - 1)
            Next
            gSortPayeeListByName(lvwPayees)
            System.Windows.Forms.Application.DoEvents()
            objFirst = lvwPayees.TopItem
            If Not objFirst Is Nothing Then
                lvwPayees.FocusedItem = objFirst
                ShowSelectedPayee()
            Else
                'UPGRADE_NOTE: Object melmPayeeToSave may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                melmPayeeToSave = Nothing
                'UPGRADE_NOTE: Object mobjDisplayedPayee may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                mobjDisplayedPayee = Nothing
                txtPayee.Text = ""
                txtAddress1.Text = ""
                txtAddress2.Text = ""
                txtCity.Text = ""
                txtState.Text = ""
                txtZip.Text = ""
                txtAccount.Text = ""
                txtBank.Text = ""
                txtMinAmount.Text = ""
                txtMaxAmount.Text = ""
                txtNumber.Text = ""
                txtAmount.Text = ""
                txtMemo.Text = ""
                cboCategory.SelectedIndex = -1
                cboBudget.SelectedIndex = -1
                cboNarrowMethod.SelectedIndex = -1
                chkAllowAutoBatchNew.Checked = False
                chkAllowAutoBatchUpdate.Checked = False
            End If

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub SyncDisplay(ByVal objItem As System.Windows.Forms.ListViewItem)
        Try

            lvwPayees.Refresh()
            System.Windows.Forms.Application.DoEvents()
            objItem.EnsureVisible()
            System.Windows.Forms.Application.DoEvents()
            lvwPayees.FocusedItem = objItem

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub ShowSelectedPayee()
        Try

            mobjDisplayedPayee = lvwPayees.FocusedItem
            melmPayeeToSave = mcolPayees.Item(CShort(mobjDisplayedPayee.Tag))
            txtPayee.Text = strPayeeAttrib("Output")
            txtAddress1.Text = strPayeeChild("Address1")
            txtAddress2.Text = strPayeeChild("Address2")
            txtCity.Text = strPayeeChild("City")
            txtState.Text = strPayeeChild("State")
            txtZip.Text = strPayeeChild("Zip")
            txtAccount.Text = strPayeeChild("Account")
            txtBank.Text = strPayeeAttrib("Input")
            txtMinAmount.Text = strPayeeAttrib("Min")
            txtMaxAmount.Text = strPayeeAttrib("Max")
            txtNumber.Text = strPayeeChild("Num")
            txtAmount.Text = strPayeeChild("Amount")
            txtMemo.Text = strPayeeChild("Memo")
            SetComboBox(cboCategory, strPayeeChild("Cat"))
            SetComboBox(cboBudget, strPayeeChild("Budget"))
            SetComboBox(cboNarrowMethod, strPayeeChild("NarrowMethod"))
            chkAllowAutoBatchNew.Checked = (strPayeeChild("AllowAutoBatchNew").ToLower() = PayeeItem.Yes)
            chkAllowAutoBatchUpdate.Checked = (strPayeeChild("AllowAutoBatchUpdate").ToLower() = PayeeItem.Yes)

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Function strPayeeAttrib(ByVal strName As String) As String
        Dim vstrValue As Object

        vstrValue = melmPayeeToSave.GetAttribute(strName)
        If XMLMisc.IsAttributeMissing(vstrValue) Then
            vstrValue = ""
        End If
        strPayeeAttrib = Trim(vstrValue)
    End Function

    Private Function strPayeeChild(ByVal strName As String) As String
        Dim elmChild As CBXmlElement

        elmChild = melmPayeeToSave.SelectSingleNode(strName)
        If elmChild Is Nothing Then
            strPayeeChild = ""
        Else
            strPayeeChild = Trim(elmChild.Text)
        End If
    End Function

    Private Sub SetComboBox(ByVal cbo As System.Windows.Forms.ComboBox, ByVal strValue As String)
        Dim intIndex As Short

        For intIndex = 0 To cbo.Items.Count - 1
            If strValue = UITools.GetItemString(cbo, intIndex) Then
                cbo.SelectedIndex = intIndex
                Exit Sub
            End If
        Next
        cbo.SelectedIndex = -1
    End Sub

    Private Function blnValidateAndCopyPayeeToXML() As Boolean
        Try

            blnValidateAndCopyPayeeToXML = False
            If Not mobjDisplayedPayee Is Nothing Then
                If blnDisplayedPayeeInvalid() Then
                    lvwPayees.FocusedItem = mobjDisplayedPayee
                    blnValidateAndCopyPayeeToXML = True
                    Exit Function
                End If
                CopyPayeeToXML()
            End If

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Function blnDisplayedPayeeInvalid() As Boolean
        blnDisplayedPayeeInvalid = True
        If Trim(txtPayee.Text) = "" Then
            mobjHostUI.ErrorMessageBox("Name (or other transaction description) is required.")
            Exit Function
        End If
        If txtAmount.Text <> "" Then
            If Not IsNumeric(txtAmount.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid amount.")
                Exit Function
            End If
        End If
        If (txtMinAmount.Text <> "") <> (txtMaxAmount.Text <> "") Then
            mobjHostUI.ErrorMessageBox("Min and max match amounts must either both be specified, " & "or both blank.")
            Exit Function
        End If
        If txtMinAmount.Text <> "" Then
            If Not IsNumeric(txtMinAmount.Text) Then
                mobjHostUI.ErrorMessageBox("Min match amount must be numeric.")
                Exit Function
            End If
            If Not IsNumeric(txtMaxAmount.Text) Then
                mobjHostUI.ErrorMessageBox("Max match amount must be numeric.")
                Exit Function
            End If
        End If
        blnDisplayedPayeeInvalid = False
    End Function

    Private Sub CopyPayeeToXML()
        Try

            gDisablePayeeListSorting(lvwPayees)

            melmPayeeToSave.Text = vbCrLf


            If mobjDisplayedPayee.SubItems.Count > 1 Then
                mobjDisplayedPayee.SubItems(1).Text = txtPayee.Text
            Else
                mobjDisplayedPayee.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, txtPayee.Text))
            End If
            melmPayeeToSave.SetAttribute("Output", txtPayee.Text)

            SaveChildElement(txtAddress1.Text, "Address1")
            SaveChildElement(txtAddress2.Text, "Address2")
            SaveChildElement(txtCity.Text, "City")
            SaveChildElement(txtState.Text, "State")
            SaveChildElement(txtZip.Text, "Zip")
            SaveChildElement(txtAccount.Text, "Account")

            melmPayeeToSave.SetAttribute("Input", txtBank.Text)

            If txtMinAmount.Text = "" Then
                melmPayeeToSave.RemoveAttribute("Min")
                melmPayeeToSave.RemoveAttribute("Max")
            Else
                melmPayeeToSave.SetAttribute("Min", txtMinAmount.Text)
                melmPayeeToSave.SetAttribute("Max", txtMaxAmount.Text)
            End If

            mobjDisplayedPayee.Text = txtNumber.Text
            SaveChildElement(txtNumber.Text, "Num")


            If mobjDisplayedPayee.SubItems.Count > 3 Then
                mobjDisplayedPayee.SubItems(3).Text = txtAmount.Text
            Else
                mobjDisplayedPayee.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, txtAmount.Text))
            End If
            SaveChildElement(txtAmount.Text, "Amount")


            If mobjDisplayedPayee.SubItems.Count > 5 Then
                mobjDisplayedPayee.SubItems(5).Text = txtMemo.Text
            Else
                mobjDisplayedPayee.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, txtMemo.Text))
            End If
            SaveChildElement(txtMemo.Text, "Memo")

            If cboCategory.SelectedIndex = -1 Then

                If mobjDisplayedPayee.SubItems.Count > 2 Then
                    mobjDisplayedPayee.SubItems(2).Text = ""
                Else
                    mobjDisplayedPayee.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                End If
            Else

                If mobjDisplayedPayee.SubItems.Count > 2 Then
                    mobjDisplayedPayee.SubItems(2).Text = cboCategory.Text
                Else
                    mobjDisplayedPayee.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, cboCategory.Text))
                End If
            End If
            SaveChildElement(cboCategory.Text, "Cat")

            If cboBudget.SelectedIndex = -1 Then

                If mobjDisplayedPayee.SubItems.Count > 4 Then
                    mobjDisplayedPayee.SubItems(4).Text = ""
                Else
                    mobjDisplayedPayee.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
                End If
            Else

                If mobjDisplayedPayee.SubItems.Count > 4 Then
                    mobjDisplayedPayee.SubItems(4).Text = cboBudget.Text
                Else
                    mobjDisplayedPayee.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, cboBudget.Text))
                End If
            End If
            SaveChildElement(cboBudget.Text, "Budget")
            SaveChildElement(cboNarrowMethod.Text, "NarrowMethod")
            SaveChildElement(If(chkAllowAutoBatchNew.Checked, PayeeItem.Yes, ""), "AllowAutoBatchNew")
            SaveChildElement(If(chkAllowAutoBatchUpdate.Checked, PayeeItem.Yes, ""), "AllowAutoBatchUpdate")

            gSortPayeeListByName(lvwPayees)

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub SaveChildElement(ByVal strValue As String, ByVal strChildName As String)
        Dim elmChild As CBXmlElement
        elmChild = melmPayeeToSave.SelectSingleNode(strChildName)
        If elmChild Is Nothing Then
            elmChild = mdomNewTransTable.CreateElement(strChildName)
            melmPayeeToSave.AppendChild(elmChild)
        End If
        elmChild.Text = Trim(strValue)
    End Sub
End Class