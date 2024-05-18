using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class BinaryDecodeViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public BinaryDecodeViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        BinDecodeTypes.Add("ASCII");
        BinDecodeTypes.Add("ANSI");
        BinDecodeTypes.Add("UTF7");
        BinDecodeTypes.Add("UTF8");
        BinDecodeTypes.Add("UTF32");
        BinDecodeTypes.Add("UNICODE");
        BinDecodeTypes.Add("Latin1");
        BinDecodeTypes.Add("HEX");
        BinDecodeTypes.Add("FILE");
    }
    
    [ObservableProperty] private string _binStringToDecode = "";
    
    [ObservableProperty] private string _binDecodedString = "";
    
    [ObservableProperty] private string _binSelectedEncoding = "";
    
    [ObservableProperty] private string _binFileName = "";
    
    public ObservableCollection<string> BinDecodeTypes { get; } = new ObservableCollection<string>();
    
    [RelayCommand]
    private async Task DecodeBinaryString()
    {
        var result = await _mediator.Send(new BinaryDecoding.Command()
        {
            Input = BinStringToDecode,
            Encoding = BinSelectedEncoding,
            File = BinFileName
        });
        
        BinDecodedString = result;
    }
}