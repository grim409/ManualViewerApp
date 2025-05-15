using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ManualViewerApp
{
    public partial class ManualsMenuWindow : Window
    {
        private readonly string _rootFolder;
        private string _currentFolder;

        public ManualsMenuWindow()
        {
            InitializeComponent();

            // Point at your “Manuals” directory next to the EXE
            _rootFolder    = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Manuals");
            _currentFolder = _rootFolder;

            ReloadView();
        }

        private void ReloadView()
        {
            StackPanelContainer.Children.Clear();

            // Back button if we’re in a subfolder
            if (!string.Equals(_currentFolder, _rootFolder, StringComparison.OrdinalIgnoreCase))
            {
                var backBtn = new Button
                {
                    Content   = "<< Back",
                    Margin    = new Thickness(5),
                    MinWidth  = 100,
                    MinHeight = 40
                };
                backBtn.Click += (s, e) =>
                {
                    _currentFolder = _rootFolder;
                    ReloadView();
                };
                StackPanelContainer.Children.Add(backBtn);
            }

            // First, look for subdirectories
            var dirs = Directory.GetDirectories(_currentFolder);
            if (dirs.Any())
            {
                // Show each subfolder
                foreach (var dir in dirs)
                {
                    var name = Path.GetFileName(dir);
                    var btn  = new Button
                    {
                        Content   = $"📁 {name}",
                        Tag       = dir,
                        Margin    = new Thickness(5),
                        MinWidth  = 200,
                        MinHeight = 50
                    };
                    btn.Click += FolderButton_Click;
                    StackPanelContainer.Children.Add(btn);
                }
                return;
            }

            // No subfolders: show PDFs in this folder
            var files = Directory.GetFiles(_currentFolder, "*.pdf");
            if (!files.Any())
            {
                MessageBox.Show(
                    $"No PDF manuals found in:\n{_currentFolder}",
                    "Empty",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }

            foreach (var file in files)
            {
                var display = Path.GetFileNameWithoutExtension(file);
                var btn     = new Button
                {
                    Content   = display,
                    Tag       = file,
                    Margin    = new Thickness(5),
                    MinWidth  = 200,
                    MinHeight = 50
                };
                btn.Click += ManualButton_Click;
                StackPanelContainer.Children.Add(btn);
            }
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Drill into the selected subfolder
            _currentFolder = (string)((Button)sender).Tag;
            ReloadView();
        }

        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the PDF viewer for this file
            var path   = (string)((Button)sender).Tag;
            var viewer = new PdfViewerWindow(path)
            {
                Owner = this
            };
            viewer.ShowDialog();
        }
    }
}
