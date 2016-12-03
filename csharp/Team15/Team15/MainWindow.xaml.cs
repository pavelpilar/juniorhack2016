using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Team15.Model;

namespace Team15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Core core;

        public MainWindow()
        {
            core = new Core();

            InitializeComponent();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string[] serialPorts = core.FindPossibleConections();
            if (serialPorts.Length == 0)
            {
                MessageBox.Show("Nenalezeno žádné spojení, zkontrolujte kabely  ", "Chyba", MessageBoxButton.OK);
            }
            else if(serialPorts.Length == 1)
            {
                core.StartServer(serialPorts[0], "URL(zaim k ničemu)");
                core.OnDisconnect = () => { OnDisconnestEvent(); };
                FindButton.Visibility = Visibility.Hidden;
                MainGrid.Visibility = Visibility.Visible;
                TemperatureMaxTextBlock.Text = core.ActualSettings.TemperatureMax.ToString();
                TemperatureMinTextBlock.Text = core.ActualSettings.TemperatureMin.ToString();
                WetnessMaxTextBlock.Text = core.ActualSettings.AirConditioningMax.ToString();
                WetnessMinTextBlock.Text = core.ActualSettings.AirConditioningMin.ToString();
            }
            else
            {
                FindGrid.Visibility = Visibility.Hidden;
                PossibleConnectionsGrid.Visibility = Visibility.Visible;
                PossibleConestionListBox.ItemsSource = (new List<string>(serialPorts));
                FindButton.Visibility = Visibility.Hidden;
                PossibleConnectionsGrid.Visibility = Visibility.Visible;
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if(PossibleConestionListBox.SelectedItem == null)
            {
                MessageBox.Show("Vyberte sériový port", "Chyba", MessageBoxButton.OK);
            }
            else
            {
                string s = PossibleConestionListBox.SelectedItem as string;
                core.StartServer(s, "URL(zatim k ničemu)");
                core.OnDisconnect = () => { OnDisconnestEvent(); };
                TemperatureMaxTextBlock.Text = core.ActualSettings.TemperatureMax.ToString();
                TemperatureMinTextBlock.Text = core.ActualSettings.TemperatureMin.ToString();
                WetnessMaxTextBlock.Text = core.ActualSettings.AirConditioningMax.ToString();
                WetnessMinTextBlock.Text = core.ActualSettings.AirConditioningMin.ToString();
            }
            PossibleConnectionsGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
        }

        private void CompleteSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            core.NewSettings(new Settings(int.Parse(TemperatureMaxTextBlock.Text), int.Parse(TemperatureMinTextBlock.Text), int.Parse(WetnessMaxTextBlock.Text), int.Parse(WetnessMinTextBlock.Text), core.ActualSettings.Windows, core.ActualSettings.Heating));
        }

        public void OnDisconnestEvent()
        {
            //to co se stane při odpojení
            MainGrid.Visibility = Visibility.Hidden;
            FindButton.Visibility = Visibility.Visible;
        }
    }
}
