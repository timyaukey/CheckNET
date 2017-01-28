Option Strict On
Option Explicit On

<AttributeUsage(AttributeTargets.Assembly, AllowMultiple:=True)>
Public Class PluginFactoryAttribute
    Inherits System.Attribute

    Public objFactoryType As Type

    Public Sub New(objFactoryType_ As Type)
        objFactoryType = objFactoryType_
    End Sub

End Class
