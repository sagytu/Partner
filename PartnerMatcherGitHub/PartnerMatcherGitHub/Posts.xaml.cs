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
using partnersMatcherPart4.Model;
using partnersMatcherPart4.Controller;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Posts.xaml
    /// </summary>
    public partial class Posts : Window
    {
        bool request;
        bool publish;
        Dictionary<int, Post> posts;
        MyController controller;
        string postID;
        string requestContent;

        /// <summary>
        /// construcor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="publish"></param>
        /// <param name="controler"></param>
        /// <param name="posts"></param>
        public Posts(bool request, bool publish, MyController controler, Dictionary<int, Post> posts)
        {
            InitializeComponent();
            this.request = request;
            this.publish = publish;
            this.controller = controler;
            this.posts = posts;
            createListForPosts();
            CenterWindowOnScreen();

        }

        /// <summary>
        /// create the posts window - get the posts to show from controller
        /// </summary>
        public void createListForPosts()
        {
            System.Windows.Forms.ListView l = new System.Windows.Forms.ListView();
            l.Columns.Add("ID", 100);
            l.Columns.Add("Title", 100);
            l.Columns.Add("Content", 100);
            l.Columns.Add("Areas", 300);
            l.Columns.Add("Status", 100);


            ListView lv = new ListView();
            lv.Width = 300;
            lv.Height = 200;
            GridView gridView = new GridView();
            lv.View = gridView;
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Id",
                DisplayMemberBinding = new Binding("postId")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Title",
                DisplayMemberBinding = new Binding("Title")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "content",
                DisplayMemberBinding = new Binding("Content")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Areas",
                DisplayMemberBinding = new Binding("areas")
            });
            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Status",
                DisplayMemberBinding = new Binding("status")
            });

            foreach (var post in posts)
            {
                string areas = "";
                if (post.Value.postAreas != null && post.Value.postAreas.Count > 0)
                {
                    foreach (var area in post.Value.postAreas)
                    {
                        areas += area + ", ";
                    }
                    if (areas.Length > 0)
                        areas.Substring(0, areas.Length - 2);

                }

                string status = "";

                if (post.Value.isPublished)
                    status = "Published";
                else
                    status = "Not Published";

                lv.Items.Add(new posttoshow { postId = post.Key, Title = post.Value.title, Content = post.Value.content, areas = areas, status = status });
            }

            this.canvas.Children.Add(lv);
            Canvas.SetLeft(lv, 20);
            Canvas.SetTop(lv, 50);
            Label label = new Label();
            label.Content = "post id: ";
            TextBox tb = new TextBox();
            tb.Width = 100;
            tb.TextChanged += postID_TextChanged;
            this.canvas.Children.Add(label);
            Canvas.SetLeft(label, 70);
            Canvas.SetTop(label, 270);
            this.canvas.Children.Add(tb);
            Canvas.SetLeft(tb, 140);
            Canvas.SetTop(tb, 270);

            if (request) // case of show posts for request - need to add send request button
            {
                Button button = new Button();
                button.Name = "button";
                button.Width = 100;
                button.Click += button_click;
                ; button.Content = "send request";
                this.canvas.Children.Add(button);
                Canvas.SetLeft(button, 140);
                Canvas.SetTop(button, 300);

                Label content = new Label();
                content.Content = "content:  ";
                TextBox tb_request = new TextBox();
                tb_request.Width = 100;
                tb_request.TextChanged += request_TextChanged;
                this.canvas.Children.Add(content);
                Canvas.SetLeft(content, 70);
                Canvas.SetTop(content, 330);
                this.canvas.Children.Add(tb_request);
                Canvas.SetLeft(tb_request, 140);
                Canvas.SetTop(tb_request, 330);


            }
            if (publish) // case of show user's posts - need to add publish / unpublish buttons
            {
                Button button = new Button();
                button.Name = "button";
                button.Click += button_click;
                button.Content = "publish";
                this.canvas.Children.Add(button);
                Canvas.SetLeft(button, 140);
                Canvas.SetTop(button, 300);

                Button button2 = new Button();
                button2.Name = "button2";
                button2.Click += button2_click;
                button2.Content = "unpublish";
                this.canvas.Children.Add(button2);
                Canvas.SetLeft(button2, 190);
                Canvas.SetTop(button2, 300);

            }
        }

        /// <summary>
        /// click on buttons - 2 cases of buttons: send request and publish
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_click(object sender, RoutedEventArgs e)
        {
            Post outPost;
            posts.TryGetValue(Convert.ToInt32(postID), out outPost);

            try
            {
                // publish button
                if (publish && postID != "" && postID.All(c => Char.IsDigit(c) && outPost != null))
                {
                    controller.publishPost(Convert.ToInt32(postID), true);
                    MessageBox.Show("The Status changed succesfully!", "Partner Matcher");
                }
                // send request button
                if (request && postID != "" && postID.All(c => Char.IsDigit(c) && outPost != null))
                {
                    Request requestt = new Request(Convert.ToInt32(postID), controller.currentUserEmail, "not decided", 0);
                    controller.sendRequest(requestt);
                    string content = requestContent;
                    MessageBox.Show("your request was sended succesfully!", "Partner Matcher");

                }
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// click on unpublish button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (publish && postID != "" && postID.All(c => Char.IsDigit(c)))
                {
                    controller.publishPost(Convert.ToInt32(postID), false);
                    MessageBox.Show("The Status changed succesfully!", "Partner Matcher");
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// handler for postID textbox change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void postID_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ... Get control that raised this event.
            var textBox = sender as TextBox;
            // ... Change Window Title.
            this.postID = textBox.Text;

        }

        /// <summary>
        /// handler for request number textbox change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void request_TextChanged(object sender, TextChangedEventArgs e)
        {
            // ... Get control that raised this event.
            var textBox = sender as TextBox;
            // ... Change Window Title.
            this.requestContent = textBox.Text;

        }

        /// <summary>
        /// center the window
        /// </summary>
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
