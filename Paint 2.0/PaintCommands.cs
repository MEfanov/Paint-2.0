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
    public class PaintCommands
    {
        public static RoutedCommand ClearCanvas { get; set; }
        public static RoutedCommand ChangeCanvasSize { get; set; }
        public static RoutedCommand ChangeWindowSize { get; set; }
        public static RoutedCommand ZoomIn { get; set; }
        public static RoutedCommand ZoomOut { get; set; }
        public static RoutedCommand LayoutCascade { get; set; }
        public static RoutedCommand LayoutHorizontal { get; set; }
        public static RoutedCommand LayoutVertical { get; set; }
        public static RoutedCommand LayoutMinimize { get; set; }
        public static RoutedCommand ShowAboutBox { get; set; }


        static PaintCommands()
        {
            ClearCanvas = new RoutedCommand("ClearCanvas", typeof(MainWindow));
            ChangeCanvasSize = new RoutedCommand("ChangeCanvasSize", typeof(MainWindow));
            ChangeWindowSize = new RoutedCommand("ChangeWindowSize", typeof(MainWindow));
            ZoomIn = new RoutedCommand("ZoomIn", typeof(MainWindow));
            ZoomOut = new RoutedCommand("ZoomOut", typeof(MainWindow));
            LayoutCascade = new RoutedCommand("LayoutCascade", typeof(MainWindow));
            LayoutHorizontal = new RoutedCommand("LayoutHorizontal", typeof(MainWindow));
            LayoutVertical = new RoutedCommand("LayoutVertical", typeof(MainWindow));
            LayoutMinimize = new RoutedCommand("LayoutMinimize", typeof(MainWindow));
            ShowAboutBox = new RoutedCommand("ShowAboutBox", typeof(MainWindow));
        }
    }
}
