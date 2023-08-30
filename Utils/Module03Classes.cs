using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module03_v2.Classes
{
   public class FurnitureTypeClass
    {
        public string FurnitureName { get; set; }
        public string RevitFamilyName { get; set; }
        public string RevitFamilyType { get; set; }
        public FurnitureTypeClass( string _FurnitureName, string _RevitFamilyName, string _RevitFamilyType) 
        {
            FurnitureName = _FurnitureName;
            RevitFamilyName = _RevitFamilyName;
            RevitFamilyType = _RevitFamilyType;
        }
    }

    public class FurnitureSetClass
    {
        public string FurnitureSet { get; set; }
        public string RoomType { get; set;}
        public string[] IncludedFurniture { get; set; }

        public FurnitureSetClass (string _FurnitureSet, string _RoomType, string _IncludedFurniture)
        {
            FurnitureSet = _FurnitureSet;
            RoomType = _RoomType;
            IncludedFurniture = _IncludedFurniture.Split(',');
        }

        public int GetFurnitureCount()
        {
            return IncludedFurniture.Length;
        }
    }
}
