Option Strict On
Option Explicit On
Public Class RegCursor

    'A cursor for navigating through a Register and returning Trx from it.
    'Keeps correct current position when Trx inserted or deleted before the
    'current Trx in the Register. If you delete the current Trx the next
    'Trx becomes current. If you delete the last Trx you move past the end
    'of the Register.

    Private WithEvents mobjReg As Register
    Private mlngIndex As Integer
    Private mlngCount As Integer

    '$Description Initialize a new RegCursor. Must be the first member used.
    '   Cursor will be positioned before first Trx.

    Public Sub Init(ByVal objReg As Register)
        mobjReg = objReg
        mlngCount = mobjReg.lngTrxCount
        mlngIndex = 0
    End Sub

    Public Sub OnlyCloneCallsThisSetIndex(ByVal lngIndex As Integer)
        mlngIndex = lngIndex
    End Sub

    '$Description Clone the current RegCursor. Creates a new object initially
    '   positioned on the same Trx in the same Register, but which can be
    '   moved indepedently.

    Public Function objClone() As RegCursor
        Dim objCursor As RegCursor
        objCursor = New RegCursor
        objCursor.Init(mobjReg)
        objCursor.OnlyCloneCallsThisSetIndex(mlngIndex)
        objClone = objCursor
    End Function

    '$Description The Register accessed by this RegCursor.

    Public ReadOnly Property objReg() As Register
        Get
            objReg = mobjReg
        End Get
    End Property

    '$Description True iff the cursor is positioned on a Trx, e.g. you
    '   didn't just call MoveBeforeFirst(), MoveAfterLast(), or step past
    '   the last Trx or before the first Trx.

    Public ReadOnly Property blnHasCurrent() As Object
        Get
            blnHasCurrent = (mlngIndex > 0) And (mlngIndex <= mlngCount)
        End Get
    End Property

    '$Description Return the current Trx, or Nothing if positioned before the
    '   first Trx or after the last Trx.

    Public ReadOnly Property objCurrent() As Object
        Get
            If mlngIndex > 0 And mlngIndex <= mlngCount Then
                objCurrent = mobjReg.objTrx(mlngIndex)
            Else
                'UPGRADE_NOTE: Object objCurrent may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                objCurrent = Nothing
            End If
        End Get
    End Property

    '$Description Move cursor to before the first Trx in Register.
    '   Works even if Register is empty. If the cursor position is not
    '   changed, then the following are true: (blnHasCurrent = False),
    '   (objGetCurrent Is Nothing).

    Public Sub MoveBeforeFirst()
        mlngIndex = 0
    End Sub

    '$Description Move cursor to after the last Trx in Register.
    '   Like MoveBeforeFirst() but for the other end of the Register.

    Public Sub MoveAfterLast()
        mlngIndex = mlngCount + 1
    End Sub

    '$Description Step to the next Trx. Will step to the first Trx the
    '   first time called after calling MoveBeforeFirst(). Call blnHasCurrent
    '   afterward to determine if MoveNext() moved past the last Trx.

    Public Sub MoveNext()
        If mlngIndex <= mlngCount Then
            mlngIndex = mlngIndex + 1
        End If
    End Sub

    '$Description Like MoveNext(), but returns Nothing if moved past the
    '   last Trx otherwise returns the new current Trx.

    Public Function objGetNext() As Trx
        If mlngIndex < mlngCount Then
            mlngIndex = mlngIndex + 1
            objGetNext = mobjReg.objTrx(mlngIndex)
        Else
            If mlngIndex = mlngCount Then
                mlngIndex = mlngIndex + 1
            End If
            'UPGRADE_NOTE: Object objGetNext may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objGetNext = Nothing
        End If
    End Function

    '$Description Like MoveNext() but in the opposite direction.

    Public Sub MovePrev()
        If mlngIndex > 0 Then
            mlngIndex = mlngIndex - 1
        End If
    End Sub

    '$Description Like objGetNext() but in the opposite direction.

    Public Function objGetPrev() As Trx
        If mlngIndex > 1 Then
            mlngIndex = mlngIndex - 1
            objGetPrev = mobjReg.objTrx(mlngIndex)
        Else
            If mlngIndex = 1 Then
                mlngIndex = mlngIndex - 1
            End If
            'UPGRADE_NOTE: Object objGetPrev may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objGetPrev = Nothing
        End If
    End Function

    Private Sub mobjReg_TrxAdded(ByVal lngIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxAdded
        mlngCount = mobjReg.lngTrxCount
        If lngIndex <= mlngIndex Then
            mlngIndex = mlngIndex + 1
        End If
    End Sub

    Private Sub mobjReg_TrxDeleted(ByVal lngIndex As Integer) Handles mobjReg.TrxDeleted
        mlngCount = mobjReg.lngTrxCount
        If lngIndex < mlngIndex Then
            mlngIndex = mlngIndex - 1
        Else
            If mlngIndex > (mlngCount + 1) Then
                mlngIndex = mlngCount
            End If
        End If
    End Sub
End Class