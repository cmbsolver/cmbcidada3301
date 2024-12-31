using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class RebuildDictionaryViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public RebuildDictionaryViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _decodedString = "";

    [RelayCommand]
    private async Task RebuildDict()
    {
        DecodedString = "Rebuilding dictionary...";
        IndexWordDirectory.Command command = new();
        await _mediator.Publish(command);
        DecodedString = "Dictionary rebuilt!";
    }
}