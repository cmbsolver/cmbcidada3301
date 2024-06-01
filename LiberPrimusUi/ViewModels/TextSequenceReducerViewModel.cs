using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Math;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class TextSequenceReducerViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public TextSequenceReducerViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text")).Result;
        foreach (var page in pages)
        {
            Pages.Add(page);
        }
        
        // Adding the sequence types to the list
        var sequenceTypes = _mediator.Send(new GetSequences.Query()).Result;
        foreach (var sequenceType in sequenceTypes)
        {
            SequenceTypes.Add(sequenceType);
        }
    }
    
    public ObservableCollection<string> Pages { get; set; } = new ObservableCollection<string>();
    
    public ObservableCollection<string> SequenceTypes { get; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _fileToDecode = "";
    
    [ObservableProperty] private string _selectedSequenceType = "";
    
    [ObservableProperty] private bool _reversed = false;
    
    [ObservableProperty] private string _result = "";
    
    [RelayCommand]
    public async void ReduceText()
    {
        if (FileToDecode is string)
        {
            var text = File.ReadAllText(FileToDecode);
            Result = await _mediator.Send(new TextSequenceReducer.Command(SelectedSequenceType, FileToDecode, Reversed));
        }
    }
}