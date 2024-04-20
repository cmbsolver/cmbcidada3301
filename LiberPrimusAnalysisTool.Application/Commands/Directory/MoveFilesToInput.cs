using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.Directory
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class MoveFilesToInput
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
                MoveFiles("./output");

                return Task.CompletedTask;
            }

            /// <summary>
            /// Flushes the directory.
            /// </summary>
            /// <param name="path">The path.</param>
            public void MoveFiles(string path)
            {
                System.IO.Directory.GetDirectories(path).ToList().ForEach(d =>
                {
                    MoveFiles(d);
                });

                if (System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.EnumerateFiles(path).ToList().ForEach(f =>
                    {
                        var file = new FileInfo(f);
                        if (file.Extension.ToLower().Contains("txt"))
                        {
                            AnsiConsole.MarkupLine($"[red]Moved {file.Name} to input text[/]");
                            File.Move(f, $"./input/text/{file.Name}", true);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine($"[red]Moved {file.Name} to input bin[/]");
                            File.Move(f, $"./input/bins/{file.Name}", true);
                        }
                    });
                }
            }
        }
    }
}