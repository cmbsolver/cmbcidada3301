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

public partial class AffineCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public AffineCipherViewModel(IMediator mediator)
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
    
    [ObservableProperty] private string _multiplier = "";
    
    [ObservableProperty] private string _shift = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();

    [RelayCommand]
    private async Task DecodeString()
    {
        if (string.IsNullOrEmpty(Shift) || string.IsNullOrEmpty(Multiplier) ||
            string.IsNullOrWhiteSpace(Shift) || string.IsNullOrWhiteSpace(Multiplier))
        {
            DecodedString = "";
            for (int i = 0; i < Alphabet.Length; i++)
            {
                for (int j = 0; j < Alphabet.Length; j++)
                {
                    try
                    {
                        DecodeAffineCipher.Command command = new(
                            StringToDecode.ToUpper(), 
                            i, 
                            j, 
                            Alphabet);
                        DecodedString += await _mediator.Send(command);
                        DecodedString += Environment.NewLine;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            
        }
        else
        {
            DecodeAffineCipher.Command command = new(
                StringToDecode.ToUpper(), 
                Convert.ToInt32(Multiplier), 
                Convert.ToInt32(Shift), 
                Alphabet);
            DecodedString = await _mediator.Send(command);
        }
    }

    [RelayCommand]
    private async Task EncodeString()
    {
        EncodeAffineCipher.Command command = new(
            StringToDecode, 
            Convert.ToInt32(Multiplier), 
            Convert.ToInt32(Shift), 
            Alphabet);
        DecodedString = await _mediator.Send(command);
    }
}