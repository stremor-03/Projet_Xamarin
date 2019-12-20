using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Newtonsoft.Json;
using ProjetALT.src;

namespace ProjetALT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public MapPage()
        {
			IconImageSource = "map.png";
			Title = "Map";

            var button = new Button
            {
                Text = "Refresh button",
                BackgroundColor = Color.Gray
            };
            button.Clicked += (sender, e) =>
            {
                getMessages();
                Console.WriteLine("Refesh cliked => " + this.messages.Count);
            };

            Position position = new Position(43.6312, 3.861477);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);

            Map map = new Map(mapSpan);

            foreach(Message message in messages)
            {
                // Label for student id
                Label studentIDLabel = new Label();
                studentIDLabel.SetBinding(Label.TextProperty, "Student_id");

                // Label for the message
                Label messageLabel = new Label();
                messageLabel.SetBinding(Label.TextProperty, "Student_message");

                Double gps_lat = message.Gps_lat;
                Double gps_long = message.Gps_long;

                Console.WriteLine("Lat : "+gps_lat+"/ Long : "+ gps_long);

                Position pos = new Position(gps_lat, gps_long);


                map.Pins.Add(new Pin
                {
                    Label = studentIDLabel.ToString(),
                    Address = messageLabel.ToString(),
                    Type = PinType.Place,
                    Position = pos
                });
            }


            Content = new StackLayout
            {
                Margin = new Thickness(10),
                Children =
                {
                    map,
                    button
                }
            };

        }

        // Auto data refresh each 'second'
        private void setRefreshAuto(int second)
        {
            _ = new System.Threading.Timer((e) => getMessages(), null, TimeSpan.Zero, TimeSpan.FromSeconds(second));
        }

        // Collect data from the server and store it in 'messages'
        private void getMessages()
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