using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using Jaze.Domain;
using Jaze.Search;
using Jaze.Views;

namespace Jaze
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string BaseDictionary => AppDomain.CurrentDomain.BaseDirectory;
        public App()
        {
            InitializeComponent();
        }

        private SplashWindow splashWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (IsDupplicate())
            {
                MessageBox.Show("Application is already running...");
                Current.Shutdown();
            }
            else
            {
                splashWindow = new SplashWindow();
                splashWindow.Show();
                Thread thread = new Thread(InitData);
                thread.Start();
            }
        }

        private void InitData()
        {
            JazeDatabaseContext.Context.Radicals.Load();
            JazeDatabaseContext.Context.Kanjis.Load();
            Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var mainWindow = new MainWindow();
                this.MainWindow = mainWindow;
                mainWindow.Show();
                splashWindow.Close();
                splashWindow = null;
            }));
        }

        private bool IsDupplicate()
        {
            Process proc = Process.GetCurrentProcess();
            int count = Process.GetProcesses().Count(p => p.ProcessName == proc.ProcessName);
            return count > 1;
        }
    }
}