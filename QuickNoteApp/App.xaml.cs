using QuickNoteApp.Configuration;
using QuickNoteApp.Shortcut;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;

namespace QuickNoteApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private const int HOTKEY_ID = 9000;
    //private const uint MOD_ALT = 0x0001;
    //private const uint VK_OEM_3 = 0xC0; // `~` key

    private HwndSource _source;
    private MainWindow _mainWindow;

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

        var config = new ConfigHelper().LoadConfig();
        if (config is not null)
        {
            var keyGesture = ShortcutParser.Parse(config.Shortcut);

            _source = new HwndSource(parameters);
            _source.AddHook(HwndHook);

            bool registered = ShortcutManager.RegisterHotKey(_source.Handle, HOTKEY_ID, ShortcutManager.GetModifierFlags(keyGesture!.modifiers), (uint)KeyInterop.VirtualKeyFromKey(keyGesture!.key));

            if (!registered)
                Console.WriteLine("Hotkey registration failed.");
        }

        //_source = new HwndSource(parameters);
        //_source.AddHook(HwndHook);
        //RegisterHotKey(_source.Handle, HOTKEY_ID, MOD_ALT, VK_OEM_3);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        if (_source != null)
        {
            ShortcutManager.UnregisterHotKey(_source.Handle, HOTKEY_ID);
            _source.Dispose();
        }

        base.OnExit(e);
    }

    private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == ShortcutManager.WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
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

