using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileVersionRead
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _oDFVI_Lst = new List<DataFileVersionInfo>();
            this.DataContext = this;
            _sFileName = Path.Combine(Environment.SystemDirectory, "Notepad.exe"); //"C:\\Windows\\system32\\notepad.exe"
            _sFileName = "C:\\runasdate\\RunAsDate.chm";
            DataGrid_ItemSrcUpd();
        }

        private string _sFileName = null;
        private FileVersionInfo _oFVI = null;
        private List<DataFileVersionInfo> _oDFVI_Lst = null;

        public List<DataFileVersionInfo> DataGrid_ItemSrc { get { return _oDFVI_Lst; } }

        private bool DataGrid_ItemSrcUpd()
        {
            try
            {
                _oFVI = FileVersionInfo.GetVersionInfo(_sFileName);
            }
            catch (FileNotFoundException e)
            {
                return false;
            }

            PropertyInfo[] oPropInfo_Lst = _oFVI.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (oPropInfo_Lst != null && oPropInfo_Lst.Length > 0)
            {
                _oDFVI_Lst.Clear();

                int iID = 0;

                foreach (PropertyInfo oPropInfo in oPropInfo_Lst)
                {
                    string sValue = null;

                    try
                    {
                        sValue = oPropInfo.GetValue(_oFVI, null).ToString();
                    }
                    catch (Exception e)
                    {
                    }

                    _oDFVI_Lst.Add(new DataFileVersionInfo(++iID, oPropInfo.Name, sValue));
                }

                return (_oDFVI_Lst.Count > 0);
            }
            else
            {
                return false;
            }
        }

        private void DataGrid_DragEnter(object sender, DragEventArgs e)
        {
            DataGrid_DragOver(sender, e);
        }

        private void DataGrid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
                DataGrid cDataGrid = (DataGrid)sender;
                cDataGrid.RowBackground = cDataGrid.Background = new SolidColorBrush(Colors.Aqua);
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DataGrid_DragLeave(object sender, DragEventArgs e)
        {
            DataGrid cDataGrid = (DataGrid)sender;
            cDataGrid.RowBackground = cDataGrid.Background = new SolidColorBrush(Colors.AntiqueWhite);
        }

        private void DataGrid_Drop(object sender, DragEventArgs e)
        {
            DataGrid cDataGrid = (DataGrid)sender;
            cDataGrid.RowBackground = cDataGrid.Background = new SolidColorBrush(Colors.AntiqueWhite);

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files != null && files.Length > 0)
            {
                _sFileName = files[0];
                if (DataGrid_ItemSrcUpd())
                {
                    e.Handled = true;
                }
                else
                {
                    e.Handled = false;
                }
            }
        }
    }
}
