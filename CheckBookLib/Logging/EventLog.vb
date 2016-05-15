Option Strict Off
Option Explicit On
Public Class EventLog

    'Collection of all ILogger objects passed to AddILogg???().
    'Note that these objects also implement another interface
    'specific to the method used to add them, like ILogAdd.
    Private mcolLoggers As Collection
    'Collection of ILogGroupStart objects for open groups.
    Private mcolGroups As Collection
    'The XML document used to build log output.
    Private mdomOutput As VB6XmlDocument
    'Element to add events to.
    Private melmEventContainer As VB6XmlElement
    'Current event element.
    Private melmEvent As VB6XmlElement
    'Register this log is for.
    Private mobjReg As Register
    'Login name of user operating the software.
    Private mstrLogin As String
    'Date and time Init() was called.
    Private mdatStart As Date
    'List of repeat keys
    Private mobjRepeats As StringTranslator

    Public Sub Init(ByVal objReg As Register, ByVal strLogin As String)
        mcolLoggers = New Collection
        mcolGroups = New Collection
        'UPGRADE_NOTE: Object mdomOutput may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mdomOutput = Nothing
        mstrLogin = strLogin
        mobjReg = objReg
        mdatStart = Now
    End Sub

    Public Sub WriteAll(ByVal strAccountTitle As String, ByVal objRepeats As StringTranslator)
        Dim objLogger As ILogger
        Dim objParseError As VB6XmlParseError
        Dim strLogFolder As String
        Dim strLogFile As String
        Dim blnRequired As Boolean

        'Some events don't require a log to be written, like saving a register,
        'unless other events for the same register do require it.
        For Each objLogger In mcolLoggers
            If objLogger.blnRequiresLog Then
                blnRequired = True
            End If
        Next objLogger

        If Not blnRequired Then
            Exit Sub
        End If

        mdomOutput = New VB6XmlDocument
        mdomOutput.LoadXml("<Activity Login=""" & mstrLogin & """ SessionStart=""" & gstrVB6Format(mdatStart, "General Date") & """></Activity>")
        objParseError = mdomOutput.ParseError
        If Not objParseError Is Nothing Then
            ShowTrxGenLoadError("", gstrXMLParseErrorText(objParseError))
            Exit Sub
        End If
        mdomOutput.SetProperty("SelectionLanguage", "XPath")
        mobjRepeats = objRepeats
        melmEventContainer = mdomOutput.DocumentElement
        For Each objLogger In mcolLoggers
            objLogger.WriteLog(Me)
        Next objLogger
        strLogFolder = gstrAddPath("EventLogs")
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(strLogFolder, FileAttribute.Directory) = "" Then
            MkDir(strLogFolder)
        End If
        strLogFolder = strLogFolder & "\" & gstrVB6Format(Today, "yyyy-mmm")
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(strLogFolder, FileAttribute.Directory) = "" Then
            MkDir(strLogFolder)
        End If
        strLogFile = strLogFolder & "\" & strAccountTitle & "_R" & mobjReg.strRegisterKey & "_" & gstrVB6Format(mdatStart, "yyyy-mmm-dd-hh-nn-ss") & ".xml"
        mdomOutput.Save(strLogFile)
    End Sub

    Public Sub AddILogAction(ByVal objActionLogger As ILogAction, ByVal strTitle As String)
        objActionLogger.Init(strTitle)
        mcolLoggers.Add(objActionLogger)
    End Sub

    Public Sub AddILogAdd(ByVal objAddLogger As ILogAdd, ByVal strTitle As String, ByVal objNewTrx As Trx)
        objAddLogger.Init(strTitle, objNewTrx.objClone(Nothing))
        mcolLoggers.Add(objAddLogger)
    End Sub

    Public Sub AddILogChange(ByVal objChangeLogger As ILogChange, ByVal strTitle As String, ByVal objNewTrx As Trx, ByVal objOldTrx As Trx)
        objChangeLogger.Init(strTitle, objNewTrx.objClone(Nothing), objOldTrx.objClone(Nothing))
        mcolLoggers.Add(objChangeLogger)
    End Sub

    Public Sub AddILogDelete(ByVal objDeleteLogger As ILogDelete, ByVal strTitle As String, ByVal objOldTrx As Trx)
        objDeleteLogger.Init(strTitle, objOldTrx.objClone(Nothing))
        mcolLoggers.Add(objDeleteLogger)
    End Sub

    Public Sub AddILogGroupStart(ByVal objStartLogger As ILogGroupStart, ByVal strTitle As String)
        objStartLogger.Init(strTitle)
        mcolLoggers.Add(objStartLogger)
        mcolGroups.Add(objStartLogger)
    End Sub

    Public Sub AddILogGroupEnd(ByVal objEndLogger As ILogGroupEnd, ByVal objStartLogger As ILogGroupStart)
        Dim objTopGroup As ILogGroupStart
        objTopGroup = mcolGroups.Item(mcolGroups.Count())
        If Not objTopGroup Is objStartLogger Then
            gRaiseError("Invalid log group nesting")
        End If
        mcolGroups.Remove(mcolGroups.Count())
        mcolLoggers.Add(objEndLogger)
    End Sub

    'Called by ILogger objects.
    Public Sub EventStart(ByVal strTitle As String, ByVal datTimestamp As Date)
        melmEvent = mdomOutput.CreateElement("Event")
        melmEvent.SetAttribute("Title", strTitle)
        melmEvent.SetAttribute("When", gstrVB6Format(datTimestamp, "General Date"))
        'UPGRADE_WARNING: Couldn't resolve default property of object melmEvent. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmEventContainer.AppendChild(melmEvent)
    End Sub

    'Called by ILogger objects.
    Public Sub EventEnd()
        'UPGRADE_NOTE: Object melmEvent may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        melmEvent = Nothing
    End Sub

    'Called by ILogger objects.
    Public Sub WriteValue(ByVal strName As String, ByVal strValue As String)
        melmEvent.SetAttribute(strName, strValue)
    End Sub

    'Called by ILogger objects.
    Public Sub WriteTrx(ByVal strName As String, ByVal objTrx As Trx)
        Dim elmTrx As VB6XmlElement
        Dim objSplit As Split_Renamed
        Dim elmSplitParent As VB6XmlElement
        elmTrx = mdomOutput.CreateElement(strName)
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrx. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmEvent.AppendChild(elmTrx)
        With elmTrx
            .SetAttribute("Date", gstrVB6Format(objTrx.datDate, gstrFORMAT_DATE))
            .SetAttribute("Number", objTrx.strNumber)
            .SetAttribute("Payee", objTrx.strDescription)
            .SetAttribute("Amount", gstrVB6Format(objTrx.curAmount, gstrFORMAT_CURRENCY))
            .SetAttribute("FakeStatus", objTrx.strFakeStatus)
            If objTrx.strMemo <> "" Then
                .SetAttribute("TrxMemo", objTrx.strMemo)
            End If
            If objTrx.strRepeatKey <> "" Then
                .SetAttribute("RptName", mobjRepeats.strKeyToValue1(objTrx.strRepeatKey))
                .SetAttribute("RptSeq", objTrx.intRepeatSeq)
            End If
            Select Case objTrx.lngType
                Case Trx.TrxType.glngTRXTYP_NORMAL
                    .SetAttribute("Type", "Normal")
                    elmSplitParent = elmTrx
                    For Each objSplit In objTrx.colSplits
                        If objTrx.lngSplits > 1 Then
                            elmSplitParent = mdomOutput.CreateElement("Split")
                            'UPGRADE_WARNING: Couldn't resolve default property of object elmSplitParent. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            elmTrx.AppendChild(elmSplitParent)
                        End If
                        With elmSplitParent
                            .SetAttribute("CatName", gobjCategories.strKeyToValue1(objSplit.strCategoryKey))
                            If objTrx.lngSplits > 1 Then
                                .SetAttribute("Amount", gstrVB6Format(objSplit.curAmount, gstrFORMAT_CURRENCY))
                            End If
                            If objSplit.strPONumber <> "" Then
                                .SetAttribute("PONum", objSplit.strPONumber)
                            End If
                            If objSplit.strInvoiceNum <> "" Then
                                .SetAttribute("InvNum", objSplit.strInvoiceNum)
                            End If
                            If objSplit.datInvoiceDate <> System.DateTime.FromOADate(0) Then
                                .SetAttribute("InvDate", gstrVB6Format(objSplit.datInvoiceDate, gstrFORMAT_DATE))
                            End If
                            If objSplit.datDueDate <> System.DateTime.FromOADate(0) Then
                                .SetAttribute("DueDate", gstrVB6Format(objSplit.datDueDate, gstrFORMAT_DATE))
                            End If
                            If objSplit.strTerms <> "" Then
                                .SetAttribute("Terms", objSplit.strTerms)
                            End If
                            If objSplit.strBudgetKey <> "" Then
                                .SetAttribute("BudgetName", gobjBudgets.strKeyToValue1(objSplit.strBudgetKey))
                            End If
                        End With
                    Next objSplit
                Case Trx.TrxType.glngTRXTYP_BUDGET
                    .SetAttribute("Type", "Budget")
                    .SetAttribute("BudgetLimit", gstrVB6Format(objTrx.curBudgetLimit, gstrFORMAT_CURRENCY))
                    .SetAttribute("BudgetName", gobjBudgets.strKeyToValue1(objTrx.strBudgetKey))
                Case Trx.TrxType.glngTRXTYP_TRANSFER
                    .SetAttribute("Type", "Transfer")
                Case Else
                    gRaiseError("Unsupported trx type")
            End Select
        End With
    End Sub

    'Called by ILogger objects.
    Public Sub GroupStart(ByVal strTitle As String)
        Dim elmNewGroup As VB6XmlElement
        elmNewGroup = mdomOutput.CreateElement("Group")
        elmNewGroup.SetAttribute("Title", strTitle)
        'UPGRADE_WARNING: Couldn't resolve default property of object elmNewGroup. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        melmEventContainer.AppendChild(elmNewGroup)
        melmEventContainer = elmNewGroup
    End Sub

    'Called by ILogger objects.
    Public Sub GroupEnd()
        melmEventContainer = melmEventContainer.ParentNode
    End Sub
End Class