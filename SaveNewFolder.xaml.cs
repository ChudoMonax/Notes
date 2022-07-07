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

namespace Notes2
{
    /// <summary>
    /// Interaction logic for SaveNewFolder.xaml
    /// </summary>
    public partial class SaveNewFolder : Window
    {
        MainWindow _M;
        public SaveNewFolder(MainWindow m)
        {
            InitializeComponent();
            _M = m;
        }

        private void CreateFolder_Click(object sender, RoutedEventArgs e)
        {
            var a = sender;
            RoutedEventArgs b = e;
            if (FolderName.Text == "")
            {
                FolderName.ToolTip = "Folder name can't be empty";
                // FolderName.Background = Brushes.Red;
            }
            else
            {
                string NameOfFolder = FolderName.Text;
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + FolderName.Text;
                System.IO.Directory.CreateDirectory(Path);
                Close();
                _M.FoldersDataGrid_Loaded(sender, e);
                _M.ListOfFiles_Loaded(sender, e);
                _M.ListOfFolders_SelectedCellsChanged(sender, null);
            }
        }
    }
}
