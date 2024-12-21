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
    }
    
    [ObservableProperty] private bool _isIntermediryLib = false;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand] 
    public async void GetValues()
    {
        var result = new StringBuilder();
        var letterFrequency = await _mediator.Send(new GetLetterForFrequencyFromLib.Query(IsIntermediryLib));
        foreach (var freq in OrderByExtension.OrderBy(letterFrequency.LetterFrequencyDetails, x => x.Frequency, OrderByDirection.Descending))
        {
            result.AppendLine($"Character: {freq.Letter}\tOccurrences: {freq.Occurrences}\tFrequency: {freq.Frequency}");
        }
        
        Result = result.ToString();
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}