﻿Option Strict On
Option Explicit On

<TestFixture>
Public Class ImportFixture
    Private mobjCompany As Company

    <Test>
    Public Sub TestImportUpdateBank()
        Dim objTrx As Trx
        Dim objUTReg As UTRegister

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/3/2000#, -25D, "Add1", 1, 1, 1, strDescription:="Descr1", blnFake:=True, blnAutoGenerated:=True)
            .AddNormal("101", #4/4/2000#, -30.2D, "Add2", 2, 2, 2, strDescription:="Descr2", vcurAmount2:=-10D)
            .AddNormal("Pmt", #4/5/2000#, -20.99D, "Add3", 3, 3, 3, strDescription:="Descr3")
        End With

        gUTSetSubTest("Test first import")

        objUTReg.objReg.ImportUpdateBank(objUTReg.objReg.objNormalTrx(1), #4/3/2001#, "200", -25D, "importkey-1")
        objTrx = objUTReg.objReg.objTrx(1)
        With DirectCast(objTrx, NormalTrx)
            gUTAssert(.datDate = #4/3/2000#, "Bad date")
            gUTAssert(.strNumber = "200", "Bad number")
            gUTAssert(.curAmount = -25D, "Bad amount")
            gUTAssert(.strImportKey = "importkey-1", "Bad import key")
            gUTAssert(.blnFake = False, "Bad blnFake")
            gUTAssert(.blnAutoGenerated = False, "Bad auto generated")
        End With

        gUTSetSubTest("Test second import")

        objUTReg.objReg.ImportUpdateBank(objUTReg.objReg.objNormalTrx(2), #4/4/2000#, "201", -50D, "importkey-2")
        objTrx = objUTReg.objReg.objTrx(2)
        With objTrx
            gUTAssert(.datDate = #4/4/2000#, "Bad date")
            gUTAssert(.strNumber = "201", "Bad number")
            gUTAssert(.curAmount = -50D, "Bad amount " & .curAmount)
            gUTAssert(.blnFake = False, "Bad blnFake")
            gUTAssert(.blnAutoGenerated = False, "Bad auto generated")
        End With

        gUTSetSubTest("Test third import")

        objUTReg.objReg.ImportUpdateBank(objUTReg.objReg.objNormalTrx(3), #4/15/2002#, "Pmt", -40.01D, "importkey-3")
        objTrx = objUTReg.objReg.objTrx(3)
        With objTrx
            gUTAssert(.datDate = #4/15/2002#, "Bad date")
            gUTAssert(.strNumber = "Pmt", "Bad number")
            gUTAssert(.curAmount = -40.01, "Bad amount " & .curAmount)
        End With

    End Sub

    <OneTimeSetUp>
    Public Sub OneTimeSetup()
        mobjCompany = gobjUTStandardSetup()
    End Sub

    <OneTimeTearDown>
    Public Sub OneTimeTearDown()
        gUTStandardTearDown(mobjCompany)
    End Sub

End Class
