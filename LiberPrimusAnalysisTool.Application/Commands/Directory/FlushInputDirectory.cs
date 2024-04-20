using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.Directory
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class FlushInputDirectory
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
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public Task Handle(Command request, CancellationToken cancellationToken)
            {
                Console.Clear();
                AnsiConsole.Write(new FigletText("Flush input directories").Centered().Color(Color.Green));
                FlushDirectory("./input/bins");
                FlushDirectory("./input/text");

                return Task.CompletedTask;
            }

            /// <summary>
            /// Flushes the directory.
            /// </summary>
            /// <param name="path">The path.</param>
            public void FlushDirectory(string path)
            {
                System.IO.Directory.GetDirectories(path).ToList().ForEach(d =>
                {
                    FlushDirectory(d);
                });

                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.EnumerateFiles(path).ToList().ForEach(f =>
                    {
                        AnsiConsole.MarkupLine($"[red]Deleting[/]: {f}");
                        File.Delete(f);
                    });
                }
            }
        }
    }
}