using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class CalculateClockAngleViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public CalculateClockAngleViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private decimal? _hour = 3;
    
    [ObservableProperty] private decimal? _minute = 30;
    
    [ObservableProperty] private string _result;
    
    [RelayCommand]
    public async void CalculateAngle()
    {
        var myResult = await _mediator.Send(new CalculateClockAngle.Command(Convert.ToInt32(Hour), Convert.ToInt32(Minute)));
        Result = $"{myResult}";
    }
}