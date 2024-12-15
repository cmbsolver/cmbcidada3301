using System;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class SumGemSentencesViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public SumGemSentencesViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _textToTranspose = "";
    
    [ObservableProperty] private string _response = "";
    
    [RelayCommand]
    public async void TransposeText()
    {
        StringBuilder sb = new StringBuilder();
        var sentences = _textToTranspose.Split(Environment.NewLine);
        foreach (var sentence in sentences)
        {
            var result = await _mediator.Send(new CalculateGematriaSum.Command(sentence));
            if (result != "0")
            {
                sb.AppendLine(result);
                sb.AppendLine("");
            }
        }
        
        Response = sb.ToString();
    }
}