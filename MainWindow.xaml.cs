using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace atomic_clock
{
    public partial class MainWindow : Window
    {
        private long _offsetTicks = 0;
        private string _selectedServer = "time.google.com";
        private DateTime? _targetTime = null;
        private bool _isLocked = false;
        private bool _beeped = false;

        public MainWindow()
        {
            InitializeComponent();

            SettingsManager.Load();
            this.Left = SettingsManager.Data.WindowLeft;
            this.Top = SettingsManager.Data.WindowTop;
            this.Opacity = SettingsManager.Data.WindowOpacity;
            _selectedServer = SettingsManager.Data.LastServer;

            CompositionTarget.Rendering += UpdateLoop;
            _ = AutoSyncLoop();
        }

        private void UpdateLoop(object? sender, EventArgs e)
        {
            DateTime atomicNow = new DateTime(DateTime.Now.Ticks + _offsetTicks);
            ClockText.Text = atomicNow.ToString("HH:mm:ss");
            MsText.Text = "." + atomicNow.ToString("fff");
            if (_targetTime.HasValue) HandleCountdown(atomicNow);
        }

        private void HandleCountdown(DateTime now)
        {
            TimeSpan diff = _targetTime.Value - now;
            if (diff.TotalSeconds <= 0)
            {
                CountdownText.Text = "VIA! CLICK!";
                MainBorder.BorderBrush = Brushes.Lime;
                return;
            }
            CountdownText.Text = $"T- {diff:hh\\:mm\\:ss\\.fff}";
            if (diff.TotalSeconds < 5) MainBorder.BorderBrush = Brushes.Red;

            if (AudioMenu.IsChecked && diff.TotalSeconds <= 3 && now.Millisecond < 50 && !_beeped)
            {
                System.Media.SystemSounds.Hand.Play();
                _beeped = true;
            }
            else if (now.Millisecond > 500) _beeped = false;
        }

        private async Task AutoSyncLoop()
        {
            while (true)
            {
                try
                {
                    _offsetTicks = await GetNtpOffset(_selectedServer);
                    StatusText.Text = $"Server: {_selectedServer} | Sync OK";
                }
                catch { StatusText.Text = "Sync Error - Retrying..."; }
                await Task.Delay(30000);
            }
        }

        private async Task<long> GetNtpOffset(string host)
        {
            var data = new byte[48]; data[0] = 0x1B;
            var adr = await Dns.GetHostAddressesAsync(host);
            using var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.ReceiveTimeout = 3000;
            await sock.ConnectAsync(new IPEndPoint(adr[0], 123));
            Stopwatch sw = Stopwatch.StartNew();
            sock.Send(data); sock.Receive(data);
            sw.Stop();

            ulong intP = (ulong)data[40] << 24 | (ulong)data[41] << 16 | (ulong)data[42] << 8 | (ulong)data[43];
            ulong fracP = (ulong)data[44] << 24 | (ulong)data[45] << 16 | (ulong)data[46] << 8 | (ulong)data[47];
            var ms = (intP * 1000) + ((fracP * 1000) / 0x100000000L);
            DateTime netTime = new DateTime(1900, 1, 1).AddMilliseconds(ms).ToLocalTime();
            return (netTime.Ticks + (sw.ElapsedTicks / 2)) - DateTime.Now.Ticks;
        }

        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr h, int n);
        [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr h, int n, int dw);
        [DllImport("user32.dll")] private static extern bool RegisterHotKey(IntPtr h, int id, uint mod, uint vk);

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // Salvataggio nel file config.json
            SettingsManager.Data.WindowLeft = this.Left;
            SettingsManager.Data.WindowTop = this.Top;
            SettingsManager.Data.WindowOpacity = this.Opacity;
            SettingsManager.Data.LastServer = _selectedServer;
            SettingsManager.Save();
            base.OnClosing(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            RegisterHotKey(hwnd, 9000, 0x0001, 0x4B); // ALT+K
            HwndSource.FromHwnd(hwnd).AddHook((IntPtr h, int m, IntPtr w, IntPtr l, ref bool hand) => {
                if (m == 0x0312 && w.ToInt32() == 9000) ToggleLock();
                return IntPtr.Zero;
            });
        }

        private void ToggleLock()
        {
            _isLocked = !_isLocked;
            var hwnd = new WindowInteropHelper(this).Handle;
            int style = GetWindowLong(hwnd, -20);
            SetWindowLong(hwnd, -20, _isLocked ? (style | 0x20) : (style & ~0x20));
            MainBorder.BorderBrush = _isLocked ? Brushes.DimGray : Brushes.SpringGreen;
        }

        private void SetTarget_Click(object s, RoutedEventArgs e)
        {
            var dialog = new SetTargetWindow();
            if (dialog.ShowDialog() == true)
            {
                if (TimeSpan.TryParse(dialog.TargetResult, out TimeSpan ts))
                {
                    DateTime now = DateTime.Now;
                    // Creiamo il target per la giornata odierna con l'orario scelto
                    _targetTime = new DateTime(now.Year, now.Month, now.Day, ts.Hours, ts.Minutes, ts.Seconds);

                    // Se l'orario inserito è già passato per oggi, assumiamo sia per domani
                    if (_targetTime < now) _targetTime = _targetTime.Value.AddDays(1);

                    MainBorder.BorderBrush = Brushes.Orange; // Feedback visivo: countdown attivo
                }
                else
                {
                    MessageBox.Show("Formato orario non valido! Usa HH:mm:ss (es. 10:00:00)");
                }
            }
        }
        private void SetServer_Click(object s, RoutedEventArgs e)
        {
            if (s is MenuItem mi && mi.Tag != null) { _selectedServer = mi.Tag.ToString()!; _ = AutoSyncLoop(); }
        }
        private void SyncButton_Click(object s, RoutedEventArgs e) => _ = AutoSyncLoop();
        private void About_Click(object s, RoutedEventArgs e) => new AboutWindow().ShowDialog();
        private void Window_MouseWheel(object s, MouseWheelEventArgs e) => this.Opacity = Math.Clamp(this.Opacity + (e.Delta > 0 ? 0.1 : -0.1), 0.2, 1.0);
        private void Window_MouseDown(object s, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed && !_isLocked) DragMove(); }
        private void Lock_Click(object s, RoutedEventArgs e) => ToggleLock();
        private void Close_Click(object s, RoutedEventArgs e) => Application.Current.Shutdown();

        private void RemoveTarget_Click(object s, RoutedEventArgs e)
        {
            _targetTime = null;
            CountdownText.Text = "Pro Atomic Clock";
            MainBorder.BorderBrush = _isLocked ? Brushes.DimGray : Brushes.SpringGreen;
        }
    }
}