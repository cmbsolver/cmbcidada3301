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

public partial class AdvancedCaesarCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public AdvancedCaesarCipherViewModel(IMediator mediator)
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
    
    [ObservableProperty] private string _shift = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();
    
    [RelayCommand]
    private async Task DecodeString()
    {
        if (Shift.Contains(","))
        {
            DecodeAdvancedCaesarCipher.Command command = new(Alphabet, StringToDecode, Shift);
            DecodedString = await _mediator.Send(command);
        }
        else
        {
            DecodeAdvancedCaesarCipher.Command command = new(Alphabet, StringToDecode, Convert.ToInt32(Shift));
            DecodedString = await _mediator.Send(command);
        }
    }
    
    [RelayCommand]
    private async Task BulkDecodeString()
    {
        StringBuilder result = new();
        int maxShift = Convert.ToInt32(Shift);

        for (int i = 0; i <= maxShift; i++)
        {
            if (i > 0)
            {
                result.AppendLine();    
            }
            
            result.AppendLine($"Trying {i}:");
            DecodeAdvancedCaesarCipher.Command command = new(Alphabet, StringToDecode, Convert.ToInt32(i));
            result.AppendLine(await _mediator.Send(command));
        }
        
        DecodedString = result.ToString();
    }
    
    [RelayCommand]
    private async Task EncodeString()
    {
        if (Shift.Contains(","))
        {
            EncodeAdvancedCaesarCipher.Command command = new(Alphabet, StringToDecode, Shift);
            DecodedString = await _mediator.Send(command);
        }
        else
        {
            EncodeAdvancedCaesarCipher.Command command = new(Alphabet, StringToDecode, Convert.ToInt32(Shift));
            DecodedString = await _mediator.Send(command);
        }
    }
}