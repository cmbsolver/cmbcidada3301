using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class BinaryInvertViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public BinaryInvertViewModel(IMediator mediator)
    {
        _mediator = mediator;

        // Adding the sequence types to the list
        var pages = _mediator.Send(new GetAllFiles.Command("./")).Result;
        foreach (var page in pages)
        {
            Files.Add(page);
        }
    }
    
    public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
    
    public ObservableCollection<string> Files { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedFile;

    [RelayCommand]
    public async void InvertBinary()
    {
        var message = await _mediator.Send(new BinaryInvert.Command(SelectedFile));
        Messages.Add(message);
    }

    [RelayCommand]
    public void ClearMessages()
    {
        Messages.Clear();
    }
}