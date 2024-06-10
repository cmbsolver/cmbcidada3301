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
    public class GetTotientSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Totient";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(ulong number)
        {
            var query = new Query() { Number = number };

            return query;
        }

        /// <summary>
        /// Query
        /// </summary>
        public class Query : IRequest<NumericSequence>
        {
            /// <summary>
            /// Gets or sets the number.
            /// </summary>
            /// <value>
            /// The number.
            /// </value>
            public ulong Number { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
        {
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
                var totient = new NumericSequence(Name);
                totient.Number = request.Number;
                var n = request.Number;

                for (ulong i = 1; i <= n; i++)
                {
                    if (GCD(i, n) == 1)
                    {
                        totient.Sequence.Add(i);
                    }
                }

                totient.Result = Convert.ToUInt64(totient.Sequence.Count);

                return Task.FromResult(totient);
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