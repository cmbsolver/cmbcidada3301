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
        /// Prompts the command.
        /// </summary>
        static abstract object PromptCommand();

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        static abstract object BuildCommand(long number);
    }
}