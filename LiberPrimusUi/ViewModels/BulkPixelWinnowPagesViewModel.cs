using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class BulkPixelWinnowPagesViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public BulkPixelWinnowPagesViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        var pages = _mediator.Send(new GetPages.Command()).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
        
        for (int i = 1; i <= 8; i++)
        {
            BitsOfInsignificance.Add(i);
        }
    }
    
    public ObservableCollection<LiberPage> LiberPages { get; set; } = new ObservableCollection<LiberPage>();
    
    public ObservableCollection<int> BitsOfInsignificance { get; set; } = new ObservableCollection<int>();
    
    [ObservableProperty] private LiberPage _selectedLiberPage;

    [ObservableProperty] private bool _includeControlCharacters = false;

    [ObservableProperty] private bool _reverseBytes = false;

    [ObservableProperty] private bool _shiftSequence = false;

    [ObservableProperty] private int _minBitOfInsignificance = 1;

    [ObservableProperty] private int _maxBitOfInsignificance = 3;

    [ObservableProperty] private bool _discardRemainder = true;

    [ObservableProperty] private bool _binaryOnlyMode = false;
    
    [ObservableProperty] private bool _isEnabled = true;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand]
    public async void ProcessImage()
    {
        Result += SelectedLiberPage.FileName + Environment.NewLine;
        IsEnabled = false;
        await _mediator.Publish(new BulkPixelWinnowPages.Command(
            SelectedLiberPage.FileName,
            IncludeControlCharacters,
            ReverseBytes,
            ShiftSequence,
            MinBitOfInsignificance,
            MaxBitOfInsignificance,
            DiscardRemainder,
            BinaryOnlyMode));
        IsEnabled = true;
    }
    
    [RelayCommand]
    public async void BulkProcessImage()
    {
        foreach (var page in LiberPages)
        {
            Result += page.FileName + Environment.NewLine;
            IsEnabled = false;
            await _mediator.Publish(new BulkPixelWinnowPages.Command(
                page.FileName,
                IncludeControlCharacters,
                ReverseBytes,
                ShiftSequence,
                MinBitOfInsignificance,
                MaxBitOfInsignificance,
                DiscardRemainder,
                BinaryOnlyMode));
        }
        
        IsEnabled = true;
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}