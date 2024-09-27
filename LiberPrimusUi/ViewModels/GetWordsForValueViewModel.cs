using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class GetWordsForValueViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public GetWordsForValueViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _textToTranspose = "";
    
    [ObservableProperty] private string _response = "";
    
    [RelayCommand]
    public async void CalculateText()
    {
        Response = "";
        
        foreach (var textValue in TextToTranspose.Split(",", StringSplitOptions.RemoveEmptyEntries))
        {
            var tresponse = await _mediator.Send(new GetWordFromInts.Command(Convert.ToInt64(textValue.Trim())));

            if (tresponse.Length == 0)
            {
                Response += $"No words found for {textValue}." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                Response += string.Join(Environment.NewLine, tresponse) + Environment.NewLine + Environment.NewLine;
            }
        }
    }

    [RelayCommand]
    public async void ClearText()
    {
        Response = "";
    }
}