using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class AffineCipherViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public AffineCipherViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        InputTypes.Add("Gematria");
        InputTypes.Add("English");
        InputTypes.Add("Other");
    }
    
    [ObservableProperty] private string _stringToDecode = "";
    
    [ObservableProperty] private string _decodedString = "";
    
    [ObservableProperty] private string _selectedEncoding = "";
    
    [ObservableProperty] private string _alphabet = "";
    
    [ObservableProperty] private string _multiplier = "";
    
    [ObservableProperty] private string _shift = "";
    
    [ObservableProperty] private string _charCount = "";
    
    public ObservableCollection<string> InputTypes { get; } = new ObservableCollection<string>();

    [RelayCommand]
    private async Task DecodeString()
    {
        if (string.IsNullOrEmpty(Shift) || string.IsNullOrEmpty(Multiplier) ||
            string.IsNullOrWhiteSpace(Shift) || string.IsNullOrWhiteSpace(Multiplier))
        {
            DecodedString = $"Multiplier and Shift were not set! Using Brute Force to decode...\n";
            string[] alphaArray = Alphabet.Split(",", StringSplitOptions.RemoveEmptyEntries);
            List<string> dict = new();
            List<TextScore> textScores = new();
            using (var context = new LiberContext())
            {
                dict.AddRange(context.DictionaryWords.Select(x => x.DictionaryWordText).ToList());
                dict.AddRange(context.DictionaryWords.Select(x => x.RuneglishWordText).ToList());
                dict.AddRange(context.DictionaryWords.Select(x => x.RuneWordText).ToList());
            }
            
            DecodedString = "";
            for (int i = 0; i < alphaArray.Length; i++)
            {
                for (int j = 0; j < alphaArray.Length; j++)
                {
                    try
                    {
                        DecodeAffineCipher.Command command = new(
                            StringToDecode.ToUpper(), 
                            i, 
                            j, 
                            Alphabet);
                        
                        string decoded = await _mediator.Send(command);
                        var score = await _mediator.Send(new ScoreText.Command(decoded, dict));
                        var textScore = new TextScore(StringToDecode, decoded, score.Item1, "", score.Item2);
                        textScores.Add(textScore);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            
            textScores = textScores.OrderByDescending(x => x.Score).ToList();
            foreach (var textScore in textScores)
            {
                DecodedString += $"{textScore.Score} - {textScore.Text}\n";
            }
        }
        else
        {
            DecodeAffineCipher.Command command = new(
                StringToDecode.ToUpper(), 
                Convert.ToInt32(Multiplier), 
                Convert.ToInt32(Shift), 
                Alphabet);
            DecodedString = await _mediator.Send(command);
        }
    }

    [RelayCommand]
    private async Task EncodeString()
    {
        EncodeAffineCipher.Command command = new(
            StringToDecode, 
            Convert.ToInt32(Multiplier), 
            Convert.ToInt32(Shift), 
            Alphabet);
        DecodedString = await _mediator.Send(command);
    }
}