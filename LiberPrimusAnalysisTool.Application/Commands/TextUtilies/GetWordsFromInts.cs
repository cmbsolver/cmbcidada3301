using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies
{
    /// <summary>
    /// Get Word From Ints
    /// </summary>
    public class GetWordFromInts
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : IRequest<string[]>
        {
            public Command(long value)
            {
                Value = value;
            }
            
            public long Value { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Command, string[]>
        {
            /// <summary>
            /// The character repo
            /// </summary>
            private readonly ICharacterRepo _characterRepo;
            
            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="characterRepo">The character repo.</param>
            /// <param name="mediator"></param>
            public Handler(ICharacterRepo characterRepo, IMediator mediator)
            {
                _mediator = mediator;
                _characterRepo = characterRepo;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task<string[]> Handle(Command request, CancellationToken cancellationToken)
            {
                List<string> words = new List<string>();
                HashSet<Tuple<string, string, long>> dictValues = new HashSet<Tuple<string, string, long>>();
                
                using (var file = File.OpenText("words.txt"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var text = await _mediator.Send(new PrepLatinToRune.Command(line));
                        var runes = await _mediator.Send(new TransposeLatinToRune.Command(text));
                        var value = await _mediator.Send(new CalculateGematriaSum.Command(runes));
                        
                        dictValues.Add(new Tuple<string, string, long>(line, runes, long.Parse(value)));
                    }

                    file.Close();
                    file.Dispose();
                }
                
                words.AddRange(dictValues.Where(x => x.Item3 == request.Value).Select(x => $"{x.Item1.ToUpper()} - {x.Item2} - {x.Item3}"));

                return words.ToArray();
            }
        }
    }
}