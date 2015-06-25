Option Explicit On
Option Strict On

Imports System.ComponentModel

Public Class BooleanStringTypeConverter
    Inherits StringConverter

    Private mValues As List(Of String)

    Public Sub New()
        mValues = New List(Of String)
        mValues.Add("true")
        mValues.Add("false")
    End Sub

    Public Overrides Function GetStandardValuesSupported(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overrides Function GetStandardValues(context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
        Return New StandardValuesCollection(mValues)
    End Function

    Public Overrides Function GetStandardValuesExclusive(context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

End Class
