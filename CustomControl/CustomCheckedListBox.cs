namespace Computer_Maintenance.CustomControl
{
    public class CustomCheckedListBox : CheckedListBox
    {
        public CustomCheckedListBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            CheckOnClick = true;

            // Используем SelectionMode.One вместо None
            SelectionMode = SelectionMode.One;

            // Скрываем выделение через обработчики
            this.SelectedIndexChanged += OnSelectedIndexCleared;
        }

        private void OnSelectedIndexCleared(object sender, EventArgs e)
        {
            // Отсоединяем обработчик чтобы избежать рекурсии
            this.SelectedIndexChanged -= OnSelectedIndexCleared;

            // Очищаем выделение
            if (SelectedIndex != -1)
            {
                SelectedIndex = -1;
            }

            // Возвращаем обработчик
            this.SelectedIndexChanged += OnSelectedIndexCleared;
        }
    }
}