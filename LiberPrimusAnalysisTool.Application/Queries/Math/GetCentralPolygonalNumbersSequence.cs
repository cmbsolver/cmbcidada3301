using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// Get Fibonacci Sequence
    /// </summary>
    public class GetCentralPolygonalNumbersSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Central Polygonal Numbers";
        
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
            var fibonacciSequence = new GetCentralPolygonalNumbersSequence.Query(number, isPostional);
            IsPositional = isPostional;

            return fibonacciSequence;
        }

        /// <summary>
        /// Command
        /// </summary>
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
            private IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
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
            public Task<NumericSequence> Handle(Query request, CancellationToken cancellationToken)
            {
                NumericSequence result = new NumericSequence(Name);
                result.Number = request.MaxNumber;
                
                var numberToCalculate = request.IsPositional ? int.MaxValue : request.MaxNumber;

                for (ulong n = 0; n < numberToCalculate; n++)
                {
                    try
                    {
                        if (!request.IsPositional)
                        {
                            var item = n * (n + 1) / 2 + 1;
                            result.Sequence.Add(item);
                        }
                        else
                        {
                            if (n == request.MaxNumber)
                            {
                                var item = n * (n - 1) / 2 + 1;
                                result.Sequence.Add(item);
                                break;
                            }
                        }
                    }
                    catch
                    {
                        break;   
                    }
                }

                return Task.FromResult(result);
            }
        }
    }
}