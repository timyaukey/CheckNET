Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportUtilities
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Helper class for importing transaction information from a bank download file.
    'The caller must parse the information, this class only receives the information
    'and makes Trx objects from it.

    Private mobjAccount As Account
    Private mblnMakeFakeTrx As Boolean

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

    'Table with trx type translation information.
    Private mdomTrxTypes As VB6XmlDocument

    Public Sub Init(ByVal objAccount_ As Account)
        mobjAccount = objAccount_
    End Sub

    Public Sub LoadTrxTypeTable()
        On Error GoTo ErrorHandler

        Dim strTableFile As String

        'UPGRADE_WARNING: Couldn't resolve default property of object gstrTrxTypeFilePath(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        strTableFile = gstrTrxTypeFilePath()
        mdomTrxTypes = gdomLoadFile(strTableFile)

        Exit Sub
ErrorHandler:
        gNestedErrorTrap("LoadTrxTypeTable")
    End Sub

    Public WriteOnly Property blnMakeFakeTrx() As Boolean
        Set(ByVal Value As Boolean)
            mblnMakeFakeTrx = Value
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
    End Sub

    '$Description Create a new Trx object.

    Public Function objMakeTrx() As Trx
        Dim objTrx As Trx
        Dim datDate As Date
        Dim strImportKey As String
        Dim strCatKey As String
        Dim intCatIndex As Short
        Dim strBudKey As String
        Dim intBudIndex As Short
        Dim strDescription As String
        Dim strMemo As String
        Dim strNumber As String
        Dim strSplitPONumber As String
        Dim strSplitInvoiceNum As String
        Dim datSplitInvoiceDate As Date
        Dim datSplitDueDate As Date
        Dim strSplitTerms As String
        Dim strSplitImageFiles As String
        Dim curAmount As Decimal
        Dim strUniqueKey As String

        On Error GoTo ErrorHandler

        ConvertTransaction()
        datDate = CDate(mstrTrxDate)
        strNumber = mstrTrxNumber
        strDescription = mstrTrxPayee
        curAmount = CDec(mstrTrxAmount)
        strMemo = Trim(mstrTrxMemo)
        datSplitInvoiceDate = System.DateTime.FromOADate(0)
        datSplitDueDate = System.DateTime.FromOADate(0)
        With gobjCategories
            intCatIndex = .intLookupValue1(mstrTrxCategory)
            If intCatIndex > 0 Then
                strCatKey = .strKey(intCatIndex)
            End If
        End With
        With gobjBudgets
            intBudIndex = .intLookupValue1(mstrTrxBudget)
            If intBudIndex > 0 Then
                strBudKey = .strKey(intBudIndex)
            End If
        End With
        If mblnMakeFakeTrx Then
            strImportKey = ""
        Else
            If mstrTrxUniqueKey <> "" Then
                strUniqueKey = mstrTrxUniqueKey
            Else
                'If the ITrxImport did not supply a unique key then we construct one.
                'Have to use left part of mstrTrxPayeeTrimmed instead of the entire
                'thing because FSBLink sometimes
                'formats the payee name with different numbers of imbedded blanks in its
                '"source prefixes", and this causes the payee name to be truncated in
                'variable ways because there is a fixed max length on the payee name
                'in the download file. I believe the actual safe characters to use is
                '20, based on the max blanks it inserts, but 16 is nice and pessimistic
                'and ought to be enough to recognize the payee.
                strUniqueKey = strNumber & "|" & Left(mstrTrxPayeeTrimmed, 16) & "|" & VB6.Format(curAmount, gstrFORMAT_CURRENCY)
            End If
            'NOTE: Date must stay in the same place, because statement reconciliation
            'parses it out to show.
            strImportKey = strSqueezeInput("|" & VB6.Format(datDate, gstrFORMAT_DATE) & "|" & strUniqueKey)
        End If

        objTrx = New Trx
        objTrx.NewStartNormal(Nothing, strNumber, datDate, strDescription, strMemo, Trx.TrxStatus.glngTRXSTS_UNREC, mblnMakeFakeTrx, 0.0#, False, False, 0, strImportKey, "")
        objTrx.AddSplit("", strCatKey, strSplitPONumber, strSplitInvoiceNum, datSplitInvoiceDate, datSplitDueDate, strSplitTerms, strBudKey, curAmount, strSplitImageFiles)
        objMakeTrx = objTrx

        Exit Function
ErrorHandler:
        NestedError("objMakeTrx")
    End Function

    '$Description Analyze the payee to determine the type of transaction.
    '   Massages other data based on the transaction type.

    Private Sub ConvertTransaction()
        On Error GoTo ErrorHandler

        Dim objTrxTypes As VB6XmlNodeList
        Dim elmTrxType As VB6XmlElement
        Dim strSqueezedInput As String
        Dim strNormalizedInput As String
        Dim blnFailMatch As Boolean
        Dim vstrBefore As Object
        Dim vstrAfter As Object
        Dim vstrNumber As Object
        Dim vintMinAfter As Object
        Dim blnMatchAfter As Boolean
        Dim intAfterLen As Short
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
            'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxType.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrBefore. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vstrBefore = elmTrxType.GetAttribute("Before")
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If Not gblnXmlAttributeMissing(vstrBefore) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrBefore. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vstrBefore = strNormalizeInput(vstrBefore)
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrBefore. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If Left(strNormalizedInput, Len(vstrBefore)) <> vstrBefore Then
                    blnFailMatch = True
                End If
            End If
            'Check if ends with at least "n" characters from the start of the "After" attrib.
            'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxType.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vstrAfter = elmTrxType.GetAttribute("After")
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If Not gblnXmlAttributeMissing(vstrAfter) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxType.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vintMinAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vintMinAfter = elmTrxType.GetAttribute("MinAfter")
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If gblnXmlAttributeMissing(vintMinAfter) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vintMinAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    vintMinAfter = 3
                End If
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vstrAfter = strNormalizeInput(vstrAfter)
                blnMatchAfter = False
                Do
                    intAfterLen = Len(vstrAfter)
                    'UPGRADE_WARNING: Couldn't resolve default property of object vstrAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If Right(strNormalizedInput, intAfterLen) = vstrAfter Then
                        blnMatchAfter = True
                        Exit Do
                    End If
                    'UPGRADE_WARNING: Couldn't resolve default property of object vintMinAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If intAfterLen <= vintMinAfter Then
                        Exit Do
                    End If
                    'UPGRADE_WARNING: Couldn't resolve default property of object vstrAfter. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    vstrAfter = Left(vstrAfter, intAfterLen - 1)
                Loop
                If Not blnMatchAfter Then
                    blnFailMatch = True
                End If
            End If
            'Sanity check.
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If gblnXmlAttributeMissing(vstrBefore) And gblnXmlAttributeMissing(vstrAfter) Then
                gRaiseError("Neither Before= or After= specified for TrxType element")
            End If
            'We matched whichever of Before= and After= were specified.
            If Not blnFailMatch Then
                'Now trim whatever part was matched, to get the real payee.
                'We used the squeezed version, not the fully normalized, so
                'we get the original case information.
                strPayeeTrimmed = mstrTrxPayeeTrimmed
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If Not gblnXmlAttributeMissing(vstrBefore) Then
                    strPayeeTrimmed = Mid(strPayeeTrimmed, Len(vstrBefore) + 1)
                End If
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If Not gblnXmlAttributeMissing(vstrAfter) Then
                    strPayeeTrimmed = Left(strPayeeTrimmed, Len(strPayeeTrimmed) - Len(vstrAfter))
                End If
                'Remove blanks here so we don't have to include them in the trx type specs.
                strPayeeTrimmed = Trim(strPayeeTrimmed)
                'If Number=(number), then the trimmed input is really a check
                'number and we perform a sanity check to insure it really is a number.
                'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxType.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrNumber. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vstrNumber = elmTrxType.GetAttribute("Number")
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If gblnXmlAttributeMissing(vstrNumber) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vstrNumber. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    vstrNumber = ""
                End If
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrNumber. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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
                    'UPGRADE_WARNING: Couldn't resolve default property of object vstrNumber. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If vstrNumber = "(number)" Then
                        mstrTrxNumber = strPayeeTrimmed
                    Else
                        'UPGRADE_WARNING: Couldn't resolve default property of object vstrNumber. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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
ErrorHandler:
        NestedError("ConvertTransaction")
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
        On Error GoTo ErrorHandler
        Dim strOutputPayee As String
        Dim objPayees As VB6XmlNodeList
        Dim elmCategory As VB6XmlElement
        Dim elmBudget As VB6XmlElement
        Dim elmPayee As VB6XmlElement
        Dim elmTrxNum As VB6XmlElement
        Dim blnMatch As Boolean
        Dim curAmount As Decimal
        Dim strInput As String

        strOutputPayee = strTrimmedPayee
        objPayees = gdomTransTable.DocumentElement.SelectNodes("Payee")
        For Each elmPayee In objPayees
            'UPGRADE_WARNING: Couldn't resolve default property of object elmPayee.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            strInput = UCase(Trim(elmPayee.GetAttribute("Input")))
            If (strInput <> "") And (InStr(UCase(strTrimmedPayee), strInput) > 0) Then
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If Not gblnXmlAttributeMissing(elmPayee.GetAttribute("Min")) Then
                    curAmount = CDec(mstrTrxAmount)
                    'UPGRADE_WARNING: Couldn't resolve default property of object elmPayee.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    blnMatch = (curAmount >= CDec(elmPayee.GetAttribute("Min"))) And (curAmount <= CDec(elmPayee.GetAttribute("Max")))
                Else
                    blnMatch = True
                End If
                If blnMatch Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object elmPayee.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    strOutputPayee = elmPayee.GetAttribute("Output")
                    elmCategory = elmPayee.SelectSingleNode("Cat")
                    If Not elmCategory Is Nothing Then
                        mstrTrxCategory = elmCategory.Text
                    End If
                    elmBudget = elmPayee.SelectSingleNode("Budget")
                    If Not elmBudget Is Nothing Then
                        mstrTrxBudget = elmBudget.Text
                    End If
                    If mstrTrxNumber = "" Then
                        elmTrxNum = elmPayee.SelectSingleNode("Num")
                        If Not elmTrxNum Is Nothing Then
                            If elmTrxNum.Text <> "" Then
                                mstrTrxNumber = elmTrxNum.Text
                            End If
                        End If
                    End If
                    Exit For
                End If
            End If
        Next elmPayee
        mstrTrxPayee = strOutputPayee
        Exit Sub
ErrorHandler:
        NestedError("LookupPayee")
    End Sub

    Private Sub NestedError(ByVal strRoutine As String)
        gNestedErrorTrap("ImportUtilities." & strRoutine)
    End Sub
End Class