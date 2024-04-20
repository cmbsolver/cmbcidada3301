namespace LiberPrimusAnalysisTool.Entity
{
    /// <summary>
    /// RgbCharacters
    /// </summary>
    public class RgbCharacters
    {
        private List<string> _r;
        private List<string> _g;
        private List<string> _b;

        /// <summary>
        /// Initializes a new instance of the <see cref="RgbCharacters" /> class.
        /// </summary>
        /// <param name="pageName">Name of the page.</param>
        public RgbCharacters(string pageName)
        {
            PageName = pageName;
            _r = new List<string>();
            _g = new List<string>();
            _b = new List<string>();
        }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        public string PageName { get; set; }

        /// <summary>
        /// Gets the r.
        /// </summary>
        /// <value>
        /// The r.
        /// </value>
        public string R
        {
            get
            {
                return string.Join("", _r);
            }
        }

        /// <summary>
        /// Gets the g.
        /// </summary>
        /// <value>
        /// The g.
        /// </value>
        public string G
        {
            get
            {
                return string.Join("", _g);
            }
        }

        /// <summary>
        /// Gets the b.
        /// </summary>
        /// <value>
        /// The b.
        /// </value>
        public string B
        {
            get
            {
                return string.Join("", _b);
            }
        }

        /// <summary>
        /// Adds the r.
        /// </summary>
        /// <param name="r">The r.</param>
        public void AddR(string r)
        {
            _r.Add(r);
        }

        /// <summary>
        /// Adds the g.
        /// </summary>
        /// <param name="g">The g.</param>
        public void AddG(string g)
        {
            _g.Add(g);
        }

        /// <summary>
        /// Adds the b.
        /// </summary>
        /// <param name="b">The b.</param>
        public void AddB(string b)
        {
            _b.Add(b);
        }
    }
}