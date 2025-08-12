using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CourseApp.Models
{
    /// <summary>
    /// Represents a tag that can be assigned to courses or modules, with support for property change notifications.
    /// </summary>
    public partial class Tag : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        private bool isSelected;

        /// <summary>
        /// Gets or sets a value indicating whether the tag is currently selected.
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. This is optional and defaults to the caller name.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
