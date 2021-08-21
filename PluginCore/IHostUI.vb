Option Strict On
Option Explicit On

''' <summary>
''' Provide plugins a way to access to the user interface and other 
''' services of the main window.
''' An instance of this is passed to all plugin constructors, and many forms.
''' </summary>

Public Interface IHostUI
    ReadOnly Property Company() As Company

    Function GetSearchTools() As IEnumerable(Of ISearchTool)

    Function GetTrxTools() As IEnumerable(Of ITrxTool)

    Function GetSearchHandlers() As IEnumerable(Of ISearchHandler)

    Function GetSearchFilters() As IEnumerable(Of ISearchFilter)

    Sub ShowRegister(ByVal objReg As Register)

    Function ChooseFile(ByVal strWindowCaption As String, ByVal strFileType As String,
        ByVal strSettingsKey As String) As String

    Function GetCurrentRegister() As Register

    Function GetMainForm() As System.Windows.Forms.Form

    Function MakeRegisterForm() As IRegisterForm

    Function MakeSearchForm() As ISearchForm

    Function MakeTrxForm() As ITrxForm

    Function AddNormalTrx(ByVal objTrx As BankTrx,
                             ByRef datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                             ByVal strLogTitle As String) As Boolean

    Function AddNormalTrxSilent(ByVal objTrx As BankTrx,
                             ByRef datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                             ByVal strLogTitle As String) As Boolean

    Function AddBudgetTrx(ByVal objReg As Register, ByRef datDefaultDate As DateTime,
                             ByVal strLogTitle As String) As Boolean

    Function AddTransferTrx(ByVal objReg As Register, ByRef datDefaultDate As DateTime,
                               ByVal strLogTitle As String) As Boolean

    Function UpdateTrx(ByVal objTrx As BaseTrx, ByRef datDefaultDate As Date,
                          ByVal strLogTitle As String) As Boolean

    Sub InfoMessageBox(ByVal strMessage As String)

    Sub ErrorMessageBox(ByVal strMessage As String)

    Sub ShowHelp(ByVal strHtmlFile As String)

    Function OkCancelMessageBox(ByVal strMessage As String) As System.Windows.Forms.DialogResult

    ReadOnly Property SoftwareName() As String

    ''' <summary>
    ''' Image to display on splash form at startup.
    ''' Image will be stretched to 4:3 (width:height).
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property SplashImagePath() As String
End Interface
