using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
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
        
        viewModel.RecalculateDropdown();
        
        viewModel.RecalculateText();
        
        viewModel.FindHowManyNotMapped();
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
            
            viewModel.FindHowManyNotMapped();
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
        
        viewModel.FindHowManyNotMapped();
    }

    private void ToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var viewModel = (RuneInteractiveSubstitutionViewModel)DataContext;
        viewModel.SelectedToWord = null;
        viewModel.PossibleWords.Clear();
        
        viewModel.GetPossibleWords();
        
        viewModel.FindHowManyNotMapped();
    }

    private void OnlyUnmappedWords_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        var viewModel = (RuneInteractiveSubstitutionViewModel)DataContext;
        
        var unMapped = ((CheckBox)sender).IsChecked;
        
        viewModel.RecalculateDropdown(unMapped);
    }
}