Option Strict On
Option Explicit On

''' <summary>
''' Represents one transaction. Subclassed for different kinds of transactions.
'''
''' To create a transaction and add it to a Register, create a Trx subclass instance
''' and call NewStartNormal(), NewStartBudget() or NewStartTransfer() depending on the
''' type of transaction you want to create. If you called NewStartNormal(), add at
''' least one split with AddSplit(). Then call Register.NewLoadEnd() if you are in
''' the process of loading a Register from some external source and will note the
''' end of the load by calling Register.LoadPostProcessing(). Otherwise call
''' Register.NewAddEnd(), which is what will typically happen if you are adding to
''' a Register which is already visible in the UI.
''' To update an existing transaction, call UpdateStartNormal(),
''' UpdateStartBudget() or UpdateStartTransfer() as appropriate for the transaction
''' type. Then call AddSplit() at least once if you called UpdateStartNormal().
''' Then call Register.UpdateEnd().
'''
''' The UI is not updated directly by the caller to these methods, rather it is
''' updated indirectly by responding to events fired by the Register object as
''' its methods are called and complete their tasks.
''' </summary>

Public MustInherit Class Trx

    Public Enum TrxStatus
        'Missing value (should never be this).
        gintTRXSTS_MISSING = 0
        'Unreconciled (new).
        glngTRXSTS_UNREC = 1
        'Reconciled.
        glngTRXSTS_RECON = 2
        'Non-bank (budget, transfer, etc.)
        glngTRXSTS_NONBANK = 3
        'Selected in current reconciliation.
        glngTRXSTS_SELECTED = 4
    End Enum

    Public Enum TrxType
        'Missing value (should never be this).
        glngTRXTYP_MISSING = 0
        'Ordinary transaction.
        glngTRXTYP_NORMAL = 1
        'Budget transaction (must be fake).
        glngTRXTYP_BUDGET = 2
        'Transfer transaction.
        glngTRXTYP_TRANSFER = 3
        'Replica transaction.
        glngTRXTYP_REPLICA = 4
    End Enum

    Public Enum TrxSearchField
        glngTRXSFL_NUMBER
        glngTRXSFL_DESCR
        glngTRXSFL_CATKEY
        glngTRXSFL_AMOUNT
        glngTRXSFL_MEMO
        glngTRXSFL_INVNUM
        glngTRXSFL_PONUMBER
    End Enum

    Public Enum TrxSearchType
        glngTRXSTP_EQUAL = 1
        glngTRXSTP_CONTAINS = 2
        glngTRXSTP_STARTS = 3
    End Enum

    Public Enum RepeatUnit
        glngRPTUNT_MISSING = 0
        glngRPTUNT_DAY = 1
        glngRPTUNT_WEEK = 2
        glngRPTUNT_MONTH = 3
    End Enum

    'Register this Trx belongs to.
    Protected mobjReg As Register
    'Index of this Trx in mobjReg.objTrx().
    Protected mlngIndex As Integer
    'Transaction number.
    Protected mstrNumber As String
    'Transaction date.
    Protected mdatDate As Date
    'Payee name, or other description.
    Protected mstrDescription As String
    'Transaction memo.
    Protected mstrMemo As String
    'Transaction status.
    Protected mlngStatus As TrxStatus
    'Is this transaction fake (future dated)?
    Protected mblnFake As Boolean
    'Trx needs to be reviewed by the operator.
    Protected mblnAwaitingReview As Boolean
    'Trx was added to register as part of an automatically generated
    'series. Such a Trx will always have a non-empty mstrRepeatKey,
    'but a non-empty mstrRepeatKey does not imply mblnAutoGenerated=True.
    Protected mblnAutoGenerated As Boolean
    'Sequence number of Trx in generated sequence.
    Protected mintRepeatSeq As Integer
    'All fake and real Trx originating from the same generated sequence
    'have the same value.
    Protected mstrRepeatKey As String
    'Computed from the sum of the Split amounts for normal trx.
    'Set directly for other trx types.
    Protected mcurAmount As Decimal
    'Register balance after mcurAmount added.
    Protected mcurBalance As Decimal
    'Key for sorting register entries.
    Protected mstrSortKey As String

    Public Sub New(ByVal objReg_ As Register)
        mobjReg = objReg_
    End Sub

    Public ReadOnly Property objReg() As Register
        Get
            Return mobjReg
        End Get
    End Property

    Public Sub ClearRepeatTrx()
        If mintRepeatSeq > 0 Then
            'Remove the repeat trx index entry for the old values
            'of repeat key and repeat seq, which may be different
            'than the new ones.
            mobjReg.RemoveRepeatTrx(Me)
        End If
    End Sub

    Public Property lngIndex() As Integer
        Set(value As Integer)
            mlngIndex = value
        End Set
        Get
            Return mlngIndex
        End Get
    End Property

    Public ReadOnly Property objNext() As Trx
        Get
            If mlngIndex >= mobjReg.lngTrxCount Then
                Return Nothing
            Else
                Return mobjReg.objTrx(mlngIndex + 1)
            End If
        End Get
    End Property

    Public ReadOnly Property objPrevious() As Trx
        Get
            If mlngIndex <= 1 Then
                Return Nothing
            Else
                Return mobjReg.objTrx(mlngIndex + 1)
            End If
        End Get
    End Property

    Public ReadOnly Property strRepeatKey() As String
        Get
            strRepeatKey = mstrRepeatKey
        End Get
    End Property

    Public ReadOnly Property intRepeatSeq() As Integer
        Get
            intRepeatSeq = mintRepeatSeq
        End Get
    End Property

    Public ReadOnly Property strRepeatId() As String
        Get
            strRepeatId = strMakeRepeatId(mstrRepeatKey, mintRepeatSeq)
        End Get
    End Property

    Public Shared Function strMakeRepeatId(ByVal strRepeatKey As String, ByVal intRepeatSeq As Integer) As String
        Return "#" & strRepeatKey & "." & intRepeatSeq
    End Function

    Public Sub ClearRepeat()
        mintRepeatSeq = 0
        mstrRepeatKey = ""
    End Sub

    Public ReadOnly Property strSortKey() As String
        Get
            strSortKey = mstrSortKey
        End Get
    End Property

    Public MustOverride ReadOnly Property lngType() As TrxType

    Public ReadOnly Property strNumber() As String
        Get
            strNumber = mstrNumber
        End Get
    End Property

    Public Property datDate() As Date
        Get
            datDate = mdatDate
        End Get
        Set(value As Date)
            mdatDate = value
        End Set
    End Property

    Public ReadOnly Property strDescription() As String
        Get
            strDescription = mstrDescription
        End Get
    End Property

    Public MustOverride ReadOnly Property strCategory() As String

    Public ReadOnly Property strMemo() As String
        Get
            strMemo = mstrMemo
        End Get
    End Property

    Public ReadOnly Property lngStatus() As TrxStatus
        Get
            lngStatus = mlngStatus
        End Get
    End Property

    Public ReadOnly Property strStatus() As String
        Get
            Select Case mlngStatus
                Case TrxStatus.glngTRXSTS_NONBANK
                    strStatus = "NonBank"
                Case TrxStatus.glngTRXSTS_UNREC
                    strStatus = "Unreconciled"
                Case TrxStatus.glngTRXSTS_SELECTED
                    strStatus = "Selected"
                Case TrxStatus.glngTRXSTS_RECON
                    strStatus = "Reconciled"
                Case Else
                    strStatus = ""
            End Select
        End Get
    End Property

    Public Property blnFake() As Boolean
        Get
            blnFake = mblnFake
        End Get
        Set(ByVal Value As Boolean)
            mblnFake = Value
        End Set
    End Property

    Public ReadOnly Property strFakeStatus() As String
        Get
            If mblnAutoGenerated Then
                strFakeStatus = "Gen"
            ElseIf mblnFake Then
                strFakeStatus = "Fake"
            Else
                strFakeStatus = ""
            End If
        End Get
    End Property

    Public ReadOnly Property blnAwaitingReview() As Boolean
        Get
            blnAwaitingReview = mblnAwaitingReview
        End Get
    End Property

    Public ReadOnly Property blnAutoGenerated() As Boolean
        Get
            blnAutoGenerated = mblnAutoGenerated
        End Get
    End Property

    Public ReadOnly Property curAmount() As Decimal
        Get
            curAmount = mcurAmount
        End Get
    End Property

    Public ReadOnly Property curBalance() As Decimal
        Get
            curBalance = mcurBalance
        End Get
    End Property

    Protected Sub RaiseErrorOnBadData(ByVal strRoutine As String)
        If mdatDate = System.DateTime.FromOADate(0) Then
            gRaiseError("Missing date in " & strRoutine)
        End If
        If mstrDescription = "" Then
            gRaiseError("Missing description in " & strRoutine)
        End If
        If mlngStatus = TrxStatus.gintTRXSTS_MISSING Then
            gRaiseError("Missing status in " & strRoutine)
        End If
    End Sub

    Public Sub SetSortKey()
        Dim strInvNum As String = ""
        If lngType = TrxType.glngTRXTYP_NORMAL Then
            strInvNum = DirectCast(Me, NormalTrx).objFirstSplit.strInvoiceNum
        End If
        Dim strDebitCredit As String
        If mcurAmount > 0 Then
            strDebitCredit = "C"
        Else
            strDebitCredit = "D"
        End If
        mstrSortKey = mdatDate.ToString("yyyyMMdd") & strDebitCredit & Mid("ZYX", lngType + 1, 1) &
            Left(mstrNumber & "          ", 10) & Left(mstrDescription & "                    ", 20) & Left(strInvNum & "                ", 16)
    End Sub

    '$Description Used only by Register.SetTrxStatus().

    Public Sub SetStatus(ByVal lngNewStatus As TrxStatus)
        mlngStatus = lngNewStatus
    End Sub

    Public Sub SetAmount(ByVal curNewAmount As Decimal)
        mcurAmount = curNewAmount
    End Sub

    '$Description Called only by Register.lngFixBalances().

    Public Sub SetBalance(ByVal curNewBal As Decimal)
        mcurBalance = curNewBal
    End Sub

    Public Sub Delete(ByVal objDeleteLogger As ILogDelete, ByVal strLogTitle As String,
                      Optional ByVal blnSetChanged As Boolean = True)
        mobjReg.Delete(mlngIndex, objDeleteLogger, strLogTitle, blnSetChanged)
    End Sub

    Public MustOverride Sub UnApply()
    Public MustOverride Sub Apply(ByVal blnLoading As Boolean)

    Public Delegate Sub AddSearchMaxTrxDelegate(ByVal objTrx As Trx)
    Public Delegate Sub AddSearchMaxSplitDelegate(ByVal objTrx As Trx, ByVal objSplit As TrxSplit)

    '$Description Determine if this Trx is a match to search criteria.
    '$Param lngSearchField Which Trx data to search.
    '$Param strSearchFor What you are searching for. For category searches
    '   is the category key, not the text, and the search succeeds if any
    '   Split for the Trx has this category key.
    '$Param lngSearchType What kind of comparison to do.

    Public Sub CheckSearchMatch(
        ByVal objCompany As Company,
        ByVal lngSearchField As TrxSearchField, ByVal strSearchFor As String,
        ByVal lngSearchType As TrxSearchType,
        ByVal dlgAddTrxResult As AddSearchMaxTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMaxSplitDelegate)

        Dim objSplit As TrxSplit
        Dim strTrxData As String = ""
        Dim strCatName As String

        Select Case lngSearchField
            Case TrxSearchField.glngTRXSFL_CATKEY
                If lngType = TrxType.glngTRXTYP_NORMAL Then
                    For Each objSplit In DirectCast(Me, NormalTrx).colSplits
                        If lngSearchType = TrxSearchType.glngTRXSTP_EQUAL Then
                            If objSplit.strCategoryKey = strSearchFor Then
                                dlgAddSplitResult(DirectCast(Me, NormalTrx), objSplit)
                            End If
                        Else
                            strCatName = objCompany.objCategories.strKeyToValue1(objSplit.strCategoryKey)
                            If (Left(strCatName, Len(strSearchFor) + 1) = (strSearchFor & ":")) Or (strCatName = strSearchFor) Then
                                dlgAddSplitResult(DirectCast(Me, NormalTrx), objSplit)
                            End If
                        End If
                    Next
                End If
                Exit Sub
            Case TrxSearchField.glngTRXSFL_INVNUM
                If lngType = TrxType.glngTRXTYP_NORMAL Then
                    For Each objSplit In DirectCast(Me, NormalTrx).colSplits
                        If blnIsStringMatch(lngSearchType, (objSplit.strInvoiceNum), strSearchFor) Then
                            dlgAddSplitResult(DirectCast(Me, NormalTrx), objSplit)
                        End If
                    Next
                End If
                Exit Sub
            Case TrxSearchField.glngTRXSFL_PONUMBER
                If lngType = TrxType.glngTRXTYP_NORMAL Then
                    For Each objSplit In DirectCast(Me, NormalTrx).colSplits
                        If blnIsStringMatch(lngSearchType, (objSplit.strPONumber), strSearchFor) Then
                            dlgAddSplitResult(DirectCast(Me, NormalTrx), objSplit)
                        End If
                    Next
                End If
                Exit Sub
            Case TrxSearchField.glngTRXSFL_DESCR
                strTrxData = mstrDescription
            Case TrxSearchField.glngTRXSFL_NUMBER
                strTrxData = mstrNumber
            Case TrxSearchField.glngTRXSFL_AMOUNT
                strTrxData = Utilities.strFormatCurrency(mcurAmount)
            Case TrxSearchField.glngTRXSFL_MEMO
                strTrxData = mstrMemo
            Case Else
                gRaiseError("Unrecognized field in Trx.blnIsSearchMatch")
        End Select

        'All the searches that check and report individual splits report their 
        'results and exit BEFORE here.
        If blnIsStringMatch(lngSearchType, strTrxData, strSearchFor) Then
            dlgAddTrxResult(Me)
        End If

    End Sub

    Private Function blnIsStringMatch(ByVal lngSearchType As TrxSearchType, ByRef strTrxData As String, ByRef strSearchFor As String) As Boolean

        Select Case lngSearchType
            Case TrxSearchType.glngTRXSTP_EQUAL
                blnIsStringMatch = (StrComp(strTrxData, strSearchFor, CompareMethod.Text) = 0)
            Case TrxSearchType.glngTRXSTP_STARTS
                blnIsStringMatch = (StrComp(Left(strTrxData, Len(strSearchFor)), strSearchFor, CompareMethod.Text) = 0)
            Case TrxSearchType.glngTRXSTP_CONTAINS
                blnIsStringMatch = (InStr(1, strTrxData, strSearchFor, CompareMethod.Text) > 0)
            Case Else
                gRaiseError("Unrecognized search type in Trx.blnIsStringMatch")
        End Select

    End Function

    '$Description Check for validation errors for Register.Validate().

    Public Overridable Sub Validate()
        Dim objRepeatTrx As Trx
        If Not mobjReg.objTrx(mlngIndex) Is Me Then
            mobjReg.RaiseValidationError(mlngIndex, "lngIndex is wrong")
        End If
        If mstrSortKey = "" Then
            mobjReg.RaiseValidationError(mlngIndex, "Missing sort key")
        End If
        If mdatDate = System.DateTime.FromOADate(0) Then
            mobjReg.RaiseValidationError(mlngIndex, "Missing date")
        End If
        If mstrRepeatKey <> "" Then
            If mintRepeatSeq = 0 Then
                mobjReg.RaiseValidationError(mlngIndex, "Repeat key has no repeat seq")
            End If
            objRepeatTrx = objReg.objRepeatTrx(mstrRepeatKey, mintRepeatSeq)
            If Not objRepeatTrx Is Me Then
                mobjReg.RaiseValidationError(mlngIndex, "objRepeatTrx() returned wrong Trx")
            End If
        Else
            If mintRepeatSeq <> 0 Then
                mobjReg.RaiseValidationError(mlngIndex, "Repeat seq should be zero")
            End If
        End If
    End Sub

    Public MustOverride Function objClone(ByVal blnWillAddToRegister As Boolean) As Trx

    Public Overrides Function ToString() As String
        Return Me.datDate.ToShortDateString() + " " + Me.strDescription + " " + Me.curAmount.ToString()
    End Function
End Class