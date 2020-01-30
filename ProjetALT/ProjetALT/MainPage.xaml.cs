using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ProjetALT.src;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace ProjetALT
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        static ObservableCollection<Message> messages = new ObservableCollection<Message>();

        // Defines the refresh logic and add Chat and Map elements to the Bottom navigation bar
        public MainPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            refreshMessages();
            Children.Add(new MapPage(messages));
            Children.Add(new ChatPage(messages));
            BarTextColor = Color.AntiqueWhite;
            BarBackgroundColor = Color.FromHex("#1A1A1A");
            

            setRefreshAuto(5);
        }

        // Auto data refresh each 'second'
        private static void setRefreshAuto(int second)
        {
            _ = new System.Threading.Timer((e) => refreshMessages(), null, TimeSpan.Zero, TimeSpan.FromSeconds(second));
        }

        // Collect data from the server and store it in 'messages'
        public static void refreshMessages()
        {
            string url = "https://hmin309-embedded-systems.herokuapp.com/message-exchange/messages/";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.GetEncoding("utf-8");

            ObservableCollection<Message> result = null;

            int size = messages.Count;
            bool firstRun = true;

            if (size > 0)
            {
                firstRun = false;
            }

            using (StreamReader translatedStream = new StreamReader(stream, encode))
            {
                string line;

                while ((line = translatedStream.ReadLine()) != null)
                {
                    result = JsonConvert.DeserializeObject<ObservableCollection<Message>>(line);
                }
            }


            foreach (Message message in result.ToList())
            {
                if (!messages.Contains(message))
                {
                    int index = !firstRun ? 0 : messages.Count;
                    messages.Insert(index, message);
                }
            }

            Console.WriteLine("Update !");
        }
    }

}

