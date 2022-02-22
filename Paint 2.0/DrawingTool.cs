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
    public enum ToolType
    {
        Pen, Line, Ellipse, Star, Eraser
    }

    public abstract class DrawingTool
    {
        public double StrokeThickness { get; set; } = 1;
        public SolidColorBrush StrokeBrush { get; set; } = new SolidColorBrush(Colors.Black);
        public bool HasDrawing { get; private set; } = false;

        protected DrawingCanvas TargetCanvas { get; set; }
        protected Point StartPoint { get; set; }

        public void StartNewDrawing(Point startPos, DrawingCanvas canvas)
        {
            TargetCanvas = canvas;
            StartPoint = startPos;
            HasDrawing = false;
            Initialize();
        }
        public void Draw(Point newPos)
        {
            HasDrawing = true;
            UpdateDrawing(newPos);
        }
        public void Clear()
        {
            HasDrawing = false;
        }
        public abstract UIElement GetDrawnElement();

        protected abstract void Initialize();
        protected abstract void UpdateDrawing(Point newPos);
    }

    public class PenTool : DrawingTool
    {
        private Path path = new Path();
        private PathGeometry geometry = new PathGeometry();
        private Point prevPoint = new Point();

        protected override void Initialize()
        {
            geometry = new PathGeometry();
            path = new Path();
            path.Data = geometry;
            prevPoint = StartPoint;

            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.Stroke = StrokeBrush;
            path.StrokeThickness = StrokeThickness;
            TargetCanvas.Body.Children.Add(path);
        }

        protected override void UpdateDrawing(Point newPos)
        {
            LineGeometry line = new LineGeometry();
            line.StartPoint = prevPoint;
            line.EndPoint = newPos;
            geometry.AddGeometry(line);
            prevPoint = newPos;
            //MessageBox.Show(StartPoint + " " + prevPoint + " " + newPos);
        }

        public override UIElement GetDrawnElement()
        {
            return path;
        }
    }

    public class LineTool : DrawingTool
    {
        private Line line = new Line();

        public static LineTool GetFrom(DrawingTool dt)
        {
            LineTool ps = new LineTool();
            ps.StrokeThickness = dt.StrokeThickness;
            ps.StrokeBrush = dt.StrokeBrush;
            return ps;
        }

        protected override void Initialize()
        {
            line = new Line();
            line.X1 = StartPoint.X;
            line.X2 = StartPoint.X;
            line.Y1 = StartPoint.Y;
            line.Y2 = StartPoint.Y;
            line.StrokeEndLineCap = PenLineCap.Round;
            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeThickness = StrokeThickness;
            line.Stroke = StrokeBrush;
            TargetCanvas.Body.Children.Add(line);
        }

        protected override void UpdateDrawing(Point newPoint)
        {
            line.X2 = newPoint.X;
            line.Y2 = newPoint.Y;
        }

        public override UIElement GetDrawnElement()
        {
            return line;
        }
    }

    public class EllipseTool : DrawingTool
    {
        private Ellipse ellipse = new Ellipse();

        protected override void Initialize()
        {
            ellipse = new Ellipse();
            Canvas.SetLeft(ellipse, StartPoint.X);
            Canvas.SetTop(ellipse, StartPoint.Y);
            ellipse.Width = 0;
            ellipse.Height = 0;
            ellipse.StrokeEndLineCap = PenLineCap.Round;
            ellipse.StrokeStartLineCap = PenLineCap.Round;
            ellipse.StrokeThickness = StrokeThickness;
            ellipse.Stroke = StrokeBrush;
            TargetCanvas.Body.Children.Add(ellipse);
        }

        protected override void UpdateDrawing(Point newPoint)
        {
            Canvas.SetLeft(ellipse, Math.Min(StartPoint.X, newPoint.X));
            Canvas.SetTop(ellipse, Math.Min(StartPoint.Y, newPoint.Y));
            ellipse.Width = Math.Abs(StartPoint.X - newPoint.X);
            ellipse.Height = Math.Abs(StartPoint.Y - newPoint.Y);
        }

        public override UIElement GetDrawnElement()
        {
            return ellipse;
        }
    }

    public class EraserTool : DrawingTool
    {
        private Path path = new Path();
        private PathGeometry geometry = new PathGeometry();
        private Point prevPoint = new Point();

        protected override void Initialize()
        {
            geometry = new PathGeometry();
            path = new Path();
            path.Data = geometry;
            prevPoint = StartPoint;

            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.Stroke = new SolidColorBrush(Colors.White);
            path.StrokeThickness = StrokeThickness;
            TargetCanvas.Body.Children.Add(path);
        }

        protected override void UpdateDrawing(Point newPos)
        {
            LineGeometry line = new LineGeometry();
            line.StartPoint = prevPoint;
            line.EndPoint = newPos;
            geometry.AddGeometry(line);
            prevPoint = newPos;
        }

        public override UIElement GetDrawnElement()
        {
            return path;
        }
    }

    public class StarTool : DrawingTool
    {
        Path path = new Path();
        PathGeometry geometry = new PathGeometry();
        private List<Point> outerPoints = new List<Point>();
        private List<Point> innerPoints = new List<Point>();
        public double RadiusRatio { get; set; }
        public int BeamCount { get; set; }
        public double Rotation { get; set; }

        protected override void Initialize()
        {
            outerPoints.Clear();
            innerPoints.Clear();
            path = new Path();
            geometry = new PathGeometry();
            path.Data = geometry;
            TargetCanvas.Body.Children.Add(path);

            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeStartLineCap = PenLineCap.Round;
            path.Stroke = StrokeBrush;
            path.StrokeThickness = StrokeThickness;

            double angle = Math.PI * 2 / BeamCount;
            double angle90 = Math.PI / 2;
            double rotation = Math.PI * Rotation / 180;
            for(int i = 0; i < BeamCount; i++)
            {
                outerPoints.Add(new Point(Math.Cos(angle*i + angle90 + rotation), Math.Sin(angle* i + angle90 + rotation)));
                innerPoints.Add(new Point(Math.Cos(angle * i + angle/ 2 + angle90 + rotation), 
                    Math.Sin(angle * i + angle/ 2 + angle90 + rotation)));
            }
        }

        protected override void UpdateDrawing(Point newPos)
        {
            double scaleX = newPos.X - StartPoint.X;
            double scaleY = newPos.Y - StartPoint.Y;
            geometry.Clear();

            for(int i = 0; i < BeamCount; i++)
            {
                LineGeometry line = new LineGeometry();
                line.StartPoint = new Point(StartPoint.X + (outerPoints[i].X * scaleX + scaleX)/2,
                    StartPoint.Y + (outerPoints[i].Y * scaleY + scaleY)/2);
                line.EndPoint = new Point(StartPoint.X + (innerPoints[i].X * scaleX * RadiusRatio + scaleX)/2,
                    StartPoint.Y + (innerPoints[i].Y * scaleY * RadiusRatio + scaleY)/2);
                geometry.AddGeometry(line);
            }
            for(int i = 0; i < BeamCount; i++)
            {
                LineGeometry line = new LineGeometry();
                line.StartPoint = new Point(StartPoint.X + (outerPoints[(i + 1) % BeamCount].X * scaleX + scaleX) / 2,
                    StartPoint.Y + (outerPoints[(i + 1) % BeamCount].Y * scaleY + scaleY) / 2);
                line.EndPoint = new Point(StartPoint.X + (innerPoints[i].X * scaleX * RadiusRatio + scaleX) / 2,
                    StartPoint.Y + (innerPoints[i].Y * scaleY * RadiusRatio + scaleY) / 2);
                geometry.AddGeometry(line);
            }
        }

        public override UIElement GetDrawnElement()
        {
            return path;
        }
    }
}
