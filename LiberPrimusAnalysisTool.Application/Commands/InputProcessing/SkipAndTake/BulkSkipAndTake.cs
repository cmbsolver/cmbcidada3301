using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing.SkipAndTake;

public class BulkSkipAndTake
{
    public class Command: IRequest<string>
    {
        public Command(string inputFile, int skip, int take, int arrayIterations, bool isSkipAndTake)
        {
            InputFile = inputFile;
            Skip = skip;
            Take = take;
            ArrayIterations = arrayIterations;
            IsSkipAndTake = isSkipAndTake;
        }

        public string InputFile { get; set; }
        
        public int Skip { get; set; }
        
        public int Take { get; set; }
        
        public int ArrayIterations { get; set; }
        
        public bool IsSkipAndTake { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, string>
    {
        private readonly IMediator _mediator;
        
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var info = new FileInfo(request.InputFile);
            
            if (!Directory.Exists("./output"))
            {
                Directory.CreateDirectory("./output");
            }
            
            if (request.IsSkipAndTake)
            {
                for (int i = 1; i <= request.Skip; i++)
                {
                    for (int j = 1; j <= request.Take; j++)
                    {
                        var retval = await _mediator.Send(new SkipAndTakeText.Command(
                            request.InputFile,
                            i,
                            j,
                            request.ArrayIterations));
                        
                        await File.WriteAllTextAsync($"./output/skip_and_take_{info.Name.Split(['.'])[0]}_{i}_{j}.txt", retval, cancellationToken);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= request.Skip; i++)
                {
                    for (int j = 1; j <= request.Take; j++)
                    {
                        var retval = await _mediator.Send(new TakeAndSkipText.Command(
                            request.InputFile,
                            i,
                            j,
                            request.ArrayIterations));
                        
                        await File.WriteAllTextAsync($"./output/take_and_skip_{info.Name.Split(['.'])[0]}_{i}_{j}.txt", retval, cancellationToken);
                    }
                }
            }
            
            var skipTakeText = request.IsSkipAndTake ? "Skip And Take" : "Take And Skip";
            
            return $"Processed {skipTakeText}: {info.Name} {request.Skip} {request.Take} {request.ArrayIterations}" + Environment.NewLine;
        }
    }  
}