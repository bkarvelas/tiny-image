using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TinyImage.Views
{
    /// <summary>
    /// Interaction logic for TinifyImages.xaml
    /// </summary>
    public partial class TinifyImages : System.Windows.Controls.UserControl
    {
        private string _sourceFolderPath;
        private string _outputFolderPath;

        private readonly FolderBrowserDialog _folderDialog;
        private DialogResult _result;

        public TinifyImages()
        {
            InitializeComponent();

            // initialization
            _folderDialog = new FolderBrowserDialog();
            _sourceFolderPath = string.Empty;
            _outputFolderPath = string.Empty;
        }

        private void sourceFolderButton_Click(object sender, RoutedEventArgs e)
        {
            _result = _folderDialog.ShowDialog();

            if (_result == DialogResult.OK)
            {
                _sourceFolderPath = _folderDialog.SelectedPath + "\\";

                sourceFolderLabel.Content = _sourceFolderPath;
            }
        }

        private void ouputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            _result = _folderDialog.ShowDialog();

            if (_result == DialogResult.OK)
            {
                _outputFolderPath = _folderDialog.SelectedPath + "\\";

                ouputFolderLabel.Content = _outputFolderPath;
            }
        }

        private void syncButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void syncButton_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
