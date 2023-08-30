#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Module03_v2.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace Module03_v2
{
[Transaction(TransactionMode.Manual)]
    public class Command1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // this is a variable for the Revit application
            UIApplication uiapp = commandData.Application;

            // this is a variable for the current Revit model
            Document doc = uiapp.ActiveUIDocument.Document;

            // EXTRACT LIST OF STRING ARRAYS AND REMOVE HEADERS
            List<string[]> furnitureTypeList = FurnitureTypesInput();
            List<string[]> furnitureSetList = FurnitureSetsInput();
            furnitureTypeList.RemoveAt(0);
            furnitureSetList.RemoveAt(0);

            // ROOM COLLECTOR
            FilteredElementCollector roomCollector = new FilteredElementCollector(doc);
            roomCollector.OfCategory(BuiltInCategory.OST_Rooms);

            // INSTANTIATE CLASSES AND POPULATE
            List<FurnitureTypeClass> furnitureType = new List<FurnitureTypeClass>();
            foreach (string[] currentFurnitureTypeArray in  furnitureTypeList)
            {
                FurnitureTypeClass currentFurnitureType = new FurnitureTypeClass(
                    currentFurnitureTypeArray[0], currentFurnitureTypeArray[1], currentFurnitureTypeArray[2]);
                furnitureType.Add(currentFurnitureType);
            }

            List<FurnitureSetClass> furnitureSet = new List<FurnitureSetClass>();
            foreach (string[] currentFurnitureSetArray in furnitureSetList)
            {
                FurnitureSetClass currentFurnitureSet = new FurnitureSetClass(
                    currentFurnitureSetArray[0], currentFurnitureSetArray[1], currentFurnitureSetArray[2]);
                furnitureSet.Add(currentFurnitureSet);
            }
            //RECOMMENDED COUNT TO BE PLACED OUTSIDE TRANSACTION
            double count = 0;

            //LOOP THROUGH ROOM
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Placing furniture in Rooms");

                foreach (SpatialElement currentRoom in roomCollector)
                {
                    //ROOM LOCATION POINT
                    LocationPoint roomLocationPoint = currentRoom.Location as LocationPoint;
                    XYZ furnitureInsertionPoint = roomLocationPoint.Point;
                    
                    string furnitureGetSetParameterValue = Utils.GetParameterValueAsString(currentRoom, "Furniture Set");

                    //FIND MATCHES
                    foreach (FurnitureSetClass curFurnitureSet in furnitureSet)
                    {
                        if (curFurnitureSet.FurnitureSet == furnitureGetSetParameterValue)
                        {
                            foreach(string furnitureElement in curFurnitureSet.IncludedFurniture)
                            {
                                foreach( FurnitureTypeClass curFurnitureType in furnitureType)
                                {
                                    if(furnitureElement.Trim() == curFurnitureType.FurnitureName)
                                    {
                                        FamilySymbol currentFamilySymbol = Utils.GetFamilySymbolByName(doc, curFurnitureType.RevitFamilyName, 
                                            curFurnitureType.RevitFamilyType);

                                        if (currentFamilySymbol != null)
                                        {
                                            if(currentFamilySymbol.IsActive == false)
                                            {
                                                currentFamilySymbol.Activate();
                                            }
                                        }
                                        FamilyInstance currentFamilyInstance = doc.Create.NewFamilyInstance(furnitureInsertionPoint, 
                                            currentFamilySymbol, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                                        count++;
                                    }
                                }
                            }
                            Utils.SetParameterValue(currentRoom, "Furniture Count", curFurnitureSet.GetFurnitureCount());
                        }
                    }
                }

                t.Commit();
            }

            TaskDialog.Show("Complete", $"Inserted {count} pieces of furniture");
            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnCommand1";
            string buttonTitle = "Button 1";

            ButtonDataClass myButtonData1 = new ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Blue_32,
                Properties.Resources.Blue_16,
                "This is a tooltip for Button 1");

            return myButtonData1.Data;
        }

        private List<string[]> FurnitureTypesInput()
        {
            List<string[]> returnList = new List<string[]>();
            returnList.Add(new string[] { "Furniture Name", "Revit Family Name", "Revit Family Type" });
            returnList.Add(new string[] { "desk", "Desk", "60in x 30in" });
            returnList.Add(new string[] { "task chair", "Chair-Task", "Chair-Task" });
            returnList.Add(new string[] { "side chair", "Chair-Breuer", "Chair-Breuer" });
            returnList.Add(new string[] { "bookcase", "Shelving", "96in x 12in x 84in" });
            returnList.Add(new string[] { "loveseat", "Sofa", "54in" });
            returnList.Add(new string[] { "teacher desk", "Table-Rectangular", "48in x 30in" });
            returnList.Add(new string[] { "student desk", "Desk", "60in x 30in Student" });
            returnList.Add(new string[] { "computer desk", "Table-Rectangular", "48in x 30in" });
            returnList.Add(new string[] { "lab desk", "Table-Rectangular", "72in x 30in" });
            returnList.Add(new string[] { "lounge chair", "Chair-Corbu", "Chair-Corbu" });
            returnList.Add(new string[] { "coffee table", "Table-Coffee", "30in x 60in x 18in" });
            returnList.Add(new string[] { "sofa", "Sofa-Corbu", "Sofa-Corbu" });
            returnList.Add(new string[] { "dining table", "Table-Dining", "30in x 84in x 22in" });
            returnList.Add(new string[] { "dining chair", "Chair-Breuer", "Chair-Breuer" });
            returnList.Add(new string[] { "stool", "Chair-Task", "Chair-Task" });

            return returnList;
        }

        private List<string[]> FurnitureSetsInput()
        {
            List<string[]> returnList = new List<string[]>();
            returnList.Add(new string[] { "Furniture Set", "Room Type", "Included Furniture" });
            returnList.Add(new string[] { "A", "Office", "desk, task chair, side chair, bookcase" });
            returnList.Add(new string[] { "A2", "Office", "desk, task chair, side chair, bookcase, loveseat" });
            returnList.Add(new string[] { "B", "Classroom - Large", "teacher desk, task chair, student desk, student desk, student desk, student desk, student desk, student desk, student desk, student desk, student desk, student desk, student desk, student desk" });
            returnList.Add(new string[] { "B2", "Classroom - Medium", "teacher desk, task chair, student desk, student desk, student desk, student desk, student desk, student desk, student desk, student desk" });
            returnList.Add(new string[] { "C", "Computer Lab", "computer desk, computer desk, computer desk, computer desk, computer desk, computer desk, task chair, task chair, task chair, task chair, task chair, task chair" });
            returnList.Add(new string[] { "D", "Lab", "teacher desk, task chair, lab desk, lab desk, lab desk, lab desk, lab desk, lab desk, lab desk, stool, stool, stool, stool, stool, stool, stool" });
            returnList.Add(new string[] { "E", "Student Lounge", "lounge chair, lounge chair, lounge chair, sofa, coffee table, bookcase" });
            returnList.Add(new string[] { "F", "Teacher's Lounge", "lounge chair, lounge chair, sofa, coffee table, dining table, dining chair, dining chair, dining chair, dining chair, bookcase" });
            returnList.Add(new string[] { "G", "Waiting Room", "lounge chair, lounge chair, sofa, coffee table" });

            return returnList;
        }

    }
}
