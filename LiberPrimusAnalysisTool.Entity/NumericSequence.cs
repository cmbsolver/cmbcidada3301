using System.Collections.Generic;

namespace LiberPrimusAnalysisTool.Entity
{
    /// <summary>
    /// Numeric Sequence object
    /// </summary>
    public class NumericSequence
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericSequence" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public NumericSequence(string name)
        {
            Sequence = new List<long>();
            Number = null;
            Result = null;
            Name = name;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public long? Number { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public long? Result { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public List<long> Sequence { get; set; }
    }
}