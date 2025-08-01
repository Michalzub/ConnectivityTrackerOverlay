using pingtest;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pingoverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        POViewmodel Pv;

        public MainWindow(POViewmodel pv)
        {
            InitializeComponent();
            Pv = pv;
            DataContext = Pv;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if(button?.DataContext is ConnectionData cd)
            {
                Pv.DeleteConnection(cd);
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonPing(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if(button.Content.ToString() == "Ping")
            {
                button.Content = "Stop";
                Pv.PingConnections();
            } else
            {
                button.Content = "Ping";
                Pv.PingConnections();
            }
        }

        private void AddConnectionButtonClick(object sender, RoutedEventArgs e)
        {
            Pv.AddConnection(connectionInputBox.Text);
            connectionInputBox.Text = "";

        }

        private void Export(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "POips";
            dlg.DefaultExt = ".text";
            dlg.Filter = "Text documents (.txt)|*.txt";


            if (dlg.ShowDialog() == true)
            {
                Pv.SaveToFile(dlg.FileName);
            }
        }

        private void Import(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".text";
            dlg.Filter = "Text documents (.txt)|*.txt";

            if(dlg.ShowDialog() == true)
            {
                Pv.ReadFromFile(dlg.FileName);
            }
        }
    }
}