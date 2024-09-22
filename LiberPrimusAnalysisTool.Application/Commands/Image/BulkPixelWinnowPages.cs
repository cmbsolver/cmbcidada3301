using System;
using System.Collections.Generic;
using System.Linq;
using LiberPrimusAnalysisTool.Application.Commands.Image.PixelProcessing;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Winnow Pages
    /// </summary>
    public class BulkPixelWinnowPages
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.INotification" />
        public class Command : INotification
        {
            public Command(
                string pageSelection, 
                bool includeControlCharacters, 
                bool reverseBytes, 
                bool shiftSequence, 
                int minBitOfInsignificance, 
                int maxBitOfInsignificance, 
                bool discardRemainder, 
                bool binaryOnlyMode)
            {
                PageSelection = pageSelection;
                IncludeControlCharacters = includeControlCharacters;
                ReverseBytes = reverseBytes;
                ShiftSequence = shiftSequence;
                MinBitOfInsignificance = minBitOfInsignificance;
                MaxBitOfInsignificance = maxBitOfInsignificance;
                DiscardRemainder = discardRemainder;
                BinaryOnlyMode = binaryOnlyMode;
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
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handles a notification
            /// </summary>
            /// <param name="notification">The notification</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task Handle(Command notification, CancellationToken cancellationToken)
            {
                var pageSelection = notification.PageSelection;

                var liberPage = await _mediator.Send(new GetPageData.Query(pageSelection, true, notification.ReverseBytes));

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

                foreach (var name in mathTypes.Select(x => x.Item2))
                {
                    string seqtext = string.Empty;
                    var seq = await _mediator.Send(
                        new CalculateSequence.Query(Convert.ToUInt64(liberPage.PixelCount), name, false));
                    var sequence = seq.Sequence;
                    seqtext = notification.ReverseBytes ? $"ReversedPixels-{seq.Name}" : $"{seq.Name}";

                    if (notification.ShiftSequence)
                    {
                        seqtext = "ShiftedSeq-" + seqtext;
                    }

                    // Getting the pixels from the sequence
                    Tuple<LiberPage, List<Entity.Pixel>> pixelData;

                    List<Entity.Pixel> tmpPixelList = new List<Entity.Pixel>();
                    foreach (var seqNumer in sequence)
                    {
                        try
                        {
                            if (notification.ShiftSequence && !seqtext.Contains("Natural"))
                            {
                                tmpPixelList.Add(liberPage.Pixels.ElementAt((int)seqNumer - 1));
                            }
                            else
                            {
                                tmpPixelList.Add(liberPage.Pixels.ElementAt((int)seqNumer));
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }

                    pixelData = new Tuple<LiberPage, List<Entity.Pixel>>(liberPage, tmpPixelList);

                    GC.Collect();

                    for (int p = 0; p <= 5; p++)
                    {
                        switch (p)
                        {
                            case 1:
                                if (!notification.BinaryOnlyMode)
                                    await _mediator.Publish(new ProcessRGB.Command(pixelData, seqtext,
                                        notification.IncludeControlCharacters, notification.DiscardRemainder));
                                break;

                            case 2:
                                if (!notification.BinaryOnlyMode)
                                {
                                    foreach (var asciiProcessing in new List<int>() { 7, 8, 9 })
                                    {
                                        for (var bitsOfSig = notification.MinBitOfInsignificance;
                                             bitsOfSig <= notification.MaxBitOfInsignificance;
                                             bitsOfSig++)
                                        {
                                            foreach (var colorOrder in new List<string>()
                                                         { "RGB", "RBG", "GBR", "GRB", "BRG", "BGR" })
                                            {
                                                await _mediator.Publish(new ProcessLSB.Command(pixelData, seqtext,
                                                    notification.IncludeControlCharacters, asciiProcessing, bitsOfSig,
                                                    colorOrder, notification.DiscardRemainder));
                                            }
                                        }
                                    }
                                }

                                break;

                            case 3:
                                for (var bitsOfSig = notification.MinBitOfInsignificance;
                                     bitsOfSig <= notification.MaxBitOfInsignificance;
                                     bitsOfSig++)
                                {
                                    foreach (var colorOrder in new List<string>()
                                                 { "RGB", "RBG", "GBR", "GRB", "BRG", "BGR" })
                                    {
                                        await _mediator.Publish(new ProcessToBytes.Command(pixelData, seqtext,
                                            bitsOfSig, colorOrder, notification.DiscardRemainder));
                                    }
                                }

                                break;

                            case 4:
                                if (!notification.BinaryOnlyMode)
                                {
                                    foreach (var asciiProcessing in new List<int>() { 7, 8, 9 })
                                    {
                                        for (var bitsOfSig = notification.MinBitOfInsignificance;
                                             bitsOfSig <= notification.MaxBitOfInsignificance;
                                             bitsOfSig++)
                                        {
                                            foreach (var colorOrder in new List<char>() { 'R', 'G', 'B' })
                                            {
                                                await _mediator.Publish(new ProcessSingleLSB.Command(pixelData,
                                                    seqtext, notification.IncludeControlCharacters, asciiProcessing, bitsOfSig,
                                                    colorOrder, notification.DiscardRemainder));
                                            }
                                        }
                                    }
                                }

                                break;

                            case 5:
                                for (var bitsOfSig = notification.MinBitOfInsignificance;
                                     bitsOfSig <= notification.MaxBitOfInsignificance;
                                     bitsOfSig++)
                                {
                                    foreach (var colorOrder in new List<char>() { 'R', 'G', 'B' })
                                    {
                                        await _mediator.Publish(new ProcessSingleToBytes.Command(pixelData, seqtext,
                                            bitsOfSig, colorOrder, notification.DiscardRemainder));
                                    }
                                }

                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}