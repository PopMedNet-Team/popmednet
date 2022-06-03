using System.Linq;
using System.Reflection;
using HtmlAgilityPack;

namespace PopMedNet.TrxToHtml.Parser
{
	internal class HtmlConverter
	{
		private TestRunResult currentRun;

		private TestRunResult previousRun;

		public HtmlConverter(TestRunResult currentRunResult)
		{
			currentRun = currentRunResult;
		}

		public HtmlConverter(TestRunResult currentRunResult, TestRunResult previousRunResult)
		{
			currentRun = currentRunResult;
			previousRun = previousRunResult;
		}

		public string GetHtml()
		{
			HtmlDocument htmlDocument = new HtmlDocument();
			HtmlCommentNode newChild = htmlDocument.CreateComment("<!DOCTYPE html>");
			HtmlNode refChild = htmlDocument.DocumentNode.SelectSingleNode("/html");
			htmlDocument.DocumentNode.InsertBefore(newChild, refChild);
			HtmlNode htmlNode = htmlDocument.CreateElement("head");
			HtmlNode htmlNode2 = htmlDocument.CreateElement("body");
			WriteHeader(currentRun, htmlDocument, htmlNode);
			htmlDocument.DocumentNode.AppendChild(htmlNode);
			HtmlNode htmlNode3 = htmlDocument.CreateElement("section");
			htmlNode3.SetAttributeValue("class", "section");
			HtmlNode htmlNode4 = htmlDocument.CreateElement("a");
			htmlNode4.SetAttributeValue("name", "_top");
			htmlNode3.AppendChild(htmlNode4);
			HtmlNode htmlNode5 = htmlDocument.CreateElement("h1");
			htmlNode5.AddClass("title");
			htmlNode5.InnerHtml = currentRun.Name;
			htmlNode3.AppendChild(htmlNode5);
			HtmlNode htmlNode6 = htmlDocument.CreateElement("div");
			htmlNode6.AddClass("contents");
			HtmlNode htmlNode7 = htmlDocument.CreateElement("a");
			htmlNode7.InnerHtml = "Totals";
			htmlNode7.SetAttributeValue("href", "#totals");
			htmlNode6.AppendChild(htmlNode7);
			htmlNode6.InnerHtml += " | ";
			HtmlNode htmlNode8 = htmlDocument.CreateElement("a");
			htmlNode8.InnerHtml = "Summary";
			htmlNode8.SetAttributeValue("href", "#summary");
			htmlNode6.AppendChild(htmlNode8);
			htmlNode6.InnerHtml += " | ";
			HtmlNode htmlNode9 = htmlDocument.CreateElement("a");
			htmlNode9.InnerHtml = "Slowers";
			htmlNode9.SetAttributeValue("href", "#slower");
			htmlNode6.AppendChild(htmlNode9);
			htmlNode6.InnerHtml += " | ";
			HtmlNode htmlNode10 = htmlDocument.CreateElement("a");
			htmlNode10.InnerHtml = "Detail";
			htmlNode10.SetAttributeValue("href", "#detail");
			htmlNode6.AppendChild(htmlNode10);
			htmlNode6.InnerHtml += " | ";
			HtmlNode htmlNode11 = htmlDocument.CreateElement("a");
			htmlNode11.InnerHtml = "Environment Information";
			htmlNode11.SetAttributeValue("href", "#envInfo");
			htmlNode6.AppendChild(htmlNode11);
			htmlNode3.AppendChild(htmlNode6);
			htmlNode3.AppendChild(htmlDocument.CreateElement("br"));
			WriteBody(currentRun, htmlDocument, htmlNode3);
			WriteEnvironmentInfo(currentRun, htmlDocument, htmlNode3);
			htmlNode2.AppendChild(htmlNode3);
			htmlDocument.DocumentNode.AppendChild(htmlNode2);
			return htmlDocument.DocumentNode.InnerHtml;
		}

		private void WriteHeader(TestRunResult run, HtmlDocument doc, HtmlNode head)
		{
			HtmlNode htmlNode = doc.CreateElement("title");
			htmlNode.InnerHtml = run.Name;
			head.AppendChild(htmlNode);
			HtmlNode htmlNode2 = doc.CreateElement("link");
			htmlNode2.SetAttributeValue("rel", "stylesheet");
			htmlNode2.SetAttributeValue("href", "https://cdn.jsdelivr.net/npm/bulma@0.9.0/css/bulma.min.css");
			head.AppendChild(htmlNode2);
			HtmlNode htmlNode3 = doc.CreateElement("style");
			htmlNode3.SetAttributeValue("type", "text/css");
			htmlNode3.InnerHtml = GetCss();
			head.AppendChild(htmlNode3);
			HtmlNode htmlNode4 = doc.CreateElement("script");
			htmlNode4.SetAttributeValue("type", "text/javascript");
			htmlNode4.InnerHtml = "function togle(anId){var el = document.getElementById(anId);if (el!=null){if (el.style.display=='none'){el.style.display='block'}else{el.style.display='none';}}}function showAllTestClassesSummary(){var tSummaryDetails = document.getElementById('tSummaryDetail');for (var i=0; i < tSummaryDetails.rows.length;i++){ var r = tSummaryDetails.rows[i]; r.style.display='block';}}";
			head.AppendChild(htmlNode4);
		}

		private string GetCss()
		{
			return "\r\n\t\t\t\tpre{font-size:8pt}\r\n\r\n\t\t\t\tspan.ok{background-color:lime;color:black;font-size:40%;color:lime;display:inline-block;}\r\n\t\t\t\tspan.ko{background-color:red;color:white;font-size:40%;color:red;display:inline-block;}\r\n\t\t\t\tspan.ignore{background-color:yellow;color:white;font-size:40%;color:yellow;display:inline-block;}\r\n\r\n\t\t\t\tp.testKo{\r\n\t\t\t\t\tcursor:hand;\r\n\t\t\t\t\twidth:40px;\r\n\t\t\t\t\theight:40px;\r\n\t\t\t\t\tbackground-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAUCAIAAAAGHlpnAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAPBSURBVDhPY/hPMWDANOHXu1fvTx94v3bmy3kdT6bXP51c9XRi5ctpzS8O7vz57jWmehQj/v398+XKqXfLJ79Z0P5pUce3Re2/5jX9nFn9pb/odUv6w/Koh03Zb88dRzMF2Yh/3y4ce7uw89Pizt+r+v8s7fw1t/7ntIqfEwt/duX8akn5Vhf1MM/3WrrPkz2bkE1BGPH1xvk381q/LO36vbLv18LmX9PKf/bl/ehM/9mc+KM+5nt1+LfywM8FXg8SbM9E2rw8ewxuCtSIv5/ev10+6cOC1t/Lu/+c3g2R/jmz9kdVyM9JxVDuxrmfMhzeJ5pfCNI7kRP++/s3iDjUiK+n9r2eUftrcftvoOMn5P97dh8o9/fJne/VYX8f3QKxH978mGr7IcHiTbThoxDt7c5q97au+fv7N8KIN8snf5ha8WtO/Y9JRT9ak35OLYfY8O/NMxD59dOX8uAP8aZvow2eh2o98lfb7yh/tCTpz9fPCCOeTq7+Mrn0x6Tin51pP2ojvxX7/to0F+7b7ws63scav4vUfx2i+cRP5a6n/FF7qQ2hjj9fv0AYca8950d//o/uzB9Nsd/K/b/muX8tCYAb8SHd4W247psQjZf+yo885W65SJ2wEV3kbvTlznWEEbca0r61pX5vjv9WFfo13+NLluPvM/vhRvw8tuNNkPorP6VnXnL3nSWu2oscNheYY6/98co5hBF3GtLfV4Z/B+ov9P2a6fxz7XSI/u+b5kEYXxZ0vvSUfeQscdNO+Jwl/2YDnoVOel/ug0IaGiMP+2seZ7p/LwDa7/S1Kuzfl09AuR8b574N1vi+cwUoRD9/eBlnds9O+JIF/0lTngWanNsyI/58BimDGvH64PYLMTZvUm0/Jln+unISFIsvHr+NM33tr/Q6XPfPs4cgF505cNWC76Qx1y5djn413ht7d/77+xdhxO9PHy5Wpp4PNnoeafg8TPdZkOazANVnPgrPPGSfuog/chS9ayN4xZz7hBHHXj3W2Socy+P8/4ATBcIIIOvlhVOHwm1PeGnc9lW946V420P2tovkTQexG7aCVyx5z5lxHTPk2KPLMl+VdZql2sNLF+CBjZJTb2/fsMXLcIe93Dkn6YsOEudtRM5ZCZwx4z1mxHlQn22bFutMZdYpBtLnt26E60dxBUT0wZH922PcF5rJrDYU3mrIt8eQZ7sO1xoN9nmq7P1KnHOcDZ8f3YesH4sRoGB78+rKqkWbkoNmOehOsFTvtVSfYqm62Nvi9KS2T08foenHbgSmIvwiACPu1otCSuPAAAAAAElFTkSuQmCC);\r\n\t\t\t\t\tbackground-repeat:no-repeat\r\n\t\t\t\t}                \r\n\t\t\t\tp.testOk{\r\n\t\t\t\t\tcursor:hand;\r\n\t\t\t\t\twidth:40px;\r\n\t\t\t\t\theight:40px;\r\n\t\t\t\t\tbackground-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAMTSURBVDhPpdTrL5tRGABw/80+LCKzGHHJqEt19KWoy2sW0a5D0a6hVcWoKu26Tlt1p1Uzd6WoqdVtQ0vEZW6xi2UXsw3r0FKq3ZHKeGUfljgfTvKcnF+ec3nOcbBdoTlcwdou40+/PopHH5M74sk98TQNiTbwIFF5l/AspnRE+O33xqVMCDz+YZSiJNS9kejML7XW7o49heyLRPqWx5t9lNabAEl9VXMdF/05nlx/laFOntjVLNp0mpPOpu2qinWBcInDm83h6JlsPYMxmOwvdG+davzrz/DW/k+mmqI3audt471HLfWbUtEKt3g2m62n502ms8aoJCVM6U8gq+K9i1zefV+z+zPct9QlneMt2CbUx62yjVLhIrtAz8h9TWONpmWOpOLlgUINV7XaTu0hRlRj0ptTEZimSho6UA1alYrNspJlDkjIGqMwhlPp2mRcbUC+imk5sZgth6x+WqwiNLgEhcDUgfv9lrbmnRrRKjdjKDFrNI0xlJKhJUfUBWYr0/cP944sR5U6UVwjLlaB8+a7IDBdS+4yNoITki7zRSM8WAFlDqfg5RhmB/Xw+ABMrdZJIhvuwIoQWB7iK3BDYEofESy4eD57Zkt3bDlmd2ehSz0y2ykms8lqtVZPlUUqMEBG1WHxNUE+fFcEJrZHi1YKOdPMp9PcXbPBZDYWdLNMR0YwqUYvDZej4QYoqi4ovCowtDzAi+uMwLm9dLaOXqBjUDUEsY5nXypotfqyMHlAjAKKkmGBDCnzDxb7YAS3EXhwWU3qjM0Ze5g+kJigjJTNVHw1fC7S5obL0HA9FuSMOJV+kAQF7lmsESCwwWSIKscS26IpfYQkZVxcM47YAoOjjpaDTWJwlWhI6hcsQaGfeLrmOa5triIwCNQLKh/erbiGMFIbHP8cD8ux0TIsKAlcBRoq9Q0WowIEns7Z10pe8C+Xpz3WLKqhEn9QwJAUFVaBxlcHgR57mtDLvcDJJfe6RCP898Owj+7sb7dPN5Fk99zyHd3ZTh6FN0DvxblJries/3h/UZ7X9qXR/wyv9JP8AUelhZbR6oNqAAAAAElFTkSuQmCC);\r\n\t\t\t\t\tbackground-repeat:no-repeat\r\n\t\t\t\t}\r\n\t\t\t\tp.testIgnore{\r\n\t\t\t\t\tcursor:hand;\r\n\t\t\t\t\twidth:40px;\r\n\t\t\t\t\theight:40px;\r\n\t\t\t\t\tbackground-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAATCAIAAAAf7rriAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAJeSURBVDhPY/hPAWDApfff328EjcWu+de3J49PWD47a3lym/Xlk0txmYJd89s7kz48UvnxLfDKEb4p1ZJ//vzGqh+L5r+/Pzw5ZfLvj+P//57//0tOreY+f2QWsZo/Pel9f8/h3//QRw9k//6TO7aTc26rGlGa//x8+vK83f+/BQsW+SnKa/d1G/z/zzetmvPCifWY+tGd/eH+1E/3ff//r01MtmFg8A3xsv7/X2j/GqbZLToENP/99fHJSZd/v5r+/8/sbjNkYkiY1a3+/z//15fcE8q47t44jqYfxeZPj2e/ueXz/3/v//+uuzbJKfA3Xjyi9P8Py9/v4ptnsM7rcsCp+d/fj8/Oevz6Vvz/f97//2pHDonqyMfdvSL4/wPDrw+ib66z9ZdyPn9yB1k/wubvr5a9ugb0IVCnw/+//FcvcCV46jy/yfb/GcPPR7x/Pois6GFeNTMSi+Z/fz68OOf+/bPX//9u/z9L/H/J+v8z07NznP/fMvy/w/DzCtOv95JPz3NOLOP9/u0DXD/U5q/PV7w6L/fvv+X/TzL/H7D/e8O8eBqLvy/7poXsQJt/nWP4eovn71vRpe2sy2fEomj+8Pb+9e0W3+9I//8s9f8W2/9bzN+fMViZMjIwxBWnJP5/wfDrLMP3c4x/ngg+Pcg0sd4eRfPrp2e3T+d/tIf7xWG2J7uZHu1gen6IcXk3U1WK2/k1Zo/3MN7Zwnh/K+OjXUwv9jGu6xdH0fz719cbh9Jn1LBPLmMDoklgcloVELFOqWCdVAZEIBEgmlrJuqBDE93PBLMuUWmbJFMAIcChwBwUNxQAAAAASUVORK5CYII=);\r\n\t\t\t\t\tbackground-repeat:no-repeat\r\n\t\t\t   }\r\n\t\t\t\tdiv.barContainer{width:100%;}\r\n\t\t\t\tdiv.trace{font:100% Courier}\r\n\t\t\t\tdiv.border{border:dotted 1px #dcdcdc;padding:2px 2px 2px 2px}\r\n\t\t\t\tdiv.contents{border:dotted 1px #dcdcdc;padding:2px 2px 2px 2px;background-color:#efefef}\r\n\t\t\t\t\t\t\t\t\t.table-container > table.table > thead > tr > th {top:0;position: sticky; background-color: grey;}";
		}

		private void WriteEnvironmentInfo(TestRunResult run, HtmlDocument doc, HtmlNode section)
		{
			HtmlNode htmlNode = doc.CreateElement("a");
			htmlNode.SetAttributeValue("name", "envInfo");
			section.AppendChild(htmlNode);
			HtmlNode htmlNode2 = doc.CreateElement("div");
			htmlNode2.AddClass("card");
			HtmlNode htmlNode3 = doc.CreateElement("header");
			htmlNode3.AddClass("card-header");
			HtmlNode htmlNode4 = doc.CreateElement("p");
			htmlNode4.AddClass("card-header-title");
			htmlNode4.InnerHtml = "Enviorment Details";
			htmlNode3.AppendChild(htmlNode4);
			htmlNode2.AppendChild(htmlNode3);
			HtmlNode htmlNode5 = doc.CreateElement("div");
			htmlNode5.AddClass("card-content");
			HtmlNode htmlNode6 = doc.CreateElement("div");
			htmlNode6.AddClass("content");
			HtmlNode htmlNode7 = doc.CreateElement("table");
			htmlNode7.AddClass("table");
			htmlNode7.AddClass("is-bordered");
			htmlNode7.AddClass("is-hovered");
			htmlNode7.SetAttributeValue("id", "tSlowerMethods");
			HtmlNode newChild = HtmlNode.CreateNode("<thead><tr><th colspan=\"2\">TestRun Environment Information</th></tr></thead>");
			htmlNode7.AppendChild(newChild);
			HtmlNode htmlNode8 = doc.CreateElement("tbody");
			HtmlNode newChild2 = HtmlNode.CreateNode("<tr><th align=\"right\">MachineName</th><td>" + run.Computers.First() + "</td></tr>");
			htmlNode8.AppendChild(newChild2);
			HtmlNode htmlNode9 = doc.CreateElement("tr");
			HtmlNode htmlNode10 = doc.CreateElement("th");
			htmlNode10.SetAttributeValue("align", "right");
			htmlNode10.InnerHtml = "Test Assemblies";
			htmlNode9.AppendChild(htmlNode10);
			HtmlNode htmlNode11 = doc.CreateElement("td");
			foreach (AssemblyName assembly in run.Assemblies)
			{
				htmlNode11.AppendChild(HtmlNode.CreateNode("<br />"));
				htmlNode11.InnerHtml += assembly.FullName;
			}
			htmlNode9.AppendChild(htmlNode11);
			htmlNode8.AppendChild(htmlNode9);
			HtmlNode newChild3 = HtmlNode.CreateNode("<tr><th align=\"right\">UserName</th><td>" + run.UserName + "</td></tr>");
			htmlNode8.AppendChild(newChild3);
			HtmlNode newChild4 = HtmlNode.CreateNode("<tr><th align=\"right\">UserName</th><td>" + run.Name + "</td></tr>");
			htmlNode8.AppendChild(newChild4);
			htmlNode7.AppendChild(htmlNode8);
			HtmlNode htmlNode12 = doc.CreateElement("div");
			htmlNode12.AddClass("table-container");
			htmlNode12.AppendChild(htmlNode7);
			htmlNode6.AppendChild(htmlNode12);
			htmlNode5.AppendChild(htmlNode6);
			htmlNode2.AppendChild(htmlNode5);
			section.AppendChild(htmlNode2);
		}

		private void WriteBody(TestRunResult run, HtmlDocument doc, HtmlNode section)
		{
			WriteSummary(run, doc, section);
			section.AppendChild(doc.CreateElement("br"));
			if (previousRun == null)
			{
				WriteSummaryDetails(run, doc, section);
			}
			else
			{
				WriteSummaryDetailsWithPrevious(run, doc, section);
			}
			WriteSlowerMethods(run, doc, section);
			foreach (IGrouping<string, TestMethodRun> item in from m in run.TestMethodRunList
															  group m by m.TestClass)
			{
				IGrouping<string, TestMethodRun> c = item;
				WriteClassResult(run.TestClassList.First((TestClassRun tc) => tc.FullName == c.Key), doc, section);
			}
		}

		private void WriteSlowerMethods(TestRunResult run, HtmlDocument doc, HtmlNode section)
		{
			HtmlNode htmlNode = doc.CreateElement("a");
			htmlNode.SetAttributeValue("name", "slower");
			section.AppendChild(htmlNode);
			HtmlNode htmlNode2 = doc.CreateElement("div");
			htmlNode2.AddClass("card");
			HtmlNode htmlNode3 = doc.CreateElement("header");
			htmlNode3.AddClass("card-header");
			HtmlNode htmlNode4 = doc.CreateElement("p");
			htmlNode4.AddClass("card-header-title");
			htmlNode4.InnerHtml = "Top 5 Slower Methods";
			htmlNode3.AppendChild(htmlNode4);
			htmlNode2.AppendChild(htmlNode3);
			HtmlNode htmlNode5 = doc.CreateElement("div");
			htmlNode5.AddClass("card-content");
			HtmlNode htmlNode6 = doc.CreateElement("div");
			htmlNode6.AddClass("content");
			HtmlNode htmlNode7 = doc.CreateElement("table");
			htmlNode7.AddClass("table");
			htmlNode7.AddClass("is-bordered");
			htmlNode7.AddClass("is-hovered");
			htmlNode7.AddClass("is-narrow");
			htmlNode7.SetAttributeValue("id", "tSlowerMethods");
			htmlNode7.AppendChild(HtmlNode.CreateNode("<colgroup><col width=\"25%\" /><col width=\"10%\" /><col width=\"50%\" /><col width=\"15%\" /></colgroup>"));
			HtmlNode newChild = HtmlNode.CreateNode("<thead><tr><th>Test Method</th><th colspan='2'>Status</th><th>Duration (Seconds)</th></tr></thead>");
			htmlNode7.AppendChild(newChild);
			HtmlNode htmlNode8 = doc.CreateElement("tbody");
			foreach (TestMethodRun topSlowerMethod in run.TopSlowerMethods)
			{
				WriteTestMethodResult(topSlowerMethod, doc, htmlNode8);
			}
			htmlNode7.AppendChild(htmlNode8);
			HtmlNode htmlNode9 = doc.CreateElement("div");
			htmlNode9.AddClass("table-container");
			htmlNode9.AppendChild(htmlNode7);
			htmlNode6.AppendChild(htmlNode9);
			htmlNode5.AppendChild(htmlNode6);
			htmlNode2.AppendChild(htmlNode5);
			section.AppendChild(htmlNode2);
		}

		private void WriteSummary(TestRunResult run, HtmlDocument doc, HtmlNode section)
		{
			HtmlNode htmlNode = doc.CreateElement("div");
			htmlNode.AddClass("card");
			HtmlNode htmlNode2 = doc.CreateElement("header");
			htmlNode2.AddClass("card-header");
			HtmlNode htmlNode3 = doc.CreateElement("p");
			htmlNode3.AddClass("card-header-title");
			htmlNode3.InnerHtml = "Totals";
			htmlNode2.AppendChild(htmlNode3);
			htmlNode.AppendChild(htmlNode2);
			HtmlNode htmlNode4 = doc.CreateElement("div");
			htmlNode4.AddClass("card-content");
			HtmlNode htmlNode5 = doc.CreateElement("div");
			htmlNode5.AddClass("content");
			HtmlNode htmlNode6 = doc.CreateElement("table");
			htmlNode6.AddClass("table");
			htmlNode6.AddClass("is-bordered");
			htmlNode6.AddClass("is-hovered");
			htmlNode6.AddClass("is-narrow");
			htmlNode6.SetAttributeValue("id", "tMainSummary");
			HtmlNode newChild = HtmlNode.CreateNode("<colgroup><col width=\"30%\" /><col width=\"10%\" /><col width=\"10%\" /><col width=\"10%\" /><col width=\"10%\" /><col width=\"20%\" /></colgroup>");
			htmlNode6.AppendChild(newChild);
			HtmlNode newChild2 = HtmlNode.CreateNode("<thead><tr><th>Status</th><th>Total Tests</th><th>Passed</th><th>Failed</th><th>Inconclusive</th><th>Duration (Seconds)</th></tr></thead>");
			htmlNode6.AppendChild(newChild2);
			HtmlNode htmlNode7 = doc.CreateElement("tbody");
			HtmlNode htmlNode8 = doc.CreateElement("tr");
			HtmlNode htmlNode9 = doc.CreateElement("td");
			HtmlNode htmlNode10 = doc.CreateElement("progress");
			htmlNode10.AddClass("progress");
			htmlNode10.AddClass("is-success");
			htmlNode10.SetAttributeValue("max", "100");
			htmlNode10.SetAttributeValue("value", run.TotalPercent.ToString());
			htmlNode9.AppendChild(htmlNode10);
			htmlNode9.InnerHtml += string.Format(" {0}%", run.TotalPercent);
			htmlNode9.SetAttributeValue("style", "text-align:center;");
			htmlNode8.AppendChild(htmlNode9);
			HtmlNode htmlNode11 = doc.CreateElement("td");
			htmlNode11.InnerHtml = run.TotalMethods.ToString();
			htmlNode8.AppendChild(htmlNode11);
			HtmlNode htmlNode12 = doc.CreateElement("td");
			htmlNode12.InnerHtml = run.Passed.ToString();
			htmlNode8.AppendChild(htmlNode12);
			HtmlNode htmlNode13 = doc.CreateElement("td");
			htmlNode13.InnerHtml = run.Failed.ToString();
			htmlNode8.AppendChild(htmlNode13);
			HtmlNode htmlNode14 = doc.CreateElement("td");
			htmlNode14.InnerHtml = run.Inconclusive.ToString();
			htmlNode8.AppendChild(htmlNode14);
			HtmlNode htmlNode15 = doc.CreateElement("td");
			htmlNode15.InnerHtml = run.TimeTaken.TotalSeconds.ToString("0.00");
			htmlNode15.SetAttributeValue("style", "text-align: right;");
			htmlNode8.AppendChild(htmlNode15);
			htmlNode7.AppendChild(htmlNode8);
			htmlNode6.AppendChild(htmlNode7);
			htmlNode5.AppendChild(htmlNode6);
			htmlNode4.AppendChild(htmlNode5);
			htmlNode.AppendChild(htmlNode4);
			section.AppendChild(htmlNode);
		}

		private string CreateHtmlBars(I3ValueBar run)
		{
			double percentOK = run.PercentOK;
			double percentKO = run.PercentKO;
			double percentIgnored = run.PercentIgnored;
			string text = "<div class=\"barContainer\">";
			if (percentOK != 0.0)
			{
				text += string.Format("<span class=\"ok\"  style=\"width:{0}%\" title=\"Passed!\" >p</span>", percentOK.ToString());
			}
			if (percentKO != 0.0)
			{
				text += string.Format("<span class=\"ko\"  style=\"width:{0}%\" title=\"Failed\">f</span>", percentKO.ToString());
			}
			if (percentIgnored != 0.0)
			{
				text += string.Format("<span class=\"ignore\"  style=\"width:{0}%\" title=\"Failed\">f</span>", percentIgnored.ToString());
			}
			return text + "</div>";
		}

		private void WriteSummaryDetails(TestRunResult run, HtmlDocument doc, HtmlNode section)
		{
			HtmlNode htmlNode = doc.CreateElement("div");
			htmlNode.AddClass("card");
			HtmlNode htmlNode2 = doc.CreateElement("header");
			htmlNode2.AddClass("card-header");
			HtmlNode htmlNode3 = doc.CreateElement("p");
			htmlNode3.AddClass("card-header-title");
			htmlNode3.InnerHtml = "Summary";
			htmlNode2.AppendChild(htmlNode3);
			htmlNode.AppendChild(htmlNode2);
			HtmlNode htmlNode4 = doc.CreateElement("div");
			htmlNode4.AddClass("card-content");
			HtmlNode htmlNode5 = doc.CreateElement("div");
			htmlNode5.AddClass("content");
			HtmlNode htmlNode6 = doc.CreateElement("table");
			htmlNode6.AddClass("table");
			htmlNode6.AddClass("is-bordered");
			htmlNode6.AddClass("is-hovered");
			htmlNode6.SetAttributeValue("id", "tMainSummary");
			HtmlNode newChild = HtmlNode.CreateNode("<thead><tr><th>Class Name</th><th>Status</th><th>Tests Passed</th><th>Tests Failed</th><th>Tests Ignored</th><th>Duration (Seconds)</th></tr></thead>");
			htmlNode6.AppendChild(newChild);
			HtmlNode htmlNode7 = doc.CreateElement("tbody");
			foreach (TestClassRun testClass in run.TestClassList)
			{
				HtmlNode htmlNode8 = doc.CreateElement("tr");
				HtmlNode newChild2 = doc.CreateElement("td");
				HtmlNode htmlNode9 = doc.CreateElement("a");
				htmlNode9.SetAttributeValue("href", "#" + testClass.Name);
				htmlNode9.InnerHtml = testClass.Name;
				htmlNode8.AppendChild(newChild2);
				HtmlNode htmlNode10 = doc.CreateElement("td");
				HtmlNode htmlNode11 = doc.CreateElement("progress");
				htmlNode11.AddClass("progress");
				htmlNode11.AddClass("is-success");
				htmlNode11.SetAttributeValue("max", "100");
				htmlNode11.SetAttributeValue("value", testClass.Percent.ToString());
				htmlNode10.AppendChild(htmlNode11);
				htmlNode10.InnerHtml += string.Format(" {0}%", testClass.Percent);
				htmlNode10.SetAttributeValue("style", "text-align:center;");
				htmlNode8.AppendChild(htmlNode10);
				HtmlNode htmlNode12 = doc.CreateElement("td");
				htmlNode12.InnerHtml = testClass.Success.ToString();
				htmlNode8.AppendChild(htmlNode12);
				HtmlNode htmlNode13 = doc.CreateElement("td");
				htmlNode13.InnerHtml = testClass.Failed.ToString();
				htmlNode8.AppendChild(htmlNode13);
				HtmlNode htmlNode14 = doc.CreateElement("td");
				htmlNode14.InnerHtml = testClass.Ignored.ToString();
				htmlNode8.AppendChild(htmlNode14);
				HtmlNode htmlNode15 = doc.CreateElement("td");
				htmlNode15.InnerHtml = testClass.Duration.TotalSeconds.ToString("0.00");
				htmlNode15.SetAttributeValue("style", "text-align: right;");
				htmlNode8.AppendChild(htmlNode15);
				htmlNode7.AppendChild(htmlNode8);
			}
			htmlNode6.AppendChild(htmlNode7);
			htmlNode5.AppendChild(htmlNode6);
			htmlNode4.AppendChild(htmlNode5);
			htmlNode.AppendChild(htmlNode4);
			section.AppendChild(htmlNode);
		}

		private void WriteSummaryDetailsWithPrevious(TestRunResult run, HtmlDocument doc, HtmlNode section)
		{
			HtmlNode htmlNode = doc.CreateElement("div");
			htmlNode.AddClass("card");
			HtmlNode htmlNode2 = doc.CreateElement("header");
			htmlNode2.AddClass("card-header");
			HtmlNode htmlNode3 = doc.CreateElement("p");
			htmlNode3.AddClass("card-header-title");
			htmlNode3.InnerHtml = "Summary";
			htmlNode2.AppendChild(htmlNode3);
			htmlNode.AppendChild(htmlNode2);
			HtmlNode htmlNode4 = doc.CreateElement("div");
			htmlNode4.AddClass("card-content");
			HtmlNode htmlNode5 = doc.CreateElement("div");
			htmlNode5.AddClass("content");
			HtmlNode htmlNode6 = doc.CreateElement("table");
			htmlNode6.AddClass("table");
			htmlNode6.AddClass("is-bordered");
			htmlNode6.AddClass("is-hovered");
			htmlNode6.SetAttributeValue("id", "tSummaryDetail");
			HtmlNode newChild = HtmlNode.CreateNode("<colgroup><col width=\"30%\" /><col width=\"24%\" /><col width=\"4%\" /><col width=\"4%\" /><col width=\"4%\" /><col width=\"4%\" /><col width=\"4%\" /><col width=\"4%\" /><col width=\"4%\" /><col width=\"4%\" /></colgroup>");
			htmlNode6.AppendChild(newChild);
			HtmlNode newChild2 = HtmlNode.CreateNode("<thead><tr><th colspan='2'>Test Class Summary</th><th colspan='3'>Previous Run</th><th colspan='3'>Current Run</th><th colspan='2'></th></tr><tr><th>Class Name</th><th>Percent</th><th>Tests Passed</th><th>Tests Failed</th><th>Tests Ignored</th><th>Tests Passed</th><th>Tests Failed</th><th>Tests Ignored</th><th>Regressions</th><th>Duration (seconds)</th></tr></thead>");
			htmlNode6.AppendChild(newChild2);
			HtmlNode htmlNode7 = doc.CreateElement("tbody");
			foreach (TestClassRun testClass in run.TestClassList)
			{
				TestClassRun previousTestClassRun = previousRun.TestClassList.Where((TestClassRun x) => x.FullName == testClass.Name).FirstOrDefault();
				HtmlNode htmlNode8 = doc.CreateElement("tr");
				HtmlNode htmlNode9 = doc.CreateElement("td");
				HtmlNode htmlNode10 = doc.CreateElement("a");
				htmlNode10.SetAttributeValue("href", "#" + testClass.Name);
				htmlNode10.InnerHtml = testClass.Name;
				htmlNode9.AppendChild(htmlNode10);
				htmlNode8.AppendChild(htmlNode9);
				HtmlNode htmlNode11 = doc.CreateElement("td");
				HtmlNode htmlNode12 = doc.CreateElement("progress");
				htmlNode12.AddClass("progress");
				htmlNode12.AddClass("is-success");
				htmlNode12.SetAttributeValue("max", "100");
				htmlNode12.SetAttributeValue("value", testClass.Percent.ToString());
				htmlNode11.AppendChild(htmlNode12);
				htmlNode11.InnerHtml += string.Format(" {0}%", testClass.Percent);
				htmlNode11.SetAttributeValue("style", "text-align:center;");
				htmlNode8.AppendChild(htmlNode11);
				HtmlNode htmlNode13 = doc.CreateElement("td");
				htmlNode13.InnerHtml = ((previousTestClassRun != null) ? previousTestClassRun.Success.ToString() : "0");
				htmlNode8.AppendChild(htmlNode13);
				HtmlNode htmlNode14 = doc.CreateElement("td");
				htmlNode14.InnerHtml = ((previousTestClassRun != null) ? previousTestClassRun.Failed.ToString() : "0");
				htmlNode8.AppendChild(htmlNode14);
				HtmlNode htmlNode15 = doc.CreateElement("td");
				htmlNode15.InnerHtml = ((previousTestClassRun != null) ? previousTestClassRun.Ignored.ToString() : "0");
				htmlNode8.AppendChild(htmlNode15);
				HtmlNode htmlNode16 = doc.CreateElement("td");
				htmlNode16.InnerHtml = testClass.Success.ToString();
				htmlNode8.AppendChild(htmlNode16);
				HtmlNode htmlNode17 = doc.CreateElement("td");
				htmlNode17.InnerHtml = testClass.Failed.ToString();
				htmlNode8.AppendChild(htmlNode17);
				HtmlNode htmlNode18 = doc.CreateElement("td");
				htmlNode18.InnerHtml = testClass.Ignored.ToString();
				htmlNode8.AppendChild(htmlNode18);
				HtmlNode htmlNode19 = doc.CreateElement("td");
				htmlNode19.InnerHtml = ((previousTestClassRun != null) ? testClass.Regressions(previousTestClassRun).ToString() : "0");
				htmlNode8.AppendChild(htmlNode19);
				HtmlNode htmlNode20 = doc.CreateElement("td");
				htmlNode20.InnerHtml = testClass.Duration.TotalSeconds.ToString("0.00");
				htmlNode20.SetAttributeValue("style", "text-align: right;");
				htmlNode8.AppendChild(htmlNode20);
				if (previousTestClassRun != null && testClass.Regressions(previousTestClassRun) > 0.0)
				{
					htmlNode8.SetAttributeValue("style", "background-color:yellow;");
				}
				htmlNode7.AppendChild(htmlNode8);
			}
			htmlNode6.AppendChild(htmlNode7);
			htmlNode5.AppendChild(htmlNode6);
			htmlNode4.AppendChild(htmlNode5);
			htmlNode.AppendChild(htmlNode4);
			section.AppendChild(htmlNode);
		}

		private void WriteClassResult(TestClassRun tcr, HtmlDocument doc, HtmlNode section)
		{
			section.AppendChild(doc.CreateElement("br"));
			HtmlNode htmlNode = doc.CreateElement("a");
			htmlNode.SetAttributeValue("name", tcr.Name);
			section.AppendChild(htmlNode);
			HtmlNode htmlNode2 = doc.CreateElement("div");
			htmlNode2.AddClass("card");
			HtmlNode htmlNode3 = doc.CreateElement("header");
			htmlNode3.AddClass("card-header");
			HtmlNode htmlNode4 = doc.CreateElement("p");
			htmlNode4.AddClass("card-header-title");
			htmlNode4.InnerHtml = tcr.Name ?? "";
			htmlNode3.AppendChild(htmlNode4);
			htmlNode2.AppendChild(htmlNode3);
			HtmlNode htmlNode5 = doc.CreateElement("div");
			htmlNode5.AddClass("card-content");
			HtmlNode htmlNode6 = doc.CreateElement("div");
			htmlNode6.AddClass("content");
			HtmlNode htmlNode7 = doc.CreateElement("table");
			htmlNode7.AddClass("table");
			htmlNode7.AddClass("is-bordered");
			htmlNode7.AddClass("is-hovered");
			htmlNode7.AddClass("is-narrow");
			htmlNode7.AddClass("is-responsive");
			HtmlNode htmlNode8 = doc.CreateElement("tbody");
			if (previousRun == null)
			{
				HtmlNode newChild = HtmlNode.CreateNode("<thead><tr><th colspan='4'><b>" + tcr.Name + "</b></th><th></th></tr></thead>");
				htmlNode7.AppendChild(newChild);
				foreach (TestMethodRun testMethod in tcr.TestMethods)
				{
					WriteTestMethodResult(testMethod, doc, htmlNode8);
				}
			}
			else
			{
				HtmlNode newChild2 = HtmlNode.CreateNode("<colgroup><col width=\"30%\" /><col width=\"10%\" /><col width=\"10%\" /><col width=\"40%\" /><col width=\"10%\" /></colgroup>");
				htmlNode7.AppendChild(newChild2);
				HtmlNode newChild3 = HtmlNode.CreateNode("<thead><tr><th><b>Name</b></th><th>Previous Run Status</th><th>Current Run Status</th><th>Error Message</th><th>Duration (Seconds)</th></tr></thead>");
				htmlNode7.AppendChild(newChild3);
				foreach (TestMethodRun testMethod2 in tcr.TestMethods)
				{
					WriteTestMethodResultWithPrevious(testMethod2, doc, htmlNode8);
				}
			}
			htmlNode7.AppendChild(htmlNode8);
			HtmlNode htmlNode9 = doc.CreateElement("div");
			htmlNode9.AddClass("table-container");
			htmlNode9.SetAttributeValue("style", "overflow-y: auto;max-height:400px;");
			htmlNode9.AppendChild(htmlNode7);
			htmlNode6.AppendChild(htmlNode9);
			htmlNode5.AppendChild(htmlNode6);
			htmlNode2.AppendChild(htmlNode5);
			section.AppendChild(htmlNode2);
		}

		private void WriteTestMethodResult(TestMethodRun m, HtmlDocument doc, HtmlNode tbody)
		{
			HtmlNode htmlNode = doc.CreateElement("tr");
			HtmlNode htmlNode2 = doc.CreateElement("td");
			htmlNode2.InnerHtml = GetClassNameFromFullName(m.TestClass) + "." + m.TestMethodName;
			htmlNode.AppendChild(htmlNode2);
			HtmlNode htmlNode3 = doc.CreateElement("td");
			string status = m.Status;
			if (!(status == "Failed"))
			{
				if (status == "Passed")
				{
					AppendPassed(m, doc, htmlNode3);
				}
				else
				{
					AppendIgnored(m, doc, htmlNode3);
				}
			}
			else
			{
				AppendFailed(m, doc, htmlNode3);
			}
			htmlNode.AppendChild(htmlNode3);
			AppendErrorInfo(m, doc, htmlNode);
			HtmlNode htmlNode4 = doc.CreateElement("td");
			htmlNode4.InnerHtml = m.Duration.TotalSeconds.ToString("0.00");
			htmlNode4.SetAttributeValue("style", "text-align: right;");
			htmlNode.AppendChild(htmlNode4);
			tbody.AppendChild(htmlNode);
		}

		private void WriteTestMethodResultWithPrevious(TestMethodRun m, HtmlDocument doc, HtmlNode tbody)
		{
			TestMethodRun testMethodRun = (from x in previousRun.TestClassList.Where((TestClassRun x) => x.FullName == m.TestClass).SelectMany((TestClassRun x) => x.TestMethods)
										   where x.TestMethodName == m.TestMethodName
										   select x).FirstOrDefault();
			HtmlNode htmlNode = doc.CreateElement("tr");
			HtmlNode htmlNode2 = doc.CreateElement("td");
			htmlNode2.InnerHtml = m.TestMethodName ?? "";
			htmlNode.AppendChild(htmlNode2);
			HtmlNode htmlNode3 = doc.CreateElement("td");
			if (testMethodRun != null)
			{
				if (testMethodRun.Status == "Passed")
				{
					AppendPassed(m, doc, htmlNode3);
				}
				else if (testMethodRun.Status == "Failed")
				{
					AppendFailed(m, doc, htmlNode3);
				}
				else
				{
					AppendIgnored(m, doc, htmlNode3);
				}
			}
			else
			{
				AppendIgnored(m, doc, htmlNode3);
			}
			htmlNode.AppendChild(htmlNode3);
			HtmlNode htmlNode4 = doc.CreateElement("td");
			string status = m.Status;
			if (!(status == "Failed"))
			{
				if (status == "Passed")
				{
					AppendPassed(m, doc, htmlNode4);
				}
				else
				{
					AppendIgnored(m, doc, htmlNode4);
				}
			}
			else
			{
				AppendFailed(m, doc, htmlNode4);
			}
			htmlNode.AppendChild(htmlNode4);
			AppendErrorInfo(m, doc, htmlNode);
			HtmlNode htmlNode5 = doc.CreateElement("td");
			htmlNode5.InnerHtml = m.Duration.TotalSeconds.ToString("0.00");
			htmlNode5.SetAttributeValue("style", "text-align: right;");
			htmlNode.AppendChild(htmlNode5);
			if (testMethodRun!= null && testMethodRun.Status == "Passed" && m.Status == "Failed")
			{
				htmlNode.SetAttributeValue("style", "background-color: yellow;");
			}
			tbody.AppendChild(htmlNode);
		}

		private string GetClassNameFromFullName(string fullName)
		{
			int num = fullName.IndexOf(' ');
			if (num > 0)
			{
				return fullName.Substring(0, num - 1);
			}
			return fullName;
		}

		private void AppendMethodBullet(TestMethodRun m, HtmlDocument doc, HtmlNode tr, string cssClass, string overColor, string outColor)
		{
			HtmlNode htmlNode = doc.CreateElement("p");
			htmlNode.AddClass(cssClass);
			htmlNode.SetAttributeValue("title", "Click to see the StackTrace");
			htmlNode.SetAttributeValue("onmouseover", "this.style.color=\"" + overColor + "\"");
			htmlNode.SetAttributeValue("onmouseout", "this.style.color=\"" + outColor + "\"");
			htmlNode.SetAttributeValue("onclick", string.Format("togle('{0}')", m.GetHashCode()));
			tr.AppendChild(htmlNode);
		}

		private void AppendIgnored(TestMethodRun m, HtmlDocument doc, HtmlNode tr)
		{
			AppendMethodBullet(m, doc, tr, "testIgnore", "white", "yellow");
		}

		private void AppendPassed(TestMethodRun m, HtmlDocument doc, HtmlNode tr)
		{
			AppendMethodBullet(m, doc, tr, "testOk", "green", "lime");
		}

		private void AppendFailed(TestMethodRun m, HtmlDocument doc, HtmlNode tr)
		{
			AppendMethodBullet(m, doc, tr, "testKo", "orange", "red");
		}

		private void AppendErrorInfo(TestMethodRun m, HtmlDocument doc, HtmlNode tr)
		{
			HtmlNode htmlNode = doc.CreateElement("td");
			htmlNode.InnerHtml = m.Description;
			htmlNode.AppendChild(doc.CreateElement("br"));
			htmlNode.InnerHtml += m.ErrorInfo.Message;
			HtmlNode htmlNode2 = doc.CreateElement("div");
			htmlNode2.SetAttributeValue("id", m.GetHashCode().ToString());
			htmlNode2.AddClass("trace");
			htmlNode2.SetAttributeValue("style", "display:none;");
			HtmlNode htmlNode3 = doc.CreateElement("div");
			htmlNode3.AddClass("border");
			htmlNode3.InnerHtml += m.ErrorInfo.StdOut;
			htmlNode3.AppendChild(doc.CreateElement("br"));
			string innerHtml = htmlNode3.InnerHtml;
			object stdErr = m.ErrorInfo.StdErr;
			htmlNode3.InnerHtml = innerHtml + ((stdErr != null) ? stdErr.ToString() : null);
			htmlNode3.AppendChild(doc.CreateElement("br"));
			htmlNode3.InnerHtml += m.ErrorInfo.Message;
			htmlNode3.AppendChild(doc.CreateElement("br"));
			htmlNode3.InnerHtml += m.ErrorInfo.StackTrace;
			htmlNode2.AppendChild(htmlNode3);
			if (m.ErrorInfo.StackTrace != null)
			{
				HtmlNode htmlNode4 = doc.CreateElement("pre");
				htmlNode4.AddClass("failureInfo");
				htmlNode4.InnerHtml = m.ErrorInfo.StackTrace;
				htmlNode2.AppendChild(htmlNode4);
			}
			htmlNode.AppendChild(htmlNode2);
			tr.AppendChild(htmlNode);
		}
	}
}