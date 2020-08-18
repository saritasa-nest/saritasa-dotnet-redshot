using Eto.Drawing;

namespace RedShot.Recording.Recorders
{
    public interface IRecorder
    {
        void Start(Rectangle area);

        void Stop();

        bool IsRecording { get; }
    }
}
