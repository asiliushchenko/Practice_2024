using System;
using System.Threading;
using System.Windows;

namespace NoSqlDatabaseWPF
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var thread = new Thread(() =>
            {
                var app = new App();
                var mainWindow = new MainWindow();
                app.Run(mainWindow);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
