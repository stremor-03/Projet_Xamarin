using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using ProjetALT.src;

namespace ProjetALT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public MapPage(ObservableCollection<Message> messages)
        {
            this.messages = messages;

            IconImageSource = "map_icon.png";
            Title = "Map";

            CustomMap customMap = new CustomMap
            {
                MapType = MapType.Street
            };

            var button = new Button
            {
                Text = "Refresh button",
                BackgroundColor = Color.Gray
            };

            button.Clicked += (sender, e) =>
            {
                MainPage.refreshMessages();
            };

            Position position = new Position(this.messages[0].Gps_lat, this.messages[0].Gps_long);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);

            Map map = new Map(mapSpan);

            Double gps_lat, gps_long;

            foreach (Message message in this.messages)
            {
                // Label for student id
                Label studentIDLabel = new Label();
                studentIDLabel.SetBinding(Label.TextProperty, "Student_id");

                // Label for the message
                Label messageLabel = new Label();
                messageLabel.SetBinding(Label.TextProperty, "Student_message");

                var rand = new Random();

                Double random_num_lat = .00065 * rand.Next(1, 10);
                Double random_num_lng = .00065 * rand.Next(1, 10);

                gps_lat = message.Gps_lat + random_num_lat;
                gps_long = message.Gps_long + random_num_lng;

                Position pos = new Position(gps_lat, gps_long);


                map.Pins.Add(new Pin
                {
                    Label = message.Student_message.ToString(),
                    Address = message.Student_id.ToString(),
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

    }
}