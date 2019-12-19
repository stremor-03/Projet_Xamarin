﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Forms;
using System.Json;
using ProjetALT.src;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjetALT
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Message> messages = new ObservableCollection<Message>();

        public MainPage()
        {
            setRefreshAuto(5);
            setView();
        }

        private void setView()
        {
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
                    Label studentIDLabel = new Label();
                    studentIDLabel.SetBinding(Label.TextProperty, "Student_id");

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

        private void setRefreshAuto(int second)
        {
            _ = new System.Threading.Timer((e) => getMessages(), null, TimeSpan.Zero, TimeSpan.FromSeconds(second));
        }

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

            if (size > 0) {
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
                    size = !firstRun ? 0 : messages.Count;
                    messages.Insert(size,message);
                    Console.WriteLine("new ID " + message.Id);
                }
            }

            Console.WriteLine("Update !");
        }
    }
}

