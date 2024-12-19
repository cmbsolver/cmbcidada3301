namespace LiberPrimusAnalysisTool.Entity.Image
{
    /// <summary>
    /// The page from the liber primus
    /// </summary>
    public class LiberPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiberPage"/> class.
        /// </summary>
        public LiberPage()
        {
            Pixels = new List<Pixel>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the total colors.
        /// </summary>
        /// <value>
        /// The total colors.
        /// </value>
        public int TotalColors { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the pixel count.
        /// </summary>
        /// <value>
        /// The pixel count.
        /// </value>
        public int PixelCount { get; set; }

        /// <summary>
        /// Gets or sets the pixels.
        /// </summary>
        /// <value>
        /// The pixels.
        /// </value>
        public List<Pixel> Pixels { get; set; }
        
        /// <summary>
        /// This is the pixel blocks.
        /// </summary>
        public List<PixelBlock> PixelBlocks { get; set; }
        
        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public List<byte> Bytes
        {
            get
            {
                return File.ReadAllBytes(FileName).ToList();
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{PageName} - ({FileName})";
        }
    }
}