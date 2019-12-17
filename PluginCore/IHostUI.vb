Option Strict On
Option Explicit On

''' <summary>
''' Implemented by CBMainForm to provide plugins access
''' to the user interface and other services of the main window.
''' An instance of this is passed to every plugin constructor,
''' and many forms.
''' </summary>

Public Interface IHostUI
    ReadOnly Property objCompany() As Company

    Property objBankImportMenu As MenuBuilder
    Property objCheckImportMenu As MenuBuilder
    Property objDepositImportMenu As MenuBuilder
    Property objInvoiceImportMenu As MenuBuilder
    Property objReportMenu As MenuBuilder
    Property objToolMenu As MenuBuilder

    Function objSearchTools() As IEnumerable(Of ISearchTool)

    Function objSearchHandlers() As IEnumerable(Of ISearchHandler)

    Sub ShowRegister(ByVal objReg As Register)

    Function strChooseFile(ByVal strWindowCaption As String, ByVal strFileType As String,
        ByVal strSettingsKey As String) As String

    Function objGetCurrentRegister() As Register

    Function objGetMainForm() As System.Windows.Forms.Form

    Function blnAddNormalTrx(ByVal objTrx As NormalTrx,
                             ByVal datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                             ByVal strLogTitle As String) As Boolean

    Function blnAddNormalTrxSilent(ByVal objTrx As NormalTrx,
                             ByVal datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                             ByVal strLogTitle As String) As Boolean

    Function blnAddBudgetTrx(ByVal objReg As Register, ByVal datDefaultDate As DateTime,
                             ByVal strLogTitle As String) As Boolean

    Function blnAddTransferTrx(ByVal objReg As Register, ByVal datDefaultDate As DateTime,
                               ByVal strLogTitle As String) As Boolean

    Function blnUpdateTrx(ByVal objTrx As Trx, ByRef datDefaultDate As Date,
                          ByVal strLogTitle As String) As Boolean

    Sub InfoMessageBox(ByVal strMessage As String)

    Sub ErrorMessageBox(ByVal strMessage As String)

    Function OkCancelMessageBox(ByVal strMessage As String) As System.Windows.Forms.DialogResult

    ReadOnly Property strSoftwareName() As String

    ReadOnly Property strSplashImagePath() As String
End Interface
