using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace ManualViewerApp
{
    public partial class MainWindow : Window
    {
        private readonly string _cfg = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, 
            "launcher.json"
        );

        public MainWindow()
        {
            InitializeComponent();
            LoadPosition();
        }

        private void LoadPosition()
        {
            if (File.Exists(_cfg))
            {
                var c = JsonSerializer.Deserialize<Config>(File.ReadAllText(_cfg));
                if (c is not null) { Left = c.X; Top = c.Y; }
            }
        }

        private void SavePosition()
        {
            var c = new Config { X = Left, Y = Top };
            File.WriteAllText(_cfg, JsonSerializer.Serialize(c));
        }

        private void Window_MouseLeftButtonDown(object s, MouseButtonEventArgs e)
        {
            DragMove();
            SavePosition();
        }

        private void LauncherButton_Click(object s, RoutedEventArgs e)
        {
            var menu = new ManualsMenuWindow { Owner = this };
            menu.ShowDialog();
        }

        private record Config { public double X { get; init; } public double Y { get; init; } }
    }
}
