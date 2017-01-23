
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
    /// Interaction logic for newPost.xaml
    /// </summary>
    public partial class newPost : Window
    {
        public MyController controller;

        public bool isPublished;

        public newPost(MyController controler)
        {
            InitializeComponent();
            this.controller = controler;
            areas.ItemsSource = controller.areas;
            CenterWindowOnScreen();

        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            string title = this.title.Text;
            string content = this.content.Text;
            string city = this.city.Text;
            string newArea = this.newArea.Text;
            List<string> areas = new List<string>();

            foreach (var item in this.areas.SelectedItems)
            {
                areas.Add(item.ToString());

            }
            if (newArea != "")
                areas.Add(newArea);

            try
            {
                Post post = new Post(title, content, areas, controller.currentUserEmail, city, isPublished);
                controller.addNewPost(post, newArea);

                MessageBox.Show("Your post was added succesfully!", "EROR");
                this.Close();

            }
            catch (Exception)
            {
            }

        }


        private void yes_Checked(object sender, RoutedEventArgs e)
        {
            isPublished = true;
        }

        private void no_Checked(object sender, RoutedEventArgs e)
        {
            isPublished = false;

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
