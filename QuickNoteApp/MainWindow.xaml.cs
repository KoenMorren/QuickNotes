using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace QuickNoteApp
{
    public partial class MainWindow : Window
    {
        private NotifyIcon trayIcon;
        private string noteFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "quicknote.txt");

        public MainWindow()
        {
            InitializeComponent();
            InitializeTray();
            LoadNote();
        }

        private void InitializeTray()
        {
            trayIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("TrayIcon_v2.ico"), // Add your icon here
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };
            trayIcon.ContextMenuStrip.Items.Add("Open", null, (s, e) => ToggleEditor());
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => ExitApp());
            trayIcon.DoubleClick += (object? sender, EventArgs e) =>
            {
                ToggleEditor();
            };
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            this.Deactivated += (s, e) =>
            {
                SaveNote();
                this.Hide();
            };
        }

        public void ToggleEditor()
        {
            if (IsVisible)
            {
                this.Hide();
                return;
            }

            LoadNote();
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
            this.Activate();
            this.Topmost = true;  // temporarily topmost
            this.Topmost = false; // then not topmost to allow focus to stick

            TextEditor.Focus();
            TextEditor.CaretIndex = TextEditor.Text.Length;
        }

        private void LoadNote()
        {
            if (File.Exists(noteFilePath))
                TextEditor.Text = File.ReadAllText(noteFilePath);
        }

        private void SaveNote()
        {
            File.WriteAllText(noteFilePath, TextEditor.Text);
        }

        private void ExitApp()
        {
            SaveNote();
            trayIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            SaveNote();

            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
                trayIcon = null;
            }
        }

        public void TextEditor_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}