using Microsoft.VisualStudio.TestTools.UnitTesting;
using fsFontCheckLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsFontCheckLib.Tests
{
    [TestClass()]
    public class FontCheckTests
    {
        [DataTestMethod]
        [DataRow(@"..\..\SampleFiles\A Sample PDF.pdf",3)]
        [DataRow(@"..\..\SampleFiles\SkypeClient.pdf", 9)]
        public void fromPDFFileTest(string filepath, int fontCount)
        { 
            var fonts = FontCheck.fromPDFFileSf(filepath);
            Assert.AreEqual(fonts.Count, fontCount);
        }
         

        /// <summary>
        /// Syncfusion DocIO Test
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="fontCount"></param>
        [DataTestMethod]        
        [DataRow(@"..\..\SampleFiles\1.doc", 2)]
        [DataRow(@"..\..\SampleFiles\2.docx", 1)]
        public void fromWordFileTestDocIO(string filepath, int fontCount)
        {
            var fonts = FontCheck.fromWordSf(filepath);
            Assert.AreEqual(fonts.Count, fontCount);
        }

        /// <summary>
        /// Syncfusion XlsIO Test
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="fontCount"></param>
        [DataTestMethod]
        [DataRow(@"..\..\SampleFiles\1.xls", 4)]
        [DataRow(@"..\..\SampleFiles\2.xls", 1)]
        public void fromXlsFileSf_Test(string filepath, int fontCount)
        {
            var fonts = FontCheck.fromXlsFileSf(filepath);
            Assert.AreEqual(fonts.Count, fontCount);
        }

        /// <summary>
        /// Syncfusion Presentation Test
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="fontCount"></param>
        [DataTestMethod]
        [DataRow(@"..\..\SampleFiles\1.pptx", 1)]
        [DataRow(@"..\..\SampleFiles\2.pptx", 4)]
        public void fromPptFileSf_Test(string filepath, int fontCount)
        {
            if(filepath.ToUpper().Substring(filepath.Length-4,4) == ".PPT")
            {
                filepath = spFontCheckLib.ConvertPPTtoPPTX(filepath);
            }
            var fonts = FontCheck.fromPptFileSf(filepath);
            Assert.AreEqual(fonts.Count, fontCount);
        }
    }
}