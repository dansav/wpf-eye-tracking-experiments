using System;
using System.Windows;
using Eyetracking.NET;

namespace XRayVision
{
    public static class WpfEyetrackerExtensions
    {
        public static Vector ToVector(this IEyetracker eyetracker)
        {
            return new Vector(eyetracker.X, eyetracker.Y);
        }

        public static IEyetracker AdaptTo(this IEyetracker eyetracker, FrameworkElement elm)
        {
            var elementScreenBounds = new Rect(
                elm.PointToScreen(new Point(0, 0)),
                elm.PointToScreen(new Point(elm.ActualWidth, elm.ActualHeight)));

            var w = System.Windows.Forms.Screen.PrimaryScreen;
            var gazeInPixels = new Point(
                eyetracker.X * w.Bounds.Width,
                eyetracker.Y * w.Bounds.Height);

            var relativeGaze = Clamp(gazeInPixels - elementScreenBounds.TopLeft, elementScreenBounds.Size);

            return new ClampedTracker(
                (float)(relativeGaze.X / elementScreenBounds.Width),
                (float)(relativeGaze.Y / elementScreenBounds.Height));
        }

        private static Vector Clamp(Vector vector, Size size)
        {
            return new Vector(
                Math.Min(size.Width, Math.Max(0, vector.X)),
                Math.Min(size.Height, Math.Max(0, vector.Y))
                );
        }

        private struct ClampedTracker : IEyetracker
        {
            public ClampedTracker(float x, float y)
            {
                X = x;
                Y = y;
            }

            public float X { get; }
            public float Y { get; }
        }
    }
}