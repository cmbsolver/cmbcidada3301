using System;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Entity.Image;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// ColorReport
    /// </summary>
    public class RgbIndex
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
        /// <seealso cref="ColorReport.Command&gt;" />
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
                Console.Clear();
                var invertPixels = false;

                var includeControlCharacters = false;

                var files = new string[0]; //var files = await _mediator.Send(new GetImageSelection.Query());

                Parallel.ForEach(files, async ifile =>
                {
                    var file = await _mediator.Send(new GetPageData.Query(ifile, true, invertPixels));
                    var rgbIndex = new RgbCharacters(file.PageName);

                    foreach (var pixel in file.Pixels)
                    {
                        rgbIndex.AddR(_characterRepo.GetASCIICharFromDec(pixel.R, includeControlCharacters));
                        rgbIndex.AddG(_characterRepo.GetASCIICharFromDec(pixel.G, includeControlCharacters));
                        rgbIndex.AddB(_characterRepo.GetASCIICharFromDec(pixel.B, includeControlCharacters));
                    }

                    File.AppendAllText($"./output/imagep/RgbIndex_{file.PageName}.txt", "Red Text:" + rgbIndex.R + Environment.NewLine + Environment.NewLine);
                    File.AppendAllText($"./output/imagep/RgbIndex_{file.PageName}.txt", "Green Text: " + rgbIndex.G + Environment.NewLine + Environment.NewLine);
                    File.AppendAllText($"./output/imagep/RgbIndex_{file.PageName}.txt", "Blue Text: " + rgbIndex.B + Environment.NewLine + Environment.NewLine);
                });
            }
        }
    }
}