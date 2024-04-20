using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries
{
    /// <summary>
    /// Indexes the liber primus pages longo the database.
    /// </summary>
    public class GetIsPrime
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<bool>
        {
            public long Number { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, bool>
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
            public Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.Number <= 1) return Task.FromResult(false);
                if (request.Number == 2) return Task.FromResult(true);
                if (request.Number % 2 == 0) return Task.FromResult(false);

                var boundary = (long)System.Math.Floor(System.Math.Sqrt(request.Number));

                for (long i = 3; i <= boundary; i += 2)
                {
                    if (request.Number % i == 0)
                    {
                        return Task.FromResult(false);
                    }
                }

                return Task.FromResult(true);
            }
        }
    }
}