 
using Syncfusion.DocIO.DLS;
using SfPDF = Syncfusion.Pdf.Parsing;
using Syncfusion.Presentation;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Diagnostics; 
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq; 

namespace fsFontCheckLib
{
    public class FontCheck
    { 
        /// <summary>
        /// Syncfusion Word ->DocIO
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> fromWordSf(string filePath)
        {
            HashSet<string> lstFont = new HashSet<string>();

            //Opens an existing document from file system through constructor of WordDocument class
            //using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (WordDocument doc = new WordDocument(filePath))
            {
                var fonttaable = doc.Document.FontSubstitutionTable;
                foreach (var font in fonttaable.Values)
                {
                    lstFont.Add(font);
                } 
            } 


            return lstFont.ToList();
        }
        /// <summary>
        /// Syncfusion Xls->XlsIO
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> fromXlsFileSf(string filePath)
        {
            HashSet<string> lstFont = new HashSet<string>();

            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            //Load the file into stream
            using (FileStream inputStream = new FileStream(filePath, FileMode.Open))
            {
                //Loads or open an existing workbook through Open method of IWorkbooks
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(inputStream);
                string standardfont = workbook.StandardFont;                 
               
                foreach (var sheet in workbook.Worksheets)
                {
                    foreach (var cell in sheet.UsedRange.Cells)
                    {
                        lstFont.Add(cell.CellStyle.Font.FontName);
                    }
                } 
            }

            return lstFont.ToList();
        }
        /// <summary>
        /// Syncfusion PPT->Presentation
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> fromPptFileSf(string filePath)
        {
            HashSet<string> lstFont = new HashSet<string>();

            //Opens an existing Presentation from file system 
            IPresentation ppt = Presentation.Open(filePath);
            foreach (var slide in ppt.Slides)
            {
                foreach (Syncfusion.Presentation.IShape shape in slide.Shapes)
                { 
                    if(shape is ITable)
                    {
                        ITable table = shape as ITable;
                        //Iterate all the cells in the table and gets the text 
                        foreach (IRow row in table.Rows)
                        {
                            foreach (ICell cell in row.Cells)
                            { 
                                foreach (IParagraph paragraph in cell.TextBody.Paragraphs)
                                {
                                    foreach (ITextPart textpart in paragraph.TextParts)
                                    {
                                        lstFont.Add(textpart.Font.FontName);
                                    }
                                }
                            }
                        }
                    }
                    else if(shape is ISmartArt)
                    {
                        ISmartArt smartArt = shape as ISmartArt;
                        foreach (ISmartArtNode node in smartArt.Nodes)
                        {
                            foreach (IParagraph paragraph in node.TextBody.Paragraphs)
                            {
                                foreach (ITextPart textpart in paragraph.TextParts)
                                {
                                    lstFont.Add(textpart.Font.FontName);
                                }
                            }
                        }
                    }
                    else
                    { 
                        foreach (IParagraph paragraph in shape.TextBody.Paragraphs)
                        {
                            foreach (ITextPart textpart in paragraph.TextParts)
                            {
                                lstFont.Add(textpart.Font.FontName);
                            }
                        }
                    } 
                }
            }
            ppt.Close(); 

            return lstFont.ToList();
        }


        /// <summary>
        /// Syncfusion PDF 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> fromPDFFileSf(string filePath)
        {
            HashSet<string> lstFont = new HashSet<string>();

            //Opens an existing document from file system through constructor of WordDocument class
            //using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (SfPDF.PdfLoadedDocument pdf = new SfPDF.PdfLoadedDocument(filePath))
            { 
                foreach (var font in pdf.UsedFonts)
                {
                    //Replace font 
                    //loadedDocument.UsedFonts[0].Replace(new PdfStandardFont(PdfFontFamily.Courier, 12));
                    //var utf = Encoding.UTF8;
                    //byte[] utfbyte = utf.GetBytes(font.Name);
                    //string strName = utf.GetString(utfbyte, 0, utfbyte.Length);


                    byte[] myByteArray = new byte[font.Name.Length];
                    for (int ix = 0; ix < font.Name.Length; ++ix)
                    {
                        char ch = font.Name[ix];
                        myByteArray[ix] = (byte)ch;
                    }

                    string strName   = Encoding.UTF8.GetString(myByteArray, 0, font.Name.Length);


                    lstFont.Add(strName);
                }
            }


            return lstFont.ToList();
        }
        

    }
}
