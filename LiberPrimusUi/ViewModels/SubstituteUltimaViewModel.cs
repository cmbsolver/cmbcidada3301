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

public partial class SubstituteUltimaViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;

    public SubstituteUltimaViewModel(IMediator mediator, IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _mediator = mediator;

        _messageBus.MessageEvent += OnMessageReceived;

        // Adding the sequence types to the list
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text")).Result;
        foreach (var page in pages)
        {
            Pages.Add(page);
        }
    }

    private void OnMessageReceived(object sender, MessageSentEventArgs e)
    {
        if (e.Screen == "SubstituteUltima")
        {
            Messages.Add(e.Message);
        }
        else if (e.Screen == "SubstituteUltima:lastrun")
        {
            LastRun = e.Message;
        }
        else if (e.Screen == "SubstituteUltima:complete")
        {
            LastRun = e.Message;
            IsButtonEnabled = true;
        }
    }

    public ObservableCollection<string> Pages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedLiberPage;
    
    [ObservableProperty] private string _lastRun;
    
    [ObservableProperty] private bool _isButtonEnabled = true;
    
    public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();

    [RelayCommand]
    public async void Process()
    {
        IsButtonEnabled = false;
        Task.Run(() => _mediator.Publish(new SubstituteUltima.Command(SelectedLiberPage)));
    }

    [RelayCommand]
    public void ClearMessages()
    {
        Messages.Clear();
    }
}