using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class ColorReportViewModel: ViewModelBase
{
    private readonly IMediator _mediator;

    public ColorReportViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        var pages = _mediator.Send(new GetPages.Command()).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
    }
    
    public ObservableCollection<LiberPage> LiberPages { get; set; } = new ObservableCollection<LiberPage>();
    
    [ObservableProperty] private LiberPage _selectedLiberPage;
    
    [ObservableProperty] private string _result = "";
    
    [ObservableProperty] private bool _processing = true;
    
    [RelayCommand]
    private async Task GetColorReport()
    {
        Processing = false;
        var result = await _mediator.Send(new ColorReport.Command(SelectedLiberPage.FileName));
        Result = result;
        Processing = true;
    }
}