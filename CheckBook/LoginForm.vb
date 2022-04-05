Option Strict Off
Option Explicit On

Friend Class LoginForm
	Inherits System.Windows.Forms.Form
	
	Private mblnCancel As Boolean
	Private mstrLogin As String
	Private mstrPassword As String

	Public Function blnGetCredentials(ByRef strLogin As String, ByRef strPassword As String, ByVal frmStartup As StartupForm) As Boolean
		mblnCancel = True
		frmStartup.PositionBelow(Me)
		Me.ShowDialog()
		strLogin = mstrLogin
		strPassword = mstrPassword
		blnGetCredentials = Not mblnCancel
	End Function

	Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
		Me.Close()
	End Sub
	
	Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
		mstrLogin = Trim(txtLogin.Text)
		mstrPassword = Trim(txtPassword.Text)
		mblnCancel = False
		Me.Close()
	End Sub
End Class