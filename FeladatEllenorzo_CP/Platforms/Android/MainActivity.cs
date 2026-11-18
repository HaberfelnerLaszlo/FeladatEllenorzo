using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using Microsoft.Identity.Client;

using System.Diagnostics;

//régi
//[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density )]
////public class MsalActivity : BrowserTabActivity
////{
////}
//public class MainActivity : MauiAppCompatActivity
//{
//	protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
//	{
//		base.OnActivityResult(requestCode, resultCode, data);
//		// Return control to MSAL
//		AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
//	}
//}
//   using Android.App;
//using Android.Content;
//using Android.Content.PM;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;

[Activity(
    Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Window?.SetDecorFitsSystemWindows(true);
    }

    protected override void OnActivityResult(
        int requestCode,
        [GeneratedEnum] Result resultCode,
        Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(
            requestCode,
            resultCode,
            data);
    }
}