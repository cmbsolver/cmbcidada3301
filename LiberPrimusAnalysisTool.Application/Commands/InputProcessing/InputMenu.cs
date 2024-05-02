using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// OutputMenu
    /// </summary>
    public class InputMenu
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
                        "1: Detect Bin Files",
                        "2: Detect Words in Files",
                        "3: Decode Base64 Lines",
                        "4: Substitute Ultima",
                        "5: Brute Force Fiestel",
                        "6: Substitute Ultima Deux",
                        "99: Previous Menu",
                        }));

                    var choice = selecttion.Split(":")[0];

                    // Echo the selection back to the terminal
                    AnsiConsole.WriteLine($"Selected - {selecttion}");

                    switch (choice.Trim())
                    {
                        case "1":
                            await _mediator.Publish(new DetectBinFiles.Command());
                            break;

                        case "2":
                            await _mediator.Publish(new CheckFilesForWords.Command());
                            break;

                        case "3":
                            await _mediator.Publish(new Base64Decoding.Command());
                            break;

                        case "4":
                            await _mediator.Publish(new SubstituteUltima.Command());
                            break;
                        
                        case "5":
                            await _mediator.Publish(new BruteForceFiestel.Command());
                            break;
                        
                        case "6":
                            await _mediator.Publish(new SubstituteUltima2.Command());
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