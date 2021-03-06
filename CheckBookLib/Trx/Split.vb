Option Strict On
Option Explicit On

''' <summary>
''' Represents an amount of money associated with a NormalTrx, plus
''' attributes such as due date, category, invoice number, memo, etc.
''' </summary>

Public Class TrxSplit

    'Memo for this Split.
    Private mstrMemo As String
    'Unique key for the category of this split.
    'This is not the name of the category, but the internal primary key.
    Private mstrCategoryKey As String
    'The purchase order number of this split.
    Private mstrPONumber As String
    'Invoice number of this split.
    Private mstrInvoiceNum As String
    'Invoice date of this split.
    Private mdatInvoiceDate As Date
    'Due date of this split.
    Private mdatDueDate As Date
    'Payment terms of this split.
    Private mstrTerms As String
    'Trx.strBudgetKey of budget Trx to apply this split toward.
    Private mstrBudgetKey As String
    'Amount of this Split.
    Private mcurAmount As Decimal

    'Parent NormalTrx for this TrxSplit.
    Private mobjParent As NormalTrx
    'Possible reference to BudgetTrx for this TrxSplit.
    Private mobjBudget As BudgetTrx
    'Possible reference to ReplicaTrxManager for this TrxSplit.
    Private mobjReplicaManager As ReplicaTrxManager

    '$Description Initialize a new Split object.

    Public Sub Init(ByVal strMemo_ As String, ByVal strCategoryKey_ As String, ByVal strPONumber_ As String, ByVal strInvoiceNum_ As String, ByVal datInvoiceDate_ As Date, ByVal datDueDate_ As Date, ByVal strTerms_ As String, ByVal strBudgetKey_ As String, ByVal curAmount_ As Decimal)

        mstrMemo = strMemo_
        mstrCategoryKey = strCategoryKey_
        mstrPONumber = strPONumber_
        mstrInvoiceNum = strInvoiceNum_
        mdatInvoiceDate = datInvoiceDate_
        mdatDueDate = datDueDate_
        mstrTerms = strTerms_
        mstrBudgetKey = strBudgetKey_
        mcurAmount = curAmount_
        mobjParent = Nothing
        mobjBudget = Nothing

    End Sub

    Public ReadOnly Property strMemo() As String
        Get
            strMemo = mstrMemo
        End Get
    End Property


    Public Property strCategoryKey() As String
        Get
            strCategoryKey = mstrCategoryKey
        End Get
        Set(ByVal Value As String)
            mstrCategoryKey = Value
        End Set
    End Property


    Public Property strPONumber() As String
        Get
            strPONumber = mstrPONumber
        End Get
        Set(ByVal Value As String)
            mstrPONumber = Value
        End Set
    End Property


    Public Property strInvoiceNum() As String
        Get
            strInvoiceNum = mstrInvoiceNum
        End Get
        Set(ByVal Value As String)
            mstrInvoiceNum = Value
        End Set
    End Property


    Public Property datInvoiceDate() As Date
        Get
            datInvoiceDate = mdatInvoiceDate
        End Get
        Set(ByVal Value As Date)
            mdatInvoiceDate = Value
        End Set
    End Property

    Public ReadOnly Property datInvoiceDateEffective() As Date
        Get
            Dim datEffective As Date = mdatInvoiceDate
            Dim intDaysBack As Integer
            Dim strTermsNormalized As String
            If datEffective = Utilities.datEmpty Then
                'Estimate invoice date from due date.
                strTermsNormalized = LCase(mstrTerms)
                strTermsNormalized = Replace(strTermsNormalized, " ", "")
                If InStr(strTermsNormalized, "net10") > 0 Then
                    intDaysBack = 10
                ElseIf InStr(strTermsNormalized, "net15") > 0 Then
                    intDaysBack = 15
                ElseIf InStr(strTermsNormalized, "net20") > 0 Then
                    intDaysBack = 20
                ElseIf InStr(strTermsNormalized, "net25") > 0 Then
                    intDaysBack = 25
                ElseIf InStr(strTermsNormalized, "net30") > 0 Then
                    intDaysBack = 30
                Else
                    'Is the category one we guessed to have short terms?
                    If InStr(objParent.objReg.objAccount.objCompany.strShortTermsCatKeys, Company.strEncodeCatKey(mstrCategoryKey)) > 0 Then
                        intDaysBack = 14
                    Else
                        intDaysBack = 30
                    End If
                End If
                datEffective = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDaysBack, datDueDateEffective)
            End If
            Return datEffective
        End Get
    End Property

    Public Property datDueDate() As Date
        Get
            datDueDate = mdatDueDate
        End Get
        Set(ByVal Value As Date)
            mdatDueDate = Value
        End Set
    End Property

    Public ReadOnly Property datDueDateEffective() As Date
        Get
            Dim datEffective = mdatDueDate
            If datEffective = Utilities.datEmpty Then
                datEffective = mobjParent.datDate
            End If
            Return datEffective
        End Get
    End Property

    Public Property strTerms() As String
        Get
            strTerms = mstrTerms
        End Get
        Set(ByVal Value As String)
            mstrTerms = Value
        End Set
    End Property

    Public ReadOnly Property strBudgetKey() As String
        Get
            strBudgetKey = mstrBudgetKey
        End Get
    End Property

    Public Property objBudget() As BudgetTrx
        Get
            Return mobjBudget
        End Get
        Set(ByVal Value As BudgetTrx)
            mobjBudget = Value
        End Set
    End Property

    Public Property objParent() As NormalTrx
        Get
            Return mobjParent
        End Get
        Set(value As NormalTrx)
            mobjParent = value
        End Set
    End Property

    Public ReadOnly Property curAmount() As Decimal
        Get
            curAmount = mcurAmount
        End Get
    End Property

    '$Description Adjust the amount of an existing split.

    Friend Sub AdjustAmount(ByVal curNewAmount As Decimal)
        mcurAmount = curNewAmount
    End Sub

    '$Description Search for matching budget Trx for mstrBudgetKey and datDate,
    '   and apply this Trx to that budget if one is found.
    '$Param objReg The Register to search for a budget Trx in.
    '$Param datDate The datDate property of the parent Trx.
    '$Param blnNoMatch See Register.lngMatchBudget().

    Public Sub ApplyToBudget(ByVal objReg As Register, ByVal datDate As Date, ByRef blnNoMatch As Boolean)

        Dim objBudgetTrx As BudgetTrx

        mobjBudget = Nothing
        blnNoMatch = False
        If mstrBudgetKey = "" Or mstrBudgetKey = objReg.objAccount.objCompany.strPlaceholderBudgetKey Then
            Exit Sub
        End If
        objBudgetTrx = objReg.objMatchBudget(mobjParent, mstrBudgetKey, blnNoMatch)
        If Not objBudgetTrx Is Nothing Then
            objBudgetTrx.ApplyToThisBudget(Me)
        End If

    End Sub

    '$Description If mobjBudget is nothing Nothing, un-apply this split from
    '   that budget Trx and set mobjBudget to nothing. Used as part of destroying
    '   this Split object.

    Friend Sub UnApplyFromBudget(ByVal objReg As Register)
        If Not mobjBudget Is Nothing Then
            mobjBudget.UnApplyFromThisBudget(Me)
            mobjBudget = Nothing
        End If
    End Sub

    Friend Sub CreateReplicaTrx(ByVal objCompany_ As Company, ByVal objNormalTrx As NormalTrx, ByVal blnLoading As Boolean)
        Dim intDotOffset As Integer = mstrCategoryKey.IndexOf("."c)
        If intDotOffset > 0 Then
            Dim intAccountKey As Integer = Integer.Parse(mstrCategoryKey.Substring(0, intDotOffset))
            For Each objAccount In objCompany_.colAccounts
                If objAccount.intKey = intAccountKey Then
                    Dim strRegKey As String = mstrCategoryKey.Substring(intDotOffset + 1)
                    For Each objReg In objAccount.colRegisters
                        If objReg.strRegisterKey = strRegKey Then
                            Dim objReplicaTrx As ReplicaTrx = New ReplicaTrx(objReg)
                            Dim strCatKey As String = objNormalTrx.objReg.objAccount.intKey.ToString() + "." + objNormalTrx.objReg.strRegisterKey
                            Dim strReplDescr As String
                            If Not String.IsNullOrEmpty(mstrMemo) Then
                                strReplDescr = mstrMemo
                            Else
                                strReplDescr = objNormalTrx.strDescription
                            End If
                            objReplicaTrx.NewStartReplica(True, objNormalTrx.datDate, strReplDescr,
                                strCatKey, mstrPONumber, mstrInvoiceNum, -mcurAmount, objNormalTrx.blnFake)
                            If blnLoading Then
                                objReg.NewLoadEnd(objReplicaTrx)
                            Else
                                objReg.NewAddEnd(objReplicaTrx, New LogAddNull(), "", blnSetChanged:=False)
                            End If
                            mobjReplicaManager = New ReplicaTrxManager(objReplicaTrx)
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Friend Sub DeleteReplicaTrx()
        If Not mobjReplicaManager Is Nothing Then
            mobjReplicaManager.objTrx.Delete(New LogDeleteNull(), "", blnSetChanged:=False)
            mobjReplicaManager = Nothing
        End If
    End Sub

    Public ReadOnly Property blnHasReplicaTrx() As Boolean
        Get
            Return (Not mobjReplicaManager Is Nothing)
        End Get
    End Property
End Class