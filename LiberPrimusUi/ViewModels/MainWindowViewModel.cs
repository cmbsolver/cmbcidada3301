using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Utility.Character;
using LiberPrimusUi.Models;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(ICharacterRepo characterRepo, IPermutator permutator, IMediator mediator)
    {
        _characterRepo = characterRepo;
        _permutator = permutator;
        _mediator = mediator;

        Items = new ObservableCollection<ListItemTemplate>(_templates);

        SelectedListItem = Items.First(vm => vm.ModelType == typeof(HomePageViewModel));
    }
    
    private readonly List<ListItemTemplate> _templates =
    [
        new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular", "Home"),
        new ListItemTemplate(typeof(PrimeCheckerViewModel), "Calculator", "Prime Checker"),
        new ListItemTemplate(typeof(GenerateSequenceViewModel), "Calculator", "Generate Sequence"),
    ];

    [ObservableProperty]
    private bool _isPaneOpen;

    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    private List<Tuple<string, object>> _windows = new List<Tuple<string, object>>();

    private readonly ICharacterRepo _characterRepo;
    private readonly IPermutator _permutator;
    private readonly IMediator _mediator;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        switch(value.ModelType)
        {
            case Type t when t == typeof(HomePageViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new HomePageViewModel()));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as HomePageViewModel;
                break;
            case Type t when t == typeof(PrimeCheckerViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new PrimeCheckerViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as PrimeCheckerViewModel;
                break;
            case Type t when t == typeof(GenerateSequenceViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new GenerateSequenceViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as GenerateSequenceViewModel;
                break;
        }
    }

    public ObservableCollection<ListItemTemplate> Items { get; }

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}