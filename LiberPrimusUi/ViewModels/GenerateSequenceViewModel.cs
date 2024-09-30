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
        
        SequenceModes.Add("Max Number");
        SequenceModes.Add("Position in Sequence");
    }

    [ObservableProperty] private string _numberToCheck = "";

    [ObservableProperty] private object _selectedSequenceType = "";
    
    [ObservableProperty] private object _selectedSequenceModeType = "Max Number";

    [ObservableProperty] private string _result = "";
    
    [ObservableProperty] private bool _commaSeperate = false;

    public ObservableCollection<string> SequenceTypes { get; } = new ObservableCollection<string>();
    
    public ObservableCollection<string> SequenceModes { get; } = new ObservableCollection<string>();

    [RelayCommand]
    private async Task CalculateSequence()
    {
        bool isPositional = SelectedSequenceModeType.ToString() == "Position in Sequence";
        
        var result = await _mediator.Send(new CalculateSequence.Query(
            Convert.ToUInt64(NumberToCheck), 
            (string)SelectedSequenceType, isPositional)
        );

        StringBuilder sb = new StringBuilder();
        if (result.Result != null)
        {
            sb.AppendLine($"Result: {result.Result}");
        }

        bool prepend = false;
        foreach (var seq in result.Sequence)
        {
            if (CommaSeperate)
            {
                string value = prepend ? ", " : "";
                sb.Append(value + seq.ToString());
                prepend = true;
            }
            else
            {
                sb.AppendLine(seq.ToString());
            }
        }

        Result = sb.ToString();
    }
}