﻿using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Queries.Selection
{
    /// <summary>
    /// Indexes the liber primus pages into the database.
    /// </summary>
    public class GetImageSelection
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
                var returnAllImages = AnsiConsole.Confirm("Get all images?", false);

                if (returnAllImages)
                {
                    return Task.FromResult(GetImageFiles("./input/images"));
                }
                else
                {
                    var imagesList = GetImageFiles("./input/images");

                    var imageSelections = AnsiConsole.Prompt(
                                            new MultiSelectionPrompt<string>()
                                            .Title("[green]Please select images[/]:")
                                            .PageSize(10)
                                            .MoreChoicesText("[grey](Move up and down to reveal more images)[/]")
                                            .AddChoices(imagesList));

                    List<string> retval = [.. imageSelections];

                    return Task.FromResult(retval);
                }
            }

            /// <summary>
            /// Gets the image files.
            /// </summary>
            /// <param name="directory">The directory.</param>
            /// <returns></returns>
            private List<string> GetImageFiles(string directory)
            {
                List<string> retval = new List<string>();

                Directory.GetDirectories(directory).ToList().ForEach(d =>
                {
                    retval.AddRange(GetImageFiles(d));
                });

                Directory.GetFiles(directory).ToList().ForEach(f =>
                {
                    retval.Add(f);
                });

                return retval;
            }
        }
    }
}