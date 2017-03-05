Option Strict On
Option Explicit On

Public Class SequencedTrx

    Public datDate As Date
    Public curAmount As Decimal
    Public intRepeatSeq As Integer
    Public blnSkip As Boolean

    Public Sub Init(ByVal datDate_ As Date, ByVal curAmount_ As Decimal, ByVal intRepeatSeq_ As Integer)
        datDate = datDate_
        curAmount = curAmount_
        intRepeatSeq = intRepeatSeq_
        blnSkip = False
    End Sub
End Class