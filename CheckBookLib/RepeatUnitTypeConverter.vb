Option Explicit On
Option Strict On

Imports System.ComponentModel

Public Class RepeatUnitTypeConverter
    Inherits StringConverter

    Private mValues As List(Of String)

    Public Sub New()
        mValues = New List(Of String)
        mValues.Add("")
        mValues.Add("day")
        mValues.Add("week")
        mValues.Add("month")
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
