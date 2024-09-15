using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Utility.Character;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing;

/// <summary>
/// Dictionary check text files
/// </summary>
public class DictionaryCheckTextFiles
{
    /// <summary>
    /// The command
    /// </summary>
    public class Command : INotification
    {
        /// <summary>
        /// Is GP strict
        /// </summary>
        public bool isGpStrict { get; set; }
        
        public int numOfLetters { get; set; }
        
        /// <summary>
        /// The summary
        /// </summary>
        /// <param name="isGpStrict"></param>
        /// <param name="numOfLetters"></param>
        public Command(bool isGpStrict, int numOfLetters)
        {
            this.isGpStrict = isGpStrict;
            this.numOfLetters = numOfLetters;
        }
    }
    
    /// <summary>
    /// The Handler
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
        /// The message bus
        /// </summary>
        private readonly IMessageBus _messageBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="Handler" /> class.
        /// </summary>
        /// <param name="characterRepo">The character repo.</param>
        /// <param name="mediator">The mediator.</param>
        public Handler(ICharacterRepo characterRepo, IMediator mediator, IMessageBus messageBus)
        {
            _characterRepo = characterRepo;
            _mediator = mediator;
            _messageBus = messageBus;
        }
        
        /// <summary>
        /// The handler
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        public async Task Handle(Command notification, CancellationToken cancellationToken)
        {
            HashSet<string> files = new HashSet<string>();
            HashSet<string> englishDictionary = new HashSet<string>();
            HashSet<Tuple<string, long>> wordCountSet = new HashSet<Tuple<string, long>>();
            
            _messageBus.SendMessage("Starting dictionary check", "DictionaryCheckTextFiles:Start");
            
            // Load the dictionary
            using (var file = File.OpenText("words.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (notification.isGpStrict)
                    {
                        line = line.ToUpper().Trim().Replace("QU", "CW").Replace("Q", "C")
                            .Replace("K", "C").Replace("V", "U").Replace("Z", "S");

                        if (line.Length >= notification.numOfLetters)
                        {
                            englishDictionary.Add(line);
                        }
                    }
                    else
                    {
                        if (line.Length >= notification.numOfLetters)
                        {
                            englishDictionary.Add(line.ToUpper().Trim());
                        }
                    }
                }

                file.Close();
                file.Dispose();
            }
            
            // Getting our files
            var processFileInfo = new FileInfo(Environment.ProcessPath);
            var pages = await _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/output"));
            foreach (var page in pages)
            {
                files.Add(page);
            }

            foreach (var file in files)
            {
                var wordCount = 0;
                var fileContent = File.ReadAllText(file);

                fileContent = fileContent.Replace("\r", "  ").Replace("\n", "  ");
                while (fileContent.Contains("  "))
                {
                    fileContent = fileContent.Replace("  ", " ");
                }

                foreach (var content in file.Split(" "))
                {
                    foreach (var word in englishDictionary)
                    {
                        if (content.ToUpper() == word.ToUpper())
                        {
                            wordCount++;
                        }
                    }
                }
                
                _messageBus.SendMessage($"Word count of {file}: {wordCount}", "DictionaryCheckTextFiles:Status");

                if (wordCount > 0)
                {
                    wordCountSet.Add(new Tuple<string, long>($"Word count of {file}: {wordCount}", wordCount));
                }
            }
            
            _messageBus.SendMessage(string.Empty, "DictionaryCheckTextFiles:Clear");

            wordCountSet = wordCountSet.OrderBy(x => x.Item2).Reverse().ToHashSet();
            foreach (var item in wordCountSet)
            {
                _messageBus.SendMessage(item.Item1, "DictionaryCheckTextFiles:File");
            }
        }
    }
}