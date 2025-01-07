using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Decoders;
using LiberPrimusAnalysisTool.Application.Commands.Encoders;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using LiberPrimusAnalysisTool.Database;
using LiberPrimusAnalysisTool.Entity.Text;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class RebuildDictionaryViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public RebuildDictionaryViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [ObservableProperty] private string _decodedString = "";

    [RelayCommand]
    private async Task RebuildDict()
    {
        DecodedString = "Rebuilding dictionary...";
        IndexWordDirectory.Command command = new();
        await _mediator.Publish(command);
        DecodedString = "Dictionary rebuilt!";
    }
    
    public async Task ExportDict(string outputFile)
    {
        DecodedString = "Exporting dictionary...";
        
        using (var context = new LiberContext())
        {
            var words = context.DictionaryWords.ToList();
            XLWorkbook workbook = new();
            var worksheet = workbook.Worksheets.Add("Dictionary");
            
            worksheet.Cell(1, 1).Value = "Word";
            worksheet.Cell(1, 2).Value = "Runeglish";
            worksheet.Cell(1, 3).Value = "Rune";
            
            for (int i = 0; i < words.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = words[i].DictionaryWordText;
                worksheet.Cell(i + 2, 2).Value = words[i].RuneglishWordText;
                worksheet.Cell(i + 2, 3).Value = words[i].RuneWordText;
            }
            
            workbook.SaveAs(outputFile);
            workbook.Dispose();
        }
        
        DecodedString = $"Dictionary exported to {outputFile}";
    }
    
    public async Task LoadCustomDict(string inputFile)
    {
        DecodedString = "Loading custom dictionary...";
        
        using (var context = new LiberContext())
        {
            foreach (var remWord in context.DictionaryWords)
            {
                context.DictionaryWords.Remove(remWord);
            }

            await context.SaveChangesAsync();
            
            var workbook = new XLWorkbook(inputFile);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RowsUsed().ToList();
            
            for (int i = 1; i < rows.Count; i++)
            {
                var word = new DictionaryWord()
                {
                    DictionaryWordText = worksheet.Cell(i + 1, 1).Value.ToString(),
                    RuneglishWordText = worksheet.Cell(i + 1, 2).Value.ToString(),
                    RuneWordText = worksheet.Cell(i + 1, 3).Value.ToString(),
                };
                
                word.GemSum = Convert.ToInt64(await _mediator.Send(new CalculateGematriaSum.Command(word.RuneWordText)));
                word.DictionaryWordLength = word.DictionaryWordText.Length;
                word.RuneglishWordLength = word.RuneglishWordText.Length;
                word.RuneWordLength = word.RuneWordText.Length;
                
                context.DictionaryWords.Add(word);
            }
            
            await context.SaveChangesAsync();
        }
        
        DecodedString = $"Custom dictionary loaded from {inputFile}";
    }
}