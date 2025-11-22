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
    public partial class FormIdentify : Form
    {
       
        public FormIdentify(IFeature feature)
        {
            InitializeComponent();
            // 加载要素属性到ListBox
            IFields fields = feature.Fields;
            for (int i = 0; i < fields.FieldCount; i++)
            {
                IField field = fields.get_Field(i);
                // 跳过OID和几何字段（仓库代码通用逻辑）
                if (field.Type == esriFieldType.esriFieldTypeOID || field.Type == esriFieldType.esriFieldTypeGeometry)
                    continue;
                listBoxInfo.Items.Add($"{field.AliasName}：{feature.get_Value(i) ?? "空"}");
            }
        }

        private void FormIdentify_Load(object sender, EventArgs e)
        {

        }
    }
}
