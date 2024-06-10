using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiberPrimusAnalysisTool.Application.Commands.Image.ByteProcessing;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Utility.Message;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Winnow Pages
    /// </summary>
    public class BulkByteWinnowPages
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.INotification" />
        public class Command : INotification
        {
            /// <summary>
            /// The byte winnow command
            /// </summary>
            /// <param name="binaryOnlyMode"></param>
            /// <param name="pageSelection"></param>
            /// <param name="includeControlCharacters"></param>
            /// <param name="reverseBytes"></param>
            /// <param name="shiftSequence"></param>
            /// <param name="minBitOfInsignificance"></param>
            /// <param name="maxBitOfInsignificance"></param>
            /// <param name="discardRemainder"></param>
            public Command(bool binaryOnlyMode, string pageSelection, bool includeControlCharacters, bool reverseBytes,
                bool shiftSequence, int minBitOfInsignificance, int maxBitOfInsignificance, bool discardRemainder)
            {
                BinaryOnlyMode = binaryOnlyMode;
                PageSelection = pageSelection;
                IncludeControlCharacters = includeControlCharacters;
                ReverseBytes = reverseBytes;
                ShiftSequence = shiftSequence;
                MinBitOfInsignificance = minBitOfInsignificance;
                MaxBitOfInsignificance = maxBitOfInsignificance;
                DiscardRemainder = discardRemainder;
            }

            public string PageSelection { get; set; }

            public bool IncludeControlCharacters { get; set; }

            public bool ReverseBytes { get; set; }

            public bool ShiftSequence { get; set; }

            public int MinBitOfInsignificance { get; set; }

            public int MaxBitOfInsignificance { get; set; }

            public bool DiscardRemainder { get; set; }

            public bool BinaryOnlyMode { get; set; }
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
            /// The message bus
            /// </summary>
            private readonly IMessageBus _messageBus;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            /// <param name="messageBus">The message bus.</param>
            public Handler(IMediator mediator, IMessageBus messageBus)
            {
                _mediator = mediator;
                _messageBus = messageBus;
            }

            /// <summary>
            /// Handles a notification
            /// </summary>
            /// <param name="notification">The notification</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task Handle(Command notification, CancellationToken cancellationToken)
            {
                Assembly asm = Assembly.GetAssembly(typeof(CalculateSequence));
                List<Tuple<Type, string>> mathTypes = new List<Tuple<Type, string>>();
                var counter = 1;

                foreach (Type type in asm.GetTypes())
                {
                    if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" &&
                        type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                    {
                        var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault()
                            .GetValue(null);
                        mathTypes.Add(new Tuple<Type, string>(type, $"{name}"));
                        counter++;
                    }
                }

                var liberPage =
                    await _mediator.Send(new GetPageData.Query(notification.PageSelection, false, false));

                foreach (var name in mathTypes.Select(x => x.Item2))
                {
                    string seqtext = string.Empty;
                    var seq = await _mediator.Send(new CalculateSequence.Query(Convert.ToUInt64(liberPage.Bytes.Count), name));
                    var sequence = seq.Sequence;
                    seqtext = notification.ReverseBytes ? $"ReversedBytes-{seq.Name}" : $"{seq.Name}";

                    if (notification.ShiftSequence)
                    {
                        seqtext = "ShiftedSeq-" + seqtext;
                    }
                    
                    _messageBus.SendMessage($"Processing {seqtext}...", "BulkByteWinnowPages");

                    // Getting the pixels from the sequence
                    List<byte> tmpPixelList = new List<byte>();
                    List<byte> fileBytes = liberPage.Bytes;

                    if (notification.ReverseBytes)
                    {
                        fileBytes.Reverse();
                    }

                    foreach (var seqNumber in sequence)
                    {
                        try
                        {
                            if (notification.ShiftSequence && !seqtext.Contains("Natural"))
                            {
                                tmpPixelList.Add(fileBytes.ElementAt((int)seqNumber - 1));
                            }
                            else
                            {
                                tmpPixelList.Add(fileBytes.ElementAt((int)seqNumber));
                            }
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }

                    Tuple<LiberPage, List<byte>> pixelData =
                        new Tuple<LiberPage, List<byte>>(liberPage, tmpPixelList);

                    GC.Collect();

                    for (int p = 0; p <= 1; p++)
                    {
                        switch (p)
                        {
                            case 0:
                                if (!notification.BinaryOnlyMode)
                                {
                                    foreach (var asciiProcessing in new List<int>() { 7, 8, 9 })
                                    {
                                        for (var bitsOfSig = notification.MinBitOfInsignificance;
                                             bitsOfSig <= notification.MaxBitOfInsignificance;
                                             bitsOfSig++)
                                        {
                                            await _mediator.Publish(new ProcessBytesLSB.Command(pixelData, seqtext,
                                                notification.IncludeControlCharacters, asciiProcessing, bitsOfSig,
                                                notification.DiscardRemainder));
                                        }
                                    }
                                }

                                break;

                            case 1:
                                for (var bitsOfSig = notification.MinBitOfInsignificance;
                                     bitsOfSig <= notification.MaxBitOfInsignificance;
                                     bitsOfSig++)
                                {
                                    await _mediator.Publish(new ProcessBytesToBytes.Command(pixelData, seqtext,
                                        bitsOfSig, notification.DiscardRemainder));
                                }

                                break;

                            default:
                                break;
                        }
                    }
                }
                
                _messageBus.SendMessage($"Complete", "BulkByteWinnowPages:Complete");
            }
        }
    }
}