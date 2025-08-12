using System.Windows.Input;

#pragma warning disable IDE0079
#pragma warning disable CA1050

/// <summary>
/// Represents a command with the ability to raise the <see cref="ICommand.CanExecuteChanged"/> event.
/// </summary>
public interface IRelayCommand : ICommand
{
    /// <summary>
    /// Raises the <see cref="ICommand.CanExecuteChanged"/> event, which is used to re-evaluate whether the command can be executed.
    /// </summary>
    void RaiseCanExecuteChanged();
}
