﻿using System;
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

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(ulong number)
        {
            var result = new GetCakeSequence.Query { MaxNumber = number };

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
            public ulong MaxNumber { get; set; }
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

                for (ulong n = 0; n <= request.MaxNumber; n++)
                {
                    try
                    {
                        var next = (System.Math.Pow(n, 3) + 5 * n + 6) / 6;
                        retval.Sequence.Add(Convert.ToUInt64(next));
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