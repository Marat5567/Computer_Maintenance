using Computer_Maintenance.Core.Services;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums;
using Computer_Maintenance.Model.Models;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Controls
{
    public partial class SystemCleaningControl : UserControl, ISystemCleaningControlView
    {
        public event EventHandler LoadDrivesRequested;
        public event EventHandler StartScanClicked;
        public event EventHandler StartCleanClicked;
        public SystemCleaningControl()
        {
            InitializeComponent();
            LoadDrivesRequested?.Invoke(this, EventArgs.Empty);
        }

        private void SystemCleaningControl_Load(object sender, EventArgs e)
        {
            LoadDrivesRequested?.Invoke(this, EventArgs.Empty);
            ThemeService.RefreshTheme(this);
        }
        private void buttonStartScan_Click(object sender, EventArgs e)
        {
            StartScanClicked?.Invoke(this, EventArgs.Empty);
        }
        private void buttonStartClean_Click(object sender, EventArgs e)
        {
            StartCleanClicked?.Invoke(this, EventArgs.Empty);
        }
        public void DisplayDrives(List<DriveInfoModel> drives)
        {
            flowLayoutPanelDrives.Controls.Clear();
            Control[] controls = new Control[drives.Count];

            for (int i = 0; i < drives.Count; i++)
            {
                controls[i] = CreateDrivePanel(drives[i]);
            }
            flowLayoutPanelDrives.Controls.AddRange(controls);

        }
        public void DisplayInfoDrives(List<DriveInfoModel> drivesChecked, UserAccess access, Dictionary<(string DiskName, CleanOption Option), long> optionSizes)
        {
            flowLayoutPanelInfoDrives.Controls.Clear();
            Control[] controls = new Control[drivesChecked.Count];

            for (int i = 0; i < drivesChecked.Count; i++)
            {
                controls[i] = CreateInfoPanel(drivesChecked[i], access, optionSizes);
            }

            flowLayoutPanelInfoDrives.Controls.AddRange(controls);
        }

        public List<DriveInfoModel> GetCheckedDrives()
        {
            List<DriveInfoModel> checkedDrives = new List<DriveInfoModel>();

            foreach (Panel panel in flowLayoutPanelDrives.Controls.OfType<Panel>())
            {
                CheckBox cb = panel.Controls.OfType<CheckBox>().FirstOrDefault();
                if (cb != null && cb.Checked)
                {
                    if (panel.Tag is DriveInfoModel drive)
                    {
                        checkedDrives.Add(drive);
                    }
                }
            }

            return checkedDrives;
        }

        public List<(DriveInfoModel Disk, OptionInfo Option)> GetSelectedOptions()
        {
            var selectedOptions = new List<(DriveInfoModel, OptionInfo)>();

            foreach (Panel panel in flowLayoutPanelInfoDrives.Controls.OfType<Panel>())
            {
                // Получаем диск из заголовка панели
                Label titleLabel = panel.Controls.OfType<Label>().FirstOrDefault(l => l.Font.Bold);
                if (titleLabel == null)
                {
                    continue;
                }

                string diskName = titleLabel.Text.Split(' ')[1]; // "Диск C:\" -> "C:\"
                DriveInfoModel disk = GetCheckedDrives().FirstOrDefault(d => d.Name.Remove(2) == diskName);
                if (disk == null) continue;

                // Перебираем все CheckBox панели
                foreach (CheckBox cb in panel.Controls.OfType<CheckBox>())
                {
                    if (cb.Checked && cb.Tag is OptionInfo opt)
                    {
                        selectedOptions.Add((disk, opt));
                    }
                }
            }

            return selectedOptions;
        }
        public void DisplayClearInfoDrives()
        {
            flowLayoutPanelInfoDrives.Controls.Clear();
        }
        private Panel CreateDrivePanel(DriveInfoModel dInfo)
        {
            Panel panel = new Panel
            {
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = GlobalSettings.BackgroundColor,
                Tag = dInfo
            };

            CheckBox checkBox = new CheckBox
            {
                ForeColor = GlobalSettings.TextColor,
                Text = dInfo.Name.Remove(2),
                Location = new Point(10, 5),
                AutoSize = true,
                Checked = dInfo.IsSystem
            };

            ProgressBar progressBar = new ProgressBar
            {
                Location = new Point(10, 30),
                Width = 200,
                Height = 15,
                Style = ProgressBarStyle.Blocks,
                Value = Math.Min(Math.Max(dInfo.PercentUsed, 0), 100)
            };

            Label labelAvailable = new Label
            {
                Location = new Point(10, 50),
                AutoSize = true,
                ForeColor = GlobalSettings.TextColor,
                Text = $"{dInfo.FreeGB:F1} ГБ"
            };

            Label labelText1 = new Label
            {
                Location = new Point(80, 50),
                AutoSize = true,
                ForeColor = GlobalSettings.TextColor,
                Text = "доступно из"
            };

            Label labelTotal = new Label
            {
                Location = new Point(160, 50),
                AutoSize = true,
                ForeColor = GlobalSettings.TextColor,
                Text = $"{dInfo.TotalGB:F1} ГБ"
            };

            panel.Controls.Add(checkBox);
            panel.Controls.Add(progressBar);
            panel.Controls.Add(labelAvailable);
            panel.Controls.Add(labelText1);
            panel.Controls.Add(labelTotal);

            return panel;
        }

        private Panel CreateInfoPanel(DriveInfoModel disk, UserAccess access, Dictionary<(string DiskName, CleanOption Option), long> optionSizes)
        {
            Panel panel = new Panel { AutoSize = true, BorderStyle = BorderStyle.FixedSingle, Padding = new Padding(10), BackColor = GlobalSettings.BackgroundColor };

            Label title = new Label
            {
                Text = $"Диск {disk.Name.Remove(2)} — доступные опции",
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = GlobalSettings.TextColor,
                Location = new Point(5, 5)
            };
            panel.Controls.Add(title);

            int y = 35;
            foreach (var opt in CleaningRules.Rules[disk.DiskType])
            {
                bool allowed = access == UserAccess.Administrator || !opt.RequiresAdmin;

                long sizeBytes = 0;
                optionSizes?.TryGetValue((disk.Name, opt.Option), out sizeBytes);

                string sizeText = sizeBytes > 0 ? $" ({FormatService.FormatBytes(sizeBytes)})" : " (0)";

                CheckBox cb = new CheckBox
                {
                    Text = allowed ? opt.Name + sizeText : opt.Name + " [требуется права админа]",
                    AutoSize = true,
                    Location = new Point(10, y),
                    BackColor = GlobalSettings.BackgroundColor,
                    ForeColor = allowed ? GlobalSettings.TextColor : Color.Red,
                    Tag = opt,
                    Checked = allowed
                };

                if (!allowed)
                {
                    cb.Checked = false;
                    cb.Click += (s, e) => cb.Checked = false;
                }

                panel.Controls.Add(cb);
                y += 30;
            }

            return panel;
        }


    }
}
