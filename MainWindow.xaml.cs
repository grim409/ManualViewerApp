using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ManualViewerApp
{
    public partial class MainWindow : Window
    {
        private readonly string _cfgPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "launcher.json");

        public MainWindow()
        {
            InitializeComponent();
            LoadPosition();
        }

        private void LoadPosition()
        {
            if (File.Exists(_cfgPath))
            {
                try
                {
                    var cfg = JsonSerializer.Deserialize<Config>(
                        File.ReadAllText(_cfgPath));
                    if (cfg is not null)
                    {
                        Left = cfg.X;
                        Top  = cfg.Y;
                    }
                }
                catch { /* malformed JSON? ignore */ }
            }
            else
            {
                // default center
                Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
                Top  = (SystemParameters.PrimaryScreenHeight - Height) / 2;
            }
        }

        // Invoked by the ContextMenu → Settings...
        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsWindow(_cfgPath, Left, Top)
            {
                Owner = this
            };
            if (dlg.ShowDialog() == true)
            {
                // Re-load the new position
                LoadPosition();
            }
        }

        private void LauncherButton_Click(object sender, RoutedEventArgs e)
        {
            var menu = new ManualsMenuWindow { Owner = this };
            menu.ShowDialog();
        }

        private record Config
        {
            public double X { get; init; }
            public double Y { get; init; }
        }
    }
}
