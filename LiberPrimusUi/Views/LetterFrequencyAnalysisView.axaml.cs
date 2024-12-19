using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class LetterFrequencyAnalysisView : UserControl
{
    public LetterFrequencyAnalysisView()
    {
        InitializeComponent();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var viewModel = (LetterFrequencyAnalysisViewModel)DataContext;
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);
        
        if (topLevel != null)
        {
            var result = await topLevel.StorageProvider.OpenFolderPickerAsync(
                new FolderPickerOpenOptions()
                {
                    Title = "Select a folder"
                });

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    var folder = item.TryGetLocalPath();
                    await viewModel?.IndexDocuments(folder);
                }
            }
        }
    }
}