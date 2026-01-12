using Computer_Maintenance.Core.Services;
using Computer_Maintenance.CustomControl;
using Computer_Maintenance.Model.Config;
using Computer_Maintenance.Model.Enums.SystemCleaning;
using Computer_Maintenance.Model.Services;
using Computer_Maintenance.Model.Structs;
using Computer_Maintenance.View.Interfaces;

namespace Computer_Maintenance.Controls
{
    public partial class SystemCleaningControl : UserControl, ISystemCleaningView
    {
        // События для взаимодействия с контроллером
        public event EventHandler LoadDrivesRequested;   // Запрос на загрузку списка доступных дисков
        public event EventHandler StartScanClicked; // Запуск  сканирования диска
        public event EventHandler StartScanCleanClicked;      // Запуск сканирования для удаления выбранных дисков
        public event EventHandler StartCleanClicked;     // Запуск очистки выбранных опций

        public bool SaveFileDeleteFail_Logs { get; set; } = false;
        public string DirectoryPath { get; set; }

        private List<DriveInfo> _selectedDrives = new List<DriveInfo>(); //выбранные диски
        private List<SubCleaningInformation> _selectedCleaning = new List<SubCleaningInformation>();

        public SystemCleaningControl()
        {
            InitializeComponent();
        }
        private void SystemCleaningControl_Load(object sender, EventArgs e)
        {
            ThemeService.RefreshTheme(this);
            LoadDrivesRequested?.Invoke(this, EventArgs.Empty);
        }

        private void buttonStartScanClean_Click(object sender, EventArgs e)
        {
            StartScanCleanClicked?.Invoke(this, EventArgs.Empty);
        }

        private void buttonStartClean_Click(object sender, EventArgs e)
        {
            StartCleanClicked?.Invoke(this, EventArgs.Empty);
        }

        private void buttonStartScan_Click(object sender, EventArgs e)
        {
            StartScanClicked?.Invoke(this, EventArgs.Empty);
        }
        private void buttonRefreshDrives_Click(object sender, EventArgs e)
        {
            flowLayoutPanelDrives.Controls.Clear();
            flowLayoutPanelInfoDrives.Controls.Clear();

            _selectedDrives.Clear();

            LoadDrivesRequested.Invoke(this, EventArgs.Empty);
        }
        private void checkBoxSaveFileDeleteFailLogs_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSaveFileDeleteFailLogs.Checked)
            {
                SaveFileDeleteFail_Logs = true;
            }
            else
            {
                SaveFileDeleteFail_Logs = false;
            }
        }

        /// <summary>
        /// Получение выбранных пользователем дисков
        /// </summary>
        public List<DriveInfo> GetSelectedDrives()
        {
            return _selectedDrives;
        }

        /// <summary>
        /// Получение выбранных опций очистки для дисков
        /// </summary>
        public List<SubCleaningInformation> GetSelectedOptions()
        {
            return _selectedCleaning;
        }

        /// <summary>
        /// Отображение всех доступных дисков на панели
        /// </summary>
        public void ShowAvailableDrives(List<DriveInfo> dInfos)
        {
            if (dInfos == null || dInfos.Count == 0) { return; }
            flowLayoutPanelDrives.Controls.Clear();

            Control[] controls = new Control[dInfos.Count];
            for (int i = 0; i < dInfos.Count; i++)
            {
                long totalBytes = dInfos[i].TotalSize;
                long freeBytes = dInfos[i].TotalFreeSpace;
                long usedBytes = totalBytes - freeBytes;

                StorageSize total = ConvertSizeService.ConvertSize((ulong)totalBytes);
                StorageSize free = ConvertSizeService.ConvertSize((ulong)freeBytes);
                StorageSize used = ConvertSizeService.ConvertSize((ulong)usedBytes);

                int percentUsed = totalBytes > 0 ? (int)((double)usedBytes / totalBytes * 100) : 0;

                Panel panelDrive = new Panel
                {
                    BackColor = ApplicationSettings.BackgroundColor,
                    BorderStyle = BorderStyle.FixedSingle,
                    Padding = new Padding(5),
                    AutoSize = true
                };

                DriveInfo currentDrive = dInfos[i];

                CheckBox checkBoxDisk = new CheckBox
                {
                    ForeColor = ApplicationSettings.TextColor,
                    Text = currentDrive.DriveType == DriveType.Fixed ? $"Локальный диск ({currentDrive.Name.Replace(":\\", ":")})" : currentDrive.Name.Replace(":\\", ":"),
                    Location = new Point(10, 10),
                    //Tag = dInfos[i],
                    AutoSize = true
                };

                CustomProgressBar progressBar = new CustomProgressBar
                {
                    BackColor = Color.White,
                    Minimum = 0,
                    Maximum = 100,
                    Value = percentUsed,
                    Size = new Size(220, 18),
                    Style = ProgressBarStyle.Continuous,
                    Location = new Point(10, 35)
                };

                Label labelDetails = new Label
                {
                    ForeColor = ApplicationSettings.TextColor,
                    Text = $"{free.GetSizeByType(free.GetMaxSizeType())} свободно из {total.GetSizeByType(total.GetMaxSizeType())}",
                    Location = new Point(10, 65),
                    AutoSize = true
                };

                checkBoxDisk.CheckedChanged += (s, e) =>
                {
                    if (checkBoxDisk.Checked)
                    {
                        _selectedDrives.Add(currentDrive);
                    }
                    else
                    {
                        _selectedDrives.Remove(currentDrive);
                    }
                };

                panelDrive.Controls.Add(checkBoxDisk);
                panelDrive.Controls.Add(progressBar);
                panelDrive.Controls.Add(labelDetails);

                controls[i] = panelDrive;
            }
            flowLayoutPanelDrives.Controls.AddRange(controls);
        }

        /// <summary>
        /// Потокобезопасный вызов ShowCheckedDrive
        /// </summary>
        public void ShowCheckedDriveSafe(DriveInfo dInfo, List<CleaningInformation> cleaningInformation, ShowTypeInfo showTypeInfo)
        {
            switch (showTypeInfo)
            {
                case ShowTypeInfo.CleaningInfo:
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => ShowCheckedDriveForCleaning(dInfo, cleaningInformation)));
                    }
                    else
                    {
                        ShowCheckedDriveForCleaning(dInfo, cleaningInformation);
                    }
                    break;
                case ShowTypeInfo.SizeInfo:
                    if (this.InvokeRequired)
                    {
                        //this.Invoke(new Action(() => ShowCheckedDriveForSizeInfo(dInfo)));
                    }
                    else
                    {
                        //ShowCheckedDriveForSizeInfo(dInfo);
                    }
                    break;

            }
        }

        public void ShowCheckedDriveForSizeInfo(List<DirectoryContents> directoryContents)
        {
            if (directoryContents == null || directoryContents.Count == 0) { return; }

            flowLayoutPanelInfoDrives.Controls.Clear();
            flowLayoutPanelInfoDrives.Dock = DockStyle.Fill;

            Panel container = new Panel
            {
                Height = 500,
                Width = flowLayoutPanelInfoDrives.ClientSize.Width,
                Name = "resizableContainer"
            };

            SplitContainer splitContainer = new SplitContainer
            {
                SplitterWidth = 10,
                Dock = DockStyle.Fill,
                SplitterDistance = 500,
                FixedPanel = FixedPanel.None,
            };

            splitContainer.Panel2Collapsed = false;
            splitContainer.Panel2MinSize = 0;

            ListView listView = new ListView
            {
                BackColor = ApplicationSettings.BackgroundColor,
                ForeColor = ApplicationSettings.TextColor,
                Scrollable = true,
                Dock = DockStyle.Fill,
                View = System.Windows.Forms.View.Details,
                FullRowSelect = true
            };

            ImageList imageList = new ImageList
            {
                ImageSize = new Size(18, 18),
                ColorDepth = ColorDepth.Depth32Bit,
            };

            listView.SmallImageList = imageList;
            listView.Columns.Add("", 28, HorizontalAlignment.Center);
            listView.Columns.Add("Имя", 200, HorizontalAlignment.Left);
            listView.Columns.Add("Размер", 100, HorizontalAlignment.Right);
            listView.Columns.Add("Полный путь", 250, HorizontalAlignment.Left);

            int iconIndex = 0;
            listView.BeginUpdate();
            foreach (DirectoryContents content in directoryContents)
            {
                if (content.Icon != null)
                {
                    imageList.Images.Add(content.Icon);
                }

                StorageSize size = ConvertSizeService.ConvertSize(content.Size);
                
                ListViewItem item = new ListViewItem("", iconIndex);
                item.SubItems.Add(content.Name);
                item.SubItems.Add($"{size.GetSizeByType(size.GetMaxSizeType())}");
                item.SubItems.Add(content.FullPath);
                item.Tag = content.FullPath;
                listView.Items.Add(item);
                iconIndex++;
            }
            listView.EndUpdate();

            listView.ColumnWidthChanging += OnColumnWidthChanging;

            splitContainer.Panel1.Controls.Add(listView);
            container.Controls.Add(splitContainer);

            flowLayoutPanelInfoDrives.Controls.Add(container);

            flowLayoutPanelInfoDrives.SizeChanged += FlowLayoutPanel_SizeChanged;

            void OnColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
            {
                if (e.ColumnIndex == 0)
                {
                    e.Cancel = true;
                    e.NewWidth = listView.Columns[e.ColumnIndex].Width;
                }
            }

            void FlowLayoutPanel_SizeChanged(object sender, EventArgs e)
            {
                if (container != null && !container.IsDisposed)
                {
                    container.Width = flowLayoutPanelInfoDrives.ClientSize.Width;
                    container.Height = Math.Max(500, flowLayoutPanelInfoDrives.ClientSize.Height - 20);
                }
            }

            FlowLayoutPanel_SizeChanged(null, EventArgs.Empty);
        }
        //private void ShowCheckedDriveForSizeInfo(DriveInfo dInfo)
        //{
        //    if (dInfo == null) { return; }

        //    long totalBytes = dInfo.TotalSize;
        //    long freeBytes = dInfo.TotalFreeSpace;
        //    long usedBytes = totalBytes - freeBytes;

        //    StorageSize total = ConvertSizeService.ConvertSize(totalBytes);
        //    StorageSize free = ConvertSizeService.ConvertSize(freeBytes);
        //    StorageSize used = ConvertSizeService.ConvertSize(usedBytes);

        //    int percentUsed = totalBytes > 0 ? (int)((double)usedBytes / totalBytes * 100) : 0;

        //    FlowLayoutPanel diskPanel = new FlowLayoutPanel
        //    {
        //        FlowDirection = FlowDirection.TopDown,
        //        AutoSize = true,

        //        BackColor = ApplicationSettings.BackgroundColor,
        //        BorderStyle = BorderStyle.FixedSingle,
        //        Padding = new Padding(10),
        //    };

        //    Label diskLabel = new Label
        //    {
        //        Text = dInfo.DriveType == DriveType.Fixed
        //            ? $"Локальный диск ({dInfo.Name.Replace(":\\", ":")})"
        //            : dInfo.Name.Replace(":\\", ":"),

        //        Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
        //        ForeColor = ApplicationSettings.TextColor,
        //        AutoSize = true,
        //        Width = 360,
        //        TextAlign = ContentAlignment.MiddleCenter,
        //        Margin = new Padding(0, 0, 0, 10)
        //    };

        //    Label driveSpaceInfo = new Label
        //    {      

        //        ForeColor = ApplicationSettings.TextColor,
        //        AutoSize = true,
        //        Margin = new Padding(0, 0, 0, 10),
        //        Text = $"{free.GetSizeByType(free.GetMaxSizeType())} свободно из {total.GetSizeByType(free.GetMaxSizeType())}"
        //    };

        //    CustomProgressBar progressBar = new CustomProgressBar
        //    {
        //        BackColor = Color.White,
        //        Minimum = 0,
        //        Maximum = 100,
        //        Value = percentUsed,
        //        Size = new Size(220, 18),
        //        Style = ProgressBarStyle.Continuous,
        //    }; ;

        //    diskPanel.Controls.Add(diskLabel);
        //    diskPanel.Controls.Add(driveSpaceInfo);
        //    diskPanel.Controls.Add(progressBar);

        //    flowLayoutPanelInfoDrives.Controls.Add(diskPanel);
        //}

        /// <summary>
        /// Отображение выбранного диска и доступных опций очистки
        /// </summary>
        private void ShowCheckedDriveForCleaning(DriveInfo dInfo, List<CleaningInformation> cleaningInformation)
        {
            if (dInfo == null || cleaningInformation == null) return;

            _selectedCleaning.Clear();

            FlowLayoutPanel diskPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                WrapContents = false,
                BackColor = ApplicationSettings.BackgroundColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
            };

            // ===== НАЗВАНИЕ ДИСКА (ПО ЦЕНТРУ) =====
            Label diskLabel = new Label
            {
                Text = dInfo.DriveType == DriveType.Fixed
                    ? $"Локальный диск ({dInfo.Name.Replace(":\\", ":")})"
                    : dInfo.Name.Replace(":\\", ":"),

                Font = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold),
                ForeColor = ApplicationSettings.TextColor,

                AutoSize = false,
                Width = 360,
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 10)
            };

            diskPanel.Controls.Add(diskLabel);
            // ====================================

            // ===== ЕСЛИ ОПЦИЙ НЕТ =====
            if (cleaningInformation.Count == 0)
            {
                Label noOptionsLabel = new Label
                {
                    Text = "Нет доступных опций очистки",
                    ForeColor = Color.Gray,
                    AutoSize = false,
                    Width = 360,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 10, 0, 5)
                };

                diskPanel.Controls.Add(noOptionsLabel);
                flowLayoutPanelInfoDrives.Controls.Add(diskPanel);
                return;
            }
            // =========================

            // ===== ПОЛЕ СУММАРНОГО РАЗМЕРА ВСЕГО ДЛЯ ОЧИСТКИ =====
            StorageSize totalSize = new StorageSize();
            foreach (var cleanInfo in cleaningInformation)
            {
                if (cleanInfo?.Size != null)
                    totalSize.AddSize(cleanInfo.Size);
            }

            FlowLayoutPanel totalSizePanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = ApplicationSettings.BackgroundColor,
                Font = new Font(FontFamily.GenericSansSerif, 10,
                FontStyle.Bold),
            };
            Label labelTextAvailableForClear = new Label
            {
                Text = "Доступно для очистки (всего) :",
                AutoSize = true,
                ForeColor = ApplicationSettings.TextColor
            };
            Label availableForClear = new Label
            {
                Text = $"[ {totalSize.GetSizeByType(totalSize.GetMaxSizeType())} ]",
                AutoSize = true,
                ForeColor = totalSize.GetColorBySize(),
            };
            totalSizePanel.Controls.Add(labelTextAvailableForClear);
            totalSizePanel.Controls.Add(availableForClear);
            diskPanel.Controls.Add(totalSizePanel);

            // ===== ОПЦИИ ОЧИСТКИ =====
            foreach (var cleanInfo in cleaningInformation)
            {
                if (cleanInfo == null) continue;

                FlowLayoutPanel optionPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    WrapContents = false,
                    BackColor = ApplicationSettings.BackgroundColor,
                    Margin = new Padding(5, 0, 0, 5)
                };

                CheckBox mainCheckBox = new CheckBox
                {
                    Text = cleanInfo.SectionName,
                    AutoSize = true,
                    ForeColor = ApplicationSettings.TextColor
                };

                Label sizeLabel = new Label
                {
                    Text = $"[{cleanInfo.Size.GetSizeByType(cleanInfo.Size.GetMaxSizeType()) ?? "0"}]",
                    AutoSize = true,
                    ForeColor = cleanInfo.Size.GetColorBySize(),
                    Margin = new Padding(5, 4, 0, 0)
                };

                FlowLayoutPanel header = new FlowLayoutPanel
                {
                    AutoSize = true,
                    WrapContents = false
                };
                header.Controls.Add(mainCheckBox);
                header.Controls.Add(sizeLabel);
                optionPanel.Controls.Add(header);

                FlowLayoutPanel subContainer = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    WrapContents = false,
                    Visible = false,
                    Margin = new Padding(25, 0, 0, 0)
                };

                if (!cleanInfo.IsSingleItem && cleanInfo.SubItems != null)
                {
                    foreach (var sub in cleanInfo.SubItems)
                    {
                        if (sub == null) continue;

                        FlowLayoutPanel subHeader = new FlowLayoutPanel
                        {
                            FlowDirection = FlowDirection.LeftToRight,
                            AutoSize = true,
                            WrapContents = false
                        };

                        CheckBox subCheck = new CheckBox
                        {
                            Text = sub.SectionName,
                            AutoSize = true,
                            ForeColor = ApplicationSettings.TextColor,
                            Margin = new Padding(0, 2, 5, 0)
                        };

                        Label subSizeLabel = new Label
                        {
                            Text = $"[{sub.Size?.GetSizeByType(sub.Size.GetMaxSizeType()) ?? "0"}]",
                            AutoSize = true,
                            ForeColor = sub.Size?.GetColorBySize() ?? ApplicationSettings.TextColor,
                            Margin = new Padding(0, 4, 0, 0)
                        };

                        subCheck.CheckedChanged += (_, _) =>
                            SetSubSelected(sub, subCheck.Checked);

                        subHeader.Controls.Add(subCheck);
                        subHeader.Controls.Add(subSizeLabel);

                        subContainer.Controls.Add(subHeader);
                    }

                }

                mainCheckBox.CheckedChanged += (_, _) =>
                {
                    if (cleanInfo.IsSingleItem)
                    {
                        if (cleanInfo.SingleItem != null)
                            SetSubSelected(cleanInfo.SingleItem, mainCheckBox.Checked);
                    }
                    else
                    {
                        subContainer.Visible = mainCheckBox.Checked;

                        foreach (Control c in subContainer.Controls)
                            if (c is CheckBox cb)
                                cb.Checked = mainCheckBox.Checked;
                    }
                };

                void SetSubSelected(SubCleaningInformation sub, bool selected)
                {
                    if (selected)
                    {
                        if (!_selectedCleaning.Contains(sub))
                            _selectedCleaning.Add(sub);
                    }
                    else
                    {
                        _selectedCleaning.Remove(sub);
                    }
                }

                optionPanel.Controls.Add(subContainer);
                diskPanel.Controls.Add(optionPanel);
            }

            flowLayoutPanelInfoDrives.Controls.Add(diskPanel);
        }
        /// <summary>
        /// Очистка панели с информацией о дисках
        /// </summary>
        public void ClearInfoDrives()
        {
            flowLayoutPanelInfoDrives.Controls.Clear();
        }

    }
}
