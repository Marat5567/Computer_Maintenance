using Computer_Maintenance.Controls;
using Computer_Maintenance.Models;
using Computer_Maintenance.Views;

namespace Computer_Maintenance.Presenters
{
    public class MainFormPresenter
    {
        private readonly IMainFormView _mainFormView;
        private readonly MainFormModel _mainFormModel;

        private MainControl _mainControl;
        private MainControlPresenter _mainControlPresenter;
        private MainControlModel _mainControlModel;

        public MainFormPresenter(IMainFormView mainFormView, MainFormModel mainFormModel)
        {
            _mainFormModel = mainFormModel;
            _mainFormView = mainFormView;

            InitializeMainControl();
        }
        private void InitializeMainControl()
        {
            _mainControl = new MainControl();
            _mainControlModel = new MainControlModel();
            _mainControlPresenter = new MainControlPresenter(_mainControl, _mainControlModel);

            _mainFormView.SetMainControl(_mainControl);
        }
    }
}
