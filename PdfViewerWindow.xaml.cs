using System.Windows;
using System.Windows.Forms.Integration;
using PdfiumViewer;

namespace ManualViewerApp
{
    public partial class PdfViewerWindow : Window
    {
        public PdfViewerWindow(string path)
        {
            InitializeComponent();

            var viewer = new PdfViewer {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Document = PdfDocument.Load(path)
            };

            var host = new WindowsFormsHost { Child = viewer };
            PdfHost.Controls.Add(host.Child);
        }
    }
}
