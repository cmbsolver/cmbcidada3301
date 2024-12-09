using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class AdvancedHillCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public AdvancedHillCipherViewModel(IMediator mediator)
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
    
    [ObservableProperty] private string _matrix = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();
    
    [RelayCommand]
    private async Task DecodeString()
    {
        string[] alphaStringArray = Alphabet.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var alphaArray = alphaStringArray.Select(c => c[0]).ToArray();
        
        alphaStringArray = Alphabet.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int[,] matrixKey = new int[,] {{Convert.ToInt32(alphaStringArray[0]), Convert.ToInt32(alphaStringArray[1])}};
        
        DecodeAdvancedHillCipher.Command command = new(StringToDecode, matrixKey, alphaArray);
        DecodedString = await _mediator.Send(command);
    }
    
    [RelayCommand]
    private async Task EncodeString()
    {
        string[] alphaStringArray = Alphabet.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var alphaArray = alphaStringArray.Select(c => c[0]).ToArray();
        
        alphaStringArray = Alphabet.Split(',', StringSplitOptions.RemoveEmptyEntries);
        int[,] matrixKey = new int[,] {{Convert.ToInt32(alphaStringArray[0]), Convert.ToInt32(alphaStringArray[1])}};
        
        EncodeAdvancedHillCipher.Command command = new(StringToDecode, matrixKey, alphaArray);
        DecodedString = await _mediator.Send(command);
    }
}