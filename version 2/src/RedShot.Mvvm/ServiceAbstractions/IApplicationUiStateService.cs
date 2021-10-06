using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Mvvm.ServiceAbstractions
{
    public interface IApplicationUiStateService
    {
        void HideUiInterface();

        void ShowUiInterface();
    }
}
