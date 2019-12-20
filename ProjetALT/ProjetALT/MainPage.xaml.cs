using Xamarin.Forms;

namespace ProjetALT
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            Children.Add(new ChatPage());
            Children.Add(new MapPage());
        }
    }
}

