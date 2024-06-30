using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Color Report
    /// </summary>
    public class ColorReport
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.INotification" />
        public class Command : IRequest<string>
        {
            public Command(string file)
            {
                File = file;
            }

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
            /// Handles a notification
            /// </summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task<string> Handle(Command request, CancellationToken cancellationToken)
            {
                StringBuilder report = new StringBuilder();
                
                // Getting the pages we want
                var page = await _mediator.Send(new GetPageData.Query(request.File, true, false));

                // Copy the images to a new directory and set colors.
                report.AppendLine($"Color Report");
                report.AppendLine($"---------------------------------");
                report.AppendLine($"Page: {page.PageName}");
                report.AppendLine($"Total Colors: {page.TotalColors}");
                report.AppendLine($"Height: {page.Height}");
                report.AppendLine($"Width: {page.Width}");
                report.AppendLine($"Total Pixels: {page.PixelCount}");
                report.AppendLine(string.Empty);
                report.AppendLine($"Colors");
                report.AppendLine($"---------------------------------");
                var colors = await _mediator.Send(new GetPageColors.Query(page.FileName));

                foreach (var color in colors)
                {
                    report.AppendLine(color.LiberColorHex);
                }

                return report.ToString();
            }
        }
    }
}