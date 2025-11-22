using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab03_4
{
   
    public enum MapOperatorType
    {
        
        Default,
        /// <summary>
        /// 画点
        /// </summary>
        DrawPoint,
        /// <summary>
        /// 画线
        /// </summary>
        DrawPolyline,
        /// <summary>
        /// 画多边形
        /// </summary>
        DrawPolygon,

        /// <summary>
        /// 画矩形
        /// </summary>
        DrawRectangle,

        /// <summary>
        /// 从地图上创建点要素
        /// </summary>
        CreatePoint,
        /// <summary>
        /// 从地图上创建线要素
        /// </summary>
        CreatePolyline,
        /// <summary>
        /// 从地图上创建面要素
        /// </summary>
        CreatePolygon,

        /// <summary>
        /// 标识/显示要素信息
        /// </summary>
        IdentifyFeature,

        /// <summary>
        /// 点选要素
        /// </summary>
        SelectFeatureByLocation,
        /// <summary>
        /// 线选要素
        /// </summary>
        SelectFeatureByPolyline,
        /// <summary>
        /// 多边形选择要素
        /// </summary>
        SelectFeatureByPolygon,
        /// <summary>
        /// 框选要素
        /// </summary>
        SelectFeatureByRectangle,

        /// <summary>
        /// 点选编辑要素
        /// </summary>
        EditFeatureByLocation,
        /// <summary>
        /// 框选编辑要素
        /// </summary>
        EditFeatureByRectangle,

        /// <summary>
        /// 点选删除要素
        /// </summary>
        DeleteFeatureByLocation,
        /// <summary>
        /// 框选删除要素
        /// </summary>
        DeleteFeatureByRectangle,
        /// <summary>
        /// 多边形选择删除要素
        /// </summary>
        DeleteFeatureByPolygon
    }

}
