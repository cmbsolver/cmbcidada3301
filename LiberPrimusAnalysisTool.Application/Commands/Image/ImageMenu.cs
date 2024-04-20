using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// ImageMenu
    /// </summary>
    public class ImageMenu
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
                var dontExit = true;

                while (dontExit)
                {
                    Console.Clear();
                    AnsiConsole.Write(new FigletText("Liber Primus Analysis Tool").Centered().Color(Color.Green));

                    var selecttion = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[green]Please select directory utility to run[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more directory utilities)[/]")
                        .AddChoices(new[] {
                        "1: Reverse bytes",
                        "2: RGB -> Text",
                        "3: Bulk Winnow Page(s) (Bytes)",
                        "4: Bulk Winnow Page(s) (Non-DCT Pixel)",
                        "5: Isolate Color(s)",
                        "6: Isolate Color(s) (Var 2)",
                        "7: Color Threshold",
                        "8: Color Report",
                        "9: Color Count to Bytes",
                        "10: Color Count to Bytes (Var 2)",
                        "99: Previous Menu",
                        }));

                    var choice = selecttion.Split(":")[0];

                    // Echo the selection back to the terminal
                    AnsiConsole.WriteLine($"Selected - {selecttion}");

                    switch (choice.Trim())
                    {
                        case "1":
                            await _mediator.Publish(new ReverseBytes.Command());
                            break;

                        case "2":
                            await _mediator.Publish(new RgbIndex.Command());
                            break;

                        case "3":
                            await _mediator.Publish(new BulkByteWinnowPages.Command());
                            break;

                        case "4":
                            await _mediator.Publish(new BulkPixelWinnowPages.Command());
                            break;

                        case "5":
                            await _mediator.Publish(new ColorIsolation.Command());
                            break;

                        case "6":
                            await _mediator.Publish(new ColorIsolationVar2.Command());
                            break;

                        case "7":
                            await _mediator.Publish(new ColorThreshold.Command());
                            break;

                        case "8":
                            await _mediator.Publish(new ColorReport.Command());
                            break;

                        case "9":
                            await _mediator.Publish(new ColorCountToBytes.Command());
                            break;

                        case "10":
                            await _mediator.Publish(new ColorCountToBytesVar2.Command());
                            break;

                        case "99":
                            dontExit = false;
                            break;

                        default:
                            AnsiConsole.Markup("[red]Not a valid choice.[/]");
                            break;
                    }
                }
            }
        }
    }
}