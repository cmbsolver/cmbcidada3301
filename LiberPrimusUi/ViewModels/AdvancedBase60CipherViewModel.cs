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

public partial class AdvancedBase60CipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public AdvancedBase60CipherViewModel(IMediator mediator)
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
    [ObservableProperty] private string _chunkSize = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();
    
    [RelayCommand]
    private async Task DecodeString()
    {
        string[] alphaStringArray = Alphabet.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var alphaArray = alphaStringArray.Select(c => c[0]).ToArray();
        
        DecodeAdvancedBase60Cipher.Command command = new(StringToDecode, alphaArray, Convert.ToInt32(ChunkSize));
        DecodedString = await _mediator.Send(command);
    }
    
    [RelayCommand]
    private async Task EncodeString()
    {
        string[] alphaStringArray = Alphabet.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var alphaArray = alphaStringArray.Select(c => c[0]).ToArray();
        
        EncodeAdvancedBase60Cipher.Command command = new(StringToDecode, alphaArray);
        DecodedString = await _mediator.Send(command);
    }
}