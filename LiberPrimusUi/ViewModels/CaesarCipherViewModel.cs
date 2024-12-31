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

namespace LiberPrimusUi.ViewModels;

public partial class CaesarCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public CaesarCipherViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        InputTypes.Add("Gematria");
        InputTypes.Add("English");
        InputTypes.Add("Other");
        
        Dictionaries.Add("Regular");
        Dictionaries.Add("Runeglish");
        Dictionaries.Add("Runes");
    }
    
    [ObservableProperty] private string _stringToDecode = "";
    
    [ObservableProperty] private string _decodedString = "";
    
    [ObservableProperty] private string _selectedEncoding = "";
    
    [ObservableProperty] private string _alphabet = "";
    
    [ObservableProperty] private string _shift = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();
    
    public ObservableCollection<string> Dictionaries { get; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedDictionary = "";
    
    [RelayCommand]
    private async Task DecodeString()
    {
        if (Shift.Contains(","))
        {
            DecodeCaesarCipher.Command command = new(Alphabet, StringToDecode, Shift);
            DecodedString = await _mediator.Send(command);
        }
        else
        {
            DecodeCaesarCipher.Command command = new(Alphabet, StringToDecode, Convert.ToInt32(Shift));
            DecodedString = await _mediator.Send(command);
        }
    }
    
    [RelayCommand]
    private async Task BulkDecodeString()
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
        
        StringBuilder result = new();

        for (int i = 0; i <= Alphabet.Split(",").Length; i++)
        {
            if (i > 0)
            {
                result.AppendLine();    
            }
            
            result.AppendLine($"Trying {i}:");
            DecodeCaesarCipher.Command command = new(Alphabet, StringToDecode, Convert.ToInt32(i));
            var decoded = await _mediator.Send(command);
            
            var score = await _mediator.Send(new ScoreText.Command(decoded, dictionary));
            scores.Add(new Tuple<ulong, string, string>(score.Item1, i.ToString(), decoded));
            if (scores.Count > 0)
            {
                DecodedString = string.Empty;
                scores = scores.OrderByDescending(x => x.Item1).ToList();

                result.Clear();
                foreach (var tscore in scores)
                {
                    result.AppendLine($"Score: {tscore.Item1} - Shift: {tscore.Item2} - {tscore.Item3}");
                }
            }

            DecodedString = result.ToString();
        }
    }
    
    [RelayCommand]
    private async Task EncodeString()
    {
        if (Shift.Contains(","))
        {
            EncodeCaesarCipher.Command command = new(Alphabet, StringToDecode, Shift);
            DecodedString = await _mediator.Send(command);
        }
        else
        {
            EncodeCaesarCipher.Command command = new(Alphabet, StringToDecode, Convert.ToInt32(Shift));
            DecodedString = await _mediator.Send(command);
        }
    }
}