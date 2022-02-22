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

namespace Paint_2._0
{
    /// <summary>
    /// Логика взаимодействия для WindowSizeDialog.xaml
    /// </summary>
    public partial class Window_SizeDialog : Window
    {
        public double SizeWidth { get; set; } = 0;
        public double SizeHeight { get; set; } = 0;

        public Window_SizeDialog()
        {
            InitializeComponent();
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            SizeWidth = Convert.ToDouble(WidthTextBox.Text);
            SizeHeight = Convert.ToDouble(HeightTextBox.Text);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WidthTextBox.Text = Math.Round(SizeWidth,2).ToString();
            HeightTextBox.Text = Math.Round(SizeHeight,2).ToString();
        }
    }
}
