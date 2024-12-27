using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Queries.Analysis;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class GetFrequencyAnalysisForLiberTextViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    
    public GetFrequencyAnalysisForLiberTextViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        var processFileInfo = new FileInfo(Environment.ProcessPath);
        var pages = _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text")).Result;
        foreach (var page in pages)
        {
            LiberPages.Add(page);
        }
        
        Modes.Add("Rune Frequency");
        Modes.Add("Rune Frequency (Med)");
        //Modes.Add("Letter Frequency");
        SelectedMode = Modes[0];
        
        CharacterExclusions.Add("0-9,A-Z");
        CharacterExclusions.Add("0-9");
        CharacterExclusions.Add("A-Z");
        CharacterExclusions.Add("Everything");
        SelectedCharacterExclusions = CharacterExclusions[3];
    }
    
    public ObservableCollection<string> CharacterExclusions { get; set; } = new ObservableCollection<string>();

    [ObservableProperty] private string _selectedCharacterExclusions = "";
    
    [ObservableProperty] private string _charactersToExclude = "";
    
    public ObservableCollection<string> LiberPages { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedLiberPage;
    
    public ObservableCollection<string> Modes { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedMode;
    
    [ObservableProperty] private string _outputFile;
    
    [ObservableProperty] private bool _isFromIntermediaryRune;
    
    [ObservableProperty] private bool _isPermuteCombinations;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand] 
    public async void ProcessFile()
    {
        if (IsPermuteCombinations)
        {
            Result = $"Permuting combinations... Please be patient and check the output file: {OutputFile}";
        }
        
        switch (SelectedMode)
        {
            case "Letter Frequency":
                throw new NotImplementedException("Not implemented yet");
                // await _mediator.Publish(new GetFrequencyAnalysisForLiberText.Query(
                //     SelectedLiberPage, 
                //     IsFromIntermediaryRune, 
                //     IsPermuteCombinations, 
                //     OutputFile,
                //     CharactersToExclude.Split(",")));
                break;
            
            case "Rune Frequency":
                await _mediator.Publish(new GetFrequencyAnalysisForRuneText.Query(
                    SelectedLiberPage,
                    IsPermuteCombinations, 
                    OutputFile,
                    "runes",
                    CharactersToExclude.Split(",")));
                break;
            
            case "Rune Frequency (Med)":
                await _mediator.Publish(new GetFrequencyAnalysisForRuneText.Query(
                    SelectedLiberPage,
                    IsPermuteCombinations, 
                    OutputFile,
                    "runes-med",
                    CharactersToExclude.Split(",")));
                break;
        }
        
        Result += $"Processed: {SelectedLiberPage}";
        Result += Environment.NewLine;
        Result += $"Check the output file: {OutputFile}";
        Result += Environment.NewLine;
        Result += Environment.NewLine;
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
    
    [RelayCommand]
    public void ExcludeCharacters()
    {
        switch (SelectedCharacterExclusions)
        {
            case "0-9,A-Z":
                CharactersToExclude = "1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
                                      "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
                break;
            case "0-9":
                CharactersToExclude = "1,2,3,4,5,6,7,8,9,0";
                break;
            case "A-Z":
                CharactersToExclude = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
                                      "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
                break;
            case "Everything":
                CharactersToExclude =
                    "•,␍,␊,␉,␈,␇,␆,␅,␄,␃,␂,␁,␀, ,\n,\t,\r,!,-,_,=,+,*,^,&,%,@,$,#,~,`,',\",|,\\,/," +
                    "?,>,<,.,:,;,{,},[,],(,),\'," +
                    "⊹,1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
                    "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
                break;
            default:
                CharactersToExclude =
                    "•,␍,␊,␉,␈,␇,␆,␅,␄,␃,␂,␁,␀, ,\n,\t,\r,!,-,_,=,+,*,^,&,%,@,$,#,~,`,',\",|,\\,/,?,>,<,.,:,;,{,}," +
                    "[,],(,),\',⊹,1,2,3,4,5,6,7,8,9,0,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z," +
                    "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
                break;
        }
    }
}