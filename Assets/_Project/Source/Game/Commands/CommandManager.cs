using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Game.Commands
{
    /// <summary>
    /// Command manager for tracking command history and supporting undo functionality
    /// </summary>
    public class CommandManager
    {
        public event Action<bool> OnUndoAvailabilityChanged;

        public bool CanUndo => _commandHistory.Count > 0;

        #region private fields
        private readonly Stack<ICommand> _commandHistory = new Stack<ICommand>();
        private readonly int _maxHistorySize;
        #endregion

        public CommandManager(int maxHistorySize = 50)
        {
            _maxHistorySize = maxHistorySize;
        }

        #region public methods

        /// <summary>
        /// Execute the command and add it to the history
        /// </summary>
        /// <param name="command">Command to execute</param>
        /// <returns>True if the command executed successfully</returns>
        public bool ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                Debug.LogError("[CommandManager] Attempted to execute null command");
                return false;
            }

            bool success = command.Execute();

            if (success)
            {
                _commandHistory.Push(command);

                // Limit the size of history
                if (_commandHistory.Count > _maxHistorySize)
                {
                    var commandsToKeep = _commandHistory.Take(_maxHistorySize).ToArray();
                    _commandHistory.Clear();
                    for (int i = commandsToKeep.Length - 1; i >= 0; i--)
                    {
                        _commandHistory.Push(commandsToKeep[i]);
                    }
                }

                OnUndoAvailabilityChanged?.Invoke(CanUndo);
                Debug.Log($"[CommandManager] Executed command: {command.Description}");
            }
            else
            {
                Debug.LogWarning($"[CommandManager] Failed to execute command: {command.Description}");
            }

            return success;
        }

        /// <summary>
        /// Undo the last command
        /// </summary>
        /// <returns>True if the undo executed successfully</returns>
        public bool UndoLastCommand()
        {
            if (!CanUndo)
            {
                Debug.LogWarning("[CommandManager] No commands to undo");
                return false;
            }

            var lastCommand = _commandHistory.Pop();

            try
            {
                lastCommand.Undo();
                OnUndoAvailabilityChanged?.Invoke(CanUndo);
                Debug.Log($"[CommandManager] Undid command: {lastCommand.Description}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"[CommandManager] Failed to undo command {lastCommand.Description}: {ex.Message}");
                // Return the command to the stack if the undo failed
                _commandHistory.Push(lastCommand);
                return false;
            }
        }

        /// <summary>
        /// Clear the entire command history
        /// </summary>
        public void ClearHistory()
        {
            _commandHistory.Clear();
            OnUndoAvailabilityChanged?.Invoke(CanUndo);
            Debug.Log("[CommandManager] Command history cleared");
        }

        /// <summary>
        /// Get the count of commands in the history
        /// </summary>
        public int GetHistoryCount()
        {
            return _commandHistory.Count;
        }

        #endregion
    }
}