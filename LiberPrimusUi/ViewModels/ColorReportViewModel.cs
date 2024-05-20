using System;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class ColorReportViewModel: ViewModelBase
{
    private readonly IMediator _mediator;

    public ColorReportViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _fileName = "";
    
    [ObservableProperty] private string _result = "";
    
    [ObservableProperty] private bool _processing = true;
    
    [RelayCommand]
    private async Task GetColorReport()
    {
        Processing = false;
        var result = await _mediator.Send(new ColorReport.Command(FileName));
        Result = result;
        Processing = true;
    }
}