﻿using System.Threading;
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

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(ulong number)
        {
            var fibonacciSequence = new GetFibonacciPrimeSequence.Query() { MaxNumber = number };

            return fibonacciSequence;
        }

        /// <summary>
        /// Command
        /// </summary>
        public class Query : IRequest<NumericSequence>
        {
            public ulong MaxNumber { get; set; }
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

                ulong a = 0;
                ulong b = 1;
                ulong c = 0;

                while (c <= request.MaxNumber)
                {
                    c = a + b;
                    a = b;
                    b = c;

                    if (c <= request.MaxNumber)
                    {
                        var isPrime = _mediator.Send(new GetIsPrime.Query { Number = c }).Result;
                        if (isPrime)
                        {
                            result.Sequence.Add(c);
                        }
                    }
                }

                return Task.FromResult(result);
            }
        }
    }
}