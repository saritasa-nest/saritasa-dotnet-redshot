using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// Format configuration option.
    /// </summary>
    public class GeneralConfigurationOption : INotifyPropertyChanged
    {
        private string pattern;
        private bool launchAtSystemStart;

        /// <summary>
        /// Initializes <see cref="GeneralConfigurationOption"/> object.
        /// </summary>
        public GeneralConfigurationOption()
        {
            pattern = "%date%rnd";
            launchAtSystemStart = true;
        }

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

        /// <summary>
        /// Whether to launch at system start.
        /// </summary>
        public bool LaunchAtSystemStart
        {
            get => launchAtSystemStart;
            set
            {
                launchAtSystemStart = value;
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