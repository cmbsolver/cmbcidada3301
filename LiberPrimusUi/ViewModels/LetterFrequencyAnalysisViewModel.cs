using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing.SkipAndTake;
using LiberPrimusAnalysisTool.Application.Queries.Analysis;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;
using MoreLinq;
using MoreLinq.Extensions;

namespace LiberPrimusUi.ViewModels;

public partial class LetterFrequencyAnalysisViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    
    public LetterFrequencyAnalysisViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text")).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
    }
    
    public ObservableCollection<string> LiberPages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedLiberPage;
    
    [ObservableProperty] private string _textToSend;
    
    [ObservableProperty] private string _charactersToExclude = "•,␍,␊,␉,␈,␇,␆,␅,␄,␃,␂,␁,␀, ,\n,\t,\r,!,-,_,=,+,*,^,&,%,@,$,#,~,`,',\",|,\\,/,?,>,<,.,:,;,{,},[,],(,),\',⊹";
    
    [ObservableProperty] private string _result = "";

    [RelayCommand] 
    public async void ProcessFile()
    {
        StringBuilder result = new();
        List<string> excludedCharacters = new();
        excludedCharacters.AddRange(CharactersToExclude.Split(',', StringSplitOptions.None));
        if (excludedCharacters.Count != 0)
        {
            excludedCharacters.Add(",");
        }
        
        var text = File.ReadAllText(SelectedLiberPage);
        StringBuilder realText = new();
        foreach (var character in text)
        {
            if (!excludedCharacters.Contains(character.ToString()))
            {
                realText.Append(character);
            }
        }
        
        var letterFrequency = await _mediator.Send(new GetFrequencyAnalysisForText.Query(realText.ToString()));
        foreach (var freq in OrderByExtension.OrderBy(letterFrequency.LetterFrequencyDetails, x => x.Frequency, OrderByDirection.Descending))
        {
            result.AppendLine($"Character: {freq.Letter}\tOccurrences: {freq.Occurrences}\tFrequency: {freq.Frequency}");
        }
        
        Result = result.ToString();
    }
    
    [RelayCommand] 
    public async void ProcessText()
    {
        StringBuilder result = new();
        List<string> excludedCharacters = new();
        excludedCharacters.AddRange(CharactersToExclude.Split(',', StringSplitOptions.None));
        if (excludedCharacters.Count != 0)
        {
            excludedCharacters.Add(",");
        }
        
        StringBuilder realText = new();
        foreach (var character in TextToSend)
        {
            if (!excludedCharacters.Contains(character.ToString()))
            {
                realText.Append(character);
            }
        }
        
        var letterFrequency = await _mediator.Send(new GetFrequencyAnalysisForText.Query(realText.ToString()));
        foreach (var freq in OrderByExtension.OrderBy(letterFrequency.LetterFrequencyDetails, x => x.Frequency, OrderByDirection.Descending))
        {
            result.AppendLine($"Character: {freq.Letter}\tOccurrences: {freq.Occurrences}\tFrequency: {freq.Frequency}");
        }
        
        Result = result.ToString();
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}