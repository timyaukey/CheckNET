Option Strict On
Option Explicit On


Public Class ImportUtilities

    'Helper class for importing transaction information from a bank download file.
    'The caller must parse the information, this class only receives the information
    'and makes BaseTrx objects from it.

    Private mobjCompany As Company
    Private mobjAccount As Account
    Private mblnMakeFakeTrx As Boolean
    Private mblnNoImportKey As Boolean

    'Data saved from current transaction in input file.
    Private mstrTrxDate As String
    Private mstrTrxPayee As String 'This may be modified
    Private mstrTrxPayeeTrimmed As String
    Private mstrTrxAmount As String
    Private mstrTrxMemo As String
    Private mstrTrxNumber As String 'This may be modified
    Private mstrTrxUniqueKey As String

    'Information computed from payee name.
    'Category to use.
    Private mstrTrxCategory As String
    'Budget to use.
    Private mstrTrxBudget As String
    'Narrowing method to use.
    Private mlngNarrowMethod As ImportMatchNarrowMethod
    'Amount range for matching.
    Private mcurMatchMin As Decimal
    Private mcurMatchMax As Decimal
    Private mblnAllowAutoBatchNew As Boolean
    Private mblnAllowAutoBatchUpdate As Boolean

    'Table with trx type translation information.
    Private mdomTrxTypes As VB6XmlDocument

    Public Sub Init(ByVal objAccount_ As Account)
        mobjAccount = objAccount_
        mobjCompany = mobjAccount.Company
    End Sub

    Public Sub LoadTrxTypeTable()
        Try

            Dim strTableFile As String

            strTableFile = mobjCompany.TrxTypeFilePath()
            mdomTrxTypes = mobjCompany.LoadXmlFile(strTableFile)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public WriteOnly Property blnMakeFakeTrx() As Boolean
        Set(ByVal Value As Boolean)
            mblnMakeFakeTrx = Value
        End Set
    End Property

    Public WriteOnly Property blnNoImportKey() As Boolean
        Set(value As Boolean)
            mblnNoImportKey = value
        End Set
    End Property

    Public WriteOnly Property strTrxDate() As String
        Set(ByVal Value As String)
            mstrTrxDate = Value
        End Set
    End Property

    Public WriteOnly Property strTrxPayee() As String
        Set(ByVal Value As String)
            mstrTrxPayee = Value
        End Set
    End Property

    Public WriteOnly Property strTrxAmount() As String
        Set(ByVal Value As String)
            mstrTrxAmount = Value
        End Set
    End Property

    Public WriteOnly Property strTrxMemo() As String
        Set(ByVal Value As String)
            mstrTrxMemo = Value
        End Set
    End Property

    Public WriteOnly Property strTrxNumber() As String
        Set(ByVal Value As String)
            mstrTrxNumber = Value
        End Set
    End Property

    Public WriteOnly Property strTrxUniqueKey() As String
        Set(ByVal Value As String)
            mstrTrxUniqueKey = Value
        End Set
    End Property

    '$Description Clear saved information about the current transaction.

    Public Sub ClearSavedTrxData()
        mstrTrxDate = ""
        mstrTrxPayee = "Unknown payee"
        mstrTrxAmount = ""
        mstrTrxMemo = ""
        mstrTrxNumber = ""
        mstrTrxUniqueKey = ""
        mstrTrxCategory = ""
        mstrTrxBudget = ""
        mlngNarrowMethod = ImportMatchNarrowMethod.None
        mcurMatchMin = 0
        mcurMatchMax = 0
        mblnAllowAutoBatchNew = False
        mblnAllowAutoBatchUpdate = False
    End Sub

    ''' <summary>
    ''' Create a new BaseTrx object.
    ''' This is where the payee name is translated using the memorized transaction list.
    ''' </summary>
    ''' <returns></returns>
    Public Function objMakeTrx() As ImportedTrx
        Dim objTrx As ImportedTrx
        Dim datDate As Date
        Dim strImportKey As String
        Dim strCatKey As String = ""
        Dim intCatIndex As Integer
        Dim strBudKey As String = ""
        Dim intBudIndex As Integer
        Dim strDescription As String = ""
        Dim strMemo As String = ""
        Dim strNumber As String = ""
        Dim strSplitPONumber As String = ""
        Dim strSplitInvoiceNum As String = ""
        Dim datSplitInvoiceDate As Date
        Dim datSplitDueDate As Date
        Dim strSplitTerms As String = ""
        Dim curAmount As Decimal
        Dim strUniqueKey As String = ""

        objMakeTrx = Nothing
        Try

            ConvertTransaction()
            datDate = CDate(mstrTrxDate)
            strNumber = mstrTrxNumber
            strDescription = mstrTrxPayee
            curAmount = CDec(mstrTrxAmount)
            strMemo = Trim(mstrTrxMemo)
            datSplitInvoiceDate = Utilities.datEmpty
            datSplitDueDate = Utilities.datEmpty
            With mobjCompany.Categories
                intCatIndex = .intLookupValue1(mstrTrxCategory)
                If intCatIndex > 0 Then
                    strCatKey = .strKey(intCatIndex)
                End If
            End With
            With mobjCompany.Budgets
                intBudIndex = .intLookupValue1(mstrTrxBudget)
                If intBudIndex > 0 Then
                    strBudKey = .strKey(intBudIndex)
                End If
            End With
            If mblnMakeFakeTrx Or mblnNoImportKey Then
                strImportKey = ""
            Else
                If mstrTrxUniqueKey <> "" Then
                    strUniqueKey = mstrTrxUniqueKey
                Else
                    'If the ITrxReader did not supply a unique key then we construct one.
                    'Have to use left part of mstrTrxPayeeTrimmed instead of the entire
                    'thing because FSBLink sometimes
                    'formats the payee name with different numbers of imbedded blanks in its
                    '"source prefixes", and this causes the payee name to be truncated in
                    'variable ways because there is a fixed max length on the payee name
                    'in the download file. I believe the actual safe characters to use is
                    '20, based on the max blanks it inserts, but 16 is nice and pessimistic
                    'and ought to be enough to recognize the payee.
                    strUniqueKey = strNumber & "|" & Left(mstrTrxPayeeTrimmed, 16) & "|" & Utilities.strFormatCurrency(curAmount)
                End If
                'NOTE: Date must stay in the same place, because statement reconciliation
                'parses it out to show.
                strImportKey = strSqueezeInput("|" & Utilities.strFormatDate(datDate) & "|" & strUniqueKey)
            End If

            objTrx = New ImportedTrx(Nothing)
            objTrx.NewStartNormal(False, strNumber, datDate, strDescription, strMemo, BaseTrx.TrxStatus.Unreconciled,
                                  mblnMakeFakeTrx, 0.0D, False, False, 0, strImportKey, "")
            objTrx.lngNarrowMethod = mlngNarrowMethod
            objTrx.curMatchMin = mcurMatchMin
            objTrx.curMatchMax = mcurMatchMax
            objTrx.blnAllowAutoBatchNew = mblnAllowAutoBatchNew
            objTrx.blnAllowAutoBatchUpdate = mblnAllowAutoBatchUpdate
            objTrx.AddSplit("", strCatKey, strSplitPONumber, strSplitInvoiceNum, datSplitInvoiceDate, datSplitDueDate, strSplitTerms, strBudKey, curAmount)
            objMakeTrx = objTrx

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Analyze the payee to determine the type of transaction.
    '   Massages other data based on the transaction type.

    Private Sub ConvertTransaction()
        Try

            Dim objTrxTypes As VB6XmlNodeList
            Dim elmTrxType As VB6XmlElement
            Dim strSqueezedInput As String
            Dim strNormalizedInput As String
            Dim blnFailMatch As Boolean
            Dim vstrBefore As String
            Dim vstrAfter As String
            Dim vstrNumber As String
            Dim vintMinAfter As Integer
            Dim blnMatchAfter As Boolean
            Dim intAfterLen As Integer
            Dim strPayeeTrimmed As String

            strSqueezedInput = strSqueezeInput(mstrTrxPayee)
            strNormalizedInput = LCase(strSqueezedInput)
            mstrTrxPayeeTrimmed = strSqueezedInput
            'Check each <TrxType> element, to decide if one matches the input payee.
            'If so, then the real payee is actually embedded inside some decoration
            'that tells us what type of transaction it is.
            objTrxTypes = mdomTrxTypes.DocumentElement.SelectNodes("TrxType")
            For Each elmTrxType In objTrxTypes
                'Decide if the Before= and After= match.
                'All data is normalized, to neutralize consecutive blank and case problems.
                'Do NOT use this trx type if "Before" or "After" attribute is specified,
                'and whichever is specified does not match the trx description.
                blnFailMatch = False
                'Check if starts with the ENTIRE  "Before" attribute.
                vstrBefore = CType(elmTrxType.GetAttribute("Before"), String)
                If Not gblnXmlAttributeMissing(vstrBefore) Then
                    vstrBefore = strNormalizeInput(vstrBefore)
                    If Left(strNormalizedInput, Len(vstrBefore)) <> vstrBefore Then
                        blnFailMatch = True
                    End If
                End If
                'Check if ends with at least "n" characters from the start of the "After" attrib.
                vstrAfter = CType(elmTrxType.GetAttribute("After"), String)
                If Not gblnXmlAttributeMissing(vstrAfter) Then
                    vintMinAfter = Integer.Parse(CType(elmTrxType.GetAttribute("MinAfter"), String))
                    If gblnXmlAttributeMissing(vintMinAfter) Then
                        vintMinAfter = 3
                    End If
                    vstrAfter = strNormalizeInput(vstrAfter)
                    blnMatchAfter = False
                    Do
                        intAfterLen = Len(vstrAfter)
                        If Right(strNormalizedInput, intAfterLen) = vstrAfter Then
                            blnMatchAfter = True
                            Exit Do
                        End If
                        If intAfterLen <= vintMinAfter Then
                            Exit Do
                        End If
                        vstrAfter = Left(vstrAfter, intAfterLen - 1)
                    Loop
                    If Not blnMatchAfter Then
                        blnFailMatch = True
                    End If
                End If
                'Sanity check.
                If gblnXmlAttributeMissing(vstrBefore) And gblnXmlAttributeMissing(vstrAfter) Then
                    gRaiseError("Neither Before= or After= specified for TrxType element")
                End If
                'We matched whichever of Before= and After= were specified.
                If Not blnFailMatch Then
                    'Now trim whatever part was matched, to get the real payee.
                    'We used the squeezed version, not the fully normalized, so
                    'we get the original case information.
                    strPayeeTrimmed = mstrTrxPayeeTrimmed
                    If Not gblnXmlAttributeMissing(vstrBefore) Then
                        strPayeeTrimmed = Mid(strPayeeTrimmed, Len(vstrBefore) + 1)
                    End If
                    If Not gblnXmlAttributeMissing(vstrAfter) Then
                        strPayeeTrimmed = Left(strPayeeTrimmed, Len(strPayeeTrimmed) - Len(vstrAfter))
                    End If
                    'Remove blanks here so we don't have to include them in the trx type specs.
                    strPayeeTrimmed = Trim(strPayeeTrimmed)
                    'In case we trimmed away too much.
                    If Len(strPayeeTrimmed) < 3 Then
                        strPayeeTrimmed = mstrTrxPayeeTrimmed
                    End If
                    'If Number=(number), then the trimmed input is really a check
                    'number and we perform a sanity check to insure it really is a number.
                    vstrNumber = CType(elmTrxType.GetAttribute("Number"), String)
                    If gblnXmlAttributeMissing(vstrNumber) Then
                        vstrNumber = ""
                    End If
                    If vstrNumber = "(number)" Then
                        If Not IsNumeric(strPayeeTrimmed) Then
                            blnFailMatch = True
                        End If
                    End If
                    'If the match was not rejected by the (number) test.
                    If Not blnFailMatch Then
                        'The match cannot be rejected after this point, so it is safe to set
                        'mstrTrxPayeeTrimmed to the value trimmed according to the matched TrxType.
                        mstrTrxPayeeTrimmed = strPayeeTrimmed
                        If vstrNumber = "(number)" Then
                            mstrTrxNumber = strPayeeTrimmed
                        Else
                            mstrTrxNumber = vstrNumber
                            LookupPayee(strPayeeTrimmed)
                        End If
                        Exit Sub
                    End If
                End If
            Next elmTrxType
            'We get here if the payee didn't match any TrxType element,
            'in which case we still want to lookup the payee.
            LookupPayee(mstrTrxPayeeTrimmed)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Squeeze input and fold to lower case.

    Private Function strNormalizeInput(ByVal strInput As String) As String
        strNormalizeInput = LCase(strSqueezeInput(strInput))
    End Function

    '$Description Reduce all consecutive blanks down to single blanks.

    Private Function strSqueezeInput(ByVal strInput As String) As String
        Dim strResult As String
        strResult = strInput
        While InStr(strResult, "  ") > 0
            strResult = Replace(strResult, "  ", " ")
        End While
        strSqueezeInput = strResult
    End Function

    '$Description Look up trimmed input payee name and translate it.
    '   Sets mstrTrxPayee, and if finds a payee match also sets mstrTrxCategory
    '   and mstrTrxBudget.

    Private Sub LookupPayee(ByVal strTrimmedPayee As String)
        Try
            Dim strOutputPayee As String
            Dim objPayees As List(Of PayeeItem)
            Dim objPayee As PayeeItem
            Dim blnMatch As Boolean
            Dim curAmount As Decimal
            Dim strInput As String
            Dim curMatchTmp As Decimal

            strOutputPayee = strTrimmedPayee
            objPayees = mobjCompany.MemorizedTrans.Payees
            For Each objPayee In objPayees
                strInput = UCase(Trim(objPayee.Input))
                If (strInput <> "") And (InStr(UCase(strTrimmedPayee), strInput) > 0) Then
                    If Not String.IsNullOrEmpty(objPayee.Min) Then
                        curAmount = CDec(mstrTrxAmount)
                        mcurMatchMin = CDec(objPayee.Min)
                        mcurMatchMax = CDec(objPayee.Max)
                        If mcurMatchMin > mcurMatchMax Then
                            curMatchTmp = mcurMatchMax
                            mcurMatchMax = mcurMatchMin
                            mcurMatchMin = curMatchTmp
                        End If
                        blnMatch = (curAmount >= mcurMatchMin) And (curAmount <= mcurMatchMax)
                    Else
                        blnMatch = True
                    End If
                    If blnMatch Then
                        strOutputPayee = objPayee.Output
                        If Not String.IsNullOrEmpty(objPayee.Cat) Then
                            mstrTrxCategory = objPayee.Cat
                        End If
                        If Not String.IsNullOrEmpty(objPayee.Budget) Then
                            mstrTrxBudget = objPayee.Budget
                        End If
                        If Not String.IsNullOrEmpty(objPayee.NarrowMethod) Then
                            Select Case objPayee.NarrowMethod.ToLower()
                                Case "none", ""
                                    mlngNarrowMethod = ImportMatchNarrowMethod.None
                                Case "earliest date"
                                    mlngNarrowMethod = ImportMatchNarrowMethod.EarliestDate
                                Case "closest date"
                                    mlngNarrowMethod = ImportMatchNarrowMethod.ClosestDate
                                Case Else
                                    gRaiseError("Unrecognized narrow method")
                            End Select
                        End If
                        mblnAllowAutoBatchNew = objPayee.blnIsAllowAutoBatchNew
                        mblnAllowAutoBatchUpdate = objPayee.blnIsAllowAutoBatchUpdate
                        If mstrTrxNumber = "" Then
                            If Not String.IsNullOrEmpty(objPayee.Num) Then
                                mstrTrxNumber = objPayee.Num
                            End If
                        End If
                        Exit For
                    End If
                End If
            Next objPayee
            mstrTrxPayee = strOutputPayee
            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    'Filter out trx that are already matched to something in maudtItem().
    Public Shared Function colRemoveAlreadyMatched(ByVal objReg As Register,
                                                   ByVal colInputMatches As ICollection(Of BankTrx),
                                                   ByVal colAllMatchedTrx As IEnumerable(Of BankTrx)) _
                                                   As ICollection(Of BankTrx)
        Dim colUnusedMatches As ICollection(Of BankTrx)
        Dim blnAlreadyMatched As Boolean
        Dim objPossibleMatchTrx As BankTrx
        Dim objMatchedTrx As BaseTrx

        colUnusedMatches = New List(Of BankTrx)
        For Each objPossibleMatchTrx In colInputMatches
            blnAlreadyMatched = False
            For Each objMatchedTrx In colAllMatchedTrx
                If objMatchedTrx Is objPossibleMatchTrx Then
                    blnAlreadyMatched = True
                    Exit For
                End If
            Next
            If Not blnAlreadyMatched Then
                colUnusedMatches.Add(objPossibleMatchTrx)
            End If
        Next
        Return colUnusedMatches
    End Function

    Public Shared Function colApplyNarrowMethod(ByVal objReg As Register,
                                                ByVal objTrx As ImportedTrx,
                                                ByVal colInputMatches As ICollection(Of BankTrx),
                                                ByRef blnExactMatch As Boolean) _
                                                As ICollection(Of BankTrx)
        Dim colResult As ICollection(Of BankTrx)
        Dim objPossibleMatchTrx As BankTrx
        Dim datTargetDate As Date
        Dim dblBestDistance As Double
        Dim dblCurrentDistance As Double
        Dim objBestMatch As BankTrx = Nothing
        Dim blnHaveFirstMatch As Boolean

        If colInputMatches.Count = 0 Then
            Return colInputMatches
        End If

        Select Case objTrx.lngNarrowMethod
            Case ImportMatchNarrowMethod.EarliestDate
                datTargetDate = #1/1/1980#
            Case ImportMatchNarrowMethod.ClosestDate
                datTargetDate = objTrx.TrxDate
            Case ImportMatchNarrowMethod.None
                Return colInputMatches
            Case Else
                gRaiseError("Unrecognized narrowing method")
        End Select

        blnHaveFirstMatch = False
        For Each objPossibleMatchTrx In colInputMatches
            If String.IsNullOrEmpty(objPossibleMatchTrx.ImportKey) And (objPossibleMatchTrx.Status <> BaseTrx.TrxStatus.Reconciled) Then
                dblCurrentDistance = Math.Abs(objPossibleMatchTrx.TrxDate.Subtract(datTargetDate).TotalDays)
                If (Not blnHaveFirstMatch) Or (dblCurrentDistance < dblBestDistance) Then
                    dblBestDistance = dblCurrentDistance
                    objBestMatch = objPossibleMatchTrx
                    blnHaveFirstMatch = True
                End If
            End If
        Next
        blnExactMatch = True
        colResult = New List(Of BankTrx)
        If Not objBestMatch Is Nothing Then
            colResult.Add(objBestMatch)
        End If
        Return colResult

    End Function
End Class
