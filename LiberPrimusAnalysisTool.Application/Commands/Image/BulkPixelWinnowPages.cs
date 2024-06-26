﻿using System;
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
                bool returnToMenu = false;

                while (!returnToMenu)
                {
                    var pageSelection = new string[0]; //var pageSelection = await _mediator.Send(new GetImageSelection.Query());

                    var includeControlCharacters = false;
                    var reversePixels = false;
                    var shiftSequence = false;
                    var minBitOfInsignificance = 1;
                    var maxBitOfInsignificance = 3;
                    var discardRemainder = true;
                    var binaryOnlyMode = false;

                    ParallelOptions parallelOptions = new ParallelOptions();
                    parallelOptions.MaxDegreeOfParallelism = 4;

                    List<LiberPage> pages = new List<LiberPage>();
                    Parallel.ForEach(pageSelection, parallelOptions, async selection =>
                    {
                        var liberPage = await _mediator.Send(new GetPageData.Query(selection, true, reversePixels));
                        pages.Add(liberPage);
                    });

                    Assembly asm = Assembly.GetAssembly(typeof(CalculateSequence));
                    List<Tuple<Type, string>> mathTypes = new List<Tuple<Type, string>>();
                    var counter = 1;

                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" && type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                        {
                            var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault().GetValue(null);
                            mathTypes.Add(new Tuple<Type, string>(type, $"{counter}: {name}"));
                            counter++;
                        }
                    }

                    Parallel.ForEach(pages, parallelOptions, async liberPage =>
                    {
                        foreach (var name in mathTypes.Select(x => x.Item2))
                        {
                            string seqtext = string.Empty;
                            var seq = await _mediator.Send(new CalculateSequence.Query(Convert.ToUInt64(liberPage.PixelCount), name));
                            var sequence = seq.Sequence;
                            seqtext = reversePixels ? $"ReversedPixels-{seq.Name}" : $"{seq.Name}";

                            if (shiftSequence)
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
                                    if (shiftSequence && !seqtext.Contains("Natural"))
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

                            for (int p = 0; p <= 4; p++)
                            {
                                switch (p)
                                {
                                    case 1:
                                        if (!binaryOnlyMode)
                                            await _mediator.Publish(new ProcessRGB.Command(pixelData, seqtext, includeControlCharacters, discardRemainder));
                                        break;

                                    case 2:
                                        if (!binaryOnlyMode)
                                        {
                                            foreach (var asciiProcessing in new List<int>() { 7, 8, 9 })
                                            {
                                                for (var bitsOfSig = minBitOfInsignificance; bitsOfSig <= maxBitOfInsignificance; bitsOfSig++)
                                                {
                                                    foreach (var colorOrder in new List<string>() { "RGB", "RBG", "GBR", "GRB", "BRG", "BGR" })
                                                    {
                                                        await _mediator.Publish(new ProcessLSB.Command(pixelData, seqtext, includeControlCharacters, asciiProcessing, bitsOfSig, colorOrder, discardRemainder));
                                                    }
                                                }
                                            }
                                        }
                                        break;

                                    case 3:
                                        for (var bitsOfSig = minBitOfInsignificance; bitsOfSig <= maxBitOfInsignificance; bitsOfSig++)
                                        {
                                            foreach (var colorOrder in new List<string>() { "RGB", "RBG", "GBR", "GRB", "BRG", "BGR" })
                                            {
                                                await _mediator.Publish(new ProcessToBytes.Command(pixelData, seqtext, bitsOfSig, colorOrder, discardRemainder));
                                            }
                                        }
                                        break;

                                    case 4:
                                        if (!binaryOnlyMode)
                                        {
                                            foreach (var asciiProcessing in new List<int>() { 7, 8, 9 })
                                            {
                                                for (var bitsOfSig = minBitOfInsignificance; bitsOfSig <= maxBitOfInsignificance; bitsOfSig++)
                                                {
                                                    foreach (var colorOrder in new List<char>() { 'R', 'G', 'B' })
                                                    {
                                                        await _mediator.Publish(new ProcessSingleLSB.Command(pixelData, seqtext, includeControlCharacters, asciiProcessing, bitsOfSig, colorOrder, discardRemainder));
                                                    }
                                                }
                                            }
                                        }
                                        break;

                                    case 5:
                                        for (var bitsOfSig = minBitOfInsignificance; bitsOfSig <= maxBitOfInsignificance; bitsOfSig++)
                                        {
                                            foreach (var colorOrder in new List<char>() { 'R', 'G', 'B' })
                                            {
                                                await _mediator.Publish(new ProcessSingleToBytes.Command(pixelData, seqtext, bitsOfSig, colorOrder, discardRemainder));
                                            }
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    });

                    returnToMenu = true;
                }
            }
        }
    }
}