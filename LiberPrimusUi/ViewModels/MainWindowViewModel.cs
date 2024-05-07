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
    ];

    [ObservableProperty]
    private bool _isPaneOpen;

    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    private readonly ICharacterRepo _characterRepo;
    private readonly IPermutator _permutator;
    private readonly IMediator _mediator;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if (value is null) return;

        switch(value.ModelType)
        {
            case Type t when t == typeof(HomePageViewModel):
                CurrentPage = new HomePageViewModel();
                break;
            case Type t when t == typeof(PrimeCheckerViewModel):
                CurrentPage = new PrimeCheckerViewModel(_mediator);
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