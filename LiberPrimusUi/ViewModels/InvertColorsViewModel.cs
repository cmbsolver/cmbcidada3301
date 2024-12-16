using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity.Image;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class InvertColorsViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;

    public InvertColorsViewModel(IMediator mediator, IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _mediator = mediator;

        _messageBus.MessageEvent += OnMessageReceived;

        // Adding the sequence types to the list
        var pages = _mediator.Send(new GetPages.Command()).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
    }

    private void OnMessageReceived(object sender, MessageSentEventArgs e)
    {
        if (e.Screen == "InvertColors")
        {
            Messages.Add(e.Message);
        }
    }

    public ObservableCollection<LiberPage> LiberPages { get; set; } = new ObservableCollection<LiberPage>();
    
    [ObservableProperty] private object _selectedLiberPage;
    
    public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();
    
    [RelayCommand]
    public async void InvertColors()
    {
        if (_selectedLiberPage is LiberPage selectedLiberPage)
        {
            _mediator.Publish(new InvertPageColor.Command(selectedLiberPage));
        }
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Messages.Clear();
    }
}