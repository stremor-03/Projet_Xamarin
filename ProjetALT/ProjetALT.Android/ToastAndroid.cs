using Android.App;
using Android.Widget;
using ProjetALT.Droid;
using ProjetALT.src;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAndroid))]
namespace ProjetALT.Droid
{
    public class ToastAndroid : IToast
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}
