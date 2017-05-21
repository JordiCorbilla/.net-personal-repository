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

using System;
using thundax.myTestingStuff.Biz;
using ClosedXML.Excel;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace thundax.myTestingStuff
{
    public class ExcelWrapper<T>
    {
        public static MemoryStream GenerateStream(string worksheetName, Collection<T> data, Collection<string> filter, Collection<string> alias)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add(worksheetName);

                //Add Columns using Reflection - this is the header and it only needs to be run once
                int column = 1;
                int entry = 1;
                if (data.Count > 0)
                {
                    var item = data[0]; //get the first element of the list
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (filter.Contains(property.Name))
                        {
                            worksheet.Cell(entry, column).SetValue(property.Name).Style.Font.Bold = true;
                            column++;
                        }
                    }
                    entry++;

                    //Add content using Reflection
                    foreach (var row in data)
                    {
                        column = 1;
                        foreach (var property in row.GetType().GetProperties())
                        {
                            if (filter.Contains(property.Name))
                            {
                                object value = property.GetValue(row, null);
                                worksheet.Cell(entry, column).SetValue(value).Style.Font.Bold = true;
                                column++;
                            }
                        }
                        entry++;
                    }
                }
                else //just print the headers using the list that has been sent through
                {
                    column = 1;
                    foreach (var header in filter)
                    {
                        worksheet.Cell(entry, column).SetValue(header).Style.Font.Bold = true;
                        column++;
                    }
                }

                worksheet.Columns().AdjustToContents();

                MemoryStream memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                return memoryStream;
            }
        }
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
                    new TestData("dd/MM/yyyy") { Id = 1, Name = "test1", Date = DateTime.UtcNow },
                    new TestData("dd/MM/yyyy") { Id =2, Name = "test2", Date = DateTime.UtcNow }
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

        private MemoryStream NormalExportReflection(Collection<string> columns)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Test");

                Collection<TestData> test = new Collection<TestData> {
                    new TestData("dd/MM/yyyy") { Id = 1, Name = "test1", Date = DateTime.UtcNow },
                    new TestData("dd/MM/yyyy") { Id =2, Name = "test2", Date = DateTime.UtcNow }
                };

                //Add Columns using Reflection - this is the header and it only needs to be run once
                int column = 1;
                int entry = 1;
                if (test.Count > 0)
                {
                    var item = test[0]; //get the first element of the list
                    foreach (var property in item.GetType().GetProperties())
                    {
                        if (columns.Contains(property.Name))
                        {
                            worksheet.Cell(entry, column).SetValue(property.Name).Style.Font.Bold = true;
                            column++;
                        }
                    }
                    entry++;

                    //Add content using Reflection
                    foreach (var row in test)
                    {
                        column = 1;
                        foreach (var property in row.GetType().GetProperties())
                        {
                            if (columns.Contains(property.Name))
                            {
                                object value = property.GetValue(row, null);
                                worksheet.Cell(entry, column).SetValue(value).Style.Font.Bold = true;
                                column++;
                            }
                        }
                        entry++;
                    }
                }
                else //just print the headers using the list that has been sent through
                {
                    column = 1;
                    foreach (var header in columns)
                    {
                        worksheet.Cell(entry, column).SetValue(header).Style.Font.Bold = true;
                        column++;
                    }
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

        [TestMethod]
        public void TestExportReflection()
        {
            using (MemoryStream ms = NormalExportReflection(new Collection<string> { "Id", "Name", "DateFormatted" }))
            {
                using (FileStream file = new FileStream("fileReflection.xlsx", FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[ms.Length];
                    ms.Read(bytes, 0, (int)ms.Length);
                    file.Write(bytes, 0, bytes.Length);
                    ms.Close();
                }
            }
        }

        [TestMethod]
        public void TestExportReflectionClass()
        {
            Collection<TestData> data = new Collection<TestData> {
                    new TestData("dd/MM/yyyy") { Id = 1, Name = "test1", Date = DateTime.UtcNow },
                    new TestData("dd/MM/yyyy") { Id =2, Name = "test2", Date = DateTime.UtcNow }
                };

            using (MemoryStream ms = ExcelWrapper<TestData>.GenerateStream("test",
                data,
                new Collection<string> { "Id", "Name", "DateFormatted" },
                new Collection<string> { "Id", "Name", "Date" }))
            {
                using (FileStream file = new FileStream("fileReflectionClass.xlsx", FileMode.Create, FileAccess.Write))
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
