Option Strict On
Option Explicit On

Imports System.Collections.Generic

''' <summary>
''' Compile a summary of the usage of each repeat key.
''' Used to create the StringTranslator for all those keys.
''' </summary>
''' <remarks></remarks>
Public Class RepeatSummarizer

    Private mKeys As Dictionary(Of String, KeySummary)

    Public Sub New()
        mKeys = New Dictionary(Of String, KeySummary)
    End Sub

    ''' <summary>
    ''' Must be called for every BaseTrx that is part of a repeat sequence,
    ''' and for each BaseTrx generator. Must be called for the generator last,
    ''' so the name in the generator is the one used.
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="name"></param>
    ''' <param name="fromGenerator"></param>
    ''' <remarks></remarks>
    Public Sub Define(ByVal key As String, ByVal name As String, ByVal fromGenerator As Boolean)
        Dim sum As KeySummary = Nothing
        If Not mKeys.TryGetValue(key, sum) Then
            sum = New KeySummary()
            mKeys.Add(key, sum)
        End If
        sum.Define(key, name, fromGenerator)
    End Sub

    Public Function BuildStringTranslator() As SimpleStringTranslator
        Dim trans As SimpleStringTranslator = New SimpleStringTranslator()
        Dim keySum As KeySummary
        Dim sortedByName As List(Of KeySummary)
        Dim seqNum As Integer

        sortedByName = New List(Of KeySummary)(mKeys.Values)
        sortedByName.Sort(AddressOf SortKeySumByName)
        For Each keySum In sortedByName
            Dim repeatName As String = keySum.Name
            If Not keySum.FromGenerator Then
                repeatName += " (old)"
            End If
            seqNum = 1
            Dim qualifiedName As String = repeatName
            Do
                If trans.FindIndexOfValue1(qualifiedName) = 0 Then
                    trans.Add(New StringTransElement(trans, keySum.Key, qualifiedName, qualifiedName))
                    Exit Do
                End If
                seqNum += 1
                qualifiedName = repeatName + " #" + seqNum.ToString()
            Loop
        Next
        BuildStringTranslator = trans
    End Function

    Private Function SortKeySumByName(ByVal keySum1 As KeySummary, ByVal keySum2 As KeySummary) As Integer
        If keySum1.FromGenerator <> keySum2.FromGenerator Then
            If keySum1.FromGenerator Then
                SortKeySumByName = -1
            Else
                SortKeySumByName = 1
            End If
        Else
            SortKeySumByName = keySum1.Name.CompareTo(keySum2.Name)
        End If
    End Function

    Public Class KeySummary
        Public Key As String
        Public Name As String
        Public NameIndex As Integer
        Public FromGenerator As Boolean
        Public NameSum As NameSummary

        Public Sub Define(ByVal key_ As String, ByVal name_ As String, ByVal fromGenerator_ As Boolean)
            Key = key_
            Name = name_
            FromGenerator = FromGenerator Or fromGenerator_
        End Sub

        Public Overrides Function ToString() As String
            Return Key & ">" & Name
        End Function
    End Class

    Public Class NameSummary
        Public UseCount As Integer
    End Class
End Class
