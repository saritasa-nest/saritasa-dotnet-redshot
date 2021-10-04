using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IEtoFormsBridgeService
    {
        Task<byte[]> OpenScreenSelectionForm();
    }
}
