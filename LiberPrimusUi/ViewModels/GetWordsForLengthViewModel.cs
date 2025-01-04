using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Queries.Text;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class GetWordsForLengthViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public GetWordsForLengthViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        Catalogs.Add("Regular");
        Catalogs.Add("Runeglish");
        Catalogs.Add("Runes");
    }
    
    [ObservableProperty] private string _textToTranspose = "";
    
    [ObservableProperty] private string _response = "";
    
    public ObservableCollection<string> Catalogs { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedCatalog;
    
    [RelayCommand]
    public async void CalculateText()
    {
        TextToTranspose = TextToTranspose.Replace(" ", "");
        Response = "";
        
        foreach (var textValue in TextToTranspose.Split(",", StringSplitOptions.RemoveEmptyEntries))
        {
            var tresponse = await _mediator.Send(new GetWordsFromLengths.Command(textValue.Trim(), SelectedCatalog));

            if (tresponse.Count() == 0)
            {
                Response += $"No words found for {textValue}." + Environment.NewLine + Environment.NewLine;
            }
            else
            {
                Response += $"Length: {textValue}" + Environment.NewLine;
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