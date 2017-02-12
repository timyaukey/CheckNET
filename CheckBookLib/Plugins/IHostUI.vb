Option Strict On
Option Explicit On

''' <summary>
''' Implemented by CBMainForm to provide plugins access
''' to the user interface and other services of the main window.
''' An instance of this is passed to every plugin constructor.
''' </summary>

Public Interface IHostUI
    Function blnImportFormAlreadyOpen() As Boolean
    Sub OpenImportForm(ByVal strWindowCaption As String, ByVal objHandler As IImportHandler,
        ByVal objReader As ITrxReader)
    Function strChooseFile(ByVal strWindowCaption As String, ByVal strFileType As String,
        ByVal strSettingsKey As String) As String
    Function objGetCurrentRegister() As Register
    Function objGetMainForm() As System.Windows.Forms.Form
End Interface
