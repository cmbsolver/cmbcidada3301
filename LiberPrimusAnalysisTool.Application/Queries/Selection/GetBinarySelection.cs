using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Queries.Selection
{
    /// <summary>
    /// Indexes the liber primus pages into the database.
    /// </summary>
    public class GetBinarySelection
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<List<string>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query"/> class.
            /// </summary>
            /// <param name="includeImageData">if set to <c>true</c> [include image data].</param>
            public Query()
            {
            }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, List<string>>
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
            public Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                var returnAllImages = AnsiConsole.Confirm("Get all binary files?", false);

                if (returnAllImages)
                {
                    return Task.FromResult(GetBinaryFiles("./input/bins"));
                }
                else
                {
                    var binList = GetBinaryFiles("./input/bins");

                    var binSelections = AnsiConsole.Prompt(
                                            new MultiSelectionPrompt<string>()
                                            .Title("[green]Please select binary files[/]:")
                                            .PageSize(10)
                                            .MoreChoicesText("[grey](Move up and down to reveal more binary files)[/]")
                                            .AddChoices(binList));

                    List<string> retval = [.. binSelections];

                    return Task.FromResult(retval);
                }
            }

            /// <summary>
            /// Gets the image files.
            /// </summary>
            /// <param name="directory">The directory.</param>
            /// <returns></returns>
            private List<string> GetBinaryFiles(string directory)
            {
                List<string> retval = new List<string>();

                Directory.GetDirectories(directory).ToList().ForEach(d =>
                {
                    retval.AddRange(GetBinaryFiles(d));
                });

                Directory.GetFiles(directory).ToList().ForEach(f =>
                {
                    retval.Add(f);
                });

                return retval.OrderBy(x => x).ToList();
            }
        }
    }
}