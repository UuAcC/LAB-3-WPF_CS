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
using System.Windows.Threading;

namespace LAB_3_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd;
        DispatcherTimer timer;
        List<Rectangle> rectangles;
        bool started;
        public MainWindow()
        {
            InitializeComponent();
            Window1 w = new Window1();
            bool? res = w.ShowDialog();
            if (res == true)
            {
                rnd = new Random();
                timer = new DispatcherTimer();
                rectangles = new List<Rectangle>(); 
                started = false;
                timer.Interval = TimeSpan.FromMilliseconds(700);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            else { this.Close(); }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.grid.Children.Count >= 10) { this.started = true; }
            if (this.grid.Children.Count == 20) { this.stop("поражение"); }
            CreateFigure09();
        }

        private void CreateFigure09()
        {
            Rectangle rect = new Rectangle();

            int lg = 200; int st = 40;
            int rotation = rnd.Next(2);
            int x, y;
            if (rotation == 0)
            {
                rect.Height = lg; rect.Width = st;
                x = rnd.Next((int)this.Width - 3 * st / 2);
                y = rnd.Next((int)this.Height - 3 * lg / 2);
            }
            else
            {
                rect.Height = st; rect.Width = lg;
                x = rnd.Next((int)this.Width - 3 * lg / 2);
                y = rnd.Next((int)this.Height - 3 * st / 2);
            }
            byte r = (byte)rnd.Next(255);
            byte g = (byte)rnd.Next(255);
            byte b = (byte)rnd.Next(255);
            rect.Margin = new Thickness(x, y, 0, 0);
            rect.HorizontalAlignment = HorizontalAlignment.Left;
            rect.VerticalAlignment = VerticalAlignment.Top;
            rect.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
            rect.Stroke = Brushes.Black;

            grid.Children.Add(rect);
            rectangles.Add(rect);
            rect.MouseDown += Rect_MouseDown;
        }

        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!started) { return; }
            Rectangle rect = (Rectangle)sender;
            int iRect = rectangles.IndexOf(rect);
            bool check = true;
            for (int i = iRect + 1; i < rectangles.Count; i++)
            {
                if (intersection(rect, rectangles[i]))
                {
                    check = false; break;
                }
            }
            if (check)
            {
                rectangles.Remove(rect);
                grid.Children.Remove(rect);
            }
            if ((rectangles.Count == 0)) { this.stop("победа"); }
        }

        private bool intersection(Rectangle r1, Rectangle r2)
        {
            return GetRect(r1).IntersectsWith(GetRect(r2));
        }

        private void stop(string res)
        {
            timer.Stop();
            Window2 w = new Window2(res);
            w.ShowDialog();
            this.Close();
        }

        private static Rect GetRect(Rectangle rectangle)
        {
            return new Rect(
                rectangle.Margin.Left,
                rectangle.Margin.Top,
                rectangle.ActualWidth,
                rectangle.ActualHeight
            );
        }
    }
}
