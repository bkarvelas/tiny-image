using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using TinifyAPI;
using MessageBox = System.Windows.MessageBox;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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

        private FileSystemWatcher _watcher;
        private DirectoryInfo _directoryInfo;

        public TinifyImages()
        {
            InitializeComponent();

            // initialization
            _folderDialog = new FolderBrowserDialog();
            _sourceFolderPath = string.Empty;
            _outputFolderPath = string.Empty;
            _watcher = null;
            _directoryInfo = null;
        }

        private void sourceFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Remove this line 
            _folderDialog.SelectedPath = @"C:\Users\arctu\Desktop\TinifyTesting";

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

        private async void syncButton_Checked(object sender, RoutedEventArgs e)
        {
            // If source folder is not selected show a MessageBox with a message and return
            if (_sourceFolderPath.Equals(string.Empty))
            {
                MessageBox.Show("You have to set Source Folder.", "Source Folder Undefined");
                syncButton.IsChecked = false;
                return;
            }

            // If output folder is not selected show a MessageBox with a message and return
            if (_outputFolderPath.Equals(string.Empty))
            {
                MessageBox.Show("You have to set Output Folder.", "Output Folder Undefined");
                syncButton.IsChecked = false;
                return;
            }

            var filesToTinify = new HashSet<Task>();

            // First of all be sure _directoryInfo is null
            if (_directoryInfo == null)
            {
                // Take all files from Folder and put them in DictionaryInfo 
                _directoryInfo = new DirectoryInfo(_sourceFolderPath);

                // If file missing from HashSet just add it and fire off the task to compress it
                foreach (var file in _directoryInfo.GetFiles())
                {
                    var sourceFile = Tinify.FromFile(_sourceFolderPath + file.Name);
                    filesToTinify.Add(Task.Run(() => sourceFile.ToFile(_outputFolderPath + System.IO.Path.GetFileNameWithoutExtension(file.Name) + "_tiny.png")));
                }
            }

            // Whenever a file finishes store it
            while(filesToTinify.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(filesToTinify);
                filesToTinify.Remove(finishedTask);
            }

            if (!_sourceFolderPath.Equals(string.Empty))
            {
                _watcher = new FileSystemWatcher(_sourceFolderPath);

                _watcher.NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size;

                _watcher.Changed += new FileSystemEventHandler(OnFolderChange);
                //watcher.Created += OnCreated;
                //watcher.Deleted += OnDeleted;
                //watcher.Renamed += OnRenamed;
                _watcher.Error += new ErrorEventHandler(OnError);

                _watcher.Filter = "*.jpg | *.png";
                _watcher.IncludeSubdirectories = true;
                _watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
            }
        }

        private void OnFolderChange(object sender, FileSystemEventArgs e)
        {


        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }

        private void syncButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
                _directoryInfo = null;
            }
        }
    }
}
