using System;
using System.Windows;
using PdfiumViewer;
using System.Windows.Forms.Integration;
using WinForms = System.Windows.Forms;

namespace ManualViewerApp
{
    public partial class PdfViewerWindow : Window
    {
        // Keep the document alive for the window's lifetime
        private readonly PdfDocument _document;

        public PdfViewerWindow(string path)
        {
            InitializeComponent();

            try
            {
                // Load once and store in a field
                _document = PdfDocument.Load(path);

                // Create a bare‐bones renderer (no toolbar, no print)
                var renderer = new PdfRenderer
                {
                    Dock = WinForms.DockStyle.Fill
                };

                // Load the document into the renderer
                renderer.Load(_document);

                // Host it
                PdfHost.Child = renderer;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load PDF:\n{ex.GetType().Name}: {ex.Message}",
                    "PDF Load Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Dispose the WinForms renderer child first
            if (PdfHost.Child is IDisposable disp) disp.Dispose();

            // Then dispose the document
            _document?.Dispose();
        }
    }
}
