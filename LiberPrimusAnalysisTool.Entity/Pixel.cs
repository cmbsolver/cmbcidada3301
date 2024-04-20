namespace LiberPrimusAnalysisTool.Entity
{
    /// <summary>
    /// Pixel
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel"/> class.
        /// </summary>
        public Pixel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pixel" /> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="r">The r.</param>
        /// <param name="g">The g.</param>
        /// <param name="b">The b.</param>
        /// <param name="hex">The hexadecimal.</param>
        /// <param name="pageName">Name of the page.</param>
        public Pixel(int x, int y, int r, int g, int b, string hex, string pageName)
        {
            X = x;
            Y = y;
            R = r;
            G = g;
            B = b;
            Hex = hex;
            PageName = pageName;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public long Position { get; set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the r.
        /// </summary>
        /// <value>
        /// The r.
        /// </value>
        public int R { get; set; }

        /// <summary>
        /// Gets or sets the g.
        /// </summary>
        /// <value>
        /// The g.
        /// </value>
        public int G { get; set; }

        /// <summary>
        /// Gets or sets the b.
        /// </summary>
        /// <value>
        /// The b.
        /// </value>
        public int B { get; set; }

        /// <summary>
        /// Gets or sets the hexadecimal.
        /// </summary>
        /// <value>
        /// The hexadecimal.
        /// </value>
        public string Hex { get; set; }

        /// <summary>
        /// Gets or sets the page identifier.
        /// </summary>
        /// <value>
        /// The page identifier.
        /// </value>
        public string PageName { get; set; }
    }
}