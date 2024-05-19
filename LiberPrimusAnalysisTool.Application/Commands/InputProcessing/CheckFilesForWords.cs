using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Entity;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Get Word From Ints
    /// </summary>
    public class CheckFilesForWords
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : INotification
        {
        }

        /// <summary>
        /// Handler
        /// </summary>
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
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                List<string> englishDictionary = new List<string>();

                var isGpStrict = true;

                var allfiles = new string[0]; //var allFiles = await _mediator.Send(new GetTextSelection.Query(false));

                using (var file = File.OpenText("words.txt"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        if (isGpStrict)
                        {
                            englishDictionary.Add(line.ToUpper().Replace("QU", "CW").Replace("Q", "C").Replace("K", "C")
                                .Replace("V", "U").Replace("Z", "S"));
                        }
                        else
                        {
                            englishDictionary.Add(line.ToUpper());
                        }
                    }

                    file.Close();
                    file.Dispose();
                }

                englishDictionary = englishDictionary.OrderBy(x => x.Length).ToList();

                foreach (var file in allfiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    var realDictionary = new List<string>();
                    realDictionary.AddRange(ScoreLine.GetDictionaryWords(file,
                        File.ReadAllLines(file).Select(x => x.ToUpper()).ToArray(), englishDictionary));

                    var scoredFile = new ScoreLine(fileInfo.Name, File.ReadAllLines(file), realDictionary);
                    if (scoredFile.Score > 0)
                    {
                        realDictionary = realDictionary.OrderBy(x => x.Length).Distinct().ToList();
                        // File.WriteAllLines($"output/word_dictionary_{fileInfo.Name}.txt", realDictionary);

                        File.AppendAllText($"output/word_readible_{fileInfo.Name}", fileInfo.Name);
                        File.AppendAllText($"output/word_readible_{fileInfo.Name}", Environment.NewLine);
                        File.AppendAllLines($"output/word_readible_{fileInfo.Name}", scoredFile.ReadableLines);
                        File.AppendAllText($"output/word_readible_{fileInfo.Name}", Environment.NewLine);
                        File.AppendAllText($"output/word_readible_{fileInfo.Name}", Environment.NewLine);
                    }
                }
            }
        }
    }
}