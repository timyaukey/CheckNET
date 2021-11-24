Option Strict On
Option Explicit On

''' <summary>
''' A convenience implementation of IPlugin.
''' </summary>

Public MustInherit Class PluginBase
    Implements IPlugin
    Protected ReadOnly HostUI As IHostUI
    Protected Company As Company
    Protected MetadataInternal As PluginMetadata

    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub

    Public MustOverride Sub Register(ByVal setup As IHostSetup) Implements IPlugin.Register

    Public ReadOnly Property Metadata As PluginMetadata Implements IPlugin.Metadata
        Get
            Return MetadataInternal
        End Get
    End Property

    Public Overridable Sub SetCompany(ByVal company_ As Company) Implements IPlugin.SetCompany
        Company = company_
    End Sub
End Class
