using System.IO;
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

    private async void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (ColorReportViewModel)DataContext;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File"
        });

        if (file is not null)
        {
            // Open writing stream from the file.
            await using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            // Write some content to the file.
            await streamWriter.WriteAsync(viewModel.Result);
            await streamWriter.FlushAsync();
            streamWriter.Close();
        }
    }
}