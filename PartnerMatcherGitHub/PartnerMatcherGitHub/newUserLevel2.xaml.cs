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
using partnersMatcherPart4.Controller;
using partnersMatcherPart4.Model;

namespace GUI
{
    /// <summary>
    /// Interaction logic for newUser.xaml
    /// </summary>
    public partial class newUserLevel2 : Window
    {
        string email;
        MyController controller;
        User user;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="controler"></param>
        /// <param name="email"></param>
        /// <param name="user"></param>
        public newUserLevel2(MyController controler, string email, User user)
        {
            InitializeComponent();
            this.controller = controler;
            this.email = email;
            this.areas.ItemsSource = controler.areas;
            this.areas.MaxHeight = 100;
            this.user = user;
            CenterWindowOnScreen();

        }

        /// <summary>
        /// handler for combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // ... A List.
            List<string> data = new List<string>();

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// handler for selction changed in the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

        }

        /// <summary>
        /// click on create - call the create new user function in the controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void create_Click(object sender, RoutedEventArgs e)
        {
            string religion = this.religion.Text;
            string marital = this.marital.Text;



            try
            {
                List<string> areas = new List<string>();
                foreach (string item in this.areas.SelectedItems)
                {
                    areas.Add(item);
                }
                user.religion = religion;
                user.maritalStatus = marital;
                user.areas = areas;
                controller.addNewUser(this.user);

                this.Close();
            }
            catch (Exception)
            {

            }


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
