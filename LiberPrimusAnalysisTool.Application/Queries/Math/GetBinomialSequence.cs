using System;
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
        public static object BuildCommand(ulong number)
        {
            var binomialSequence = new GetBinomialSequence.Query() { MaxNumber = number };

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

                List<ulong> nnumbers = new List<ulong>();
                for (ulong n = 0; n <= request.MaxNumber; n++)
                {
                    nnumbers.Add(n);
                }

                ParallelOptions parallelOptions = new ParallelOptions();
                parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;

                List<Tuple<ulong, List<ulong>>> binomialSequences = new List<Tuple<ulong, List<ulong>>>();

                foreach (var n in nnumbers)
                {
                    List<ulong> binomialSequence = new List<ulong>();
                    ulong k = 0;
                    while (k <= n)
                    {
                        binomialSequence.Add(BinomialCoefficient(n, k));
                        k++;
                    }

                    if (binomialSequence.Any(x => x > request.MaxNumber))
                    {
                        break;
                    }
                    else
                    {
                        binomialSequences.Add(new Tuple<ulong, List<ulong>>(n, binomialSequence));
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
            private ulong BinomialCoefficient(ulong n, ulong k)
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
                    return 0;
                }
            }
        }
    }
}