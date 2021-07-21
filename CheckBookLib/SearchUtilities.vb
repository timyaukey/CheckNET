Option Strict On
Option Explicit On

Public Class SearchUtilities

    Delegate Function PruneMatchesTrx(ByVal objTrx As BaseTrx) As Boolean

    ''' <summary>
    ''' Narrow down the results to one or more BaseTrx in colExactMatches if 
    ''' there is anything in colExactMatches.
    ''' </summary>
    ''' <param name="colExactMatches"></param>
    ''' <param name="colMatches"></param>
    ''' <param name="blnExactMatch"></param>
    ''' <param name="blnTrxPruner"></param>
    ''' <remarks></remarks>
    Public Shared Sub PruneSearchMatches(ByVal colExactMatches As ICollection(Of BankTrx), ByRef colMatches As ICollection(Of BankTrx),
                                  ByRef blnExactMatch As Boolean, ByVal blnTrxPruner As PruneMatchesTrx)
        Dim objPerfectMatch As BankTrx
        Dim datFirstMatch As DateTime
        Dim datLastMatch As DateTime
        Dim blnFirstIteration As Boolean
        Dim objTrx As BankTrx

        'If we have multiple exact matches, see if all are within a range of 5 days
        'and one passes the test of blnTrxPruner(). If so use that one alone as the
        'list of exact matches.
        If colExactMatches.Count() > 1 Then
            objPerfectMatch = Nothing
            blnFirstIteration = True
            For Each objTrx In colExactMatches
                If blnFirstIteration Then
                    datFirstMatch = objTrx.datDate
                    datLastMatch = objTrx.datDate
                    blnFirstIteration = False
                Else
                    If objTrx.datDate < datFirstMatch Then
                        datFirstMatch = objTrx.datDate
                    End If
                    If objTrx.datDate > datLastMatch Then
                        datLastMatch = objTrx.datDate
                    End If
                End If
                If blnTrxPruner(objTrx) Then
                    If objPerfectMatch Is Nothing Then
                        objPerfectMatch = objTrx
                    End If
                    Exit For
                End If
            Next
            If (Not objPerfectMatch Is Nothing) And datLastMatch.Subtract(datFirstMatch).TotalDays <= 2D Then
                colExactMatches = New List(Of BankTrx)
                colExactMatches.Add(objPerfectMatch)
            End If
        End If

        'If have exact matches, return them only.
        If colExactMatches.Count > 0 Then
            colMatches = colExactMatches
            'If we have one exact match, say we have only one.
            If colExactMatches.Count = 1 Then
                blnExactMatch = True
            End If
        End If

    End Sub

    Public Shared Sub PruneToExactMatches(ByVal colExactMatches As ICollection(Of BankTrx), ByVal datDate As Date, ByRef colMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean)

        PruneSearchMatches(colExactMatches, colMatches, blnExactMatch, Function(objTrx As BaseTrx) objTrx.datDate = datDate)

    End Sub

    Public Shared Sub PruneToNonImportedExactMatches(ByVal colExactMatches As ICollection(Of BankTrx), ByVal datDate As Date, ByRef colMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean)

        PruneSearchMatches(colExactMatches, colMatches, blnExactMatch,
                           Function(objTrx As BaseTrx) As Boolean
                               If objTrx.datDate = datDate Then
                                   If objTrx.lngStatus <> BaseTrx.TrxStatus.Reconciled Then
                                       If TypeOf objTrx Is BankTrx Then
                                           If String.IsNullOrEmpty(DirectCast(objTrx, BankTrx).strImportKey) Then
                                               Return True
                                           End If
                                       End If
                                   End If
                               End If
                               Return False
                           End Function)
    End Sub

End Class
