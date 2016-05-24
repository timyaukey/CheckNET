﻿Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms
Imports System.Windows.Forms.Design

Public MustInherit Class StringTranslatorUIEditor
    Inherits UITypeEditor

    Public Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.DropDown
    End Function

    Public Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
        Dim editorService As IWindowsFormsEditorService
        Dim selectionControl As ListBoxUIEditor
        Dim objTranslator As StringTranslator
        Dim strCatName As String
        Dim i As Integer

        If (provider Is Nothing) Then
            Return String.Empty
        End If

        editorService = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
        If (editorService Is Nothing) Then
            Return String.Empty
        End If

        objTranslator = GetStringTranslator()
        selectionControl = New ListBoxUIEditor(editorService)
        For i = 1 To objTranslator.intElements
            selectionControl.Items.Add(objTranslator.strValue1(CShort(i)))
        Next
        editorService.DropDownControl(selectionControl)
        strCatName = CType(selectionControl.SelectedItem, String)
        For i = 1 To objTranslator.intElements
            If strCatName = objTranslator.strValue1(CShort(i)) Then
                Return objTranslator.strKey(CShort(i))
            End If
        Next
        Return String.Empty
    End Function

    Protected MustOverride Function GetStringTranslator() As StringTranslator

End Class

Public Class CategoryUIEditor
    Inherits StringTranslatorUIEditor

    Protected Overrides Function GetStringTranslator() As StringTranslator
        Return gobjCategories
    End Function
End Class

Public Class BudgetUIEditor
    Inherits StringTranslatorUIEditor

    Protected Overrides Function GetStringTranslator() As StringTranslator
        Return gobjBudgets
    End Function
End Class

Public Class ListBoxUIEditor
    Inherits ListBox

    Private editorService As IWindowsFormsEditorService

    Public Sub New(ByVal editorService As IWindowsFormsEditorService)
        Me.editorService = editorService
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        Me.editorService.CloseDropDown()
    End Sub
End Class

Public Class CategoryConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) And TypeOf value Is String Then
            Return gobjCategories.strKeyToValue1(CType(value, String))
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class

Public Class BudgetConverter
    Inherits TypeConverter

    Public Overrides Function CanConvertTo(context As ITypeDescriptorContext, destinationType As Type) As Boolean
        If destinationType Is GetType(String) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overrides Function ConvertTo(context As ITypeDescriptorContext, culture As Globalization.CultureInfo, value As Object, destinationType As Type) As Object
        If destinationType Is GetType(String) And TypeOf value Is String Then
            Return gobjBudgets.strKeyToValue1(CType(value, String))
        End If

        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function
End Class