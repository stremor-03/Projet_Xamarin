using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjetALT.src;

namespace ProjetALT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageDetailPage : ContentPage

    {
        Message message;
        private ObservableCollection<Message> messageList;

        public MessageDetailPage(Message message, ObservableCollection<Message> messageList)
        {
            this.message = message;
            this.messageList = messageList;

            Title = this.message.Student_id.ToString() + "'s messages";

            NavigationPage.SetHasBackButton(this, true);

            BackgroundColor = Color.FromHex("#1A1A1A");

            InitializeComponent();
            DisplayContent();
        }

        void DisplayContent()

        {
            ObservableCollection<Message> messageListOfStudent = new ObservableCollection<Message>();
            messageListOfStudent.Where(i => i.Student_id == this.message.Student_id && i.Id != this.message.Id);
            foreach ( Message message in this.messageList.Where(i => i.Student_id == this.message.Student_id && i.Id != this.message.Id)) {
                messageListOfStudent.Add(message);
            };

            string descriptionLabelText = "Other Messages";
            if (messageListOfStudent.Count < 1)
            {
                descriptionLabelText = "No other messages";
            }

            Label studentIDLabel = new Label();
            studentIDLabel.Text = this.message.Student_id.ToString();
            studentIDLabel.TextColor = Color.White;
            studentIDLabel.FontAttributes = FontAttributes.Bold;
            studentIDLabel.FontSize = 20;
            studentIDLabel.FontFamily = "";


            // Label for the message
            Label messageLabel = new Label();
            messageLabel.Text = this.message.Student_message;
            messageLabel.TextColor = Color.LightGray;

            Label descriptionLabel = new Label();
            descriptionLabel.Text = descriptionLabelText;
            descriptionLabel.FontSize = 15;
            descriptionLabel.TextColor = Color.White;
            descriptionLabel.VerticalOptions = LayoutOptions.Center;
            descriptionLabel.HorizontalOptions = LayoutOptions.Center;

            TableView tableView = new TableView
            {
                HeightRequest = 40,
                Root = new TableRoot
                {
                    new TableSection
                    {
                        new SwitchCell
                        {
                            Text = "Favorite",
                            OnColor = Color.Orange
                        }
                    }
                }
            };

            ListView listView = new ListView
            {
                SelectionMode = ListViewSelectionMode.None,
                ItemsSource = messageListOfStudent,

                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Label idLabel = new Label();
                    idLabel.SetBinding(Label.TextProperty, "Student_id");
                    idLabel.TextColor = Color.White;
                    idLabel.FontAttributes = FontAttributes.Bold;
                    idLabel.FontSize = 20;
                    idLabel.FontFamily = "";

                    Label messageLabel2 = new Label();
                    messageLabel2.SetBinding(Label.TextProperty, "Student_message");
                    messageLabel2.TextColor = Color.LightGray;
                    messageLabel2.LineBreakMode = LineBreakMode.TailTruncation;


                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.Center,
                            Spacing = 1,
                            BackgroundColor = Color.FromHex("#1A1A1A"),
                            Children =
                            {
                                messageLabel2
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

            Frame cardFrame = new Frame
            {
                BorderColor = Color.Orange,
                BackgroundColor = Color.FromHex("#1A1A1A"),
                CornerRadius = 5,
                Padding = 8,
                Content = new StackLayout
                {
                    Children =
                    {
                        tableView,
                        new BoxView
                        {
                            Color = Color.Orange,
                            HeightRequest = 2,
                            HorizontalOptions = LayoutOptions.Fill
                        },
                        messageLabel,
                    }
                }
            };


            // Build the page.
            Content = new StackLayout
            {
                Margin = new Thickness(50),
                VerticalOptions = LayoutOptions.Center,
                Spacing = 20,
                Children =
                {
                    cardFrame,
                    descriptionLabel,
                    listView
                }
            };
        }
    }
}