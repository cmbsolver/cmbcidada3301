using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Queries;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class PrimeCheckerViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public PrimeCheckerViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty]
    private decimal? _numberToCheckIsPrime = 3301;
    
    [ObservableProperty]
    private string _isPrime = "";
    
    [RelayCommand]
    private async Task CheckNumber()
    {
        var result = await _mediator.Send(new GetIsPrime.Query() { Number = Convert.ToUInt64(NumberToCheckIsPrime) });
        
        if (result)
        {
            IsPrime = $"{NumberToCheckIsPrime} Is Prime";
        }
        else
        {
            IsPrime = $"{NumberToCheckIsPrime} Is Not Prime";
        }
    }
}