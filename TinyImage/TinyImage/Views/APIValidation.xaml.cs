using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TinifyAPI;

namespace TinyImage.Views
{
    /// <summary>
    /// Interaction logic for APIValidation.xaml
    /// </summary>
    public partial class APIValidation : UserControl
    {

        public APIValidation()
        {
            InitializeComponent();
        }

        // If validate button clicked run validateAPIKey
        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            validateAPIKey();
        }

        // If Enter key is Down/Pressed run validateAPIKey
        private void OnEnterKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                validateAPIKey();
            }
        }
        public async void validateAPIKey()
        {
            try
            {
                Tinify.Key = APITextBox.Text;
                await Tinify.Validate();

                // Change view to TinifyImages
                Application.Current.MainWindow.Content = new TinifyImages();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "An Error has occured");
            }
        }
    }
}
