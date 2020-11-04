Option Strict Off
Option Explicit On

Module UTMisc
	
	Public gstrUTTestTitle As String
	Public gstrUTSubTest As String
	
	Public Sub gUTSetTestTitle(ByVal strTitle As String)
		gstrUTTestTitle = strTitle
		gstrUTSubTest = ""
	End Sub
	
	Public Sub gUTSetSubTest(ByVal strSubTest As String)
		gstrUTSubTest = strSubTest
	End Sub

	Public Sub gUTAssert(ByVal blnCondition As Boolean, ByVal strFailMsg As String)
		Assert.IsTrue(blnCondition, strFailMsg)
		'If Not blnCondition Then
		'	gUTFailure(strFailMsg)
		'End If
	End Sub

	Public Sub gUTFailure(ByVal strFailMsg As String)
		Assert.Fail(strFailMsg)
		'MsgBox("Test failed:" & vbCrLf & "Test: " & gstrUTTestTitle & vbCrLf & "Subtest: " & gstrUTSubTest & vbCrLf & strFailMsg)
	End Sub

	Public Function gobjUTNewReg(Optional ByVal strRegisterKey As String = "1") As UTRegister
		Dim objUTReg As UTRegister
		objUTReg = New UTRegister
		objUTReg.Init(strRegisterKey)
		gobjUTNewReg = objUTReg
	End Function
End Module