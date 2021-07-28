Option Strict On
Option Explicit On

Public Class EventLog

    'Collection of all ILogger objects passed to AddILogg???().
    'Note that these objects also implement another interface
    'specific to the method used to add them, like ILogAdd.
    Private mcolLoggers As List(Of ILogger)
    'Collection of ILogGroupStart objects for open groups.
    Private mcolGroups As List(Of ILogGroupStart)
    'The XML document used to build log output.
    Private mdomOutput As CBXmlDocument
    'Element to add events to.
    Private melmEventContainer As CBXmlElement
    'Current event element.
    Private melmEvent As CBXmlElement
    'Register this log is for.
    Private mobjReg As Register
    'Company this log is for.
    Private mobjCompany As Company
    'Login name of user operating the software.
    Private mstrLogin As String
    'Date and time Init() was called.
    Private mdatStart As Date
    'List of repeat keys
    Private mobjRepeats As IStringTranslator

    Public Sub Init(ByVal objReg As Register, ByVal strLogin As String)
        mcolLoggers = New List(Of ILogger)
        mcolGroups = New List(Of ILogGroupStart)
        mdomOutput = Nothing
        mstrLogin = strLogin
        mobjReg = objReg
        mobjCompany = mobjReg.Account.Company
        mdatStart = Now
    End Sub

    Public ReadOnly Property Company() As Company
        Get
            Return mobjCompany
        End Get
    End Property

    Public Sub WriteAll(ByVal strAccountTitle As String, ByVal objRepeats As IStringTranslator)
        Dim objLogger As ILogger
        Dim objParseError As CBXmlParseError
        Dim strLogFolder As String
        Dim strLogFile As String
        Dim blnRequired As Boolean

        'Some events don't require a log to be written, like saving a register,
        'unless other events for the same register do require it.
        For Each objLogger In mcolLoggers
            If objLogger.RequiresLog Then
                blnRequired = True
            End If
        Next objLogger

        If Not blnRequired Then
            Exit Sub
        End If

        mdomOutput = New CBXmlDocument
        mdomOutput.LoadXml("<Activity Login=""" & mstrLogin & """ SessionStart=""" & Utilities.strFormatDate(mdatStart, "G") & """></Activity>")
        objParseError = mdomOutput.ParseError
        If Not objParseError Is Nothing Then
            ShowTrxGeneratorLoadError("", gstrXMLParseErrorText(objParseError))
            Exit Sub
        End If
        mdomOutput.SetProperty("SelectionLanguage", "XPath")
        mobjRepeats = objRepeats
        melmEventContainer = mdomOutput.DocumentElement
        For Each objLogger In mcolLoggers
            objLogger.WriteLog(Me)
        Next objLogger
        strLogFolder = mobjCompany.AddNameToDataPath("EventLogs")
        If Dir(strLogFolder, FileAttribute.Directory) = "" Then
            MkDir(strLogFolder)
        End If
        strLogFolder = strLogFolder & "\" & Utilities.strFormatDate(Today, "yyyy-MMM")
        If Dir(strLogFolder, FileAttribute.Directory) = "" Then
            MkDir(strLogFolder)
        End If
        strLogFile = strLogFolder & "\" & strAccountTitle & "_R" & mobjReg.RegisterKey & "_" & Utilities.strFormatDate(mdatStart, "yyyy-MMM-dd-HH-mm-ss") & ".xml"
        mdomOutput.Save(strLogFile)
    End Sub

    Public Sub AddILogAction(ByVal objActionLogger As ILogAction, ByVal strTitle As String)
        objActionLogger.Init(strTitle)
        mcolLoggers.Add(objActionLogger)
    End Sub

    Public Sub AddILogAdd(ByVal objAddLogger As ILogAdd, ByVal strTitle As String, ByVal objNewTrx As BaseTrx)
        objAddLogger.Init(strTitle, objNewTrx.CloneTrx(False))
        mcolLoggers.Add(objAddLogger)
    End Sub

    Public Sub AddILogChange(ByVal objChangeLogger As ILogChange, ByVal strTitle As String, ByVal objNewTrx As BaseTrx, ByVal objOldTrx As BaseTrx)
        objChangeLogger.Init(strTitle, objNewTrx.CloneTrx(False), objOldTrx.CloneTrx(False))
        mcolLoggers.Add(objChangeLogger)
    End Sub

    Public Sub AddILogDelete(ByVal objDeleteLogger As ILogDelete, ByVal strTitle As String, ByVal objOldTrx As BaseTrx)
        objDeleteLogger.Init(strTitle, objOldTrx.CloneTrx(False))
        mcolLoggers.Add(objDeleteLogger)
    End Sub

    Public Sub AddILogGroupStart(ByVal objStartLogger As ILogGroupStart, ByVal strTitle As String)
        objStartLogger.Init(strTitle)
        mcolLoggers.Add(objStartLogger)
        mcolGroups.Add(objStartLogger)
    End Sub

    Public Sub AddILogGroupEnd(ByVal objEndLogger As ILogGroupEnd, ByVal objStartLogger As ILogGroupStart)
        Dim objTopGroup As ILogGroupStart
        objTopGroup = mcolGroups.Item(mcolGroups.Count() - 1)
        If Not objTopGroup Is objStartLogger Then
            gRaiseError("Invalid log group nesting")
        End If
        mcolGroups.RemoveAt(mcolGroups.Count() - 1)
        mcolLoggers.Add(objEndLogger)
    End Sub

    'Called by ILogger objects.
    Public Sub EventStart(ByVal strTitle As String, ByVal datTimestamp As Date)
        melmEvent = mdomOutput.CreateElement("Event")
        melmEvent.SetAttribute("Title", strTitle)
        melmEvent.SetAttribute("When", Utilities.strFormatDate(datTimestamp, "G"))
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
    Public Sub WriteTrx(ByVal strName As String, ByVal objTrx As BaseTrx)
        Dim elmTrx As CBXmlElement
        Dim objSplit As TrxSplit
        Dim elmSplitParent As CBXmlElement
        Dim objNormalTrx As BankTrx
        elmTrx = mdomOutput.CreateElement(strName)
        melmEvent.AppendChild(elmTrx)
        With elmTrx
            .SetAttribute("Date", Utilities.strFormatDate(objTrx.TrxDate))
            .SetAttribute("Number", objTrx.Number)
            .SetAttribute("Payee", objTrx.Description)
            .SetAttribute("Amount", Utilities.strFormatCurrency(objTrx.Amount))
            .SetAttribute("FakeStatus", objTrx.FakeStatusLabel)
            If objTrx.Memo <> "" Then
                .SetAttribute("TrxMemo", objTrx.Memo)
            End If
            If objTrx.RepeatKey <> "" Then
                .SetAttribute("RptName", mobjRepeats.KeyToValue1(objTrx.RepeatKey))
                .SetAttribute("RptSeq", CStr(objTrx.RepeatSeq))
            End If
            If TypeOf objTrx Is BankTrx Then
                .SetAttribute("Type", "Normal")
                elmSplitParent = elmTrx
                objNormalTrx = DirectCast(objTrx, BankTrx)
                For Each objSplit In objNormalTrx.Splits
                    If objNormalTrx.SplitCount > 1 Then
                        elmSplitParent = mdomOutput.CreateElement("Split")
                        elmTrx.AppendChild(elmSplitParent)
                    End If
                    With elmSplitParent
                        .SetAttribute("CatName", mobjCompany.Categories.KeyToValue1(objSplit.CategoryKey))
                        If objNormalTrx.SplitCount > 1 Then
                            .SetAttribute("Amount", Utilities.strFormatCurrency(objSplit.Amount))
                        End If
                        If objSplit.PONumber <> "" Then
                            .SetAttribute("PONum", objSplit.PONumber)
                        End If
                        If objSplit.InvoiceNum <> "" Then
                            .SetAttribute("InvNum", objSplit.InvoiceNum)
                        End If
                        If objSplit.InvoiceDate <> Utilities.datEmpty Then
                            .SetAttribute("InvDate", Utilities.strFormatDate(objSplit.InvoiceDate))
                        End If
                        If objSplit.DueDate <> Utilities.datEmpty Then
                            .SetAttribute("DueDate", Utilities.strFormatDate(objSplit.DueDate))
                        End If
                        If objSplit.Terms <> "" Then
                            .SetAttribute("Terms", objSplit.Terms)
                        End If
                        If objSplit.BudgetKey <> "" Then
                            .SetAttribute("BudgetName", mobjCompany.Budgets.KeyToValue1(objSplit.BudgetKey))
                        End If
                    End With
                Next objSplit
            ElseIf TypeOf objTrx Is BudgetTrx Then
                Dim objBudgetTrx As BudgetTrx = DirectCast(objTrx, BudgetTrx)
                .SetAttribute("Type", "Budget")
                .SetAttribute("BudgetLimit", Utilities.strFormatCurrency(objBudgetTrx.BudgetLimit))
                .SetAttribute("BudgetName", mobjCompany.Budgets.KeyToValue1(objBudgetTrx.BudgetKey))
            ElseIf TypeOf objTrx Is TransferTrx Then
                .SetAttribute("Type", "Transfer")
            Else
                gRaiseError("Unsupported trx type")
            End If
        End With
    End Sub

    'Called by ILogger objects.
    Public Sub GroupStart(ByVal strTitle As String)
        Dim elmNewGroup As CBXmlElement
        elmNewGroup = mdomOutput.CreateElement("Group")
        elmNewGroup.SetAttribute("Title", strTitle)
        melmEventContainer.AppendChild(elmNewGroup)
        melmEventContainer = elmNewGroup
    End Sub

    'Called by ILogger objects.
    Public Sub GroupEnd()
        melmEventContainer = DirectCast(melmEventContainer.ParentNode, CBXmlElement)
    End Sub
End Class