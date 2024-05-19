using System;
using System.IO;
using LiberPrimusAnalysisTool.Utility.Character;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// ReverseBytes - Reverses the bytes of an image and compares the bytes
    /// </summary>
    public class ReverseBytes
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
            /// Handles a request
            /// </summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var files = new string[0]; //var files = await _mediator.Send(new GetImageSelection.Query());

                foreach (var file in files)
                {
                    var byteArray = File.ReadAllBytes(file);
                    var reversedByteArray = byteArray.Reverse().ToArray();

                    StringBuilder reverseBuilder = new StringBuilder();

                    File.WriteAllBytes($"./output/{Path.GetFileName(file).Split(".")[0]}-reversed.jpg", reversedByteArray);

                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        if (byteArray[i] != reversedByteArray[i])
                        {
                            var reversedBin = Convert.ToString(Convert.ToInt32(reversedByteArray[i].ToString()), 2).PadLeft(7, '0');
                            var character = _characterRepo.GetASCIICharFromBin(reversedBin, false);
                            reverseBuilder.Append(character);
                        }
                    }

                    File.AppendAllLines($"./output/{Path.GetFileName(file).Split(".")[0]}-reversed.txt", new string[] { reverseBuilder.ToString() });

                    Console.ReadLine();
                }
            }
        }
    }
}