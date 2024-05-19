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
                    var pageSelection = new string[0]; //await _mediator.Send(new GetImageSelection.Query());

                    var includeControlCharacters = false;
                    var reverseBytes = false;
                    var shiftSequence = false;
                    var minBitOfInsignificance = 1;
                    var maxBitOfInsignificance = 3;
                    var discardRemainder = true;
                    var binaryOnlyMode = false;

                    Assembly asm = Assembly.GetAssembly(typeof(CalculateSequence));
                    List<Tuple<Type, string>> mathTypes = new List<Tuple<Type, string>>();
                    var counter = 1;

                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.Namespace == "LiberPrimusAnalysisTool.Application.Queries.Math" && type.GetInterfaces().Any(x => x.Name.Contains("ISequence")))
                        {
                            var name = type.GetProperties().Where(p => p.Name == "Name").FirstOrDefault().GetValue(null);
                            mathTypes.Add(new Tuple<Type, string>(type, $"{name}"));
                            counter++;
                        }
                    }

                    ParallelOptions parallelOptions = new ParallelOptions();
                    parallelOptions.MaxDegreeOfParallelism = 64;

                    // Getting the page data.
                    Parallel.ForEach(pageSelection, parallelOptions, async selection =>
                    {
                        var liberPage = await _mediator.Send(new GetPageData.Query(selection, false, false));

                        foreach (var name in mathTypes.Select(x => x.Item2))
                        {
                            string seqtext = string.Empty;
                            var seq = await _mediator.Send(new CalculateSequence.Query(liberPage.Bytes.Count, name));
                            var sequence = seq.Sequence;
                            seqtext = reverseBytes ? $"ReversedBytes-{seq.Name}" : $"{seq.Name}";

                            if (shiftSequence)
                            {
                                seqtext = "ShiftedSeq-" + seqtext;
                            }

                            // Getting the pixels from the sequence
                            List<byte> tmpPixelList = new List<byte>();
                            List<byte> fileBytes = liberPage.Bytes;

                            if (reverseBytes)
                            {
                                fileBytes.Reverse();
                            }

                            foreach (var seqNumber in sequence)
                            {
                                try
                                {
                                    if (shiftSequence && !seqtext.Contains("Natural"))
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

                            Tuple<LiberPage, List<byte>> pixelData = new Tuple<LiberPage, List<byte>>(liberPage, tmpPixelList);

                            GC.Collect();

                            for (int p = 0; p <= 1; p++)
                            {
                                switch (p)
                                {
                                    case 0:
                                        if (!binaryOnlyMode)
                                        {
                                            foreach (var asciiProcessing in new List<int>() { 7, 8, 9 })
                                            {
                                                for (var bitsOfSig = minBitOfInsignificance; bitsOfSig <= maxBitOfInsignificance; bitsOfSig++)
                                                {
                                                    await _mediator.Publish(new ProcessBytesLSB.Command(pixelData, seqtext, includeControlCharacters, asciiProcessing, bitsOfSig, discardRemainder));
                                                }
                                            }
                                        }
                                        break;

                                    case 1:
                                        for (var bitsOfSig = minBitOfInsignificance; bitsOfSig <= maxBitOfInsignificance; bitsOfSig++)
                                        {
                                            await _mediator.Publish(new ProcessBytesToBytes.Command(pixelData, seqtext, bitsOfSig, discardRemainder));
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