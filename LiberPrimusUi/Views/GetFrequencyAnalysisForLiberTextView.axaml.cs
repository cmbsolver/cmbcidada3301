using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class GetFrequencyAnalysisForLiberTextView : UserControl
{
    public GetFrequencyAnalysisForLiberTextView()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (GetFrequencyAnalysisForLiberTextViewModel)DataContext;
        
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File"
        });

        if (file is not null)
        {
            viewModel.OutputFile = file.Path.AbsolutePath;
        }
    }
    
    
}