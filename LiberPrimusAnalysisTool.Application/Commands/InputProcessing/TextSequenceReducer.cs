using System.Text;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing;

public class TextSequenceReducer
{
    public class Command : IRequest<string>
    {
        public Command(string sequenceName, string fileName, bool reversed)
        {
            SequenceName = sequenceName;
            FileName = fileName;
            Reversed = reversed;
        }
        
        public string SequenceName { get; set; }
        
        public string FileName { get; set; }
        
        public bool Reversed { get; set; }
    }

    public class Handler : IRequestHandler<Command, string>
    {
        private IMediator _mediator;
        
        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var text = request.Reversed ? 
                File.ReadAllText(request.FileName).Reverse().ToList() 
                : File.ReadAllText(request.FileName).ToList();
            
            StringBuilder sb = new StringBuilder();

            if (request.SequenceName != "Natural")
            {
                int counter = 0;
                while (counter < int.MaxValue / 2 && text.Count > 0)
                {
                    if (text.Count <= 2)
                    {
                        sb.Append(text.ToArray());
                        break;
                    }

                    var sequence = await _mediator.Send(new CalculateSequence.Query(Convert.ToUInt64(text.Count), request.SequenceName));

                    var removeCount = request.SequenceName;

                    foreach (var number in sequence.Sequence)
                    {
                        sb.Append(text[(int)number - 1]);
                    }

                    for (int i = (int)sequence.Sequence.Count - 1; i >= 0; i--)
                    {
                        text.RemoveAt((int)sequence.Sequence[i] - 1);
                    }

                    counter++;
                }

                if (text.Count > 0)
                {
                    sb.Append(text.ToArray());
                }
            }
            else
            {
                sb.Append(text.ToArray());
            }

            var retval = await _mediator.Send(new TransposeRuneToLatin.Command(sb.ToString()));
            retval = await _mediator.Send(new FixUpControlChars.Command(retval));
            
            return retval;
        }
    }
}