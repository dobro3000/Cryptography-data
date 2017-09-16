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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using LibraryWithBaseCripts;

namespace Criptograf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string[] typeCrypts = { "Permutation", "Monoalphabetic", "Polyalphabetic", "Summirovanii" };
        private string fname { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.cbUn.IsChecked = true;
            this.cbChoosCript.ItemsSource = typeCrypts;
        }

        private void btnFileOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog saveXlsxDialog = new System.Windows.Forms.OpenFileDialog
                {
                    DefaultExt = ".*",
                    AddExtension = true,
                    RestoreDirectory = true
                };

                bool? result = Convert.ToBoolean(saveXlsxDialog.ShowDialog());

                if (result == true)
                {
                    FileInfo xlsxFile = new FileInfo(saveXlsxDialog.FileName);
                    this.tbFileName.Text = xlsxFile.Name;
                    fname = xlsxFile.FullName;
                }
            }
            catch 
            {
                
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (tbKey.Text == "" || tbFileName.Text == "")
            {
                MessageBox.Show("Opps, some faild is empty! Pls check it and run agein!");
                return;
            }

            LibCryptographer crypte = new LibCryptographer();

            crypte.Cryptographer(this.fname, this.tbKey.Text, this.cbChoosCript.Text, this.cbUn.IsChecked.Value);


        }
    }
}
