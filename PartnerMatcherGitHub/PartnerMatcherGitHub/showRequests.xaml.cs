using partnersMatcherPart4.Controller;
using partnersMatcherPart4.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for showRequests.xaml
    /// </summary>
    public partial class showRequests : Window
    {

        List<Request> requests;
        MyController controller;
        string postID;
        string requestContent;
        int rank;
        string status;

        public showRequests(MyController controler, List<Request> requests)
        {
            InitializeComponent();
            this.controller = controler;
            this.requests = requests;
            creatRequestsList();
            CenterWindowOnScreen();

        }

        public void creatRequestsList()
        {
            System.Windows.Forms.ListView l = new System.Windows.Forms.ListView();

            l.Columns.Add("Post id", 100);
            l.Columns.Add("status", 100);
            l.Columns.Add("rank", 100);



            ListView lv = new ListView();
            lv.Width = 300;
            lv.Height = 200;
            GridView gridView = new GridView();
            lv.View = gridView;

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Post id",
                DisplayMemberBinding = new Binding("postId")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "status",
                DisplayMemberBinding = new Binding("status")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "rank",
                DisplayMemberBinding = new Binding("rank")
            });


            foreach (var request in requests)
            {
                requestToshow requestToShow = new requestToshow { postId = request.postID, status = request.status, rank = request.rank };
                lv.Items.Add(requestToShow);
            }


            //lv.ItemsSource = requests;

            this.canvas.Children.Add(lv);
            Canvas.SetLeft(lv, 20);
            Canvas.SetTop(lv, 50);
            Label label = new Label();
            label.Content = "post id: ";
            TextBox tb = new TextBox();
            tb.Width = 100;
            tb.TextChanged += postID_TextChanged;
            this.canvas.Children.Add(label);
            Canvas.SetLeft(label, 50);
            Canvas.SetTop(label, 270);
            this.canvas.Children.Add(tb);
            Canvas.SetLeft(tb, 150);
            Canvas.SetTop(tb, 275);

            ComboBox comboStatus = new ComboBox();
            comboStatus.Width = 100;
            comboStatus.ItemsSource = new List<string>() { "aproved", "denied" };
            comboStatus.SelectionChanged += status_SelctionChanged;
            comboStatus.SelectedItem = "aproved";
            comboStatus.ToolTip = " Status ";
            this.canvas.Children.Add(comboStatus);
            Canvas.SetLeft(comboStatus, 50);
            Canvas.SetTop(comboStatus, 310);

            ComboBox comboRank = new ComboBox();
            comboRank.Width = 100;
            comboRank.ItemsSource = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            comboRank.SelectionChanged += rank_SelctionChanged;
            comboRank.SelectedItem = 5;
            comboRank.ToolTip = " Rank ";
            this.canvas.Children.Add(comboRank);
            Canvas.SetLeft(comboRank, 160);
            Canvas.SetTop(comboRank, 310);


            Button button = new Button();
            button.Name = "button";
            button.Width = 100;
            button.Click += button_click;
            button.Content = "update status";
            this.canvas.Children.Add(button);
            Canvas.SetLeft(button, 110);
            Canvas.SetTop(button, 370);

        }


        private void button_click(object sender, RoutedEventArgs e)
        {
            if (postID != "" && postID.All(c => Char.IsDigit(c)) && requests.Any(r => r.postID == Convert.ToInt32(postID)))
            {
                try
                {
                    string senderID = requests.FirstOrDefault(r => r.postID == Convert.ToInt32(postID)).senderID;
                    controller.setRequestStatus(senderID, Convert.ToInt32(postID), rank, status);
                    MessageBox.Show("The Status changed succesfully!", "Partner Matcher");
                }
                catch (Exception)
                {

                }
            }
            else
                MessageBox.Show("The Status change Failed, check the post ID", "Partner Matcher");


        }


        private void postID_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ... Get control that raised this event.
            var textBox = sender as TextBox;
            // ... Change Window Title.
            this.postID = textBox.Text;

        }

        private void status_SelctionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get control that raised this event.
            var combo = sender as ComboBox;
            // ... Change Window Title.
            this.status = combo.SelectedItem.ToString();

        }

        private void rank_SelctionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get control that raised this event.
            var combo = sender as ComboBox;
            // ... Change Window Title.
            this.rank = Convert.ToInt32(combo.SelectedItem);

        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
