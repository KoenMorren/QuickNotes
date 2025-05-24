using QuickNoteApp.Configuration;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace QuickNoteApp
{
    public partial class MainWindow : Window
    {
        private NotifyIcon trayIcon;
        private readonly string noteFilePath;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTray();


            var config = new ConfigHelper().LoadConfig();
            noteFilePath = config.FilePath;

            if (config is not null)
            {
                if (config.FilePath != null)
                {
                    noteFilePath = config.FilePath;
                }

                ApplyFontConfig(config.Font);
            }

            LoadNote();

            TextEditor.CaretIndex = TextEditor.Text.Length;
        }

        private void ApplyFontConfig(FontConfig fontConfig)
        {
            if (fontConfig == null)
            {
                return;
            }

            TextEditor.FontFamily = new System.Windows.Media.FontFamily(fontConfig.FontName);
            TextEditor.FontSize = fontConfig.FontSize;
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