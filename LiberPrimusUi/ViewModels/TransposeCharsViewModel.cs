using System.Collections.ObjectModel;
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
        
        Modes.Add("Latin to Rune");
        Modes.Add("Rune to Latin");
        SelectedMode = Modes[0];
    }
    
    [ObservableProperty] private string _textToTranspose = "";
    
    [ObservableProperty] private string _response = "";
    
    [ObservableProperty] private string _gemSum = "";
    
    public ObservableCollection<string> Modes { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedMode = "";
    
    [ObservableProperty] private bool _prepTextToIrl = false;
    
    [RelayCommand]
    public async void TransposeText()
    {
        switch (SelectedMode)
        {
            case "Latin to Rune":
                if (PrepTextToIrl)
                {
                    var text = await _mediator.Send(new PrepLatinToRune.Command(TextToTranspose));
                    Response = await _mediator.Send(new TransposeLatinToRune.Command(text));
                    GemSum = await _mediator.Send(new CalculateGematriaSum.Command(Response));
                }
                else
                {
                    Response = await _mediator.Send(new TransposeLatinToRune.Command(TextToTranspose));
                    GemSum = await _mediator.Send(new CalculateGematriaSum.Command(Response));
                }
                break;
            
            case "Rune to Latin":
                GemSum = await _mediator.Send(new CalculateGematriaSum.Command(TextToTranspose));
                Response = await _mediator.Send(new TransposeRuneToLatin.Command(TextToTranspose));
                break;
        }
    }
}