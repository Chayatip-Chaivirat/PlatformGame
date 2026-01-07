using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            int y = Convert.ToInt32(obj.GetValue("positionY"));
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
                int y = (int)obj["positionY"];
                int width = (int)obj["width"];
                int height = (int)obj["height"];

                recList.Add(new Rectangle(x, y, width, height));
            }

            return recList;
        }
        public static void WriteToJsonFile(string fileName, List<CollidableObject> gList)
        {
            JArray enemyArray = new JArray();
            JArray platformArray = new JArray();
            JObject bigObject = new JObject();

            for (int i = 0; i < gList.Count; i++)
            {
                if (gList[i] is Enemy)
                {
                    JObject obj = CreateObject(gList[i].hitBoxLive);
                    enemyArray.Add(obj);
                }
                else if (gList[i] is Platform)
                {
                    JObject obj = CreateObject(gList[i].hitBoxLive);
                    platformArray.Add(obj);
                }
                else if (gList[i] is Player)
                {
                    JObject obj = CreateObject(gList[i].hitBoxLive);
                    bigObject.Add("player",obj);
                }
            }
            bigObject.Add("enemies", enemyArray);
            bigObject.Add("platforms", platformArray);

            System.Diagnostics.Debug.WriteLine(bigObject.ToString());
            //File.Open("jsonfile.json", FileMode.Create);
            File.WriteAllText(fileName, bigObject.ToString());
        }

        private static JObject CreateObject(Rectangle rect)
        {
            JObject obj = new JObject();
            obj.Add("positionX", rect.X);
            obj.Add("positionY", rect.Y);
            obj.Add("width", rect.Width);
            obj.Add("height", rect.Height);

            return obj;
        }
    }
}