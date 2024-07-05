using System;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Utility.Character;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class CalculateSectionSums
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : IRequest<string>
        {
            
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Command, string>
        {
            /// <summary>
            /// The mediator
            /// </summary>
            private readonly IMediator _mediator;
            
            /// <summary>
            /// The character repo.
            /// </summary>
            private readonly ICharacterRepo _characterRepo;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            public Handler(IMediator mediator, ICharacterRepo characterRepo)
            {
                _mediator = mediator;
                _characterRepo = characterRepo;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                StringBuilder sb = new StringBuilder();
                var processFileInfo = new FileInfo(Environment.ProcessPath);
                var pages = await _mediator.Send(new GetTextPages.Command($"{processFileInfo.Directory}/input/text"));

                foreach (var page in pages)
                {
                    var fileInfo = new FileInfo(page);
                    sb.Append($"{fileInfo.Name}: ");
                    string content = File.ReadAllText(page);
                    int sum = 0;
                    
                    foreach (var character in content)
                    {
                        sum += _characterRepo.GetValueFromRune(character.ToString());
                    }
                    
                    sb.Append($"{sum}");
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }
    }
}