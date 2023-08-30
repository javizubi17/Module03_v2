using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module03_v2
{
    internal static class Utils
    {

        internal static string GetParameterValueAsString(Element element, string paramName)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter myParam = paramList.First();

            return myParam.AsString();
        }

        internal static double GetParameterValueAsDouble(Element element, string paramName)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter myParam = paramList.First();

            return myParam.AsDouble();
        }

        internal static void SetParameterValue(Element element, string paramName, string value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter param = paramList.First();

            param.Set(value);
        }
        internal static void SetParameterValue(Element element, string paramName, double value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter param = paramList.First();

            param.Set(value);
        }

        //internal static void SetParameterValue(Element element, string paramName, int value)
        //{
        //    IList<Parameter> paramList = element.GetParameters(paramName);
        //    Parameter param = paramList.First();

        //    param.Set(value);
        //}

        internal static void SetParameterValue(Element element, string paramName, ElementId value)
        {
            IList<Parameter> paramList = element.GetParameters(paramName);
            Parameter param = paramList.First();

            param.Set(value);
        }

        //OTHER ALTERNATIVES TO SET PARAMTER VALUE METHODS

        //METHOD 1 (best one)

        //internal static void SetParameterValue(Element element, string paramName, ElementId value)
        //{
        //    Parameter curParam = element.LookupParameter(paramName);
        //    if (curParam != null)
        //    {
        //        curParam.Set(value);
        //    }
        //}

        //METHOD 2

        //internal static void SetParameterValue(Element element, string paramName, ElementId value)
        //{
        //    foreach (Parameter curParam in element.Parameters)
        //    {
        //        if (curParam.Definition.Name == paramName)
        //        {
        //            curParam.Set(value);
        //        }
        //    }
        //}

        internal static FamilySymbol GetFamilySymbolByName(Document doc, string famName, string fsName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilySymbol));

            foreach (FamilySymbol fs in collector)
            {
                if (fs.Name == fsName && fs.FamilyName == famName)
                {
                    return fs;
                }
            }
            return null;
        }

        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel currentPanel = GetRibbonPanelByName(app, tabName, panelName);

            if (currentPanel == null)
                currentPanel = app.CreateRibbonPanel(tabName, panelName);

            return currentPanel;
        }

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

    }
}
