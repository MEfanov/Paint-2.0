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
using WPF.MDI;
using System.IO;

namespace Paint_2._0
{
    public partial class WindowManager : UserControl
    {
        public event RoutedEventHandler ActiveWindowChanged;

        private const double defaultWindowWidth = 230;
        private const double defaultWindowHeight = 250;
        private int windowCounter = 1;

        private MdiChild activeWindow;
        private MdiChild? savedWindow = null;

        public MdiChild? ActiveWindow 
        {
            get { return activeWindow; }
            private set
            {
                activeWindow = value;
                ActiveWindowChanged?.Invoke(this, new RoutedEventArgs());
            }
        }
        public DrawingCanvas ActiveCanvas
        {
            get
            {
                if(ActiveWindow != null)
                    return (DrawingCanvas)ActiveWindow.Content;
                return null;
            }
        }
        public MdiLayout WindowLayout
        {
            get
            {
                return MDI_Container.MdiLayout;
            }
            set
            {
                UnmimimizeAll();
                MDI_Container.MdiLayout = value;
            }
        }
        public string? ActiveWindowSavePath
        {
            get 
            {
                if (ActiveWindow == null)
                    return null;
                return ActiveWindow.Tag as string;
            }
        }

        public WindowManager()
        {
            InitializeComponent();
        }

        public MdiChild CreateNewWindow(double Width = defaultWindowWidth, double Height = defaultWindowHeight)
        {
            MdiChild newWindow = new MdiChild()
            {
                Width = Width,
                Height = Height,
                Content = new DrawingCanvas(),
                Title = "NewWindow" + windowCounter++,
                Tag = null
            };
            newWindow.GotFocus += WindowGotFocus;
            newWindow.Closed += WindowClosed;
            newWindow.Closing += WindowClosing;
            MDI_Container.Children.Add(newWindow);
            return newWindow;
        }

        public void MinimizeAll()
        {
            foreach(var child in MDI_Container.Children)
            {
                child.WindowState = WindowState.Minimized;
            }
        }

        public void UnmimimizeAll()
        {
            foreach (var child in MDI_Container.Children)
            {
                child.WindowState = WindowState.Normal;
            }
        }

        private void WindowGotFocus(object sender, RoutedEventArgs e)
        {
            ActiveWindow = (MdiChild)sender;
        }

        private void WindowClosed(object sender, RoutedEventArgs e)
        {
            if(savedWindow != null)
            {
                MDI_Container.Children.Add(savedWindow);
                savedWindow = null;
            }
            else if(MDI_Container.Children.Count == 0)
                ActiveWindow = null;
        }

        private void WindowClosing(object sender, RoutedEventArgs e)
        {
            MdiChild window = (MdiChild)sender;
            DrawingCanvas? closingCanvas = window.Content as DrawingCanvas;

            if (closingCanvas != null && closingCanvas.WasEdited)
            {
                MessageBoxResult res = MessageBox.Show($"Вы хотите сохранить изменения в файле {((MdiChild)sender).Title}?", 
                    "Paint 2.0",
                    MessageBoxButton.YesNoCancel, 
                    MessageBoxImage.Exclamation, 
                    MessageBoxResult.Cancel);

                if (res == MessageBoxResult.Yes)
                {
                    ApplicationCommands.Save.Execute(null, window);
                    savedWindow = null;
                }
                else if (res == MessageBoxResult.Cancel)
                {
                    //savedCanvas = window.Content as DrawingCanvas;
                    savedWindow = window;
                }
                else
                    savedWindow = null;
            }
        }
    }
}
