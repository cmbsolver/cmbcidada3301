using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using Spectre.Console;
using System.Text;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Color Report
    /// </summary>
    public class ColorReport
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
                    Console.Clear();
                    AnsiConsole.Write(new FigletText("Color Report").Centered().Color(Color.Green));

                    // Getting the pages we want
                    List<LiberPage> liberPages = new List<LiberPage>();
                    var pageSelection = new string[0]; //var pageSelection = await _mediator.Send(new GetImageSelection.Query());

                    foreach (var selection in pageSelection)
                    {
                        var tmpPage = await _mediator.Send(new GetPageData.Query(selection, true, false));
                        liberPages.Add(tmpPage);
                    }

                    ParallelOptions parallelOptions = new ParallelOptions();
                    parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount / 2;

                    // Copy the images to a new directory and set colors.
                    Parallel.ForEach(liberPages, parallelOptions, async page =>
                    {
                        string reportFile = $"./output/{page.PageName}.report.txt";

                        StringBuilder report = new StringBuilder();
                        report.AppendLine($"Color Report");
                        report.AppendLine($"---------------------------------");
                        report.AppendLine($"Page: {page.PageName}");
                        report.AppendLine($"Total Colors: {page.TotalColors}");
                        report.AppendLine($"Height: {page.Height}");
                        report.AppendLine($"Width: {page.Width}");
                        report.AppendLine($"Total Pixels: {page.PixelCount}");
                        report.AppendLine($"Signature: {page.PageSig}");
                        report.AppendLine(string.Empty);
                        report.AppendLine($"Colors");
                        report.AppendLine($"---------------------------------");
                        var colors = await _mediator.Send(new GetPageColors.Query(page.FileName));

                        foreach (var color in colors)
                        {
                            report.AppendLine(color.LiberColorHex);
                        }

                        await File.WriteAllTextAsync(reportFile, report.ToString());
                    });

                    returnToMenu = AnsiConsole.Confirm("Return to main menu?");
                }
            }
        }
    }
}