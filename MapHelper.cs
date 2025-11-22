using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;

namespace Lab03_4
{
    public class MapHelper
    {
        private AxMapControl _axMap;
        public MapHelper(AxMapControl axMap)
        {
            _axMap = axMap; // 接收仓库主窗体的axMap控件
        }
        // 选择要素方法（适配仓库图层对象）
        public IFeature SelectFeature(IFeatureLayer layer, MapOperatorType opType, IMapControlEvents2_OnMouseDownEvent e)
        {
            IFeature selectedFeature = null; // 存储选中的要素
            ISpatialFilter spatialFilter = new SpatialFilterClass(); // 空间过滤器（用于查询要素）

            try
            {
                // 1. 根据操作类型，执行不同的选择逻辑
                switch (opType)
                {
                    // 点选要素（对应 EditFeatureByLocation / IdentifyFeature）
                    case MapOperatorType.EditFeatureByLocation:
                    case MapOperatorType.IdentifyFeature:
                        // 步骤1：将鼠标屏幕坐标转为地图坐标（获取点击点）
                        IPoint clickPoint = _axMap.ToMapPoint(e.x, e.y);
                        // 步骤2：设置空间过滤器（查询包含该点的要素）
                        spatialFilter.Geometry = clickPoint; // 过滤几何：点击点
                        spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains; // 空间关系：包含
                        break;

                    // 框选要素（对应 EditFeatureByRectangle）
                    case MapOperatorType.EditFeatureByRectangle:
                        // 步骤1：让用户在地图上拖动鼠标，绘制选择矩形
                        IEnvelope selectEnvelope = _axMap.TrackRectangle();
                        // 步骤2：设置空间过滤器（查询与矩形相交的要素）
                        spatialFilter.Geometry = selectEnvelope; // 过滤几何：选择矩形
                        spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; // 空间关系：相交
                        break;

                    // 其他操作类型（暂不处理）
                    default:
                        return null;
                }

                // 2. 执行空间查询，获取要素
                IFeatureCursor featureCursor = layer.FeatureClass.Search(spatialFilter, false);
                // 取第一个要素（框选时即使多个，也只返回第一个，符合指导书要求）
                selectedFeature = featureCursor.NextFeature();

                // 3. 高亮显示选中的要素（在地图上可视化选中效果）
                if (selectedFeature != null)
                {
                    _axMap.Map.SelectFeature(layer, selectedFeature); // 选中要素
                    _axMap.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null); // 刷新地图显示
                }

                // 4. 释放COM对象（ArcEngine必须手动释放，避免内存泄漏）
                Marshal.ReleaseComObject(featureCursor);
            }
            catch (Exception ex)
            {
                // 捕获异常并提示
                MessageBox.Show($"选择要素失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return selectedFeature; // 返回选中的要素（无选中则返回null）
        }

    }
}
