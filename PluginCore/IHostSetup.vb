Option Strict On
Option Explicit On

''' <summary>
''' Passed to IPlugin.Register() to give plugins a way to
''' install themselves into the user interface.
''' </summary>

Public Interface IHostSetup
    Property FileMenu As MenuBuilder
    Property BankImportMenu As MenuBuilder
    Property CheckImportMenu As MenuBuilder
    Property DepositImportMenu As MenuBuilder
    Property InvoiceImportMenu As MenuBuilder
    Property ReportMenu As MenuBuilder
    Property ToolMenu As MenuBuilder
    Property HelpMenu As MenuBuilder

    Sub SetTrxFormFactory(ByVal objFactory As Func(Of ITrxForm))
    Sub SetRegisterFormFactory(ByVal objFactory As Func(Of IRegisterForm))
    Sub SetSearchFormFactory(ByVal objFactory As Func(Of ISearchForm))

    Sub AddExtraLicense(ByVal objLicense As Willowsoft.TamperProofData.IStandardLicense)

End Interface
