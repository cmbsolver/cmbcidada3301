using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Application.Queries.Analysis;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;
using MoreLinq;
using MoreLinq.Extensions;

namespace LiberPrimusUi.ViewModels;

public partial class GetLetterForFrequencyFromLibViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    
    public GetLetterForFrequencyFromLibViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        Modes.Add("runes");
        Modes.Add("runes-med");
        Modes.Add("letters");
        Modes.Add("intermediary");
        SelectedMode = Modes[0];
    }
    
    [ObservableProperty] private string _result = "";
    
    public ObservableCollection<string> Modes { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedMode = "";

    [RelayCommand] 
    public async void GetValues()
    {
        var result = new StringBuilder();
        var letterFrequency = await _mediator.Send(new GetLetterForFrequencyFromLib.Query(SelectedMode));
        
        result.AppendLine($"Character\tOccurrences\tFrequency");
        
        foreach (var freq in OrderByExtension.OrderBy(letterFrequency.LetterFrequencyDetails, x => x.Frequency, OrderByDirection.Descending))
        {
            result.AppendLine($"{freq.Letter}\t{freq.Occurrences}\t{freq.Frequency}");
        }
        
        Result = result.ToString();
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}