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
using TinifyAPI;

namespace TinyImage.Views
{
    /// <summary>
    /// Interaction logic for APIValidation.xaml
    /// </summary>
    public partial class APIValidation : UserControl
    {
        private ViewModels.APIValidation apiValidation;

        public APIValidation()
        {
            InitializeComponent();
            apiValidation = new ViewModels.APIValidation();
        }

        // If validate button clicked run validateAPIKey
        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            apiValidation.validateAPIKey(errorLabel,APITextBox);
        }

        // If Enter key is Down/Pressed run validateAPIKey
        private void OnEnterKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                apiValidation.validateAPIKey(errorLabel,APITextBox);
            }
        }
    }
}
