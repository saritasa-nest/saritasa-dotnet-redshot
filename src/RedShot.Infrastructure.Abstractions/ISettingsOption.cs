using Eto.Forms;

namespace RedShot.Infrastructure.Abstractions
{
    public interface ISettingsOption
    {
        string Name { get; }

        Control GetControl();

        void Save();
    }
}
