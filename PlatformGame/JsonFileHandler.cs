using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformGame
{
    static class JsonFileHandler
    {
        private static JObject wholeObject;
        private static string jsonFileName;

        public static Rectangle AllInOneRec(string fileName, string propertyName)
        {
            if (wholeObject == null || jsonFileName == null || jsonFileName != fileName)
            {
                jsonFileName = fileName;
                StreamReader file = File.OpenText(fileName);
                JsonTextReader reader = new JsonTextReader(file);
                wholeObject = JObject.Load(reader);
            }
            JObject obj = (JObject)wholeObject.GetValue(propertyName);

            int x = Convert.ToInt32(obj.GetValue("positionX"));
            int y = Convert.ToInt32(obj.GetValue("postitionY"));
            int width = Convert.ToInt32(obj.GetValue("width"));
            int height = Convert.ToInt32(obj.GetValue("height"));
            Rectangle rec = new Rectangle(x, y, width, height);
            return rec;
        }

        public static List<Rectangle> AllInOneRecList(string fileName, string propertyName)
        {
            if (wholeObject == null || jsonFileName == null || jsonFileName != fileName)
            {
                jsonFileName = fileName;
                using StreamReader file = File.OpenText(fileName);
                using JsonTextReader reader = new JsonTextReader(file);
                wholeObject = JObject.Load(reader);
            }

            List<Rectangle> recList = new List<Rectangle>();

            JArray arrayObject = (JArray)wholeObject[propertyName];

            foreach (JObject obj in arrayObject)
            {
                int x = (int)obj["positionX"];
                int y = (int)obj["positionY"]; //
                int width = (int)obj["width"];
                int height = (int)obj["height"];

                recList.Add(new Rectangle(x, y, width, height));
            }

            return recList;
        }

        //public static List<Rectangle> AllInOneRecList (string fileName, string propertyName)
        //{
        //    if (wholeObject == null || jsonFileName == null || jsonFileName != fileName)
        //    {
        //        jsonFileName = fileName;
        //        StreamReader file =  File.OpenText(fileName);
        //        JsonTextReader reader = new JsonTextReader(file);
        //        wholeObject = JObject.Load(reader);
        //    }

        //    List<Rectangle> recList = new List<Rectangle>();
        //    JArray arrayObject = (JArray)wholeObject.GetValue(propertyName);
        //    for(int i=0;i<arrayObject.Count;i++)
        //    {
        //        JObject obj = (JObject)wholeObject.GetValue(propertyName);

        //        int x = Convert.ToInt32(obj.GetValue("positionX"));
        //        int y = Convert.ToInt32(obj.GetValue("positionY"));
        //        int width = Convert.ToInt32(obj.GetValue("width"));
        //        int height = Convert.ToInt32(obj.GetValue("height"));
        //        Rectangle rec = new Rectangle(x, y, width, height);
        //        recList.Add(rec);
        //    }
        //    return recList;

        //}

        //public void ReadFromFile(string fileName)
        //{
        //    List<Rectangle> platformList = AllInOneRecList(fileName, "platforms");
        //    foreach (Rectangle rec in platformList)
        //    {
        //        Platform platform = new Platform(rec);
        //    }
        //}
    }
}
