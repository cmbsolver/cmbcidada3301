using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Image.PixelProcessing
{
    /// <summary>
    /// ProcessRGB
    /// </summary>
    public class ProcessRGB
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : INotification
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Command" /> class.
            /// </summary>
            /// <param name="pixelData">The pixel data.</param>
            /// <param name="method">The method.</param>
            /// <param name="includeControlCharacters">if set to <c>true</c> [include control characters].</param>
            /// <param name="discardRemainingBits">if set to <c>true</c> [discard remaining bits].</param>
            public Command(Tuple<LiberPage, List<Pixel>> pixelData, string method, bool includeControlCharacters, bool discardRemainingBits)
            {
                PixelData = pixelData;
                Method = method;
                IncludeControlCharacters = includeControlCharacters;
                DiscardRemainingBits = discardRemainingBits;
            }

            /// <summary>
            /// Gets or sets the pixel data.
            /// </summary>
            /// <value>
            /// The pixel data.
            /// </value>
            public Tuple<LiberPage, List<Pixel>> PixelData { get; set; }

            /// <summary>
            /// Gets or sets the method.
            /// </summary>
            /// <value>
            /// The method.
            /// </value>
            public string Method { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [include control characters].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [include control characters]; otherwise, <c>false</c>.
            /// </value>
            public bool IncludeControlCharacters { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [discard remaining bits].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [discard remaining bits]; otherwise, <c>false</c>.
            /// </value>
            public bool DiscardRemainingBits { get; set; }
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
            public Task Handle(Command request, CancellationToken cancellationToken)
            {
                var rgbIndex = new RgbCharacters(request.PixelData.Item1.PageName);

                foreach (var pixel in request.PixelData.Item2)
                {
                    rgbIndex.AddR(_characterRepo.GetASCIICharFromDec(pixel.R, request.IncludeControlCharacters));
                    rgbIndex.AddG(_characterRepo.GetASCIICharFromDec(pixel.G, request.IncludeControlCharacters));
                    rgbIndex.AddB(_characterRepo.GetASCIICharFromDec(pixel.B, request.IncludeControlCharacters));
                }

                File.AppendAllText($"./output/imagep/IMG-{request.PixelData.Item1.PageName}-{request.Method}-RGB-red.txt", rgbIndex.R);
                File.AppendAllText($"./output/imagep/IMG-{request.PixelData.Item1.PageName}-{request.Method}-RGB-green.txt", rgbIndex.G);
                File.AppendAllText($"./output/imagep/IMG-{request.PixelData.Item1.PageName}-{request.Method}-RGB-blue.txt", rgbIndex.B);

                return Task.CompletedTask;
            }
        }
    }
}