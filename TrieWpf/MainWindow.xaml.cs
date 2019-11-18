using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TrieWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
        // DoubleClick ist schwer direkt mit Command zu verdrahten, deshalb gehe über event:
        public void ListViewDblClick(object sender, MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;

            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                // (DataContext as MainViewModel)?.EmbeddedDocumentMouseDoubleClickCommand?.Execute(textBlock.Text);

            }
        }
    }
}
