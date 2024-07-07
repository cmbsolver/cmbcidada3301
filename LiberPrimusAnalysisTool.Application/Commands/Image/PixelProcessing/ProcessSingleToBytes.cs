using System;
using System.Collections.Generic;
using System.IO;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiberPrimusAnalysisTool.Application.Commands.Image.PixelProcessing
{
    /// <summary>
    /// ProcessToBytes
    /// </summary>
    public class ProcessSingleToBytes
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
            /// <param name="bitsOfSig">The bits of sig.</param>
            /// <param name="colorOrder">The color order.</param>
            /// <param name="discardRemainingBits">if set to <c>true</c> [discard remaining bits].</param>
            public Command(Tuple<LiberPage, List<Pixel>> pixelData, string method, int bitsOfSig, char colorOrder, bool discardRemainingBits)
            {
                PixelData = pixelData;
                Method = method;
                BitsOfSig = bitsOfSig;
                ColorOrder = colorOrder;
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
            /// Gets or sets the bits of sig.
            /// </summary>
            /// <value>
            /// The bits of sig.
            /// </value>
            public int BitsOfSig { get; set; }

            /// <summary>
            /// Gets or sets the color order.
            /// </summary>
            /// <value>
            /// The color order.
            /// </value>
            public char ColorOrder { get; set; }

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
                if (!Directory.Exists("./output/imagep"))
                {
                    Directory.CreateDirectory("./output/imagep");
                }
                
                List<char> bits = new List<char>();
                List<char> builderBits = new List<char>();
                bool skipRemainingBits = false;

                foreach (var pixel in request.PixelData.Item2)
                {
                    skipRemainingBits = false;
                    var rBits = Convert.ToString(pixel.R, 2).PadLeft(8, '0');
                    var gBits = Convert.ToString(pixel.G, 2).PadLeft(8, '0');
                    var bBits = Convert.ToString(pixel.B, 2).PadLeft(8, '0');

                    switch (request.ColorOrder)
                    {
                        case 'R':
                            var tmpRbits = rBits.Substring(8 - request.BitsOfSig, request.BitsOfSig);
                            foreach (var bit in tmpRbits)
                            {
                                if (builderBits.Count <= 8 && !skipRemainingBits)
                                {
                                    builderBits.Add(bit);
                                }

                                if (builderBits.Count == 8)
                                {
                                    if (request.DiscardRemainingBits && !skipRemainingBits)
                                    {
                                        skipRemainingBits = true;
                                        bits.AddRange(builderBits);
                                        builderBits.Clear();
                                    }
                                    else
                                    {
                                        skipRemainingBits = false;
                                        bits.AddRange(builderBits);
                                        builderBits.Clear();
                                    }
                                }
                            }
                            break;

                        case 'G':
                            var tmpGbits = gBits.Substring(8 - request.BitsOfSig, request.BitsOfSig);
                            foreach (var bit in tmpGbits)
                            {
                                if (builderBits.Count <= 8 && !skipRemainingBits)
                                {
                                    builderBits.Add(bit);
                                }

                                if (builderBits.Count == 8)
                                {
                                    if (request.DiscardRemainingBits && !skipRemainingBits)
                                    {
                                        skipRemainingBits = true;
                                        bits.AddRange(builderBits);
                                        builderBits.Clear();
                                    }
                                    else
                                    {
                                        skipRemainingBits = false;
                                        bits.AddRange(builderBits);
                                        builderBits.Clear();
                                    }
                                }
                            }
                            break;

                        case 'B':
                            var tmpBbits = bBits.Substring(8 - request.BitsOfSig, request.BitsOfSig);
                            foreach (var bit in tmpBbits)
                            {
                                if (builderBits.Count <= 8 && !skipRemainingBits)
                                {
                                    builderBits.Add(bit);
                                }

                                if (builderBits.Count == 8)
                                {
                                    if (request.DiscardRemainingBits && !skipRemainingBits)
                                    {
                                        skipRemainingBits = true;
                                        bits.AddRange(builderBits);
                                        builderBits.Clear();
                                    }
                                    else
                                    {
                                        skipRemainingBits = false;
                                        bits.AddRange(builderBits);
                                        builderBits.Clear();
                                    }
                                }
                            }
                            break;
                    }
                }

                List<byte> bytes = new List<byte>();
                StringBuilder ascii = new StringBuilder();
                foreach (var character in bits)
                {
                    ascii.Append(character);
                    if (ascii.Length >= 8)
                    {
                        bytes.Add(Convert.ToByte(ascii.ToString(), 2));
                        ascii.Clear();
                    }
                }

                File.WriteAllBytes($"./output/imagep/IMG-{request.PixelData.Item1.PageName}-LSBSingle-{request.Method}-{request.ColorOrder}-{request.BitsOfSig}.bin", bytes.ToArray());

                return Task.CompletedTask;
            }
        }
    }
}