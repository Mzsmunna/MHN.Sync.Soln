using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Canvas.Wmf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using Image = iText.Layout.Element.Image;
using Path = System.IO.Path;
using TabAlignment = iText.Layout.Properties.TabAlignment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MHN.Sync.Telemetry;
using MHN.Sync.Entity;
using MHN.Sync.Utility.SFTP;
using MHN.Sync.Utility.FTP;
using MHN.Sync.Helper;
using MHN.Sync.Entity.MongoEntity;

namespace MHN.Export.Soln.Helpers.PdfGenerate
{
    public static class PdfGeneratorForPTC
    {
        private static string BaseAddress = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static List<SectionPageNumber> tableOfContent;
        private static Color paragraphColor = new DeviceRgb(31, 40, 91);
        private static Paragraph SectionParagraph = new Paragraph();

        public static string GeneratePdf(PTCRequest memberContent, JobManagerResult result, SFTPUtility sFTPUtility)
        {
            try
            {
                if (memberContent.MemberName.Length > 1)
                {
                    memberContent.MemberName = memberContent.MemberName.ToUpper();
                }

                tableOfContent = new List<SectionPageNumber>();
                var dateString = DateTime.Now.Date.ToString("MMddyyyy");

                var memberIdFileName = memberContent.MemberId.Replace("*", "-");
                string fileLocation = BaseAddress + @"\Template\Output.pdf";
                string generatedFileName = "INSIGHT_GTWY_" + memberIdFileName + "_" + memberContent.PlanType + "_" + dateString + ".pdf";
                if (memberContent.PlanType == PlanType.TRANSITION.ToString() || memberContent.PlanType == PlanType.INT_REASSESS.ToString())
                {
                    generatedFileName = "INSIGHT_GTWY_" + memberIdFileName + "_" + memberContent.PlanType.Replace("_", "-") + "_" + dateString + "_" + memberContent.SubmitId + ".pdf";
                }
                string sourchPdfFileName = BaseAddress + @"\Template\CarePlan-" + memberContent.PlanType;
                if (memberContent.Language.ToLower() == "spanish")
                {
                    sourchPdfFileName = BaseAddress + @"\Template\CarePlan-Spanish-" + memberContent.PlanType;
                }

                PdfDocument pdf = new PdfDocument(new PdfReader(sourchPdfFileName + ".pdf"), new PdfWriter(sourchPdfFileName + "-tmp.pdf"));
                FillupFields(pdf, memberContent);
                pdf.Close();
                var contentCount = 1;
                if (memberContent.PdfContents.Count != 0)
                {
                    PdfDocument pdfDocument = new PdfDocument(new PdfWriter(BaseAddress + @"\Template\Result.pdf"));
                    pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new MyEventHandler(BaseAddress));
                    Document document = new Document(pdfDocument, new PageSize((float)8.5 * 72, (float)11.0 * 72));
                    document.SetTopMargin(80);
                    document.SetBottomMargin(50);
                    document.SetRightMargin(45);
                    foreach (var pdfContent in memberContent.PdfContents)
                    {
                        GenerateSection(document, pdfContent, pdfDocument);
                        var sectionPageNumber = new SectionPageNumber
                        {
                            Section = pdfContent.Section,
                        };
                        var count = 0;
                        foreach (var subSestion in pdfContent.SubSestions)
                        {
                            GenerateParagraph(document, subSestion, count, pdfDocument, sectionPageNumber);
                            count++;
                        }
                        sectionPageNumber.EndPageNumber = (pdfDocument.GetNumberOfPages() + 2).ToString();
                        if (contentCount != memberContent.PdfContents.Count)
                        {
                            document.Add(new Paragraph("").SetMarginBottom(15));
                        }
                        tableOfContent.Add(sectionPageNumber);
                        contentCount++;
                    }
                    document.Close();
                }

                PdfDocument pdfDocumentMerge = new PdfDocument(new PdfWriter(fileLocation));
                PdfMerger merger = new PdfMerger(pdfDocumentMerge);
                PdfDocument sourcePdf = new PdfDocument(new PdfReader(sourchPdfFileName + "-tmp.pdf"));
                merger.Merge(sourcePdf, new[] { 1, 2, 3, 4 });
                PdfDocument sourcePdf2 = null;
                if (memberContent.PdfContents.Count != 0)
                {
                    sourcePdf2 = new PdfDocument(new PdfReader(BaseAddress + @"\Template\Result.pdf"));
                    merger.Merge(sourcePdf2, 1, sourcePdf2.GetNumberOfPages());
                }
                int notePages = 4;
                if (memberContent.PlanType == PlanType.HEDIS.ToString())
                {
                    merger.Merge(sourcePdf, 10, 13);
                    var totalPages = pdfDocumentMerge.GetNumberOfPages() + 1;
                    notePages = 4 - (totalPages % 4);
                    for (int i = 0; i < notePages; i++)
                    {
                        merger.Merge(sourcePdf, 17, 17);
                    }
                    merger.Merge(sourcePdf, 18, 18);
                }
                else
                {
                    merger.Merge(sourcePdf, 13, 16);
                    var totalPages = pdfDocumentMerge.GetNumberOfPages() + 1;
                    notePages = 4 - (totalPages % 4);
                    for (int i = 0; i < notePages; i++)
                    {
                        merger.Merge(sourcePdf, 17, 17);
                    }
                    merger.Merge(sourcePdf, 18, 18);
                }

                sourcePdf.Close();
                if (memberContent.PdfContents.Count != 0)
                    sourcePdf2.Close();

                GeneratePageNumber(pdfDocumentMerge, notePages);
                GenerateTableOfContent(pdfDocumentMerge, memberContent);

                pdfDocumentMerge.Close();

                var contents = File.ReadAllBytes(fileLocation);
                if (ApplicationConstants.ManualProcess)
                {
                    File.WriteAllBytes(BaseAddress + @"\Output\" + generatedFileName, contents);
                }
                else
                {
                    File.WriteAllBytes(BaseAddress + @"\Output\tmpPdf.pdf", contents);
                    if (memberContent.PlanType == PlanType.TRANSITION.ToString() || memberContent.PlanType == PlanType.INT_REASSESS.ToString())
                    {
                        sFTPUtility.UploadFile(contents, ApplicationConstants.ExportCareplanContentLocation, @"/" + generatedFileName);
                    }
                    MhnFtpsUtility.UploadFile(contents, ApplicationConstants.ExportCareplanContentLocationGateway, @"/" + generatedFileName);
                }

                return generatedFileName;
            }
            catch (Exception exception)
            {
                result.Message.CustomAppender(Environment.NewLine + "Exception for file " + memberContent.FolderLocation + " in GenerateDocument due to " + exception.Message);
                if (exception.InnerException != null)
                {
                    result.Message.CustomAppender(Environment.NewLine + exception.InnerException.Message);
                }
                if (!string.IsNullOrEmpty(ApplicationConstants.InstrumentationKey))
                {
                    TelemetryLogger.LogException(exception);
                }

                return "";
            }
        }
        private static void GenerateTableOfContent(PdfDocument toPdfDocument, CareplanMemberContent memberContent)
        {
            //PdfDocument existing = new PdfDocument(new PdfReader(BaseAddress + @"\Template\CarePlan-HEDIS.pdf"));
            //PdfPage page = existing.GetPage(3);
            //var pageSize = page.GetPageSize();

            //PdfDocument toPdfDocument = new PdfDocument(new PdfWriter(@"E:\test.pdf"));
            //PdfFormXObject xObject2 = page.CopyAsFormXObject(toPdfDocument);
            //Image img2 = new Image(xObject2);
            //img2.ScaleToFit(pageSize.GetWidth(), pageSize.GetHeight());
            //Document toDocument = new Document(toPdfDocument, new PageSize((float)8.5 * 72, (float)11.0 * 72));
            //toDocument.SetMargins(0, 0, 0, 0);
            //toDocument.Add(img2);

            //var page1 = toPdfDocument.GetPage(toPdfDocument.GetNumberOfPages());
            var page1 = toPdfDocument.GetPage(3);

            //toPdfDocument.AddNewPage();
            //Document document = new Document(toPdfDocument);

            PdfCanvas pdfCanvas1 = new PdfCanvas(page1.NewContentStreamAfter(), page1.GetResources(), toPdfDocument);

            //PdfCanvas pdfCanvas2 = new PdfCanvas(page1.NewContentStreamAfter(), page1.GetResources(), toPdfDocument);
            //pdfCanvas2.SetColor(new DeviceRgb(32, 40, 92), true).Fill().Stroke();



            //for (int i = 0; i < tableOfContent.Count; i++)
            //{
            //    Paragraph paragraph = new Paragraph(tableOfContent.Keys.ElementAt(i) + " - " + tableOfContent[tableOfContent.Keys.ElementAt(i)]);
            //    document.Add(paragraph);
            //}
            //document.Close();

            var paragraphFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Book.otf", true);

            List<TabStop> tabstops = new List<TabStop>();
            tabstops.Add(new TabStop(300, TabAlignment.RIGHT, new DottedLine()));

            for (int i = 0; i < tableOfContent.Count; i++)
            {
                //Console.WriteLine(tableOfContent.Keys.ElementAt(i) + " - " + tableOfContent[tableOfContent.Keys.ElementAt(i)]);
                var rectangle = new Rectangle(0, (540 - i * 50), (float)8.5 * 72, 12);
                Canvas canvas = new Canvas(pdfCanvas1, toPdfDocument, rectangle);

                //var p = new Paragraph()
                //    .AddTabStops(tabstops)
                //    .Add(tableOfContent.Keys.ElementAt(i))
                //    .Add(new Tab())
                //    .Add(tableOfContent[tableOfContent.Keys.ElementAt(i)].ToString())
                //    .SetAction(PdfAction.CreateGoTo(tableOfContent.Keys.ElementAt(i)));

                var imageUrl = memberContent.PdfContents.FirstOrDefault(x => x.Section == tableOfContent[i].Section);
                if (imageUrl != null)
                {
                    if (!string.IsNullOrEmpty(imageUrl.SectionImage))
                    {
                        Image image = new Image(ImageDataFactory.Create(BaseAddress + @"\Template\icon-" + imageUrl.SectionImage))
                        .SetFixedPosition(130, (542 - i * 50))
                        .ScaleToFit(40, 40);

                        canvas.Add(image);
                    }

                    var p = new Paragraph()
                    .SetFixedPosition(190, (546 - i * 50), 400)

                    .AddTabStops(tabstops)
                    .Add(new Text(tableOfContent[i].Section))
                    .Add(new Tab())
                    .Add(tableOfContent[i].StartPageNumber + "-" + tableOfContent[i].EndPageNumber);

                    canvas.Add(
                        p
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFontColor(paragraphColor)
                        .SetFont(paragraphFont)
                        .SetFontSize(16));

                    canvas.Close();
                }
            }

            //toPdfDocument.Close();
            //toDocument.Close();
        }

        private static void FillupFields(PdfDocument pdfDocument, PTCRequest memberContent)
        {
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, false);
            if (form != null)
            {
                var today = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                IDictionary<String, PdfFormField> fields = form.GetFormFields();
                PdfFormField toSet;

                var subTitleFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Bold.otf", true);
                var paragraphFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Book.otf", true);
                var paragraphBoldFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Black.otf", true);
                var nameFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\Gotham-ExtraLight.otf", true);

                var phoneFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamXNarrow-Bold.otf", true);

                fields.TryGetValue("txtFLName", out toSet);
                if (toSet != null)
                {
                    if (memberContent.MemberName.Length <= 20)
                    {
                        toSet.SetFontSize(22);
                    }
                    else
                    {
                        toSet.SetFontSize(20);
                    }
                    toSet.SetColor(new DeviceRgb(12, 125, 158));
                    toSet.SetFont(nameFont);
                    toSet.SetValue(memberContent.MemberName);
                }

                fields.TryGetValue("txtFLName2", out toSet);
                if (toSet != null)
                {
                    toSet.SetColor(paragraphColor);
                    toSet.SetFontSize(12);
                    toSet.SetFont(subTitleFont);
                    toSet.SetValue(memberContent.MemberName + ",");
                }

                fields.TryGetValue("txtCarePlanDate", out toSet);
                if (toSet != null)
                {
                    toSet.SetColor(paragraphColor);
                    toSet.SetFontSize(12);
                    toSet.SetFont(subTitleFont);
                    toSet.SetValue(today);
                }

                fields.TryGetValue("txtPhoneNo1", out toSet);
                if (toSet != null)
                {
                    toSet.SetColor(paragraphColor);
                    toSet.SetFontSize(12);
                    toSet.SetFont(paragraphBoldFont);
                    toSet.SetValue(memberContent.PhoneNumber ?? "");
                }

                fields.TryGetValue("txtPhoneNo2", out toSet);
                if (toSet != null)
                {
                    toSet.SetColor(paragraphColor);
                    toSet.SetFontSize(12);
                    toSet.SetFont(subTitleFont);
                    toSet.SetValue(memberContent.PhoneNumber ?? "");
                }

                fields.TryGetValue("txtNameAddress", out toSet);
                if (toSet != null)
                {
                    var addressString = string.Empty;

                    if (memberContent.PlanType == PlanType.HEDIS.ToString())
                    {
                        addressString = memberContent.MemberName.ToUpper() + "\n" + memberContent.AddressLine1.ToUpper() +
                                        "\n" + memberContent.AddressLine2.ToUpper();
                    }
                    else if (memberContent.PlanType == PlanType.INT_REASSESS.ToString() || memberContent.PlanType == PlanType.TRANSITION.ToString())
                    {
                        addressString = memberContent.MemberName.ToUpper();
                        if (!string.IsNullOrEmpty(memberContent.AddressLine2))
                        {
                            addressString += "\n" + memberContent.AddressLine2.ToUpper();
                        }
                        else
                        {
                            addressString = "\n" + addressString;
                        }
                        if (!string.IsNullOrEmpty(memberContent.AddressLine1))
                        {
                            addressString += "\n" + memberContent.AddressLine1.ToUpper();
                        }


                        if (!string.IsNullOrEmpty(memberContent.LastLine))
                        {
                            addressString += "\n" + memberContent.LastLine.ToUpper();
                        }
                        else if (!string.IsNullOrEmpty(memberContent.City))
                        {
                            addressString += "\n" + memberContent.City.ToUpper();
                            if (!string.IsNullOrEmpty(memberContent.State))
                            {
                                addressString += " " + memberContent.State.ToUpper();
                            }
                            if (!string.IsNullOrEmpty(memberContent.Zip))
                            {
                                addressString += " " + memberContent.Zip.ToUpper();
                            }
                        }
                    }

                    //toSet.SetFontSize(11);
                    //toSet.SetFont(paragraphFont);
                    toSet.SetValue(addressString);
                }

                fields.TryGetValue("txtComplienceMemberCode", out toSet);
                if (toSet != null)
                {
                    string txtComplienceMemberCode = GetComplienceCode(memberContent);
                    toSet.SetValue(txtComplienceMemberCode + "\n" + memberContent.SubmitId);
                    //toSet.SetFontSize(12);
                    //toSet.SetFont(paragraphFont);
                }

                form.FlattenFields();
            }
        }
        public static string GetComplienceCode(PTCRequest memberContent)
        {
            string txtComplienceMemberCode = "";
            if ((memberContent.PlanType == PlanType.HEDIS.ToString() && memberContent.Language.ToLower() != "spanish") || memberContent.PlanType == PlanType.INT_REASSESS.ToString())
            {
                txtComplienceMemberCode = "NS_1473A (12/2017)";
            }
            else if (memberContent.PlanType == PlanType.HEDIS.ToString() && memberContent.Language.ToLower() == "spanish")
            {
                txtComplienceMemberCode = "NS_1487ASP (12/2017)";
            }
            else if (memberContent.PlanType == PlanType.TRANSITION.ToString() && memberContent.Language.ToLower() == "spanish")
            {
                txtComplienceMemberCode = "NS_1482ASP (12/2017)";
            }
            else if (memberContent.PlanType == PlanType.TRANSITION.ToString())
            {
                txtComplienceMemberCode = "NS_1482A (11/2017)";
            }
            return txtComplienceMemberCode;
        }
        public static void GeneratePageNumber(PdfDocument pdfDocument, int notePages)
        {
            var pageNumberFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Book.otf", true);
            int afterNotePageStart = 2 + notePages;
            int afterContentPageFinish = 4 + afterNotePageStart;
            var totalPages = pdfDocument.GetNumberOfPages();
            for (int i = 4; i < totalPages - 1; i++)
            {
                var pageNumber = i - 1;
                PdfPage page = pdfDocument.GetPage(i + 1);
                var pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDocument);

                if (pageNumber % 2 == 0)
                {
                    pdfCanvas
                        .BeginText().SetFontAndSize(pageNumberFont, 12)
                        .MoveText(40, 40)
                        .SetColor(new DeviceRgb(32, 40, 92), true)
                        .ShowText(pageNumber.ToString())
                        .SetColor(new DeviceRgb(32, 40, 92), true)
                        .EndText();
                }
                else
                {
                    if (pageNumber >= totalPages - afterNotePageStart)
                    {
                        pdfCanvas
                            .BeginText().SetFontAndSize(pageNumberFont, 12)
                            .MoveText(pageSize.GetWidth() - 40, 40)
                            .SetColor(new DeviceRgb(32, 40, 92), true)
                            .ShowText(pageNumber.ToString())
                            .SetColor(new DeviceRgb(32, 40, 92), true)
                            .EndText();
                    }
                    else if (pageNumber >= totalPages - afterContentPageFinish && pageNumber < totalPages - afterNotePageStart)
                    {
                        pdfCanvas
                            .BeginText().SetFontAndSize(pageNumberFont, 12)
                            .MoveText(pageSize.GetWidth() - 40, 40)
                            .SetColor(new DeviceRgb(255, 255, 255), true)
                            .ShowText(pageNumber.ToString())
                            .SetColor(new DeviceRgb(32, 40, 92), true)
                            .EndText();
                    }
                    else
                    {
                        pdfCanvas
                            .BeginText().SetFontAndSize(pageNumberFont, 12)
                            .MoveText(pageSize.GetWidth() - 40, 40)
                            .SetColor(new DeviceRgb(32, 40, 92), true)
                            .ShowText(pageNumber.ToString())
                            .SetColor(new DeviceRgb(32, 40, 92), true)
                            .EndText();
                    }
                }
            }
        }

        public static void GenerateSection(Document document, PdfContent pdfContent, PdfDocument pdfDocument)
        {
            if (!string.IsNullOrEmpty(pdfContent.SectionImage))
            {
                //Paragraph paragraph = new Paragraph();
                Image eatImage = new Image(ImageDataFactory.Create(BaseAddress + @"\Template\" + pdfContent.SectionImage));
                SectionParagraph.Add(eatImage.ScaleToFit(530, 310));
                //document.Add(paragraph.SetKeepWithNext(true));
            }
        }

        public static void GenerateParagraph(Document document, SubSection subSestion, int count, PdfDocument pdfDocument, SectionPageNumber sectionPageNumber)
        {
            var subTitleFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Bold.otf", true);
            var paragraphFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Book.otf", true);
            var paragraphBoldFont = PdfFontFactory.CreateFont(BaseAddress + @"\Template\fonts\GothamNarrow-Black.otf", true);

            Paragraph paragraph = new Paragraph().SetMarginLeft(75).SetWidth(530);
            if (count > 0)
            {
                paragraph.SetBorderTop(new SolidBorder(0.5f));
                paragraph.SetMarginTop(30);
            }
            paragraph.SetMarginBottom(-8);


            if (!string.IsNullOrEmpty(subSestion.Header))
            {
                if (subSestion.Header.Contains("<nl>"))
                {
                    var startNewLineIndex = subSestion.Header.IndexOf("<nl>");
                    var beforeNewLineText = subSestion.Header.Substring(0, startNewLineIndex);
                    paragraph.Add(
                    new Text(beforeNewLineText.ToUpper()).SetFont(subTitleFont)
                        .SetFontSize(14)
                        .SetFontColor(paragraphColor));

                    var endNewLineText = subSestion.Header.Substring(startNewLineIndex + 4, subSestion.Header.Length - startNewLineIndex - 4);
                    if (endNewLineText.Contains("<nl>"))
                    {
                        startNewLineIndex = endNewLineText.IndexOf("<nl>");
                        beforeNewLineText = endNewLineText.Substring(0, startNewLineIndex);
                        endNewLineText = endNewLineText.Substring(startNewLineIndex + 4, endNewLineText.Length - startNewLineIndex - 4);

                        var tmpParagraph2 = new Paragraph()
                                    .SetMargin(0)
                                    .SetMarginTop(-5);
                        tmpParagraph2.Add(new Text(beforeNewLineText.ToUpper())
                            .SetFont(subTitleFont)
                            .SetFontColor(paragraphColor)
                            .SetFontSize(14));
                        paragraph.Add(tmpParagraph2);
                    }

                    var tmpParagraph = new Paragraph()
                                    .SetMargin(0)
                                    .SetMarginTop(-5);
                    tmpParagraph.Add(new Text(endNewLineText.ToUpper())
                        .SetFont(subTitleFont)
                        .SetFontColor(paragraphColor)
                        .SetFontSize(14));
                    paragraph.Add(tmpParagraph);

                    SectionParagraph.Add(paragraph);
                    SectionParagraph.SetMarginBottom(-8);
                    SectionParagraph.SetKeepTogether(true);

                    document.Add(SectionParagraph.SetKeepWithNext(true));
                    SectionParagraph = new Paragraph();
                }
                else
                {
                    paragraph.Add(
                    new Text(subSestion.Header.ToUpper()).SetFont(subTitleFont)
                        .SetFontSize(14)
                        .SetFontColor(paragraphColor));
                    SectionParagraph.Add(paragraph);
                    SectionParagraph.SetMarginBottom(-8);
                    SectionParagraph.SetKeepTogether(true);

                    document.Add(SectionParagraph.SetKeepWithNext(true));
                    SectionParagraph = new Paragraph();
                }
            }
            else
            {
                document.Add(SectionParagraph.SetKeepWithNext(true));
                SectionParagraph = new Paragraph();
            }

            var paragraphCount = 1;
            foreach (var content in subSestion.Contents)
            {
                Paragraph paragraph2 = new Paragraph().SetMarginLeft(75).SetFixedLeading(15).SetKeepTogether(true);
                if (paragraphCount > 1)
                {
                    paragraph2.SetMarginTop(22);
                }

                int startIndex = content.IndexOf("<b>");
                if (startIndex >= 0)
                {
                    int endIndex = content.IndexOf("</b>", startIndex);
                    if (endIndex >= 0)
                    {
                        var data1 = content.Substring(0, startIndex);
                        paragraph2.Add(new Text(data1).SetFont(paragraphFont).SetFontColor(paragraphColor).SetFontSize(12));

                        var data2 = content.Substring(startIndex + 3, endIndex - startIndex - 3);
                        if (!string.IsNullOrEmpty(data2))
                        {
                            if (data2.Contains("<nl>"))
                            {
                                data2 = data2.Replace("<nl>", "");

                                var tmpParagraph = new Paragraph()
                                    .SetFixedLeading(15)
                                    .SetMargin(0);

                                tmpParagraph.Add(new Text(data2)
                                    .SetFont(paragraphBoldFont)
                                    .SetFontColor(paragraphColor)
                                    .SetFontSize(12));

                                paragraph2.Add(tmpParagraph);
                            }
                            else
                            {
                                paragraph2.Add(new Text(data2).SetFont(paragraphBoldFont).SetFontColor(paragraphColor).SetFontSize(12));
                            }
                        }

                        var data3 = content.Substring(endIndex + 4, content.Length - endIndex - 4);
                        paragraph2.Add(new Text(data3).SetFont(paragraphFont).SetFontColor(paragraphColor).SetFontSize(12));
                    }
                }
                else
                {
                    paragraph2.Add(new Text(content).SetFont(paragraphFont).SetFontColor(paragraphColor).SetFontSize(12));
                }

                document.Add(paragraph2);

                if (count == 0 && paragraphCount == 1)
                {
                    sectionPageNumber.StartPageNumber = (pdfDocument.GetNumberOfPages() + 2).ToString();
                }

                paragraphCount++;
            }
        }

        public static string CareplanCSV(CareplanMemberContent memberContent)
        {
            //Generate CSV Document
            string fileLocation = null;
            var memberIdFolder = memberContent.MemberId.Replace("*", "-");
            CsvExport<CareplanContentDetails> csv = new CsvExport<CareplanContentDetails>(memberContent.Contents);
            byte[] data = csv.ExportToBytes();
            using (Stream stream = new MemoryStream(data))
            {
                fileLocation = memberContent.FolderLocation + @"\" + memberIdFolder + ".csv";// + HelperUtility.GenerateFileName(CareplanType.HEDIS);

                //Directory.CreateDirectory(fileLocation);
                using (var fileStream = File.Create(fileLocation))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }
            return memberIdFolder + ".csv";
        }
    }
}
