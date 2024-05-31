using System.Text;
using LiberPrimusAnalysisTool.Application.Commands.TextUtilies;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing;

public class DeScytaleText
{
    public class Command : IRequest<string>
    {
        public Command(string text, int columnCount, bool reversed = false)
        {
            if (reversed)
            {
                Text = new string(text.Reverse().ToArray());
            }
            else
            {
                Text = text;
            }

            ColumnCount = columnCount;
        }
        
        public string Text { get; set; }
        
        public int ColumnCount { get; set; }
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
            StringBuilder sb = new StringBuilder();
            List<StringBuilder> rows = new List<StringBuilder>();
            
            for(int i = 0; i < request.ColumnCount; i++)
            {
                rows.Add(new StringBuilder());
            }

            int counter = 0;
            foreach (var character in request.Text)
            {
                rows[counter].Append(character);
                counter++;
                
                if (counter >= request.ColumnCount)
                {
                    counter = 0;
                }
            }

            foreach (var row in rows)
            {
                var text = await _mediator.Send(new TransposeRuneToLatin.Command(row.ToString()));
                text = await _mediator.Send(new FixUpControlChars.Command(text));
                sb.AppendLine(text);
            }
            
            return sb.ToString();
        }
    }
}