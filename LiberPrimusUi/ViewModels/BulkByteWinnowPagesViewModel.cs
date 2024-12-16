using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity.Image;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class BulkByteWinnowPagesViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    private readonly IMessageBus _messageBus;
    
    public BulkByteWinnowPagesViewModel(IMediator mediator, IMessageBus messageBus)
    {
        _mediator = mediator;
        _messageBus = messageBus;
        
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
        
        _messageBus.MessageEvent += OnMessageReceived;
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
    
    private void OnMessageReceived(object sender, MessageSentEventArgs e)
    {
        if (e.Screen == "BulkByteWinnowPages")
        {
            Result += e.Message + Environment.NewLine;
        }
        else if (e.Screen == "BulkByteWinnowPages:Complete")
        {
            IsEnabled = true;
            Result += e.Message + Environment.NewLine;
        }
    }

    [RelayCommand]
    public async void ProcessImage()
    {
        IsEnabled = false;
        Task.Run(() => _mediator.Publish(new BulkByteWinnowPages.Command(
            BinaryOnlyMode,
            SelectedLiberPage.FileName,
            IncludeControlCharacters,
            ReverseBytes,
            ShiftSequence,
            MinBitOfInsignificance,
            MaxBitOfInsignificance,
            DiscardRemainder)));
    }
    
    [RelayCommand]
    public async void BulkProcessImage()
    {
        foreach (var page in LiberPages)
        {
            IsEnabled = false;
            await _mediator.Publish(new BulkByteWinnowPages.Command(
                BinaryOnlyMode,
                page.FileName,
                IncludeControlCharacters,
                ReverseBytes,
                ShiftSequence,
                MinBitOfInsignificance,
                MaxBitOfInsignificance,
                DiscardRemainder));
        }
        
        IsEnabled = true;
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}