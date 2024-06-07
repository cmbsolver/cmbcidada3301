using System;
using System.Collections.Generic;
using System.IO;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Utility.Message;

namespace LiberPrimusAnalysisTool.Application.Commands.Image.ByteProcessing
{
    /// <summary>
    /// ProcessBytesToBytes
    /// </summary>
    public class ProcessBytesToBytes
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
            /// <param name="bitsOfSig">The bits of sig.</param>
            public Command(Tuple<LiberPage, List<byte>> byteData, string method, int bitsOfSig, bool discardRemainingBits)
            {
                ByteData = byteData;
                Method = method;
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
        /// Handler
        /// </summary>
        public class Handler : INotificationHandler<Command>
        {
            /// <summary>
            /// The message bus
            /// </summary>
            private readonly IMessageBus _messageBus;
            
            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            public Handler(IMessageBus messageBus)
            {
                _messageBus = messageBus;
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
                foreach (var bytedata in request.ByteData.Item2)
                {
                    skipRemainingBits = false;
                    var bBits = Convert.ToString(bytedata, 2).PadLeft(8, '0');
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
                
                if (!Directory.Exists("./output/bytep"))
                {
                    Directory.CreateDirectory("./output/bytep");
                }

                File.WriteAllBytes($"./output/bytep/BYTE-{request.ByteData.Item1.PageName}-LSB-{request.Method}-{request.BitsOfSig}.bin", bytes.ToArray());

                return Task.CompletedTask;
            }
        }
    }
}