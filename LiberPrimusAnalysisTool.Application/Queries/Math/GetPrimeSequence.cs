﻿using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// Get the prime sequence
    /// </summary>
    public class GetPrimeSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Prime";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(long number)
        {
            var primeSequence = new GetPrimeSequence.Query() { Number = number };

            return primeSequence;
        }

        /// <summary>
        /// Prompts the command.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static object PromptCommand()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("Output Prime Sequence").Centered().Color(Color.Green));

            var number = AnsiConsole.Ask<int>("What is the max number?");
            var primeSequence = new GetPrimeSequence.Query() { Number = number };

            return primeSequence;
        }

        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<NumericSequence>
        {
            public long Number { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
        {
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
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
                NumericSequence numericSequence = new NumericSequence(Name);
                numericSequence.Number = request.Number;

                for (long i = 0; i <= request.Number; i++)
                {
                    var isPrime = await _mediator.Send(new GetIsPrime.Query() { Number = i });

                    if (isPrime)
                    {
                        numericSequence.Sequence.Add(i);
                    }
                }

                return numericSequence;
            }
        }
    }
}