using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Math;

public class CalculateClockAngle
{
    public class Command : IRequest<decimal>
    {
        public Command(decimal hour, decimal minute)
        {
            Hour = hour;
            Minute = minute;
        }
        
        public decimal Hour { get; set; }
        
        public decimal Minute { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, decimal>
    {
        public async Task<decimal> Handle(Command request, CancellationToken cancellationToken)
        {
            decimal hour = request.Hour;
            decimal minute = request.Minute;
            decimal hourAngle = (hour % 12) * 30 + minute * 0.5m;
            decimal minuteAngle = minute * 6;
            decimal angle = System.Math.Abs(hourAngle - minuteAngle);
            return System.Math.Min(angle, 360 - angle);
        }
    }
}