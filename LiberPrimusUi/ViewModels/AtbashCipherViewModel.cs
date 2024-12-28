using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class AtbashCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public AtbashCipherViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        InputTypes.Add("Gematria");
        InputTypes.Add("English");
        InputTypes.Add("Other");
    }
    
    [ObservableProperty] private string _stringToDecode = "";
    
    [ObservableProperty] private string _decodedString = "";
    
    [ObservableProperty] private string _selectedEncoding = "";
    
    [ObservableProperty] private string _alphabet = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();

    [RelayCommand]
    private async Task DecodeString()
    {
        DecodeAtbashCipher.Command command = new(StringToDecode, Alphabet);
        DecodedString = await _mediator.Send(command);
    }

    [RelayCommand]
    private async Task EncodeString()
    {
        EncodeAtbashCipher.Command command = new(StringToDecode, Alphabet);
        DecodedString = await _mediator.Send(command);
    }
}