using System.Configuration;
using System.Data;
using System.Windows;

namespace pingoverlay
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            POViewmodel Pv = new POViewmodel();

            MainWindow mw = new MainWindow(Pv);
            Compact compact = new Compact(Pv);

            this.MainWindow = mw;

            mw.Show();
            compact.Show();
        }
    }

}
