using LiberPrimusAnalysisTool.Application.Queries;
using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.Directory
{
    /// <summary>
    /// Checks is number is prime
    /// </summary>
    public class CheckIfNumberIsPrime
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.IRequest" />
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
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                bool returnToMenu = false;

                while (!returnToMenu)
                {
                    Console.Clear();
                    AnsiConsole.Write(new FigletText("Check If Number Is Prime").Centered().Color(Color.Green));

                    var number = AnsiConsole.Ask<int>("What is the number?");

                    var isPrime = await _mediator.Send(new GetIsPrime.Query() { Number = number });

                    if (isPrime)
                    {
                        AnsiConsole.MarkupLine($"[green]{number} is prime[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]{number} is not prime[/]");
                    }

                    returnToMenu = AnsiConsole.Confirm("Return to menu?");
                }
            }
        }
    }
}