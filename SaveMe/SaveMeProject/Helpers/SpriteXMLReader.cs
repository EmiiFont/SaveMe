using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WaveEngine.Common.Math;
using WaveEngine.Components.Animation;

namespace SaveMeProject.Helpers
{
    class SpriteXMLReader : ISpriteSheetLoader
    {
        public Rectangle[] Parse(string path)
        {
            var spriteXML = XDocument.Load(path);
            var animation = spriteXML.Descendants(XName.Get("sprite")).ToList()
                .Select(x => new Rectangle
                                 {
                                     X = int.Parse(x.Attribute("x").Value),
                                     Y = int.Parse(x.Attribute("y").Value),
                                    Height = int.Parse(x.Attribute("h").Value),
                                    Width = int.Parse(x.Attribute("w").Value)

                                 }).ToArray();
          
            return animation;
        }
    }

    class SpriteXMLReaderRevese : ISpriteSheetLoader
    {

        public Rectangle[] Parse(string path)
        {
            var spriteXML = XDocument.Load(path);
            var animation = spriteXML.Descendants(XName.Get("sprite")).ToList()
                .Select(x => new Rectangle
                {
                    X = int.Parse(x.Attribute("x").Value),
                    Y = int.Parse(x.Attribute("y").Value),
                    Height = int.Parse(x.Attribute("h").Value),
                    Width = int.Parse(x.Attribute("w").Value)

                }).Reverse().ToArray();
            return animation;
        }
    }
}
