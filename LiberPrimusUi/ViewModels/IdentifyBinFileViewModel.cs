using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class IdentifyBinFileViewModel : ViewModelBase
{
    private readonly IMediator _mediator;

    public IdentifyBinFileViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        // Adding the sequence types to the list
        GetBinList();
    }

    private void GetBinList()
    {
        BinFiles.Clear();
        FileInfo fi = new FileInfo(Environment.ProcessPath);
        var bins = _mediator.Send(new GetBinFiles.Command(fi.DirectoryName)).Result;
        foreach (var bin in bins)
        {
            BinFiles.Add(bin);
        }
    }

    public ObservableCollection<string> BinFiles { get; set; } = new ObservableCollection<string>();
    
    [ObservableProperty] private string _selectedBinFile;
    
    [ObservableProperty] private string _result = "";

    [RelayCommand]
    public async void ProcessBinFile()
    {
        Result = await _mediator.Send(new DetectBinFile.Command(
            SelectedBinFile));
    }
    
    [RelayCommand]
    public async void BulkProcessBinFile()
    {
        foreach (var binFile in BinFiles)
        {
            try
            {
                Result += await _mediator.Send(new DetectBinFile.Command(
                    binFile));
                Result += Environment.NewLine;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    
    [RelayCommand]
    public async void CleanUpUnidentifiedFiles()
    {
        foreach (var binFile in BinFiles)
        {
            var result = await _mediator.Send(new DetectBinFile.Command(
                binFile));
            
            if (result.StartsWith("Could not detect file type for"))
            {
                try
                {
                    File.Delete(binFile);
                    Result += $"Deleted {binFile}" + Environment.NewLine;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        
        GetBinList();
    }
    
    [RelayCommand]
    public void ClearMessages()
    {
        Result = string.Empty;
    }
}