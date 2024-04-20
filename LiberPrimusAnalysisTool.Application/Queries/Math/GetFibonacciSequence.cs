using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// Get Fibonacci Sequence
    /// </summary>
    public class GetFibonacciSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Fibonacci";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(long number)
        {
            var fibonacciSequence = new GetFibonacciSequence.Query() { MaxNumber = number };

            return fibonacciSequence;
        }

        /// <summary>
        /// Prompts the command.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static object PromptCommand()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("Output Fib Sequence").Centered().Color(Color.Green));

            var number = AnsiConsole.Ask<int>("What is the max number?");
            var fibonacciSequence = new GetFibonacciSequence.Query() { MaxNumber = number };

            return fibonacciSequence;
        }

        /// <summary>
        /// Command
        /// </summary>
        public class Query : IRequest<NumericSequence>
        {
            public long MaxNumber { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            public Handler()
            {
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

                long a = 0;
                long b = 1;
                long c = 0;

                while (c <= request.MaxNumber)
                {
                    c = a + b;
                    a = b;
                    b = c;

                    if (c <= request.MaxNumber)
                    {
                        result.Sequence.Add(c);
                    }
                }

                return Task.FromResult(result);
            }
        }
    }
}