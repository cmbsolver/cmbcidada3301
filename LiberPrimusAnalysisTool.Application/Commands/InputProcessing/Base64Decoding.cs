using LiberPrimusAnalysisTool.Application.Queries.Selection;
using MediatR;
using Spectre.Console;
using System.Text;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class Base64Decoding
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
                AnsiConsole.Write(new FigletText("Decode Base64 Lines").Centered().Color(Color.Green));
                var binFiles = _mediator.Send(new GetTextSelection.Query(false)).Result;

                DecodeFiles(binFiles);

                return Task.CompletedTask;
            }

            /// <summary>
            /// Decodes the files.
            /// </summary>
            /// <param name="files">The files.</param>
            private static void DecodeFiles(List<string> files)
            {
                List<FileInfo> fileInfos = [.. files.Select(file => new FileInfo(file))];
                fileInfos = fileInfos.OrderBy(fileInfos => fileInfos.Length).ToList();

                int counter = 1;
                foreach (var file in fileInfos)
                {
                    var base64Lines = File.ReadAllLines(file.FullName);
                    List<byte> bytes = new List<byte>();

                    AnsiConsole.WriteLine($"{counter}/{fileInfos.Count}: {file.Name} - {base64Lines.Length}");
                    counter++;

                    int lineCounter = 1;
                    foreach (var line in base64Lines)
                    {
                        AnsiConsole.WriteLine($"Processing line {lineCounter}/{base64Lines.Length}");
                        lineCounter++;

                        try
                        {
                            var decoded = Convert.FromBase64String(line);
                            if (decoded.Length > 0)
                            {
                                bytes.AddRange(decoded);
                                File.AppendAllText($"./output/text/{file.Name}.ASCII.txt", Encoding.ASCII.GetString(decoded));
                                File.AppendAllText($"./output/text/{file.Name}.UTF8.txt", Encoding.UTF8.GetString(decoded));
                            }
                        }
                        catch (Exception)
                        {
                            //AnsiConsole.WriteLine($"Error decoding base64 line in {file}: {line}");
                        }
                    }

                    if (bytes.Count > 0)
                    {
                        File.WriteAllBytes($"./output/bin/{file.Name}.bin", bytes.ToArray());
                    }
                }
            }
        }
    }
}