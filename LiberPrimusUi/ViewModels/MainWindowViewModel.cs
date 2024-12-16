using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Utility.Character;
using LiberPrimusAnalysisTool.Utility.Message;
using LiberPrimusUi.Models;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(ICharacterRepo characterRepo, IPermutator permutator, IMediator mediator, IMessageBus messageBus)
    {
        _characterRepo = characterRepo;
        _permutator = permutator;
        _mediator = mediator;
        _messageBus = messageBus;

        Items = new ObservableCollection<ListItemTemplate>(_templates);

        SelectedListItem = Items.First(vm => vm.ModelType == typeof(HomePageViewModel));
    }
    
    private readonly List<ListItemTemplate> _templates =
    [
        // Main
        new ListItemTemplate(typeof(HomePageViewModel), "HomeRegular", "Home"),
        new ListItemTemplate(typeof(PrimeCheckerViewModel), "Calculator", "Prime Checker"),
        new ListItemTemplate(typeof(GenerateSequenceViewModel), "NumberList", "Generate Sequence"),
        
        // Utilities
        new ListItemTemplate(typeof(TransposeCharsViewModel), "Wrench", "Latin and Rune Transposer"),
        new ListItemTemplate(typeof(SumGemSentencesViewModel), "Wrench", "Gem Sum Utility"),
        new ListItemTemplate(typeof(GetWordsForValueViewModel), "Wrench", "Get Words for Value"),
        new ListItemTemplate(typeof(Base64DecodeViewModel), "Wrench", "Base64 Decode"),
        new ListItemTemplate(typeof(BinaryDecodeViewModel), "Wrench", "Binary Decode"),
        //new ListItemTemplate(typeof(CalculateSectionSumsViewModel), "Wrench", "Calculate Section Sums"),
        new ListItemTemplate(typeof(TextSequenceReducerViewModel), "Wrench", "Text Sequence Reducer"),
        new ListItemTemplate(typeof(HexDecodeRunesViewModel), "Wrench", "Hex Decode Runes"),
        //new ListItemTemplate(typeof(SubstituteUltimaViewModel), "Wrench", "Substitute Ultima"),
        new ListItemTemplate(typeof(IdentifyBinFileViewModel), "Wrench", "Identify Bin File"),
        new ListItemTemplate(typeof(BinaryInvertViewModel), "Wrench", "Invert Binary Values"),
        new ListItemTemplate(typeof(RuneDecimalLsbViewModel), "Wrench", "Rune Decimal LSB"),
        new ListItemTemplate(typeof(SkipAndTakeViewModel), "Wrench", "Skip and Take"),
        new ListItemTemplate(typeof(DictionaryCheckTextFilesViewModel), "Wrench", "Dictionary Check Text Files"),
        new ListItemTemplate(typeof(CalculateClockAngleViewModel), "Wrench", "Clock Angle Calculator"),
        
        // Text analysis
        new ListItemTemplate(typeof(LetterFrequencyAnalysisViewModel), "Analysis", "Letter Frequency Analysis"),
        
        // Ciphers
        new ListItemTemplate(typeof(DeScytaleViewModel), "Lock", "Scytale"),
        new ListItemTemplate(typeof(AdvancedCaesarCipherViewModel), "Lock", "Caesar"),
        //new ListItemTemplate(typeof(AdvancedBase60CipherViewModel), "Lock", "Base 60"),
        //new ListItemTemplate(typeof(AdvancedHillCipherViewModel), "Lock", "Shitty Hill"),
        new ListItemTemplate(typeof(SpiralSquareSolveViewModel), "Lock", "Spiral"),
        
        // Image analysis
        new ListItemTemplate(typeof(ColorReportViewModel), "ImageIcon", "Color Report"),
        new ListItemTemplate(typeof(InvertColorsViewModel), "ImageIcon", "Invert Colors"),
        new ListItemTemplate(typeof(BulkByteWinnowPagesViewModel), "ImageIcon", "Byte Winnow Pages"),
        new ListItemTemplate(typeof(BulkPixelWinnowPagesViewModel), "ImageIcon", "Pixel Winnow Pages"),
        
        // Credits
        new ListItemTemplate(typeof(CreditsViewModel), "ListIcon", "Credits"),
    ];

    [ObservableProperty] private bool _isPaneOpen = true;

    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    [ObservableProperty] private string _titleBar = "Liber Primus Analysis Tool";

    private List<Tuple<string, object>> _windows = new List<Tuple<string, object>>();

    private readonly ICharacterRepo _characterRepo;
    private readonly IPermutator _permutator;
    private readonly IMediator _mediator;
    private readonly IMessageBus _messageBus;

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
            
            case Type t when t == typeof(CreditsViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new CreditsViewModel()));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as CreditsViewModel;
                break;
            
            case Type t when t == typeof(Base64DecodeViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new Base64DecodeViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as Base64DecodeViewModel;
                break;
            
            case Type t when t == typeof(BinaryDecodeViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new BinaryDecodeViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as BinaryDecodeViewModel;
                break;
            
            case Type t when t == typeof(ColorReportViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new ColorReportViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as ColorReportViewModel;
                break;
            
            case Type t when t == typeof(InvertColorsViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new InvertColorsViewModel(_mediator, _messageBus)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as InvertColorsViewModel;
                break;
            
            case Type t when t == typeof(CalculateSectionSumsViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new CalculateSectionSumsViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as CalculateSectionSumsViewModel;
                break;
            
            case Type t when t == typeof(DeScytaleViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new DeScytaleViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as DeScytaleViewModel;
                break; 
            
            case Type t when t == typeof(TextSequenceReducerViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new TextSequenceReducerViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as TextSequenceReducerViewModel;
                break; 
            
            case Type t when t == typeof(SubstituteUltimaViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new SubstituteUltimaViewModel(_mediator, _messageBus)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as SubstituteUltimaViewModel;
                break; 
            
            case Type t when t == typeof(BulkByteWinnowPagesViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new BulkByteWinnowPagesViewModel(_mediator, _messageBus)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as BulkByteWinnowPagesViewModel;
                break;
            
            case Type t when t == typeof(IdentifyBinFileViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new IdentifyBinFileViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as IdentifyBinFileViewModel;
                break;
            
            case Type t when t == typeof(HexDecodeRunesViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new HexDecodeRunesViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as HexDecodeRunesViewModel;
                break;
            
            case Type t when t == typeof(BinaryInvertViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new BinaryInvertViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as BinaryInvertViewModel;
                break;
            
            case Type t when t == typeof(BulkPixelWinnowPagesViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new BulkPixelWinnowPagesViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as BulkPixelWinnowPagesViewModel;
                break;
            
            case Type t when t == typeof(RuneDecimalLsbViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new RuneDecimalLsbViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as RuneDecimalLsbViewModel;
                break;
            
            case Type t when t == typeof(SkipAndTakeViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new SkipAndTakeViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as SkipAndTakeViewModel;
                break;
            
            case Type t when t == typeof(DictionaryCheckTextFilesViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new DictionaryCheckTextFilesViewModel(_mediator, _messageBus)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as DictionaryCheckTextFilesViewModel;
                break;
            
            case Type t when t == typeof(TransposeCharsViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new TransposeCharsViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as TransposeCharsViewModel;
                break;
            
            case Type t when t == typeof(GetWordsForValueViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new GetWordsForValueViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as GetWordsForValueViewModel;
                break;
            
            case Type t when t == typeof(AdvancedCaesarCipherViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new AdvancedCaesarCipherViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as AdvancedCaesarCipherViewModel;
                break;
            
            case Type t when t == typeof(CalculateClockAngleViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new CalculateClockAngleViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as CalculateClockAngleViewModel;
                break;
            
            case Type t when t == typeof(AdvancedBase60CipherViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new AdvancedBase60CipherViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as AdvancedBase60CipherViewModel;
                break;
            
            case Type t when t == typeof(AdvancedHillCipherViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new AdvancedHillCipherViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as AdvancedHillCipherViewModel;
                break;
            
            case Type t when t == typeof(SpiralSquareSolveViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new SpiralSquareSolveViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as SpiralSquareSolveViewModel;
                break;
            
            case Type t when t == typeof(SumGemSentencesViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new SumGemSentencesViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as SumGemSentencesViewModel;
                break;
            
            case Type t when t == typeof(LetterFrequencyAnalysisViewModel):
                if (!_windows.Any(w => w.Item1 == value.Label))
                    _windows.Add(new Tuple<string, object>(value.Label, new LetterFrequencyAnalysisViewModel(_mediator)));
                CurrentPage = _windows.FirstOrDefault(w => w.Item1 == value.Label)?.Item2 as LetterFrequencyAnalysisViewModel;
                break;
        }
        
        TitleBar = $"Liber Primus Analysis Tool - {value.Label}";
    }

    public ObservableCollection<ListItemTemplate> Items { get; }

    [RelayCommand]
    private void TriggerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
}