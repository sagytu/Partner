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
using System.Windows.Navigation;
using System.Windows.Shapes;
using partnersMatcherPart4;
using partnersMatcherPart4.Model;
using partnersMatcherPart4.Controller;
using System.IO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyController controller;
        public MainWindow(MyController controler)
        {
            InitializeComponent();
            this.controller = controler;
            CenterWindowOnScreen();
            string accessFilePath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            BitmapImage b = new BitmapImage();
            b.BeginInit();
            b.UriSource = new Uri(accessFilePath + "/images/logo.png");
            b.EndInit();
            logo.Source = b;

        }

        private void click_Search(object sender, RoutedEventArgs e)
        {
            if (controller.currentUserEmail != "")
            {
                searchWindow search = new searchWindow(controller);
                search.Show();

            }
            else
                MessageBox.Show("you have to login first!", "EROR");
        }

        private void click_newUser(object sender, RoutedEventArgs e)
        {
            newUser newUser = new newUser(controller);
            newUser.Show();
        }

        private void click_Post(object sender, RoutedEventArgs e)
        {
            if (controller.currentUserEmail != "")
            {
                newPost newPost = new newPost(controller);
                newPost.Show();
            }
            else
                MessageBox.Show("you have to login first!", "EROR");

        }



        private void Login_Click(object sender, RoutedEventArgs e)
        {

            if (this.email.Text == "" || this.pass.Password == "")
                MessageBox.Show("one or more of the fields is empty", "EROR");
            if (!this.email.Text.Contains("@") && !this.email.Text.Contains("."))
                MessageBox.Show("The email is invalid", "EROR");

            string email = this.email.Text;
            string pass = this.pass.Password;

            try
            {

                if (controller.login(email, pass))
                {
                    MessageBox.Show("login seccesful!");
                    controller.setCurrentFullName();
                    this.name.Text = "Hello " + controller.currentFullName;
                    this.email.Text = "";
                    this.pass.Password = "";
                }
                else
                {
                    MessageBox.Show("login failed!  email or password is invalid");
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void click_MyPosts(object sender, RoutedEventArgs e)
        {
            if (controller.currentUserEmail != "")
            {
                Dictionary<int, Post> posts = controller.getUsersPosts();
                Posts postsWindow = new Posts(false, true, controller, posts);
                postsWindow.Show();
            }
            else
                MessageBox.Show("you have to login first!", "EROR");

        }

        private void click_AllPosts(object sender, RoutedEventArgs e)
        {
            if (controller.currentUserEmail != "")
            {
                try
                {
                    Dictionary<int, Post> posts = controller.getPublishedPost();
                    Posts postsWindow = new Posts(true, false, controller, posts);
                    postsWindow.Show();
                }
                catch (Exception)
                {

                }
            }
            else
                MessageBox.Show("you have to login first!", "EROR");

        }

        private void click_AllRequests(object sender, RoutedEventArgs e)
        {
            if (controller.currentUserEmail != "")
            {
                try
                {
                    List<Request> requests = controller.getRequests();
                    showRequests requestsWindow = new showRequests(controller, requests);
                    requestsWindow.Show();
                }
                catch (Exception)
                {

                }
            }
            else
                MessageBox.Show("you have to login first!", "EROR");

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
