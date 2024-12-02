using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing.SkipAndTake;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class SkipAndTakeViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    
    public SkipAndTakeViewModel(IMediator mediator)
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
    
    [ObservableProperty] private bool _isSkipAndTake = true;
    
    [ObservableProperty] private bool _isTakeAndSkip = false;
    
    [ObservableProperty] private bool _isBulkSkipAndTake = false;
    
    [ObservableProperty] private bool _isBulkTakeAndSkip = false;
    
    public ObservableCollection<string> LiberPages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private decimal? _arrayIterations;
    
    [ObservableProperty] private decimal? _skip;
    
    [ObservableProperty] private decimal? _take;
    
    [ObservableProperty] private string _selectedLiberPage;
    
    [ObservableProperty] private bool _isEnabled = true;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand] 
    public async void ProcessFile()
    {
        if (IsSkipAndTake)
        {
            Result = await _mediator.Send(new SkipAndTakeText.Command(
                SelectedLiberPage,
                Convert.ToInt32(Skip),
                Convert.ToInt32(Take),
                Convert.ToInt32(ArrayIterations)));
        }
        else if (IsTakeAndSkip)
        {
            Result = await _mediator.Send(new TakeAndSkipText.Command(
                SelectedLiberPage,
                Convert.ToInt32(Skip),
                Convert.ToInt32(Take),
                Convert.ToInt32(ArrayIterations)));
        }
        else if (IsBulkSkipAndTake || IsBulkTakeAndSkip)
        {
            Result += await _mediator.Send(new BulkSkipAndTake.Command(
                SelectedLiberPage,
                Convert.ToInt32(Skip),
                Convert.ToInt32(Take),
                Convert.ToInt32(ArrayIterations),
                IsBulkSkipAndTake));
        }
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}