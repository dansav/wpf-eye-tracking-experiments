using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Eyetracking.NET;

namespace XRayVision
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IEyetracker _eyetracker;
        private FrameworkElement _rootElement;
        private Vector _lastGazePoint;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private Vector GazePoint => _eyetracker
            .AdaptTo(_rootElement)
            .ToVector();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _eyetracker = new Eyetracker();
            _rootElement = GetVisualChild(0) as FrameworkElement;

            ImageEffect.Texture2 = new ImageBrush(new BitmapImage(new Uri("./Images/wood.jpg", UriKind.Relative)));

            CompositionTarget.Rendering += CompositionTargetOnRendering;
        }

        private void CompositionTargetOnRendering(object sender, EventArgs e)
        {
            var gaze = GazePoint;

            // stay well within the window
            gaze = new Vector(
                Clamp(gaze.X, 0.1f, 0.85f),
                Clamp(gaze.Y, 0.1f, 0.85f));

            ImageEffect.CenterPoint = new Point(gaze.X, gaze.Y);
            if (_lastGazePoint != new Vector())
            {
                var delta = (gaze - _lastGazePoint);
                var speed = Smooth(delta.Length, 0.05f);

                ImageEffect.FuzzyAmount = 0.01 + 5 * speed;
                ImageEffect.CircleSize = 0.8 - 14 * speed;
            }

            _lastGazePoint = gaze;
        }

        private static double _smoothed;
        private static double Smooth(double input, double a)
        {
            _smoothed = _smoothed * (1 - a) + input * a;
            return _smoothed;
        }

        private static double Clamp(double value, double low, double high)
        {
            return Math.Max(low, Math.Min(high, value));
        }
    }
}
