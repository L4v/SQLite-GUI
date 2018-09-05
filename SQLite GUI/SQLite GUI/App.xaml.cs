using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SQLite_GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var dialog = new LoadDatabaseWindow();

            /*if (dialog.ShowDialog() == true)
            {
                var main = new MainWindow(dialog.GetDatabase());

                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = main;
                main.Show();
            }*/
            if (dialog.ShowDialog() == true)
            {
                MainWindow main = new MainWindow(dialog.GetDatabase());
                main.Show();
                TODO AAAAAAAAAAAAAAAAAAAAAA POPRAVITI OVAJ NERED
            }
        }
    }
}
