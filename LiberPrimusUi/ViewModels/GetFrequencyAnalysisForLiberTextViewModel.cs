using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Queries.Analysis;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class GetFrequencyAnalysisForLiberTextViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    
    public GetFrequencyAnalysisForLiberTextViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text")).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
    }
    
    public ObservableCollection<string> LiberPages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedLiberPage;
    
    [ObservableProperty] private string _outputFile;
    
    [ObservableProperty] private bool _isFromIntermediaryRune;
    
    [ObservableProperty] private bool _isPermuteCombinations;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand] 
    public async void ProcessFile()
    {
        await _mediator.Publish(new GetFrequencyAnalysisForLiberText.Query(
            _selectedLiberPage, 
            IsFromIntermediaryRune, 
            IsPermuteCombinations, 
            _outputFile));
        
        Result += $"Processing: {SelectedLiberPage}";
        Result += Environment.NewLine;
        Result += $"Check the output file: {OutputFile}";
        Result += Environment.NewLine;
        Result += Environment.NewLine;
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}