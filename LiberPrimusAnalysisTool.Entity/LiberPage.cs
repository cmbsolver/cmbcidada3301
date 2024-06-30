using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace LiberPrimusAnalysisTool.Entity
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
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [Required]
        [Column("FILE_NAME")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        /// <value>
        /// The name of the page.
        /// </value>
        [Required]
        [Column("PAGE_NAME")]
        public string PageName { get; set; }

        /// <summary>
        /// Gets or sets the total colors.
        /// </summary>
        /// <value>
        /// The total colors.
        /// </value>
        [Required]
        [Column("TOTAL_COLORS")]
        public int TotalColors { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [Required]
        [Column("HEIGHT")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [Required]
        [Column("WIDTH")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the pixel count.
        /// </summary>
        /// <value>
        /// The pixel count.
        /// </value>
        [Required]
        [Column("PIXEL_COUNT")]
        public int PixelCount { get; set; }

        /// <summary>
        /// Gets or sets the pixels.
        /// </summary>
        /// <value>
        /// The pixels.
        /// </value>
        [NotMapped]
        public List<Pixel> Pixels { get; set; }

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