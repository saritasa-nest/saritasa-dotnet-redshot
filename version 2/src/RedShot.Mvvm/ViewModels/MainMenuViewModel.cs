using Microsoft.Toolkit.Mvvm.Input;
using RedShot.Infrastructure.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedShot.Mvvm.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly IEtoFormsBridgeService etoFormsBridge;

        public RelayCommand OpenTestDialogCommand { get; }

        public MainMenuViewModel(IEtoFormsBridgeService etoFormsBridge)
        {
            this.etoFormsBridge = etoFormsBridge;

            OpenTestDialogCommand = new RelayCommand(() => { });
        }
    }
}
