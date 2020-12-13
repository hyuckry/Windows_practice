using Spire.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsFontCheckLib
{
    public class spFontCheckLib
    {
        public static string ConvertPPTtoPPTX(string filePath)
        { 
            Presentation ppt = new Presentation();
            ppt.LoadFromFile(filePath);

            ppt.SaveToFile(filePath + ".pptx", FileFormat.Pptx2013);
           

            return filePath + ".pptx";
        }
    }
}
