using RedShot.Infrastructure.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Format configuration option.
    /// </summary>
    public class FormatConfigurationOption : IConfigurationOption, INotifyPropertyChanged
    {
        private string pattern;

        public FormatConfigurationOption()
        {
            Pattern = "%[RedShot-]%date";
        }

        /// <inheritdoc />
        public string UniqueName => "Formatting";

        /// <summary>
        /// Pattern for formatting.
        /// </summary>
        public string Pattern
        {
            get
            {
                return pattern;
            }

            set
            {
                pattern = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}