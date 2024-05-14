using MediatR;
using System.Text;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class Base64Decoding
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : IRequest<string>
        {
            /// <summary>
            /// The input string
            /// </summary>
            public string Input { get; set; }
            
            /// <summary>
            /// What to decode it as.
            /// </summary>
            public string Encoding { get; set; }

            /// <summary>
            ///  The other encoding.
            /// </summary>
            public string OtherEncoding { get; set; }
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
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// <summary>
            /// Handles the specified request.
            /// </summary>
            /// <param name="request">The request.</param>
            /// <param name="cancellationToken">The cancellation token.</param>
            public Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                string decoded = string.Empty;
                
                try
                {
                    var decodedBytes = Convert.FromBase64String(request.Input);
                    switch (request.Encoding)
                    {
                        case "ASCII":
                            decoded = Encoding.ASCII.GetString(decodedBytes);
                            break;
                        case "UTF8":
                            decoded = Encoding.UTF8.GetString(decodedBytes);
                            break;
                        case "UTF7":
                            decoded = Encoding.UTF7.GetString(decodedBytes);
                            break;
                        case "UTF32":
                            decoded = Encoding.UTF32.GetString(decodedBytes);
                            break;
                        case "Latin1":
                            decoded = Encoding.Latin1.GetString(decodedBytes);
                            break;
                        case "UNICODE":
                            decoded = Encoding.Unicode.GetString(decodedBytes);
                            break;
                        case "HEX":
                            decoded = BitConverter.ToString(decodedBytes).Replace("-", "");
                            break;
                        case "OTHER":
                            decoded = Encoding.GetEncoding(request.Encoding).GetString(decodedBytes);
                            break;
                        default:
                            decoded = Encoding.ASCII.GetString(decodedBytes);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    decoded = ex.Message;
                }

                return Task.FromResult(decoded);
            }
        }
    }
}