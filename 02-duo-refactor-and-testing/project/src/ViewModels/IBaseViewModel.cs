using System.ComponentModel;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0079
#pragma warning disable CA1050

/// <summary>
/// Base interface for view models that includes property change notifications and utility methods.
/// </summary>
public interface IBaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Notifies that a property has changed, triggering any UI updates.
    /// </summary>
    void OnPropertyChanged([CallerMemberName] string propertyName = "");

    /// <summary>
    /// Sets a property value and notifies when the value has changed.
    /// </summary>
    bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "");
}