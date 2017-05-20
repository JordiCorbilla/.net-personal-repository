//  Copyright (c) 2017, Jordi Corbilla
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//
//  - Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//  - Neither the name of this library nor the names of its contributors may be
//    used to endorse or promote products derived from this software without
//    specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
//  ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
//  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
//  SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
//  INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
//  CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
//  POSSIBILITY OF SUCH DAMAGE.

using ClosedXML.Excel;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace myTestingStuff
{
    public class TestData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [TestClass]
    public class ExcelExportWrapperTest
    {
        private MemoryStream NormalExport()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Test");

                Collection<TestData> test = new Collection<TestData> {
                    new TestData { Id = 1, Name = "test1" },
                    new TestData { Id =2, Name = "test2" }
                };

                //Add Columns
                worksheet.Cell(1, 1).SetValue("Id").Style.Font.Bold = true;
                worksheet.Cell(1, 2).SetValue("Name").Style.Font.Bold = true;

                //Add content
                for (int i = 0; i < test.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).SetValue(test[i].Id);
                    worksheet.Cell(i + 2, 2).SetValue(test[i].Name);
                }

                worksheet.Columns().AdjustToContents();

                MemoryStream memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }

        [TestMethod]
        public void TestExport()
        {
            using (MemoryStream ms = NormalExport())
            {
                using (FileStream file = new FileStream("file.xlsx", FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, (int)ms.Length);
                    file.Write(bytes, 0, bytes.Length);
                    ms.Close();
                }
            }
        }
    }
}
