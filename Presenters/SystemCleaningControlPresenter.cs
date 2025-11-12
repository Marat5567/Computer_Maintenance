using Computer_Maintenance.Views;
using Computer_Maintenance.Models;

namespace Computer_Maintenance.Presenters
{
    public class SystemCleaningControlPresenter
    {
        private readonly ISystemCleaningControlView _systemCleaningControlView;
        private readonly SystemCleaningControlModel _systemCleaningControlModel;
        public SystemCleaningControlPresenter(ISystemCleaningControlView systemCleaningControlView, SystemCleaningControlModel systemCleaningControlModel)
        {
            _systemCleaningControlView = systemCleaningControlView;
            _systemCleaningControlModel = systemCleaningControlModel;
        }

    }
}
