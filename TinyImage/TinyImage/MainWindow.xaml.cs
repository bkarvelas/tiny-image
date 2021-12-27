using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TinifyAPI;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TinyImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            validateAPI();
        }

        private void compressFile()
        {
        }

        private async void validateAPI()
        {
            try
            {
                labelError.Content = string.Empty;
                Tinify.Key = textBoxAPI.Text;
                await Tinify.Validate();
                labelError.Content = "SUCCESS";
            }
            catch (Exception e)
            {
                labelError.Content = e.Message;
            }
        }
    }
}
