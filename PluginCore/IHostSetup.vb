Option Strict On
Option Explicit On

''' <summary>
''' Passed to IPlugin.Register() to give plugins a way to
''' install themselves into the user interface.
''' </summary>

Public Interface IHostSetup
    Property objFileMenu As MenuBuilder
    Property objBankImportMenu As MenuBuilder
    Property objCheckImportMenu As MenuBuilder
    Property objDepositImportMenu As MenuBuilder
    Property objInvoiceImportMenu As MenuBuilder
    Property objReportMenu As MenuBuilder
    Property objToolMenu As MenuBuilder
    Property objHelpMenu As MenuBuilder

    Sub SetTrxFormFactory(ByVal objFactory As Func(Of ITrxForm))
    Sub SetRegisterFormFactory(ByVal objFactory As Func(Of IRegisterForm))
    Sub SetSearchFormFactory(ByVal objFactory As Func(Of ISearchForm))

    Sub AddExtraLicense(ByVal objLicense As Willowsoft.TamperProofData.IStandardLicense)

End Interface
