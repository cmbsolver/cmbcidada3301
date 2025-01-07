using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class RebuildDictionaryView : UserControl
{
    public RebuildDictionaryView()
    {
        InitializeComponent();
    }

    private async void ExportButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (RebuildDictionaryViewModel)DataContext;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var options = new FilePickerSaveOptions();
        options.Title = "Save Excel File";
        options.DefaultExtension = ".xlsx";
        options.SuggestedFileName = "DictionaryExport";
        options.FileTypeChoices = new List<FilePickerFileType>()
        {
            new FilePickerFileType(".xlsx")
            {
                Patterns = ["*.xlsx"]
            }
        };
        
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(options);

        if (file is not null)
        {
            await viewModel.ExportDict(file.Path.AbsolutePath);
        }
    }

    private async void LoadCustomDict_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (RebuildDictionaryViewModel)DataContext;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var options = new FilePickerOpenOptions();
        options.Title = "Open Excel File";
        options.AllowMultiple = false;
        options.FileTypeFilter = new List<FilePickerFileType>()
        {
            new FilePickerFileType(".xlsx")
            {
                Patterns = ["*.xlsx"]
            }
        };
        
        var file = await topLevel.StorageProvider.OpenFilePickerAsync(options);

        if (file is not null)
        {
            await viewModel.LoadCustomDict(file[0].Path.AbsolutePath);
        }
    }
}