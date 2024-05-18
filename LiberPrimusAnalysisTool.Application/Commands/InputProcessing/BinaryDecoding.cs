using MediatR;
using System.Text;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class BinaryDecoding
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
            public string File { get; set; }
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
            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                StringBuilder sb = new StringBuilder();
                int byteLength = request.Encoding == "ASCII"? 7 : 8;
                List<byte> bytes = new List<byte>();
                string decoded = string.Empty;

                foreach (var character in request.Input)
                {
                    if (character == ' ')
                    {
                        continue;
                    }

                    if (character != '0' && character != '1')
                    {
                        return "Invalid binary string.";
                    }

                    sb.Append(character);
                    if (sb.Length == byteLength)
                    {
                        if (sb.Length == 7)
                        {
                            sb.Insert(0, '0');
                        }
                        bytes.Add(Convert.ToByte(sb.ToString(), 2));
                        sb.Clear();
                    }
                }
                
                if (sb.Length == byteLength)
                {
                    if (sb.Length == 7)
                    {
                        sb.Insert(0, '0');
                    }
                    bytes.Add(Convert.ToByte(sb.ToString(), 2));
                    sb.Clear();
                }
                
                try
                {
                    switch (request.Encoding)
                    {
                        case "ASCII":
                        case "ANSI":
                            decoded = Encoding.ASCII.GetString(bytes.ToArray());
                            break;
                        case "UTF8":
                            decoded = Encoding.UTF8.GetString(bytes.ToArray());
                            break;
                        case "UTF7":
                            decoded = Encoding.UTF7.GetString(bytes.ToArray());
                            break;
                        case "UTF32":
                            decoded = Encoding.UTF32.GetString(bytes.ToArray());
                            break;
                        case "Latin1":
                            decoded = Encoding.Latin1.GetString(bytes.ToArray());
                            break;
                        case "UNICODE":
                            decoded = Encoding.Unicode.GetString(bytes.ToArray());
                            break;
                        case "HEX":
                            decoded = BitConverter.ToString(bytes.ToArray()).Replace("-", "");
                            break;
                        case "FILE":
                            File.WriteAllBytes(request.File, bytes.ToArray());
                            decoded = await _mediator.Send(new DetectBinFile.Command { FileName = request.File });
                            break;
                        default:
                            decoded = Encoding.ASCII.GetString(bytes.ToArray());
                            break;
                    }
                }
                catch (Exception ex)
                {
                    decoded = ex.Message;
                }

                return decoded;
            }
        }
    }
}