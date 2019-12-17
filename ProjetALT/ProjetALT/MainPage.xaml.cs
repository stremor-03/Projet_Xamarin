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

namespace ProjetALT
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            string url = "https://hmin309-embedded-systems.herokuapp.com/message-exchange/messages/";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            Encoding encode = Encoding.GetEncoding("utf-8");

            List<Message> messages = null;

            using (StreamReader translatedStream = new StreamReader(stream, encode))
            {
                string line;

                while ((line = translatedStream.ReadLine()) != null)
                {
                    messages = JsonConvert.DeserializeObject<List<Message>>(line);
                }
            }

            // Create the ListView.
            ListView listView = new ListView
            {
                // Source of data items.
                ItemsSource = messages,

                // Define template for displaying each item.
                // (Argument of DataTemplate constructor is called for 
                //      each item; it must return a Cell derivative.)
                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Label studentIDLabel = new Label();
                    studentIDLabel.SetBinding(Label.TextProperty, "Student_id");

                    Label messageLabel = new Label();
                    messageLabel.SetBinding(Label.TextProperty,"Student_message");

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
                    this.Padding = new Thickness(10, 20, 10, 5);
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
                    listView
                }
            };
        }
    }
}

