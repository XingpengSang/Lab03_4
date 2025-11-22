using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Lab03_4.MyForms;
using Lab03_4.MyForms.FeatureClassManagement.Services;
using System.Data;
using ESRI.ArcGIS.Geodatabase;


namespace Lab03_4
{
    public partial class Form_4 : Form
    {
        private MapOperatorType mapOp = MapOperatorType.Default;
        private ILayer m_selectedLayer; // 当前选中的图层
        IEnvelope ext = null; // 主地图显示范围

        public Form_4()
        {
            InitializeComponent();
            this.Load += Form_4_Load;
            InitializeFeatureClassManagement(); // Lab03_4新增
        }

        #region 窗体加载和初始化
        private void Form_4_Load(object sender, EventArgs e)
        {
            // 动态设置缩放大小
            menu.ImageScalingSize = new Size(16, 16);
            tool.ImageScalingSize = new Size(16, 16);

            //关闭滚轮缩放功能
            this.axThum.AutoMouseWheel = false;
        }
        #endregion

        #region 菜单-文件功能
        private void menuFileNew_Click(object sender, EventArgs e)
        {
            CreateNewMap();
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            executeCommandByProgId("esriControls.ControlsOpenDocCommand");
        }

        private void menuFileSave_Click(object sender, EventArgs e)
        {
            executeCommandByProgId("esriControls.ControlsSaveAsDocCommand");
        }

        private void menuFileCloseAll_Click(object sender, EventArgs e)
        {
            RemoveAll();
        }

        private void menuFileExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }
        #endregion

        #region 菜单-图层功能
        private void menuLayerAllShp_Click(object sender, EventArgs e)
        {
            LoadAllShapefiles();
        }

        private void menuLayerAddShp_Click(object sender, EventArgs e)
        {
            AddShapefile();
        }

        private void menuLayerRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedLayer();
        }

        private void menuLayerSelectable_Click(object sender, EventArgs e)
        {
            SetLayerSelectable();
        }

        private void menuLayerVisible_Click(object sender, EventArgs e)
        {
            ToggleLayerVisibility();
        }

        private void menuLayerThum_Click(object sender, EventArgs e)
        {
            AddLayerToThumbnail();
        }
        #endregion

        #region 菜单-帮助功能
        private void menuHelp_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 工具栏-图层
        private void tlbLayerAllShp_Click(object sender, EventArgs e)
        {
            LoadAllShapefiles();
        }

        private void tlbLayerAddShp_Click(object sender, EventArgs e)
        {
            AddShapefile();
        }

        private void tlbLayerRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedLayer();
        }

        private void tlbLayerSelectable_Click(object sender, EventArgs e)
        {
            SetLayerSelectable();
        }

        private void tlbLayerVisible_Click(object sender, EventArgs e)
        {
            ToggleLayerVisibility();
        }

        private void tlbLayerThum_Click(object sender, EventArgs e)
        {
            AddLayerToThumbnail();
        }
        #endregion

        #region TOC右键菜单功能
        private void tsmUp_Click(object sender, EventArgs e)
        {
            MoveLayerUp();
        }

        private void tsmDown_Click(object sender, EventArgs e)
        {
            MoveLayerDown();
        }

        private void tsmRemove_Click(object sender, EventArgs e)
        {
            RemoveSelectedLayer();
        }

        private void tsmSelectable_Click(object sender, EventArgs e)
        {
            SetLayerSelectable();
        }

        private void tsmVisible_Click(object sender, EventArgs e)
        {
            ToggleLayerVisibility();
        }

        private void tsmThum_Click(object sender, EventArgs e)
        {
            AddLayerToThumbnail();
        }

        // TOC鼠标点击事件，记录选中的图层
        private void axTOC_OnMouseDown(object sender, ESRI.ArcGIS.Controls.ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2) // 右键
            {
                // 获取选中的图层
                m_selectedLayer = GetSelectedLayer();
                // 显示右键菜单
                if (m_selectedLayer != null)
                {
                    cmTOC.Show(axTOC, new System.Drawing.Point(e.x, e.y));
                }
            }
            else // 左键
            {
                m_selectedLayer = GetSelectedLayer();
                UpdateMenuStatus();
            }
        }
        #endregion

        // Lab03_4
        #region 菜单-要素类管理

        private void menuFeatureClassNew_Click(object sender, EventArgs e)
        {
            CreateNewFeatureClass();
        }

        private void menuFeatureClassEdit_Click(object sender, EventArgs e)
        {
            EditFeatureClassFields();
        }

        private void menuFeatureClassDelete_Click(object sender, EventArgs e)
        {
            DeleteFeatureClass();
        }

        #endregion

        #region 工具栏-要素类管理

        private void tlbFeatureClassNew_Click(object sender, EventArgs e)
        {
            CreateNewFeatureClass();
        }

        private void tlbFeatureClassEdit_Click(object sender, EventArgs e)
        {
            EditFeatureClassFields();
        }

        private void tlbFeatureClassDelete_Click(object sender, EventArgs e)
        {
            DeleteFeatureClass();
        }


        #endregion

        #region C部分
        private void menuFeatureBrowse_Click(object sender, EventArgs e)
        {
            BrowseFeatures();
        }
        private void tlbFeatureBrowse_Click(object sender, EventArgs e)
        {
            BrowseFeatures();
        }
        // 公共浏览方法（核心逻辑）
        private void BrowseFeatures()
        {
            // 复用仓库已有GetSelectedLayer()方法
            IFeatureLayer selectedLayer = GetSelectedLayer() as IFeatureLayer;
            if (selectedLayer == null || selectedLayer.FeatureClass == null)
            {
                MessageBox.Show("请先在TOC中选择要素图层！", "提示");
                return;
            }
            // 调用工具类转换数据
            DataTable dt = FeatureHelper.ToDataTable(selectedLayer.FeatureClass);
            // 打开新增的浏览窗体
            MyForms.FormBrowseFeatures frm = new MyForms.FormBrowseFeatures();
            frm.dgvFeatures.DataSource = dt;
            frm.ShowDialog();
        }

        private void menuFeatureIdentify_Click(object sender, EventArgs e)
        {
            mapOp = MapOperatorType.IdentifyFeature;
            MessageBox.Show("请在地图上点击要素查看信息！", "提示");
        }
        private void tlbFeatureIdentify_Click(object sender, EventArgs e)
        {
            menuFeatureIdentify_Click(sender, e);
        }

        //点选编辑
        private void menuFeatureEditByLocation_Click(object sender, EventArgs e)
        {
            mapOp = MapOperatorType.EditFeatureByLocation;
            MessageBox.Show("请点击地图要素进行编辑！", "提示");
        }

        //框选编辑
        private void menuFeatureEditByRectangle_Click(object sender, EventArgs e)
        {
            mapOp = MapOperatorType.EditFeatureByRectangle;
            MessageBox.Show("请框选地图要素进行编辑！", "提示");
        }
        private void tlbFeatureEditByLocation_Click(object sender, EventArgs e)
        {
            menuFeatureEditByLocation_Click(sender, e);
        }

    
        private void axMap_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            IFeatureLayer selectedLayer = GetSelectedLayer() as IFeatureLayer;
            if (selectedLayer == null) return;
            MapHelper mapHelper = new MapHelper(this.axMap); // 传递仓库的axMap
                                                             // 要素信息（你的模块）
            if (mapOp == MapOperatorType.IdentifyFeature)
            {
                this.axMap.Map.ClearSelection();
                IFeature feature = mapHelper.SelectFeature(selectedLayer, mapOp, e);
                if (feature != null)
                {
                    new MyForms.FormIdentify(feature).ShowDialog();
                }
            }
            // 要素编辑（你的模块）
            else if (mapOp == MapOperatorType.EditFeatureByLocation || mapOp == MapOperatorType.EditFeatureByRectangle)
            {
                this.axMap.Map.ClearSelection();
                IFeature feature = mapHelper.SelectFeature(selectedLayer, mapOp, e);
                if (feature != null)
                {
                    new MyForms.FormEditFeature(feature).ShowDialog();
                    this.axMap.ActiveView.Refresh(); // 刷新仓库地图显示
                }
            }
            // 重置操作类型（避免冲突）
            mapOp = MapOperatorType.Default;
        }
    }
    #endregion


}