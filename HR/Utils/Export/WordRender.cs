using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace HR.Utils.Export
{
    public static class WordRender
    {
        private static void ReplaceParserTag(this OpenXmlElement elem, Dictionary<string, string> data)
        {
            var pool = new List<Run>();
            var matchText = string.Empty;
            var hiliteRuns = elem.Descendants<Run>() //找出鮮明提示
                .Where(o => o.RunProperties?.Elements<Highlight>().Any() ?? false).ToList();

            foreach (var run in hiliteRuns)
            {
                var t = run.InnerText;
                if (t.StartsWith("["))
                {
                    pool = new List<Run> { run };
                    matchText = t;
                }
                else
                {
                    matchText += t;
                    pool.Add(run);
                }

                if (t.EndsWith("]"))
                {
                    var m = Regex.Match(matchText, @"\[(?<n>\w+)\]");
                    if (m.Success && data.ContainsKey(m.Groups["n"].Value))
                    {
                        var firstRun = pool.First();
                        firstRun.RemoveAllChildren<Text>();
                        firstRun.RunProperties?.RemoveAllChildren<Highlight>();
                        var newText = data[m.Groups["n"].Value];
                        var firstLine = true;
                        foreach (var line in Regex.Split(newText, @"\\n"))
                        {
                            if (firstLine) firstLine = false;
                            else firstRun.Append(new Break());
                            firstRun.Append(new Text(line));
                        }
                        pool.Skip(1).ToList().ForEach(o => o.Remove());
                    }
                }
            }
        }

        public static byte[] GenerateDocx(byte[] template, Dictionary<string, string> data, Dictionary<string, string> experiences)
        {
            using MemoryStream ms = new();
            ms.Write(template, 0, template.Length);
            using (var docx = WordprocessingDocument.Open(ms, true))
            {
                docx.MainDocumentPart?.HeaderParts.ToList().ForEach(hdr =>
                {
                    hdr.Header.ReplaceParserTag(data);
                });

                docx.MainDocumentPart?.FooterParts.ToList().ForEach(ftr =>
                {
                    ftr.Footer.ReplaceParserTag(data);
                });

                docx.MainDocumentPart?.Document.Body?.ReplaceParserTag(data);

                #region 工作經驗表格
                Table table = docx.MainDocumentPart.Document.Body.Elements<Table>().First();
                TableRow baseline = table.Elements<TableRow>().ElementAt(6);
                TableRow row = table.Elements<TableRow>().ElementAt(7);
                if (experiences.ContainsKey("ExperienceCount"))
                {
                    int expCount = Convert.ToInt32(experiences["ExperienceCount"]);
                    for (int i = 1; i <= expCount; i++)
                    {
                        TableRow copyRow = (TableRow)row.CloneNode(true);

                        Dictionary<string, string> expDict = new()
                        {
                            { "Agency", $"{experiences[$"Experience{i}-Agency"]}" },
                            { "JobTitle", $"{experiences[$"Experience{i}-JobTitle"]}" },
                            { "ServeDateStart", $"自 {Convert.ToDateTime(experiences[$"Experience{i}-ServeDateStart"]):yyyy 年 MM 月}" },
                            { "ServeDateEnd", $"至 {Convert.ToDateTime(experiences[$"Experience{i}-ServeDateEnd"]):yyyy 年 MM 月}" },
                            { "Salary", $"{experiences[$"Experience{i}-SalaryStandar"]} {experiences[$"Experience{i}-Salary"]} 元" },
                            { "LeavingReason", $"{experiences[$"Experience{i}-LeavingReason"]}" }
                        };

                        copyRow.ReplaceParserTag(expDict);
                        table.InsertAfter(copyRow, baseline);
                    }
                    table.RemoveChild(row);
                }
                else
                {
                    Dictionary<string, string> expDict = new()
                    {
                        { "Agency", "" },
                        { "JobTitle", "" },
                        { "ServeDateStart", "" },
                        { "ServeDateEnd", "" },
                        { "Salary", "" },
                        { "LeavingReason", "" }
                    };
                    row.ReplaceParserTag(expDict);
                }
                #endregion

                docx.Save();
            }
            return ms.ToArray();
        }
    }
}
