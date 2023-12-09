namespace Services.PlatformSpecific
{
	internal enum LoginState
	{
		None,
		NewFacebookLogin,
		NewGoogleLogin,
		FacebookLinking,
		GoogleLinking,
		GoogleLogin,
		FacebookLogin
	}
}
