using System.Text;
using LiberPrimusAnalysisTool.Entity.Spiral;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Spiral;

public class TunnelText
{
    public class Command(
        SpiralDirection spiralDirection,
        TunnelStyle tunnelStyle,
        StartPosition startPosition,
        MatrixSquare matrixSquare)
        : IRequest<string>
    {
        public SpiralDirection SpiralDirection { get; set; } = spiralDirection;
        public TunnelStyle TunnelStyle { get; set; } = tunnelStyle;
        public StartPosition StartPosition { get; set; } = startPosition;
        public MatrixSquare MatrixSquare { get; set; } = matrixSquare;
    }
    
    public class Handler: IRequestHandler<Command, string>
    {
        public Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            StringBuilder stringBuilder = new();
            request.MatrixSquare.ChangeSpiralDirection(request.SpiralDirection);
            request.MatrixSquare.ChangeTunnelStyle(request.TunnelStyle);
            
            if (request.TunnelStyle == TunnelStyle.TunnelOut)
            {
                request.MatrixSquare.ChangeStartPosition(StartPosition.Center);
            }
            else
            {
                request.MatrixSquare.ChangeStartPosition(request.StartPosition);    
            }

            for (int i = 0; i < request.MatrixSquare.GetCount(); i++)
            {
                stringBuilder.Append(request.MatrixSquare.Next()?.SquareValue);
            }
            
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}