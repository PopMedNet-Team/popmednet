using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PopMedNet.DMCS.Code;
using PopMedNet.DMCS.Data;
using PopMedNet.DMCS.Data.Model;
using PopMedNet.DMCS.Models;
using PopMedNet.DMCS.PMNApi;
using PopMedNet.DMCS.PMNApi.PMNDto;
using PopMedNet.DMCS.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Controllers
{
    [Route("api/logs")]
    [ApiController, Authorize]
    public class LogsController : BaseApiController
    {
        public LogsController(Data.Model.ModelContext modelDb, ILogger logger)
            : base(modelDb, logger.ForContext("SourceContext", typeof(LogsController).FullName))
        {
        }


        [HttpGet, Route("global")]
        public async Task<IEnumerable<LogDTO>> GetGlobalLogs(int take = 50, int skip = 0)
        {
            return await (from log in this.modelDb.Logs.AsNoTracking()
                          where !log.ResponseID.HasValue
                          orderby log.DateTime descending
                          select new LogDTO
                          {
                              DateTime = log.DateTime,
                              Level = log.Level,
                              Message = log.Message,
                              Source = log.Source,
                              Exception = log.Exception
                          }).Skip(skip).Take(take).ToArrayAsync();
        }

        [HttpGet, Route("export")]
        public async Task<IActionResult> ExportGlobalLogs(bool exportAll, Guid? responseID, DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            var results = this.modelDb.Logs.AsNoTracking();

            if (responseID.HasValue)
                results = results.Where(x => x.ResponseID == responseID);
            else
                results = results.Where(x => !x.ResponseID.HasValue);

            if (!exportAll)
            {
                if (startDate.HasValue)
                {
                    results = results.Where(x => x.DateTime >= startDate.Value.Date);
                }
                if (endDate.HasValue)
                {
                    //12:00am the next day to encompass the entire day
                    var endD = endDate.Value.Date.AddDays(1);
                    results = results.Where(x => x.DateTime < endD);
                }
            }

            var stream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                var dateHeaderCell = new Cell();
                dateHeaderCell.DataType = CellValues.String;
                dateHeaderCell.CellValue = new CellValue("Date Time");
                headerRow.AppendChild(dateHeaderCell);

                var levelHeaderCell = new Cell();
                levelHeaderCell.DataType = CellValues.String;
                levelHeaderCell.CellValue = new CellValue("Level");
                headerRow.AppendChild(levelHeaderCell);

                var sourceHeaderCell = new Cell();
                sourceHeaderCell.DataType = CellValues.String;
                sourceHeaderCell.CellValue = new CellValue("Source");
                headerRow.AppendChild(sourceHeaderCell);

                var messageHeaderCell = new Cell();
                messageHeaderCell.DataType = CellValues.String;
                messageHeaderCell.CellValue = new CellValue("Message");
                headerRow.AppendChild(messageHeaderCell);

                var exceptionHeaderCell = new Cell();
                exceptionHeaderCell.DataType = CellValues.String;
                exceptionHeaderCell.CellValue = new CellValue("Exception");
                headerRow.AppendChild(exceptionHeaderCell);

                sheetData.AppendChild(headerRow);

                foreach (var res in await results.OrderBy(x => x.DateTime).ToArrayAsync())
                {
                    Row newRow = new Row();

                    var dateCell = new Cell();
                    dateCell.DataType = CellValues.String;
                    dateCell.CellValue = new CellValue(res.DateTime.ToString("MM/dd/yyyy HH:mm:ss.fff K"));
                    newRow.AppendChild(dateCell);

                    var levelCell = new Cell();
                    levelCell.DataType = CellValues.String;
                    levelCell.CellValue = new CellValue(res.Level.GetDisplayValue());
                    newRow.AppendChild(levelCell);

                    var sourceCell = new Cell();
                    sourceCell.DataType = CellValues.String;
                    sourceCell.CellValue = new CellValue(res.Source);
                    newRow.AppendChild(sourceCell);

                    var inlineString = new InlineString();
                    var text = new Text { Text = res.Message };
                    inlineString.AppendChild(text);

                    var messageCell = new Cell();
                    messageCell.DataType = CellValues.InlineString;
                    messageCell.AppendChild(inlineString);
                    newRow.AppendChild(messageCell);

                    if (string.IsNullOrEmpty(res.Exception))
                    {
                        var exceptionEmptyCell = new Cell();
                        exceptionEmptyCell.DataType = CellValues.String;
                        exceptionEmptyCell.CellValue = new CellValue(string.Empty);
                        newRow.AppendChild(exceptionEmptyCell);
                    }
                    else
                    {
                        var exceptionString = new InlineString();
                        exceptionString.AppendChild(new Text { Text = res.Exception });

                        var exceptionCell = new Cell();
                        exceptionCell.DataType = CellValues.InlineString;
                        exceptionCell.AppendChild(exceptionString);
                        newRow.AppendChild(exceptionCell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }

            stream.Position = 0;

            return File(
            stream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"LogExport-{DateTime.Now.ToString("MM-dd-yyyy")}.xlsx");
        }

        static string EscapeXmlText(string value)
        {
            return new System.Xml.Linq.XElement("t", value).LastNode.ToString();
        }

        [HttpGet, Route("response")]
        public async Task<IEnumerable<LogDTO>> GetResponseLogs(Guid responseID)
        {
            return await (from log in this.modelDb.Logs.AsNoTracking()
                          where log.ResponseID == responseID
                          orderby log.DateTime descending
                          select new LogDTO
                          {
                              DateTime = log.DateTime,
                              Level = log.Level,
                              Message = log.Message,
                              Source = log.Source,
                              Exception = log.Exception
                          }).ToArrayAsync();
        }
    }
}
