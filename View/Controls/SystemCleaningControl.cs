using Computer_Maintenance.Core.Services;
using Computer_Maintenance.CustomControl;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Controls
{
    public partial class SystemCleaningControl : UserControl, ISystemCleaningView
    {
        public event EventHandler LoadDrivesRequested;
        public event EventHandler StartScanClicked;
        public event EventHandler StartCleanClicked;
        public SystemCleaningControl()
        {
            InitializeComponent();
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
        private void buttonRefreshDrives_Click(object sender, EventArgs e)
        {
            LoadDrivesRequested.Invoke(this, EventArgs.Empty);
        }
        //<summary>
        //Метод для вывода всех доступных дисков
        //<summary>
        public void ShowAvailableDrives(ref List<DriveInfo> dInfos)
        {
            flowLayoutPanelDrives.Controls.Clear();

            Control[] controls = new Control[dInfos.Count];

            for (int i = 0; i < dInfos.Count; i++)
            {
                long totalBytes = dInfos[i].TotalSize;
                long freeBytes = dInfos[i].TotalFreeSpace;
                long usedBytes = totalBytes - freeBytes;

                StorageSize total = ConvertSizeService.ConvertSize(totalBytes);
                StorageSize free = ConvertSizeService.ConvertSize(freeBytes);
                StorageSize used = ConvertSizeService.ConvertSize(usedBytes);

                uint totalGB = total.GB;
                uint freeGB = free.GB;
                uint usedGB = used.GB;

                int percentUsed = totalBytes > 0 ? (int)((double)usedBytes / totalBytes * 100) : 0;

                Panel panelDrive = new Panel
                {
                    BackColor = ApplicationSettings.BackgroundColor,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),
                    AutoSize = true
                };

                CheckBox checkBoxDisk = new CheckBox
                {
                    ForeColor = ApplicationSettings.TextColor,
                    Text = dInfos[i].DriveType == DriveType.Fixed ? $"Локальный диск ({dInfos[i].Name.Replace(":\\", ":")})" : dInfos[i].Name.Replace(":\\", ":"),
                    Location = new Point(10, 10),
                    Tag = dInfos[i],
                    AutoSize = true
                };

                CustomProgressBar progressBar = new CustomProgressBar
                {
                    BackColor = Color.White,
                    Minimum = 0,
                    Maximum = 100,
                    Value = (int)percentUsed,
                    Size = new Size(220, 18),
                    Style = ProgressBarStyle.Continuous,
                    Location = new Point(10, 35)
                };

                Label labelDetails = new Label
                {
                    ForeColor = ApplicationSettings.TextColor,
                    Text = $"{free.GetSizeByType(free.GetMaxSizeType(), 1)} свободно из {total.GetSizeByType(total.GetMaxSizeType(), 1)}",
                    //Text = $"{free.GB}, {(free.MB / 100):F0} ГБ свободно из {total.GB}, {(total.MB / 100):F0} ГБ",
                    Location = new Point(10, 65),
                    AutoSize = true
                };

                panelDrive.Controls.Add(checkBoxDisk);
                panelDrive.Controls.Add(progressBar);
                panelDrive.Controls.Add(labelDetails);

                controls[i] = panelDrive;
            }

            flowLayoutPanelDrives.Controls.AddRange(controls);
        }

        //<summary>
        //Метод для получения выбранных дисков
        //<summary>
        public List<DriveInfo> GetSelectedDrives()
        {
            List<DriveInfo> selectedDrives = new List<DriveInfo>();

            foreach (Control panelControl in flowLayoutPanelDrives.Controls)
            {
                Panel panelDrive = (panelControl as Panel)!;
                if (panelDrive != null)
                {
                    foreach (Control innerControl in panelDrive.Controls)
                    {
                        CheckBox checkBoxDisk = (innerControl as CheckBox)!;
                        if (checkBoxDisk != null && checkBoxDisk.Checked)
                        {
                            DriveInfo driveInfo = (checkBoxDisk.Tag as DriveInfo)!;
                            if (driveInfo != null)
                            {
                                selectedDrives.Add(driveInfo);
                            }
                        }
                    }
                }
            }
            return selectedDrives;
        }

        //<summary>
        //Метод для вывода выбранных дисков
        //<summary>
        public void ShowCheckedDrive(DriveInfo dInfo, ref List<CleaningInformation> cleaningInformation)
        {
            int optionCount = cleaningInformation.Count;

            int rowCount = optionCount > 0 ? optionCount + 1 : 2;

            TableLayoutPanel tablePanel = new TableLayoutPanel
            {
                BackColor = ApplicationSettings.BackgroundColor,
                BorderStyle = BorderStyle.FixedSingle,
                RowCount = rowCount,
                ColumnCount = 1,
                AutoSize = true,
            };

            for (int r = 0; r < rowCount; r++)
            {
                tablePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, r == 0 ? 35 : 25));
            }

            Label labelDiskName = new Label
            {
                ForeColor = ApplicationSettings.TextColor,
                Text = dInfo.DriveType == DriveType.Fixed ?
                       $"Локальный диск ({dInfo.Name.Replace(":\\", ":")})" :
                       dInfo.Name.Replace(":\\", ":"),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = true
            };

            tablePanel.Controls.Add(labelDiskName, 0, 0);

            int rowIndex = 1;

            if (cleaningInformation.Count == 0)
            {
                tablePanel.Controls.Add(new Label
                {
                    Text = "Нет доступных опций очистки",
                    ForeColor = Color.Gray,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    AutoSize = true
                }, 0, 1);
            }
            else
            {
                foreach (CleaningInformation cleanInfo in cleaningInformation)
                {
                    // Основная панель для строки
                    Panel container = new Panel
                    {
                        BackColor = ApplicationSettings.BackgroundColor,
                        Dock = DockStyle.Fill,
                        AutoSize = true,
                        Margin = new Padding(10, 0, 0, 0)
                    };

                    // CheckBox
                    CheckBox checkBox = new CheckBox
                    {
                        Text = cleanInfo.SectionName,
                        ForeColor = ApplicationSettings.TextColor,
                        AutoSize = true,
                        Location = new Point(0, 0),
                        Tag = cleanInfo
                    };

                    // Label с цветным текстом
                    Label sizeLabel = new Label
                    {
                        Text = $"  [{cleanInfo.Size.GetSizeByType(cleanInfo.Size.GetMaxSizeType(), 1)}]",
                        ForeColor = cleanInfo.Size.GetColorBySize(),
                        AutoSize = true,
                    };

                    container.Controls.Add(checkBox);
                    container.Controls.Add(sizeLabel);
                    sizeLabel.Location = new Point(checkBox.Width, 0);

                    tablePanel.Controls.Add(container, 0, rowIndex);

                    rowIndex++;
                }
            }

            flowLayoutPanelInfoDrives.Controls.Add(tablePanel);
        }

        //<summary>
        //Метод для получения выбранных опций
        //<summary>
        public List<CleaningInformation> GetSelectedOptions()
        {
            List<CleaningInformation> selectedOptions = new List<CleaningInformation>();

            foreach (Control panelControl in flowLayoutPanelInfoDrives.Controls)
            {
                Panel panelInfo = (panelControl as Panel)!;
                if (panelInfo != null)
                {
                    foreach (Control innerControl in panelInfo.Controls)
                    {
                        CheckBox checkBoxOption = (innerControl as CheckBox)!;
                        if (checkBoxOption != null && checkBoxOption.Checked && (checkBoxOption.Tag is CleaningInformation cleaningInfo))
                        {
                            selectedOptions.Add(cleaningInfo);
                        }
                    }
                }
            }

            return selectedOptions;
        }

        ///<summary>
        ///Метод для очистки панели выбранных дисков
        ///<summary>
        public void ClearCheckedDrives()
        {
            if (flowLayoutPanelInfoDrives.Controls.Count >= 1)
            {
                flowLayoutPanelInfoDrives.Controls.Clear();
            }
        }
        ///<summary>
        ///Метод для очистки всех эдеметов по событию LoadDrives
        ///<summary>
        public void ClearAllByEvent_LoadDrives()
        {
            flowLayoutPanelDrives.Controls.Clear();
            flowLayoutPanelInfoDrives.Controls.Clear();
        }
    }
}
