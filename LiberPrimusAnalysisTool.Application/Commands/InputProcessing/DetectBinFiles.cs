using MediatR;
using Spectre.Console;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class DetectBinFiles
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
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
            public Task Handle(Command request, CancellationToken cancellationToken)
            {
                Console.Clear();
                AnsiConsole.Write(new FigletText("Detect Bin Files").Centered().Color(Color.Green));
                var binFiles = new string[0]; //var binFiles = _mediator.Send(new GetBinarySelection.Query()).Result;

                DetectBinaryFiles(binFiles);

                return Task.CompletedTask;
            }

            /// <summary>
            /// Detects the binary files.
            /// </summary>
            /// <param name="files">The files.</param>
            private static void DetectBinaryFiles(string[] files)
            {
                List<string> lines = new List<string>();

                if (files.Length == 0)
                {
                    AnsiConsole.WriteLine("No bin files found.");
                }
                else
                {
                    AnsiConsole.WriteLine("Bin files found:");
                    Parallel.ForEach(files, file =>
                    {
                        string justFileName = Path.GetFileName(file);
                        AnsiConsole.WriteLine(file);
                        FileTypeInterrogator.IFileTypeInterrogator interrogator = new FileTypeInterrogator.FileTypeInterrogator();

                        byte[] fileBytes = File.ReadAllBytes(file);

                        FileTypeInterrogator.FileTypeInfo fileTypeInfo = interrogator.DetectType(fileBytes);

                        if (fileTypeInfo == null)
                        {
                            AnsiConsole.WriteLine($"Could not detect file type for {justFileName}.");
                            lines.Add($"Could not detect file type for {file}.");
                            lines.Add("");
                        }
                        else
                        {
                            AnsiConsole.WriteLine("Name = " + fileTypeInfo.Name);
                            AnsiConsole.WriteLine("Extension = " + fileTypeInfo.FileType);
                            AnsiConsole.WriteLine("Mime Type = " + fileTypeInfo.MimeType);

                            lines.Add($"./output/bin/{justFileName}.{fileTypeInfo.FileType}");
                            lines.Add("Name = " + fileTypeInfo.Name);
                            lines.Add("Extension = " + fileTypeInfo.FileType);
                            lines.Add("Mime Type = " + fileTypeInfo.MimeType);
                            lines.Add("");

                            File.Copy(file, $"./output/bin/{justFileName}.{fileTypeInfo.FileType}");
                        }
                    });

                    File.AppendAllLines("./output/bin/detect_bin_files.txt", lines);
                }
            }
        }
    }
}