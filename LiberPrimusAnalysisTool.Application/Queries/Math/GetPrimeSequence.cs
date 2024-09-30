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
    public class GetPrimeSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Prime";
        
        // <summary>
        /// Get whether the sequence is positional.
        /// </summary>
        public static bool IsPositional { get; set; }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(ulong number, bool isPostional)
        {
            var primeSequence = new GetPrimeSequence.Query(number, isPostional);
            IsPositional = isPostional;

            return primeSequence;
        }

        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<NumericSequence>
        {
            public Query(ulong maxNumber, bool isPositional)
            {
                MaxNumber = maxNumber;
                IsPositional = isPositional;
            }
            
            /// <summary>
            /// Gets or sets the maximum n number.
            /// </summary>
            /// <value>
            /// The maximum n number.
            /// </value>
            public ulong MaxNumber { get; set; }
            
            /// <summary>
            /// Gets whether it is positional.
            /// </summary>
            public bool IsPositional { get; set; }
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
                NumericSequence numericSequence = new NumericSequence(Name);
                numericSequence.Number = request.MaxNumber;
                
                var numberToCalculate = request.IsPositional ? long.MaxValue : request.MaxNumber;
                ulong counter = 0;
                
                for (ulong i = 0; i <= numberToCalculate; i++)
                {
                    var isPrime = await _mediator.Send(new GetIsPrime.Query() { Number = i });

                    if (isPrime)
                    {
                        if (!request.IsPositional)
                        {
                            numericSequence.Sequence.Add(i);
                        }
                        else
                        {
                            if (counter == request.MaxNumber)
                            {
                                numericSequence.Sequence.Add(i);
                                break;
                            }
                        }

                        counter++;
                    }
                }

                return numericSequence;
            }
        }
    }
}