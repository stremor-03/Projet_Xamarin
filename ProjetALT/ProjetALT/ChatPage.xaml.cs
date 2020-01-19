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

        // Setting up title, icon (for the bottom Bar) and global background of the specific Chat page
        public ChatPage(ObservableCollection<Message> messages)
        {
            this.messages = messages;

            IconImageSource = "chat_icon.png";
            Title = "CHAT";
            BackgroundColor = Color.FromHex("#1A1A1A");

            setView();
        }

        // Set the View of the first Page (chat)
        private void setView()
        {
            // Creating a Button component to ask a Refresh of data (messages)
            var button = new Button
            {
                Text = "Refresh",
                TextColor = Color.AntiqueWhite,
                BorderColor = Color.White,
                BackgroundColor = Color.Black
            };

            button.Clicked += (sender, e) =>
            {
                MainPage.refreshMessages();
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
                    studentIDLabel.TextColor = Color.White;
                    studentIDLabel.FontAttributes = FontAttributes.Bold;



                    // Label for the message
                    Label messageLabel = new Label();
                    messageLabel.SetBinding(Label.TextProperty, "Student_message");
                    messageLabel.TextColor = Color.LightGray;
                    studentIDLabel.FontSize = 20;
                    studentIDLabel.FontFamily = "";


                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Spacing = 1,
                            BackgroundColor = Color.FromHex("#1A1A1A"),
                            Children =
                            {
                                studentIDLabel,
                                messageLabel
                            }
                        }
                    };
                })
            };

            listView.BackgroundColor = Color.DimGray;

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