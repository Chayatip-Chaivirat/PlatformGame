using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System;

namespace PlatformGame
{
    static class JsonFileHandler
    {
        private static JObject platformObject;
        private static string platformFileName;

        public static Rectangle AllInOneRec (string fileName, string propertyName)
        {
            if (platformObject == null || platformFileName == null || platformFileName != fileName)
            {
                platformFileName = fileName;
                StreamReader file =  File.OpenText(fileName);
                JsonTextReader reader = new JsonTextReader(file);
                platformObject = JObject.Load(reader);
            }

            JObject obj = (JObject)platformObject.GetValue(propertyName);

            int x = Convert.ToInt32(obj.GetValue("positionX"));
            int y = Convert.ToInt32(obj.GetValue("postitionY"));
            int width = Convert.ToInt32(obj.GetValue("width"));
            int height = Convert.ToInt32(obj.GetValue("height"));

            Rectangle rec = new Rectangle(x, y, width, height);
            return rec;

        }
    }
}
