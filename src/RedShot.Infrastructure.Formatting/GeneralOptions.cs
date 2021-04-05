using RedShot.Infrastructure.Abstractions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// General options.
    /// </summary>
    public class GeneralOptions : INotifyPropertyChanged
    {
        private UpdateInterval updateInterval;
        private string pattern;
        private bool launchAtSystemStart;

        /// <inheritdoc/>
        public string UniqueName => "GeneralConfiguration";

        /// <summary>
        /// Initializes <see cref="GeneralOptions"/> object.
        /// </summary>
        public GeneralOptions()
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

        /// <summary>
        /// Update interval.
        /// </summary>
        public UpdateInterval UpdateInterval
        {
            get => updateInterval;
            set
            {
                updateInterval = value;
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