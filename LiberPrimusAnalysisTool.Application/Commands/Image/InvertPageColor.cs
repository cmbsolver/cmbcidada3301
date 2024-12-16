using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using LiberPrimusAnalysisTool.Application.Queries;
using LiberPrimusAnalysisTool.Application.Queries.Page;
using LiberPrimusAnalysisTool.Entity.Image;
using LiberPrimusAnalysisTool.Utility.Message;
using MediatR;

namespace LiberPrimusAnalysisTool.Application.Commands.Image
{
    /// <summary>
    /// Color Isolation
    /// </summary>
    public class InvertPageColor
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.INotification" />
        public class Command : INotification
        {
            /// <summary>
            /// Command constructor
            /// </summary>
            /// <param name="liberPage"></param>
            public Command(LiberPage liberPage)
            {
                LiberPage = liberPage;
            }

            /// <summary>
            /// The LiberPage
            /// </summary>
            public LiberPage LiberPage { get; set; }
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
            /// The message bus
            /// </summary>
            private readonly IMessageBus _messageBus;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class.
            /// </summary>
            /// <param name="mediator">The mediator.</param>
            /// <param name="messageBus">The message bus.</param>
            public Handler(IMediator mediator, IMessageBus messageBus)
            {
                _mediator = mediator;
                _messageBus = messageBus;
            }

            /// <summary>
            /// Handles a notification
            /// </summary>
            /// <param name="notification">The notification</param>
            /// <param name="cancellationToken">Cancellation token</param>
            public async Task Handle(Command notification, CancellationToken cancellationToken)
            {
                _messageBus.SendMessage($"Inverting the colors for {notification.LiberPage.PageName}", "InvertColors");
                _messageBus.SendMessage("Creating the output directory.", "InvertColors");
                var fileInfo = new FileInfo(notification.LiberPage.FileName);
                if (Directory.Exists(fileInfo.Directory.FullName.Replace("input", "output")) == false)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName.Replace("input", "output"));
                }
                
                _messageBus.SendMessage("Copy the file to the new place.", "InvertColors");
                var outFileName = notification.LiberPage.FileName.Replace("input", "output");
                File.Copy(notification.LiberPage.FileName, outFileName, true);
                
                _messageBus.SendMessage("Reading the bytes", "InvertColors");
                byte[] imgBytes = File.ReadAllBytes(outFileName);
                using (MagickImage origImage = new MagickImage(imgBytes))
                {
                    _messageBus.SendMessage("Inverting the colors", "InvertColors");
                    origImage.Negate();
                    origImage.Write(outFileName);
                    _messageBus.SendMessage("Done.", "InvertColors");
                }
            }
        }
    }
}