using MediatR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

public class LsbJpegCalculator
{
    public class Command : IRequest<ulong[]>
    {
        public string FileName { get; }

        public Command(string fileName, int rotationDegrees, int lsbCount)
        {
            FileName = fileName;
        }
    }

    public class Handler : IRequestHandler<Command, ulong[]>
    {
        public async Task<ulong[]> Handle(Command request, CancellationToken cancellationToken)
        {
            using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(request.FileName))
            {
                int width = image.Width;
                int height = image.Height;

                List<Rgba32[,]> blocks = new List<Rgba32[,]>();

                for (int y = 0; y < height; y += 8)
                {
                    for (int x = 0; x < width; x += 8)
                    {
                        Rgba32[,] block = new Rgba32[8, 8];

                        for (int blockY = 0; blockY < 8; blockY++)
                        {
                            for (int blockX = 0; blockX < 8; blockX++)
                            {
                                if (x + blockX < width && y + blockY < height)
                                {
                                    block[blockX, blockY] = image[x + blockX, y + blockY];
                                }
                                else
                                {
                                    block[blockX, blockY] = new Rgba32(0, 0, 0, 0); // Fill with transparent pixels if out of bounds
                                }
                            }
                        }

                        blocks.Add(block);
                    }
                }

                // Process the blocks as needed
                // ...

                return new ulong[0]; // Placeholder return value
            }
        }
    }
}