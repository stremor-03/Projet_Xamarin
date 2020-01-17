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
    public partial class ChatPage : ContentPage
    {
        ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public ChatPage()
        {
            IconImageSource = "chat_icon.png";
            Title = "Chat";

            setRefreshAuto(5);
            setView();
        }

        // Set the view of the first page
        private void setView()
        {
            // Button to ask a refresh of data
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

            // Create the ListView.
            ListView listView = new ListView
            {
                ItemsSource = this.messages,

                ItemTemplate = new DataTemplate(() =>
                {
                    // Label for student id
                    Label studentIDLabel = new Label();
                    studentIDLabel.SetBinding(Label.TextProperty, "Student_id");

                    // Label for the message
                    Label messageLabel = new Label();
                    messageLabel.SetBinding(Label.TextProperty, "Student_message");

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Spacing = 0,
                            Children =
                            {
                                studentIDLabel,
                                messageLabel
                            }
                        }
                    };
                })
            };

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    this.Padding = new Thickness(10, 50, 10, 5);
                    break;
                default:
                    this.Padding = new Thickness(10, 0, 10, 5);
                    break;
            }

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    button,
                    listView
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