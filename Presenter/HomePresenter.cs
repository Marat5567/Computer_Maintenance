using Computer_Maintenance.Controls;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Presenters
{
    public class HomePresenter
    {
        private readonly IHomeView _homeControlView;
        private readonly HomeModel _homeControlModel;

        private SystemCleaningControl _systemCleaningControl;
        private SystemCleaningModel _systemCleaningControlModel;
        private SystemCleaningPresenter _systemCleaningControlPresenter;
        public HomePresenter(IHomeView homeControlView, HomeModel homeControlModel)
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
            _systemCleaningControlModel = new SystemCleaningModel();
            _systemCleaningControlPresenter = new SystemCleaningPresenter(_systemCleaningControl, _systemCleaningControlModel);
        }
    }
}
