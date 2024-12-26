using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class CircularShiftView : UserControl
{
    public CircularShiftView()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (CircularShiftViewModel)DataContext!;
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);
        
        if (topLevel != null)
        {
            var result = await topLevel.StorageProvider.SaveFilePickerAsync(
                new FilePickerSaveOptions()
                {
                    Title = "Select an output file"
                });

            if (result != null)
            {
                var file = result.TryGetLocalPath();
                if (viewModel != null) viewModel.SelectedFile = file;
            }
        }
    }
}