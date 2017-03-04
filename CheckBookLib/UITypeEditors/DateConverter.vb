Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms
Imports System.Windows.Forms.Design

Public Class DateConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertFrom(context As ITypeDescriptorContext, sourceType As Type) As Boolean
        If (sourceType Is GetType(String)) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object) As Object
        If (TypeOf value Is String) Then
            Dim tmp As DateTime
            Dim formats(2) As String
            formats(0) = "M/d/yy"
            formats(1) = "M/d/yyyy"
            If Not DateTime.TryParseExact(CType(value, String), formats, _
                                          New System.Globalization.CultureInfo("en-US"), _
                                          System.Globalization.DateTimeStyles.None, tmp) Then
                Throw New ArgumentException("Invalid date")
            End If
            Return value
        End If

        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If (destinationType Is GetType(System.String) _
            AndAlso TypeOf value Is String) Then

            Return value
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class
