using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.InputProcessing
{
    /// <summary>
    /// Flush Output Directory
    /// </summary>
    public class DetectBinFile
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="IRequest" />
        public class Command : IRequest<string>
        {
            public Command(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; set; }
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
                return Task.FromResult(DetectBinaryFile(request.FileName));
            }

            /// <summary>
            /// Detects the binary files.
            /// </summary>
            /// <param name="file">The file.</param>
            private string DetectBinaryFile(string file)
            {
                string fileType = string.Empty;
                FileTypeInterrogator.IFileTypeInterrogator interrogator =
                    new FileTypeInterrogator.FileTypeInterrogator();

                byte[] fileBytes = File.ReadAllBytes(file);

                FileTypeInterrogator.FileTypeInfo fileTypeInfo = interrogator.DetectType(fileBytes);

                if (fileTypeInfo == null)
                {
                    fileType = $"Could not detect file type for {file}." + Environment.NewLine;
                }
                else
                {
                    fileType += $"File Name {file}" + Environment.NewLine;
                    fileType += $"Type = {fileTypeInfo.Name}" + Environment.NewLine;
                    fileType += $"Extension = {fileTypeInfo.FileType}" + Environment.NewLine;
                    fileType += $"Mime Type = {fileTypeInfo.MimeType}" + Environment.NewLine;
                }
                
                return fileType;
            }
        }
    }
}