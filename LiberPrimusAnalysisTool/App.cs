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
            AnsiConsole.WriteLine("Moving items to GUI.");
        }
    }
}