using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.TextUtilies
{
    /// <summary>
    /// TextUtilMenu
    /// </summary>
    public class TextUtilMenu
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
                    AnsiConsole.Write(new FigletText("Text Utilities").Centered().Color(Color.Green));

                    var selecttion = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[green]Please select text utility to run[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more text utilities)[/]")
                        .AddChoices(new[] {
                        "1: Get Words From Number List",
                        "99: Previous Menu",
                        }));

                    var choice = selecttion.Split(":")[0];

                    // Echo the selection back to the terminal
                    AnsiConsole.WriteLine($"Selected - {selecttion}");

                    switch (choice.Trim())
                    {
                        case "1":
                            //await _mediator.Publish(new GetWordFromInts.Command());
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