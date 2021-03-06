Option Strict On
Option Explicit On

''' <summary>
''' Authentication and authorization tools.
''' Use of these tools is voluntary. This system may be used without actually 
''' authenticating the user or checking their authorization.
''' </summary>

Public Class Security

    Private mobjCompany As Company
    Private mstrFilePath As String
    Private mdomSecurity As VB6XmlDocument
    Private melmUser As VB6XmlElement
    Private mstrLoginSaved As String
    Private mstrLogin As String
    Private mblnNoFile As Boolean

    Public Sub New(ByVal objCompany As Company)
        mobjCompany = objCompany
    End Sub

    Private Sub Init()
        'UPGRADE_NOTE: Object melmUser may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        melmUser = Nothing
        mstrLogin = ""
        mblnNoFile = False
    End Sub

    Public ReadOnly Property strAdminLogin() As String
        Get
            Return "admin"
        End Get
    End Property

    Public Sub Load()
        Init()
        MakePath()
        If Dir(mstrFilePath) = "" Then
            mblnNoFile = True
            mstrLogin = "anonymous"
            Return
        End If
        mdomSecurity = mobjCompany.domLoadFile(mstrFilePath)
        If mdomSecurity.DocumentElement.Name <> "security" Then
            gRaiseError("Invalid security file document element")
        End If
    End Sub

    Public Sub CreateEmpty()
        Init()
        MakePath()
        mdomSecurity = New VB6XmlDocument
        mdomSecurity.LoadXml("<security>" & vbCrLf & "</security>")
        mstrLogin = ""
    End Sub

    Private Sub MakePath()
        mstrFilePath = mobjCompany.strAddPath("Security.xml")
    End Sub

    Public ReadOnly Property blnNoFile() As Boolean
        Get
            Return mblnNoFile
        End Get
    End Property

    Public ReadOnly Property blnHaveUser() As Boolean
        Get
            If melmUser Is Nothing Then
                blnHaveUser = False
            Else
                blnHaveUser = True
            End If
        End Get
    End Property

    Public ReadOnly Property strLogin() As String
        Get
            strLogin = mstrLogin
        End Get
    End Property

    Public ReadOnly Property blnIsAdministrator() As Boolean
        Get
            blnIsAdministrator = (mstrLogin = "admin") Or blnUserBoolean("isadmin") Or mblnNoFile
        End Get
    End Property

    'Strictly for debugging use
    Public ReadOnly Property elmDbgUser() As VB6XmlElement
        Get
            elmDbgUser = melmUser
        End Get
    End Property

    Public Sub CreateSignatures()
        Dim colUsers As VB6XmlNodeList
        Dim elmUser As VB6XmlElement
        Dim strSignature As String

        colUsers = mdomSecurity.DocumentElement.SelectNodes("user")
        For Each elmUser In colUsers
            strSignature = strCreateUserSignature(elmUser)
            elmUser.SetAttribute("signhash", strSignature)
        Next elmUser
    End Sub

    Private Function strCreateUserSignature(ByVal elmUser As VB6XmlElement) As String
        strCreateUserSignature = strMakeHash(CStr(elmUser.GetAttribute("login")) & CStr(elmUser.GetAttribute("name")) & ":isadmin=" & CStr(elmUser.GetAttribute("isadmin")))
    End Function

    Public Sub Save()
        mdomSecurity.Save(mstrFilePath)
    End Sub

    Public Sub CreateUser(ByVal strLogin As String, ByVal strName As String)
        melmUser = mdomSecurity.CreateElement("user")
        melmUser.SetAttribute("login", strLogin)
        melmUser.SetAttribute("name", strName)
        mdomSecurity.DocumentElement.AppendChild(melmUser)
        Dim objText As VB6XmlText
        objText = mdomSecurity.CreateTextNode(vbCrLf)
        mdomSecurity.DocumentElement.AppendChild(objText)
        mstrLogin = strLogin
    End Sub

    Public Function blnFindUser(ByVal strLogin As String) As Boolean
        melmUser = DirectCast(mdomSecurity.DocumentElement.SelectSingleNode("user[@login=""" & strLogin & """]"), VB6XmlElement)
        mstrLogin = strLogin
        blnFindUser = blnHaveUser
    End Function

    Public Function blnUserSignatureIsValid() As Boolean
        Dim strComputedSignature As String
        strComputedSignature = strCreateUserSignature(melmUser)
        blnUserSignatureIsValid = (strComputedSignature = CStr(melmUser.GetAttribute("signhash")))
    End Function

    Public Function blnPasswordMatches(ByVal strPassword As String) As Boolean
        blnPasswordMatches = (CStr(melmUser.GetAttribute("passwordhash")) = strMakeHash(strPassword))
    End Function

    Public Function blnAuthenticate(ByVal strLogin As String, ByVal strPassword As String) As Boolean
        If Not blnFindUser(strLogin) Then
            Return False
        End If
        If Not blnPasswordMatches(strPassword) Then
            Return False
        End If
        If Not blnUserSignatureIsValid() Then
            Return False
        End If
        Return True
    End Function

    Public Sub SetPassword(ByVal strPassword As String)
        melmUser.SetAttribute("passwordhash", strMakeHash(strPassword))
        Dim strSignature As String = strCreateUserSignature(melmUser)
        melmUser.SetAttribute("signhash", strSignature)
    End Sub

    Public Sub DeleteUser()
        mdomSecurity.DocumentElement.RemoveChild(melmUser)
    End Sub

    Private Function blnUserBoolean(ByVal strAttrib As String) As Boolean
        Dim strValue As String
        If blnHaveUser Then
            strValue = LCase(strUserValue(strAttrib))
            blnUserBoolean = (strValue = "yes") Or (strValue = "true")
        End If
    End Function

    Private Function strUserValue(ByVal strAttrib As String) As String
        Dim vstrValue As Object
        vstrValue = melmUser.GetAttribute(strAttrib)
        If gblnXmlAttributeMissing(vstrValue) Then
            strUserValue = ""
        Else
            strUserValue = CStr(vstrValue)
        End If
    End Function

    Private Function strMakeHash(ByVal strInput As String) As String
        Dim lngHash As Integer
        Dim intIndex As Short
        Dim strChar As String
        lngHash = 1
        intIndex = 1
        Do
            If intIndex > Len(strInput) Then
                Exit Do
            End If
            strChar = Mid(strInput, intIndex, 1)
            intIndex = intIndex + 1S
            lngHash = (lngHash + 1) * (Asc(strChar) + 1)
            lngHash = lngHash And &HFFFFFF
        Loop
        strMakeHash = Hex(lngHash)
    End Function

    Public Sub SaveUserContext()
        mstrLoginSaved = mstrLogin
    End Sub

    Public Sub RestoreUserContext()
        blnFindUser(mstrLoginSaved)
    End Sub
End Class