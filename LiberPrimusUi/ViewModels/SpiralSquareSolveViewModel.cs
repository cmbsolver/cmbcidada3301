using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiberPrimusAnalysisTool.Application.Commands.Spiral;
using LiberPrimusAnalysisTool.Entity.Spiral;
using MediatR;

namespace LiberPrimusUi.ViewModels;

public partial class SpiralSquareSolveViewModel : ViewModelBase
{
    private readonly IMediator _mediator;
    
    public SpiralSquareSolveViewModel(IMediator mediator)
    {
        _mediator = mediator;
        
        SpiralDirections.Add("Clockwise");
        SpiralDirections.Add("Counter Clockwise");
    
        TunnelStyles.Add("Tunnel In");
        TunnelStyles.Add("Tunnel Out");
        
        StartPositions.Add("Upper Left");
        StartPositions.Add("Upper Right");
        StartPositions.Add("Lower Left");
        StartPositions.Add("Lower Right");
        StartPositions.Add("Center");
        
        SpaceShifts.Add("Top");
        SpaceShifts.Add("Bottom");
        SpaceShifts.Add("Left");
        SpaceShifts.Add("Right");
    }
    
    public ObservableCollection<string> SpiralDirections { get; set; } = new();
    
    public ObservableCollection<string> TunnelStyles { get; set; } = new();
    
    public ObservableCollection<string> StartPositions { get; set; } = new();
    
    public ObservableCollection<string> SpaceShifts { get; set; } = new();

    [ObservableProperty] private string _selectedSpiralDirection = "";
    
    [ObservableProperty] private string _selectedTunnelStyle = "";
    
    [ObservableProperty] private string _selectedStartPosition = "";
    
    [ObservableProperty] private string _selectedSpaceShift = "";
    
    [ObservableProperty] private string _textToSquare = "";
    
    [ObservableProperty] private bool _isEnabled = true;
    
    [ObservableProperty] private string _result = "";
    
    [ObservableProperty] private string _squaredText = "";
    
    [ObservableProperty] private string _squareSize = "0";
    
    [RelayCommand]
    private void SquareText()
    {
        TunnelStyle tunnelStyle = SelectedTunnelStyle switch
        {
            "Tunnel In" => TunnelStyle.TunnelIn,
            "Tunnel Out" => TunnelStyle.TunnelOut,
            _ => TunnelStyle.TunnelIn
        };
        
        SpaceShift spaceShift = SelectedSpaceShift switch
        {
            "Top" => SpaceShift.Top,
            "Bottom" => SpaceShift.Bottom,
            "Left" => SpaceShift.Left,
            "Right" => SpaceShift.Right,
            _ => SpaceShift.Top
        };
        
        MatrixSquare matrixSquare = _mediator.Send(new SquareText.Command(TextToSquare, tunnelStyle, spaceShift)).Result;
        SquaredText = matrixSquare.ToString();
        SquareSize = matrixSquare.GetCount().ToString();
    }

    [RelayCommand]
    private void ProcessText()
    {
        SpiralDirection spiralDirection = SelectedSpiralDirection switch
        {
            "Clockwise" => SpiralDirection.Clockwise,
            "Counter Clockwise" => SpiralDirection.CounterClockwise,
            _ => SpiralDirection.Clockwise
        };
        
        TunnelStyle tunnelStyle = SelectedTunnelStyle switch
        {
            "Tunnel In" => TunnelStyle.TunnelIn,
            "Tunnel Out" => TunnelStyle.TunnelOut,
            _ => TunnelStyle.TunnelIn
        };
        
        StartPosition startPosition = SelectedStartPosition switch
        {
            "Upper Left" => StartPosition.UpperLeft,
            "Upper Right" => StartPosition.UpperRight,
            "Lower Left" => StartPosition.LowerLeft,
            "Lower Right" => StartPosition.LowerRight,
            "Center" => StartPosition.Center,
            _ => StartPosition.UpperLeft
        };
        
        SpaceShift spaceShift = SelectedSpaceShift switch
        {
            "Top" => SpaceShift.Top,
            "Bottom" => SpaceShift.Bottom,
            "Left" => SpaceShift.Left,
            "Right" => SpaceShift.Right,
            _ => SpaceShift.Top
        };
        
        MatrixSquare matrixSquare = _mediator.Send(new SquareText.Command(TextToSquare, tunnelStyle, spaceShift)).Result;
        SquaredText = matrixSquare.ToString();
        SquareSize = matrixSquare.GetCount().ToString();
        
        Result = _mediator.Send(new TunnelText.Command(spiralDirection, tunnelStyle, startPosition, matrixSquare)).Result;
    }

    [RelayCommand]
    private void BulkProcessText()
    {
        StringBuilder outputText = new();

        foreach (var ts in TunnelStyles)
        {
            foreach (var sd in SpiralDirections)
            {
                foreach (var ss in SpaceShifts)
                {
                    TunnelStyle tunnelStyle = ts switch
                    {
                        "Tunnel In" => TunnelStyle.TunnelIn,
                        "Tunnel Out" => TunnelStyle.TunnelOut,
                        _ => TunnelStyle.TunnelIn
                    };

                    switch (tunnelStyle)
                    {
                        case TunnelStyle.TunnelIn:
                            foreach (var sp in StartPositions.Where(x => x != "Center"))
                            {
                                BulkProcessingActual(sd, ts, sp, ss, outputText);
                            }
                            
                            break;
                        case TunnelStyle.TunnelOut:
                            BulkProcessingActual(sd, ts, "Center", ss, outputText);
                            
                            break;
                    }

                }
            }
        }

        Result = outputText.ToString();
    }

    private void BulkProcessingActual(string sd, string ts, string sp, string ss, StringBuilder outputText)
    {
        SpiralDirection spiralDirection = sd switch
        {
            "Clockwise" => SpiralDirection.Clockwise,
            "Counter Clockwise" => SpiralDirection.CounterClockwise,
            _ => SpiralDirection.Clockwise
        };
        
        TunnelStyle tunnelStyle = ts switch
        {
            "Tunnel In" => TunnelStyle.TunnelIn,
            "Tunnel Out" => TunnelStyle.TunnelOut,
            _ => TunnelStyle.TunnelIn
        };
        
        StartPosition startPosition = sp switch
        {
            "Upper Left" => StartPosition.UpperLeft,
            "Upper Right" => StartPosition.UpperRight,
            "Lower Left" => StartPosition.LowerLeft,
            "Lower Right" => StartPosition.LowerRight,
            "Center" => StartPosition.Center,
            _ => StartPosition.UpperLeft
        };
        
        SpaceShift spaceShift = ss switch
        {
            "Top" => SpaceShift.Top,
            "Bottom" => SpaceShift.Bottom,
            "Left" => SpaceShift.Left,
            "Right" => SpaceShift.Right,
            _ => SpaceShift.Top
        };
        
        MatrixSquare matrixSquare = _mediator.Send(new SquareText.Command(TextToSquare, tunnelStyle, spaceShift)).Result;
        SquaredText = matrixSquare.ToString();
        SquareSize = matrixSquare.GetCount().ToString();
                        
        outputText.AppendLine($"TunnelStyle: {ts}, SpiralDirection: {sd}, StartPosition: {sp}, SpaceShift: {ss}");
        outputText.AppendLine(_mediator.Send(new TunnelText.Command(spiralDirection, tunnelStyle, startPosition, matrixSquare)).Result);
        outputText.AppendLine();
    }

    [RelayCommand]
    private void ClearMessages()
    {
        Result = string.Empty;
    }
}