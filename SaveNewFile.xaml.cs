using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SaveNewFile.xaml
    /// </summary>
    public partial class SaveNewFile : Window
    {
        MainWindow _M;
        Folder _F;
        public string CreatedFileName { get; set; }
        public SaveNewFile(MainWindow m, Folder f)
        {
            InitializeComponent();
            _M = m;
            _F = f;
        }

        private void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            if (FileName.Text == "")
            {
                FileName.ToolTip = "File name can't be empty";
            }
            else
            {
                string NameOfFolder = FileName.Text;
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + _F.Title + @"\" + FileName.Text;
                CreatedFileName = Path;
                Close();
                _M.ListOfFiles_Loaded(sender, e);
            }
        }
    }
}
