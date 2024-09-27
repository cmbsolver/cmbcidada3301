using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class TransposeCharsViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public TransposeCharsViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _textToTranspose = "";
    
    [ObservableProperty] private string _response = "";
    
    [ObservableProperty] private string _gemSum = "";
    
    [ObservableProperty] private bool _isRune = false;
    
    [RelayCommand]
    public async void TransposeText()
    {
        if (!IsRune)
        {
            GemSum = await _mediator.Send(new CalculateGematriaSum.Command(TextToTranspose));
            Response = await _mediator.Send(new TransposeRuneToLatin.Command(TextToTranspose));
        }
        else
        {
            Response = await _mediator.Send(new TransposeLatinToRune.Command(TextToTranspose));
            GemSum = await _mediator.Send(new CalculateGematriaSum.Command(Response));
        }
    }
}