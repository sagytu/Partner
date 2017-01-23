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
    /// Interaction logic for searchWindow.xaml
    /// </summary>
    public partial class searchWindow : Window
    {
        MyController controler;
        public searchWindow(MyController controler)
        {
            InitializeComponent();
            // need to fill areas here
            this.controler = controler;

            this.areas.ItemsSource = controler.areas;
            CenterWindowOnScreen();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (this.city.Text == "" || this.areas.Text == "")
                MessageBox.Show("one or more of the fields is empty", "EROR");
            if (!this.city.Text.All(c => Char.IsLetter(c)))
                MessageBox.Show("the city is invalid", "EROR");
            string city = this.city.Text;
            string area = this.areas.Text;
            Dictionary<int, Post> pl = new Dictionary<int, Post>();
            pl = controler.search(city, area);

            Posts postWin = new Posts(true, false, controler, pl);

            postWin.Show();
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
