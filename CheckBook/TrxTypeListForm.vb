Option Strict On
Option Explicit On

Friend Class TrxTypeListForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mdomTypeTable As CBXmlDocument
    'This is the <Table> element that will be modified.
    Private melmTypeTable As CBXmlElement
    'Results of searching for all <TrxType> in melmTypeTable. Must be recreated
    'when <TrxType> nodes are added or deleted in melmTypeTable.
    Private mcolTrxTypes As CBXmlNodeList
    'If a TrxType is displayed in the controls, this is the <TrxType> element it came from.
    Private melmTrxTypeToSave As CBXmlElement
    'If a TrxType is displayed in the controls, this is it ListItem.
    Private mobjDisplayedTrxType As System.Windows.Forms.ListViewItem
    'True iff Form_Activate event has fired.
    Private mblnActivated As Boolean

    Public Sub ShowMe(ByVal objHostUI As IHostUI)
        Dim strTableFile As String

        Try
            mobjHostUI = objHostUI
            mobjCompany = mobjHostUI.Company
            strTableFile = mobjCompany.TrxTypeFilePath()
            mdomTypeTable = mobjCompany.LoadXmlFile(strTableFile)
            melmTypeTable = mdomTypeTable.DocumentElement
            Me.ShowDialog()

            Exit Sub
        Catch ex As Exception
            Me.Close()
            NestedException(ex)
        End Try
    End Sub

    Private Sub TrxTypeListForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        Try

            If Not mblnActivated Then
                mblnActivated = True
                ShowTrxTypeList()
            End If

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdSaveChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSaveChanges.Click
        Try

            If blnValidateAndCopyTrxTypeToXML() Then
                Exit Sub
            End If
            mdomTypeTable.Save(mobjCompany.TrxTypeFilePath())
            mobjHostUI.InfoMessageBox("Changes saved.")
            Me.Close()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdDiscardChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDiscardChanges.Click
        Me.Close()
    End Sub

    Private Sub cmdMoveUp_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveUp.Click
        Dim intNewIndex As Integer

        Try

            If blnValidateAndCopyTrxTypeToXML() Then
                Exit Sub
            End If
            If mobjDisplayedTrxType.Index = gintLISTITEM_LOWINDEX Then
                Exit Sub
            End If
            intNewIndex = mobjDisplayedTrxType.Index - 1
            melmTypeTable.RemoveChild(melmTrxTypeToSave)
            melmTypeTable.InsertBefore(melmTrxTypeToSave, mcolTrxTypes.Item(intNewIndex - gintLISTITEM_LOWINDEX))
            ShowTrxTypeList()
            SyncDisplay(lvwTrxTypes.Items.Item(intNewIndex))
            ShowSelectedTrxType()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdMoveDown_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveDown.Click
        Dim intNewIndex As Integer

        Try

            If blnValidateAndCopyTrxTypeToXML() Then
                Exit Sub
            End If
            If (mobjDisplayedTrxType.Index - gintLISTITEM_LOWINDEX + 1) = lvwTrxTypes.Items.Count Then
                Exit Sub
            End If
            intNewIndex = mobjDisplayedTrxType.Index + 1
            melmTypeTable.RemoveChild(melmTrxTypeToSave)
            melmTypeTable.InsertBefore(melmTrxTypeToSave, mcolTrxTypes.Item(intNewIndex - gintLISTITEM_LOWINDEX + 1))
            ShowTrxTypeList()
            SyncDisplay(lvwTrxTypes.Items.Item(intNewIndex))
            ShowSelectedTrxType()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdNewTrxType_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewTrxType.Click
        Dim elmTrxType As CBXmlElement
        Dim objNewItem As System.Windows.Forms.ListViewItem

        Try

            If blnValidateAndCopyTrxTypeToXML() Then
                Exit Sub
            End If
            elmTrxType = mdomTypeTable.CreateElement("TrxType")
            elmTrxType.SetAttribute("Before", "(edit or remove this prefix)")
            elmTrxType.SetAttribute("After", "(edit or remove this ending)")
            melmTypeTable.AppendChild(elmTrxType)
            mcolTrxTypes = melmTypeTable.SelectNodes("TrxType")
            objNewItem = objCreateTrxTypeListItem(elmTrxType, mcolTrxTypes.Length - 1)
            System.Windows.Forms.Application.DoEvents()
            SyncDisplay(objNewItem)
            ShowSelectedTrxType()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdDeleteTrxType_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteTrxType.Click
        Try

            If melmTrxTypeToSave Is Nothing Then
                mobjHostUI.ErrorMessageBox("You must select a transaction type to delete.")
                Exit Sub
            End If

            melmTypeTable.RemoveChild(melmTrxTypeToSave)
            ShowTrxTypeList()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    'lvwTrxTypes_ItemClick is not converted to an event handler in .NET,
    'so we have to add our own ItemSelectionChanged handler which calls it.

    Private Sub lvwTrxTypes_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lvwTrxTypes.ItemSelectionChanged
        If e.IsSelected Then
            lvwTrxTypes_ItemClick(e.Item)
        End If
    End Sub

    'UPGRADE_ISSUE: MSComctlLib.ListView event lvwTrxTypes.ItemClick was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="ABD9AF39-7E24-4AFF-AD8D-3675C1AA3054"'
    Private Sub lvwTrxTypes_ItemClick(ByVal Item As System.Windows.Forms.ListViewItem)
        'It does not appear this event is fired by setting .SelectedItem,
        'but just to be sure...
        Static sblnInItemClick As Boolean

        Try

            If sblnInItemClick Then
                Exit Sub
            End If
            sblnInItemClick = True
            If lvwTrxTypes.FocusedItem Is Nothing Then
                sblnInItemClick = False
                Exit Sub
            End If
            If Not mobjDisplayedTrxType Is Nothing Then
                If blnDisplayedTrxTypeInvalid() Then
                    lvwTrxTypes.FocusedItem = mobjDisplayedTrxType
                    sblnInItemClick = False
                    Exit Sub
                End If
                CopyTrxTypeToXML()
            End If
            ShowSelectedTrxType()
            sblnInItemClick = False

            Exit Sub
        Catch ex As Exception
            sblnInItemClick = False
            TopException(ex)
        End Try
    End Sub

    Private Sub ShowTrxTypeList()
        Dim elmTrxType As CBXmlElement
        Dim intIndex As Integer
        Dim objFirst As System.Windows.Forms.ListViewItem

        Try

            lvwTrxTypes.Items.Clear()
            mcolTrxTypes = melmTypeTable.SelectNodes("TrxType")
            For intIndex = 1 To mcolTrxTypes.Length
                elmTrxType = DirectCast(mcolTrxTypes.Item(intIndex - 1), CBXmlElement)
                objCreateTrxTypeListItem(elmTrxType, intIndex - 1)
            Next
            System.Windows.Forms.Application.DoEvents()
            objFirst = lvwTrxTypes.TopItem
            If Not objFirst Is Nothing Then
                lvwTrxTypes.FocusedItem = objFirst
                ShowSelectedTrxType()
            Else
                'UPGRADE_NOTE: Object melmTrxTypeToSave may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                melmTrxTypeToSave = Nothing
                'UPGRADE_NOTE: Object mobjDisplayedTrxType may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                mobjDisplayedTrxType = Nothing
                txtNumber.Text = ""
                txtBefore.Text = ""
                txtAfter.Text = ""
                txtMinAfter.Text = ""
            End If

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub SyncDisplay(ByVal objItem As System.Windows.Forms.ListViewItem)
        Try

            lvwTrxTypes.Refresh()
            System.Windows.Forms.Application.DoEvents()
            objItem.EnsureVisible()
            System.Windows.Forms.Application.DoEvents()
            lvwTrxTypes.FocusedItem = objItem

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub ShowSelectedTrxType()
        Try

            mobjDisplayedTrxType = lvwTrxTypes.FocusedItem
            melmTrxTypeToSave = DirectCast(mcolTrxTypes.Item(CShort(mobjDisplayedTrxType.Tag)), CBXmlElement)
            txtNumber.Text = strTrxTypeAttrib(melmTrxTypeToSave, "Number")
            txtBefore.Text = strTrxTypeAttrib(melmTrxTypeToSave, "Before")
            txtAfter.Text = strTrxTypeAttrib(melmTrxTypeToSave, "After")
            txtMinAfter.Text = strTrxTypeAttrib(melmTrxTypeToSave, "MinAfter")

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Function strTrxTypeAttrib(ByVal elm As CBXmlElement, ByVal strName As String) As String

        Dim vstrValue As Object

        vstrValue = elm.GetAttribute(strName)
        If XMLMisc.IsAttributeMissing(vstrValue) Then
            vstrValue = ""
        End If
        strTrxTypeAttrib = CStr(vstrValue)
    End Function

    Private Function blnValidateAndCopyTrxTypeToXML() As Boolean
        Try

            blnValidateAndCopyTrxTypeToXML = False
            If Not mobjDisplayedTrxType Is Nothing Then
                If blnDisplayedTrxTypeInvalid() Then
                    lvwTrxTypes.FocusedItem = mobjDisplayedTrxType
                    blnValidateAndCopyTrxTypeToXML = True
                    Exit Function
                End If
                CopyTrxTypeToXML()
            End If

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Function blnDisplayedTrxTypeInvalid() As Boolean
        blnDisplayedTrxTypeInvalid = True
        If txtBefore.Text = "" And txtAfter.Text = "" Then
            mobjHostUI.ErrorMessageBox("Either ""starts with"" or ""ends with"" must be specified.")
            Exit Function
        End If
        If txtAfter.Text <> "" Then
            If txtMinAfter.Text <> "" Then
                If Not IsNumeric(txtMinAfter.Text) Then
                    mobjHostUI.ErrorMessageBox("Min. after must be a number.")
                    Exit Function
                End If
                If CShort(txtMinAfter.Text) < 2 Then
                    mobjHostUI.ErrorMessageBox("Min. after must be at least 2.")
                    Exit Function
                End If
            End If
        End If
        blnDisplayedTrxTypeInvalid = False
    End Function

    Private Sub CopyTrxTypeToXML()
        Try

            With melmTrxTypeToSave
                .Text = vbCrLf
                If txtBefore.Text <> "" Then
                    .SetAttribute("Before", Trim(txtBefore.Text))
                Else
                    .RemoveAttribute("Before")
                End If
                If txtAfter.Text <> "" Then
                    .SetAttribute("After", Trim(txtAfter.Text))
                Else
                    .RemoveAttribute("After")
                End If
                If txtMinAfter.Text <> "" Then
                    .SetAttribute("MinAfter", Trim(txtMinAfter.Text))
                Else
                    .RemoveAttribute("MinAfter")
                End If
                If txtNumber.Text <> "" Then
                    .SetAttribute("Number", Trim(txtNumber.Text))
                Else
                    .RemoveAttribute("Number")
                End If
            End With

            With mobjDisplayedTrxType
                .Text = Trim(txtBefore.Text)
                If mobjDisplayedTrxType.SubItems.Count > 1 Then
                    mobjDisplayedTrxType.SubItems(1).Text = Trim(txtAfter.Text)
                Else
                    mobjDisplayedTrxType.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(txtAfter.Text)))
                End If
                If mobjDisplayedTrxType.SubItems.Count > 2 Then
                    mobjDisplayedTrxType.SubItems(2).Text = Trim(txtMinAfter.Text)
                Else
                    mobjDisplayedTrxType.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(txtMinAfter.Text)))
                End If
                If mobjDisplayedTrxType.SubItems.Count > 3 Then
                    mobjDisplayedTrxType.SubItems(3).Text = Trim(txtNumber.Text)
                Else
                    mobjDisplayedTrxType.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(txtNumber.Text)))
                End If
            End With

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Function objCreateTrxTypeListItem(ByVal elmTrxType As CBXmlElement, ByVal intDOMIndex As Integer) As System.Windows.Forms.ListViewItem
        Dim objItem As System.Windows.Forms.ListViewItem

        objCreateTrxTypeListItem = Nothing
        Try

            objItem = lvwTrxTypes.Items.Add(strTrxTypeAttrib(elmTrxType, "Before"))
            With objItem
                If objItem.SubItems.Count > 1 Then
                    objItem.SubItems(1).Text = strTrxTypeAttrib(elmTrxType, "After")
                Else
                    objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTrxTypeAttrib(elmTrxType, "After")))
                End If
                If objItem.SubItems.Count > 2 Then
                    objItem.SubItems(2).Text = strTrxTypeAttrib(elmTrxType, "MinAfter")
                Else
                    objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTrxTypeAttrib(elmTrxType, "MinAfter")))
                End If
                If objItem.SubItems.Count > 3 Then
                    objItem.SubItems(3).Text = strTrxTypeAttrib(elmTrxType, "Number")
                Else
                    objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTrxTypeAttrib(elmTrxType, "Number")))
                End If
                .Tag = CStr(CShort(intDOMIndex))
            End With
            objCreateTrxTypeListItem = objItem

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function
End Class