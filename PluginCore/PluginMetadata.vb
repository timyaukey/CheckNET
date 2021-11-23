Option Strict On
Option Explicit On

''' <summary>
''' Description of plugin. Returned by IPlugin.Register().
''' </summary>

Public Class PluginMetadata
    Public ReadOnly PluginName As String
    Public ReadOnly Manufacturer As String
    Public ReadOnly Assembly As System.Reflection.Assembly
    Public ReadOnly Version As System.Version
    Public ReadOnly InfoURL As String
    Public ReadOnly Description As String
    Public ReadOnly License As Willowsoft.TamperProofData.IStandardLicense

    Public Sub New(ByVal pluginName_ As String, ByVal manufacturer_ As String,
                   ByVal assembly_ As System.Reflection.Assembly, ByVal infoURL_ As String,
                   ByVal description_ As String, ByVal license_ As Willowsoft.TamperProofData.IStandardLicense)
        PluginName = pluginName_
        Manufacturer = manufacturer_
        Assembly = assembly_
        Version = Assembly.GetName().Version
        InfoURL = infoURL_
        Description = description_
        License = license_
    End Sub

    Public Sub New(ByVal assembly_ As System.Reflection.Assembly)
        Me.New(Nothing, Nothing, assembly_, Nothing, Nothing, Nothing)
    End Sub

End Class
