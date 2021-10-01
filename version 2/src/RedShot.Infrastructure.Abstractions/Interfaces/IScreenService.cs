using RedShot.Infrastructure.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Infrastructure.Abstractions.Interfaces
{
    public interface IScreenService
    {
        IReadOnlyCollection<Screen> GetScreens();
    }
}
