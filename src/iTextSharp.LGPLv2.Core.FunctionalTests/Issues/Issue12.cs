using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace iTextSharp.LGPLv2.Core.FunctionalTests.Issues
{
    /// <summary>
    /// https://github.com/VahidN/iTextSharp.LGPLv2.Core/issues/12
    /// </summary>
    [TestClass]
    public class Issue12
    {
        [TestMethod]
        public void Verify_Issue12_CanBe_Processed()
        {
            var document = new Document(PageSize.A4.Rotate());
            var table = new PdfPTable(7)
            {
                //actual width of table in points

                TotalWidth = 800f,

                //fix the absolute width of the table

                LockedWidth = true,
                HorizontalAlignment = Element.ALIGN_CENTER,
                SpacingBefore = 20f,
                SpacingAfter = 20f
            };

            document.SetMargins(5, 5, 5, 5);

            var tahoma = TestUtils.GetUnicodeFont("Tahoma", TestUtils.GetTahomaFontPath(), 10, Font.NORMAL, BaseColor.Black);

            var font12 = new Font(tahoma.BaseFont, 12, Font.NORMAL);
            var font14 = new Font(tahoma.BaseFont, 14, Font.NORMAL);
            var font16 = new Font(tahoma.BaseFont, 16, Font.BOLD);
            var filePath = TestUtils.GetOutputFileName();
            var fs = new FileStream(filePath, FileMode.Create);

            PdfWriter.GetInstance(document, fs);

            document.AddAuthor(TestUtils.Author);
            document.Open();

            var cell = new PdfPCell(new Phrase("ZG£OSZENIE/AKTUALIZACJA PRZEZ PODMIOT PROWADZ¥CY SERWIS KAS LUB PROWADZ¥CY SERWIS G£سWNY DANYCH DOTYCZ¥CYCH KASY"/*, font14*/))
            {
                Rowspan = 2,
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Nr dokumentu: 123/456/789"))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Data przyjêcia dokumentu:" + DateTime.Now.ToString("dd.MM.yyyy")))
            {
                Colspan = 2
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("MIEJSCE SK£ADANIA ZAWIADOMIENIA", font16))
            {
                Colspan = 7
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Naczelnik Urzêdu Skarbowego do ktَrego skierowane jest zawiadomienie: " + "UrZ_Nazwa", font12))
            {
                Colspan = 7
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Ulica: " + "UrZ_Ulica", font12))
            {
                Colspan = 5
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Nr domu: " + "UrZ_NrDomu", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Miejscowoœو: " + "UrZ_Miasto", font12))
            {
                Colspan = 5
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Kod pocztowy: " + "UrZ_KodPocztowy", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DANE IDENTYFIKACYJNE SPRZEDAWCY KAS", font16))
            {
                Colspan = 7
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nazwa (imiê i nazwisko): TRON Computers Sp. z o.o.", font12))
            {
                Colspan = 7
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Numer identyfikacyjny: 6332058814", font12))
            {
                Colspan = 3
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("REGON / PESEL: 89050315795", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Telefon 32 4731002", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Wojewَdztwo: Œl¹skie", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Gmina / Dzielnica: Jastrzêbie-Zdrَj", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Ulica: 11 Listopada", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Nr domu: 71", font12))
            {
                Colspan = 1
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nr lokalu: ", font12))
            {
                Colspan = 1
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Miejscowoœو: Jastrzêbie-Zdrَj", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Kod pocztowy: 44-335", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Poczta: Jastrzêbie-Zdrَj", font12))
            {
                Colspan = 2
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("DANE IDENTYFIKACYJNE PRODUCENTA (IMPORTERA) SPRZEDAWANYCH KAS", font16))
            {
                Colspan = 7
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nazwa (imiê i nazwisko) oraz adres", font12))
            {
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Typ / Model kasy", font12))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("ELZAB" + "\n" + "Jakaœ tam" + "\n" + "32-456" + " " + "Zabrze", font14))
            {
                Colspan = 5,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                MinimumHeight = 60
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("JOTA", font14))
            {
                Colspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("MIEJSCE INSTALACJI KAS REJESTRUJACYCH", font16))
            {
                Colspan = 7
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Dane identyfikacyjne podatnika:\nNazwa, dok³adny adres, NIP,\nMiejsce instalacji", font12))
            {
                Colspan = 2,
                Rowspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Typ / Model kasy", font12))
            {
                Rowspan = 2,
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Numery kasy / Data", font12))
            {
                Colspan = 3,
                Rowspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Uwagi Urzêdu Skarbowego", font12))
            {
                Colspan = 1,
                Rowspan = 2,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Unikatowy\nData fiskalizacji", font12))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Fabryczny", font12))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Ewidencyjny", font12))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Test Test", font12))
            {
                Colspan = 2,
                Rowspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("JOTA", font12))
            {
                Rowspan = 1,
                Colspan = 1,
                MinimumHeight = 120,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Test Test", font12))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Test Test", font12))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Test Test", font12))
            {
                Colspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Coœ tam napisze, ؟eby nie by³o :D", font12))
            {
                Colspan = 1,
                Rowspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Test Test", font12))
            {
                Colspan = 3,
                Rowspan = 1,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                MinimumHeight = 100
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Test Test", font12))
            {
                Rowspan = 1,
                Colspan = 4,
                MinimumHeight = 100,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_BOTTOM
            };
            table.AddCell(cell);

            document.Add(table);
            document.Close();
            fs.Dispose();

            TestUtils.VerifyPdfFileIsReadable(filePath);
        }
    }
}