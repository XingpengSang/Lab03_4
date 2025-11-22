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
            LoadFeatureAttributes(); // 加载属性
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
       
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int i;
                IField field;
                foreach (DataGridViewRow row in this.dgvFields.Rows)
                {
                    i = (int)row.Tag;
                    field = _feature.Fields.Field[i];
                    if (field.Type == esriFieldType.esriFieldTypeString)
                        _feature.Value[i] = row.Cells[1].Value.ToString();
                    else if (field.Type == esriFieldType.esriFieldTypeInteger
                         || field.Type == esriFieldType.esriFieldTypeSmallInteger
                         )
                        _feature.Value[i] = int.Parse(row.Cells[1].Value.ToString());
                    else if (field.Type == esriFieldType.esriFieldTypeSingle
                          || field.Type == esriFieldType.esriFieldTypeDouble)
                        _feature.Value[i] = Single.Parse(row.Cells[1].Value.ToString());
                    else
                        _feature.Value[i] = row.Cells[1].Value;
                }
                _feature.Store();

                MessageBox.Show("要素修改成功！",
"提示",
MessageBoxButtons.OK,
MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("[异常]" + ex.Message,
"错误",
MessageBoxButtons.OK,
MessageBoxIcon.Error);
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormEditFeature_Load(object sender, EventArgs e)
        {

        }
    }
}
