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
    public class GetFibonacciPrimeSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Fibonacci Prime";
        
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
            var fibonacciSequence = new GetFibonacciPrimeSequence.Query(number, isPostional);
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
                
                var numberToCalculate = request.IsPositional ? ulong.MaxValue : request.MaxNumber;

                ulong a = 0;
                ulong b = 1;
                ulong c = 0;
                ulong counter = 0;

                while (c <= numberToCalculate)
                {
                    c = a + b;
                    a = b;
                    b = c;

                    if (c <= numberToCalculate)
                    {
                        var isPrime = _mediator.Send(new GetIsPrime.Query { Number = c }).Result;
                        if (isPrime)
                        {
                            if (!request.IsPositional)
                            {
                                result.Sequence.Add(c);
                            }
                            else
                            {
                                if (counter == request.MaxNumber)
                                {
                                    result.Sequence.Add(c);
                                    break;
                                }
                            }

                            counter++;
                        }
                    }
                }

                return Task.FromResult(result);
            }
        }
    }
}