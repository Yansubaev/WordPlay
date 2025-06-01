namespace Source.Game.Commands
{
    /// <summary>
    /// Base interface for commands that support execution and undo
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Command description for debugging
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Execute the command
        /// </summary>
        /// <returns>True if the command was executed successfully</returns>
        bool Execute();
        /// <summary>
        /// Undo the command
        /// </summary>
        void Undo();
    }
}