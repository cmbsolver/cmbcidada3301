using LiberPrimusAnalysisTool.Application.Commands.Directory;
using LiberPrimusAnalysisTool.Application.Commands.Image;
using LiberPrimusAnalysisTool.Application.Commands.InputProcessing;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool
{
    /// <summary>
    /// App
    /// </summary>
    public class App
    {
        /// <summary>
        /// The mediator
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="App" /> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public App(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Runs the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Run(string[] args)
        {
            _mediator.Publish(new CheckForOutputDirectory.Command()).Wait();

            var dontExit = true;

            while (dontExit)
            {
                Console.Clear();
                AnsiConsole.Write(new FigletText("Liber Primus Analysis Tool").Centered().Color(Color.Green));

                var selecttion = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("[green]Please select analysis to run[/]:")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more tests)[/]")
                    .AddChoices(new[] {
                        "1: Directory Utilities",
                        "2: Math Functions",
                        "3: Image Processing",
                        "4: Input Processing",
                        "98: Show Credits",
                        "99: Exit Program",
                    }));

                var choice = selecttion.Split(":")[0];

                // Echo the selection back to the terminal
                AnsiConsole.WriteLine($"Selected - {selecttion}");

                switch (choice.Trim())
                {
                    case "1":
                        _mediator.Publish(new DirectoryMenu.Command()).Wait();
                        break;

                    case "2":
                        _mediator.Publish(new MathMenu.Command()).Wait();
                        break;

                    case "3":
                        _mediator.Publish(new ImageMenu.Command()).Wait();
                        break;

                    case "4":
                        _mediator.Publish(new InputMenu.Command()).Wait();
                        break;

                    case "98":
                        _mediator.Publish(new ShowCredits.Command()).Wait();
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