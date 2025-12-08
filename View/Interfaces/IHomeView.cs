namespace Computer_Maintenance.View.Interfaces
{
    public interface IHomeView
    {
        event EventHandler<TreeViewEventArgs> FunctionalityClicked; //Событие на выбранный функционал
        void SetFunctionalityControl(UserControl functionalityControl); //Метод показа выбранного функционала
    }
}
