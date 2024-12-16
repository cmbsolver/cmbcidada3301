using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity.Image;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Color Isolation
    /// </summary>
    public class ColorIsolation
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.INotification" />
        public class Command : INotification
        {
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : INotificationHandler<Command>
        {
            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handles a notification
            /// </summary>
            /// <param name="notification">The notification</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task Handle(Command notification, CancellationToken cancellationToken)
            {
                bool returnToMenu = false;

                while (!returnToMenu)
                {
                    // Getting the pages we want
                    List<LiberPage> liberPages = new List<LiberPage>();
                    var pageSelection = new string[0]; //var pageSelection = await _mediator.Send(new GetImageSelection.Query());

                    foreach (var selection in pageSelection)
                    {
                        var tmpPage = await _mediator.Send(new GetPageData.Query(selection, false, false));
                        liberPages.Add(tmpPage);
                    }

                    ParallelOptions parallelOptions = new ParallelOptions();
                    parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;

                    // Copy the images to a new directory and set colors.
                    Parallel.ForEach(liberPages, parallelOptions, async page =>
                    {
                        string directiory = $"./output/imagep/{page.PageName}ci";

                        if (!System.IO.Directory.Exists(directiory))
                        {
                            System.IO.Directory.CreateDirectory(directiory);
                        }

                        var pageColors = await _mediator.Send(new GetPageColors.Query(page.FileName));

                        pageColors = pageColors.OrderByDescending(x => x.LiberColorInteger).ToList();

                        foreach (var color in pageColors)
                        {
                            File.Copy(page.FileName, $"{directiory}/{page.PageName}-{color.LiberColorHashless}.jpg", true);
                            using (var image = new MagickImage($"{directiory}/{page.PageName}-{color.LiberColorHashless}.jpg"))
                            using (var pixels = image.GetPixels())
                            {
                                foreach (var pixel in pixels)
                                {
                                    if (pixel.ToColor().ToHexString() == color.LiberColorHex)
                                    {
                                        if (image.ChannelCount == 2)
                                        {
                                            pixel.SetChannel(0, 0);
                                            pixel.SetChannel(1, 0);
                                        }
                                        else
                                        {
                                            pixel.SetChannel(0, 0);
                                            pixel.SetChannel(1, 0);
                                            pixel.SetChannel(2, 0);
                                        }
                                        pixels.SetPixel(pixel);
                                    }
                                }

                                image.Write($"{directiory}/{page.PageName}-{color.LiberColorHashless}.jpg");
                                pixels.Dispose();
                            }
                        }
                    });

                    returnToMenu = true;
                }
            }
        }
    }
}