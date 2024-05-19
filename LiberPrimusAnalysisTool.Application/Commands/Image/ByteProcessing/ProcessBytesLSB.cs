using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiberPrimusAnalysisTool.Application.Commands.Image.ByteProcessing
{
    /// <summary>
    /// ProcessBytesLSB
    /// </summary>
    public class ProcessBytesLSB
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
            /// <param name="byteData">The byte data.</param>
            /// <param name="method">The method.</param>
            /// <param name="includeControlCharacters">if set to <c>true</c> [include control characters].</param>
            /// <param name="asciiProcessing">The ASCII processing.</param>
            /// <param name="bitsOfSig">The bits of sig.</param>
            public Command(Tuple<LiberPage, List<byte>> byteData, string method, bool includeControlCharacters, int asciiProcessing, int bitsOfSig, bool discardRemainingBits)
            {
                ByteData = byteData;
                Method = method;
                IncludeControlCharacters = includeControlCharacters;
                AsciiProcessing = asciiProcessing;
                BitsOfSig = bitsOfSig;
                DiscardRemainingBits = discardRemainingBits;
            }

            /// <summary>
            /// Gets or sets the pixel data.
            /// </summary>
            /// <value>
            /// The pixel data.
            /// </value>
            public Tuple<LiberPage, List<byte>> ByteData { get; set; }

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
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            /// <param name="characterRepo">The character repo.</param>
            public Handler(ICharacterRepo characterRepo)
            {
                _characterRepo = characterRepo;
            }

            /// <summary>
            /// Handles a request
            /// </summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public Task Handle(Command request, CancellationToken cancellationToken)
            {
                List<char> bits = new List<char>();
                List<char> builderBits = new List<char>();
                bool skipRemainingBits = false;
                int bitBreak = request.AsciiProcessing < 9 ? request.AsciiProcessing : 8;

                foreach (var bytedata in request.ByteData.Item2)
                {
                    skipRemainingBits = false;
                    var bBits = Convert.ToString(bytedata, 2).PadLeft(8, '0');
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
                }

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

                charBinList = charBinList.Where(x => x.Length == bitBreak).ToList();

                using (var file = File.CreateText($"./output/bytep/BYTE-{request.ByteData.Item1.PageName}-LSB-{request.Method}-{request.AsciiProcessing}-{request.BitsOfSig}.txt"))
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