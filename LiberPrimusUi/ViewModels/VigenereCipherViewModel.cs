using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LiberPrimusUi.ViewModels;

public partial class VigenereCipherViewModel : ViewModelBase
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

    [ObservableProperty] private string _currentDepth = "";
    
    private ulong _processedCount = 0;

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

        ulong processedCount = 0;

        using (var context = new LiberContext())
        {
            var processQueueItem = context.ProcessQueueItems.FirstOrDefault();
            while (processQueueItem != null)
            {
                try
                {
                    var combo = processQueueItem.HopperString;

                    UpdateProcessedCount($"Processed Count: {processedCount} & Top Score: {scores.FirstOrDefault()?.Item1}");

                    if (combo != null)
                    {
                        DecodeVigenereCipher.Command command = new(
                            Alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries),
                            combo,
                            StringToDecode);
                        var decoded = await _mediator.Send(command);

                        var score = await _mediator.Send(new ScoreText.Command(decoded, dictionary));

                        lock (scores)
                        {
                            scores.Add(new Tuple<ulong, string, string>(score.Item1, combo, decoded));
                            if (scores.Count > 25)
                            {
                                var tscore = scores.OrderByDescending(x => x.Item1).Take(25).ToList();
                                scores.Clear();
                                scores.AddRange(tscore);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    lock (DecodedString)
                    {
                        DecodedString += $"Error: {e.Message}\n";
                    }
                }
                
                context.ProcessQueueItems.Remove(processQueueItem);
                await context.SaveChangesAsync();
                processedCount++;
                if (processedCount > ulong.MaxValue - 100)
                {
                    processedCount = 0;
                }
                processQueueItem = context.ProcessQueueItems.FirstOrDefault();
            }
        }

        DecodedString = string.Join("\n",
            scores.Select(x => $"Score: {x.Item1} - Keyword: {x.Item2} - {x.Item3}"));
    }
    
    private void UpdateProcessedCount(string text)
    {
        CurrentDepth = text;
    }

    [RelayCommand]
    private async void BuildHopper()
    {
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

        var maxDepth = Convert.ToInt32(SelectedMaxWordCombinations);
        
        _processedCount = 0;
        
        UpdateProcessedCount($"Building Hopper...");
        using (var context = new LiberContext())
        {
            await Task.Run(() => GetWordCombos(0, maxDepth, string.Empty, dictionary, context));
        }
        UpdateProcessedCount($"Hopper built for {maxDepth} word combinations.");
    }

    private async Task GetWordCombos(
        int depth, 
        int maxDepth, 
        string currentWordString, 
        List<string> wordList, 
        LiberContext context)
    {
        if (depth >= maxDepth)
        {
            context.ProcessQueueItems.Add(ProcessQueueItem.GenerateQueueItem(currentWordString));
            _processedCount++;
            
            if (_processedCount > 1000000)
            {
                await context.SaveChangesAsync();
                _processedCount = 0;
            }
        }
        else if (depth < maxDepth)
        {
            foreach (var word in wordList)
            {
                var newWordString = currentWordString + word;
                await GetWordCombos(depth + 1, maxDepth, newWordString, wordList, context);
            }
        }
        
        if (depth == 0)
        {
            await context.SaveChangesAsync();
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