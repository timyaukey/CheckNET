Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class UTRegister

    'A wrapper around a Register with support for unit testing.
    'Includes an array of UTTrx describing the expected contents of
    'the Register after a series of operations, and a Validate()
    'method to confirm that the Register matches the UTTrx array.

    'Selected fields from one Trx expected to be in the Register.
    Public Structure UTTrx
        Dim datDate As Date
        Dim strNumber As String
        Dim strDescription As String
        Dim curAmount As Decimal
        Dim blnFake As Boolean
        Dim lngType As Trx.TrxType
        Dim strRepeatKey As String
        Dim intRepeatSeq As Short
    End Structure

    'List of UTTrx representing expected contents of the Register,
    'dimensioned 1 to intTrx.
    Private maudtTrx() As UTTrx
    Public intTrx As Short

    'The Register managed by this UTRegister.
    Private WithEvents mobjReg As Register

    'Errors discovered by Register.Validate().
    Private mcolErrors As Collection

    'Set by Register event handlers from arguments passed to them
    'so other code can confirm events were fired as expected.
    Private mlngTrxAddedIndex As Integer
    Private mlngTrxUpdatedOldIndex As Integer
    Private mlngTrxUpdatedNewIndex As Integer
    Private mlngTrxDeletedIndex As Integer
    Private mobjTrxReported As Trx
    Private mstrBudgetsChanged As String
    Private mlngBalanceChangeFirstIndex As Integer
    Private mlngBalanceChangeLastIndex As Integer

    'Initialize a new UTRegister with an empty Register.

    Public Sub Init(ByVal strRegisterKey As String)
        mobjReg = New Register
        mobjReg.Init("title", strRegisterKey, False, False, 3, DateAdd(Microsoft.VisualBasic.DateInterval.Year, -20, Today), False)
    End Sub

    'The Register managed by this UTRegister.

    Public ReadOnly Property objReg() As Register
        Get
            objReg = mobjReg
        End Get
    End Property

    Public ReadOnly Property strBudgetsChanged() As String
        Get
            strBudgetsChanged = mstrBudgetsChanged
        End Get
    End Property

    'Add a normal Trx to Register and UTTrx array without any unit testing assertions.
    'Used to add Trx which will be input to later tests. Does NOT fire any Register events.

    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
    Public Sub LoadNormal(ByVal strNumber As String, ByVal datDate As Date, ByVal curAmount As Decimal, _
                          Optional ByVal lngStatus As Trx.TrxStatus = Trx.TrxStatus.glngTRXSTS_UNREC, _
                          Optional ByVal blnFake As Boolean = False, Optional ByVal curNormalMatchRange As Decimal = 0, _
                          Optional ByVal strImportKey As String = "", Optional ByVal strRepeatKey As String = "", _
                          Optional ByVal strCategoryKey As String = "cat1", Optional ByVal strBudgetKey As String = "", _
                          Optional ByVal vcurAmount2 As Object = Nothing, Optional ByVal strBudgetKey2 As String = "", _
                          Optional ByVal blnAwaitingReview As Boolean = False, Optional ByVal blnAutoGenerated As Boolean = False, _
                          Optional ByVal strPONumber As String = "", Optional ByVal strPONumber2 As String = "", _
                          Optional ByVal strInvoiceNum As String = "", Optional ByVal strInvoiceNum2 As String = "", _
                          Optional ByVal datInvoiceDate As Date = #12:00:00 AM#, Optional ByVal datDueDate As Date = #12:00:00 AM#, _
                          Optional ByVal strTerms As String = "", Optional ByVal intRepeatSeq As Short = 0)

        Dim objTrx As Trx

        objTrx = New Trx
        objTrx.NewStartNormal(objReg, strNumber, datDate, "descr", "memo", lngStatus, blnFake, curNormalMatchRange, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strImportKey, strRepeatKey)
        objTrx.AddSplit("smemo", strCategoryKey, strPONumber, strInvoiceNum, datInvoiceDate, datDueDate, strTerms, strBudgetKey, curAmount, "")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If Not gblnXmlAttributeMissing(vcurAmount2) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vcurAmount2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            objTrx.AddSplit("smemo2", strCategoryKey, strPONumber2, strInvoiceNum2, datInvoiceDate, datDueDate, strTerms, strBudgetKey2, vcurAmount2, "")
        End If
        objReg.NewLoadEnd(objTrx)
        AddTrx(objTrx)

    End Sub

    'Add a budget Trx to Register and UTTrx array without any unit testing assertions.
    'Used to add Trx which will be input to later tests. Does NOT fire any Register events.

    Public Sub LoadBudget(ByVal datDate As Date, ByVal curBudgetLimit As Decimal, ByVal datBudgetEnds As Date, ByVal strBudgetKey As String, Optional ByVal blnAwaitingReview As Boolean = False, Optional ByVal blnAutoGenerated As Boolean = False, Optional ByVal strRepeatKey As String = "", Optional ByVal intRepeatSeq As Short = 0)

        Dim objTrx As Trx

        objTrx = New Trx
        objTrx.NewStartBudget(objReg, datDate, "descr", "memo", blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, curBudgetLimit, datBudgetEnds, strBudgetKey)
        objReg.NewLoadEnd(objTrx)
        AddTrx(objTrx)

    End Sub

    'Add a normal Trx to Register and UTTrx array, with unit testing assertions.
    'Fires Register events.

    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
    Public Sub AddNormal(ByVal strNumber As String, ByVal datDate As Date, ByVal curAmount As Decimal, ByVal strFailMsg As String, _
                         ByVal lngExpectedIndex As Integer, ByVal lngBalanceChangeFirst As Integer, ByVal lngBalanceChangeLast As Integer, _
                         Optional ByVal lngStatus As Trx.TrxStatus = Trx.TrxStatus.glngTRXSTS_UNREC, _
                         Optional ByVal blnFake As Boolean = False, Optional ByVal curNormalMatchRange As Decimal = 0, _
                         Optional ByVal strImportKey As String = "", Optional ByVal strRepeatKey As String = "", _
                         Optional ByVal strCategoryKey As String = "cat1", Optional ByVal strBudgetKey As String = "", _
                         Optional ByVal vcurAmount2 As Object = Nothing, Optional ByVal strBudgetKey2 As String = "", _
                         Optional ByVal strDescription As String = "descr", Optional ByVal blnAwaitingReview As Boolean = False, _
                         Optional ByVal blnAutoGenerated As Boolean = False, Optional ByVal strPONumber As String = "", _
                         Optional ByVal strPONumber2 As String = "", Optional ByVal strInvoiceNum As String = "", _
                         Optional ByVal strInvoiceNum2 As String = "", Optional ByVal datInvoiceDate As Date = #1/1/0100#, _
                         Optional ByVal datDueDate As Date = #1/1/0100#, Optional ByVal strTerms As String = "", _
                         Optional ByVal intRepeatSeq As Short = 0)

        Dim objTrx As Trx

        ClearEventReporting()
        objTrx = New Trx
        objTrx.NewStartNormal(objReg, strNumber, datDate, strDescription, "memo", lngStatus, blnFake, curNormalMatchRange, _
                              blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strImportKey, strRepeatKey)
        objTrx.AddSplit("smemo", strCategoryKey, strPONumber, strInvoiceNum, datInvoiceDate, datDueDate, strTerms, strBudgetKey, curAmount, "")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If Not gblnXmlAttributeMissing(vcurAmount2) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vcurAmount2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            objTrx.AddSplit("smemo2", strCategoryKey, strPONumber2, strInvoiceNum2, datInvoiceDate, datDueDate, strTerms, strBudgetKey2, vcurAmount2, "")
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogAdd). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objReg.NewAddEnd(objTrx, New LogAdd, "UTAddNormal")
        AddTrx(objTrx)

        gUTAssert(lngExpectedIndex = mlngTrxAddedIndex, strFailMsg & ": wrong index in add normal report")
        gUTAssert(objTrx Is mobjTrxReported, strFailMsg & ": wrong Trx in add normal report")
        gUTAssert(mlngBalanceChangeFirstIndex = lngBalanceChangeFirst, strFailMsg & ": wrong bal chg first index in add normal report")
        gUTAssert(mlngBalanceChangeLastIndex = lngBalanceChangeLast, strFailMsg & ": wrong bal chg last index in add normal report")

    End Sub

    'Update a normal Trx, with unit testing assertions.
    'Fires Register events.

    'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
    Public Sub UpdateNormal(ByVal strNumber As String, ByVal datDate As Date, ByVal curAmount As Decimal, ByVal strFailMsg As String, _
                            ByVal lngExpectedOldIndex As Integer, ByVal lngExpectedNewIndex As Integer, ByVal lngBalanceChangeFirst As Integer, _
                            ByVal lngBalanceChangeLast As Integer, Optional ByVal lngStatus As Trx.TrxStatus = Trx.TrxStatus.glngTRXSTS_UNREC, _
                            Optional ByVal blnFake As Boolean = False, Optional ByVal curNormalMatchRange As Decimal = 0, _
                            Optional ByVal strCategoryKey As String = "cat1", Optional ByVal strBudgetKey As String = "", _
                            Optional ByVal vcurAmount2 As Object = Nothing, Optional ByVal strBudgetKey2 As String = "", _
                            Optional ByVal blnAwaitingReview As Boolean = False, Optional ByVal blnAutoGenerated As Boolean = False, _
                            Optional ByVal strImportKey As String = "", Optional ByVal strRepeatKey As String = "", _
                            Optional ByVal strPONumber As String = "", Optional ByVal strPONumber2 As String = "", _
                            Optional ByVal strInvoiceNum As String = "", Optional ByVal strInvoiceNum2 As String = "", _
                            Optional ByVal datInvoiceDate As Date = #12:00:00 AM#, Optional ByVal datDueDate As Date = #12:00:00 AM#, _
                            Optional ByVal strTerms As String = "", Optional ByVal intRepeatSeq As Short = 0)

        Dim objTrx As Trx
        Dim objTrxOld As Trx

        ClearEventReporting()
        objTrx = objReg.objTrx(lngExpectedOldIndex)
        objTrxOld = objTrx.objClone(Nothing)
        objTrx.UpdateStartNormal(objReg, strNumber, datDate, "descr", "memo", lngStatus, blnFake, curNormalMatchRange, _
                                 blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strImportKey, strRepeatKey)
        objTrx.AddSplit("smemo", strCategoryKey, strPONumber, strInvoiceNum, datInvoiceDate, datDueDate, strTerms, strBudgetKey, curAmount, "")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If Not gblnXmlAttributeMissing(vcurAmount2) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vcurAmount2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            objTrx.AddSplit("smemo2", strCategoryKey, strPONumber2, strInvoiceNum2, datInvoiceDate, datDueDate, strTerms, strBudgetKey2, vcurAmount2, "")
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objReg.UpdateEnd(lngExpectedOldIndex, New LogChange, "UTUpdateNormal", objTrxOld)

        gUTAssert(lngExpectedOldIndex = mlngTrxUpdatedOldIndex, strFailMsg & ": wrong old index in upd normal report")
        gUTAssert(lngExpectedNewIndex = mlngTrxUpdatedNewIndex, strFailMsg & ": wrong new index in upd normal report")
        gUTAssert(objTrx Is mobjTrxReported, strFailMsg & ": wrong Trx in upd normal report")
        gUTAssert(mlngBalanceChangeFirstIndex = lngBalanceChangeFirst, strFailMsg & ": wrong bal chg first index in upd normal report")
        gUTAssert(mlngBalanceChangeLastIndex = lngBalanceChangeLast, strFailMsg & ": wrong bal chg last index in upd normal report")

    End Sub

    'Add a budget Trx to Register and UTTrx array, with unit testing assertions.
    'Fires Register events.

    Public Sub AddBudget(ByVal datDate As Date, ByVal curBudgetLimit As Object, ByVal datBudgetEnds As Object, ByVal strBudgetKey As String, ByVal strFailMsg As String, ByVal lngExpectedIndex As Integer, ByVal lngBalanceChangeFirst As Integer, ByVal lngBalanceChangeLast As Integer, Optional ByVal blnAwaitingReview As Boolean = False, Optional ByVal blnAutoGenerated As Boolean = False, Optional ByVal strRepeatKey As String = "", Optional ByVal intRepeatSeq As Short = 0)

        Dim objTrx As Trx

        ClearEventReporting()
        objTrx = New Trx
        'UPGRADE_WARNING: Couldn't resolve default property of object datBudgetEnds. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object curBudgetLimit. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objTrx.NewStartBudget(objReg, datDate, "descr", "memo", blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, curBudgetLimit, datBudgetEnds, strBudgetKey)
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogAdd). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objReg.NewAddEnd(objTrx, New LogAdd, "UTAddBudget")
        AddTrx(objTrx)

        gUTAssert(lngExpectedIndex = mlngTrxAddedIndex, strFailMsg & ": wrong index in add budget report")
        gUTAssert(objTrx Is mobjTrxReported, strFailMsg & ": wrong Trx in add budget report")
        gUTAssert(mlngBalanceChangeFirstIndex = lngBalanceChangeFirst, strFailMsg & ": wrong bal chg first index in add budget report")
        gUTAssert(mlngBalanceChangeLastIndex = lngBalanceChangeLast, strFailMsg & ": wrong bal chg last index in add budget report")

    End Sub

    'Update a budget Trx, with unit testing assertions.
    'Fires Register events.

    Public Sub UpdateBudget(ByVal datDate As Date, ByVal curBudgetLimit As Object, ByVal datBudgetEnds As Object, ByVal strBudgetKey As String, ByVal strFailMsg As String, ByVal lngExpectedOldIndex As Integer, ByVal lngExpectedNewIndex As Integer, ByVal lngBalanceChangeFirst As Integer, ByVal lngBalanceChangeLast As Integer, Optional ByVal blnAwaitingReview As Boolean = False, Optional ByVal blnAutoGenerated As Boolean = False, Optional ByVal strRepeatKey As String = "", Optional ByVal intRepeatSeq As Short = 0)

        Dim objTrx As Trx
        Dim objTrxOld As Trx

        ClearEventReporting()
        objTrx = objReg.objTrx(lngExpectedOldIndex)
        objTrxOld = objTrx.objClone(Nothing)
        'UPGRADE_WARNING: Couldn't resolve default property of object datBudgetEnds. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object curBudgetLimit. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objTrx.UpdateStartBudget(objReg, datDate, "descr", "memo", blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, curBudgetLimit, datBudgetEnds, strBudgetKey)
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objReg.UpdateEnd(lngExpectedOldIndex, New LogChange, "UTUpdateBudget", objTrxOld)

        gUTAssert(lngExpectedOldIndex = mlngTrxUpdatedOldIndex, strFailMsg & ": wrong old index in upd budget report")
        gUTAssert(lngExpectedNewIndex = mlngTrxUpdatedNewIndex, strFailMsg & ": wrong new index in upd budget report")
        gUTAssert(objTrx Is mobjTrxReported, strFailMsg & ": wrong Trx in upd budget report")
        gUTAssert(mlngBalanceChangeFirstIndex = lngBalanceChangeFirst, strFailMsg & ": wrong bal chg first index in upd budget report")
        gUTAssert(mlngBalanceChangeLastIndex = lngBalanceChangeLast, strFailMsg & ": wrong bal chg last index in upd budget report")

    End Sub

    'Delete a Trx, with unit testing assertions.
    'Fires Register events.

    Public Sub DeleteEntry(ByVal lngIndex As Integer, ByVal lngBalanceChangeFirst As Integer, ByVal lngBalanceChangeLast As Integer, ByVal strFailMsg As String)

        ClearEventReporting()
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogDelete). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objReg.Delete(lngIndex, New LogDelete, "UTDeleteEntry")
        gUTAssert(mlngTrxDeletedIndex = lngIndex, strFailMsg & ": wrong deleted trx")
        gUTAssert(mlngBalanceChangeFirstIndex = lngBalanceChangeFirst, strFailMsg & ": wrong bal chg first index in del budget report")
        gUTAssert(mlngBalanceChangeLastIndex = lngBalanceChangeLast, strFailMsg & ": wrong bal chg last index in del budget report")

    End Sub

    'Clear all the values set the Register events.

    Private Sub ClearEventReporting()
        'UPGRADE_NOTE: Object mobjTrxReported may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjTrxReported = Nothing
        mlngTrxAddedIndex = 0
        mlngTrxUpdatedOldIndex = 0
        mlngTrxUpdatedNewIndex = 0
        mlngTrxDeletedIndex = 0
        mlngBalanceChangeFirstIndex = 0
        mlngBalanceChangeLastIndex = 0
        mstrBudgetsChanged = ""
    End Sub

    'Add a Trx to the UTTrx array.

    Public Sub AddTrx(ByVal objTrx As Trx)
        intTrx = intTrx + 1
        'UPGRADE_WARNING: Lower bound of array maudtTrx was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        ReDim Preserve maudtTrx(intTrx)
        With maudtTrx(intTrx)
            .datDate = objTrx.datDate
            .curAmount = objTrx.curAmount
            .lngType = objTrx.lngType
            .blnFake = objTrx.blnFake
            .strDescription = objTrx.strDescription
            .strNumber = objTrx.strNumber
            .strRepeatKey = objTrx.strRepeatKey
            .intRepeatSeq = objTrx.intRepeatSeq
        End With
    End Sub

    'Modify value in UTTrx array.

    Public Sub SetTrxAmount(ByVal intIndex As Short, ByVal curAmount As Decimal)
        maudtTrx(intIndex).curAmount = curAmount
    End Sub

    'Modify value in UTTrx array.

    Public Sub SetTrxNumber(ByVal intIndex As Short, ByVal strNumber As String)
        maudtTrx(intIndex).strNumber = strNumber
    End Sub

    'Modify value in UTTrx array.

    Public Sub SetTrxDate(ByVal intIndex As Short, ByVal datDate As Date)
        maudtTrx(intIndex).datDate = datDate
    End Sub

    'Modify value in UTTrx array.

    Public Sub SetTrxRepeatKey(ByVal intIndex As Short, ByVal strRepeatKey As String)
        maudtTrx(intIndex).strRepeatKey = strRepeatKey
    End Sub

    'Modify value in UTTrx array.

    Public Sub SetTrxRepeatSeq(ByVal intIndex As Short, ByVal intRepeatSeq As Short)
        maudtTrx(intIndex).intRepeatSeq = intRepeatSeq
    End Sub
    'Assert that each Trx in the Register matches a specified element in the UTTrx array,
    'and then report any errors detected by Register.Validate.

    'UPGRADE_WARNING: ParamArray avntUTIdx was changed from ByRef to ByVal. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="93C6A0DC-8C99-429A-8696-35FC4DCEFCCC"'
    Public Sub Validate(ByVal strSubTest As String, ByVal ParamArray avntUTIdx() As Object)

        Dim intRegIndex As Short
        Dim intUTIndex As Short
        Dim objTrx As Trx

        If strSubTest <> "" Then
            gUTSetSubTest(strSubTest)
        End If

        If objReg.lngTrxCount <> (UBound(avntUTIdx) - LBound(avntUTIdx) + 1) Then
            gUTFailure("Wrong Trx count")
            Exit Sub
        End If

        For intRegIndex = 1 To objReg.lngTrxCount
            objTrx = objReg.objTrx(intRegIndex)
            'UPGRADE_WARNING: Couldn't resolve default property of object avntUTIdx(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            intUTIndex = avntUTIdx(LBound(avntUTIdx) + intRegIndex - 1)
            With maudtTrx(intUTIndex)
                gUTAssert(objTrx.datDate = .datDate, "Bad datDate on reg index " & intRegIndex)
                gUTAssert(objTrx.strNumber = .strNumber, "Bad strNumber on reg index " & intRegIndex)
                gUTAssert(objTrx.curAmount = .curAmount, "Bad curAmount on reg index " & intRegIndex)
                gUTAssert(objTrx.blnFake = .blnFake, "Bad blnFake on reg index " & intRegIndex)
                gUTAssert(objTrx.strRepeatKey = .strRepeatKey, "Bad strRepeatKey on reg index " & intRegIndex)
                gUTAssert(objTrx.intRepeatSeq = .intRepeatSeq, "Bad intRepeatSeq on reg index " & intRegIndex)
            End With
        Next

        mcolErrors = New Collection
        mobjReg.ValidateRegister()
        If mcolErrors.Count() > 0 Then
            'UPGRADE_WARNING: Couldn't resolve default property of object mcolErrors.Item(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTFailure("Unexpected validation errors: " & mcolErrors.Item(1))
        End If

    End Sub

    Private Sub mobjReg_BalancesChanged(ByVal lngFirstIndex As Integer, ByVal lngLastIndex As Integer) Handles mobjReg.BalancesChanged
        mlngBalanceChangeFirstIndex = lngFirstIndex
        mlngBalanceChangeLastIndex = lngLastIndex
    End Sub

    Private Sub mobjReg_BudgetChanged(ByVal lngIndex As Integer, ByVal objBudget As Trx) Handles mobjReg.BudgetChanged
        mstrBudgetsChanged = mstrBudgetsChanged & "," & lngIndex
    End Sub

    Private Sub mobjReg_TrxAdded(ByVal lngIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxAdded
        mobjTrxReported = objTrx
        mlngTrxAddedIndex = lngIndex
    End Sub

    Private Sub mobjReg_TrxDeleted(ByVal lngIndex As Integer) Handles mobjReg.TrxDeleted
        mlngTrxDeletedIndex = lngIndex
    End Sub

    Private Sub mobjReg_TrxUpdated(ByVal lngOldIndex As Integer, ByVal lngNewIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxUpdated
        mobjTrxReported = objTrx
        mlngTrxUpdatedOldIndex = lngOldIndex
        mlngTrxUpdatedNewIndex = lngNewIndex
    End Sub

    Private Sub mobjReg_ValidationError(ByVal lngIndex As Integer, ByVal strMsg As String) Handles mobjReg.ValidationError
        mcolErrors.Add("Index=" & lngIndex & ": " & strMsg)
    End Sub
End Class