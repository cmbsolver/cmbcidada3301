using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class CalculateSectionSumsViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public CalculateSectionSumsViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _reportString = "";
    
    [RelayCommand]
    private async Task CalculateString()
    {
        var result = await _mediator.Send(new CalculateSectionSums.Command());
        ReportString = result;
    }
}