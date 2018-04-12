using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Lpp.Utilities;
using System;
using System.Data;
using System.IO;
using System.Web;
using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
using Drawing = DocumentFormat.OpenXml.Drawing;
using Theme = DocumentFormat.OpenXml.Office2013.Theme;
using Path = System.IO.Path;

namespace Lpp.Dns.General
{
    public class ExcelHelper
    {
        //Row limits older Excel version per sheet
        const int rowLimit = 65000;

        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private static string replaceXmlChar(string input)
        {
            input = input.Replace("&", "&");
            input = input.Replace("<", "<");
            input = input.Replace(">", ">");
            input = input.Replace("\\\"", "\"");
            input = input.Replace("'", "&apos;");
            return input;
        }

        static void GenerateStyles(WorkbookStylesPart part)
        {
            Stylesheet stylesheet = new Stylesheet();

            Fonts fonts = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            Font baseFont = new Font();
            FontSize baseFontSize = new FontSize() { Val = 11D };
            Color baseFontColor = new Color() { Theme = (UInt32Value)1U };
            FontName baseFontName = new FontName() { Val = "Calibri" };
            FontFamilyNumbering baseFontFamilyNumber = new FontFamilyNumbering() { Val = 2 };
            DocumentFormat.OpenXml.Spreadsheet.FontScheme baseFontScheme = new DocumentFormat.OpenXml.Spreadsheet.FontScheme() { Val = FontSchemeValues.Minor };

            baseFont.Append(baseFontSize);
            baseFont.Append(baseFontColor);
            baseFont.Append(baseFontName);
            baseFont.Append(baseFontFamilyNumber);
            baseFont.Append(baseFontScheme);

            Font boldFont = new Font();
            Bold bold = new Bold();
            FontSize boldFontSize = new FontSize() { Val = 11D };
            Color boldColor = new Color() { Theme = (UInt32Value)1U };
            FontName boldFontName = new FontName() { Val = "Calibri" };
            FontFamilyNumbering boldFontFamily = new FontFamilyNumbering() { Val = 2 };
            DocumentFormat.OpenXml.Spreadsheet.FontScheme boldFontScheme = new DocumentFormat.OpenXml.Spreadsheet.FontScheme() { Val = FontSchemeValues.Minor };

            boldFont.Append(bold);
            boldFont.Append(boldFontSize);
            boldFont.Append(boldColor);
            boldFont.Append(boldFontName);
            boldFont.Append(boldFontFamily);
            boldFont.Append(boldFontScheme);

            fonts.Append(baseFont);
            fonts.Append(boldFont);

            Fills fills = new Fills() { Count = (UInt32Value)2U };

            DocumentFormat.OpenXml.Spreadsheet.Fill baseFill = new DocumentFormat.OpenXml.Spreadsheet.Fill();
            DocumentFormat.OpenXml.Spreadsheet.PatternFill basePatternFill = new DocumentFormat.OpenXml.Spreadsheet.PatternFill() { PatternType = PatternValues.None };

            baseFill.Append(basePatternFill);

            DocumentFormat.OpenXml.Spreadsheet.Fill fill2 = new DocumentFormat.OpenXml.Spreadsheet.Fill();
            DocumentFormat.OpenXml.Spreadsheet.PatternFill patternFill2 = new DocumentFormat.OpenXml.Spreadsheet.PatternFill() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            fills.Append(baseFill);
            fills.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)1U };

            DocumentFormat.OpenXml.Spreadsheet.Border border = new DocumentFormat.OpenXml.Spreadsheet.Border();
            DocumentFormat.OpenXml.Spreadsheet.LeftBorder leftBorder = new DocumentFormat.OpenXml.Spreadsheet.LeftBorder();
            DocumentFormat.OpenXml.Spreadsheet.RightBorder rightBorder = new DocumentFormat.OpenXml.Spreadsheet.RightBorder();
            DocumentFormat.OpenXml.Spreadsheet.TopBorder topBorder = new DocumentFormat.OpenXml.Spreadsheet.TopBorder();
            DocumentFormat.OpenXml.Spreadsheet.BottomBorder bottomBorder = new DocumentFormat.OpenXml.Spreadsheet.BottomBorder();
            DocumentFormat.OpenXml.Spreadsheet.DiagonalBorder diagonalBorder = new DocumentFormat.OpenXml.Spreadsheet.DiagonalBorder();

            border.Append(leftBorder);
            border.Append(rightBorder);
            border.Append(topBorder);
            border.Append(bottomBorder);
            border.Append(diagonalBorder);

            borders1.Append(border);

            CellStyleFormats cellStyleFormats = new CellStyleFormats() { Count = (UInt32Value)1U };
            CellFormat cellFormat = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats.Append(cellFormat);

            CellFormats cellFormats = new CellFormats() { Count = (UInt32Value)2U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U, ApplyFont = true };

            cellFormats.Append(cellFormat2);
            cellFormats.Append(cellFormat3);

            CellStyles cellStyles = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles.Append(cellStyle);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };


            stylesheet.Append(fonts);
            stylesheet.Append(fills);
            stylesheet.Append(borders1);
            stylesheet.Append(cellStyleFormats);
            stylesheet.Append(cellFormats);
            stylesheet.Append(cellStyles);
            stylesheet.Append(differentialFormats1);
            stylesheet.Append(tableStyles1);

            part.Stylesheet = stylesheet;


        }

        static void GenerateTheme(ThemePart part)
        {
            Drawing.Theme theme1 = new Drawing.Theme() { Name = "Office Theme" };
            theme1.AddNamespaceDeclaration("a", "http://schemas.openxmlformats.org/drawingml/2006/main");

            Drawing.ThemeElements themeElements1 = new Drawing.ThemeElements();

            Drawing.ColorScheme colorScheme1 = new Drawing.ColorScheme() { Name = "Office" };

            Drawing.Dark1Color dark1Color1 = new Drawing.Dark1Color();
            Drawing.SystemColor systemColor1 = new Drawing.SystemColor() { Val = Drawing.SystemColorValues.WindowText, LastColor = "000000" };

            dark1Color1.Append(systemColor1);

            Drawing.Light1Color light1Color1 = new Drawing.Light1Color();
            Drawing.SystemColor systemColor2 = new Drawing.SystemColor() { Val = Drawing.SystemColorValues.Window, LastColor = "FFFFFF" };

            light1Color1.Append(systemColor2);

            Drawing.Dark2Color dark2Color1 = new Drawing.Dark2Color();
            Drawing.RgbColorModelHex rgbColorModelHex1 = new Drawing.RgbColorModelHex() { Val = "44546A" };

            dark2Color1.Append(rgbColorModelHex1);

            Drawing.Light2Color light2Color1 = new Drawing.Light2Color();
            Drawing.RgbColorModelHex rgbColorModelHex2 = new Drawing.RgbColorModelHex() { Val = "E7E6E6" };

            light2Color1.Append(rgbColorModelHex2);

            Drawing.Accent1Color accent1Color1 = new Drawing.Accent1Color();
            Drawing.RgbColorModelHex rgbColorModelHex3 = new Drawing.RgbColorModelHex() { Val = "4472C4" };

            accent1Color1.Append(rgbColorModelHex3);

            Drawing.Accent2Color accent2Color1 = new Drawing.Accent2Color();
            Drawing.RgbColorModelHex rgbColorModelHex4 = new Drawing.RgbColorModelHex() { Val = "ED7D31" };

            accent2Color1.Append(rgbColorModelHex4);

            Drawing.Accent3Color accent3Color1 = new Drawing.Accent3Color();
            Drawing.RgbColorModelHex rgbColorModelHex5 = new Drawing.RgbColorModelHex() { Val = "A5A5A5" };

            accent3Color1.Append(rgbColorModelHex5);

            Drawing.Accent4Color accent4Color1 = new Drawing.Accent4Color();
            Drawing.RgbColorModelHex rgbColorModelHex6 = new Drawing.RgbColorModelHex() { Val = "FFC000" };

            accent4Color1.Append(rgbColorModelHex6);

            Drawing.Accent5Color accent5Color1 = new Drawing.Accent5Color();
            Drawing.RgbColorModelHex rgbColorModelHex7 = new Drawing.RgbColorModelHex() { Val = "5B9BD5" };

            accent5Color1.Append(rgbColorModelHex7);

            Drawing.Accent6Color accent6Color1 = new Drawing.Accent6Color();
            Drawing.RgbColorModelHex rgbColorModelHex8 = new Drawing.RgbColorModelHex() { Val = "70AD47" };

            accent6Color1.Append(rgbColorModelHex8);

            Drawing.Hyperlink hyperlink1 = new Drawing.Hyperlink();
            Drawing.RgbColorModelHex rgbColorModelHex9 = new Drawing.RgbColorModelHex() { Val = "0563C1" };

            hyperlink1.Append(rgbColorModelHex9);

            Drawing.FollowedHyperlinkColor followedHyperlinkColor1 = new Drawing.FollowedHyperlinkColor();
            Drawing.RgbColorModelHex rgbColorModelHex10 = new Drawing.RgbColorModelHex() { Val = "954F72" };

            followedHyperlinkColor1.Append(rgbColorModelHex10);

            colorScheme1.Append(dark1Color1);
            colorScheme1.Append(light1Color1);
            colorScheme1.Append(dark2Color1);
            colorScheme1.Append(light2Color1);
            colorScheme1.Append(accent1Color1);
            colorScheme1.Append(accent2Color1);
            colorScheme1.Append(accent3Color1);
            colorScheme1.Append(accent4Color1);
            colorScheme1.Append(accent5Color1);
            colorScheme1.Append(accent6Color1);
            colorScheme1.Append(hyperlink1);
            colorScheme1.Append(followedHyperlinkColor1);

            Drawing.FontScheme fontScheme1 = new Drawing.FontScheme() { Name = "Office" };

            Drawing.MajorFont majorFont1 = new Drawing.MajorFont();
            Drawing.LatinFont latinFont1 = new Drawing.LatinFont() { Typeface = "Calibri Light", Panose = "020F0302020204030204" };
            Drawing.EastAsianFont eastAsianFont1 = new Drawing.EastAsianFont() { Typeface = "" };
            Drawing.ComplexScriptFont complexScriptFont1 = new Drawing.ComplexScriptFont() { Typeface = "" };
            Drawing.SupplementalFont supplementalFont1 = new Drawing.SupplementalFont() { Script = "Jpan", Typeface = "游ゴシック Light" };
            Drawing.SupplementalFont supplementalFont2 = new Drawing.SupplementalFont() { Script = "Hang", Typeface = "맑은 고딕" };
            Drawing.SupplementalFont supplementalFont3 = new Drawing.SupplementalFont() { Script = "Hans", Typeface = "等线 Light" };
            Drawing.SupplementalFont supplementalFont4 = new Drawing.SupplementalFont() { Script = "Hant", Typeface = "新細明體" };
            Drawing.SupplementalFont supplementalFont5 = new Drawing.SupplementalFont() { Script = "Arab", Typeface = "Times New Roman" };
            Drawing.SupplementalFont supplementalFont6 = new Drawing.SupplementalFont() { Script = "Hebr", Typeface = "Times New Roman" };
            Drawing.SupplementalFont supplementalFont7 = new Drawing.SupplementalFont() { Script = "Thai", Typeface = "Tahoma" };
            Drawing.SupplementalFont supplementalFont8 = new Drawing.SupplementalFont() { Script = "Ethi", Typeface = "Nyala" };
            Drawing.SupplementalFont supplementalFont9 = new Drawing.SupplementalFont() { Script = "Beng", Typeface = "Vrinda" };
            Drawing.SupplementalFont supplementalFont10 = new Drawing.SupplementalFont() { Script = "Gujr", Typeface = "Shruti" };
            Drawing.SupplementalFont supplementalFont11 = new Drawing.SupplementalFont() { Script = "Khmr", Typeface = "MoolBoran" };
            Drawing.SupplementalFont supplementalFont12 = new Drawing.SupplementalFont() { Script = "Knda", Typeface = "Tunga" };
            Drawing.SupplementalFont supplementalFont13 = new Drawing.SupplementalFont() { Script = "Guru", Typeface = "Raavi" };
            Drawing.SupplementalFont supplementalFont14 = new Drawing.SupplementalFont() { Script = "Cans", Typeface = "Euphemia" };
            Drawing.SupplementalFont supplementalFont15 = new Drawing.SupplementalFont() { Script = "Cher", Typeface = "Plantagenet Cherokee" };
            Drawing.SupplementalFont supplementalFont16 = new Drawing.SupplementalFont() { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
            Drawing.SupplementalFont supplementalFont17 = new Drawing.SupplementalFont() { Script = "Tibt", Typeface = "Microsoft Himalaya" };
            Drawing.SupplementalFont supplementalFont18 = new Drawing.SupplementalFont() { Script = "Thaa", Typeface = "MV Boli" };
            Drawing.SupplementalFont supplementalFont19 = new Drawing.SupplementalFont() { Script = "Deva", Typeface = "Mangal" };
            Drawing.SupplementalFont supplementalFont20 = new Drawing.SupplementalFont() { Script = "Telu", Typeface = "Gautami" };
            Drawing.SupplementalFont supplementalFont21 = new Drawing.SupplementalFont() { Script = "Taml", Typeface = "Latha" };
            Drawing.SupplementalFont supplementalFont22 = new Drawing.SupplementalFont() { Script = "Syrc", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont23 = new Drawing.SupplementalFont() { Script = "Orya", Typeface = "Kalinga" };
            Drawing.SupplementalFont supplementalFont24 = new Drawing.SupplementalFont() { Script = "Mlym", Typeface = "Kartika" };
            Drawing.SupplementalFont supplementalFont25 = new Drawing.SupplementalFont() { Script = "Laoo", Typeface = "DokChampa" };
            Drawing.SupplementalFont supplementalFont26 = new Drawing.SupplementalFont() { Script = "Sinh", Typeface = "Iskoola Pota" };
            Drawing.SupplementalFont supplementalFont27 = new Drawing.SupplementalFont() { Script = "Mong", Typeface = "Mongolian Baiti" };
            Drawing.SupplementalFont supplementalFont28 = new Drawing.SupplementalFont() { Script = "Viet", Typeface = "Times New Roman" };
            Drawing.SupplementalFont supplementalFont29 = new Drawing.SupplementalFont() { Script = "Uigh", Typeface = "Microsoft Uighur" };
            Drawing.SupplementalFont supplementalFont30 = new Drawing.SupplementalFont() { Script = "Geor", Typeface = "Sylfaen" };
            Drawing.SupplementalFont supplementalFont31 = new Drawing.SupplementalFont() { Script = "Armn", Typeface = "Arial" };
            Drawing.SupplementalFont supplementalFont32 = new Drawing.SupplementalFont() { Script = "Bugi", Typeface = "Leelawadee UI" };
            Drawing.SupplementalFont supplementalFont33 = new Drawing.SupplementalFont() { Script = "Bopo", Typeface = "Microsoft JhengHei" };
            Drawing.SupplementalFont supplementalFont34 = new Drawing.SupplementalFont() { Script = "Java", Typeface = "Javanese Text" };
            Drawing.SupplementalFont supplementalFont35 = new Drawing.SupplementalFont() { Script = "Lisu", Typeface = "Segoe UI" };
            Drawing.SupplementalFont supplementalFont36 = new Drawing.SupplementalFont() { Script = "Mymr", Typeface = "Myanmar Text" };
            Drawing.SupplementalFont supplementalFont37 = new Drawing.SupplementalFont() { Script = "Nkoo", Typeface = "Ebrima" };
            Drawing.SupplementalFont supplementalFont38 = new Drawing.SupplementalFont() { Script = "Olck", Typeface = "Nirmala UI" };
            Drawing.SupplementalFont supplementalFont39 = new Drawing.SupplementalFont() { Script = "Osma", Typeface = "Ebrima" };
            Drawing.SupplementalFont supplementalFont40 = new Drawing.SupplementalFont() { Script = "Phag", Typeface = "Phagspa" };
            Drawing.SupplementalFont supplementalFont41 = new Drawing.SupplementalFont() { Script = "Syrn", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont42 = new Drawing.SupplementalFont() { Script = "Syrj", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont43 = new Drawing.SupplementalFont() { Script = "Syre", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont44 = new Drawing.SupplementalFont() { Script = "Sora", Typeface = "Nirmala UI" };
            Drawing.SupplementalFont supplementalFont45 = new Drawing.SupplementalFont() { Script = "Tale", Typeface = "Microsoft Tai Le" };
            Drawing.SupplementalFont supplementalFont46 = new Drawing.SupplementalFont() { Script = "Talu", Typeface = "Microsoft New Tai Lue" };
            Drawing.SupplementalFont supplementalFont47 = new Drawing.SupplementalFont() { Script = "Tfng", Typeface = "Ebrima" };

            majorFont1.Append(latinFont1);
            majorFont1.Append(eastAsianFont1);
            majorFont1.Append(complexScriptFont1);
            majorFont1.Append(supplementalFont1);
            majorFont1.Append(supplementalFont2);
            majorFont1.Append(supplementalFont3);
            majorFont1.Append(supplementalFont4);
            majorFont1.Append(supplementalFont5);
            majorFont1.Append(supplementalFont6);
            majorFont1.Append(supplementalFont7);
            majorFont1.Append(supplementalFont8);
            majorFont1.Append(supplementalFont9);
            majorFont1.Append(supplementalFont10);
            majorFont1.Append(supplementalFont11);
            majorFont1.Append(supplementalFont12);
            majorFont1.Append(supplementalFont13);
            majorFont1.Append(supplementalFont14);
            majorFont1.Append(supplementalFont15);
            majorFont1.Append(supplementalFont16);
            majorFont1.Append(supplementalFont17);
            majorFont1.Append(supplementalFont18);
            majorFont1.Append(supplementalFont19);
            majorFont1.Append(supplementalFont20);
            majorFont1.Append(supplementalFont21);
            majorFont1.Append(supplementalFont22);
            majorFont1.Append(supplementalFont23);
            majorFont1.Append(supplementalFont24);
            majorFont1.Append(supplementalFont25);
            majorFont1.Append(supplementalFont26);
            majorFont1.Append(supplementalFont27);
            majorFont1.Append(supplementalFont28);
            majorFont1.Append(supplementalFont29);
            majorFont1.Append(supplementalFont30);
            majorFont1.Append(supplementalFont31);
            majorFont1.Append(supplementalFont32);
            majorFont1.Append(supplementalFont33);
            majorFont1.Append(supplementalFont34);
            majorFont1.Append(supplementalFont35);
            majorFont1.Append(supplementalFont36);
            majorFont1.Append(supplementalFont37);
            majorFont1.Append(supplementalFont38);
            majorFont1.Append(supplementalFont39);
            majorFont1.Append(supplementalFont40);
            majorFont1.Append(supplementalFont41);
            majorFont1.Append(supplementalFont42);
            majorFont1.Append(supplementalFont43);
            majorFont1.Append(supplementalFont44);
            majorFont1.Append(supplementalFont45);
            majorFont1.Append(supplementalFont46);
            majorFont1.Append(supplementalFont47);

            Drawing.MinorFont minorFont1 = new Drawing.MinorFont();
            Drawing.LatinFont latinFont2 = new Drawing.LatinFont() { Typeface = "Calibri", Panose = "020F0502020204030204" };
            Drawing.EastAsianFont eastAsianFont2 = new Drawing.EastAsianFont() { Typeface = "" };
            Drawing.ComplexScriptFont complexScriptFont2 = new Drawing.ComplexScriptFont() { Typeface = "" };
            Drawing.SupplementalFont supplementalFont48 = new Drawing.SupplementalFont() { Script = "Jpan", Typeface = "游ゴシック" };
            Drawing.SupplementalFont supplementalFont49 = new Drawing.SupplementalFont() { Script = "Hang", Typeface = "맑은 고딕" };
            Drawing.SupplementalFont supplementalFont50 = new Drawing.SupplementalFont() { Script = "Hans", Typeface = "等线" };
            Drawing.SupplementalFont supplementalFont51 = new Drawing.SupplementalFont() { Script = "Hant", Typeface = "新細明體" };
            Drawing.SupplementalFont supplementalFont52 = new Drawing.SupplementalFont() { Script = "Arab", Typeface = "Arial" };
            Drawing.SupplementalFont supplementalFont53 = new Drawing.SupplementalFont() { Script = "Hebr", Typeface = "Arial" };
            Drawing.SupplementalFont supplementalFont54 = new Drawing.SupplementalFont() { Script = "Thai", Typeface = "Tahoma" };
            Drawing.SupplementalFont supplementalFont55 = new Drawing.SupplementalFont() { Script = "Ethi", Typeface = "Nyala" };
            Drawing.SupplementalFont supplementalFont56 = new Drawing.SupplementalFont() { Script = "Beng", Typeface = "Vrinda" };
            Drawing.SupplementalFont supplementalFont57 = new Drawing.SupplementalFont() { Script = "Gujr", Typeface = "Shruti" };
            Drawing.SupplementalFont supplementalFont58 = new Drawing.SupplementalFont() { Script = "Khmr", Typeface = "DaunPenh" };
            Drawing.SupplementalFont supplementalFont59 = new Drawing.SupplementalFont() { Script = "Knda", Typeface = "Tunga" };
            Drawing.SupplementalFont supplementalFont60 = new Drawing.SupplementalFont() { Script = "Guru", Typeface = "Raavi" };
            Drawing.SupplementalFont supplementalFont61 = new Drawing.SupplementalFont() { Script = "Cans", Typeface = "Euphemia" };
            Drawing.SupplementalFont supplementalFont62 = new Drawing.SupplementalFont() { Script = "Cher", Typeface = "Plantagenet Cherokee" };
            Drawing.SupplementalFont supplementalFont63 = new Drawing.SupplementalFont() { Script = "Yiii", Typeface = "Microsoft Yi Baiti" };
            Drawing.SupplementalFont supplementalFont64 = new Drawing.SupplementalFont() { Script = "Tibt", Typeface = "Microsoft Himalaya" };
            Drawing.SupplementalFont supplementalFont65 = new Drawing.SupplementalFont() { Script = "Thaa", Typeface = "MV Boli" };
            Drawing.SupplementalFont supplementalFont66 = new Drawing.SupplementalFont() { Script = "Deva", Typeface = "Mangal" };
            Drawing.SupplementalFont supplementalFont67 = new Drawing.SupplementalFont() { Script = "Telu", Typeface = "Gautami" };
            Drawing.SupplementalFont supplementalFont68 = new Drawing.SupplementalFont() { Script = "Taml", Typeface = "Latha" };
            Drawing.SupplementalFont supplementalFont69 = new Drawing.SupplementalFont() { Script = "Syrc", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont70 = new Drawing.SupplementalFont() { Script = "Orya", Typeface = "Kalinga" };
            Drawing.SupplementalFont supplementalFont71 = new Drawing.SupplementalFont() { Script = "Mlym", Typeface = "Kartika" };
            Drawing.SupplementalFont supplementalFont72 = new Drawing.SupplementalFont() { Script = "Laoo", Typeface = "DokChampa" };
            Drawing.SupplementalFont supplementalFont73 = new Drawing.SupplementalFont() { Script = "Sinh", Typeface = "Iskoola Pota" };
            Drawing.SupplementalFont supplementalFont74 = new Drawing.SupplementalFont() { Script = "Mong", Typeface = "Mongolian Baiti" };
            Drawing.SupplementalFont supplementalFont75 = new Drawing.SupplementalFont() { Script = "Viet", Typeface = "Arial" };
            Drawing.SupplementalFont supplementalFont76 = new Drawing.SupplementalFont() { Script = "Uigh", Typeface = "Microsoft Uighur" };
            Drawing.SupplementalFont supplementalFont77 = new Drawing.SupplementalFont() { Script = "Geor", Typeface = "Sylfaen" };
            Drawing.SupplementalFont supplementalFont78 = new Drawing.SupplementalFont() { Script = "Armn", Typeface = "Arial" };
            Drawing.SupplementalFont supplementalFont79 = new Drawing.SupplementalFont() { Script = "Bugi", Typeface = "Leelawadee UI" };
            Drawing.SupplementalFont supplementalFont80 = new Drawing.SupplementalFont() { Script = "Bopo", Typeface = "Microsoft JhengHei" };
            Drawing.SupplementalFont supplementalFont81 = new Drawing.SupplementalFont() { Script = "Java", Typeface = "Javanese Text" };
            Drawing.SupplementalFont supplementalFont82 = new Drawing.SupplementalFont() { Script = "Lisu", Typeface = "Segoe UI" };
            Drawing.SupplementalFont supplementalFont83 = new Drawing.SupplementalFont() { Script = "Mymr", Typeface = "Myanmar Text" };
            Drawing.SupplementalFont supplementalFont84 = new Drawing.SupplementalFont() { Script = "Nkoo", Typeface = "Ebrima" };
            Drawing.SupplementalFont supplementalFont85 = new Drawing.SupplementalFont() { Script = "Olck", Typeface = "Nirmala UI" };
            Drawing.SupplementalFont supplementalFont86 = new Drawing.SupplementalFont() { Script = "Osma", Typeface = "Ebrima" };
            Drawing.SupplementalFont supplementalFont87 = new Drawing.SupplementalFont() { Script = "Phag", Typeface = "Phagspa" };
            Drawing.SupplementalFont supplementalFont88 = new Drawing.SupplementalFont() { Script = "Syrn", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont89 = new Drawing.SupplementalFont() { Script = "Syrj", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont90 = new Drawing.SupplementalFont() { Script = "Syre", Typeface = "Estrangelo Edessa" };
            Drawing.SupplementalFont supplementalFont91 = new Drawing.SupplementalFont() { Script = "Sora", Typeface = "Nirmala UI" };
            Drawing.SupplementalFont supplementalFont92 = new Drawing.SupplementalFont() { Script = "Tale", Typeface = "Microsoft Tai Le" };
            Drawing.SupplementalFont supplementalFont93 = new Drawing.SupplementalFont() { Script = "Talu", Typeface = "Microsoft New Tai Lue" };
            Drawing.SupplementalFont supplementalFont94 = new Drawing.SupplementalFont() { Script = "Tfng", Typeface = "Ebrima" };

            minorFont1.Append(latinFont2);
            minorFont1.Append(eastAsianFont2);
            minorFont1.Append(complexScriptFont2);
            minorFont1.Append(supplementalFont48);
            minorFont1.Append(supplementalFont49);
            minorFont1.Append(supplementalFont50);
            minorFont1.Append(supplementalFont51);
            minorFont1.Append(supplementalFont52);
            minorFont1.Append(supplementalFont53);
            minorFont1.Append(supplementalFont54);
            minorFont1.Append(supplementalFont55);
            minorFont1.Append(supplementalFont56);
            minorFont1.Append(supplementalFont57);
            minorFont1.Append(supplementalFont58);
            minorFont1.Append(supplementalFont59);
            minorFont1.Append(supplementalFont60);
            minorFont1.Append(supplementalFont61);
            minorFont1.Append(supplementalFont62);
            minorFont1.Append(supplementalFont63);
            minorFont1.Append(supplementalFont64);
            minorFont1.Append(supplementalFont65);
            minorFont1.Append(supplementalFont66);
            minorFont1.Append(supplementalFont67);
            minorFont1.Append(supplementalFont68);
            minorFont1.Append(supplementalFont69);
            minorFont1.Append(supplementalFont70);
            minorFont1.Append(supplementalFont71);
            minorFont1.Append(supplementalFont72);
            minorFont1.Append(supplementalFont73);
            minorFont1.Append(supplementalFont74);
            minorFont1.Append(supplementalFont75);
            minorFont1.Append(supplementalFont76);
            minorFont1.Append(supplementalFont77);
            minorFont1.Append(supplementalFont78);
            minorFont1.Append(supplementalFont79);
            minorFont1.Append(supplementalFont80);
            minorFont1.Append(supplementalFont81);
            minorFont1.Append(supplementalFont82);
            minorFont1.Append(supplementalFont83);
            minorFont1.Append(supplementalFont84);
            minorFont1.Append(supplementalFont85);
            minorFont1.Append(supplementalFont86);
            minorFont1.Append(supplementalFont87);
            minorFont1.Append(supplementalFont88);
            minorFont1.Append(supplementalFont89);
            minorFont1.Append(supplementalFont90);
            minorFont1.Append(supplementalFont91);
            minorFont1.Append(supplementalFont92);
            minorFont1.Append(supplementalFont93);
            minorFont1.Append(supplementalFont94);

            fontScheme1.Append(majorFont1);
            fontScheme1.Append(minorFont1);

            Drawing.FormatScheme formatScheme1 = new Drawing.FormatScheme() { Name = "Office" };

            Drawing.FillStyleList fillStyleList1 = new Drawing.FillStyleList();

            Drawing.SolidFill solidFill1 = new Drawing.SolidFill();
            Drawing.SchemeColor schemeColor1 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };

            solidFill1.Append(schemeColor1);

            Drawing.GradientFill gradientFill1 = new Drawing.GradientFill() { RotateWithShape = true };

            Drawing.GradientStopList gradientStopList1 = new Drawing.GradientStopList();

            Drawing.GradientStop gradientStop1 = new Drawing.GradientStop() { Position = 0 };

            Drawing.SchemeColor schemeColor2 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.LuminanceModulation luminanceModulation1 = new Drawing.LuminanceModulation() { Val = 110000 };
            Drawing.SaturationModulation saturationModulation1 = new Drawing.SaturationModulation() { Val = 105000 };
            Drawing.Tint tint1 = new Drawing.Tint() { Val = 67000 };

            schemeColor2.Append(luminanceModulation1);
            schemeColor2.Append(saturationModulation1);
            schemeColor2.Append(tint1);

            gradientStop1.Append(schemeColor2);

            Drawing.GradientStop gradientStop2 = new Drawing.GradientStop() { Position = 50000 };

            Drawing.SchemeColor schemeColor3 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.LuminanceModulation luminanceModulation2 = new Drawing.LuminanceModulation() { Val = 105000 };
            Drawing.SaturationModulation saturationModulation2 = new Drawing.SaturationModulation() { Val = 103000 };
            Drawing.Tint tint2 = new Drawing.Tint() { Val = 73000 };

            schemeColor3.Append(luminanceModulation2);
            schemeColor3.Append(saturationModulation2);
            schemeColor3.Append(tint2);

            gradientStop2.Append(schemeColor3);

            Drawing.GradientStop gradientStop3 = new Drawing.GradientStop() { Position = 100000 };

            Drawing.SchemeColor schemeColor4 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.LuminanceModulation luminanceModulation3 = new Drawing.LuminanceModulation() { Val = 105000 };
            Drawing.SaturationModulation saturationModulation3 = new Drawing.SaturationModulation() { Val = 109000 };
            Drawing.Tint tint3 = new Drawing.Tint() { Val = 81000 };

            schemeColor4.Append(luminanceModulation3);
            schemeColor4.Append(saturationModulation3);
            schemeColor4.Append(tint3);

            gradientStop3.Append(schemeColor4);

            gradientStopList1.Append(gradientStop1);
            gradientStopList1.Append(gradientStop2);
            gradientStopList1.Append(gradientStop3);
            Drawing.LinearGradientFill linearGradientFill1 = new Drawing.LinearGradientFill() { Angle = 5400000, Scaled = false };

            gradientFill1.Append(gradientStopList1);
            gradientFill1.Append(linearGradientFill1);

            Drawing.GradientFill gradientFill2 = new Drawing.GradientFill() { RotateWithShape = true };

            Drawing.GradientStopList gradientStopList2 = new Drawing.GradientStopList();

            Drawing.GradientStop gradientStop4 = new Drawing.GradientStop() { Position = 0 };

            Drawing.SchemeColor schemeColor5 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.SaturationModulation saturationModulation4 = new Drawing.SaturationModulation() { Val = 103000 };
            Drawing.LuminanceModulation luminanceModulation4 = new Drawing.LuminanceModulation() { Val = 102000 };
            Drawing.Tint tint4 = new Drawing.Tint() { Val = 94000 };

            schemeColor5.Append(saturationModulation4);
            schemeColor5.Append(luminanceModulation4);
            schemeColor5.Append(tint4);

            gradientStop4.Append(schemeColor5);

            Drawing.GradientStop gradientStop5 = new Drawing.GradientStop() { Position = 50000 };

            Drawing.SchemeColor schemeColor6 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.SaturationModulation saturationModulation5 = new Drawing.SaturationModulation() { Val = 110000 };
            Drawing.LuminanceModulation luminanceModulation5 = new Drawing.LuminanceModulation() { Val = 100000 };
            Drawing.Shade shade1 = new Drawing.Shade() { Val = 100000 };

            schemeColor6.Append(saturationModulation5);
            schemeColor6.Append(luminanceModulation5);
            schemeColor6.Append(shade1);

            gradientStop5.Append(schemeColor6);

            Drawing.GradientStop gradientStop6 = new Drawing.GradientStop() { Position = 100000 };

            Drawing.SchemeColor schemeColor7 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.LuminanceModulation luminanceModulation6 = new Drawing.LuminanceModulation() { Val = 99000 };
            Drawing.SaturationModulation saturationModulation6 = new Drawing.SaturationModulation() { Val = 120000 };
            Drawing.Shade shade2 = new Drawing.Shade() { Val = 78000 };

            schemeColor7.Append(luminanceModulation6);
            schemeColor7.Append(saturationModulation6);
            schemeColor7.Append(shade2);

            gradientStop6.Append(schemeColor7);

            gradientStopList2.Append(gradientStop4);
            gradientStopList2.Append(gradientStop5);
            gradientStopList2.Append(gradientStop6);
            Drawing.LinearGradientFill linearGradientFill2 = new Drawing.LinearGradientFill() { Angle = 5400000, Scaled = false };

            gradientFill2.Append(gradientStopList2);
            gradientFill2.Append(linearGradientFill2);

            fillStyleList1.Append(solidFill1);
            fillStyleList1.Append(gradientFill1);
            fillStyleList1.Append(gradientFill2);

            Drawing.LineStyleList lineStyleList1 = new Drawing.LineStyleList();

            Drawing.Outline outline1 = new Drawing.Outline() { Width = 6350, CapType = Drawing.LineCapValues.Flat, CompoundLineType = Drawing.CompoundLineValues.Single, Alignment = Drawing.PenAlignmentValues.Center };

            Drawing.SolidFill solidFill2 = new Drawing.SolidFill();
            Drawing.SchemeColor schemeColor8 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };

            solidFill2.Append(schemeColor8);
            Drawing.PresetDash presetDash1 = new Drawing.PresetDash() { Val = Drawing.PresetLineDashValues.Solid };
            Drawing.Miter miter1 = new Drawing.Miter() { Limit = 800000 };

            outline1.Append(solidFill2);
            outline1.Append(presetDash1);
            outline1.Append(miter1);

            Drawing.Outline outline2 = new Drawing.Outline() { Width = 12700, CapType = Drawing.LineCapValues.Flat, CompoundLineType = Drawing.CompoundLineValues.Single, Alignment = Drawing.PenAlignmentValues.Center };

            Drawing.SolidFill solidFill3 = new Drawing.SolidFill();
            Drawing.SchemeColor schemeColor9 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };

            solidFill3.Append(schemeColor9);
            Drawing.PresetDash presetDash2 = new Drawing.PresetDash() { Val = Drawing.PresetLineDashValues.Solid };
            Drawing.Miter miter2 = new Drawing.Miter() { Limit = 800000 };

            outline2.Append(solidFill3);
            outline2.Append(presetDash2);
            outline2.Append(miter2);

            Drawing.Outline outline3 = new Drawing.Outline() { Width = 19050, CapType = Drawing.LineCapValues.Flat, CompoundLineType = Drawing.CompoundLineValues.Single, Alignment = Drawing.PenAlignmentValues.Center };

            Drawing.SolidFill solidFill4 = new Drawing.SolidFill();
            Drawing.SchemeColor schemeColor10 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };

            solidFill4.Append(schemeColor10);
            Drawing.PresetDash presetDash3 = new Drawing.PresetDash() { Val = Drawing.PresetLineDashValues.Solid };
            Drawing.Miter miter3 = new Drawing.Miter() { Limit = 800000 };

            outline3.Append(solidFill4);
            outline3.Append(presetDash3);
            outline3.Append(miter3);

            lineStyleList1.Append(outline1);
            lineStyleList1.Append(outline2);
            lineStyleList1.Append(outline3);

            Drawing.EffectStyleList effectStyleList1 = new Drawing.EffectStyleList();

            Drawing.EffectStyle effectStyle1 = new Drawing.EffectStyle();
            Drawing.EffectList effectList1 = new Drawing.EffectList();

            effectStyle1.Append(effectList1);

            Drawing.EffectStyle effectStyle2 = new Drawing.EffectStyle();
            Drawing.EffectList effectList2 = new Drawing.EffectList();

            effectStyle2.Append(effectList2);

            Drawing.EffectStyle effectStyle3 = new Drawing.EffectStyle();

            Drawing.EffectList effectList3 = new Drawing.EffectList();

            Drawing.OuterShadow outerShadow1 = new Drawing.OuterShadow() { BlurRadius = 57150L, Distance = 19050L, Direction = 5400000, Alignment = Drawing.RectangleAlignmentValues.Center, RotateWithShape = false };

            Drawing.RgbColorModelHex rgbColorModelHex11 = new Drawing.RgbColorModelHex() { Val = "000000" };
            Drawing.Alpha alpha1 = new Drawing.Alpha() { Val = 63000 };

            rgbColorModelHex11.Append(alpha1);

            outerShadow1.Append(rgbColorModelHex11);

            effectList3.Append(outerShadow1);

            effectStyle3.Append(effectList3);

            effectStyleList1.Append(effectStyle1);
            effectStyleList1.Append(effectStyle2);
            effectStyleList1.Append(effectStyle3);

            Drawing.BackgroundFillStyleList backgroundFillStyleList1 = new Drawing.BackgroundFillStyleList();

            Drawing.SolidFill solidFill5 = new Drawing.SolidFill();
            Drawing.SchemeColor schemeColor11 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };

            solidFill5.Append(schemeColor11);

            Drawing.SolidFill solidFill6 = new Drawing.SolidFill();

            Drawing.SchemeColor schemeColor12 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.Tint tint5 = new Drawing.Tint() { Val = 95000 };
            Drawing.SaturationModulation saturationModulation7 = new Drawing.SaturationModulation() { Val = 170000 };

            schemeColor12.Append(tint5);
            schemeColor12.Append(saturationModulation7);

            solidFill6.Append(schemeColor12);

            Drawing.GradientFill gradientFill3 = new Drawing.GradientFill() { RotateWithShape = true };

            Drawing.GradientStopList gradientStopList3 = new Drawing.GradientStopList();

            Drawing.GradientStop gradientStop7 = new Drawing.GradientStop() { Position = 0 };

            Drawing.SchemeColor schemeColor13 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.Tint tint6 = new Drawing.Tint() { Val = 93000 };
            Drawing.SaturationModulation saturationModulation8 = new Drawing.SaturationModulation() { Val = 150000 };
            Drawing.Shade shade3 = new Drawing.Shade() { Val = 98000 };
            Drawing.LuminanceModulation luminanceModulation7 = new Drawing.LuminanceModulation() { Val = 102000 };

            schemeColor13.Append(tint6);
            schemeColor13.Append(saturationModulation8);
            schemeColor13.Append(shade3);
            schemeColor13.Append(luminanceModulation7);

            gradientStop7.Append(schemeColor13);

            Drawing.GradientStop gradientStop8 = new Drawing.GradientStop() { Position = 50000 };

            Drawing.SchemeColor schemeColor14 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.Tint tint7 = new Drawing.Tint() { Val = 98000 };
            Drawing.SaturationModulation saturationModulation9 = new Drawing.SaturationModulation() { Val = 130000 };
            Drawing.Shade shade4 = new Drawing.Shade() { Val = 90000 };
            Drawing.LuminanceModulation luminanceModulation8 = new Drawing.LuminanceModulation() { Val = 103000 };

            schemeColor14.Append(tint7);
            schemeColor14.Append(saturationModulation9);
            schemeColor14.Append(shade4);
            schemeColor14.Append(luminanceModulation8);

            gradientStop8.Append(schemeColor14);

            Drawing.GradientStop gradientStop9 = new Drawing.GradientStop() { Position = 100000 };

            Drawing.SchemeColor schemeColor15 = new Drawing.SchemeColor() { Val = Drawing.SchemeColorValues.PhColor };
            Drawing.Shade shade5 = new Drawing.Shade() { Val = 63000 };
            Drawing.SaturationModulation saturationModulation10 = new Drawing.SaturationModulation() { Val = 120000 };

            schemeColor15.Append(shade5);
            schemeColor15.Append(saturationModulation10);

            gradientStop9.Append(schemeColor15);

            gradientStopList3.Append(gradientStop7);
            gradientStopList3.Append(gradientStop8);
            gradientStopList3.Append(gradientStop9);
            Drawing.LinearGradientFill linearGradientFill3 = new Drawing.LinearGradientFill() { Angle = 5400000, Scaled = false };

            gradientFill3.Append(gradientStopList3);
            gradientFill3.Append(linearGradientFill3);

            backgroundFillStyleList1.Append(solidFill5);
            backgroundFillStyleList1.Append(solidFill6);
            backgroundFillStyleList1.Append(gradientFill3);

            formatScheme1.Append(fillStyleList1);
            formatScheme1.Append(lineStyleList1);
            formatScheme1.Append(effectStyleList1);
            formatScheme1.Append(backgroundFillStyleList1);

            themeElements1.Append(colorScheme1);
            themeElements1.Append(fontScheme1);
            themeElements1.Append(formatScheme1);
            Drawing.ObjectDefaults objectDefaults1 = new Drawing.ObjectDefaults();
            Drawing.ExtraColorSchemeList extraColorSchemeList1 = new Drawing.ExtraColorSchemeList();

            Drawing.OfficeStyleSheetExtensionList officeStyleSheetExtensionList1 = new Drawing.OfficeStyleSheetExtensionList();

            Drawing.OfficeStyleSheetExtension officeStyleSheetExtension1 = new Drawing.OfficeStyleSheetExtension() { Uri = "{05A4C25C-085E-4340-85A3-A5531E510DB2}" };

            Theme.ThemeFamily themeFamily1 = new Theme.ThemeFamily() { Name = "Office Theme", Id = "{62F939B6-93AF-4DB8-9C6B-D6C7DFDC589F}", Vid = "{4A3C46E8-61CC-4603-A589-7422A47A8E4A}" };
            themeFamily1.AddNamespaceDeclaration("thm15", "http://schemas.microsoft.com/office/thememl/2012/main");

            officeStyleSheetExtension1.Append(themeFamily1);

            officeStyleSheetExtensionList1.Append(officeStyleSheetExtension1);

            theme1.Append(themeElements1);
            theme1.Append(objectDefaults1);
            theme1.Append(extraColorSchemeList1);
            theme1.Append(officeStyleSheetExtensionList1);

            part.Theme = theme1;


        }

        static Cell CreateCell(string value, CellValues dataType, uint style = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = style
            };
        }

        public static void ToExcel(DataSet dsInput, string filename, HttpResponse response, int rowStart = 0)
        {
            MemoryStream stream = new MemoryStream();
            using (SpreadsheetDocument s = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {

                // Each response will be on a separate worksheet
                WorkbookPart workbookPart = s.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                #region styles
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                GenerateStyles(stylePart);

                ThemePart themePart = workbookPart.AddNewPart<ThemePart>();
                GenerateTheme(themePart);


                #endregion

                workbookPart.Workbook = new Workbook();
                s.WorkbookPart.Workbook.Sheets = new Sheets();
                Sheets sheets = s.WorkbookPart.Workbook.GetFirstChild<Sheets>();


                for (uint i = 0; i < dsInput.Tables.Count; i++)
                {
                    var table = dsInput.Tables[Convert.ToInt32(i)];

                    var responseSourceName = (string.IsNullOrWhiteSpace(table.TableName) ? "Sheet " + i : table.TableName).Trim().MaxLength(30);
                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

                    Sheet sheet = new Sheet() { Id = s.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = (i + 1), Name = responseSourceName };
                    sheets.Append(sheet);

                    SheetData sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    for (int r = 0; r < table.Rows.Count; r++)
                    {
                        var row = table.Rows[r];

                        if ((r % rowLimit) == 0)
                        {
                            Row headerRow = new Row();

                            //If rowStart > 0, then rows = 0 to rowStart-1 should be treated as group headers and shown before column headers.
                            if (rowStart > 0 && table.Rows.Count > rowStart)
                            {
                                for (int hdr = 0; hdr < rowStart; hdr++)
                                {
                                    foreach (DataColumn dc in table.Columns)
                                        headerRow.Append(CreateCell((table.Rows[hdr][dc.ColumnName] == null) ? "" : table.Rows[hdr][dc.ColumnName].ToString(), CellValues.String, 1U));

                                }
                            }
                            //write column name row
                            foreach (DataColumn dc in table.Columns)
                                headerRow.Append(CreateCell(dc.ColumnName, CellValues.String, 1U));

                            sheetData.AppendChild(headerRow);
                        }

                        if (r >= rowStart)
                        {
                            Row dataRow = new Row();
                            foreach (DataColumn col in table.Columns)
                                dataRow.Append(CreateCell(replaceXmlChar(row[col.ColumnName].ToString()), CellValues.String));

                            sheetData.AppendChild(dataRow);

                        }
                    }

                }

                workbookPart.Workbook.Save();
                s.Close();
            }
            response.Clear();
            response.AppendHeader("Content-Type", "application/vnd.ms-excel");
            response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
            stream.WriteTo(response.OutputStream);
            response.Flush();
            response.End();
        }

        /// <summary>
        /// TODO: Text of the form "10-14" are being auto-interpreted as dates by Excel. This can be
        /// "fixed" by coding it as "=""10-14""", but may not be supported by other applications,
        /// so I've decided not to "fix" it at this time.
        /// </summary>
        /// <param name="dsInput"></param>
        /// <param name="filename"></param>
        /// <param name="sw"></param>
        public static void ToCSV(DataSet dsInput, StringWriter sw)
        {
            bool isHeaderAdded = false;

            foreach (DataTable dt in dsInput.Tables)
            {
                // Prepend a space to force each cell to be a string.

                DataRowCollection rows = dt.Rows;
                for (int idx = 0; idx < rows.Count; idx++)
                {
                    DataRow r = rows[idx];
                    for (int cIdx = 0; cIdx < dt.Columns.Count; cIdx++)
                    {
                        if (cIdx == 2)
                            r[cIdx] = " " + Convert.ToString(r[cIdx]);
                    }
                }

                // Write the column headers.
                int iColCount = dt.Columns.Count;
                if (!isHeaderAdded)
                {
                    for (int i = 0; i < iColCount; i++)
                    {
                        sw.Write("\"" + dt.Columns[i].ToString().Replace("\"", "\"\"") + "\"");
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                    isHeaderAdded = true;
                }

                // Now write all the rows. Quote each cell to allow for comma in the text.
                foreach (DataRowView drv in dt.DefaultView)
                {
                    DataRow dr = drv.Row;
                    for (int i = 0; i < iColCount; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            sw.Write("\"" + dr[i].ToString().Replace("\"", "\"\"") + "\"");
                        }
                        if (i < iColCount - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
            }
        }
    }
}
