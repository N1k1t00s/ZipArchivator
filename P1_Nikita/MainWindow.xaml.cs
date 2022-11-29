using System.Windows;
using P1_Nikita.ViewModel;

namespace P1_Nikita
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel model = new MainViewModel();
            DataContext = model;
            model.SelectedFiles = FilesListView.SelectedItems;
        }
    }
}