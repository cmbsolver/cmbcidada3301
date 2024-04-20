// FROM https://github.com/ghost1face/FileTypeInterrogator

using System;

namespace FileTypeInterrogator
{
    /// <summary>
    /// Information regarding the file type, including name, extension, mime type and signature.
    /// </summary>
    public class FileTypeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileTypeInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="header">The header.</param>
        /// <param name="alias">The alias.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="subHeader">The sub header.</param>
        internal FileTypeInfo(string name, string fileType, string mimeType, byte[] header, string[] alias = null, int offset = 0, byte[] subHeader = null)
        {
            Name = name;
            MimeType = mimeType;
            FileType = fileType;
            Header = header;
            Offset = offset;
            SubHeader = subHeader;
            Alias = alias;
        }

        /// <summary>
        /// Gets the name of the file type.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the extension which is related to this type
        /// </summary>
        public string FileType { get; private set; }

        /// <summary>
        /// Gets the MimeType of this type
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Other names for this file type
        /// </summary>
        public string[] Alias { get; private set; }

        /// <summary>
        /// Gets unique header 'Magic Numbers' to identifiy this file type
        /// </summary>
        /// <value>
        /// The header.
        /// </value>
        public byte[] Header { get; private set; }

        /// <summary>
        /// Gets the offset location of the Header details
        /// </summary>
        /// <value>
        /// The offset.
        /// </value>
        public int Offset { get; private set; }

        /// <summary>
        /// Gets the additional identifier to guarantee uniqueness of the file type
        /// </summary>
        /// <value>
        /// The additional identifier.
        /// </value>
        public byte[] SubHeader { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $@"{Name} ({FileType})
{MimeType}
{string.Join("|", Alias ?? Array.Empty<string>())}";
        }
    }
}