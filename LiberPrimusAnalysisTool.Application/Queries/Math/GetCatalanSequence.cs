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
            var result = new GetCatalanSequence.Query(number, isPostional);
            IsPositional = isPostional;

            return result;
        }

        /// <summary>
        /// Request
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
                
                var numberToCalculate = request.IsPositional ? int.MaxValue : request.MaxNumber;
                
                ulong n = 0;
                try
                {
                    ulong catalan = 1;

                    while (catalan <= numberToCalculate)
                    {
                        catalan = (2 * (2 * n + 1) * catalan) / (n + 2);
                        n++;

                        if (!request.IsPositional)
                        {
                            if (catalan > request.MaxNumber)
                            {
                                break;
                            }
                            else
                            {
                                retval.Sequence.Add(Convert.ToUInt64(catalan));
                            }
                        }
                        else
                        {
                            if (n == request.MaxNumber)
                            {
                                retval.Sequence.Add(Convert.ToUInt64(catalan));
                                break;
                            }
                        }
                    }
                }
                catch { }

                return Task.FromResult(retval);
            }
        }
    }
}