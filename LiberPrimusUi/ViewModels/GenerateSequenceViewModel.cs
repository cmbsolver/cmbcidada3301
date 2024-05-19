using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Application.Queries.Math;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class GenerateSequenceViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public GenerateSequenceViewModel(IMediator mediator)
    {
        _mediator = mediator;

        // Adding the sequence types to the list
        var sequenceTypes = _mediator.Send(new GetSequences.Query()).Result;
        foreach (var sequenceType in sequenceTypes)
        {
            SequenceTypes.Add(sequenceType);
        }
    }

    [ObservableProperty] private string _numberToCheck = "";

    [ObservableProperty] private object _selectedSequenceType = "";

    [ObservableProperty] private string _result = "";

    public ObservableCollection<string> SequenceTypes { get; } = new ObservableCollection<string>();

    [RelayCommand]
    private async Task CalculateSequence()
    {
        var result = await _mediator.Send(new CalculateSequence.Query(
            Convert.ToInt64(NumberToCheck), 
            (string)SelectedSequenceType)
        );

        StringBuilder sb = new StringBuilder();
        if (result.Result != null)
        {
            sb.AppendLine($"Result: {result.Result}");
        }

        foreach (var seq in result.Sequence)
        {
            sb.AppendLine(seq.ToString());
        }

        Result = sb.ToString();
    }
}