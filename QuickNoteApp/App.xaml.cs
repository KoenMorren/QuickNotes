using System.Runtime.InteropServices;
using System;
using System.Windows;
using System.Windows.Interop;

namespace QuickNoteApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const int HOTKEY_ID = 9000;
    private const uint MOD_ALT = 0x0001;
    private const uint VK_OEM_3 = 0xC0; // `~` key

    private HwndSource _source;
    private MainWindow _mainWindow;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _mainWindow = new MainWindow();
        _mainWindow.Hide();

        // Create an invisible window to register hotkey early
        var parameters = new HwndSourceParameters("HiddenHotkeyHandler")
        {
            Width = 0,
            Height = 0,
            PositionX = 0,
            PositionY = 0,
            WindowStyle = unchecked((int)0x80000000), // WS_POPUP
        };

        _source = new HwndSource(parameters);
        _source.AddHook(HwndHook);
        RegisterHotKey(_source.Handle, HOTKEY_ID, MOD_ALT, VK_OEM_3);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_source != null)
        {
            UnregisterHotKey(_source.Handle, HOTKEY_ID);
            _source.Dispose();
        }

        base.OnExit(e);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;
        if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
        {
            _mainWindow.Dispatcher.Invoke(() =>
            {
                _mainWindow.ToggleEditor();
            });
            handled = true;
        }
        return IntPtr.Zero;
    }
}

