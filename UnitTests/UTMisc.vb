Option Strict Off
Option Explicit On

Module UTMisc

	Public gstrUTSubTest As String

	Public Sub gUTSetSubTest(ByVal strSubTest As String)
		gstrUTSubTest = strSubTest
	End Sub

	Public Sub gUTAssert(ByVal blnCondition As Boolean, ByVal strFailMsg As String)
		Assert.IsTrue(blnCondition, gstrUTSubTest + ":: " + strFailMsg)
	End Sub

	Public Sub gUTFailure(ByVal strFailMsg As String)
		Assert.Fail(gstrUTSubTest + ":: " + strFailMsg)
	End Sub

	Public Function gobjUTStandardSetup() As Company
		Dim strDataPathValue As String = My.Application.Info.DirectoryPath & "\..\..\Data"
		Dim objCompany As Company = New Company(strDataPathValue)
		CompanyLoader.LoadGlobalLists(objCompany)
		objCompany.SecData.CreateEmpty()
		Return objCompany
	End Function

	Public Sub gUTStandardTearDown(ByVal objCompany As Company)
		objCompany.Teardown()
	End Sub

	Public Function gobjUTNewReg(Optional ByVal strRegisterKey As String = "1") As UTRegister
		Dim objUTReg As UTRegister
		objUTReg = New UTRegister
		objUTReg.Init(strRegisterKey)
		gobjUTNewReg = objUTReg
	End Function
End Module