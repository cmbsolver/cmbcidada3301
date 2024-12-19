using Avalonia.Controls;

namespace LiberPrimusUi.Views;

public partial class MainWindow : Window
{
    public static MainWindow Instance;
    
    public MainWindow()
    {
        InitializeComponent();
        Instance = this;
    }
}