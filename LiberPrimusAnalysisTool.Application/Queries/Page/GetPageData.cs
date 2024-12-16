using System;
using LiberPrimusAnalysisTool.Entity;
using MediatR;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiberPrimusAnalysisTool.Entity.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LiberPrimusAnalysisTool.Application.Queries
{
    /// <summary>
    /// Indexes the liber primus pages into the database.
    /// </summary>
    public class GetPageData
    {
        /// <summary>
        /// Command
        /// </summary>
        /// <seealso cref="MediatR.IRequest" />
        public class Query : MediatR.IRequest<LiberPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Query" /> class.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            /// <param name="includePixels">if set to <c>true</c> [include pixels].</param>
            /// <param name="invertPixels">if set to <c>true</c> [invert pixels].</param>
            public Query(string fileName, bool includePixels, bool invertPixels)
            {
                FileName = fileName;
                IncludePixels = includePixels;
                InvertPixels = invertPixels;
            }

            /// <summary>
            /// Gets or sets the page identifier.
            /// </summary>
            /// <value>
            /// The page identifier.
            /// </value>
            public string FileName { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [include pixels].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [include pixels]; otherwise, <c>false</c>.
            /// </value>
            public bool IncludePixels { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [invert pixels].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [invert pixels]; otherwise, <c>false</c>.
            /// </value>
            public bool InvertPixels { get; set; }
        }

        /// <summary>
        /// Handler
        /// </summary>
        public class Handler : IRequestHandler<Query, LiberPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Handler" /> class.
            /// </summary>
            public Handler()
            {
            }

            /// <summary>
            /// Handles a request
            /// </summary>
            /// <param name="request">The request</param>
            /// <param name="cancellationToken">Cancellation token</param>
            /// <returns>
            /// Response from the request
            /// </returns>
            public Task<LiberPage> Handle(Query request, CancellationToken cancellationToken)
            {
                LiberPage page;

                using (Image<Rgba32> image = Image.Load<Rgba32>(request.FileName))
                {
                    var metadata = image.Metadata.GetJpegMetadata();

                    page = new LiberPage
                    {
                        FileName = request.FileName,
                        PageName = Path.GetFileName(request.FileName).ToUpper().Split(".")[0],
                        TotalColors = 256, //Revisit
                        Height = image.Height,
                        Width = image.Width,
                        PixelCount = image.Height * image.Width,
                        PixelBlocks = new List<PixelBlock>()
                    };

                    Rgba32[] pixelArray = new Rgba32[image.Width * image.Height];
                    image.CopyPixelDataTo(pixelArray);
                    page.Pixels = pixelArray.Select(x => new Pixel(
                        x.R,
                        x.G,
                        x.B,
                        x.ToHex(),
                        page.PageName)).ToList();

                    if (request.InvertPixels)
                    {
                        page.Pixels.Reverse();
                    }

                    page.TotalColors = page.Pixels.Select(x => x.Hex).Distinct().Count();

                    // // Create 8x8 blocks
                    // int blockSize = 8;
                    // for (int y = 0; y < image.Height; y += blockSize)
                    // {
                    //     for (int x = 0; x < image.Width; x += blockSize)
                    //     {
                    //         PixelBlock block = new PixelBlock
                    //         {
                    //             Width = System.Math.Min(blockSize, image.Width - x),
                    //             Height = System.Math.Min(blockSize, image.Height - y)
                    //         };
                    //
                    //         for (int by = 0; by < block.Height; by++)
                    //         {
                    //             for (int bx = 0; bx < block.Width; bx++)
                    //             {
                    //                 int pixelIndex = (y + by) * image.Width + (x + bx);
                    //                 Pixel pixel = page.Pixels[pixelIndex];
                    //                 pixel.SetBlockPosition(bx, by);
                    //                 block.Pixels.Add(pixel);
                    //             }
                    //         }
                    //
                    //         page.PixelBlocks.Add(block);
                    //     }
                    // }

                    image.Dispose();
                }

                GC.Collect();

                return Task.FromResult(page);
            }
        }
    }
}