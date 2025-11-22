using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace Lab03_4.MyForms
{
    public partial class FormEditFeature : Form
    {
        private IFeature _feature; // 接收仓库传递的待编辑要素
        public FormEditFeature(IFeature feature)
        {
            InitializeComponent();
            _feature = feature;
            InitializeDataGridViewColumns(); // 初始化列
            LoadFeatureAttributes();

       
        }

        private void InitializeDataGridViewColumns()
        {
            // 添加第一列：字段别名（标题为"属性名称"）
            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.Name = "FieldName";
            colName.HeaderText = "属性名称";
            colName.ReadOnly = true; // 只读（不允许编辑字段名）
            dgvFields.Columns.Add(colName);

            // 添加第二列：字段值（标题为"属性值"）
            DataGridViewTextBoxColumn colValue = new DataGridViewTextBoxColumn();
            colValue.Name = "FieldValue";
            colValue.HeaderText = "属性值";
            colValue.ReadOnly = false; // 允许编辑值（如果需要修改属性）
            dgvFields.Columns.Add(colValue);
        }


        // 加载要素属性到表格
        private void LoadFeatureAttributes()
        {
            int j = 0;
            try
            {
                IFields fields = _feature.Fields;
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    if (fields.Field[i].Type == esriFieldType.esriFieldTypeOID
                        || fields.Field[i].Type == esriFieldType.esriFieldTypeGeometry)
                        continue;

                    //在表格中显示字段别名
                    this.dgvFields.Rows.Add(fields.Field[i].AliasName, _feature.Value[i]);
                    //在行的tag属性中记录字段序号，以便在创建要素数据时使用
                    this.dgvFields.Rows[j].Tag = i;
                    j++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("[异常] " + ex.Message,
"错误",
MessageBoxButtons.OK,
MessageBoxIcon.Error);
            }

        }
       

        private void FormEditFeature_Load(object sender, EventArgs e)
        {
           
        }

        private void btnOK_Click_1(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvFields.Rows)
                {
                    if (row.IsNewRow) continue;

                    int index = (int)row.Tag;
                    IField field = _feature.Fields.get_Field(index);
                    string cellValue = row.Cells[1].Value?.ToString() ?? "";

                    // 处理字段为空
                    if (string.IsNullOrWhiteSpace(cellValue))
                    {
                        if (field.IsNullable)
                        {
                            _feature.set_Value(index, DBNull.Value);
                            continue;
                        }
                        else
                        {
                            dgvFields.CurrentCell = row.Cells[1];
                            throw new ArgumentException($"字段 “{field.AliasName}” 不允许为空，请输入有效值。");
                        }
                    }

                    // 按字段类型解析并赋值
                    switch (field.Type)
                    {
                        case esriFieldType.esriFieldTypeString:
                            _feature.set_Value(index, cellValue);
                            break;

                        case esriFieldType.esriFieldTypeInteger:
                        case esriFieldType.esriFieldTypeSmallInteger:
                            if (int.TryParse(cellValue, out int intValue))
                                _feature.set_Value(index, intValue);
                            else
                            {
                                dgvFields.CurrentCell = row.Cells[1];
                                throw new ArgumentException($"字段 “{field.AliasName}” 必须为整数。");
                            }
                            break;

                        case esriFieldType.esriFieldTypeSingle:
                            if (float.TryParse(cellValue, out float floatValue))
                                _feature.set_Value(index, floatValue);
                            else
                            {
                                dgvFields.CurrentCell = row.Cells[1];
                                throw new ArgumentException($"字段 “{field.AliasName}” 必须为单精度浮点数。");
                            }
                            break;

                        case esriFieldType.esriFieldTypeDouble:
                            if (double.TryParse(cellValue, out double doubleValue))
                                _feature.set_Value(index, doubleValue);
                            else
                            {
                                dgvFields.CurrentCell = row.Cells[1];
                                throw new ArgumentException($"字段 “{field.AliasName}” 必须为双精度浮点数。");
                            }
                            break;

                        case esriFieldType.esriFieldTypeDate:
                            if (DateTime.TryParse(cellValue, out DateTime dateValue))
                                _feature.set_Value(index, dateValue);
                            else
                            {
                                dgvFields.CurrentCell = row.Cells[1];
                                throw new ArgumentException($"字段 “{field.AliasName}” 必须为有效日期（如 2022-01-01）。");
                            }
                            break;

                        default:
                            _feature.set_Value(index, row.Cells[1].Value);
                            break;
                    }
                }

                _feature.Store();
                MessageBox.Show("要素属性修改已保存 ✔", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[异常] " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
