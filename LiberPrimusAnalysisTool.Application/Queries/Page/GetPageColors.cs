using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Queries.Page
{
    /// <summary>
    /// Index Colors
    /// </summary>
    public class GetPageColors
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<IEnumerable<LiberColor>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query"/> class.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            public Query(string fileName)
            {
                FileName = fileName;
            }

            /// <summary>
            /// Gets or sets the page identifier.
            /// </summary>
            /// <value>
            /// The page identifier.
            /// </value>
            public string FileName { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, IEnumerable<LiberColor>>
        {
            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;

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
            public async Task<IEnumerable<LiberColor>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<LiberColor> colors = new List<LiberColor>();

                var page = await _mediator.Send(new GetPageData.Query(request.FileName, true, false));

                var pixColors = page.Pixels.Select(x => x.Hex).Distinct().OrderBy(x => x).ToList();
                
                colors.AddRange(pixColors.Select(x => new LiberColor(x)));

                GC.Collect();

                return colors;
            }
        }
    }
}