using System.Text;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using MediatR;
using MediatR.Wrappers;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing.SkipAndTake;

public class SkipAndTakeText
{
    public class Command: IRequest<string>
    {
        public Command(string inputFile, int skip, int take, int arrayIterations)
        {
            InputFile = inputFile;
            Skip = skip;
            Take = take;
            ArrayIterations = arrayIterations;
        }

        public string InputFile { get; set; }
        
        public int Skip { get; set; }
        
        public int Take { get; set; }
        
        public int ArrayIterations { get; set; }
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
            var text = File.ReadAllText(request.InputFile);
            StringBuilder sb = new StringBuilder();

            int iterCounter = 0;
            int skipCounter = 0;
            int takeCounter = request.Take;
            
            while (iterCounter <= request.ArrayIterations)
            {
                skipCounter = skipCounter + request.Skip;
                
                if (skipCounter > text.Length - 1)
                {
                    skipCounter = 0;
                    sb.Append(text.Substring(skipCounter, takeCounter));
                    iterCounter++;
                    skipCounter = 0 + takeCounter;
                }
                else if (skipCounter + request.Take > text.Length - 1)
                {
                    var tmpLength = text.Length - skipCounter;
                    sb.Append(text.Substring(skipCounter, tmpLength));
                    takeCounter = request.Take - tmpLength;
                    sb.Append(text.Substring(0, takeCounter));
                    takeCounter = request.Take;
                    iterCounter++;
                    skipCounter = 0 + takeCounter;
                }
                else
                {
                    sb.Append(text.Substring(skipCounter, takeCounter));
                    skipCounter = skipCounter + request.Take;
                }
            }
            
            var rtext = await _mediator.Send(new TransposeRuneToLatin.Command(sb.ToString()));
            rtext = await _mediator.Send(new FixUpControlChars.Command(rtext));
            return rtext;
        }
    }
}