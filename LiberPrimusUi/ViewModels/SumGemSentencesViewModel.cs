using System;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Application.Queries;
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
            if (sentence.Trim().Length == 0)
            {
                continue;
            }
            
            var result = await _mediator.Send(new CalculateDetailGematriaSum.Command(sentence.Trim()));
            var value = Convert.ToUInt64(result.Item1);
            var isPrime = await _mediator.Send(new GetIsPrime.Query(value));

            counter++;
            sb.AppendLine($"Line: {counter} - {sentence.Trim()}");
            sb.AppendLine($"Line: {counter} - Is Prime: {isPrime} - Sum: {result.Item1}");
            sb.AppendLine($"Line: {counter} - Letter Values: {string.Join(",", result.Item2)}");
            sb.AppendLine($"Line: {counter} - Word Sums: {string.Join(",", result.Item3)}");
            sb.AppendLine("");
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