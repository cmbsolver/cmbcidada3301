using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.Directory
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class FlushZeroOutputDirectory
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
                FlushDirectory("./output");

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
                        var file = new FileInfo(f);
                        if (file.Length == 0)
                        {
                            AnsiConsole.MarkupLine($"[red]Deleting 0 byte file[/]: {f}");
                            File.Delete(f);
                        }
                    });
                }
            }
        }
    }
}