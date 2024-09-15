using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusUi.ViewModels;

/// <summary>
/// 
/// </summary>
public partial class DictionaryCheckTextFilesViewModel: ViewModelBase
{
    /// <summary>
    /// The mediator
    /// </summary>
    private readonly IMediator _mediator;
    
    /// <summary>
    /// Message bus
    /// </summary>
    private readonly IMessageBus _messageBus;

    /// <summary>
    /// The constructor
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="messageBus"></param>
    public DictionaryCheckTextFilesViewModel(IMediator mediator, IMessageBus messageBus)
    {
        _messageBus = messageBus;
        _mediator = mediator;
        _messageBus.MessageEvent += OnMessageReceived;
    }
    
    /// <summary>
    /// Is GP Strict
    /// </summary>
    [ObservableProperty] private bool _isGpStrict = true;
    
    /// <summary>
    /// Is button enabled
    /// </summary>
    [ObservableProperty] private bool _isButtonEnabled = true;
    
    /// <summary>
    /// The message list.
    /// </summary>
    public ObservableCollection<string> Messages { get; set; } = new ObservableCollection<string>();

    /// <summary>
    /// On message received
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMessageReceived(object sender, MessageSentEventArgs e)
    {
        if (e.Screen == "DictionaryCheckTextFiles:File")
        {
            IsButtonEnabled = true;
            Messages.Add(e.Message);
        }
        else if (e.Screen == "DictionaryCheckTextFiles:Clear")
        {
            Messages.Clear();
        }
        else
        {
            Messages.Add(e.Message);
        }
    }
    
    /// <summary>
    /// Process files
    /// </summary>
    [RelayCommand]
    public async void Process()
    {
        IsButtonEnabled = false;
        Task.Run(() => _mediator.Publish(new DictionaryCheckTextFiles.Command(IsGpStrict)));
    }

    /// <summary>
    /// Clear messages
    /// </summary>
    [RelayCommand]
    public void ClearMessages()
    {
        Messages.Clear();
    }
}