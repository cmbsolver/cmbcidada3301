using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class RuneDecimalLsbViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public RuneDecimalLsbViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        FileInfo fi = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command(fi.DirectoryName)).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
        
        for (int i = 1; i <= 8; i++)
        {
            BitsOfInsignificance.Add(i);
        }
        
        OutputTypes.Add("string");
        OutputTypes.Add("file");
    }
    
    public ObservableCollection<string> OutputTypes { get; set; } = new ObservableCollection<string>();
    
    public ObservableCollection<string> LiberPages { get; set; } = new ObservableCollection<string>();
    
    public ObservableCollection<int> BitsOfInsignificance { get; set; } = new ObservableCollection<int>();
    
    [ObservableProperty] private string _selectedLiberPage;
    
    [ObservableProperty] private string _selectedOutputType;

    [ObservableProperty] private bool _includeControlCharacters = false;

    [ObservableProperty] private bool _reverseBytes = false;

    [ObservableProperty] private bool _shiftSequence = false;

    [ObservableProperty] private int _bitOfInsignificance = 1;

    [ObservableProperty] private bool _discardRemainder = true;
    
    [ObservableProperty] private bool _isEnabled = true;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand]
    public async void ProcessFile()
    {
        var returnString = await _mediator.Send(new RuneDecimalLsb.Command(
            SelectedOutputType, 
            SelectedLiberPage, 
            IncludeControlCharacters, 
            ReverseBytes, 
            ShiftSequence, 
            BitOfInsignificance,
            DiscardRemainder));
        
        Result = Result + returnString + Environment.NewLine;
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}