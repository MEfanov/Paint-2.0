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

namespace Paint_2._0
{
    public partial class DrawingCanvas : UserControl
    {
        private bool isDrawing = false;
        private bool isMoving = false;
        private Point windowMousePos = new Point(0, 0);
        private double zoom = 1;
        private Image fullImage = new Image();

        public bool WasEdited { get; private set; }
        public string? ImagePath { get; private set; }

        public double Zoom 
        { 
            get
            { 
                return zoom;
            }
            private set
            {
                zoom = value;
                CanvasScaleTransform.ScaleX = zoom;
                CanvasScaleTransform.ScaleY = zoom;
            }
        }

        public static DrawingTool DrawingTool { get; set; } = new PenTool();

        public DrawingCanvas()
        {
            InitializeComponent();
            Body.Children.Add(fullImage);
            RenderOptions.SetBitmapScalingMode(fullImage, BitmapScalingMode.NearestNeighbor);
        }

        public void Clear()
        {
            Body.Children.Clear();
            fullImage.Source = null;
            Body.Children.Add(fullImage);
        }

        public void ZoomIn()
        {
            Zoom *= 2;
        }

        public void ZoomOut()
        {
            Zoom /= 2;
        }

        public Image GetDrawing()
        {
            Image image = new Image();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)Body.ActualWidth + 1, (int)Body.ActualHeight + 1,
                96, 96, PixelFormats.Pbgra32);
            bmp.Render(Body);
            image.Source = bmp;
            return image;
        }

        public void SaveImageTo(string path)
        {
            //MessageBox.Show(path);
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)GetDrawing().Source));
                    encoder.Save(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи в файл.{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            ImagePath = path;
            WasEdited = false;
        }

        public void LoadImageFrom(string path)
        {
            //MessageBox.Show(path);
            //fullImage.Source = new BitmapImage(new Uri(path));
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
                {
                    var image = new BitmapImage();

                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();

                    image.Freeze();
                    fullImage.Source = image;
                }

                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    BitmapFrame frame = BitmapFrame.Create(fileStream, BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
                    Size s = new Size(frame.PixelWidth, frame.PixelHeight);
                    Body.Width = s.Width;
                    Body.Height = s.Height;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении из файла.\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ImagePath = path;
            WasEdited = false;
        }

        private void RenderDrawing()
        {
            Rect rect = new Rect(0,
                0,
                Math.Max((int)Body.ActualWidth + 1, (int)fullImage.ActualWidth + 1),
                Math.Max((int)Body.ActualHeight + 1, (int)fullImage.ActualHeight + 1));
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)rect.Right, (int)rect.Bottom,
                96, 96, PixelFormats.Pbgra32);
            bmp.Render(fullImage);
            bmp.Render(DrawingTool.GetDrawnElement());
            fullImage.Source = bmp;
            DrawingTool.Clear();
            Body.Children.Clear();
            Body.Children.Add(fullImage);
            WasEdited = true;
        }

        private void Body_MouseMove(object sender, MouseEventArgs e)
        {
            Point newWindowMousePos = e.GetPosition(this);

            if (isDrawing)
                DrawingTool.Draw(e.GetPosition(Body));
            else if (isMoving)
                Move(windowMousePos, newWindowMousePos);

            windowMousePos = newWindowMousePos;
        }

        private void Move(Point from, Point to)
        {
            CanvasTranslateTransform.X += to.X - from.X;
            CanvasTranslateTransform.Y += to.Y - from.Y;
        }

        private void Body_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!isMoving && e.ChangedButton == MouseButton.Left)
            {
                isDrawing = true;
                if (DrawingTool.HasDrawing)
                    RenderDrawing();
                DrawingTool.StartNewDrawing(e.GetPosition(Body), this);
            }
            if (!isDrawing && e.ChangedButton == MouseButton.Middle)
            {
                isMoving = true;
                Cursor = Cursors.SizeAll;
            }
        }

        private void Body_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing && e.ChangedButton == MouseButton.Left)
            {
                isDrawing = false;
                if(DrawingTool.HasDrawing)
                    RenderDrawing();
            }
            if (isMoving && e.ChangedButton == MouseButton.Middle)
            {
                isMoving = false;
                Cursor = Cursors.Arrow;
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (DrawingTool.HasDrawing && isDrawing)
                RenderDrawing();
            isDrawing = false;
            isMoving = false;
        }

        private void Body_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeBox.Text = e.NewSize.ToString();
        }
    }
}
