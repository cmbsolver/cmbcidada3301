using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using LiberPrimusAnalysisTool.Entity.Text;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class RuneInteractiveSubstitutionView : UserControl
{
    public RuneInteractiveSubstitutionView()
    {
        InitializeComponent();
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var viewModel = (RuneInteractiveSubstitutionViewModel)DataContext;
        var runes = viewModel.GetRunes();
        
        viewModel.SelectedFromRuneDetail = null;
        viewModel.SelectedToRuneDetail = null;
        viewModel.SelectedToWord = null;
        viewModel.PossibleWords.Clear();
        viewModel.SelectedToWord = null;
        
        List<string> splitCharacters = new();
        
        // Getting the split characters
        foreach (var character in ((TextBox)sender).Text)
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
        var splitText = ((TextBox)sender).Text.Split(splitCharacters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        
        // Getting the words
        viewModel.Words.Clear();
        int counter = 0;
        foreach (var word in splitText)
        {
            counter++;
            viewModel.Words.Add(new WordListing
            (
                word,
                counter,
                viewModel.GetRuneGlish(word)
            ));
        }
        
        viewModel.RecalculateText();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            
            var viewModel = (RuneInteractiveSubstitutionViewModel)DataContext;
            var mapping = (RuneDetailMapping)e.AddedItems[0];
            viewModel.SelectedFromRuneDetail = viewModel.FromRunes.FirstOrDefault(x => x.Rune == mapping.FromRune.Rune);
            viewModel.SelectedToRuneDetail = viewModel.ToRunes.FirstOrDefault(x => x.Rune == mapping.ToRune.Rune);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private void ComboItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var viewModel = (RuneInteractiveSubstitutionViewModel)DataContext;
        viewModel.SelectedToWord = null;
        viewModel.PossibleWords.Clear();
        
        viewModel.GetPossibleWords();
    }
}