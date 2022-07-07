using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Notes2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Folder SelectedFolder = new Folder();
        File SelectedFile = new File();

        public delegate Point GetPosition(IInputElement element);
        int rowIndex = -1;
        string dgName;

        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

            ListOfFolders.AutoGenerateColumns = false;
            ListOfFolders.ColumnWidth = DataGridLength.Auto;

            ListOfFiles.AutoGenerateColumns = false;
            ListOfFiles.ColumnWidth = DataGridLength.Auto;
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            rtbEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {
            SaveNewFolder savenewfolder = new SaveNewFolder(this);
            savenewfolder.ShowDialog();
        }

        public void FoldersDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes";
            List<Folder> AllFolders = Directory.GetDirectories(Path).Select(s => new Folder() { Title = s.Replace(Path + @"\", "") }).ToList();
            ListOfFolders.ItemsSource = AllFolders;
        }

        public void ListOfFolders_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (ListOfFolders.SelectedItem != null)
            {
                SelectedFolder = ((Folder)ListOfFolders.SelectedItem);
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title;
                List<File> AllFiles = Directory.GetFiles(Path).Select(s => new File() { FileName = s.Replace(Path + @"\", "").Replace(".rtf", "") }).ToList();
                ListOfFiles.ItemsSource = AllFiles;

                ListOfFolders.CurrentCell.Column.IsReadOnly = true;
            }
            ListOfFiles_Loaded(sender, null);
        }

        public void ListOfFiles_Loaded(object sender, RoutedEventArgs e)
        {
            if (ListOfFolders.SelectedItem != null)
            {
                SelectedFolder = ((Folder)ListOfFolders.SelectedItem);
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title;
                List<File> AllFiles = Directory.GetFiles(Path).Select(s => new File() { FileName = s.Replace(Path + @"\", "").Replace(".rtf", "") }).ToList();
                ListOfFiles.ItemsSource = AllFiles;

                if (ListOfFolders.CurrentCell.Column != null)
                {
                    ListOfFolders.CurrentCell.Column.IsReadOnly = true;
                }
            }
        }

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            SaveNewFile savenewfile = new SaveNewFile(this, SelectedFolder);
            savenewfile.ShowDialog();

            // string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title + @"\" + SelectedFile.FileName + ".rtf";
            FileStream fileStream = new FileStream(savenewfile.CreatedFileName + ".rtf", FileMode.Create);

            RichTextBox rtbClear = new RichTextBox();

            TextRange range = new TextRange(rtbClear.Document.ContentStart, rtbClear.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();

            ListOfFiles_Loaded(sender, e);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title + @"\" + SelectedFile.FileName + ".rtf";
            FileStream fileStream = new FileStream(Path, FileMode.Create);
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.Save(fileStream, DataFormats.Rtf);
            fileStream.Close();
        }

        private void ListOfFiles_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            SaveButton_Click(sender, null);
            if (ListOfFiles.SelectedItem != null)
            {
                SelectedFile = ((File)ListOfFiles.SelectedItem);
                string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title + @"\" + SelectedFile.FileName + ".rtf";
                FileStream fileStream = new FileStream(Path, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
                fileStream.Close();

                rtbEditor.Focusable = true;
            }
        }

        private void ListOfFolders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListOfFolders.CurrentCell.Column.IsReadOnly = false;
            ListOfFolders.BeginEdit();
        }

        private void ListOfFolders_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string editFolder = e.EditingElement.ToString();
            editFolder = editFolder.Replace("System.Windows.Controls.TextBox: ", "");
            string OriginalPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title;
            string NewPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + editFolder;

            if (NewPath != "" && NewPath != OriginalPath && NewPath != null && NewPath != Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + "System.Windows.Controls.TextBox")
            {
                Directory.Move(OriginalPath, NewPath);
                SelectedFolder.Title = NewPath;
            }
            else if (NewPath == "" || NewPath == null || NewPath == Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + "System.Windows.Controls.TextBox" || editFolder == "System.Windows.Controls.TextBox")
            {
                NewPath = OriginalPath;
            }
            FoldersDataGrid_Loaded(sender, null);
        }

        private void ListOfFiles_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }

    public class Folder
    {
        public string Title { get; set; }
        public int Id { get; set; }
    }

    public class File
    {
        public string FileName { get; set; }
    }
}
