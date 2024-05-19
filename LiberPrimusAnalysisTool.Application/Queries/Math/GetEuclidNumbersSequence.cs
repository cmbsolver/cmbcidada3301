using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// Get the prime sequence
    /// </summary>
    public class GetEuclidNumbersSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Euclid Numbers";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(long number)
        {
            var primeSequence = new GetEuclidNumbersSequence.Query() { Number = number };

            return primeSequence;
        }

        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<NumericSequence>
        {
            public long Number { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
        {
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handles a request
            /// </summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            /// <returns>
            /// Response from the request
            /// </returns>
            public async Task<NumericSequence> Handle(Query request, CancellationToken cancellationToken)
            {
                var primoSequence = await _mediator.Send(new GetPrimorialSequence.Query() { Number = request.Number });
                NumericSequence numericSequence = new NumericSequence(Name);
                numericSequence.Number = request.Number;
                
                for (int i = 0; i < primoSequence.Sequence.Count; i++)
                {
                    var value = primoSequence.Sequence[i] + 1;
                    numericSequence.Sequence.Add(value);
                }

                return numericSequence;
            }
        }
    }
}