using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Entity.Numeric;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class CircularShiftViewModel: ViewModelBase
{
    private readonly IMediator _mediator;
    
    public CircularShiftViewModel(IMediator mediator)
    {
        _mediator = mediator;

        foreach (var item in IntegerUtils.GetIntBitsForList())
        {
            IntBitList.Add(item);
        }
        
        ShiftDirections.Add("Left");
        ShiftDirections.Add("Right");
        
        OutputList.Add("File");
        OutputList.Add("ANSI");
        OutputList.Add("ASCII");
        OutputList.Add("NUMBERS");
        
        for(int i = 1; i <= 64; i++)
        {
            BitShiftList.Add(i.ToString());
        }
    }
    
    public ObservableCollection<string> BitShiftList { get; set; } = new ObservableCollection<string>();

    [ObservableProperty] private string _selectedBitShift = "";
    
    public ObservableCollection<string> OutputList { get; set; } = new ObservableCollection<string>();

    [ObservableProperty] private string _selectedOutput = "";
    
    public ObservableCollection<string> IntBitList { get; set; } = new ObservableCollection<string>();

    [ObservableProperty] private string _selectedIntBit = "";
    
    public ObservableCollection<string> ShiftDirections { get; set; } = new ObservableCollection<string>();

    [ObservableProperty] private string _selectedShiftDirection = "";
    
    [ObservableProperty] private string _textInput = "";
    
    [ObservableProperty] private string _selectedFile = "";

    [ObservableProperty] private string _result = "";
    
    [RelayCommand]
    public async void Process()
    {
        string output = "";
        switch (SelectedOutput)
        {
            case "File":
                output = SelectedFile;
                break;
            case "ANSI":
                output = "ANSI";
                break;
            case "ASCII":
                output = "ASCII";
                break;
            case "NUMBERS":
                output = "NUMBERS";
                break;
        }
        
        var command = new CircularShift.Command(
            TextInput, 
            Convert.ToInt32(SelectedBitShift), 
            SelectedShiftDirection,
            IntegerUtils.ConvertToEnum(SelectedIntBit), 
            output);

        try
        {
            Result = await _mediator.Send(command);
        }
        catch (Exception e)
        {
            Result = e.Message;
        }
    }
}