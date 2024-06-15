using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Chains;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Math;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class HexDecodeRunesViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public HexDecodeRunesViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command(processFileInfo.Directory.FullName)).Result;
        foreach (var page in pages)
        {
            Pages.Add(page);
        }
    }
    
    public ObservableCollection<string> Pages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _fileToDecode = "";
    
    [ObservableProperty] private string _result = "";
    
    [RelayCommand]
    public async void ProcessText()
    {
        if (FileToDecode is string)
        {
            var text = File.ReadAllText(FileToDecode);
            Result = await _mediator.Send(new HexDecodeRunes.Command(FileToDecode));
        }
    }
}