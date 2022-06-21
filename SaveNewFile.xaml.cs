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
        public SaveNewFile(MainWindow m, Folder f)
        {
            InitializeComponent();
            _M = m;
            _F = f;
        }

        private void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            //RichTextBox rtb = new RichTextBox();
            //var a = sender;
            //RoutedEventArgs b = e;
            if (FileName.Text == "")
            {
                FileName.ToolTip = "File name can't be empty";
                // FolderName.Background = Brushes.Red;
            }
            else
            {
                string NameOfFolder = FileName.Text;
            }
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + _F.Title + @"\" + FileName.Text + ".rtf";

            //FileStream fileStream = new FileStream(Path, FileMode.Create);
            //TextRange range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            //range.Save(fileStream, DataFormats.Rtf);

            System.IO.File.Create(Path);
            //System.IO.File.WriteAllText(Path, "123123");
            Close();
            _M.ListOfFiles_Loaded(sender, e);
        }
    }
}
