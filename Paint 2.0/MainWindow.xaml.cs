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
using Microsoft.Win32;

namespace Paint_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateNewWindowCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.CreateNewWindow();

        private void OpenFile_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = "Bitmap(*.bmp)|*.bmp|JPG(*.jpg)|*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;

                MdiChild newWindow = PaintWindowManager.CreateNewWindow();
                int titleBeginInd = path.LastIndexOf(@"\") + 1;
                int titleLength = path.LastIndexOf('.') - titleBeginInd;
                newWindow.Title = path.Substring(titleBeginInd, titleLength);
                ((DrawingCanvas)newWindow.Content).LoadImageFrom(path);
            }
        }

        private void SaveImageAs_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveActiveAs();
        }

        private void SaveImage_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                DrawingCanvas canvasToSave = PaintWindowManager.ActiveCanvas;
                if (canvasToSave.ImagePath != null)
                    canvasToSave.SaveImageTo(canvasToSave.ImagePath);
                else
                    SaveActiveAs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Возникла ошибка при сохранении рисунка\n{ex.Message}");
            }
        }

        private void SaveActiveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "Bitmap(*.bmp)|*.bmp|JPG(*.jpg)|*.jpg";

            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                int titleBeginInd = path.LastIndexOf(@"\") + 1;
                int titleLength = path.LastIndexOf('.') - titleBeginInd;
                PaintWindowManager.ActiveCanvas.SaveImageTo(saveFileDialog.FileName);
                PaintWindowManager.ActiveWindow.Title = path.Substring(titleBeginInd, titleLength);
                PaintWindowManager.ActiveWindow.Title = path.Substring(titleBeginInd, titleLength);
            }
        }

        private void Close_Command_Executed(object sender, ExecutedRoutedEventArgs e) =>
            Close();

        private void ZoomInCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.ActiveCanvas.ZoomIn();

        private void ZoomOutCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.ActiveCanvas.ZoomOut();

        private void ChangeCanvasSizeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Canvas canvas = PaintWindowManager.ActiveCanvas.BodyHolder;
            Window_SizeDialog sizeDialog = new Window_SizeDialog();
            sizeDialog.Title = "Размер холста";
            sizeDialog.SizeWidth = canvas.ActualWidth;
            sizeDialog.SizeHeight = canvas.ActualHeight;

            if (sizeDialog.ShowDialog() == true)
            {
                canvas.Width = sizeDialog.SizeWidth;
                canvas.Height = sizeDialog.SizeHeight;
            }
        }

        private void ChangeWindowSizeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MdiChild window = PaintWindowManager.ActiveWindow;
            Window_SizeDialog sizeDialog = new Window_SizeDialog();
            sizeDialog.Title = "Размер окна";
            sizeDialog.SizeWidth = window.ActualWidth;
            sizeDialog.SizeHeight = window.ActualHeight;

            if (sizeDialog.ShowDialog() == true)
            {
                window.Width = sizeDialog.SizeWidth;
                window.Height = sizeDialog.SizeHeight;
            }
        }

        private void ClearCanvasCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.ActiveCanvas.Clear();

        private void LayoutCascadeCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.WindowLayout = MdiLayout.Cascade;

        private void LayoutHorizontalCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.WindowLayout = MdiLayout.TileHorizontal;

        private void LayoutVerticalCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.WindowLayout = MdiLayout.TileVertical;

        private void LayoutMinimizeCommand_Executed(object sender, ExecutedRoutedEventArgs e) =>
            PaintWindowManager.MinimizeAll();

        private void PaintWindowManager_ActiveWindowChanged(object sender, RoutedEventArgs e)
        {
            if(PaintWindowManager.ActiveWindow == null)
                menu.SetAllWindowRelatedInactive();   
            else
                menu.SetAllWindowRelatedActive();
        }

        private void PaintToolBar_DrawingToolTypeChanged(object sender, RoutedEventArgs e) =>
            DrawingCanvas.DrawingTool = ((PaintToolBar)sender).CurrentTool;

        private void ShowAboutBoxCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dictionary<DrawingCanvas, MdiChild> editedCanvases = new Dictionary<DrawingCanvas, MdiChild>();

            foreach(var window in PaintWindowManager.MDI_Container.Children)
            {
                DrawingCanvas canvas = (DrawingCanvas)window.Content;

                if (canvas.WasEdited)
                    editedCanvases.Add(canvas, window);
            }

            if(editedCanvases.Count > 0)
            {
                MessageBoxResult res = MessageBox.Show("Некоторые рисунки не были сохранены.\nСохранить?",
                    "Внимание",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Warning);

                if (res == MessageBoxResult.Yes)
                {
                    foreach(DrawingCanvas canvas in editedCanvases.Keys)
                    {
                        MessageBoxResult canvasRes = MessageBox.Show($"Вы хотите сохранить изменения в файле {editedCanvases[canvas].Title}?",
                            "Paint 2.0",
                            MessageBoxButton.YesNoCancel,
                            MessageBoxImage.Exclamation,
                            MessageBoxResult.Cancel);

                        if (canvasRes == MessageBoxResult.Yes)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.CheckPathExists = true;
                            saveFileDialog.Filter = "Bitmap(*.bmp)|*.bmp|JPG(*.jpg)|*.jpg";

                            if (saveFileDialog.ShowDialog() == true)
                                canvas.SaveImageTo(saveFileDialog.FileName);
                        }
                        else if (canvasRes == MessageBoxResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                else if(res == MessageBoxResult.Cancel)
                    e.Cancel = true;
            }
        }
    }
}
