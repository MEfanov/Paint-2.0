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

namespace Paint_2._0
{
    /// <summary>
    /// Логика взаимодействия для PaintMenu.xaml
    /// </summary>
    public partial class PaintMenu : UserControl
    {
        public PaintMenu()
        {
            InitializeComponent();
            SetAllWindowRelatedInactive();
        }

        public void SetAllWindowRelatedInactive()
        {
            Menu_CanvasSize.IsEnabled = false;
            Menu_Cascade.IsEnabled = false;
            Menu_Clear.IsEnabled = false;
            Menu_Horizontal.IsEnabled = false;
            Menu_Minimize.IsEnabled = false;
            Menu_Save.IsEnabled = false;
            Menu_SaveAs.IsEnabled = false;
            Menu_Vertical.IsEnabled = false;
            Menu_WindowSize.IsEnabled = false;
            Menu_ZoomIn.IsEnabled = false;
            Menu_ZoomOut.IsEnabled = false;
        }

        public void SetAllWindowRelatedActive()
        {
            Menu_CanvasSize.IsEnabled = true;
            Menu_Cascade.IsEnabled = true;
            Menu_Clear.IsEnabled = true;
            Menu_Horizontal.IsEnabled = true;
            Menu_Minimize.IsEnabled = true;
            Menu_Save.IsEnabled = true;
            Menu_SaveAs.IsEnabled = true;
            Menu_Vertical.IsEnabled = true;
            Menu_WindowSize.IsEnabled = true;
            Menu_ZoomIn.IsEnabled = true;
            Menu_ZoomOut.IsEnabled = true;
        }
    }
}
