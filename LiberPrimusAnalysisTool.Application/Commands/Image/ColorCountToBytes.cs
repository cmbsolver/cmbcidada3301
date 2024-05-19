using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// ColorReport
    /// </summary>
    public class ColorCountToBytes
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : INotification
        {
        }

        /// <summary>
        ///Handler
        /// </summary>
        public class Handler : INotificationHandler<Command>
        {
            /// <summary>
            /// The character repo
            /// </summary>
            private readonly ICharacterRepo _characterRepo;

            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="characterRepo">The character repo.</param>
            /// <param name="mediator">The mediator.</param>
            public Handler(ICharacterRepo characterRepo, IMediator mediator)
            {
                _characterRepo = characterRepo;
                _mediator = mediator;
            }

            /// <summary>
            /// Handles a request
            /// </summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var invertPixels = false;

                var tag = invertPixels ? "Reversed" : "Normal";

                var files = new string[0]; //var files = await _mediator.Send(new GetImageSelection.Query());

                Parallel.ForEach(files, async ifile =>
                {
                    var fileInfo = new FileInfo(ifile);
                    var file = await _mediator.Send(new GetPageData.Query(ifile, true, invertPixels));

                    string currentColor = string.Empty;
                    int currentColorCount = 0;
                    List<byte> bytes = new List<byte>();

                    foreach (var pixel in file.Pixels)
                    {
                        if (pixel.Hex != currentColor)
                        {
                            if (currentColor.Length > 0 && currentColorCount <= byte.MaxValue)
                            {
                                bytes.Add(Convert.ToByte(currentColorCount));
                            }

                            currentColor = pixel.Hex;
                            currentColorCount = 0;
                        }

                        currentColorCount++;
                    }

                    if (currentColorCount <= byte.MaxValue)
                    {
                        bytes.Add(Convert.ToByte(currentColorCount));
                    }

                    File.WriteAllBytes($"./output/bin/CTB-{tag}-{fileInfo.Name}.bin", bytes.ToArray());
                });
            }
        }
    }
}