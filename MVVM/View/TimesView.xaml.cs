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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryDeweyDecimalApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for TimesView.xaml
    /// </summary>
    public partial class TimesView : UserControl
    {
        public TimesView()
        {
            InitializeComponent();
            LoadDataFromTextFiles();
        }

        private void LoadDataFromTextFiles()
        {
            string file1Path = "ReplacingBooksTimes.txt";
            string file2Path = "IdentifyingAreasTimes.txt";
            string file3Path = "FindingCallNumbersTimes.txt";

            try
            {
                // Read the contents of the first text file and load them into ListBox1
                if (File.Exists(file1Path))
                {
                    string[] lines = File.ReadAllLines(file1Path);
                    ReplacingBooksTimesList.ItemsSource = lines;
                }

                // Read the contents of the second text file and load them into ListBox2
                if (File.Exists(file2Path))
                {
                    string[] lines = File.ReadAllLines(file2Path);
                    IdentifyingAreasTimesList.ItemsSource = lines;
                }

                // Read the contents of the second text file and load them into ListBox2
                if (File.Exists(file3Path))
                {
                    string[] lines = File.ReadAllLines(file2Path);
                    FindingCallNumbersList.ItemsSource = lines;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading text files: {ex.Message}");
            }
        }
    }
}
