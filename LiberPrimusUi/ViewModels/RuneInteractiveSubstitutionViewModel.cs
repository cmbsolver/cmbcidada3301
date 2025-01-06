using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Application.Queries.Text;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class RuneInteractiveSubstitutionViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public RuneInteractiveSubstitutionViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Load runes
        var runes = _mediator.Send(new GetRunes.Query()).Result;
        foreach (var rune in runes)
        {
            FromRunes.Add(rune);
            ToRunes.Add(rune);
        }
    }
    
    public ObservableCollection<RuneDetail> FromRunes { get; set; } = 
        new ObservableCollection<RuneDetail>();
    
    public ObservableCollection<RuneDetail> ToRunes { get; set; } = 
        new ObservableCollection<RuneDetail>();
    
    public ObservableCollection<RuneDetailMapping> RuneMappings { get; set; } =
        new ObservableCollection<RuneDetailMapping>();
    
    [ObservableProperty] private RuneDetail _selectedFromRuneDetail;
    
    [ObservableProperty] private RuneDetail _selectedToRuneDetail;
    
    [ObservableProperty] private RuneDetailMapping _selectedRuneMapping;
    
    [ObservableProperty] private string _textToTranslate = string.Empty;
    
    [ObservableProperty] private string _replacedText = string.Empty;
    
    [ObservableProperty] private string _transposedText = string.Empty;

    [RelayCommand]
    public async void AddRuneMapping()
    {
        RuneMappings.Add(new RuneDetailMapping(SelectedFromRuneDetail, SelectedToRuneDetail));
        
        RecalculateDropDowns();
        RecalculateText();
    }

    [RelayCommand]
    public async void RemoveRuneMapping()
    {
        if (SelectedRuneMapping == null)
        {
            return;
        }
        
        var index = RuneMappings.IndexOf(SelectedRuneMapping);

        if (index >= 0)
        {
            RuneMappings.RemoveAt(index);
        }
        
        RecalculateDropDowns();
        RecalculateText();
    }

    private void RecalculateText()
    {
        ReplacedText = string.Empty;
        TransposedText = string.Empty;

        foreach (var character in TextToTranslate)
        {
            var runeMapping = RuneMappings.FirstOrDefault(x => x.FromRune.Rune == character.ToString());
            if (runeMapping != null)
            {
                ReplacedText += runeMapping.ToRune.Rune;
            }
            else
            {
                ReplacedText += character;
            }   
        }
        
        TransposedText = _mediator.Send(new TransposeRuneToLatin.Command(ReplacedText)).Result;
    }

    private void RecalculateDropDowns()
    {
        SelectedFromRuneDetail = null;
        SelectedToRuneDetail = null;

        List<RuneDetail> fromRunes = new();
        fromRunes.AddRange(_mediator.Send(new GetRunes.Query()).Result);
        List<RuneDetail> toRunes = new();
        toRunes.AddRange(_mediator.Send(new GetRunes.Query()).Result);
        
        foreach (var mapping in RuneMappings)
        {
            var fromRuneIdx = fromRunes.FindIndex(x => x.Value == mapping.FromRune.Value);
            var toRuneIdx = toRunes.FindIndex(x => x.Value == mapping.ToRune.Value);
            
            fromRunes.RemoveAt(fromRuneIdx);
            toRunes.RemoveAt(toRuneIdx);
        }

        FromRunes.Clear();
        ToRunes.Clear();
        
        foreach (var rune in fromRunes)
        {
            FromRunes.Add(rune);
        }
        
        foreach (var rune in toRunes)
        {
            ToRunes.Add(rune);
        }
    }
}