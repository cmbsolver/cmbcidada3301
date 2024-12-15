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
    public async void SumText()
    {
        int counter = 0;
        StringBuilder sb = new StringBuilder();
        var sentences = _textToTranspose.Split(Environment.NewLine);
        foreach (var sentence in sentences)
        {
            var result = await _mediator.Send(new CalculateGematriaSum.Command(sentence.Trim()));
            if (result != "0")
            {
                counter++;
                sb.Append($"Line: {counter} - ");
                sb.AppendLine(result);
                sb.AppendLine("");
            }
        }
        
        Response = sb.ToString();
    }

    [RelayCommand]
    public async void ClearText()
    {
        Response = string.Empty;
        TextToTranspose = string.Empty;
    }
}