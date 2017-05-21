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
using System.IO;

namespace thundax.excel.wrapper
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
}
