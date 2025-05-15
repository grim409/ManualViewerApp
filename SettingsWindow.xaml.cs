using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ManualViewerApp
{
    public partial class SettingsWindow : Window
    {
        private readonly string _cfgPath;

        public SettingsWindow(string cfgPath, double currentX, double currentY)
        {
            InitializeComponent();
            _cfgPath    = cfgPath;
            XTextBox.Text = currentX.ToString("F0");
            YTextBox.Text = currentY.ToString("F0");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(XTextBox.Text, out var x) &&
                double.TryParse(YTextBox.Text, out var y))
            {
                var cfg = new { X = x, Y = y };
                File.WriteAllText(_cfgPath, JsonSerializer.Serialize(cfg));
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for X and Y.", 
                                "Invalid Input", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Warning);
            }
        }
    }
}
