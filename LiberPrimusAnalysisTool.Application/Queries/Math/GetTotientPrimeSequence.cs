using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// Get Totient Sequence
    /// </summary>
    public class GetTotientPrimeSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Totient Prime";
        
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
            var query = new Query(number, isPostional);
            IsPositional = isPostional;

            return query;
        }

        /// <summary>
        /// Query
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
            /// <summary>
            /// mediator interface.
            /// </summary>
            private IMediator _mediatr;
            
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="mediatr"></param>
            public Handler(IMediator mediatr)
            {
                _mediatr = mediatr;
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
                var totient = new NumericSequence(Name);
                totient.Number = request.MaxNumber;
                var n = request.MaxNumber;
                
                for (ulong i = 1; i <= n; i++)
                {
                    if (GCD(i, n) == 1)
                    {
                        var isPrime = await _mediatr.Send(new GetIsPrime.Query() { Number = i });
                        
                        if (isPrime)
                            totient.Sequence.Add(i);
                    }
                }

                return totient;
            }

            /// <summary>
            /// GCDs the specified a.
            /// </summary>
            /// <param name="a">a.</param>
            /// <param name="b">The b.</param>
            /// <returns></returns>
            private ulong GCD(ulong a, ulong b)
            {
                while (b != 0)
                {
                    var t = b;
                    b = a % b;
                    a = t;
                }

                return a;
            }
        }
    }
}