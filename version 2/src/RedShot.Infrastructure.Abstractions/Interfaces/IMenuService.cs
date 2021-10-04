using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IMenuService
    {
        Task TakeScreenshotAsync();
    }
}
