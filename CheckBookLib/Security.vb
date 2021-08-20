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
    Private mdomSecurity As CBXmlDocument
    Private melmUser As CBXmlElement
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

    Public ReadOnly Property AdminLogin() As String
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
        mdomSecurity = mobjCompany.LoadXmlFile(mstrFilePath)
        If mdomSecurity.DocumentElement.Name <> "security" Then
            RaiseErrorMsg("Invalid security file document element")
        End If
    End Sub

    Public Sub CreateEmpty()
        Init()
        MakePath()
        mdomSecurity = New CBXmlDocument
        mdomSecurity.LoadXml("<security>" & vbCrLf & "</security>")
        mstrLogin = ""
    End Sub

    Private Sub MakePath()
        mstrFilePath = mobjCompany.AddNameToDataPath("Security.xml")
    End Sub

    Public ReadOnly Property NoFile() As Boolean
        Get
            Return mblnNoFile
        End Get
    End Property

    Public ReadOnly Property HaveUser() As Boolean
        Get
            If melmUser Is Nothing Then
                HaveUser = False
            Else
                HaveUser = True
            End If
        End Get
    End Property

    Public ReadOnly Property LoginName() As String
        Get
            LoginName = mstrLogin
        End Get
    End Property

    Public ReadOnly Property IsAdministrator() As Boolean
        Get
            IsAdministrator = (mstrLogin = "admin") Or UserAttributeIsTrue("isadmin") Or mblnNoFile
        End Get
    End Property

    'Strictly for debugging use
    Public ReadOnly Property DbgUserXmlElement() As CBXmlElement
        Get
            DbgUserXmlElement = melmUser
        End Get
    End Property

    Public Sub CreateSignatures()
        Dim colUsers As CBXmlNodeList
        Dim elmUser As CBXmlElement
        Dim strSignature As String

        colUsers = mdomSecurity.DocumentElement.SelectNodes("user")
        For Each elmUser In colUsers
            strSignature = CreateUserSignature(elmUser)
            elmUser.SetAttribute("signhash", strSignature)
        Next elmUser
    End Sub

    Private Function CreateUserSignature(ByVal elmUser As CBXmlElement) As String
        CreateUserSignature = MakeHash(CStr(elmUser.GetAttribute("login")) & CStr(elmUser.GetAttribute("name")) & ":isadmin=" & CStr(elmUser.GetAttribute("isadmin")))
    End Function

    Public Sub Save()
        mdomSecurity.Save(mstrFilePath)
    End Sub

    Public Sub CreateUser(ByVal strLogin As String, ByVal strName As String)
        melmUser = mdomSecurity.CreateElement("user")
        melmUser.SetAttribute("login", strLogin)
        melmUser.SetAttribute("name", strName)
        mdomSecurity.DocumentElement.AppendChild(melmUser)
        Dim objText As CBXmlText
        objText = mdomSecurity.CreateTextNode(vbCrLf)
        mdomSecurity.DocumentElement.AppendChild(objText)
        mstrLogin = strLogin
    End Sub

    Public Function FindUser(ByVal strLogin As String) As Boolean
        melmUser = DirectCast(mdomSecurity.DocumentElement.SelectSingleNode("user[@login=""" & strLogin & """]"), CBXmlElement)
        mstrLogin = strLogin
        FindUser = HaveUser
    End Function

    Public Function IsUserSignatureValid() As Boolean
        Dim strComputedSignature As String
        strComputedSignature = CreateUserSignature(melmUser)
        IsUserSignatureValid = (strComputedSignature = CStr(melmUser.GetAttribute("signhash")))
    End Function

    Public Function PasswordMatches(ByVal strPassword As String) As Boolean
        PasswordMatches = (CStr(melmUser.GetAttribute("passwordhash")) = MakeHash(strPassword))
    End Function

    Public Function Authenticate(ByVal strLogin As String, ByVal strPassword As String) As Boolean
        If Not FindUser(strLogin) Then
            Return False
        End If
        If Not PasswordMatches(strPassword) Then
            Return False
        End If
        If Not IsUserSignatureValid() Then
            Return False
        End If
        Return True
    End Function

    Public Sub SetPassword(ByVal strPassword As String)
        melmUser.SetAttribute("passwordhash", MakeHash(strPassword))
        Dim strSignature As String = CreateUserSignature(melmUser)
        melmUser.SetAttribute("signhash", strSignature)
    End Sub

    Public Sub DeleteUser()
        mdomSecurity.DocumentElement.RemoveChild(melmUser)
    End Sub

    Private Function UserAttributeIsTrue(ByVal strAttrib As String) As Boolean
        Dim strValue As String
        If HaveUser Then
            strValue = LCase(GetUserAttributeValue(strAttrib))
            UserAttributeIsTrue = (strValue = "yes") Or (strValue = "true")
        End If
    End Function

    Private Function GetUserAttributeValue(ByVal strAttrib As String) As String
        Dim vstrValue As Object
        vstrValue = melmUser.GetAttribute(strAttrib)
        If gblnXmlAttributeMissing(vstrValue) Then
            GetUserAttributeValue = ""
        Else
            GetUserAttributeValue = CStr(vstrValue)
        End If
    End Function

    Private Function MakeHash(ByVal strInput As String) As String
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
        MakeHash = Hex(lngHash)
    End Function

    Public Sub SaveUserContext()
        mstrLoginSaved = mstrLogin
    End Sub

    Public Sub RestoreUserContext()
        FindUser(mstrLoginSaved)
    End Sub
End Class