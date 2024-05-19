using System;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// GetCatalanSequence
    /// </summary>
    public class GetCatalanSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Catalan";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(long number)
        {
            var result = new GetCatalanSequence.Query { MaxNumber = number };

            return result;
        }

        /// <summary>
        /// Request
        /// </summary>
        public class Query : IRequest<NumericSequence>
        {
            /// <summary>
            /// Gets or sets the maximum number.
            /// </summary>
            /// <value>
            /// The maximum number.
            /// </value>
            public long MaxNumber { get; set; }
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
                NumericSequence retval = new NumericSequence(Name);
                retval.Number = request.MaxNumber;
                long n = 0;
                try
                {
                    long catalan = 1;

                    while (catalan <= request.MaxNumber)
                    {
                        catalan = (2 * (2 * n + 1) * catalan) / (n + 2);
                        n++;

                        if (catalan > request.MaxNumber)
                        {
                            break;
                        }
                        else
                        {
                            retval.Sequence.Add(Convert.ToInt64(catalan));
                        }
                    }
                }
                catch { }

                return Task.FromResult(retval);
            }
        }
    }
}