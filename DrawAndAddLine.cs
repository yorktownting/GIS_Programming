﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;

namespace GIS_Programming
{
    class DrawAndAddLine
    {
        protected ESRI.ArcGIS.Display.INewLineFeedback m_lineFeedback;
        protected IMap m_map;
        protected IActiveView m_focusMap;

        public DrawAndAddLine(IMap m_map)
        {
            m_focusMap = m_map as IActiveView;
        }

        public void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            IPoint point = m_focusMap.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y) as IPoint;

            if (m_lineFeedback == null)
            {
                m_lineFeedback = new ESRI.ArcGIS.Display.NewLineFeedback();
                m_lineFeedback.Display = m_focusMap.ScreenDisplay;
                m_lineFeedback.Start(point);
            }

            m_lineFeedback.AddPoint(point);
            m_lineFeedback.MoveTo(point);
            
        }
        public void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            // TODO: Add WxCrossSectionTool.OnMouseMove implementation
            IPoint point = m_focusMap.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y) as IPoint;
            if(m_lineFeedback != null)
                m_lineFeedback.MoveTo(point);
        }
        public IPolyline OnDoubleClick(int Button, int Shift, int X, int Y)
        {
            //首先会先触发Down，然后执行下面的程序：
            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            IPolyline polyline = null;
            if (m_lineFeedback != null)
            {
                polyline = m_lineFeedback.Stop();
            }

            m_focusMap.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);

            m_lineFeedback = null;

            return polyline;
        }
    }
}
    