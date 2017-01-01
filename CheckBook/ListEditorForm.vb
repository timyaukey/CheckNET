Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Friend Class ListEditorForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mlngListType As ListType
    Private mobjList As StringTranslator
    Private mstrFile As String
    'If not Nothing, list is private to this account.
    Private mobjAccount As Account
    Private mblnModified As Boolean

    Public Enum ListType
        glngLIST_TYPE_CATEGORY = 1
        glngLIST_TYPE_BUDGET = 2
        glngLIST_TYPE_REPEAT = 3
    End Enum

    Public Sub ShowMe(ByVal lngListType As ListType, ByVal strFile As String, ByVal objList As StringTranslator, ByVal strCaption As String, ByVal objAccount As Account)

        Dim frm As System.Windows.Forms.Form

        Try

            For Each frm In gcolForms()
                'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                If TypeOf frm Is RegisterForm Then
                    '
                    'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                ElseIf TypeOf frm Is CBMainForm Then
                    '
                Else
                    MsgBox("Lists may not be edited if any windows other than registers " & "are open.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            Next frm
            Me.Text = strCaption
            mlngListType = lngListType
            mobjList = objList
            mstrFile = strFile
            mblnModified = False
            mobjAccount = objAccount
            If (mobjAccount Is Nothing) <> (mlngListType <> ListType.glngLIST_TYPE_REPEAT) Then
                gRaiseError("Invalid account passed as arg")
            End If
            LoadList()
            Me.ShowDialog()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        Try

            gSaveChangedAccounts()
            RebuildTranslator()
            WriteFile()
            gBuildShortTermsCatKeys()
            gFindPlaceholderBudget()
            MsgBox("Updated list has been saved. The new names will not appear in " & "register windows until you close and re-open those windows.", MsgBoxStyle.Information)
            mblnModified = False
            Me.Close()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdDiscard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDiscard.Click
        Me.Close()
    End Sub

    Private Sub ListEditorForm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason
        Try

            If mblnModified Then
                If MsgBox("Do you wish to discard changes made on this window?", MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                    Cancel = 1
                    Exit Sub
                End If
            End If

            Exit Sub
        Catch ex As Exception
            eventArgs.Cancel = Cancel
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNew.Click
        Dim strElement As String
        Dim intKey As Short
        Dim strKey As String
        Dim strError As String = ""

        Try

            strElement = Trim(InputBox("Name to add:"))
            If strElement = "" Then
                Exit Sub
            End If
            intKey = intGetUnusedKey()
            strKey = strMakeKey(intKey)
            If strKey = "" Then
                MsgBox("Too many entries in list.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If blnInsertElement(strElement, intKey, False, strError) Then
                MsgBox(strError, MsgBoxStyle.Critical)
                Exit Sub
            End If
            mblnModified = True

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdChange_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChange.Click
        Dim strOldElement As String
        Dim strNewElement As String
        Dim intIndex As Short
        Dim intKey As Short
        Dim strError As String = ""
        Dim blnFoundChild As Boolean
        Dim strChildMatchPrefix As String
        Dim intMatchLen As Short
        Dim strNewChildElement As String

        Try

            'Get the new name, and validate.
            intIndex = lstElements.SelectedIndex
            If intIndex = -1 Then
                MsgBox("Please select the list entry to change.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            strOldElement = gstrVB6GetItemString(lstElements, intIndex)
            strNewElement = InputBox("Edit list entry name:", , strOldElement)
            If strNewElement = "" Or strNewElement = strOldElement Then
                Exit Sub
            End If

            'Delete the old element, and insert the new one with the same key.
            intKey = gintVB6GetItemData(lstElements, intIndex)
            'Always delete first, so insert isn't confused by finding the old entry.
            DeleteElement(intIndex)
            If blnInsertElement(strNewElement, intKey, False, strError) Then
                'Insert failed, so put back the entry we just deleted.
                lstElements.Items.Insert(intIndex, strOldElement)
                gVB6SetItemData(lstElements, intIndex, intKey)
                MsgBox(strError, MsgBoxStyle.Critical)
                Exit Sub
            End If

            'Change all children of the changed entry.
            strChildMatchPrefix = UCase(strOldElement & ":")
            intMatchLen = Len(strChildMatchPrefix)
            Do
                blnFoundChild = False
                For intIndex = 0 To lstElements.Items.Count - 1
                    If UCase(VB.Left(gstrVB6GetItemString(lstElements, intIndex), intMatchLen)) = strChildMatchPrefix Then
                        intKey = gintVB6GetItemData(lstElements, intIndex)
                        strNewChildElement = strNewElement & ":" & Mid(gstrVB6GetItemString(lstElements, intIndex), intMatchLen + 1)
                        DeleteElement(intIndex)
                        If blnInsertElement(strNewChildElement, intKey, False, strError) Then
                            gRaiseError("Unexpected error renaming children: " & strError)
                        End If
                        blnFoundChild = True
                        Exit For
                    End If
                Next
                If Not blnFoundChild Then
                    Exit Do
                End If
            Loop
            mblnModified = True

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        Dim strElement As String
        Dim intIndex As Short

        Try

            intIndex = lstElements.SelectedIndex
            If intIndex < 0 Then
                MsgBox("Please select the list entry to delete.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            strElement = gstrVB6GetItemString(lstElements, intIndex)
            If InStr(strElement, ":") = 0 Then
                If Not blnFirstLevelChangesAllowed Then
                    MsgBox("You may not delete top level entries in this list.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
            End If
            If blnMultipleLevelsAllowed Then
                If intIndex < (lstElements.Items.Count - 1) Then
                    If UCase(strElement & ":") = UCase(VB.Left(gstrVB6GetItemString(lstElements, intIndex + 1), Len(strElement) + 1)) Then
                        MsgBox("You may not delete a list entry with child entries.", MsgBoxStyle.Critical)
                        Exit Sub
                    End If
                End If
            End If
            If blnElementIsUsed(intIndex) Then
                MsgBox("List entry is used by an account.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            DeleteElement(intIndex)
            mblnModified = True

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    '$Description Load lstElements from mobjList, inserting elements in sorted order.
    '   lstElements.ItemData() is index of that element in mobjList.

    Private Sub LoadList()
        Dim intIndex As Short
        Dim strError As String = ""

        Try

            With lstElements
                .Items.Clear()
                For intIndex = 1 To mobjList.intElements
                    If blnInsertElement(mobjList.strValue1(intIndex), CShort(mobjList.strKey(intIndex)), True, strError) Then
                        MsgBox(strError)
                    End If
                Next
            End With

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Insert an element in the correct place in the list based
    '   on sort order. Check for specified parents existing, whether
    '   a list element may be inserted at the level specified, and whether
    '   an element with that path already exists.
    '$Param strNewElement The element to insert, which is either a word like
    '   "Groceries" for a non-heirarchical list, or a path like "E:Groceries"
    '   for a heirarchical list. Used as the text of the list element.
    '$Param intKey The key value for the element. Stored in the list ItemData.
    '$Returns True iff element could not be inserted.

    Private Function blnInsertElement(ByVal strNewElement As String, ByVal intKey As Short, ByVal blnInLoad As Boolean, ByRef strError As String) As Boolean

        Dim intIndex As Short
        Dim strExistingElementUCS As String
        Dim strNewElementUCS As String
        Dim strNewParentUCS As String
        Dim intLastColon As Short
        Dim strPrecedingUCS As String

        Try

            strError = ""
            blnInsertElement = True
            strNewElementUCS = strNormalizeElement(strNewElement)
            If strNewElementUCS = "" Then
                strError = "List entry name is required."
                Exit Function
            End If
            If InStr(strNewElement, "/") > 0 Then
                strError = "List entry names may not contain ""/""."
                Exit Function
            End If
            If VB.Right(strNewElement, 1) = ":" Then
                strError = "List entry names may not end in "":""."
                Exit Function
            End If
            intLastColon = InStrRev(strNewElement, ":")
            If intLastColon > 0 Then
                If Not blnMultipleLevelsAllowed Then
                    strError = "Multiple levels are not allowed in this list."
                    Exit Function
                End If
                strNewParentUCS = strNormalizeElement(VB.Left(strNewElement, intLastColon - 1))
            Else
                If Not (blnFirstLevelChangesAllowed Or blnInLoad) Then
                    strError = "No changes are allowed to first level entries in this list."
                    Exit Function
                End If
                strNewParentUCS = ""
            End If
            With lstElements
                'Iterate one more than the number of elements.
                For intIndex = 0 To .Items.Count
                    If intIndex < .Items.Count Then
                        strExistingElementUCS = strNormalizeElement(gstrVB6GetItemString(lstElements, intIndex))
                        If strExistingElementUCS = strNewElementUCS Then
                            strError = """" & strNewElement & """ is already in this list."
                            Exit Function
                        End If
                    Else
                        strExistingElementUCS = ""
                    End If
                    If (strExistingElementUCS > strNewElementUCS) Or (intIndex >= .Items.Count) Then
                        If intIndex = 0 Then
                            strPrecedingUCS = ""
                        Else
                            strPrecedingUCS = strNormalizeElement(gstrVB6GetItemString(lstElements, intIndex - 1))
                        End If
                        If strNewParentUCS <> "" Then
                            If Not ((strNewParentUCS = strPrecedingUCS) Or ((strNewParentUCS & ":") = VB.Left(strPrecedingUCS, Len(strNewParentUCS) + 1))) Then
                                strError = "Parent of """ & strNewElement & """ not found in list."
                                Exit Function
                            End If
                        End If
                        .Items.Insert(intIndex, gobjCreateListBoxItem(strNewElement, intKey))
                        'gVB6SetItemData(lstElements, intIndex, intKey)
                        Exit For
                    End If
                Next
            End With
            blnInsertElement = False

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Function strNormalizeElement(ByVal strElement As String) As String
        Dim strResult As String
        strResult = UCase(Trim(strElement))
        'Make leading "I" sort before "E" for categories.
        If mlngListType = ListType.glngLIST_TYPE_CATEGORY Then
            If VB.Left(strResult, 1) = "I" Then
                Mid(strResult, 1, 1) = Chr(1)
            ElseIf VB.Left(strResult, 1) = "E" Then
                Mid(strResult, 1, 1) = Chr(2)
            End If
        End If
        strNormalizeElement = strResult
    End Function

    '$Description Remove an element from the list. Does not do any validation
    '   checking - the caller is responsible for things like key in use.

    Private Sub DeleteElement(ByVal intListIndex As Short)
        lstElements.Items.RemoveAt(intListIndex)
    End Sub

    '$Description Rebuild mobjList from lstElements. lstElements is assumed to be in
    '   sorted order.

    Private Sub RebuildTranslator()
        Dim intIndex As Short
        Dim strName1 As String
        Dim strName2 As String
        Dim strKey As String
        Dim intColonPos As Short
        Dim intColonPos2 As Short
        Dim intIndent As Short

        Try

            mobjList.Init()
            For intIndex = 0 To lstElements.Items.Count - 1
                strName1 = gstrVB6GetItemString(lstElements, intIndex)
                strKey = strMakeKey(gintVB6GetItemData(lstElements, intIndex))
                If blnMultipleLevelsAllowed Then
                    intColonPos = 0
                    intIndent = 0
                    Do
                        intColonPos2 = InStr(intColonPos + 1, strName1, ":")
                        If intColonPos2 = 0 Then
                            strName2 = Space(intIndent) & Mid(strName1, intColonPos + 1)
                            Exit Do
                        End If
                        intColonPos = intColonPos2
                        intIndent = intIndent + 1
                    Loop
                Else
                    strName2 = strName1
                End If
                mobjList.Add(strKey, strName1, strName2)
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Write new file from mobjList. mobjList is assumed to be in sorted order.

    Private Sub WriteFile()
        Dim intFile As Short
        Dim intIndex As Short
        Dim strTmpFile As String

        Try

            strTmpFile = gstrAddPath("NewList.tmp")
            intFile = FreeFile()
            FileOpen(intFile, strTmpFile, OpenMode.Output)
            PrintLine(intFile, "Generated " & Now)
            With mobjList
                For intIndex = 1 To .intElements
                    PrintLine(intFile, "/" & .strKey(intIndex) & "/" & .strValue1(intIndex) & "/" & .strValue2(intIndex))
                Next
            End With
            FileClose(intFile)
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Dir(mstrFile) <> "" Then
                Kill(mstrFile)
            End If
            Rename(strTmpFile, mstrFile)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Determine if lstElements entry intListIndex is used in any account.
    '   Checks for usage indicated by mlngListType.

    Private Function blnElementIsUsed(ByVal intListIndex As Short) As Boolean
        Dim strKey As String
        Dim objAccount As Account

        Try

            strKey = strMakeKey(gintVB6GetItemData(lstElements, intListIndex))
            If mobjAccount Is Nothing Then
                For Each objAccount In gcolAccounts
                    If blnElementIsUsedInAccount(objAccount, strKey) Then
                        blnElementIsUsed = True
                        Exit Function
                    End If
                Next objAccount
                blnElementIsUsed = False
            Else
                blnElementIsUsed = blnElementIsUsedInAccount(mobjAccount, strKey)
            End If

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Function blnElementIsUsedInAccount(ByVal objAccount As Account, ByVal strKey As String) As Boolean

        Dim objReg As Register

        Try

            For Each objReg In objAccount.colRegisters
                If blnElementIsUsedInRegister(objReg, strKey) Then
                    blnElementIsUsedInAccount = True
                    Exit Function
                End If
            Next objReg

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Function blnElementIsUsedInRegister(ByVal objReg As Register, ByVal strKey As String) As Boolean

        Dim lngIndex As Integer
        Dim objTrx As Trx
        Dim objSplit As TrxSplit

        Try

            For lngIndex = 1 To objReg.lngTrxCount
                objTrx = objReg.objTrx(lngIndex)
                If mlngListType = ListType.glngLIST_TYPE_REPEAT Then
                    If objTrx.strRepeatKey = strKey Then
                        blnElementIsUsedInRegister = True
                        Exit Function
                    End If
                Else
                    If objTrx.lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                        For Each objSplit In objTrx.colSplits
                            If mlngListType = ListType.glngLIST_TYPE_CATEGORY Then
                                If objSplit.strCategoryKey = strKey Then
                                    blnElementIsUsedInRegister = True
                                    Exit Function
                                End If
                            ElseIf mlngListType = ListType.glngLIST_TYPE_BUDGET Then
                                If objSplit.strBudgetKey = strKey Then
                                    blnElementIsUsedInRegister = True
                                    Exit Function
                                End If
                            Else
                                gRaiseError("Unsupported list type")
                            End If
                        Next
                    ElseIf mlngListType = ListType.glngLIST_TYPE_BUDGET Then
                        If objTrx.lngType = Trx.TrxType.glngTRXTYP_BUDGET Then
                            If objTrx.strBudgetKey = strKey Then
                                blnElementIsUsedInRegister = True
                                Exit Function
                            End If
                        End If
                    End If
                End If
            Next

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Return the lowest positive integer which is not used as a key value
    '   by the list. Will fill in holes in the key sequence, even if those holes are
    '   created by deleting elements. Does not check if the value is used by a transaction,
    '   because all used key values must be present in the list and an element cannot
    '   be deleted if the key is in use.

    Private Function intGetUnusedKey() As Short
        Dim intKey As Short
        Dim intIndex As Short
        Dim intMax As Short
        Dim blnFound As Boolean

        intMax = lstElements.Items.Count - 1
        intKey = 1
        Do
            blnFound = False
            For intIndex = 0 To intMax
                If gintVB6GetItemData(lstElements, intIndex) = intKey Then
                    blnFound = True
                    Exit For
                End If
            Next
            If Not blnFound Then
                intGetUnusedKey = intKey
                Exit Function
            End If
            intKey = intKey + 1
        Loop
    End Function

    '$Description Construct a key string from an integer, if possible.
    '$Param intKey The integer to construct the key from.
    '$Returns The key string, or an empty string if the integer could not be
    '   converted to a key string. The only plausible reason for failure is if
    '   there are too many keys, so the integer is too large.

    Private Function strMakeKey(ByVal intKey As Short) As String
        If intKey < 1 Or intKey > 999 Then
            strMakeKey = ""
        Else
            If mlngListType = ListType.glngLIST_TYPE_CATEGORY Or intKey > 99 Then
                strMakeKey = gstrFormatInteger(intKey, "000")
            Else
                strMakeKey = gstrFormatInteger(intKey, "00")
            End If
        End If
    End Function

    '$Description Determine if first level changes are allowed to list.

    Private ReadOnly Property blnFirstLevelChangesAllowed() As Boolean
        Get
            blnFirstLevelChangesAllowed = (mlngListType <> ListType.glngLIST_TYPE_CATEGORY)
        End Get
    End Property

    '$Description Determine if list can be multiple level.

    Private ReadOnly Property blnMultipleLevelsAllowed() As Boolean
        Get
            blnMultipleLevelsAllowed = (mlngListType = ListType.glngLIST_TYPE_CATEGORY)
        End Get
    End Property
End Class