namespace Computer_Maintenance.CustomControl
{
    public class CustomProgressBar : ProgressBar
    {
        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int borderWidth = 1;

            Rectangle rect = new Rectangle(
                0, 0,
                this.Width - borderWidth,
                this.Height - borderWidth);

            Graphics g = e.Graphics;

            // 1. Очищаем область
            g.Clear(this.Parent?.BackColor ?? SystemColors.Control);

            // 2. Рисуем фон (белый)
            using (Brush bgBrush = new SolidBrush(Color.White))
                g.FillRectangle(bgBrush, rect);

            // 3. Рисуем прогресс
            if (this.Value > 0)
            {
                int progressWidth = (int)(rect.Width * ((double)this.Value / this.Maximum));
                Rectangle progressRect = new Rectangle(
                    rect.X, rect.Y, progressWidth, rect.Height);

                Color progressColor = GetProgressColor(this.Value);
                using (Brush progressBrush = new SolidBrush(progressColor))
                    g.FillRectangle(progressBrush, progressRect);
            }

            // 4. Рисуем рамку
            using (Pen borderPen = new Pen(Color.LightGray, borderWidth))
                g.DrawRectangle(borderPen, rect);
        }

        private Color GetProgressColor(int value)
        {
            int percent = (int)((float)value / this.Maximum * 100);
            if (percent > 90) return Color.FromArgb(196, 49, 75);
            if (percent > 75) return Color.FromArgb(255, 140, 0);
            return Color.FromArgb(0, 116, 224);
        }

        // Обязательно перерисовываем при изменении размера
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }
    }
}