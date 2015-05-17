Option Strict Off
Option Explicit On
'UPGRADE_NOTE: Split was upgraded to Split_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
Public Class Split_Renamed
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

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
    'List of image files, separated by semicolons.
    Private mstrImageFiles As String

    'Reference to budget Trx for this Split.
    Private mobjBudget As Trx

    '$Description Initialize a new Split object.

    Public Sub Init(ByVal strMemo_ As String, ByVal strCategoryKey_ As String, ByVal strPONumber_ As String, ByVal strInvoiceNum_ As String, ByVal datInvoiceDate_ As Date, ByVal datDueDate_ As Date, ByVal strTerms_ As String, ByVal strBudgetKey_ As String, ByVal curAmount_ As Decimal, ByVal strImageFiles_ As String)

        mstrMemo = strMemo_
        mstrCategoryKey = strCategoryKey_
        mstrPONumber = strPONumber_
        mstrInvoiceNum = strInvoiceNum_
        mdatInvoiceDate = datInvoiceDate_
        mdatDueDate = datDueDate_
        mstrTerms = strTerms_
        mstrBudgetKey = strBudgetKey_
        mcurAmount = curAmount_
        mstrImageFiles = strImageFiles_
        'UPGRADE_NOTE: Object mobjBudget may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
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


    Public Property datDueDate() As Date
        Get
            datDueDate = mdatDueDate
        End Get
        Set(ByVal Value As Date)
            mdatDueDate = Value
        End Set
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

    Public ReadOnly Property objBudget() As Trx
        Get
            objBudget = mobjBudget
        End Get
    End Property

    Public ReadOnly Property curAmount() As Decimal
        Get
            curAmount = mcurAmount
        End Get
    End Property


    Public Property strImageFiles() As String
        Get
            strImageFiles = mstrImageFiles
        End Get
        Set(ByVal Value As String)
            mstrImageFiles = Value
        End Set
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

        Dim lngMatchIndex As Integer

        'UPGRADE_NOTE: Object mobjBudget may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjBudget = Nothing
        blnNoMatch = False
        If mstrBudgetKey = "" Or mstrBudgetKey = gstrPlaceholderBudgetKey Then
            Exit Sub
        End If
        lngMatchIndex = objReg.lngMatchBudget(datDate, mstrBudgetKey, blnNoMatch)
        If lngMatchIndex = 0 Then
            Exit Sub
        End If
        mobjBudget = objReg.objTrx(lngMatchIndex)
        mobjBudget.ApplyToThisBudget(Me, objReg)

    End Sub

    '$Description If mobjBudget is nothing Nothing, un-apply this split from
    '   that budget Trx and set mobjBudget to nothing. Used as part of destroying
    '   this Split object.

    Friend Sub UnApplyFromBudget(ByVal objReg As Register)
        If Not mobjBudget Is Nothing Then
            mobjBudget.UnApplyFromThisBudget(Me, objReg)
            ClearBudgetReference()
        End If
    End Sub

    '$Description Clear any reference to a budget Trx.
    '   Used outside this class only by Trx.DestroyThisBudget().

    Friend Sub ClearBudgetReference()
        'UPGRADE_NOTE: Object mobjBudget may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjBudget = Nothing
    End Sub
End Class