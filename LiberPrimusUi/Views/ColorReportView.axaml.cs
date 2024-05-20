using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class ColorReportView : UserControl
{
    public ColorReportView()
    {
        InitializeComponent();
    }

    private async void GetFile_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (ColorReportViewModel)DataContext;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Image File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            viewModel.FileName = files[0].TryGetLocalPath();
        }
    }
}