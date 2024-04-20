using LiberPrimusAnalysisTool.Application.Commands.Directory;
using LiberPrimusAnalysisTool.Application.Commands.Image.ByteProcessing;
using LiberPrimusAnalysisTool.Application.Commands.Math;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Application.Queries.Selection;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using Spectre.Console;
using System.Reflection;

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
                    Console.Clear();
                    AnsiConsole.Write(new FigletText("Winnow By Bytes").Centered().Color(Color.Green));

                    var pageSelection = await _mediator.Send(new GetImageSelection.Query());

                    var includeControlCharacters = AnsiConsole.Confirm("Include control characters?", false);
                    var reverseBytes = AnsiConsole.Confirm("Reverse Bytes?", false);
                    var shiftSequence = AnsiConsole.Confirm("Shift sequence down by one?", false);
                    var minBitOfInsignificance = AnsiConsole.Ask<int>("Min bits of insignificance?", 1);
                    var maxBitOfInsignificance = AnsiConsole.Ask<int>("Max bits of insignificance?", 3);
                    var discardRemainder = AnsiConsole.Confirm("Discard remainder bits?", true);
                    var binaryOnlyMode = AnsiConsole.Confirm("Binary only mode?", false);

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
                            var seq = await _mediator.Send(new CalculateSequence.Query(false, false, liberPage.Bytes.Count, name));
                            var sequence = seq.Sequence;
                            seqtext = reverseBytes ? $"ReversedBytes-{seq.Name}" : $"{seq.Name}";

                            if (shiftSequence)
                            {
                                seqtext = "ShiftedSeq-" + seqtext;
                            }

                            // Getting the pixels from the sequence
                            AnsiConsole.WriteLine($"Getting bytes from sequence {seqtext}");

                            AnsiConsole.WriteLine($"Sequencing {liberPage.PageName}");
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

                            AnsiConsole.WriteLine($"Sequenced {liberPage.PageName}");

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

                    await _mediator.Publish(new FlushZeroOutputDirectory.Command());

                    bool moveFiles = AnsiConsole.Confirm("Move files to input directory?", false);
                    {
                        await _mediator.Publish(new MoveFilesToInput.Command());
                    }

                    returnToMenu = AnsiConsole.Confirm("Return to main menu?");
                }
            }
        }
    }
}