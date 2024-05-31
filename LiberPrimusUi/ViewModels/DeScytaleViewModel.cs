using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class DeScytaleViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public DeScytaleViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text")).Result;
        foreach (var page in pages)
        {
            Pages.Add(page);
        }
    }
    
    public ObservableCollection<string> Pages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _fileToDecode = "";
    
    [ObservableProperty] private string _cols = "";
    
    [ObservableProperty] private bool _reversed = false;
    
    [ObservableProperty] private string _result = "";
    
    [RelayCommand]
    public async void DeScytaleText()
    {
        if (FileToDecode is string)
        {
            var text = File.ReadAllText(FileToDecode);
            Result = await _mediator.Send(new DeScytaleText.Command(text, int.Parse(Cols), Reversed));
        }
    }
    
    [RelayCommand]
    public async void BulkDeScytaleText()
    {
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var outputDirectory = $"{processFileInfo.Directory}/output/text";
        
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        foreach (var page in Pages)
        {
            var counter = 1;
            FileInfo fileInfo = new FileInfo(page);
            var text = File.ReadAllText(page);

            while (counter < 250)
            {
                if (!Directory.Exists($"{outputDirectory}/{fileInfo.Name.Replace(".txt", string.Empty)}"))
                {
                    Directory.CreateDirectory($"{outputDirectory}/{fileInfo.Name.Replace(".txt", string.Empty)}");
                }

                var tempResult = await _mediator.Send(new DeScytaleText.Command(text, counter, Reversed));
                File.WriteAllText(
                    $"{outputDirectory}/{fileInfo.Name.Replace(".txt", string.Empty)}/{fileInfo.Name.Replace(".txt", string.Empty)}_{counter}.txt",
                    tempResult);
                counter++;
            }
        }

        Result = "Done!";
    }
}