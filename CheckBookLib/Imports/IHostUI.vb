Option Strict On
Option Explicit On

Public Interface IHostUI
    Function blnImportFormAlreadyOpen() As Boolean
    Sub OpenImportForm(ByVal strWindowCaption As String, ByVal objHandler As IImportHandler,
        ByVal objReader As ITrxReader, ByVal blnFake As Boolean)
    Function strChooseFile(ByVal strWindowCaption As String, ByVal strFileType As String,
        ByVal strSettingsKey As String) As String
    Function objGetCurrentRegister() As Register
End Interface
