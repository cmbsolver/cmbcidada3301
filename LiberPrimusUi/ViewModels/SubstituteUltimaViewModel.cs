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
        
        // Adding the versions to the list
        Versions.Add("Version 1");
        Versions.Add("Version 2");
    }

    private void OnMessageReceived(object sender, MessageSentEventArgs e)
    {
        if (e.Screen == "SubstituteUltima")
        {
            Messages.Add(e.Message);
        }
    }

    public ObservableCollection<string> Pages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedLiberPage;
    
    public ObservableCollection<string> Versions { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedVersion;
    
    public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
    
    [RelayCommand]
    public async void Process()
    {
        switch (SelectedVersion)
        {
            case "Version 1":
                Task.Run(() => _mediator.Publish(new SubstituteUltima.Command(SelectedLiberPage)));
                break;
            case "Version 2":
                Task.Run(() => _mediator.Publish(new SubstituteUltima2.Command(SelectedLiberPage)));
                break;
        }
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Messages.Clear();
    }
}