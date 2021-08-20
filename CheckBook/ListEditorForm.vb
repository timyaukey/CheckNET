Option Strict On
Option Explicit On

Imports VB = Microsoft.VisualBasic

Friend Class ListEditorForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mlngListType As ListType
    Private mobjList As SimpleStringTranslator
    Private mstrFile As String
    Private mobjCompany As Company
    Private mblnModified As Boolean
    Private mblnSaved As Boolean
    Private mblnEditElem As EditStringTransElement

    Public Enum ListType
        Category = 1
        Budget = 2
    End Enum

    Public Delegate Function EditStringTransElement(ByVal objTransElem As StringTransElement, ByVal blnNew As Boolean) As Boolean

    Public Function blnShowMe(ByVal objHostUI As IHostUI, ByVal lngListType As ListType, ByVal strFile As String,
                              ByVal objList As SimpleStringTranslator, ByVal strCaption As String,
                              ByVal blnEditElem As EditStringTransElement) As Boolean

        Dim frm As System.Windows.Forms.Form
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany

        Try
            For Each frm In gcolForms()
                If TypeOf frm Is IRegisterForm Then
                    '
                ElseIf TypeOf frm Is CBMainForm Then
                    '
                Else
                    mobjHostUI.ErrorMessageBox("Lists may not be edited if any windows other than registers " & "are open.")
                    Return False
                End If
            Next frm
            Me.Text = strCaption
            mlngListType = lngListType
            mobjList = objList
            mstrFile = strFile
            mblnModified = False
            mblnSaved = False
            mblnEditElem = blnEditElem
            LoadList()
            Me.ShowDialog()
            Return mblnSaved

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        Try

            CompanySaver.SaveChangedAccounts(mobjHostUI.objCompany)
            RebuildTranslator()
            WriteFile()
            mobjCompany.BuildShortTermsCatKeys()
            CompanyLoader.FindPlaceholderBudget(mobjCompany)
            mobjHostUI.InfoMessageBox("Updated list has been saved. The new names will not appear in " & "register windows until you close and re-open those windows.")
            mblnModified = False
            mblnSaved = True
            Me.Close()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdDiscard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDiscard.Click
        Me.Close()
    End Sub

    Private Sub ListEditorForm_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            If mblnModified Then
                If mobjHostUI.OkCancelMessageBox("Do you wish to discard changes made on this window?") <> DialogResult.OK Then
                    eventArgs.Cancel = True
                    Exit Sub
                End If
            End If

            Exit Sub
        Catch ex As Exception
            eventArgs.Cancel = True
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNew.Click
        'Dim strValue1 As String
        Dim strKey As String
        Dim strError As String = ""
        Dim objTransElem As StringTransElement

        Try

            'strValue1 = Trim(InputBox("Name to add:"))
            'If strValue1 = "" Then
            'Exit Sub
            'End If
            strKey = strMakeKey(intGetUnusedKey())
            If strKey = "" Then
                mobjHostUI.ErrorMessageBox("Too many entries in list.")
                Exit Sub
            End If
            'objTransElem = New StringTransElement(strKey, strValue1, strMakeValue2(strValue1))
            objTransElem = New StringTransElement(mobjList, strKey, "", "")
            If Not mblnEditElem(objTransElem, True) Then
                Exit Sub
            End If
            objTransElem.Value2 = strMakeValue2(objTransElem.Value1)
            If blnInsertElement(objTransElem, False, strError) Then
                mobjHostUI.ErrorMessageBox(strError)
                Exit Sub
            End If
            mblnModified = True

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdChange_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChange.Click
        Dim objOrigTransElem As StringTransElement
        Dim strOrigValue1 As String
        Dim objNewTransElem As StringTransElement
        Dim strNewValue1 As String
        Dim intIndex As Integer
        Dim strError As String = ""
        Dim blnFoundChild As Boolean
        Dim strChildMatchPrefix As String
        Dim intMatchLen As Integer
        Dim objScanTransElem As StringTransElement

        Try

            'Get the new name, and validate.
            intIndex = lstElements.SelectedIndex
            If intIndex = -1 Then
                mobjHostUI.ErrorMessageBox("Please select the list entry to change.")
                Exit Sub
            End If
            objOrigTransElem = DirectCast(lstElements.SelectedItem, StringTransElement)
            strOrigValue1 = objOrigTransElem.Value1
            'strNewValue1 = InputBox("Edit list entry name:", , strOrigValue1)
            'If strNewValue1 = "" Or strNewValue1 = strOrigValue1 Then
            'Exit Sub
            'End If
            objNewTransElem = objOrigTransElem.CloneElement()
            If Not mblnEditElem(objNewTransElem, False) Then
                Exit Sub
            End If
            strNewValue1 = objNewTransElem.Value1
            'objNewTransElem.strValue1 = strNewValue1
            objNewTransElem.Value2 = strMakeValue2(strNewValue1)

            'Always delete first, so insert isn't confused by finding the old entry.
            DeleteElement(intIndex)
            If blnInsertElement(objNewTransElem, False, strError) Then
                'Insert failed, so put back the entry we just deleted.
                lstElements.Items.Insert(intIndex, objOrigTransElem)
                mobjHostUI.ErrorMessageBox(strError)
                Exit Sub
            End If

            'Change all children of the changed entry.
            strChildMatchPrefix = UCase(strOrigValue1 & ":")
            intMatchLen = Len(strChildMatchPrefix)
            Do
                blnFoundChild = False
                For intIndex = 0 To lstElements.Items.Count - 1
                    objScanTransElem = DirectCast(lstElements.Items(intIndex), StringTransElement)
                    If UCase(VB.Left(objScanTransElem.Value1, intMatchLen)) = strChildMatchPrefix Then
                        objScanTransElem.Value1 = strNewValue1 & ":" & Mid(objScanTransElem.Value1, intMatchLen + 1)
                        DeleteElement(intIndex)
                        If blnInsertElement(objScanTransElem, False, strError) Then
                            RaiseErrorMsg("Unexpected error renaming children: " & strError)
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
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        Dim strValue1 As String
        Dim intIndex As Integer

        Try

            intIndex = lstElements.SelectedIndex
            If intIndex < 0 Then
                mobjHostUI.ErrorMessageBox("Please select the list entry to delete.")
                Exit Sub
            End If
            strValue1 = DirectCast(lstElements.Items(intIndex), StringTransElement).Value1
            If InStr(strValue1, ":") = 0 Then
                If Not blnFirstLevelChangesAllowed Then
                    mobjHostUI.ErrorMessageBox("You may not delete top level entries in this list.")
                    Exit Sub
                End If
            End If
            If blnMultipleLevelsAllowed Then
                If intIndex < (lstElements.Items.Count - 1) Then
                    If UCase(strValue1 & ":") = UCase(VB.Left(DirectCast(lstElements.Items(intIndex + 1), StringTransElement).Value1, Len(strValue1) + 1)) Then
                        mobjHostUI.ErrorMessageBox("You may not delete a list entry with child entries.")
                        Exit Sub
                    End If
                End If
            End If
            Dim strAccounts As String = ""
            If blnElementIsUsed(intIndex, strAccounts) Then
                mobjHostUI.ErrorMessageBox("Cannot delete list entry, because it is used by these accounts: " + strAccounts)
                Exit Sub
            End If

            DeleteElement(intIndex)
            mblnModified = True

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    '$Description Load lstElements from mobjList, inserting elements in sorted order.
    '   lstElements.ItemData() is index of that element in mobjList.

    Private Sub LoadList()
        Dim intIndex As Integer
        Dim strError As String = ""
        Dim blnFoundPersonal As Boolean = False
        Dim objTransElem As StringTransElement

        'Only for debugging, but harmless to leave in because it is so fast.
        CompareTest("A", "A", 0)
        CompareTest("One", "Two", -1)
        CompareTest("Two", "One", 1)
        CompareTest("One:A", "One:A", 0)
        CompareTest("One:A", "One:B", -1)
        CompareTest("Two:A", "One:B", 1)
        CompareTest("One:B", "One:A", 1)
        CompareTest("One:A", "One:AA", -1)
        CompareTest("A", "A:B", -1)
        CompareTest("C:Test", "C:Test2", -1)
        CompareTest("C:Test2", "C:Test", 1)
        CompareTest("C:Test:New", "C:Test2", -1)
        CompareTest("C:Test:New", "C:Test2:New", -1)
        CompareTest("C:Test2:New", "C:Test:New", 1)

        Try

            With lstElements
                .Items.Clear()
                For intIndex = 1 To mobjList.ElementCount
                    objTransElem = mobjList.GetElement(intIndex)
                    If blnInsertElement(objTransElem.CloneElement(), True, strError) Then
                        mobjHostUI.InfoMessageBox(strError)
                    End If
                    If mlngListType = ListType.Category Then
                        If CategoryTranslator.IsPersonal(objTransElem.Value1) Then
                            blnFoundPersonal = True
                        End If
                    End If
                Next
                If mlngListType = ListType.Category And Not blnFoundPersonal Then
                    Dim intKey As Integer = intGetUnusedKey()
                    objTransElem = New StringTransElement(mobjList, strMakeKey(intKey), "C", "C")
                    If blnInsertElement(objTransElem, True, strError) Then
                        mobjHostUI.InfoMessageBox(strError)
                    End If
                End If
            End With

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    '$Description Insert an element in the correct place in the list based
    '   on sort order. Check for specified parents existing, whether
    '   a list element may be inserted at the level specified, and whether
    '   an element with that path already exists.
    '$Param strNewElement The element to insert, which is either a word like
    '   "Groceries" for a non-heirarchical list, or a path like "E:Groceries"
    '   for a heirarchical list. Used as the text of the list element.
    '$Param AccountKey The key value for the element. Stored in the list ItemData.
    '$Returns True iff element could not be inserted.

    Private Function blnInsertElement(ByVal objTransElem As StringTransElement, ByVal blnInLoad As Boolean, ByRef strError As String) As Boolean

        Dim intIndex As Integer
        Dim strExistingElementUCS As String
        Dim strNewElement As String
        Dim strNewElementUCS As String
        Dim strNewParentUCS As String
        Dim intLastColon As Integer
        Dim strPrecedingUCS As String

        Try

            strError = ""
            blnInsertElement = True
            strNewElement = objTransElem.Value1
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
            For Each objExtraPair As KeyValuePair(Of String, String) In objTransElem.ExtraValues
                If objExtraPair.Key.Contains("/") Or objExtraPair.Key.Contains(":") Or
                        objExtraPair.Value.Contains("/") Or objExtraPair.Value.Contains(":") Then
                    strError = "List entry values may not contain ""//"" or "":""."
                    Exit Function
                End If
            Next
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
                        strExistingElementUCS = strNormalizeElement(DirectCast(lstElements.Items(intIndex), StringTransElement).Value1)
                        If strExistingElementUCS = strNewElementUCS Then
                            strError = """" & strNewElement & """ is already in this list."
                            Exit Function
                        End If
                    Else
                        strExistingElementUCS = ""
                    End If
                    If (intCompareCatNames(strExistingElementUCS, strNewElementUCS) = 1) Or (intIndex >= .Items.Count) Then
                        If intIndex = 0 Then
                            strPrecedingUCS = ""
                        Else
                            strPrecedingUCS = strNormalizeElement(DirectCast(lstElements.Items(intIndex - 1), StringTransElement).Value1)
                        End If
                        If strNewParentUCS <> "" Then
                            If Not ((strNewParentUCS = strPrecedingUCS) Or ((strNewParentUCS & ":") = VB.Left(strPrecedingUCS, Len(strNewParentUCS) + 1))) Then
                                strError = "Parent of """ & strNewElement & """ not found in list."
                                Exit Function
                            End If
                        End If
                        .Items.Insert(intIndex, objTransElem)
                        Exit For
                    End If
                Next
            End With
            blnInsertElement = False

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Function intCompareCatNames(ByVal strName1 As String, ByVal strName2 As String) As Integer
        Dim strParts1() As String = strName1.Split(":"(0))
        Dim strParts2() As String = strName2.Split(":"(0))
        Dim intIndex As Integer
        intIndex = LBound(strParts1)
        Do
            If intIndex > UBound(strParts1) Then
                If intIndex > UBound(strParts2) Then
                    Return 0
                Else
                    Return -1
                End If
            ElseIf intIndex > UBound(strParts2) Then
                Return 1
            Else
                If strParts1(intIndex) > strParts2(intIndex) Then
                    Return 1
                ElseIf strParts2(intIndex) > strParts1(intIndex) Then
                    Return -1
                End If
            End If
            intIndex = intIndex + 1
        Loop
    End Function

    Private Sub CompareTest(ByVal strName1 As String, ByVal strName2 As String, ByVal intExpectedResult As Integer)
        If intCompareCatNames(strName1, strName2) <> intExpectedResult Then
            Throw New Exception("Unexpected category name compare result: " + strName1 + " " + strName2)
        End If
    End Sub

    Private Function strNormalizeElement(ByVal strElement As String) As String
        Dim strResult As String
        strResult = UCase(Trim(strElement))
        'Make leading "I" sort before "E" for categories, and "C" at the end.
        If mlngListType = ListType.Category Then
            If VB.Left(strResult, 1) = "I" Then
                Mid(strResult, 1, 1) = Chr(1)
            ElseIf VB.Left(strResult, 1) = "E" Then
                Mid(strResult, 1, 1) = Chr(2)
            ElseIf VB.Left(strResult, 1) = "C" Then
                Mid(strResult, 1, 1) = Chr(3)
            End If
        End If
        strNormalizeElement = strResult
    End Function

    '$Description Remove an element from the list. Does not do any validation
    '   checking - the caller is responsible for things like key in use.

    Private Sub DeleteElement(ByVal intListIndex As Integer)
        lstElements.Items.RemoveAt(intListIndex)
    End Sub

    '$Description Rebuild mobjList from lstElements. lstElements is assumed to be in
    '   sorted order.

    Private Sub RebuildTranslator()
        Dim intIndex As Integer
        Dim objTransElem As StringTransElement

        Try

            mobjList.Init()
            For intIndex = 0 To lstElements.Items.Count - 1
                objTransElem = DirectCast(lstElements.Items(intIndex), StringTransElement)
                mobjList.Add(objTransElem.CloneElement())
            Next

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Function strMakeValue2(ByVal strValue1 As String) As String
        Dim intColonPos As Integer
        Dim intColonPos2 As Integer
        Dim intIndent As Integer
        If blnMultipleLevelsAllowed Then
            intColonPos = 0
            intIndent = 0
            Do
                intColonPos2 = InStr(intColonPos + 1, strValue1, ":")
                If intColonPos2 = 0 Then
                    Return Space(intIndent) & Mid(strValue1, intColonPos + 1)
                    Exit Do
                End If
                intColonPos = intColonPos2
                intIndent = intIndent + 1
            Loop
        Else
            Return strValue1
        End If
    End Function

    '$Description Write new file from mobjList. mobjList is assumed to be in sorted order.

    Private Sub WriteFile()
        Dim intIndex As Integer
        Dim strTmpFile As String

        Try
            strTmpFile = mobjCompany.AddNameToDataPath("NewList.tmp")
            Using objFile As IO.StreamWriter = New IO.StreamWriter(strTmpFile)
                objFile.WriteLine("Generated " & Now)
                With mobjList
                    For intIndex = 1 To .ElementCount
                        Dim strExtraPairs As String = ""
                        For Each objPair As KeyValuePair(Of String, String) In .GetElement(intIndex).ExtraValues
                            strExtraPairs += ("/" + objPair.Key + ":" + objPair.Value)
                        Next
                        objFile.WriteLine("/" & .GetKey(intIndex) & "/" & .GetValue1(intIndex) & "/" & .GetValue2(intIndex) & strExtraPairs)
                    Next
                End With
            End Using
            If Dir(mstrFile) <> "" Then
                Kill(mstrFile)
            End If
            Rename(strTmpFile, mstrFile)

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    '$Description Determine if lstElements entry intListIndex is used in any account.
    '   Checks for usage indicated by mlngListType.

    Private Function blnElementIsUsed(ByVal intListIndex As Integer, ByRef strAccounts As String) As Boolean
        Dim strKey As String
        Dim objAccount As Account

        Try

            strKey = DirectCast(lstElements.Items(intListIndex), StringTransElement).Key
            strAccounts = ""
            Dim strSep As String = ""
            Dim isUsed As Boolean = False
            For Each objAccount In mobjCompany.Accounts
                If blnElementIsUsedInAccount(objAccount, strKey) Then
                    strAccounts = strAccounts + strSep + objAccount.Title
                    strSep = ", "
                    isUsed = True
                End If
            Next objAccount

            Return isUsed
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Function blnElementIsUsedInAccount(ByVal objAccount As Account, ByVal strKey As String) As Boolean

        Dim objReg As Register

        Try

            For Each objReg In objAccount.Registers
                If blnElementIsUsedInRegister(objReg, strKey) Then
                    blnElementIsUsedInAccount = True
                    Exit Function
                End If
            Next objReg

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    Private Function blnElementIsUsedInRegister(ByVal objReg As Register, ByVal strKey As String) As Boolean

        Dim objTrx As BaseTrx
        Dim objSplit As TrxSplit

        Try

            For Each objTrx In objReg.GetAllTrx(Of BaseTrx)()
                If objTrx.GetType() Is GetType(BankTrx) Then
                    For Each objSplit In DirectCast(objTrx, BankTrx).Splits
                        If mlngListType = ListType.Category Then
                            If objSplit.CategoryKey = strKey Then
                                blnElementIsUsedInRegister = True
                                Exit Function
                            End If
                        ElseIf mlngListType = ListType.Budget Then
                            If objSplit.BudgetKey = strKey Then
                                blnElementIsUsedInRegister = True
                                Exit Function
                            End If
                        Else
                            RaiseErrorMsg("Unsupported list type")
                        End If
                    Next
                ElseIf mlngListType = ListType.Budget Then
                    If objTrx.GetType() Is GetType(BudgetTrx) Then
                        If DirectCast(objTrx, BudgetTrx).BudgetKey = strKey Then
                            blnElementIsUsedInRegister = True
                            Exit Function
                        End If
                    End If
                End If
            Next

            Exit Function
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Function

    '$Description Return the lowest positive integer which is not used as a key value
    '   by the list. Will fill in holes in the key sequence, even if those holes are
    '   created by deleting elements. Does not check if the value is used by a transaction,
    '   because all used key values must be present in the list and an element cannot
    '   be deleted if the key is in use.

    Private Function intGetUnusedKey() As Integer
        Dim intKey As Integer
        Dim intIndex As Integer
        Dim blnFound As Boolean

        intKey = 1
        Do
            blnFound = False
            For intIndex = 0 To lstElements.Items.Count - 1
                If DirectCast(lstElements.Items(intIndex), StringTransElement).Key = strMakeKey(intKey) Then
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
    '$Param AccountKey The integer to construct the key from.
    '$Returns The key string, or an empty string if the integer could not be
    '   converted to a key string. The only plausible reason for failure is if
    '   there are too many keys, so the integer is too large.

    Private Function strMakeKey(ByVal intKey As Integer) As String
        If intKey < 1 Or intKey > 999 Then
            strMakeKey = ""
        Else
            If mlngListType = ListType.Category Or intKey > 99 Then
                strMakeKey = Utilities.FormatInteger(intKey, "000")
            Else
                strMakeKey = Utilities.FormatInteger(intKey, "00")
            End If
        End If
    End Function

    '$Description Determine if first level changes are allowed to list.

    Private ReadOnly Property blnFirstLevelChangesAllowed() As Boolean
        Get
            blnFirstLevelChangesAllowed = (mlngListType <> ListType.Category)
        End Get
    End Property

    '$Description Determine if list can be multiple level.

    Private ReadOnly Property blnMultipleLevelsAllowed() As Boolean
        Get
            blnMultipleLevelsAllowed = (mlngListType = ListType.Category)
        End Get
    End Property
End Class