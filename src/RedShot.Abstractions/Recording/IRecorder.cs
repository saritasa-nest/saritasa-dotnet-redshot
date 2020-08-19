using Eto.Drawing;

namespace RedShot.Abstractions.Recording
{
    public interface IRecorder
    {
        void Start(Rectangle area);

        void Stop();

        IFile GetVideo();

        bool IsRecording { get; }
    }
}
