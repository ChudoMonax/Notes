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
        public SaveNewFolder()
        {
            InitializeComponent();
        }

        private void CreateFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FolderName.Text == "")
            {
                FolderName.ToolTip = "Folder name can't be empty";
                //FolderName.Background = Brushes.Red;
            }
            else
            {
                string NameOfFolder = FolderName.Text;
            }
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + FolderName.Text;
            System.IO.Directory.CreateDirectory(Path);

            Close();
        }
    }
}
