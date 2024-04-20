using LiberPrimusAnalysisTool.Entity;
using MediatR;
using Spectre.Console;
using System.Reflection;

namespace LiberPrimusAnalysisTool.Application.Commands.Math
{
    /// <summary>
    /// Calculate Totient
    /// </summary>
    public class CalculateSequence
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Query : IRequest<NumericSequence>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query" /> class.
            /// </summary>
            /// <param name="outputToFile">if set to <c>true</c> [output to file].</param>
            /// <param name="promptForCommand">if set to <c>true</c> [prompt for command].</param>
            /// <param name="value">The value.</param>
            /// <param name="name">The name.</param>
            public Query(bool outputToFile, bool promptForCommand, long value, string name)
            {
                OutputToFile = outputToFile;
                PromptForCommand = promptForCommand;
                Value = value;
                NameToRun = name;
            }

            /// <summary>
            /// Gets or sets a value indicating whether [output to file].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [output to file]; otherwise, <c>false</c>.
            /// </value>
            public bool OutputToFile { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [prompt for command].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [prompt for command]; otherwise, <c>false</c>.
            /// </value>
            public bool PromptForCommand { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string NameToRun { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public long Value { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, NumericSequence>
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
            public async Task<NumericSequence> Handle(Query request, CancellationToken cancellationToken)
            {
                bool returnToMenu = false;
                NumericSequence numericSequence = null;

                while (!returnToMenu)
                {
                    if (request.PromptForCommand)
                    {
                        Console.Clear();
                        AnsiConsole.Write(new FigletText("Calculate Sequence").Centered().Color(Color.Green));
                    }

                    object query = null;

                    Assembly asm = Assembly.GetAssembly(typeof(CalculateSequence));
                    List<Tuple<Type, string>> mathTypes = new List<Tuple<Type, string>>();
                    var counter = 1;

                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" && type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                        {
                            var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault().GetValue(null);
                            mathTypes.Add(new Tuple<Type, string>(type, $"{counter}: {name}"));
                            counter++;
                        }
                    }

                    if (request.PromptForCommand)
                    {
                        var selecttion = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("[green]Please select sequence to calculate[/]:")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more sequences)[/]")
                        .AddChoices(mathTypes.Select(x => x.Item2)));

                        // Echo the selection back to the terminal
                        AnsiConsole.WriteLine($"Selected - {selecttion}");

                        var stype = mathTypes.FirstOrDefault(x => x.Item2 == selecttion).Item1;

                        query = stype.GetMethod("PromptCommand").Invoke(null, null);
                    }
                    else
                    {
                        var stype = mathTypes.FirstOrDefault(x => x.Item2.Contains(request.NameToRun)).Item1;

                        query = stype.GetMethod("BuildCommand").Invoke(null, new object[1] { request.Value });
                    }

                    AnsiConsole.MarkupLine($"[green]Calculating sequence...[/]");
                    numericSequence = (NumericSequence)await _mediator.Send(query);

                    if (numericSequence != null)
                    {
                        if (numericSequence.Result != null)
                        {
                            AnsiConsole.MarkupLine($"[green]Result = {numericSequence.Result}[/]");
                        }

                        if (numericSequence.Sequence.Any() && request.OutputToFile)
                        {
                            AnsiConsole.Status()
                                .AutoRefresh(true)
                                .Spinner(Spinner.Known.Circle)
                                .SpinnerStyle(Style.Parse("green bold"))
                                .Start($"./output/math/sequence-{numericSequence.Name}-{numericSequence.Number}.txt", ctx =>
                        {
                            foreach (var item in numericSequence.Sequence)
                            {
                                ctx.Status($"Outputting sequence to ./output/math/sequence-{numericSequence.Name}-{numericSequence.Number}.txt - {item}");
                                ctx.Refresh();
                                File.AppendAllText($"./output/math/sequence-{numericSequence.Name}-{numericSequence.Number}.txt", $"{item}" + Environment.NewLine);
                            }
                        });

                            AnsiConsole.MarkupLine($"[red]Check output math directory for sequence file[/]");
                        }

                        if (request.PromptForCommand)
                        {
                            returnToMenu = AnsiConsole.Confirm("Return to menu?");
                        }
                        else
                        {
                            returnToMenu = true;
                        }
                    }
                }

                return numericSequence;
            }
        }
    }
}