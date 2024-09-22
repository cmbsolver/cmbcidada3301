namespace LiberPrimusAnalysisTool.Application.Interfaces
{
    /// <summary>
    /// The interface for the sequence classes.
    /// </summary>
    public interface ISequence
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        static abstract string Name { get; }
        
        /// <summary>
        /// Get whether the sequence is positional.
        /// </summary>
        static bool IsPositional { get; set; }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="isPositional">Whether is positional.</param>
        /// <returns></returns>
        static abstract object BuildCommand(ulong number, bool isPositional);
    }
}