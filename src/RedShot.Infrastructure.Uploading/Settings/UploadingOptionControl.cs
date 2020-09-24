using Eto.Drawing;
using Eto.Forms;
using RedShot.Infrastructure.Abstractions.Uploading;
using RedShot.Infrastructure.Common.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedShot.Infrastructure.Uploading.Settings
{
    internal class UploadingOptionControl : Panel
    {
        private readonly UploadingConfiguration uploadingConfiguration;
        private readonly IEnumerable<UploadingDataGridObject> uploadingDataGridObjects;
        private GridView uploadersGridView;
        private CheckBox autoUploadCheckBox;

        public UploadingOptionControl(UploadingConfiguration uploadingConfiguration)
        {
            this.uploadingConfiguration = uploadingConfiguration;
            uploadingDataGridObjects = UploadingOptionControlHelper.GetUploadingDataGridObjects(UploadingManager.GetUploadingServices(UploadingManager.GetAllUploadingTypes()), uploadingConfiguration);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            autoUploadCheckBox = GetAutoUploadCheckBox();
            uploadersGridView = GetUploadersGridView();

            uploadersGridView.Bind(u => u.Enabled, autoUploadCheckBox, c => c.Checked, DualBindingMode.OneWay);

            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                Padding = 30,
                Spacing = 10,
                Items =
                {
                    FormsHelper.GetCheckBoxStack(autoUploadCheckBox, "Enable auto upload mode"),
                    uploadersGridView
                }
            };
        }

        private CheckBox GetAutoUploadCheckBox()
        {
            var checkBox = new CheckBox();
            checkBox.CheckedBinding.Bind(uploadingConfiguration, u => u.AutoUpload);

            return checkBox;
        }

        private GridView GetUploadersGridView()
        {
            var gridView = new GridView()
            {
                Size = new Size(250, 200)
            };

            gridView.Columns.Add(new GridColumn
            {
                DataCell = new TextBoxCell
                {
                    Binding = new DelegateBinding<UploadingDataGridObject, string>(r => r.UploaderName)
                },
                HeaderText = "Uploader name",
                Editable = false,
                Width = 150
            });

            gridView.Columns.Add(new GridColumn
            {
                DataCell = new CheckBoxCell
                {
                    Binding = new DelegateBinding<UploadingDataGridObject, bool?>(r => r.AutoUploadSelected, (u, v) => u.AutoUploadSelected = v ?? false),
                },
                HeaderText = "Auto upload",
                Editable = true,
                Width = 100
            });

            gridView.CellEdited += GridViewCellEdited;
            gridView.DataStore = uploadingDataGridObjects;

            return gridView;
        }

        private void GridViewCellEdited(object sender, GridViewCellEventArgs e)
        {
            UploadingOptionControlHelper.SetUploadersTypesOnAutoSelect(uploadingDataGridObjects, uploadingConfiguration);
        }
    }
}
