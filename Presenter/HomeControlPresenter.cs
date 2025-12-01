using Computer_Maintenance.Controls;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenters
{
    public class HomeControlPresenter
    {
        private readonly IHomeControlView _homeControlView;
        private readonly HomeControlModel _homeControlModel;

        private SystemCleaningControl _systemCleaningControl;
        private SystemCleaningControlModel _systemCleaningControlModel;
        private SystemCleaningControlPresenter _systemCleaningControlPresenter;
        public HomeControlPresenter(IHomeControlView homeControlView, HomeControlModel homeControlModel)
        {
            _homeControlView = homeControlView;
            _homeControlModel = homeControlModel;

            _homeControlView.FunctionalityClicked += OnFunctionalityClicked;

            InitializeSystemCleaningControl();
        }
        private void OnFunctionalityClicked(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is FunctionalityType functionalityType)
            {
                switch (functionalityType)
                {
                    case FunctionalityType.SystemCleaning:
                        _homeControlView.SetFunctionalityControl(_systemCleaningControl);
                        break;
                }
            }
        }
        private void InitializeSystemCleaningControl()
        {
            _systemCleaningControl = new SystemCleaningControl();
            _systemCleaningControlModel = new SystemCleaningControlModel();
            _systemCleaningControlPresenter = new SystemCleaningControlPresenter(_systemCleaningControl, _systemCleaningControlModel);
        }
    }
}
