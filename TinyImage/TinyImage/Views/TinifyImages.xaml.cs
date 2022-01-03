using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TinifyAPI;
using MessageBox = System.Windows.MessageBox;

namespace TinyImage.Views
{
    /// <summary>
    /// Interaction logic for TinifyImages.xaml
    /// </summary>
    public partial class TinifyImages : System.Windows.Controls.UserControl
    {
        private string _sourceFolderPath;
        private string _outputFolderPath;

        private FileSystemWatcher _watcher;

        public TinifyImages()
        {
            InitializeComponent();

            // initialization
            _sourceFolderPath = string.Empty;
            _outputFolderPath = string.Empty;
            _watcher = null;
        }

        private void sourceFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Dispose after using
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult selectFolderDialogResult = folderDialog.ShowDialog();

                if (selectFolderDialogResult == DialogResult.OK)
                {
                    _sourceFolderPath = folderDialog.SelectedPath + "\\";

                    sourceFolderLabel.Content = _sourceFolderPath;
                }
            }
        }

        private void outputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Dispose after using
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                DialogResult selectFolderDialogResult = folderDialog.ShowDialog();

                if (selectFolderDialogResult == DialogResult.OK)
                {
                    _outputFolderPath = folderDialog.SelectedPath + "\\";

                    outputFolderLabel.Content = _outputFolderPath;
                }
            }
        }

        private void syncButton_Checked(object sender, RoutedEventArgs e)
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

            TinifyFiles();

            InitWatcher(_sourceFolderPath);
        }

        private void syncButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_watcher != null)
            {
                _watcher.Dispose();
            }
        }

        private async void TinifyFiles()
        {
            List<Task> filesToTinify = new List<Task>();

            // Take all files from Source Folder and put them in DirectoryInfo 
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(_sourceFolderPath);

            // Take all files from Output Folder and put them in DirectoryInfo 
            DirectoryInfo outputDirectoryInfo = new DirectoryInfo(_outputFolderPath);

            if (outputDirectoryInfo.GetFiles().Length == 0 && sourceDirectoryInfo.GetFiles().Length > 0)
            {
                foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                {
                    var sourceFileTask = Tinify.FromFile(_sourceFolderPath + file.Name);
                    filesToTinify.Add(Task.Run(() =>
                         sourceFileTask.ToFile(
                             _outputFolderPath + System.IO.Path.GetFileNameWithoutExtension(file.Name) + "_tiny.png")));
                }

                // whenever a file finishes store it
                while (filesToTinify.Count > 0)
                {
                    Task finishedtask = await Task.WhenAny(filesToTinify);
                    filesToTinify.Remove(finishedtask);
                }
            }

            if (sourceDirectoryInfo.GetFiles().Length > 0 && outputDirectoryInfo.GetFiles().Length > 0)
            {
                foreach (FileInfo sourceFile in sourceDirectoryInfo.GetFiles())
                {
                    foreach (FileInfo outFile in outputDirectoryInfo.GetFiles())
                    {
                        string cleanOutFile = Path.GetFileNameWithoutExtension(outFile.Name).Replace("_tiny", "");

                        if (sourceDirectoryInfo.GetFiles().Select(name => name.ToString()).Contains(cleanOutFile))
                        {
                            continue;
                        }
                        else
                        {
                            var sourceFileTask = Tinify.FromFile(_sourceFolderPath + sourceFile.Name);
                            filesToTinify.Add(Task.Run(() =>
                                 sourceFileTask.ToFile(
                                     _outputFolderPath + Path.GetFileNameWithoutExtension(sourceFile.Name) + "_tiny.png")));
                        }
                    }

                    // whenever a file finishes store it
                    while (filesToTinify.Count > 0)
                    {
                        Task finishedtask = await Task.WhenAny(filesToTinify);
                        filesToTinify.Remove(finishedtask);
                    }
                }
            }
        }

        private void InitWatcher(string folder)
        {
            if (!folder.Equals(string.Empty))
            {
                _watcher = new FileSystemWatcher(folder)
                {
                    NotifyFilter = NotifyFilters.Attributes
                                     | NotifyFilters.CreationTime
                                     | NotifyFilters.DirectoryName
                                     | NotifyFilters.FileName
                                     | NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.Security
                                     | NotifyFilters.Size,
                    Filter = "*.png"
                };

                _watcher.Changed += new FileSystemEventHandler(OnFolderChange);

                _watcher.Created += new FileSystemEventHandler(OnFolderChange);
                _watcher.Error += new ErrorEventHandler(OnError);

                // TODO: Use commented events
                //watcher.Deleted += OnDeleted;
                //watcher.Renamed += OnRenamed;
                
                _watcher.IncludeSubdirectories = true;
                _watcher.EnableRaisingEvents = true;
            }
        }

        private async void OnFolderChange(object sender, FileSystemEventArgs e)
        {
            // OnFolderChange compress every new file
            var sourceFileTask = Tinify.FromFile(e.FullPath);
            await sourceFileTask.ToFile(_outputFolderPath + Path.GetFileNameWithoutExtension(e.Name) + "_tiny.png");
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}
