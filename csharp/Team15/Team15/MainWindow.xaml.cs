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

namespace Team15
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            

            InitializeComponent();
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            //najdi možné prvky
            string[] serialPorts = new string[0];
            if (serialPorts.Length == 0)
            {
                //chyba
                MessageBox.Show("Nenalezeno žádné spojení, zkontrolujte kabely  ", "Chyba", MessageBoxButton.OK);
            }
            else if(serialPorts.Length == 1)
            {
                //přímá volba
                //serialPorts[0]
            }
            else
            {
                //výběr kabelu
                FindGrid.Visibility = Visibility.Hidden;
                PossibleConnectionsGrid.Visibility = Visibility.Visible;
                PossibleConestionListBox.ItemsSource = (new List<string>(serialPorts));
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
                //PossibleConestionListBox.SelectedItem as string;
            }
        }
    }
}
