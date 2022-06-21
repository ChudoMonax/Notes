﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

		public MainWindow()
		{
			InitializeComponent();
			cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
			cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

			ListOfFolders.AutoGenerateColumns = false;
			ListOfFolders.ColumnWidth = DataGridLength.Auto;
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
			}
		}

        public void ListOfFiles_Loaded(object sender, RoutedEventArgs e)
        {
			if (ListOfFolders.SelectedItem != null)
			{
				SelectedFolder = ((Folder)ListOfFolders.SelectedItem);
				string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title;
				List<File> AllFiles = Directory.GetFiles(Path).Select(s => new File() { FileName = s.Replace(Path + @"\", "").Replace(".rtf", "") }).ToList();
				ListOfFiles.ItemsSource = AllFiles;
			}
		}

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
			SaveNewFile savenewfile = new SaveNewFile(this, SelectedFolder);
			savenewfile.ShowDialog();
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
			if (ListOfFiles.SelectedItem != null)
			{
				SelectedFile = ((File)ListOfFiles.SelectedItem);
				string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + "Notes" + @"\" + SelectedFolder.Title + @"\" + SelectedFile.FileName + ".rtf";
				FileStream fileStream = new FileStream(Path, FileMode.Open);
				TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
				range.Load(fileStream, DataFormats.Rtf);
				fileStream.Close();
			}
		}
    }

    public class Folder
    {
		public string Title { get; set; }
    }

	public class File
    {
		public string FileName { get; set; }
    }
}
