using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LiberPrimusUi.ViewModels;

public partial class VigenereCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public VigenereCipherViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        InputTypes.Add("Gematria");
        InputTypes.Add("English");
        InputTypes.Add("Other");

        for (int i = 1; i <= 10; i++)
        {
            MaxWordCombinations.Add(i.ToString());
        }
        SelectedMaxWordCombinations = "1";
        
        Dictionaries.Add("Regular");
        Dictionaries.Add("Runeglish");
        Dictionaries.Add("Runes");
    }
    
    [ObservableProperty] private string _stringToDecode = "";
    
    [ObservableProperty] private string _decodedString = "";
    
    [ObservableProperty] private string _selectedEncoding = "";
    
    [ObservableProperty] private string _alphabet = "";
    
    [ObservableProperty] private string _keyword = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();
    
    public ObservableCollection<string> MaxWordCombinations { get; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedMaxWordCombinations = "";
    
    public ObservableCollection<string> Dictionaries { get; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedDictionary = "";

    [RelayCommand]
    private async Task DecodeString()
    {
        DecodeVigenereCipher.Command command = new(
            Alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries), 
            Keyword, 
            StringToDecode);
        DecodedString = await _mediator.Send(command);
    }
    
    [RelayCommand]
    private async Task DictionaryDecodeString()
    {
        List<Tuple<ulong, string, string>> scores = new();
        List<string> dictionary = new List<string>();
        using (var context = new LiberContext())
        {
            switch (SelectedDictionary)
            {
                case "Regular":
                    dictionary = new List<string>(context.DictionaryWords.Select(x => x.DictionaryWordText).ToList());
                    break;
                case "Runeglish":
                    dictionary = new List<string>(context.DictionaryWords.Select(x => x.RuneglishWordText).ToList());
                    break;
                case "Runes":
                    dictionary = new List<string>(context.DictionaryWords.Select(x => x.RuneWordText).ToList());
                    break;
            }
        }
        
        if (dictionary.Count == 0)
        {
            DecodedString = "No dictionary words found.  Please select a dictionary.";
            return;
        }

        for (int i = 1; i <= Convert.ToInt32(SelectedMaxWordCombinations); i++)
        {
            foreach (var word in dictionary)
            {
                foreach (var combo in GetWordCombos(1, i, word, dictionary))
                {
                    try
                    {
                        DecodeVigenereCipher.Command command = new(
                            Alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries),
                            combo,
                            StringToDecode);
                        var decoded = await _mediator.Send(command);

                        var score = await _mediator.Send(new ScoreText.Command(decoded, dictionary));
                        scores.Add(new Tuple<ulong, string, string>(score.Item1, combo, decoded));
                        if (scores.Count > 100)
                        {
                            var beforeScores = scores.Select(x => x.Item1).ToList();
                            DecodedString = string.Empty;
                            scores = scores.OrderByDescending(x => x.Item1).Take(100).ToList();
                            var afterScores = scores.Select(x => x.Item1).ToList();

                            if (!beforeScores.SequenceEqual(afterScores))
                            {
                                foreach (var tscore in scores)
                                {
                                    DecodedString +=
                                        $"Score: {tscore.Item1} - Keyword: {tscore.Item2} - {tscore.Item3}\n";
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        DecodedString += $"Error: {e.Message}\n";
                    }
                }
            }
        }
    }

    IEnumerable<string> GetWordCombos(int depth, int maxDepth, string currentWordString, List<string> wordList)
    {
        if (depth < maxDepth)
        {
            foreach (var word in wordList)
            {
                var newCombo = currentWordString + word;
                
                foreach (var subWord in GetWordCombos(depth + 1, maxDepth, newCombo, wordList))
                {
                    yield return subWord;
                }
            }
        }
        else
        {
            yield return currentWordString;
        }
    }

    [RelayCommand]
    private async Task EncodeString()
    {
        EncodeVigenereCipher.Command command = new(
            Alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries), 
            Keyword, 
            StringToDecode);
        DecodedString = await _mediator.Send(command);
    }
}