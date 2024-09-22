using System;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// GetCakeSequence
    /// </summary>
    public class GetCakeSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Cake";
        
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
            var result = new GetCakeSequence.Query(number, isPostional);
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

                for (ulong n = 0; n <= numberToCalculate; n++)
                {
                    try
                    {
                        if (!request.IsPositional)
                        {
                            var next = (System.Math.Pow(n, 3) + 5 * n + 6) / 6;
                            retval.Sequence.Add(Convert.ToUInt64(next));
                        }
                        else
                        {
                            if (n == request.MaxNumber)
                            {
                                var next = (System.Math.Pow(n, 3) + 5 * n + 6) / 6;
                                retval.Sequence.Add(Convert.ToUInt64(next));
                                break;
                            }
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                return Task.FromResult(retval);
            }
        }
    }
}