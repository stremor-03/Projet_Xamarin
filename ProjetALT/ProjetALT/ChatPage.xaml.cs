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
        ObservableCollection<Message> messages;

        public ChatPage(ObservableCollection<Message> messages)
        {
            this.messages = messages;

            IconImageSource = "chat_icon.png";
            Title = "Chat";

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
                //getMessages();
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

    }
}