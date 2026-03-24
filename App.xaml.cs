using System;
using System.Diagnostics;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using MessageBoxButton = System.Windows.MessageBoxButton;
using MessageBoxImage = System.Windows.MessageBoxImage;

namespace PriceScanner
{
    public partial class App : Application
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            DispatcherUnhandledException += OnDispatcherException;

            // ✅ تحميل الترخيص والإعدادات عند البدء
            Core.LicenseService.Instance.LoadLicense();
            Core.AppState.Instance.LoadFromSettings();

            base.OnStartup(e);
        }

        protected override void OnExit(System.Windows.ExitEventArgs e)
        {
            Core.AppState.Instance.SaveToSettings();
            base.OnExit(e);
        }

        private void OnDispatcherException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"خطأ غير متوقع:\n{e.Exception.Message}",
                "خطأ", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                MessageBox.Show($"خطأ فادح:\n{ex.Message}", "خطأ فادح",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}