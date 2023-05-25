using Microsoft.Win32;
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

namespace CustomShaderEffectTest {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void Load_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog().Value) {
                var bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                image.Source = bitmapImage;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            DrawingVisual dv = new DrawingVisual() {
                Effect = image.Effect
            };
            using (DrawingContext dc = dv.RenderOpen()) {
                dc.DrawImage(image.Source, new Rect(0, 0, image.Source.Width, image.Source.Height));
            }
            var renderTargetBitmap = new RenderTargetBitmap((int)image.Source.Width, (int)image.Source.Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(dv);

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog().Value) {
                using (var fileStream = new System.IO.FileStream(saveFileDialog.FileName, System.IO.FileMode.Create)) {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                    encoder.Save(fileStream);
                }
            }
        }
    }
}
