using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Queries.Text;
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
        TextToTranspose = TextToTranspose.Replace(" ", "");
        Response = "";
        
        foreach (var textValue in TextToTranspose.Split(",", StringSplitOptions.RemoveEmptyEntries))
        {
            var tresponse = await _mediator.Send(new GetWordsFromInts.Command(textValue.Trim()));

            if (tresponse.Count() == 0)
            {
                Response += $"No words found for {textValue}." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                Response += $"Value: {textValue}" + Environment.NewLine;
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