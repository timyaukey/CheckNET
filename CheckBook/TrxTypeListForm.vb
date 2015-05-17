Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class TrxTypeListForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mdomTypeTable As VB6XmlDocument
    'This is the <Table> element that will be modified.
    Private melmTypeTable As VB6XmlElement
    'Results of searching for all <TrxType> in melmTypeTable. Must be recreated
    'when <TrxType> nodes are added or deleted in melmTypeTable.
    Private mcolTrxTypes As VB6XmlNodeList
    'If a TrxType is displayed in the controls, this is the <TrxType> element it came from.
    Private melmTrxTypeToSave As VB6XmlElement
    'If a TrxType is displayed in the controls, this is it ListItem.
    Private mobjDisplayedTrxType As System.Windows.Forms.ListViewItem
    'True iff Form_Activate event has fired.
    Private mblnActivated As Boolean

    Public Sub ShowMe()
        Dim strTableFile As String
        Dim frm As System.Windows.Forms.Form

        On Error GoTo ErrorHandler

        For Each frm In gcolForms()
            'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If TypeOf frm Is BankImportForm Then
                MsgBox("You may not edit transaction types while importing from " & "the bank.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next frm
        'UPGRADE_WARNING: Couldn't resolve default property of object gstrTrxTypeFilePath(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        strTableFile = gstrTrxTypeFilePath()
        mdomTypeTable = gdomLoadFile(strTableFile)
        melmTypeTable = mdomTypeTable.DocumentElement
        Me.ShowDialog()

        Exit Sub
ErrorHandler:
        Me.Close()
        NestedError("ShowMe")
    End Sub

    'UPGRADE_WARNING: Form event TrxTypeListForm.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub TrxTypeListForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated

        On Error GoTo ErrorHandler

        If Not mblnActivated Then
            mblnActivated = True
            ShowTrxTypeList()
        End If

        Exit Sub
ErrorHandler:
        TopError("Form_Activate")
    End Sub

    Private Sub cmdSaveChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSaveChanges.Click
        On Error GoTo ErrorHandler

        If blnValidateAndCopyTrxTypeToXML() Then
            Exit Sub
        End If
        mdomTypeTable.Save(gstrTrxTypeFilePath())
        MsgBox("Changes saved.", MsgBoxStyle.Information)
        Me.Close()

        Exit Sub
ErrorHandler:
        TopError("cmdSaveChanges_Click")
    End Sub

    Private Sub cmdDiscardChanges_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDiscardChanges.Click
        Me.Close()
    End Sub

    Private Sub cmdMoveUp_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveUp.Click
        Dim intNewIndex As Short

        On Error GoTo ErrorHandler

        If blnValidateAndCopyTrxTypeToXML() Then
            Exit Sub
        End If
        If mobjDisplayedTrxType.Index = gintLISTITEM_LOWINDEX Then
            Exit Sub
        End If
        intNewIndex = mobjDisplayedTrxType.Index - 1
        'UPGRADE_WARNING: Couldn't resolve default property of object melmTrxTypeToSave. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmTypeTable.RemoveChild(melmTrxTypeToSave)
        'UPGRADE_WARNING: Couldn't resolve default property of object melmTrxTypeToSave. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmTypeTable.InsertBefore(melmTrxTypeToSave, mcolTrxTypes.Item(intNewIndex - gintLISTITEM_LOWINDEX))
        ShowTrxTypeList()
        'UPGRADE_WARNING: Lower bound of collection lvwTrxTypes.ListItems has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        SyncDisplay(lvwTrxTypes.Items.Item(intNewIndex))
        ShowSelectedTrxType()

        Exit Sub
ErrorHandler:
        TopError("cmdMoveUp_Click")
    End Sub

    Private Sub cmdMoveDown_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMoveDown.Click
        Dim intNewIndex As Short

        On Error GoTo ErrorHandler

        If blnValidateAndCopyTrxTypeToXML() Then
            Exit Sub
        End If
        If (mobjDisplayedTrxType.Index - gintLISTITEM_LOWINDEX + 1) = lvwTrxTypes.Items.Count Then
            Exit Sub
        End If
        intNewIndex = mobjDisplayedTrxType.Index + 1
        'UPGRADE_WARNING: Couldn't resolve default property of object melmTrxTypeToSave. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmTypeTable.RemoveChild(melmTrxTypeToSave)
        'UPGRADE_WARNING: Couldn't resolve default property of object melmTrxTypeToSave. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmTypeTable.InsertBefore(melmTrxTypeToSave, mcolTrxTypes.Item(intNewIndex - gintLISTITEM_LOWINDEX + 1))
        ShowTrxTypeList()
        'UPGRADE_WARNING: Lower bound of collection lvwTrxTypes.ListItems has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        SyncDisplay(lvwTrxTypes.Items.Item(intNewIndex))
        ShowSelectedTrxType()

        Exit Sub
ErrorHandler:
        TopError("cmdMoveDown_Click")
    End Sub

    Private Sub cmdNewTrxType_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewTrxType.Click
        Dim elmTrxType As VB6XmlElement
        Dim objNewItem As System.Windows.Forms.ListViewItem

        On Error GoTo ErrorHandler

        If blnValidateAndCopyTrxTypeToXML() Then
            Exit Sub
        End If
        elmTrxType = mdomTypeTable.CreateElement("TrxType")
        elmTrxType.SetAttribute("Before", "(edit or remove this prefix)")
        elmTrxType.SetAttribute("After", "(edit or remove this ending)")
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxType. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmTypeTable.AppendChild(elmTrxType)
        mcolTrxTypes = melmTypeTable.SelectNodes("TrxType")
        objNewItem = objCreateTrxTypeListItem(elmTrxType, mcolTrxTypes.Length - 1)
        System.Windows.Forms.Application.DoEvents()
        SyncDisplay(objNewItem)
        ShowSelectedTrxType()

        Exit Sub
ErrorHandler:
        TopError("cmdNewTrxType_Click")
    End Sub

    Private Sub cmdDeleteTrxType_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteTrxType.Click
        On Error GoTo ErrorHandler

        If melmTrxTypeToSave Is Nothing Then
            MsgBox("You must select a transaction type to delete.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object melmTrxTypeToSave. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmTypeTable.RemoveChild(melmTrxTypeToSave)
        ShowTrxTypeList()

        Exit Sub
ErrorHandler:
        TopError("cmdDeleteTrxType_Click")
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

        On Error GoTo ErrorHandler

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
ErrorHandler:
        sblnInItemClick = False
        TopError("lvwTrxTypes_ItemClick")
    End Sub

    Private Sub ShowTrxTypeList()
        Dim elmTrxType As VB6XmlElement
        Dim intIndex As Short
        Dim objFirst As System.Windows.Forms.ListViewItem

        On Error GoTo ErrorHandler

        lvwTrxTypes.Items.Clear()
        mcolTrxTypes = melmTypeTable.SelectNodes("TrxType")
        For intIndex = 1 To mcolTrxTypes.Length
            elmTrxType = mcolTrxTypes.item(intIndex - 1)
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
ErrorHandler:
        NestedError("ShowTrxTypeList")
    End Sub

    Private Sub SyncDisplay(ByVal objItem As System.Windows.Forms.ListViewItem)
        On Error GoTo ErrorHandler

        lvwTrxTypes.Refresh()
        System.Windows.Forms.Application.DoEvents()
        'UPGRADE_WARNING: MSComctlLib.ListItem method objItem.EnsureVisible has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        objItem.EnsureVisible()
        System.Windows.Forms.Application.DoEvents()
        lvwTrxTypes.FocusedItem = objItem

        Exit Sub
ErrorHandler:
        NestedError("SyncDisplay")
    End Sub

    Private Sub ShowSelectedTrxType()
        On Error GoTo ErrorHandler

        mobjDisplayedTrxType = lvwTrxTypes.FocusedItem
        melmTrxTypeToSave = mcolTrxTypes.item(CShort(mobjDisplayedTrxType.Tag))
        txtNumber.Text = strTrxTypeAttrib(melmTrxTypeToSave, "Number")
        txtBefore.Text = strTrxTypeAttrib(melmTrxTypeToSave, "Before")
        txtAfter.Text = strTrxTypeAttrib(melmTrxTypeToSave, "After")
        txtMinAfter.Text = strTrxTypeAttrib(melmTrxTypeToSave, "MinAfter")

        Exit Sub
ErrorHandler:
        NestedError("ShowSelectedTrxType")
    End Sub

    Private Function strTrxTypeAttrib(ByVal elm As VB6XmlElement, ByVal strName As String) As String

        Dim vstrValue As Object

        'UPGRADE_WARNING: Couldn't resolve default property of object elm.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vstrValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vstrValue = elm.GetAttribute(strName)
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vstrValue) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vstrValue = ""
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vstrValue. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        strTrxTypeAttrib = vstrValue
    End Function

    Private Function blnValidateAndCopyTrxTypeToXML() As Boolean
        On Error GoTo ErrorHandler

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
ErrorHandler:
        NestedError("blnValidateAndCopyTrxTypeToXML")
    End Function

    Private Function blnDisplayedTrxTypeInvalid() As Boolean
        blnDisplayedTrxTypeInvalid = True
        If txtBefore.Text = "" And txtAfter.Text = "" Then
            MsgBox("Either ""starts with"" or ""ends with"" must be specified.", MsgBoxStyle.Critical)
            Exit Function
        End If
        If txtAfter.Text <> "" Then
            If txtMinAfter.Text <> "" Then
                If Not IsNumeric(txtMinAfter.Text) Then
                    MsgBox("Min. after must be a number.", MsgBoxStyle.Critical)
                    Exit Function
                End If
                If CShort(txtMinAfter.Text) < 2 Then
                    MsgBox("Min. after must be at least 2.", MsgBoxStyle.Critical)
                    Exit Function
                End If
            End If
        End If
        blnDisplayedTrxTypeInvalid = False
    End Function

    Private Sub CopyTrxTypeToXML()
        On Error GoTo ErrorHandler

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
            'UPGRADE_WARNING: Lower bound of collection mobjDisplayedTrxType has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If mobjDisplayedTrxType.SubItems.Count > 1 Then
                mobjDisplayedTrxType.SubItems(1).Text = Trim(txtAfter.Text)
            Else
                mobjDisplayedTrxType.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(txtAfter.Text)))
            End If
            'UPGRADE_WARNING: Lower bound of collection mobjDisplayedTrxType has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If mobjDisplayedTrxType.SubItems.Count > 2 Then
                mobjDisplayedTrxType.SubItems(2).Text = Trim(txtMinAfter.Text)
            Else
                mobjDisplayedTrxType.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(txtMinAfter.Text)))
            End If
            'UPGRADE_WARNING: Lower bound of collection mobjDisplayedTrxType has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If mobjDisplayedTrxType.SubItems.Count > 3 Then
                mobjDisplayedTrxType.SubItems(3).Text = Trim(txtNumber.Text)
            Else
                mobjDisplayedTrxType.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, Trim(txtNumber.Text)))
            End If
        End With

        Exit Sub
ErrorHandler:
        NestedError("CopyTrxTypeToXML")
    End Sub

    Private Function objCreateTrxTypeListItem(ByVal elmTrxType As VB6XmlElement, ByVal intDOMIndex As Short) As System.Windows.Forms.ListViewItem

        Dim objItem As System.Windows.Forms.ListViewItem

        On Error GoTo ErrorHandler

        objItem = lvwTrxTypes.Items.Add(strTrxTypeAttrib(elmTrxType, "Before"))
        With objItem
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > 1 Then
                objItem.SubItems(1).Text = strTrxTypeAttrib(elmTrxType, "After")
            Else
                objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTrxTypeAttrib(elmTrxType, "After")))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > 2 Then
                objItem.SubItems(2).Text = strTrxTypeAttrib(elmTrxType, "MinAfter")
            Else
                objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTrxTypeAttrib(elmTrxType, "MinAfter")))
            End If
            'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            If objItem.SubItems.Count > 3 Then
                objItem.SubItems(3).Text = strTrxTypeAttrib(elmTrxType, "Number")
            Else
                objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strTrxTypeAttrib(elmTrxType, "Number")))
            End If
            .Tag = CStr(CShort(intDOMIndex))
        End With
        objCreateTrxTypeListItem = objItem

        Exit Function
ErrorHandler:
        NestedError("objCreateTrxTypeListItem")
    End Function

    Private Sub NestedError(ByVal strRoutine As String)
        gNestedErrorTrap("TrxTypeListForm." & strRoutine)
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("TrxTypeListForm." & strRoutine)
    End Sub
End Class