using System.Windows;
using TinyImage.Views;

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
            Content = new APIValidation();
        }
    }
}
