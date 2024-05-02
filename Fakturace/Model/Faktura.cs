using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Drawing;
using System.Xml.Linq;
using Syncfusion.Pdf.Barcode;
using Microsoft.Maui.ApplicationModel;
using QRCoder;
using System.ComponentModel.DataAnnotations.Schema;
using static QRCoder.PayloadGenerator.SwissQrCode;
using SinKien.IBAN4Net;

namespace Fakturace.Model
{
    public class Faktura
    {
        public int Id { get; set; }
        public string CisloFaktury { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Zeme { get; set; }
        public string Mesto { get; set; }
        public string Ulice { get; set; }
        public string CisloPopisne { get; set; }
        public string Psc { get; set; }
        public string Ico { get; set; }
        public string Produkty { get; set; }
        public bool Odberatel { get; set; }

        [NotMapped] // Tato vlastnost nebude mapována do databáze
        public string[] ProduktyList { get; set; }

        RectangleF TotalPriceCellBounds = RectangleF.Empty;
        RectangleF QuantityCellBounds = RectangleF.Empty;
        // Vygenerování faktury
        public Faktura(string jmeno, string prijmeni, string zeme, string mesto, string ulice, string cisloPopisne, string psc, string ico, string produkty, bool odberatel)
        {
            CisloFaktury = DateTime.Today.ToString() + Id;/*Dnešní datum + číslo faktury (od 1 podle počtu existujících faktur)*/;
            Jmeno = jmeno;
            Prijmeni = prijmeni;
            Zeme = zeme;
            Mesto = mesto;
            Ulice = ulice;
            CisloPopisne = cisloPopisne;
            Psc = psc;
            Ico = ico;
            Produkty = produkty;
            Odberatel = odberatel;
            
            //Create a new PDF document.
            PdfDocument document = new PdfDocument();
            //Add a page to the document.
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;

            //Get the page width and height.
            float pageWidth = page.GetClientSize().Width;
            float pageHeight = page.GetClientSize().Height;
            RectangleF TotalPriceCellBounds = RectangleF.Empty;
            RectangleF QuantityCellBounds = RectangleF.Empty;
            //Set the header height.
            float headerHeight = 90;
            //Create a brush with a light blue color. 
            PdfColor lightBlue = new PdfColor(72, 116, 196);
            PdfBrush lightBlueBrush = new PdfSolidBrush(lightBlue);
            //Create a brush with a dark blue color. 
            PdfColor darkBlue = new PdfColor(48, 105, 226);
            PdfBrush darkBlueBrush = new PdfSolidBrush(darkBlue);
            //Create a brush with a white color. 
            PdfBrush whiteBrush = new PdfSolidBrush(new PdfColor(255, 255, 255));

            //Get the font file stream from the assembly. 
            Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
            string basePath = "Fakturace.Resources.Fonts.";
            Stream fontStream = assembly.GetManifestResourceStream("Fakturace.Resources.Fonts.arial.ttf");

            //Create a PdfTrueTypeFont from the stream with the different sizes. 
            PdfTrueTypeFont headerFont = new PdfTrueTypeFont(fontStream, 30, PdfFontStyle.Regular);
            PdfTrueTypeFont arialRegularFont = new PdfTrueTypeFont(fontStream, 18, PdfFontStyle.Regular);
            PdfTrueTypeFont arialBoldFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Bold);
            //Create a string format.
            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Center;
            format.LineAlignment = PdfVerticalAlignment.Middle;

            float y = 0;
            float x = 0;
            //Set the margins of the address.
            float margin = 30;
            //Set the line space.
            float lineSpace = 10;
            //Create a border pen and draw the border to on the PDF page. 
            PdfColor borderColor = new PdfColor(72, 116, 196);
            PdfPen borderPen = new PdfPen(borderColor, 1f);
            graphics.DrawRectangle(borderPen, new RectangleF(0, 0, pageWidth, pageHeight));

            //Create a new PdfGrid. 
            PdfGrid grid = new PdfGrid();
            //Add five columns to the grid.
            grid.Columns.Add(5);
            //Create the header row of the grid.
            PdfGridRow[] headerRow = grid.Headers.Add(1);
            //Set style to the header row and set value to the header cells. 
            headerRow[0].Style.BackgroundBrush = new PdfSolidBrush(new PdfColor(68, 114, 196));
            headerRow[0].Style.TextBrush = PdfBrushes.White;
            headerRow[0].Cells[0].Value = "ID Produktu";
            headerRow[0].Cells[0].StringFormat.Alignment = PdfTextAlignment.Center;
            headerRow[0].Cells[1].Value = "Název produktu";
            headerRow[0].Cells[2].Value = "Cena/Ks (Kč)";
            headerRow[0].Cells[3].Value = "Počet ks";
            headerRow[0].Cells[4].Value = "Celková cena (Kč)";
            //Add products to the grid table.

            if (Produkty != null)
            {
                int index = 0;

                ProduktyList = Produkty.Split("§");

                for (int i = 0; i < ProduktyList.Length / 5; i++)
                {
                    AddProducts(ProduktyList[index], ProduktyList[index + 1], double.Parse(ProduktyList[index + 2]), int.Parse(ProduktyList[index + 3]), double.Parse(ProduktyList[index + 4]), grid);
                    index += 5;
                }
            }
            
            #region Header         
            //Fill the header with a light blue brush. 
            graphics.DrawRectangle(lightBlueBrush, new RectangleF(0, 0, pageWidth, headerHeight));
            string title = "FAKTURA";
            //Specificy the bounds for the total value. 
            RectangleF headerTotalBounds = new RectangleF(400, 0, pageWidth - 400, headerHeight);
            //Measure the string size using the font. 
            Syncfusion.Drawing.SizeF textSize = headerFont.MeasureString(title);
            graphics.DrawString(title, headerFont, whiteBrush, new RectangleF(0, 0, textSize.Width + 50, headerHeight), format);
            //Draw a rectangle in the PDF page. 
            graphics.DrawRectangle(darkBlueBrush, headerTotalBounds);
            //Draw the total value to the PDF page. 
            graphics.DrawString(GetTotalAmount(grid).ToString() + " Kč", arialRegularFont, whiteBrush, new RectangleF(400, 0, pageWidth - 400, headerHeight + 10), format);
            //Create a font from the font stream. 
            arialRegularFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Regular);
            //Set the bottom line alignment and draw the text to the PDF page. 
            format.LineAlignment = PdfVerticalAlignment.Bottom;
            graphics.DrawString("Částka", arialRegularFont, whiteBrush, new RectangleF(400, 0, pageWidth - 400, headerHeight / 2 - arialRegularFont.Height), format);
            #endregion

            //Measure the string size using the font. 
            Syncfusion.Drawing.SizeF size = arialRegularFont.MeasureString($"Číslo faktury: {CisloFaktury}");
            y = headerHeight + margin;
            x = (pageWidth - margin) - size.Width;
            //Draw text to a PDF page with the provided font and location. 
            graphics.DrawString($"Číslo faktury: {CisloFaktury}", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            //Measure the string size using the font.
            size = arialRegularFont.MeasureString("Datum: " + DateTime.Now.ToString("d, MMMM yyyy"));
            x = (pageWidth - margin) - size.Width;
            y += arialRegularFont.Height + lineSpace;
            //Draw text to a PDF page with the provided font and location. 
            graphics.DrawString("Datum: " + DateTime.Now.ToString("d, MMMM yyyy"), arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));

            y = headerHeight + margin;
            x = margin;
            //Draw text to a PDF page with the provided font and location.
            if (Odberatel)
            {
                graphics.DrawString("Odběratel:", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            }
            else
            {
                graphics.DrawString("Dodavatel:", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            }

            y += arialRegularFont.Height + lineSpace;
            graphics.DrawString($"{jmeno} {prijmeni}", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            y += arialRegularFont.Height + lineSpace;
            graphics.DrawString($"{ulice} {cisloPopisne}", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            y += arialRegularFont.Height + lineSpace;
            graphics.DrawString($"{mesto}, {psc}", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            y += arialRegularFont.Height + lineSpace;
            graphics.DrawString($"{zeme}", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));
            y += arialRegularFont.Height + lineSpace;
            graphics.DrawString($"IČO: {ico}", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));

            #region Grid
            //Set the width of theto grid columns. 
            grid.Columns[0].Width = 110;
            grid.Columns[1].Width = 150;
            grid.Columns[2].Width = 110;
            grid.Columns[3].Width = 70;
            grid.Columns[4].Width = 100;
            for (int i = 0; i < grid.Headers.Count; i++)
            {
                //Set the height ofto the grid header row. 
                grid.Headers[i].Height = 20;
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    //Create a string format for the header cell. 
                    PdfStringFormat pdfStringFormat = new PdfStringFormat();
                    pdfStringFormat.LineAlignment = PdfVerticalAlignment.Middle;
                    pdfStringFormat.Alignment = PdfTextAlignment.Left;
                    //Set cell padding for header cell. 
                    if (j == 0 || j == 2)
                        grid.Headers[i].Cells[j].Style.CellPadding = new PdfPaddings(30, 1, 1, 1);
                    //Set a string format to the grid header cell. 
                    grid.Headers[i].Cells[j].StringFormat = pdfStringFormat;
                    //Set font to the grid header cell. 
                    grid.Headers[i].Cells[j].Style.Font = arialBoldFont;
                }
                //Set the value ofto the grid header cell. 
                grid.Headers[0].Cells[0].Value = "ID Produktu";
            }
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                //Set the height ofto the grid row. 
                grid.Rows[i].Height = 23;
                for (int j = 0; j < grid.Columns.Count; j++)
                {
                    //Create a string format for the grid row. 
                    PdfStringFormat pdfStringFormat = new PdfStringFormat();
                    pdfStringFormat.LineAlignment = PdfVerticalAlignment.Middle;
                    pdfStringFormat.Alignment = PdfTextAlignment.Left;

                    //Set cell padding for grid row cell. 
                    if (j == 0 || j == 2)
                        grid.Rows[i].Cells[j].Style.CellPadding = new PdfPaddings(30, 1, 1, 1);

                    //Seta string format to the grid row cell. 
                    grid.Rows[i].Cells[j].StringFormat = pdfStringFormat;
                    //Set the font to the grid row cell. 
                    grid.Rows[i].Cells[j].Style.Font = arialRegularFont;
                }
            }

            //Apply built-in table style to the grid. 
            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.ListTable4Accent5);
            //Subscribeing to begin the cell layout event.
            grid.BeginCellLayout += Grid_BeginCellLayout;
            //Draw the PDF grid to the PDF page and get the layout result. 
            PdfGridLayoutResult result = grid.Draw(page, new Syncfusion.Drawing.PointF(0, y + 40));

            //Using the layout result, continue to draw the text. 
            y = result.Bounds.Bottom + lineSpace;
            format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Center;
            RectangleF bounds = new RectangleF(QuantityCellBounds.X + 47, y, QuantityCellBounds.Width, QuantityCellBounds.Height);
            //Draw the text to the PDF page based on the layout result. 
            page.Graphics.DrawString("Celková cena:", arialRegularFont, PdfBrushes.Black, bounds, format);
            //Draw the total amount value to the PDF page based on the layout result. 
            bounds = new RectangleF(TotalPriceCellBounds.X + 435, y, TotalPriceCellBounds.Width, TotalPriceCellBounds.Height);
            page.Graphics.DrawString(GetTotalAmount(grid).ToString() + " Kč", arialRegularFont, PdfBrushes.Black, bounds);

            //Create a border pen with the custom dash style and draw the border to the page. 
            borderPen.DashStyle = PdfDashStyle.Custom;
            borderPen.DashPattern = new float[] { 3, 3 };
            graphics.DrawLine(borderPen, new Syncfusion.Drawing.PointF(0, pageHeight - 100), new Syncfusion.Drawing.PointF(pageWidth, pageHeight - 100));

            basePath = "Fakturace.Resources.Images.";
            //Get the image file stream from the assembly.
            Stream imageStream = assembly.GetManifestResourceStream(basePath + "profilovka.png");

            //Create a PDF bitmap image from the stream.
            PdfBitmap bitmap = new PdfBitmap(imageStream);
            //Draw the image to the PDF page. 
            graphics.DrawImage(bitmap, new RectangleF(10, pageHeight - 90, 80, 80));

            //Calculate the text position and draw the text to the PDF page. 
            y = pageHeight - 100 + margin;
            size = arialRegularFont.MeasureString("SPŠ Jičín");
            x = pageWidth - size.Width - margin;
            graphics.DrawString("SPŠ Jičín", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));

            //Calculate the text position and draw the text to the PDF page. 
            y += arialRegularFont.Height + lineSpace;
            size = arialRegularFont.MeasureString("Pod Koželuhy 100,  Jičín, Česká republika, 506 01");
            x = pageWidth - size.Width - margin;
            graphics.DrawString("Pod Koželuhy 100,  Jičín, Česká republika, 506 01", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));


            //Calculate the text position and draw the text to the PDF page. 
            y += arialRegularFont.Height + lineSpace; // Tady bylo původně y += 120, ale pak to nikde nebylo vidět...
            size = arialRegularFont.MeasureString("Máš dotaz? support@honda.cz");
            x = pageWidth - size.Width - margin;
            graphics.DrawString("Máš dotaz? support@honda.cz", arialRegularFont, PdfBrushes.Black, new Syncfusion.Drawing.PointF(x, y));



            //Kryštof

            string countryCode = "CZ";
            string accountNumber = "4020628063/0800";
            string[] casti = accountNumber.Split("/0");
            
            SinKien.IBAN4Net.Iban iban = new IbanBuilder()
                .CountryCode(CountryCode.GetCountryCode(countryCode))
                .BankCode(casti[1].ToString())
                .AccountNumber(casti[0].ToString())
                .Build();

            PdfQRBarcode qrBarcode = new PdfQRBarcode();
            qrBarcode.ErrorCorrectionLevel = PdfErrorCorrectionLevel.High;
            qrBarcode.XDimension = 2;

            float celkovaCastka = GetTotalAmount(grid);

            // Formát pro QR platbu dle evropského standardu SEPA
            string sepaQRCode = $"SPD*1.0*ACC:{iban}*AM:{celkovaCastka}*CC:CZK*MSG:Platba za fakturu {CisloFaktury}*X-VS:{CisloFaktury}";

            qrBarcode.Text = sepaQRCode;

            // Vykreslení textu "QR Barcode" (pouze pro demonstrační účely)
            page.Graphics.DrawString("QR Barcode", new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold), PdfBrushes.Black, new Syncfusion.Drawing.PointF(435, 560));

            // Vykreslení QR kódu na stránku PDF
            qrBarcode.Draw(page, new Syncfusion.Drawing.PointF(395, 540), new Syncfusion.Drawing.SizeF(100,100)); //(427, 575)

            //---------------------------------------------------------------------------------------------------------


            MemoryStream stream = new MemoryStream();
            document.Save(stream);

            // Uložení
            SaveFile($"Faktura {CisloFaktury}.pdf", stream.ToArray());
        }

        #region Helper Methods
        //Create and row for the grid.
        void AddProducts(string productId, string productName, double price, int quantity, double total, PdfGrid grid)
        {
            //Add a new row and set the product value to the grid row cells. 
            PdfGridRow row = grid.Rows.Add();
            row.Cells[0].Value = productId;
            row.Cells[1].Value = productName;
            row.Cells[2].Value = price.ToString();
            row.Cells[3].Value = quantity.ToString();
            row.Cells[4].Value = total.ToString();
        }

        private float GetTotalAmount(PdfGrid grid)
        {
            float Total = 0f;
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                string cellValue = grid.Rows[i].Cells[grid.Columns.Count - 1].Value.ToString();
                float result = float.Parse(cellValue, System.Globalization.CultureInfo.InvariantCulture);
                Total += result;
            }
            return Total;

        }
        #endregion

        public void Grid_BeginCellLayout(object sender, PdfGridBeginCellLayoutEventArgs args)
        {
            PdfGrid grid = sender as PdfGrid;
            if (args.CellIndex == grid!.Columns.Count - 1)
            {
                //Get the bounds of price cell in grid row. 
                TotalPriceCellBounds = args.Bounds;
            }
            else if (args.CellIndex == grid.Columns.Count - 2)
            {
                //Get the bounds of quantity cell in grid row. 
                QuantityCellBounds = args.Bounds;
            }

        }

        // Metoda pro uložení a otevření souboru
        private void SaveFile(string fileName, byte[] data)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            File.WriteAllBytes(filePath, data);
        }

        public async void OpenFile(string fileName, string contentType)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            try
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath, contentType)
                });
            }
            catch (System.IO.FileNotFoundException)
            {
                await Application.Current.MainPage.DisplayAlert("Chyba", "Soubor nelze najít. Byl buď přesunut nebo smazán.", "OK");
            }
        }

        public override string ToString() => $"Faktura: {CisloFaktury}";
    }
}
#endregion
