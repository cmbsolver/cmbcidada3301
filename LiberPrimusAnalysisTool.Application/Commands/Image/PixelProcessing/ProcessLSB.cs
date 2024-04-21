using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using Spectre.Console;
using System.Text;

namespace LiberPrimusAnalysisTool.Application.Commands.Image.PixelProcessing
{
    /// <summary>
    /// ProcessLSB
    /// </summary>
    public class ProcessLSB
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
            /// <param name="asciiProcessing">The ASCII processing.</param>
            /// <param name="bitsOfSig">The bits of sig.</param>
            /// <param name="colorOrder">The color order.</param>
            /// <param name="discardRemainingBits">if set to <c>true</c> [discard remaining bits].</param>
            public Command(Tuple<LiberPage, List<Pixel>> pixelData, string method, bool includeControlCharacters, int asciiProcessing, int bitsOfSig, string colorOrder, bool discardRemainingBits)
            {
                PixelData = pixelData;
                Method = method;
                IncludeControlCharacters = includeControlCharacters;
                AsciiProcessing = asciiProcessing;
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
            /// Gets or sets a value indicating whether [include control characters].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [include control characters]; otherwise, <c>false</c>.
            /// </value>
            public bool IncludeControlCharacters { get; set; }

            /// <summary>
            /// Gets or sets the ASCII processing.
            /// </summary>
            /// <value>
            /// The ASCII processing.
            /// </value>
            public int AsciiProcessing { get; set; }

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
            public string ColorOrder { get; set; }

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
        /// <seealso cref="IRequestHandler&lt;LiberPrimusAnalysisTool.Analyzers.ColorReport.Command&gt;" />
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
                AnsiConsole.WriteLine($"ProcessLSB-{request.Method}: Getting bits from {request.PixelData.Item1.PageName}");
                List<char> bits = new List<char>();
                List<char> builderBits = new List<char>();
                bool skipRemainingBits = false;
                int bitBreak = request.AsciiProcessing < 9 ? request.AsciiProcessing : 8;

                char[] orderProcessing = new char[3] { request.ColorOrder[0], request.ColorOrder[1], request.ColorOrder[2] };

                foreach (var pixel in request.PixelData.Item2)
                {
                    skipRemainingBits = false;
                    var rBits = Convert.ToString(pixel.R, 2).PadLeft(8, '0');
                    var gBits = Convert.ToString(pixel.G, 2).PadLeft(8, '0');
                    var bBits = Convert.ToString(pixel.B, 2).PadLeft(8, '0');

                    foreach (var order in orderProcessing)
                    {
                        switch (order)
                        {
                            case 'R':
                                var tmpRbits = rBits.Substring(8 - request.BitsOfSig, request.BitsOfSig);
                                foreach (var bit in tmpRbits)
                                {
                                    if (builderBits.Count <= bitBreak && !skipRemainingBits)
                                    {
                                        builderBits.Add(bit);
                                    }

                                    if (builderBits.Count == bitBreak)
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
                                    if (builderBits.Count <= bitBreak && !skipRemainingBits)
                                    {
                                        builderBits.Add(bit);
                                    }

                                    if (builderBits.Count == bitBreak)
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
                                    if (builderBits.Count <= bitBreak && !skipRemainingBits)
                                    {
                                        builderBits.Add(bit);
                                    }

                                    if (builderBits.Count == bitBreak)
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
                }

                AnsiConsole.WriteLine($"ProcessLSB-{request.Method}: Building bytes for bits for {request.PixelData.Item1.PageName}");
                List<string> charBinList = new List<string>();
                StringBuilder ascii = new StringBuilder();
                foreach (var character in bits)
                {
                    ascii.Append(character);
                    if (ascii.Length >= bitBreak)
                    {
                        charBinList.Add(ascii.ToString());
                        ascii.Clear();
                    }
                }

                AnsiConsole.WriteLine($"ProcessLSB-{request.Method}: Filtering out nibbles for {request.PixelData.Item1.PageName}");
                charBinList = charBinList.Where(x => x.Length == bitBreak).ToList();

                AnsiConsole.WriteLine($"ProcessLSB-{request.Method}: Building character string for {request.PixelData.Item1.PageName}");
                AnsiConsole.WriteLine($"ProcessLSB-{request.Method}: Outputting file for {request.PixelData.Item1.PageName}");
                using (var file = File.CreateText($"./output/imagep/IMG-{request.PixelData.Item1.PageName}-LSB-{request.Method}-{request.ColorOrder}-{request.AsciiProcessing}-{request.BitsOfSig}.txt"))
                {
                    foreach (var charBin in charBinList)
                    {
                        switch (request.AsciiProcessing)
                        {
                            case 7:
                                file.Write(_characterRepo.GetASCIICharFromBin(charBin, request.IncludeControlCharacters));
                                break;

                            case 8:
                                file.Write(_characterRepo.GetANSICharFromBin(charBin, request.IncludeControlCharacters));
                                break;

                            case 9:
                                try
                                {
                                    file.Write(_characterRepo.GetRuneFromValue(Convert.ToInt32(charBin, 2)));
                                }
                                catch (Exception e)
                                {
                                    file.Write(string.Empty);
                                    AnsiConsole.WriteLine($"Error: {e.Message}");
                                }
                                break;

                            default:
                                file.Write(string.Empty);
                                break;
                        }
                    }

                    file.Flush();
                    file.Close();
                    file.Dispose();
                }

                return Task.CompletedTask;
            }
        }
    }
}