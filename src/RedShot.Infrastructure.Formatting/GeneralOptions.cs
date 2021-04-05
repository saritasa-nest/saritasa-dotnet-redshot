using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RedShot.Infrastructure.Formatting
{
    /// <summary>
    /// General options.
    /// </summary>
    public class GeneralOptions : INotifyPropertyChanged
    {
        private string pattern;
        private bool launchAtSystemStart;

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
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string memberName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}