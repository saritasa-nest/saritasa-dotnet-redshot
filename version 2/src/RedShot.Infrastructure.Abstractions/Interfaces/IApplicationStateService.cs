using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IApplicationStateService
    {
        void ShutdownApplication();
    }
}
