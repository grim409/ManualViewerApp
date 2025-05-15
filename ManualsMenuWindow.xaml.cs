using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ManualViewerApp
{
    public partial class ManualsMenuWindow : Window
    {
        public ManualsMenuWindow()
        {
            InitializeComponent();
            foreach (var pdf in Directory.GetFiles(
                       AppDomain.CurrentDomain.BaseDirectory + "Manuals\\", "*.pdf"))
            {
                var btn = new Button {
                    Content   = Path.GetFileNameWithoutExtension(pdf),
                    Tag       = pdf,
                    Margin    = new Thickness(5),
                    MinWidth  = 200,
                    MinHeight = 50
                };
                btn.Click += Manual_Click;
                StackPanelContainer.Children.Add(btn);
            }
        }

        private void Manual_Click(object s, RoutedEventArgs e)
        {
            var path = (string)((Button)s).Tag;
            var viewer = new PdfViewerWindow(path) { Owner = this };
            viewer.ShowDialog();
        }
    }
}
