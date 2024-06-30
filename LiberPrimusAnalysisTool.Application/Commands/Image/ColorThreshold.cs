using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Color Isolation
    /// </summary>
    public class ColorThreshold
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
                        List<Tuple<LiberColor, string, string, List<LiberColor>>> tests = new List<Tuple<LiberColor, string, string, List<LiberColor>>>();
                        var pageColors = await _mediator.Send(new GetPageColors.Query(page.FileName));

                        for (int i = 0; i < int.MaxValue; i++)
                        {
                            List<LiberColor> thresholdColors = new List<LiberColor>();
                            string threasholdTest = string.Empty;

                            switch (i)
                            {
                                case 0:
                                    threasholdTest = "Normal";
                                    break;

                                case 1:
                                    threasholdTest = "Reverse";
                                    pageColors = pageColors.Reverse().ToList();
                                    break;

                                case 3:
                                    threasholdTest = "W2B";
                                    pageColors = pageColors.OrderByDescending(x => x.LiberColorInteger).ToList();
                                    break;

                                case 4:
                                    threasholdTest = "B2W";
                                    pageColors = pageColors.OrderBy(x => x.LiberColorInteger).ToList();
                                    break;

                                default:
                                    continue;
                            }

                            foreach (var color in pageColors)
                            {
                                string directiory = $"./output/imagep/{page.PageName}cit/{threasholdTest}";
                                thresholdColors.Add(color);

                                List<LiberColor> tmpThresholdColors = new List<LiberColor>();
                                foreach (var threasholdColor in thresholdColors)
                                {
                                    tmpThresholdColors.Add(new LiberColor(threasholdColor.LiberColorHex));
                                }

                                tests.Add(new Tuple<LiberColor, string, string, List<LiberColor>>(color, directiory, color.LiberColorHex, tmpThresholdColors));

                                if (!System.IO.Directory.Exists(directiory))
                                {
                                    System.IO.Directory.CreateDirectory(directiory);
                                }
                            }
                        }

                        Parallel.ForEach(tests, parallelOptions, test =>
                        {
                            string fileName = $"{test.Item2}/{page.PageName}-{test.Item1.LiberColorHashless}.jpg";
                            File.Copy(page.FileName, fileName, true);
                            using (var image = new MagickImage(fileName))
                            using (var pixels = image.GetPixels())
                            {
                                foreach (var pixel in pixels)
                                {
                                    if (image.ChannelCount == 2)
                                    {
                                        if (test.Item4.Any(x => x.LiberColorHex == pixel.ToColor().ToHexString()))
                                        {
                                            pixel.SetChannel(0, 0);
                                            pixel.SetChannel(1, 255);
                                            pixels.SetPixel(pixel);
                                        }
                                        else
                                        {
                                            pixel.SetChannel(0, 65535);
                                            pixel.SetChannel(1, 255);
                                            pixels.SetPixel(pixel);
                                        }
                                    }
                                    else
                                    {
                                        if (test.Item4.Any(x => x.LiberColorHex == pixel.ToColor().ToHexString()))
                                        {
                                            pixel.SetChannel(0, 0);
                                            pixel.SetChannel(1, 0);
                                            pixel.SetChannel(2, 0);
                                            pixels.SetPixel(pixel);
                                        }
                                        else
                                        {
                                            pixel.SetChannel(0, 65535);
                                            pixel.SetChannel(1, 65535);
                                            pixel.SetChannel(2, 65535);
                                            pixels.SetPixel(pixel);
                                        }
                                    }
                                }

                                image.Write(fileName);
                                pixels.Dispose();
                            }
                        });
                    });

                    returnToMenu = true;
                }
            }
        }
    }
}