using RedShot.Infrastructure.Domain.Files;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IEtoFormsBridgeService
    {
        Task<File> OpenScreenshotForm();

        Task<Rectangle> OpenRecordingAreaSelectionForm();

        Task<File> OpenRecordingForm(Rectangle recordingArea);
    }
}
