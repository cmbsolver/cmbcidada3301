using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class Base64DecodeViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public Base64DecodeViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        InputTypes.Add("ASCII");
        InputTypes.Add("UTF7");
        InputTypes.Add("UTF8");
        InputTypes.Add("UTF32");
        InputTypes.Add("UNICODE");
        InputTypes.Add("Latin1");
        InputTypes.Add("HEX");
        InputTypes.Add("OTHER");
    }
    
    [ObservableProperty] private string _stringToDecode = "";
    
    [ObservableProperty] private string _decodedString = "";
    
    [ObservableProperty] private string _selectedEncoding = "";
    
    [ObservableProperty] private string _otherEncoding = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();
    
    [RelayCommand]
    private async Task DecodeString()
    {
        var result = await _mediator.Send(new Base64Decoding.Command()
        {
            Input = StringToDecode,
            Encoding = SelectedEncoding,
            OtherEncoding = OtherEncoding
        });
        
        DecodedString = result;
    }
}