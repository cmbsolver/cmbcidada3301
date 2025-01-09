using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Application.Queries.Text;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using MoreLinq;
using MoreLinq.Extensions;

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
    
    public ObservableCollection<WordListing> Words { get; set; } =
        new ObservableCollection<WordListing>();
    
    public ObservableCollection<DictionaryWord> PossibleWords { get; set; } =
        new ObservableCollection<DictionaryWord>();
    
    [ObservableProperty] private WordListing _selectedFromWord;
    
    [ObservableProperty] private DictionaryWord _selectedToWord;
    
    [ObservableProperty] private RuneDetail _selectedFromRuneDetail;
    
    [ObservableProperty] private RuneDetail _selectedToRuneDetail;
    
    [ObservableProperty] private RuneDetailMapping _selectedRuneMapping;
    
    [ObservableProperty] private string _textToTranslate = string.Empty;
    
    [ObservableProperty] private string _replacedText = string.Empty;
    
    [ObservableProperty] private string _transposedText = string.Empty;
    
    [ObservableProperty] private string _charactersLeft = string.Empty;
    
    [ObservableProperty] private bool _ignorePattern = false;
    
    [ObservableProperty] private bool _onlyUnmappedWords = true;
    
    [RelayCommand]
    public async void GetPossibleWords()
    {
        if (SelectedFromWord == null)
        {
            return;
        }
        
        PossibleWords.Clear();
        var words = _mediator.Send(new GetWordsFromLengths.Command(
            SelectedFromWord.Word.Length.ToString(), "Runes")).Result;
        
        if (IgnorePattern)
        {
            foreach (var word in words)
            {
                PossibleWords.Add(word);
            }
        }
        else
        {
            var fromPattern = SelectedFromWord.GetRunePattern();

            foreach (var word in words)
            {
                var toPattern = word.GetRunePattern();

                if (fromPattern == toPattern)
                {
                    PossibleWords.Add(word);
                }
            }
        }
        
        FindHowManyNotMapped();
    }
    
    [RelayCommand]
    public async void AddWordRuneMapping()
    {
        if (SelectedFromWord == null || SelectedToWord == null)
        {
            return;
        }
        
        List<RuneDetail> runeDetails = new();
        runeDetails.AddRange(_mediator.Send(new GetRunes.Query()).Result);
        
        var fromRunes = SelectedFromWord.Word.ToCharArray();
        var toRunes = SelectedToWord.RuneWordText.ToCharArray();
        
        for (int i = 0; i < fromRunes.Length; i++)
        {
            var fromRune = runeDetails.FirstOrDefault(x => x.Rune == fromRunes[i].ToString());
            var toRune = runeDetails.FirstOrDefault(x => x.Rune == toRunes[i].ToString());
            
            if (fromRune != null && toRune != null)
            {
                if (RuneMappings.Any(x => x.FromRune.Rune == fromRune.Rune))
                {
                    int counter = 0;
                    foreach (var map in RuneMappings)
                    {
                        if (map.FromRune.Value == fromRune.Value)
                        {
                            RuneMappings[counter] = new RuneDetailMapping(fromRune, toRune);
                            break;
                        }

                        counter++;
                    }
                }
                else
                {
                    RuneMappings.Add(new RuneDetailMapping(fromRune, toRune));    
                }
            }
        }
        
        RecalculateText();
        FindHowManyNotMapped();
        RecalculateDropdown();
    }

    [RelayCommand]
    public async void AddRuneMapping()
    {
        if (RuneMappings.Any(x => x.FromRune.Rune == SelectedFromRuneDetail.Rune))
        {
            int counter = 0;
            foreach (var map in RuneMappings)
            {
                if (map.FromRune.Value == SelectedFromRuneDetail.Value)
                {
                    RuneMappings[counter] = new RuneDetailMapping(SelectedFromRuneDetail, SelectedToRuneDetail);
                    break;
                }

                counter++;
            }
        }
        else
        {
            RuneMappings.Add(new RuneDetailMapping(SelectedFromRuneDetail, SelectedToRuneDetail));    
        }
        RecalculateText();
        FindHowManyNotMapped();
        RecalculateDropdown();
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
        
        RecalculateText();
        FindHowManyNotMapped();
        RecalculateDropdown();
    }
    
    [RelayCommand]
    public async void RemoveRuneMappings()
    {
        RuneMappings.Clear();
        SelectedFromRuneDetail = null;
        SelectedToRuneDetail = null;
        
        RecalculateText();
        FindHowManyNotMapped();
        RecalculateDropdown();
    }
    
    [RelayCommand]
    public async void GetPatternForText()
    {
        string pattern = WordListing.GetFullRunePattern(TextToTranslate);
        ReplacedText = pattern;
    }

    public string[] FindHowManyNotMapped()
    {
        var runes = _mediator.Send(new GetRunes.Query()).Result;
        List<RuneDetail> notMapped = new();
        notMapped.AddRange(runes);
        
        List<RuneDetail> tnotMapped = new();
        tnotMapped.AddRange(runes);

        foreach (var rune in tnotMapped)
        {
            if (!TextToTranslate.Contains(rune.Rune))
            {
                var index = notMapped.IndexOf(notMapped.FirstOrDefault(x => x.Rune == rune.Rune));
                notMapped.RemoveAt(index);
            }
        }

        foreach (var mapping in RuneMappings)
        {
            var index = notMapped.IndexOf(notMapped.FirstOrDefault(x => x.Rune == mapping.FromRune.Rune));
            notMapped.RemoveAt(index);
        }
        
        CharactersLeft = string.Join(",", notMapped.Select(x => x.Rune));
        
        return notMapped.Select(x => x.Rune).ToArray();
    }
    
    public string[] GetRunes()
    {
        return _mediator.Send(new GetRunes.Query()).Result.Select(x => x.Rune).ToArray();
    }
    
    public string GetRuneGlish(string word)
    {
        var runeglish = _mediator.Send(new TransposeRuneToLatin.Command(word)).Result;
        
        return runeglish;
    }

    public void RecalculateText()
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

    public void RecalculateDropdown(bool? onlyUnmapped = null)
    {
        if (onlyUnmapped != null)
        {
            OnlyUnmappedWords = (bool)onlyUnmapped;
        }
        
        this.Words.Clear();
        var runes = GetRunes();
        
        List<string> splitCharacters = new();
        
        // Getting the split characters
        foreach (var character in TextToTranslate)
        {
            if (!runes.Contains(character.ToString()))
            {
                splitCharacters.Add(character.ToString());
            }
        }

        if (splitCharacters.Any(x => x == "'"))
        {
            splitCharacters.Remove("'");
        }
        
        // Splitting the text
        var splitText = TextToTranslate.Split(splitCharacters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        
        // Getting the words
        Words.Clear();
        int counter = 0;
        
        if (OnlyUnmappedWords)
        {
            var notMapped = FindHowManyNotMapped();
            
            foreach (var word in splitText)
            {
                bool isMapped = true;
                foreach (var letter in word)
                {
                    if (notMapped.Contains(letter.ToString()))
                    {
                        isMapped = false;
                        break;
                    }
                }

                counter++;

                if (isMapped)
                {
                    continue;
                }

                Words.Add(new WordListing
                (
                    word,
                    counter,
                    GetRuneGlish(word)
                ));
            }
        }
        else
        {
            foreach (var word in splitText)
            {
                counter++;
                Words.Add(new WordListing
                (
                    word,
                    counter,
                    GetRuneGlish(word)
                ));
            }
        }
    }
}