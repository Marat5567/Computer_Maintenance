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
        public void ShowCheckedDriveSafe(DriveInfo dInfo, List<CleaningInformation> cleaningInformation)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action(() => ShowCheckedDrive(dInfo, ref cleaningInformation)));
            else
                ShowCheckedDrive(dInfo, ref cleaningInformation);
        }


        //<summary>
        //Метод для вывода выбранных дисков
        //<summary>
        public void ShowCheckedDrive(DriveInfo dInfo, ref List<CleaningInformation> cleaningInformation)
        {

            if (flowLayoutPanelInfoDrives == null)
                throw new InvalidOperationException("flowLayoutPanelInfoDrives не инициализирован");

            if (dInfo == null) return;


            cleaningInformation ??= new List<CleaningInformation>();

            FlowLayoutPanel diskPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                BackColor = ApplicationSettings.BackgroundColor,
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Top,
                Padding = new Padding(10),
            };

            // Суммарный размер
            StorageSize totalSize = new StorageSize();
            foreach (var cleanInfo in cleaningInformation)
            {
                if (cleanInfo?.Size != null)
                    totalSize.AddSize(cleanInfo.Size);
            }

            Label labelDiskName = new Label
            {
                ForeColor = ApplicationSettings.TextColor,
                Text = dInfo.DriveType == DriveType.Fixed
                    ? $"Локальный диск ({dInfo.Name.Replace(":\\", ":")})"
                    : dInfo.Name.Replace(":\\", ":"),
                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 10),
            };
            diskPanel.Controls.Add(labelDiskName);

            // Общий размер для очистки
            FlowLayoutPanel totalSizePanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = ApplicationSettings.BackgroundColor,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
            };

            Label labelTextAvailableForClear = new Label
            {
                Text = "Доступно для очистки (всего) :",
                AutoSize = true,
                ForeColor = ApplicationSettings.TextColor
            };

            Label availableForClear = new Label
            {
                Text = $"[ {totalSize.GetSizeByType(totalSize.GetMaxSizeType(), 2)} ]",
                AutoSize = true,
                ForeColor = totalSize.GetColorBySize(),
            };

            totalSizePanel.Controls.Add(labelTextAvailableForClear);
            totalSizePanel.Controls.Add(availableForClear);
            diskPanel.Controls.Add(totalSizePanel);

            if (cleaningInformation.Count == 0)
            {
                Label noOptions = new Label
                {
                    Text = "Нет доступных опций очистки",
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(20, 5, 5, 5)
                };
                diskPanel.Controls.Add(noOptions);
            }
            else
            {
                foreach (CleaningInformation cleanInfo in cleaningInformation)
                {
                    if (cleanInfo == null) continue;

                    FlowLayoutPanel optionPanel = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.TopDown,
                        AutoSize = true,
                        WrapContents = false,
                        Margin = new Padding(5, 0, 0, 0),
                        BackColor = ApplicationSettings.BackgroundColor,
                    };

                    CheckBox mainCheckBox = new CheckBox
                    {
                        Text = cleanInfo.SectionName,
                        AutoSize = true,
                        BackColor = ApplicationSettings.BackgroundColor,
                        ForeColor = ApplicationSettings.TextColor,
                        Tag = cleanInfo
                    };

                    Label sizeLabel = new Label
                    {
                        Text = $"[{cleanInfo.Size.GetSizeByType(cleanInfo.Size.GetMaxSizeType(), 1) ?? "0"}]",
                        AutoSize = true,
                        ForeColor = cleanInfo.Size.GetColorBySize(),
                        Margin = new Padding(0, 4, 0, 0)
                    };

                    FlowLayoutPanel subItemsContainer = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.TopDown,
                        AutoSize = true,
                        WrapContents = false,
                        Visible = false,
                        Margin = new Padding(25, 0, 0, 5),
                        BackColor = ApplicationSettings.BackgroundColor,
                        Padding = new Padding(0, 5, 0, 0)
                    };

                    if (cleanInfo.SubItems != null)
                    {
                        foreach (var sub in cleanInfo.SubItems)
                        {
                            if (sub == null || cleanInfo.OnlyOnePoint)
                                continue;

                            FlowLayoutPanel subItemPanel = new FlowLayoutPanel
                            {
                                FlowDirection = FlowDirection.LeftToRight,
                                AutoSize = true,
                                WrapContents = false,
                                Margin = new Padding(0, 0, 0, 2),
                                BackColor = ApplicationSettings.BackgroundColor
                            };

                            CheckBox subCheckBox = new CheckBox
                            {
                                Text = sub.SectionName,
                                AutoSize = true,
                                BackColor = ApplicationSettings.BackgroundColor,
                                ForeColor = ApplicationSettings.TextColor,
                                Margin = new Padding(0, 2, 5, 0),
                                Tag = sub
                            };

                            Label subSizeLabel = new Label
                            {
                                Text = $"[{sub.Size.GetSizeByType(sub.Size.GetMaxSizeType(), 1) ?? "0"}]",
                                AutoSize = true,
                                ForeColor = sub.Size.GetColorBySize(),
                                Margin = new Padding(0, 4, 0, 0),
                                Font = new Font(subCheckBox.Font, FontStyle.Bold)
                            };

                            subItemPanel.Controls.Add(subCheckBox);
                            subItemPanel.Controls.Add(subSizeLabel);
                            subItemsContainer.Controls.Add(subItemPanel);
                        }
                    }

                    mainCheckBox.CheckedChanged += (s, e) =>
                    {
                        if (!cleanInfo.OnlyOnePoint)
                            subItemsContainer.Visible = mainCheckBox.Checked;
                    };

                    FlowLayoutPanel headerPanel = new FlowLayoutPanel
                    {
                        BackColor = ApplicationSettings.BackgroundColor,
                        FlowDirection = FlowDirection.LeftToRight,
                        AutoSize = true,
                        WrapContents = false
                    };
                    headerPanel.Controls.Add(mainCheckBox);
                    headerPanel.Controls.Add(sizeLabel);

                    optionPanel.Controls.Add(headerPanel);
                    optionPanel.Controls.Add(subItemsContainer);
                    diskPanel.Controls.Add(optionPanel);
                }
            }

            flowLayoutPanelInfoDrives.Controls.Add(diskPanel);
            flowLayoutPanelInfoDrives.PerformLayout();
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
