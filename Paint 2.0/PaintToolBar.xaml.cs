using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace Paint_2._0
{
    public partial class PaintToolBar : UserControl
    {
        public event RoutedEventHandler DrawingToolTypeChanged;
        private DrawingTool currentTool;

        public DrawingTool CurrentTool
        {
            get
            {
                return currentTool;
            } 
            private set
            {
                currentTool = value;
                GetToolFromToolBar();
                DrawingToolTypeChanged?.Invoke(this, new RoutedEventArgs());
            }
        }

        public PaintToolBar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PenButton.IsChecked = true;
        }



        private void StrokeThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void StrokeThicknessSlider_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.MessageBox.Show("");
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (CurrentTool == null)
                return;

            if (StrokeColorPicker.SelectedColor == null)
                CurrentTool.StrokeBrush.Color = Colors.Transparent;
            else
                CurrentTool.StrokeBrush.Color = (Color)StrokeColorPicker.SelectedColor;
        }

        private void PenButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new PenTool();
        }

        private void EraserButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new EraserTool();
        }

        private void LineButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new LineTool();
        }

        private void EllipseButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new EllipseTool();
        }

        private void StarButton_Checked(object sender, RoutedEventArgs e)
        {
            CurrentTool = new StarTool();
        }

        private void GetToolFromToolBar()
        {
            CurrentTool.StrokeThickness = (int)StrokeThicknessSelector.Value;

            if(StrokeColorPicker.SelectedColor == null)
                CurrentTool.StrokeBrush.Color = Colors.Transparent;
            else
                CurrentTool.StrokeBrush.Color = (Color)StrokeColorPicker.SelectedColor;

            if(CurrentTool is StarTool)
            {
                ((StarTool)CurrentTool).BeamCount = (int)BeamCountSelector.Value;
                ((StarTool)CurrentTool).RadiusRatio = (int)RadiusRatioSelector.Value / 100d;
                ((StarTool)CurrentTool).Rotation = (int)StarRotationSelector.Value;
            }
        }

        private void StrokeThicknessSelector_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CurrentTool != null)
                CurrentTool.StrokeThickness = (int)StrokeThicknessSelector.Value;
        }

        private void BeamCountSelector_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(CurrentTool is StarTool)
                ((StarTool)CurrentTool).BeamCount = (int)BeamCountSelector.Value;
        }

        private void RadiusRatioSelector_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CurrentTool is StarTool)
                ((StarTool)CurrentTool).RadiusRatio = (int)RadiusRatioSelector.Value / 100d;
        }

        private void StarRotationSelector_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (CurrentTool is StarTool)
                ((StarTool)CurrentTool).Rotation = (int)StarRotationSelector.Value;
        }
    }
}
