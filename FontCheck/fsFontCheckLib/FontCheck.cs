using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsFontCheckLib
{
    public class FontCheck
    {
        List<string> GetFontList(string filePath)
        {
            List<string> lstFont = new List<string>();
            fromPDFFile(filePath);

            return lstFont;

        }

        public static void fromPDFFile(string filePath)
        {
            PdfReader reader = new PdfReader(filePath);
            HashSet<String> names = new HashSet<string>();
            PdfDictionary resources;
            for (int p = 1; p <= reader.NumberOfPages; p++)
            {
                PdfDictionary dic = reader.GetPageN(p);
                resources = dic.GetAsDict(PdfName.RESOURCES);

                if (resources != null)
                {
                    //get fonts dictionary
                    PdfDictionary fonts = resources.GetAsDict(PdfName.FONT);
                    if (fonts != null)
                    {
                        PdfDictionary font;
                        foreach (PdfName key in fonts.Keys)
                        {
                            font = fonts.GetAsDict(key);
                            String name = font.GetAsName(PdfName.BASEFONT).ToString();

                            //check for prefix subsetted font
                            if (name.Length > 8 && name.ToCharArray()[7] == '+')
                            {
                                name = String.Format("%s subset (%s)", name.Substring(8), name.Substring(1, 7));

                            }
                            else
                            {
                                //get type of fully embeded fonts
                                name = name.Substring(1);
                                PdfDictionary desc = font.GetAsDict(PdfName.FONTDESCRIPTOR);
                                if (desc == null)
                                    name += " no font descriptor";
                                else if (desc.Get(PdfName.FONTFILE) != null)
                                    name += " (Type 1) embedded";
                                else if (desc.Get(PdfName.FONTFILE2) != null)
                                    name += " (TrueType) embedded";
                                else if (desc.Get(PdfName.FONTFILE3) != null)
                                    name += " (" + font.GetAsName(PdfName.SUBTYPE).ToString().Substring(1) + ") embedded";
                            }
                            names.Add(name);
                        }

                    }
                }
            }
            var collections = from name in names
                              select name;
            foreach (String fname in collections)
            {
               Trace.WriteLine(fname);
            }
            //Console.Read();
        }
    }
}
