﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// GetBinomialSequence
    /// </summary>
    public class GetZekendorfRepresentationSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Zekenforf Representation";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(ulong number)
        {
            var binomialSequence = new GetZekendorfRepresentationSequence.Query() { MaxNumber = number };

            return binomialSequence;
        }
        
        /// <summary>
        /// Request
        /// </summary>
        public class Query : IRequest<NumericSequence>
        {
            /// <summary>
            /// Gets or sets the maximum n number.
            /// </summary>
            /// <value>
            /// The maximum n number.
            /// </value>
            public ulong MaxNumber { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
        {
            private readonly IMediator _mediator;
            
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
                List<ulong> sequence = new List<ulong>();
                var remainder = request.MaxNumber;
                
                while (remainder > 0)
                {
                    var fibSequence = await _mediator.Send(new GetFibonacciSequence.Query() { MaxNumber = remainder });
                    sequence.Add(fibSequence.Sequence.Last());
                    remainder = remainder - fibSequence.Sequence.Last();
                }

                sequence.Reverse();
                
                var retval = new NumericSequence(Name) { Sequence = sequence, Number = request.MaxNumber }; 
                
                return retval;
            }
        }
    }
}