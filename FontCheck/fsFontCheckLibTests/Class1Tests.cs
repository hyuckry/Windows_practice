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
    public class Class1Tests
    {
        [TestMethod()]
        public void fromPDFFileTest()
        {
            A Sample PDF.pdf
            FontCheck.fromPDFFile(@"..\..\SampleFiles\A Sample PDF.pdf");
            Assert.Fail();
        }
    }
}