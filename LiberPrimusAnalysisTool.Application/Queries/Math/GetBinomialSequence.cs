using LiberPrimusAnalysisTool.Application.Interfaces;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Queries.Math
{
    /// <summary>
    /// GetBinomialSequence
    /// </summary>
    public class GetBinomialSequence : ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public static string Name => "Binomial";

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static object BuildCommand(long number)
        {
            var binomialSequence = new GetBinomialSequence.Query() { MaxNumber = number };

            return binomialSequence;
        }

        /// <summary>
        /// Prompts the command.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static object PromptCommand()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("Output Binomial Sequence").Centered().Color(Color.Green));

            var nnumber = AnsiConsole.Ask<long>("What is the max N number?");
            var binomialSequence = new GetBinomialSequence.Query() { MaxNumber = nnumber };

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
                NumericSequence numericSequence = new NumericSequence(Name);
                numericSequence.Number = request.MaxNumber;

                List<long> nnumbers = new List<long>();
                for (long n = 0; n <= request.MaxNumber; n++)
                {
                    nnumbers.Add(n);
                }

                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;

                List<Tuple<long, List<long>>> binomialSequences = new List<Tuple<long, List<long>>>();

                foreach (var n in nnumbers)
                {
                    AnsiConsole.WriteLine($"Calculating binomial sequence for N={n}");
                    List<long> binomialSequence = new List<long>();
                    var k = 0;
                    while (k <= n)
                    {
                        binomialSequence.Add(BinomialCoefficient(n, k));
                        k++;
                    }

                    if (binomialSequence.Any(x => x > request.MaxNumber))
                    {
                        AnsiConsole.WriteLine($"Calculated binomial sequence for N={n}");
                        break;
                    }
                    else
                    {
                        binomialSequences.Add(new Tuple<long, List<long>>(n, binomialSequence));
                        AnsiConsole.WriteLine($"Calculated binomial sequence for N={n}");
                    }
                }

                binomialSequences = binomialSequences.OrderBy(x => x.Item1).ToList();

                foreach (var binomialSequence in binomialSequences)
                {
                    numericSequence.Sequence.AddRange(binomialSequence.Item2);
                }

                return Task.FromResult(numericSequence);
            }

            /// <summary>
            /// Binomials the coefficient.
            /// </summary>
            /// <param name="n">The n.</param>
            /// <param name="k">The k.</param>
            /// <returns></returns>
            private long BinomialCoefficient(long n, long k)
            {
                try
                {
                    if (k <= 0 || k == n)
                    {
                        return 1;
                    }

                    return BinomialCoefficient(n - 1, k - 1) + BinomialCoefficient(n - 1, k);
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine(ex.Message);
                    return 0;
                }
            }
        }
    }
}