Option Strict On
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Public Class BankImportForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private WithEvents mobjAccount As Account
    Private mobjCompany As Company
    Private mobjImportHandler As IImportHandler
    Private mobjTrxReader As ITrxReader
    Private mblnIgnoreItemCheckedEvents As Boolean
    Private mobjHashedMatches As HashSet(Of NormalTrx)
    Private mblnTrapKeyPress As Boolean
    Public Shared intOpenCount As Integer

    Private Enum ImportStatus
        'Have not decided what to do with item yet.
        mlngIMPSTS_UNRESOLVED = 0
        'Item was matched to a prior import.
        mlngIMPSTS_PRIOR = 1
        'Item was used to create a new Trx.
        mlngIMPSTS_NEW = 2
        'Item was used to update an existing trx.
        mlngIMPSTS_UPDATE = 3
    End Enum

    'Item obtained from ITrxReader.
    Private Class ImportItem
        'ImportedTrx created by ITrxReader.
        Public objImportedTrx As ImportedTrx
        'Register objImportedTrx was added to or updated in.
        Public objReg As Register
        'The status of this ImportItem.
        Public lngStatus As ImportStatus
        'If this is not Nothing then objImportedTrx will be inserted or updated in objMatchedReg.
        Public objMatchedReg As Register
        'If set, identifies an exact match to be updated in objMatchedReg.
        'If not set, but objMatchedReg is set, then objImportedTrx will be inserted in objMatchedReg.
        Public objMatchedTrx As NormalTrx
        'Additional NormalTrx objImportedTrx is matched to as part of
        'a multi-part import match. These will get their amounts set to zero,
        'and modified versions of objImportedTrx.strImportKey.
        Public colExtraMatchedTrx As List(Of NormalTrx) = New List(Of NormalTrx)()
        'If not Nothing, this ImportItem is part of the indicated MultiPartMatch.
        'Multiple ImportItems may share the same MultiPartMatch object.
        Public objMultiPart As MultiPartMatch
        'If true, and objMultiPart is not Nothing, then this ImportItem is the one that
        'resulted in the multi-part match.
        Public blnMultiPartPrimary As Boolean

        Public Overrides Function ToString() As String
            With objImportedTrx
                Return Utilities.strFormatDate(.datDate) + " " + .strDescription + " " + Utilities.strFormatCurrency(.curAmount)
            End With
        End Function
    End Class

    Private Class ListWithTotal(Of T)
        Inherits List(Of T)
        Public curTotal As Decimal
    End Class

    Private Class ImportSubset
        Inherits ListWithTotal(Of ImportItem)
    End Class

    Private Class MatchSubset
        Inherits ListWithTotal(Of NormalTrx)
    End Class

    Private Class MultiPartMatch
        Public colImports As ImportSubset
        Public colMatches As MatchSubset
        Public intPriority As Integer       'The lower the number, the higher the priority

        Public Function strSummary() As String
            Return " a multi-part match of " + colImports.Count.ToString() +
                " import items to " + colMatches.Count.ToString() +
                " matches for $" + Utilities.strFormatCurrency(colImports.curTotal)
        End Function
    End Class

    'Column number in item list with index into maudtItem().
    Private Const mintITMCOL_INDEX As Integer = 7
    'Column number in match list with index into maudtMatch().
    Private Const mintMCHCOL_INDEX As Integer = 10

    'Imported transaction information.
    Private maudtItem() As ImportItem '1 to mintItems
    Private mintItems As Integer

    Private ReadOnly Iterator Property colAllMatchedTrx() As IEnumerable(Of NormalTrx)
        Get
            Dim item As ImportItem
            For Each item In maudtItem
                If Not item.objMatchedTrx Is Nothing Then
                    Yield item.objMatchedTrx
                End If
                For Each objExtraMatch As NormalTrx In item.colExtraMatchedTrx
                    Yield objExtraMatch
                Next
            Next
        End Get
    End Property

    'Matches to currently selected ImportItem.
    Private maudtMatch() As NormalTrx '1 to mintMatches
    Private mintMatches As Integer

    'Register selected in cboRegister.
    Private mobjSelectedRegister As Register

    'String for matching import(s).
    Private mstrImportSearchText As String
    Private mintNextImportToSearch As Integer

    Public Sub ShowMe(ByVal strTitle As String, ByVal objAccount As Account,
                      ByVal objImportHandler As IImportHandler,
                      ByVal objTrxReader As ITrxReader,
                      ByVal objHostUI As IHostUI)

        Try
            mobjHostUI = objHostUI
            mobjAccount = objAccount
            mobjCompany = mobjAccount.objCompany
            mobjImportHandler = objImportHandler
            mobjTrxReader = objTrxReader

            'This form is an MDI child.
            'This code simulates the VB6 
            ' functionality of automatically
            ' loading and showing an MDI
            ' child's parent.
            Me.MdiParent = objHostUI.objGetMainForm()
            objHostUI.objGetMainForm().Show()

            mstrImportSearchText = ""
            mintNextImportToSearch = 0
            mobjHashedMatches = Nothing
            ShowSearchFor()
            If Not blnLoadImports() Then
                Exit Sub
            End If
            DisplayImportItems()
            LoadRegisterList()
            UITools.LoadComboFromStringTranslator(cboDefaultCategory, mobjCompany.objCategories, True)

            Me.Text = strTitle
            ConfigureButtons()
            Me.Show()

            Exit Sub
        Catch ex As Exception
            Me.Close()
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ConfigureButtons()
        cmdBatchNew.Enabled = mobjImportHandler.blnAllowNew
        cmdFindNew.Enabled = mobjImportHandler.blnAllowNew
        cmdCreateNew.Enabled = mobjImportHandler.blnAllowNew
        cmdBatchUpdates.Enabled = mobjImportHandler.blnAllowBatchUpdates
        cmdFindUpdates.Enabled = mobjImportHandler.blnAllowBatchUpdates
        cmdUpdateExisting.Enabled = mobjImportHandler.blnAllowIndividualUpdates
    End Sub

    Private Sub chkHideCompleted_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkHideCompleted.CheckStateChanged
        Try

            DisplayImportItems()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdFindUpdates_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFindUpdates.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Integer
            Dim intFoundCount As Integer
            Dim strFailReason As String = ""

            ClearUpdateMatches()
            intFoundCount = 0
            For Each objItem In lvwTrx.Items
                ChangeItemCheckedSilently(objItem, False)

                intItemIndex = CInt(objItem.SubItems(mintITMCOL_INDEX).Text)
                If blnValidForAutoUpdate(intItemIndex, False, strFailReason) Then
                    ChangeItemCheckedSilently(objItem, True)
                    intFoundCount = intFoundCount + 1
                    With maudtItem(intItemIndex)
                        If .objMatchedTrx Is Nothing Then
                            objItem.ToolTipText = "Add as part of " + .objMultiPart.strSummary()
                        Else
                            Dim strTip As String
                            Dim strMultiPartNote As String = ""
                            If Not .objMultiPart Is Nothing Then
                                strMultiPartNote = ", in a " + .objMultiPart.strSummary()
                            End If
                            With .objMatchedTrx
                                strTip = "Matched to " + Utilities.strFormatDate(.datDate) + " " + .strDescription + " " + Utilities.strFormatCurrency(.curAmount) +
                                    strMultiPartNote + "."
                            End With
                            objItem.ToolTipText = strTip
                        End If
                        If .blnMultiPartPrimary And Not .objMultiPart Is Nothing Then
                            Dim objExplain As System.Text.StringBuilder = New System.Text.StringBuilder()
                            Dim curImportTotal As Decimal = 0D
                            objExplain.AppendLine("Import Items:")
                            For Each objImport As ImportItem In .objMultiPart.colImports
                                objExplain.AppendLine(" " + Utilities.strFormatDate(objImport.objImportedTrx.datDate) + " " +
                                                          objImport.objImportedTrx.strDescription + " " +
                                                          Utilities.strFormatCurrency(objImport.objImportedTrx.curAmount))
                                curImportTotal = curImportTotal + objImport.objImportedTrx.curAmount
                            Next
                            objExplain.AppendLine("Import Total Amount: " + Utilities.strFormatCurrency(curImportTotal))
                            objExplain.AppendLine()
                            objExplain.AppendLine("Matched Transactions:")
                            Dim curMatchTotal As Decimal = 0D
                            For Each objNormalTrx As NormalTrx In .objMultiPart.colMatches
                                objExplain.AppendLine(" " + Utilities.strFormatDate(objNormalTrx.datDate) + " " +
                                                          objNormalTrx.strDescription + " " +
                                                          Utilities.strFormatCurrency(objNormalTrx.curAmount))
                                curMatchTotal = curMatchTotal + objNormalTrx.curAmount
                            Next
                            objExplain.AppendLine("Match Total Amount: " + Utilities.strFormatCurrency(curImportTotal))
                            MsgBox(objExplain.ToString(), MsgBoxStyle.Information, "Multi-Part Match")
                        End If
                    End With
                Else
                    objItem.ToolTipText = strFailReason
                End If
            Next objItem

            MsgBox("Found " & intFoundCount & " imported transactions with exact matches.")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdBatchUpdates_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBatchUpdates.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Integer
            Dim strFailReason As String = ""
            Dim intUpdateCount As Integer
            Dim intMultiPartSeqNumber As Integer

            ClearUpdateMatches()
            For Each objItem In lvwTrx.Items
                If objItem.Checked Then
                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    'Could be set if a previous looked ahead for a multi-trx match
                    If maudtItem(intItemIndex).objMatchedTrx Is Nothing Then
                        If Not blnValidForAutoUpdate(intItemIndex, True, strFailReason) Then
                            ChangeItemCheckedSilently(objItem, False)
                            MsgBox("Skipping and unchecking " & strDescribeItem(intItemIndex) & " because: " & strFailReason & ".")
                        End If
                    End If
                End If
            Next objItem

            BeginProgress()
            intUpdateCount = 0
            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    With maudtItem(intItemIndex)
                        If .objMatchedTrx Is Nothing Then
                            If Not .objMatchedReg Is Nothing Then
                                'Insert .objImportedTrx in .objMatchedReg with .objImportedTrx.strImportKey
                                Dim datDummy As DateTime
                                .objImportedTrx.objReg = .objMatchedReg
                                If mobjHostUI.blnAddNormalTrxSilent(.objImportedTrx, datDummy, True, "ImportAutoBatch") Then
                                    MsgBox("Failed to insert transaction " + strDescribeItem(intItemIndex) + " required as part of a multi-part match.")
                                End If
                                .lngStatus = ImportStatus.mlngIMPSTS_NEW
                                .objReg = .objMatchedReg
                            End If
                        Else
                            mobjImportHandler.BatchUpdate(.objImportedTrx, .objMatchedTrx, 0)
                            .lngStatus = ImportStatus.mlngIMPSTS_UPDATE
                            .objReg = .objMatchedReg
                        End If
                        intMultiPartSeqNumber = 0
                        For Each objExtraMatch As NormalTrx In .colExtraMatchedTrx
                            intMultiPartSeqNumber = intMultiPartSeqNumber + 1
                            mobjImportHandler.BatchUpdate(.objImportedTrx, objExtraMatch, intMultiPartSeqNumber)
                        Next
                        DisplayOneImportItem(objItem, intItemIndex)
                        intUpdateCount = intUpdateCount + 1
                        UpdateProgress(.objReg)
                    End With
                End If
            Next objItem
            EndProgress()
            MsgBox("Marked " & intUpdateCount & " transactions as imported, " & mobjImportHandler.strBatchUpdateFields + ".")

            Exit Sub
        Catch ex As Exception
            EndProgress()
            gTopException(ex)
        End Try
    End Sub

    Private Sub ClearUpdateMatches()
        Dim intIndex As Integer
        For intIndex = 1 To mintItems
            With maudtItem(intIndex)
                .objMatchedReg = Nothing
                .objMatchedTrx = Nothing
                .colExtraMatchedTrx = New List(Of NormalTrx)()
                mobjHashedMatches = Nothing
                .objMultiPart = Nothing
                .blnMultiPartPrimary = False
            End With
        Next
    End Sub

    Private Function blnValidForAutoUpdate(ByVal intItemIndex As Integer, ByVal blnAllowNonExact As Boolean, ByRef strFailReason As String) As Boolean

        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim colUnusedMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim intExactCount As Integer
        Dim objPossibleMatchTrx As NormalTrx
        Dim blnNonExactConfirmed As Boolean
        Dim blnCheckWithoutAmount As Boolean

        strFailReason = "Unspecified"

        With maudtItem(intItemIndex)
            If .lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
                strFailReason = "Transaction already imported"
                Return False
            End If

            If Not .objMatchedReg Is Nothing Then
                'ImportItem was chosen for insert or update as part of an earlier multi-part match.
                'This will cause the item to be checked if this method was called from cmdFindUpdates_Click(),
                'which is redundant because all items in a multi-part matched are checked by UseMultiPartMatch(),
                'but there is no harm from this.
                Return True
            End If

            objImportedTrx = .objImportedTrx
            'Verify that all the checked ImportItems really do have a
            'single exact match, because the user may have checked additional items.
            intExactCount = 0
            For Each objReg In mobjAccount.colRegisters

                mobjImportHandler.BatchUpdateSearch(objReg, objImportedTrx, colAllMatchedTrx, colUnusedMatches, blnExactMatch)
                'If we have one match that wasn't matched by a previous import item.
                If colUnusedMatches.Count() = 1 Then
                    blnNonExactConfirmed = False
                    If blnAllowNonExact And Not blnExactMatch Then
                        If MsgBox(strDescribeItem(intItemIndex) & " is only a partial match. " & "Update it anyway?", MsgBoxStyle.YesNo, "Confirm") = MsgBoxResult.Yes Then
                            blnNonExactConfirmed = True
                        End If
                    End If
                    objPossibleMatchTrx = Utilities.objFirstElement(colUnusedMatches)
                    blnCheckWithoutAmount = False
                    'A check in the register with a zero amount means we didn't know the amount when we entered it, or imported it.
                    If Val(objPossibleMatchTrx.strNumber) > 0 And objPossibleMatchTrx.curAmount = 0.0# Then
                        blnCheckWithoutAmount = True
                    End If
                    If (blnExactMatch Or blnNonExactConfirmed Or blnCheckWithoutAmount) Then
                        If objPossibleMatchTrx.strImportKey = "" Then
                            .objMatchedTrx = objPossibleMatchTrx
                            .objMatchedReg = objReg
                            mobjHashedMatches = Nothing
                            intExactCount = intExactCount + 1
                        End If
                    End If
                End If
            Next objReg
            If intExactCount = 1 Then
                Return True
            End If

            If mobjImportHandler.blnAllowMultiPartMatches Then
                If blnFindMultiPartMatch(intItemIndex) Then
                    Return True
                End If
            End If
        End With

        If intExactCount = 0 Then
            strFailReason = "Could not find an identical or very similar transaction to update"
        Else
            strFailReason = "Cannot decide between multiple identical or very similar transactions to update"
        End If
        Return False

    End Function

    'Look for import and match trx with the same payee name root within a few days of objImportedTrx.
    'Construct all subsets we want to try of each set in multi-part matches.
    'Find all pairs of these subsets with the same total amounts.
    'Choose the highest priority multi-part match and use it.
    'There can be many multi-part matches among these pairs, because two pairs that don't
    'overlap can be used to create a new pair by adding their contents together.

    Private Function blnFindMultiPartMatch(ByVal intRootItemIndex As Integer) As Boolean

        Dim colImportSubset As ImportSubset
        Dim colImportSubsets As List(Of ImportSubset)
        Dim colMatchSubset As MatchSubset
        Dim colMatchSubsets As List(Of MatchSubset)
        Dim colMultiPartMatches As List(Of MultiPartMatch)
        Dim strDescrStartsWith As String
        Dim colAllCandidateImports As List(Of ImportItem)
        Dim colAllCandidateMatches As List(Of NormalTrx)
        Dim objRootImportItem As ImportItem
        Dim objRootImportedTrx As ImportedTrx

        objRootImportItem = maudtItem(intRootItemIndex)
        objRootImportedTrx = objRootImportItem.objImportedTrx
        strDescrStartsWith = VB.Left(objRootImportedTrx.strDescription, 8)
        colAllCandidateImports = colFindAllCandidateImports(strDescrStartsWith, intRootItemIndex)
        colAllCandidateMatches = colFindAllCandidateMatches(strDescrStartsWith, objRootImportedTrx.datDate)

        'No multi-part matches are possible unless at least one of the input collections
        'has more than one member, and both input collections have at least one possible match.
        'colAllOtherPossibleImports can be empty because we add the root item to it to get
        'the import item input collection.
        If (colAllCandidateImports.Count + 1) < 2 And colAllCandidateMatches.Count < 2 Then
            Return False
        End If
        If colAllCandidateMatches.Count = 0 Then
            Return False
        End If

        'Prevent combinatorial explosion from too many combinations.
        If colAllCandidateImports.Count + colAllCandidateMatches.Count > 24 Then
            Return False
        End If

        'Limit size of subsets to keep the number of subsets down,
        'and thereby the number of subset combinations.
        colImportSubsets = New List(Of ImportSubset)()
        For Each colImportSubset In colGetSubsets(Of ImportItem, ImportSubset)(colAllCandidateImports, 9,
                Function(ByVal objItem As ImportItem) As Decimal
                    Return objItem.objImportedTrx.curAmount
                End Function)
            colImportSubset.Add(objRootImportItem)
            colImportSubset.curTotal = colImportSubset.curTotal + objRootImportItem.objImportedTrx.curAmount
            colImportSubsets.Add(colImportSubset)
        Next

        'Limit size of subsets to keep the number of subsets down,
        'and thereby the number of subset combinations.
        colMatchSubsets = New List(Of MatchSubset)()
        For Each colMatchSubset In colGetSubsets(Of NormalTrx, MatchSubset)(colAllCandidateMatches, 9,
            Function(ByVal objItem As NormalTrx) As Decimal
                Return objItem.curAmount
            End Function)
            colMatchSubsets.Add(colMatchSubset)
        Next

        'Find all subset pairs with the same totals.
        colMultiPartMatches = New List(Of MultiPartMatch)()
        For Each colImportSubset In colImportSubsets
            For Each colMatchSubset In colMatchSubsets
                If colImportSubset.curTotal = colMatchSubset.curTotal Then
                    Dim objMultiPart As MultiPartMatch = New MultiPartMatch()
                    objMultiPart.colImports = colImportSubset
                    objMultiPart.colMatches = colMatchSubset
                    colMultiPartMatches.Add(objMultiPart)
                End If
            Next
        Next

        If colMultiPartMatches.Count = 0 Then
            Return False
        End If

        'The order in which we try MultiPartMatch objects is an art, not a science,
        'and reflects the general notion we want to try "simpler" matches first.
        'So I use the total number of match parts as a "score", with a credit 
        'for a better priority if there are the same number of ImportItems and matched NormalTrx.
        'Then sort in order of priority (lower numbers first, i.e. higher priority) and
        'use the first multi-part match in this order (the highest priority).

        For Each objMultiPart As MultiPartMatch In colMultiPartMatches
            objMultiPart.intPriority = objMultiPart.colImports.Count + objMultiPart.colMatches.Count
            If objMultiPart.colImports.Count = objMultiPart.colMatches.Count Then
                objMultiPart.intPriority = objMultiPart.intPriority - 1
            End If
        Next
        colMultiPartMatches.Sort(Function(ByVal objMP1 As MultiPartMatch, ByVal objMP2 As MultiPartMatch) As Integer
                                     Return objMP1.intPriority.CompareTo(objMP2.intPriority)
                                 End Function)

        Dim objHighestPriorityMultiPartMatch As MultiPartMatch = colMultiPartMatches(0)
        UseMultiPartMatch(intRootItemIndex, objHighestPriorityMultiPartMatch)

        Return True

    End Function

    ''' <summary>
    ''' Find all ImportItems that may be used in multi-part matches involving the ImportItem
    ''' at the index passed (the root ImportItem), except for the root ImportItem itself.
    ''' This includes all ImportItems with matching description and amount that have not
    ''' already been matched to a NormalTrx.
    ''' </summary>
    ''' <param name="strDescrStartsWith"></param>
    ''' <param name="intRootItemIndex"></param>
    ''' <returns></returns>
    Private Function colFindAllCandidateImports(ByVal strDescrStartsWith As String, ByVal intRootItemIndex As Integer) As List(Of ImportItem)
        Dim datRootDate As DateTime
        Dim datStartDate As DateTime
        Dim datEndDate As DateTime
        Dim objImportItem As ImportItem
        Dim objImportedTrx As ImportedTrx

        datRootDate = maudtItem(intRootItemIndex).objImportedTrx.datDate
        datStartDate = datRootDate.AddDays(-2.0#)
        datEndDate = datRootDate.AddDays(2.0#)
        Dim colResult As List(Of ImportItem) = New List(Of ImportItem)()
        For intItemIndex As Integer = 1 To mintItems
            objImportItem = maudtItem(intItemIndex)
            If (objImportItem.objMatchedReg Is Nothing) And (objImportItem.lngStatus = ImportStatus.mlngIMPSTS_UNRESOLVED) Then
                objImportedTrx = objImportItem.objImportedTrx
                If objImportedTrx.datDate >= datStartDate And objImportedTrx.datDate <= datEndDate Then
                    If intItemIndex <> intRootItemIndex Then
                        If objImportedTrx.strDescription.StartsWith(strDescrStartsWith, StringComparison.InvariantCultureIgnoreCase) Then
                            colResult.Add(objImportItem)
                        End If
                    End If
                End If
            End If
        Next
        Return colResult
    End Function

    ''' <summary>
    ''' Find all NormalTrx that may be used in multi-part matches for the given
    ''' transaction description and date. This includes all NormalTrx that have
    ''' not already been assigned to an ImportItem, that match the description and date.
    ''' </summary>
    ''' <param name="strDescrStartsWith"></param>
    ''' <param name="datDate"></param>
    ''' <returns></returns>
    Private Function colFindAllCandidateMatches(ByVal strDescrStartsWith As String, ByVal datDate As DateTime) As List(Of NormalTrx)
        Dim objReg As Register
        Dim intRegIndex As Integer
        Dim objTrx As Trx
        Dim objNormalTrx As NormalTrx
        Dim colResult As List(Of NormalTrx) = New List(Of NormalTrx)()
        Dim datStartDate As DateTime
        Dim datEndDate As DateTime
        datStartDate = datDate.AddDays(-6.0#)
        datEndDate = datDate.AddDays(2.0#)
        For Each objReg In mobjAccount.colRegisters
            For intRegIndex = objReg.lngFindBeforeDate(datStartDate) + 1 To objReg.lngTrxCount
                objTrx = objReg.objTrx(intRegIndex)
                If objTrx.datDate > datEndDate Then
                    Exit For
                End If
                objNormalTrx = TryCast(objTrx, NormalTrx)
                If Not objNormalTrx Is Nothing Then
                    If objNormalTrx.strDescription.StartsWith(strDescrStartsWith) Then
                        If Not blnTrxIsMatched(objNormalTrx) Then
                            colResult.Add(objNormalTrx)
                            'Else  'uncomment to allow a breakpoint when debugging
                            '    objNormalTrx = objNormalTrx
                        End If
                    End If
                End If
            Next
        Next
        Return colResult
    End Function

    Private Function blnTrxIsMatched(ByVal objNormalTrx As NormalTrx) As Boolean
        If mobjHashedMatches Is Nothing Then
            mobjHashedMatches = New HashSet(Of NormalTrx)()
            For Each objAdd As NormalTrx In colAllMatchedTrx
                mobjHashedMatches.Add(objAdd)
            Next
        End If
        Return mobjHashedMatches.Contains(objNormalTrx)
    End Function

    Private Delegate Function curGetAmount(Of T)(ByVal objItem As T) As Decimal

    ''' <summary>
    ''' Iterate through all possible subsets of colCandidates, except the null set and
    ''' sets with more than intMaxSize members. Call a delegate for each element added
    ''' to a subset, each time that element is added.
    ''' Each element returned by the iteration is a List(Of T). So if there are multiple
    ''' subsets of colCandidates, the iterator will get a List(Of T) for each one.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="colCandidates"></param>
    ''' <returns></returns>
    Private Iterator Function colGetSubsets(Of T, TList As {ListWithTotal(Of T), New})(
                ByVal colCandidates As List(Of T), ByVal intMaxSize As Integer,
                ByVal dlgGetAmount As curGetAmount(Of T)) As IEnumerable(Of TList)
        'Members of a set can be mapped to a bit positions in the binary representation 
        'of a number with the same number of bits as members of the set.
        'Generate all possible numbers that can be represented in that number of bits with at least one bit set,
        'and for each number return the candidates that map to bits that are set in that number.
        'For example, decimal 5 (binary 101) represents the first and third candidates.
        'Skip zero because it has no bits set, i.e. it represents the null set.
        For intCounter As Integer = 1 To CInt(2 ^ colCandidates.Count) - 1
            Dim intBitMask As Integer = 1
            Dim colResults As TList = New TList()
            colResults.curTotal = 0D
            For intBitPosition As Integer = 0 To colCandidates.Count - 1
                If (intCounter And intBitMask) <> 0 Then
                    Dim objItem As T = colCandidates.Item(intBitPosition)
                    colResults.curTotal = colResults.curTotal + dlgGetAmount(objItem)
                    colResults.Add(objItem)
                End If
                intBitMask = intBitMask * 2
            Next
            If colResults.Count <= intMaxSize Then
                Yield colResults
            End If
        Next
    End Function

    ''' <summary>
    ''' Decide what to do with each ImportItem and NormalTrx in a multi-part match.
    ''' Make each element of colImportSubset point to one element of colMatchSubset,
    ''' for the first "n" elements in each list where "n" is the lesser of colImportSubset.Count 
    ''' and colMatchSubset.Count. Those NormalTrx will have their amounts set to the amounts
    ''' of their paired ImportItems. Extra elements of colImportSubset will be inserted in the last
    ''' register updated by the preceding step. Extra elements of colMatchSubset have their
    ''' amounts set to zero. The net result is to make the matched NormalTrx look like the
    ''' ImportItems, inserting, adjusting or zeroing out NormalTrx as needed. The actual inserts and
    ''' updates are done later, not here, but WHAT WILL happen is decided here and reflected in how
    ''' .objMatchedReg, .objMatchedTrx and .colExtraMatchedTrx are set.
    ''' </summary>
    Private Sub UseMultiPartMatch(ByVal intRootItemIndex As Integer, ByVal objMultiPart As MultiPartMatch)

        Dim objImportEnum As IEnumerator(Of ImportItem) = objMultiPart.colImports.GetEnumerator()
        Dim objMatchEnum As IEnumerator(Of NormalTrx) = objMultiPart.colMatches.GetEnumerator()
        Dim objLastImportItem As ImportItem = Nothing
        Dim objLastMatchedTrx As NormalTrx = Nothing

        maudtItem(intRootItemIndex).blnMultiPartPrimary = True
        'Walk through both enumerators in parallel, until one reaches the end.
        'Then do the appropriate thing to the remaining elements in the other enumerator.
        Do
            If Not objImportEnum.MoveNext() Then
                'We ran out of ImportItem objects, so have to put the remaining matched NormalTrx
                'in the list to be zeroed out later.
                Do
                    '.MoveNext() BEFORE using .Current, because we haven't
                    'advanced objMatchEnum yet in the outer loop.
                    If Not objMatchEnum.MoveNext() Then
                        Exit Do
                    End If
                    'Add to the list of matched NormalTrx to zero out.
                    objLastImportItem.colExtraMatchedTrx.Add(objMatchEnum.Current)
                    mobjHashedMatches = Nothing
                Loop
                Exit Do
            End If
            objLastImportItem = objImportEnum.Current
            If Not objMatchEnum.MoveNext() Then
                'We ran out of matched NormalTrx, so create new NormalTrx for each remaining ImportItem.
                Do
                    objImportEnum.Current.objMultiPart = objMultiPart
                    objImportEnum.Current.objMatchedReg = objLastMatchedTrx.objReg
                    objImportEnum.Current.objMatchedTrx = Nothing
                    mobjHashedMatches = Nothing
                    '.MoveNext() AFTER using .Current, because we already advanced
                    'objImportEnum in the outer loop.
                    If Not objImportEnum.MoveNext() Then
                        Exit Do
                    End If
                Loop
                Exit Do
            End If
            objLastMatchedTrx = objMatchEnum.Current
            objImportEnum.Current.objMultiPart = objMultiPart
            objImportEnum.Current.objMatchedReg = objMatchEnum.Current.objReg
            objImportEnum.Current.objMatchedTrx = objMatchEnum.Current
            mobjHashedMatches = Nothing
        Loop
        'Any of these items that come after the root item in the multi-part set would also be
        'found by blnValidForAutoUpdate() when it checks .objMatchedReg, which would cause them
        'to be checked redundantly after UseMultiPartMatch() returns, but there is no harm in this.
        SetCheckStateOnAllMultiParts(objMultiPart, True)
    End Sub

    Private Sub cmdFindNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFindNew.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Integer
            Dim intFoundCount As Integer
            Dim strFailReason As String = ""

            intFoundCount = 0
            For Each objItem In lvwTrx.Items
                ChangeItemCheckedSilently(objItem, False)

                intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                If blnValidForAutoNew(intItemIndex, False, False, strFailReason) Then
                    ChangeItemCheckedSilently(objItem, True)
                    intFoundCount = intFoundCount + 1
                    'UPGRADE_ISSUE: MSComctlLib.ListItem property objItem.ToolTipText was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    objItem.ToolTipText = "Selected"
                Else
                    'UPGRADE_ISSUE: MSComctlLib.ListItem property objItem.ToolTipText was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    objItem.ToolTipText = strFailReason
                End If
            Next objItem

            MsgBox("Found " & intFoundCount & " imported transactions to turn into new transactions.")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdBatchNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBatchNew.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Integer
            Dim intCreateCount As Integer
            Dim datDummy As Date
            Dim objImportedTrx As ImportedTrx
            Dim blnItemImported As Boolean
            Dim blnManualSelectionAllowed As Boolean
            Dim strFailReason As String = ""

            If mobjSelectedRegister Is Nothing Then
                mobjHostUI.ErrorMessageBox("First select the register to create the transactions in.")
                Exit Sub
            End If

            If MsgBox("Do you really want to create new transactions for everything" & " you have checked?",
                      MsgBoxStyle.Question Or MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
                Exit Sub
            End If

            blnManualSelectionAllowed = (chkAllowManualBatchNew.CheckState = System.Windows.Forms.CheckState.Checked)

            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    If Not blnValidForAutoNew(intItemIndex, blnManualSelectionAllowed, True, strFailReason) Then
                        MsgBox("Skipping and unchecking " & strDescribeItem(intItemIndex) & " because: " & strFailReason & ".")
                        ChangeItemCheckedSilently(objItem, False)
                    End If
                End If
            Next objItem

            BeginProgress()
            intCreateCount = 0
            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    objImportedTrx = maudtItem(intItemIndex).objImportedTrx
                    blnItemImported = mobjImportHandler.blnAlternateAutoNewHandling(objImportedTrx, mobjSelectedRegister)
                    'If we did not use alternate handling.
                    If Not blnItemImported Then
                        objImportedTrx.objReg = mobjSelectedRegister
                        If Not mobjHostUI.blnAddNormalTrxSilent(objImportedTrx, datDummy, True, "ImportNewBatch") Then
                            blnItemImported = True
                        End If
                    End If
                    'Now update the UI on the import form and any open register forms.
                    If blnItemImported Then
                        With maudtItem(intItemIndex)
                            .lngStatus = ImportStatus.mlngIMPSTS_NEW
                            .objReg = mobjSelectedRegister
                        End With
                        DisplayOneImportItem(objItem, intItemIndex)
                        intCreateCount = intCreateCount + 1
                        UpdateProgress(mobjSelectedRegister)
                    End If
                End If
            Next objItem
            EndProgress()

            MsgBox("Imported " & intCreateCount & " transactions into the selected register.")

            Exit Sub
        Catch ex As Exception
            EndProgress()
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidForAutoNew(ByRef intItemIndex As Integer, ByVal blnManualSelectionAllowed As Boolean, ByVal blnSetMissingCategory As Boolean, ByRef strFailReason As String) As Boolean

        Dim objReg As Register
        Dim objImportedTrx As ImportedTrx
        Dim objSplit As TrxSplit
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim lngCatIdx As Integer
        Dim strDefaultCatKey As String

        blnValidForAutoNew = False
        strFailReason = "Unspecified"

        If maudtItem(intItemIndex).lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
            strFailReason = "Transaction already imported"
            Return False
        End If

        objImportedTrx = maudtItem(intItemIndex).objImportedTrx
        If objImportedTrx.lngSplits = 0 Then
            strFailReason = "Transaction has no splits"
            Return False
        End If
        If objImportedTrx.lngNarrowMethod <> ImportMatchNarrowMethod.None Then
            strFailReason = "Memorized transaction has a narrowing method"
            Return False
        End If

        objSplit = objImportedTrx.objFirstSplit
        If objSplit.strCategoryKey = "" And blnSetMissingCategory Then
            If cboDefaultCategory.SelectedIndex <> -1 Then
                lngCatIdx = UITools.GetItemData(cboDefaultCategory, cboDefaultCategory.SelectedIndex)
                If lngCatIdx > 0 Then
                    strDefaultCatKey = mobjCompany.objCategories.strKey(lngCatIdx)
                    objSplit.strCategoryKey = strDefaultCatKey
                End If
            End If
        End If
        If objSplit.strCategoryKey = "" Then
            strFailReason = "Transaction has no category"
            Return False
        End If

        strFailReason = mobjImportHandler.strAutoNewValidationError(objImportedTrx, mobjAccount, blnManualSelectionAllowed)
        If Not String.IsNullOrEmpty(strFailReason) Then
            Return False
        End If

        For Each objReg In mobjAccount.colRegisters
            colMatches = New List(Of NormalTrx)
            blnExactMatch = False
            mobjImportHandler.AutoNewSearch(objImportedTrx, objReg, colMatches, blnExactMatch)
            If colMatches.Count() > 0 And blnExactMatch Then
                strFailReason = "An identical or very similar transaction already exists"
                Return False
            End If
        Next objReg

        Return True

    End Function

    Private Sub BeginProgress()
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
    End Sub

    Private Sub UpdateProgress(ByVal objReg As Register)
        System.Windows.Forms.Application.DoEvents()
        objReg.RaiseShowCurrent()
    End Sub

    Private Sub EndProgress()
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdRefreshItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRefreshItems.Click
        Try

            DisplayImportItems()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnLoadImports() As Boolean
        Dim objImportedTrx As ImportedTrx

        Try

            If Not mobjTrxReader.blnOpenSource(mobjAccount) Then
                Exit Function
            End If
            lblReadFrom.Text = "Items read from " & mobjTrxReader.strSource

            blnLoadImports = True
            mintItems = 0
            ReDim maudtItem(1)
            maudtItem(0) = New ImportItem()

            Do
                objImportedTrx = mobjTrxReader.objNextTrx()
                If objImportedTrx Is Nothing Then
                    Exit Do
                End If
                mintItems = mintItems + 1
                ReDim Preserve maudtItem(mintItems)
                maudtItem(mintItems) = New ImportItem()
                With maudtItem(mintItems)
                    .objReg = Nothing
                    .objImportedTrx = objImportedTrx
                    .lngStatus = ImportStatus.mlngIMPSTS_UNRESOLVED
                End With
            Loop

            Array.Sort(Of ImportItem)(maudtItem, AddressOf ImportItemComparer)

            mobjTrxReader.CloseSource()

            Exit Function
        Catch ex As Exception
            mobjTrxReader.CloseSource()
            gNestedException(ex)
        End Try
    End Function

    Private Function ImportItemComparer(ByVal objItem1 As ImportItem, ByVal objItem2 As ImportItem) As Integer
        Dim intResult As Integer
        If objItem1.objImportedTrx Is Nothing Then
            If objItem2.objImportedTrx Is Nothing Then
                Return 0
            End If
            Return -1
        End If
        If objItem2.objImportedTrx Is Nothing Then
            Return 1
        End If
        intResult = objItem1.objImportedTrx.datDate.CompareTo(objItem2.objImportedTrx.datDate)
        If intResult <> 0 Then
            Return intResult
        End If
        intResult = objItem1.objImportedTrx.strNumber.CompareTo(objItem2.objImportedTrx.strNumber)
        If intResult <> 0 Then
            Return intResult
        End If
        Return objItem1.objImportedTrx.strDescription.CompareTo(objItem2.objImportedTrx.strDescription)
    End Function

    '$Description Display import items.

    Private Sub DisplayImportItems()
        Dim intIndex As Integer
        Dim blnShowCompleted As Boolean
        Dim intOldSelectedIndex As Integer
        Dim objNewItem As System.Windows.Forms.ListViewItem = Nothing
        Dim objNewSelectedItem As System.Windows.Forms.ListViewItem = Nothing

        Try

            ClearCurrentItemMatches()
            blnShowCompleted = (chkHideCompleted.CheckState <> System.Windows.Forms.CheckState.Checked)
            If Not lvwTrx.FocusedItem Is Nothing Then
                intOldSelectedIndex = intSelectedItemIndex()
            End If
            mblnIgnoreItemCheckedEvents = True
            Dim strDescriptionFilter As String = txtDescriptionFilter.Text.TrimEnd().ToLower()
            lvwTrx.Items.Clear()
            For intIndex = 1 To mintItems
                Dim blnMatchesFilter As Boolean
                If strDescriptionFilter = "" Then
                    blnMatchesFilter = True
                Else
                    blnMatchesFilter = maudtItem(intIndex).objImportedTrx.strDescription.ToLower().Contains(strDescriptionFilter)
                End If
                If (maudtItem(intIndex).lngStatus = ImportStatus.mlngIMPSTS_UNRESOLVED Or blnShowCompleted) And blnMatchesFilter Then
                    blnMatchImport(intIndex)
                    objNewItem = objAddToImportList(intIndex)
                    If intIndex = intOldSelectedIndex Then
                        objNewSelectedItem = objNewItem
                    End If
                End If
            Next
            ClearLvwSelection(lvwTrx)
            If Not objNewSelectedItem Is Nothing Then
                lvwTrx.FocusedItem = objNewSelectedItem
                objNewSelectedItem.EnsureVisible()
            End If
            mblnIgnoreItemCheckedEvents = False

            Exit Sub
        Catch ex As Exception
            mblnIgnoreItemCheckedEvents = False
            gNestedException(ex)
        End Try
    End Sub

    Private Function objAddToImportList(ByVal intIndex As Integer) As System.Windows.Forms.ListViewItem
        Dim objItem As System.Windows.Forms.ListViewItem

        objAddToImportList = Nothing
        Try

            objItem = UITools.ListViewAdd(lvwTrx)
            DisplayOneImportItem(objItem, intIndex)
            objAddToImportList = objItem

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub DisplayOneImportItem(ByVal objItem As System.Windows.Forms.ListViewItem, ByVal intIndex As Integer)

        Dim objTrx As ImportedTrx = maudtItem(intIndex).objImportedTrx
        Dim objReg As Register = maudtItem(intIndex).objReg
        Dim strStatus As String = ""
        Dim strRegTitle As String = ""

        Try

            Select Case maudtItem(intIndex).lngStatus
                Case ImportStatus.mlngIMPSTS_PRIOR
                    strStatus = "Prior"
                Case ImportStatus.mlngIMPSTS_NEW
                    strStatus = "New"
                Case ImportStatus.mlngIMPSTS_UPDATE
                    strStatus = "Update"
            End Select
            With objItem
                .Text = Utilities.strFormatDate(objTrx.datDate)
                .SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
                .SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
                .SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, Utilities.strFormatCurrency(objTrx.curAmount)))
                .SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, strSummarizeTrxCat(objTrx)))
                .SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, strStatus))
                If Not objReg Is Nothing Then
                    strRegTitle = objReg.strTitle
                End If
                .SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strRegTitle))
                .SubItems.Insert(mintITMCOL_INDEX, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(intIndex)))
            End With

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub LoadRegisterList()
        Dim objReg As Register
        Dim intIndex As Integer

        With cboRegister
            .Items.Clear()
            intIndex = 0
            For Each objReg In mobjAccount.colRegisters
                .Items.Add(UITools.CreateListBoxItem(objReg.strTitle, intIndex))
                intIndex = intIndex + 1
            Next objReg
        End With
    End Sub

    Private Sub BankImportForm_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        If mblnTrapKeyPress Then
            Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
            Try
                If KeyAscii >= 32 And KeyAscii <= 126 Then
                    mstrImportSearchText = mstrImportSearchText & Char.ToLower(Chr(KeyAscii))
                    ShowSearchFor()
                    CancelKeyPress(eventArgs)
                    Return
                ElseIf KeyAscii = 3 Then  '^C (clear search string)
                    mstrImportSearchText = ""
                    ShowSearchFor()
                    CancelKeyPress(eventArgs)
                    Return
                ElseIf KeyAscii = 8 Then  'Backspace (delete last char from search string)
                    If Len(mstrImportSearchText) > 0 Then
                        mstrImportSearchText = VB.Left(mstrImportSearchText, Len(mstrImportSearchText) - 1)
                    End If
                    ShowSearchFor()
                    CancelKeyPress(eventArgs)
                    Return
                ElseIf KeyAscii = 19 Then  '^S (search for search string)
                    FindMatchingImport()
                    CancelKeyPress(eventArgs)
                    Return
                End If

            Catch ex As Exception
                gTopException(ex)
            End Try
        End If
    End Sub

    Private Sub CancelKeyPress(eventArgs As KeyPressEventArgs)
        eventArgs.KeyChar = Chr(0)
        eventArgs.Handled = True
    End Sub

    Private Sub FindMatchingImport()
        Dim intListItemIndex As Integer
        Dim intItemArrayIndex As Integer

        If mstrImportSearchText = "" Then
            MsgBox("Type something to search for before pressing ^S to find it.")
            Exit Sub
        End If
        intListItemIndex = mintNextImportToSearch
        Do
            If intListItemIndex >= lvwTrx.Items.Count Then
                MsgBox("Could not find an import item matching """ & mstrImportSearchText & """.")
                Exit Sub
            End If
            intItemArrayIndex = CShort(lvwTrx.Items.Item(intListItemIndex).SubItems(mintITMCOL_INDEX).Text)
            With maudtItem(intItemArrayIndex).objImportedTrx
                If StrComp(.strNumber.ToLower(), mstrImportSearchText, CompareMethod.Text) = 0 Or Utilities.strFormatCurrency(.curAmount) = mstrImportSearchText Or InStr(1, .strDescription.ToLower(), mstrImportSearchText, CompareMethod.Text) > 0 Then
                    lvwTrx.SelectedItems.Clear()
                    lvwTrx.FocusedItem = lvwTrx.Items.Item(intListItemIndex)
                    lvwTrx.FocusedItem.Selected = True
                    lvwTrx.FocusedItem.EnsureVisible()
                    mintNextImportToSearch = intListItemIndex + 1
                    Exit Sub
                End If
            End With
            intListItemIndex = intListItemIndex + 1
        Loop
    End Sub

    Private Sub ShowSearchFor()
        If mstrImportSearchText = "" Then
            lblSearchFor.Text = "Type something and press ^S to search for that text"
        Else
            lblSearchFor.Text = "^S to search for """ & mstrImportSearchText & """, backspace to edit, ^C to start over)"
        End If
        mintNextImportToSearch = 0
    End Sub

    Private Sub BankImportForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 1115
        Me.Height = 587
        mblnTrapKeyPress = True
        intOpenCount = intOpenCount + 1
    End Sub

    Private Sub BankImportForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        intOpenCount = intOpenCount - 1
    End Sub

    Private Sub lvwTrx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwTrx.Click
        Try

            If Not lvwTrx.FocusedItem Is Nothing Then
                mintNextImportToSearch = lvwTrx.FocusedItem.Index
                SearchForMatches()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdRepeatSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRepeatSearch.Click
        Try

            'Did they select anything?
            If lvwTrx.FocusedItem Is Nothing Then
                mobjHostUI.ErrorMessageBox("Select an item in the top list first.")
                Exit Sub
            End If
            SearchForMatches()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    'Search for matches to the import item currently selected in the top list.
    'Redisplays the import item with current status if that import item has already been imported.

    Private Sub SearchForMatches()
        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim intItemIndex As Integer
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim objMatchedTrx As NormalTrx

        Try
            ClearCurrentItemMatches()

            'Has the selected item already been processed?
            intItemIndex = intSelectedItemIndex()
            If maudtItem(intItemIndex).lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
                Exit Sub
            End If

            'This is the import item they selected.
            objImportedTrx = maudtItem(intItemIndex).objImportedTrx

            'Not usually possible to match here, because the item would have been matched
            'when loaded and detected a few lines above where it checks the import status.
            'Probably means the matching trx was changed since the imports were loaded,
            'and it only matches now.
            If blnMatchImport(intItemIndex) Then
                RedisplaySelectedItem()
                Exit Sub
            End If

            'Look for possible matches in ALL registers, not just the selected register.
            For Each objReg In mobjAccount.colRegisters
                mobjImportHandler.IndividualSearch(objReg, objImportedTrx,
                    chkLooseMatch.CheckState = System.Windows.Forms.CheckState.Checked,
                    colMatches, blnExactMatch)
                For Each objMatchedTrx In colMatches
                    mintMatches = mintMatches + 1
                    ReDim Preserve maudtMatch(mintMatches)
                    maudtMatch(mintMatches) = objMatchedTrx
                    DisplayMatch(objMatchedTrx, mintMatches)
                Next
            Next objReg

            'Deselect everything in list (the first item is selected by default).
            ClearLvwSelection(lvwMatches)
            'Select the first matched trx which is not already imported,
            'then make a trx a few rows below visible to scroll the first
            'non-imported to the middle of the list control.
            Dim objMatchItem As ListViewItem
            Dim objMatchTrx As NormalTrx
            Dim blnFoundNonImported As Boolean = False
            Dim intNumberVisible As Integer = 0
            For Each objMatchItem In lvwMatches.Items
                objMatchTrx = maudtMatch(CInt(objMatchItem.SubItems(mintMCHCOL_INDEX).Text))
                If objMatchTrx.strImportKey = "" Then
                    If Not blnFoundNonImported Then
                        objMatchItem.Selected = True
                        blnFoundNonImported = True
                    End If
                End If
                If blnFoundNonImported Then
                    objMatchItem.EnsureVisible()
                    intNumberVisible = intNumberVisible + 1
                    If intNumberVisible >= 4 Then
                        Exit For
                    End If
                End If
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    'Look for an existing transaction that matches the specified import item.
    'This sets the import status of the import item.

    Private Function blnMatchImport(ByVal intItemIndex As Integer) As Boolean
        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim objImportMatch As NormalTrx
        Dim lngNumber As Integer

        Try

            'This is the import item they selected.
            objImportedTrx = maudtItem(intItemIndex).objImportedTrx
            If IsNumeric(objImportedTrx.strNumber) Then
                lngNumber = CInt(objImportedTrx.strNumber)
            Else
                lngNumber = 0
            End If

            'Look for an import match in ALL registers, not just the selected register.
            'If found, update maudtItem() and redisplay it with the match info.
            For Each objReg In mobjAccount.colRegisters
                objImportMatch = mobjImportHandler.objStatusSearch(objImportedTrx, objReg)
                If Not objImportMatch Is Nothing Then
                    maudtItem(intItemIndex).lngStatus = ImportStatus.mlngIMPSTS_PRIOR
                    maudtItem(intItemIndex).objReg = objReg
                    blnMatchImport = True
                    Exit Function
                End If
            Next objReg

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub ClearLvwSelection(ByRef lvw As System.Windows.Forms.ListView)
        Dim objItem As System.Windows.Forms.ListViewItem

        'UPGRADE_NOTE: Object lvw.SelectedItem may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        lvw.FocusedItem = Nothing
        For Each objItem In lvw.Items
            If objItem.Selected Then
                objItem.Selected = False
            End If
        Next objItem
    End Sub

    Private Sub ClearCurrentItemMatches()
        lvwMatches.Items.Clear()
        mintMatches = 0
        Erase maudtMatch
    End Sub

    Private Sub DisplayMatch(ByVal objTrx As NormalTrx, ByVal intIndex As Integer)

        Dim objItem As ListViewItem = UITools.ListViewAdd(lvwMatches)
        Dim strFake As String = ""
        Dim strGen As String = ""
        Dim strImport As String = ""
        If objTrx.blnFake Then
            strFake = "Y"
        End If
        If objTrx.blnAutoGenerated Then
            strGen = "Y"
        End If
        If objTrx.strImportKey <> "" Then
            strImport = "Y"
        End If
        With objItem
            .Text = Utilities.strFormatDate(objTrx.datDate)
            .SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
            .SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
            .SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, Utilities.strFormatCurrency(objTrx.curAmount)))
            .SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, strSummarizeTrxCat(objTrx)))
            .SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, objTrx.strSummarizeDueDate()))
            .SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, strFake))
            .SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, strGen))
            .SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, strImport))
            .SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, objTrx.objReg.strTitle))
            .SubItems.Insert(mintMCHCOL_INDEX, New ListViewItem.ListViewSubItem(Nothing, CStr(intIndex)))
        End With

    End Sub

    Private Function strSummarizeTrxCat(ByVal objTrx As NormalTrx) As String

        If objTrx.lngSplits = 1 Then
            strSummarizeTrxCat = mobjCompany.objCategories.strKeyToValue1(objTrx.objFirstSplit.strCategoryKey)
        Else
            strSummarizeTrxCat = "(split)"
        End If

    End Function

    Private Sub cmdCreateNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCreateNew.Click
        Dim datDummy As Date

        Try

            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If mobjSelectedRegister Is Nothing Then
                mobjHostUI.ErrorMessageBox("First select the register to create the transaction in.")
                Exit Sub
            End If

            With maudtItem(intSelectedItemIndex())
                .objImportedTrx.objReg = mobjSelectedRegister
                If mobjHostUI.blnAddNormalTrx(.objImportedTrx, datDummy, True, "Import.CreateNew") Then
                    Exit Sub
                End If
                .lngStatus = ImportStatus.mlngIMPSTS_NEW
                .objReg = mobjSelectedRegister
            End With
            RedisplaySelectedItem()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lvwTrx_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwTrx.DoubleClick
        Dim datDummy As Date

        Try

            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If mobjSelectedRegister Is Nothing Then
                mobjHostUI.ErrorMessageBox("First select the register to create the transaction in.")
                Exit Sub
            End If

            With maudtItem(intSelectedItemIndex())
                If MsgBox("Create transaction " & strDescribeTrx(.objImportedTrx) & "?", MsgBoxStyle.OkCancel, "Create Transaction") <> MsgBoxResult.Ok Then
                    Exit Sub
                End If
                .objImportedTrx.objReg = mobjSelectedRegister
                If mobjHostUI.blnAddNormalTrxSilent(.objImportedTrx, datDummy, True, "ImportNewSilent") Then
                    Exit Sub
                End If
                .lngStatus = ImportStatus.mlngIMPSTS_NEW
                .objReg = mobjSelectedRegister
            End With
            RedisplaySelectedItem()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdUpdateExisting_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUpdateExisting.Click
        Dim objMatchedTrx As NormalTrx

        Try
            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If lvwMatches.FocusedItem Is Nothing Then
                mobjHostUI.ErrorMessageBox("First select the matched transaction to update.")
                Exit Sub
            End If

            objMatchedTrx = maudtMatch(intSelectedMatchIndex())
            If objMatchedTrx.strImportKey <> "" Then
                mobjHostUI.ErrorMessageBox("You may not update a transaction that has already been imported.")
                Exit Sub
            End If
            With maudtItem(intSelectedItemIndex())
                If Not objMatchedTrx Is Nothing Then
                    If mobjImportHandler.blnIndividualUpdate(.objImportedTrx, objMatchedTrx) Then
                        .lngStatus = ImportStatus.mlngIMPSTS_UPDATE
                        .objReg = objMatchedTrx.objReg
                    End If
                End If
            End With
            RedisplaySelectedItem()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidImportItemSelected() As Boolean
        If lvwTrx.FocusedItem Is Nothing Then
            mobjHostUI.ErrorMessageBox("First select the import item in the top list.")
            Exit Function
        End If
        Select Case maudtItem(intSelectedItemIndex()).lngStatus
            Case ImportStatus.mlngIMPSTS_UNRESOLVED
            Case ImportStatus.mlngIMPSTS_NEW, ImportStatus.mlngIMPSTS_UPDATE
                mobjHostUI.ErrorMessageBox("This import item has already been processed.")
                Exit Function
            Case ImportStatus.mlngIMPSTS_PRIOR
                If MsgBox("This item exactly matches an item imported in " & "a previous import session. Do you wish to import it anyway?",
                          MsgBoxStyle.Question Or MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
                    Exit Function
                End If
            Case Else
                gRaiseError("Unexpected import status in blnValidImportItemSelected")
        End Select
        blnValidImportItemSelected = True
    End Function

    Private Sub RedisplaySelectedItem()
        DisplayOneImportItem(lvwTrx.FocusedItem, intSelectedItemIndex())
    End Sub

    Private Function intSelectedItemIndex() As Integer
        intSelectedItemIndex = CInt(lvwTrx.FocusedItem.SubItems(mintITMCOL_INDEX).Text)
    End Function

    Private Function intSelectedMatchIndex() As Integer
        intSelectedMatchIndex = CInt(lvwMatches.FocusedItem.SubItems(mintMCHCOL_INDEX).Text)
    End Function

    Private Sub cboRegister_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboRegister.SelectedIndexChanged
        Try

            With cboRegister
                If .SelectedIndex < 0 Then
                    'UPGRADE_NOTE: Object mobjSelectedRegister may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    mobjSelectedRegister = Nothing
                Else
                    mobjSelectedRegister = mobjAccount.colRegisters.Item(UITools.GetItemData(cboRegister, .SelectedIndex))
                End If
            End With

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjAccount_ChangeMade() Handles mobjAccount.ChangeMade
        Try
            'Because MatchItem.lngRegIndex may have changed for any matches.
            'Also, this clears the list after "Create New" or "Update Existing".
            ClearCurrentItemMatches()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function strDescribeItem(ByRef intItemIndex As Integer) As String
        strDescribeItem = strDescribeTrx(maudtItem(intItemIndex).objImportedTrx)
    End Function

    Private Function strDescribeTrx(ByRef objTrx As ImportedTrx) As String
        strDescribeTrx = "[ " & Utilities.strFormatDate(objTrx.datDate) & " " & objTrx.strDescription & " $" & Utilities.strFormatCurrency(objTrx.curAmount) & " ]"
    End Function

    Private Sub lvwTrx_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvwTrx.ItemChecked
        If Not mblnIgnoreItemCheckedEvents Then
            If e.Item.SubItems.Count >= mintITMCOL_INDEX Then
                Dim intItemIndex As Integer = CInt(e.Item.SubItems(mintITMCOL_INDEX).Text)
                Dim objMultiPart As MultiPartMatch = maudtItem(intItemIndex).objMultiPart
                If Not objMultiPart Is Nothing Then
                    SetCheckStateOnAllMultiParts(objMultiPart, e.Item.Checked)
                End If
            End If
        End If
    End Sub

    Private Sub SetCheckStateOnAllMultiParts(ByVal objMultiPart As MultiPartMatch, ByVal blnChecked As Boolean)
        For Each objItem As ListViewItem In lvwTrx.Items
            Dim objImportItem As ImportItem = maudtItem(CInt(objItem.SubItems(mintITMCOL_INDEX).Text))
            If objImportItem.objMultiPart Is objMultiPart Then
                ChangeItemCheckedSilently(objItem, blnChecked)
            End If
        Next
    End Sub

    Private Sub ChangeItemCheckedSilently(ByVal objItem As ListViewItem, ByVal blnChecked As Boolean)
        Dim blnSaveFlag As Boolean = mblnIgnoreItemCheckedEvents
        mblnIgnoreItemCheckedEvents = True
        objItem.Checked = blnChecked
        mblnIgnoreItemCheckedEvents = blnSaveFlag
    End Sub

    Private Sub txtDescriptionFilter_GotFocus(sender As Object, e As EventArgs) Handles txtDescriptionFilter.GotFocus
        mblnTrapKeyPress = False
    End Sub

    Private Sub txtDescriptionFilter_LostFocus(sender As Object, e As EventArgs) Handles txtDescriptionFilter.LostFocus
        mblnTrapKeyPress = True
    End Sub

    Private Sub cmdSelectAll_Click(sender As Object, e As EventArgs) Handles cmdSelectAll.Click
        Dim itm As ListViewItem
        For Each itm In lvwTrx.Items
            itm.Checked = Not itm.Checked
        Next
    End Sub

End Class