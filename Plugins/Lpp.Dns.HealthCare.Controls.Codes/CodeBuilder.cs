using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

namespace Lpp.Dns.HealthCare.Controls.Codes
{
    class CodeBuilder
    {
        CategoryBuilder _categoryBuilder = null;
        private IList<string> _listToUpdate = new List<string>();
        private int _goCounter = 0;
        
        public void BuildCodes(string CodeFile, string CategoryFile, string OutputSQLFile)
        {
             try
            {
                ProcessCategoryFile(CategoryFile);
                ProcessCodeFile(CodeFile, OutputSQLFile);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + e.Message);
            }
        }
        
        public void ProcessCategoryFile(string categoryFileName)
        {
            try
            {
                if (!String.IsNullOrEmpty(categoryFileName))
                {
                    _categoryBuilder = new CategoryBuilder();
                    _categoryBuilder.DeserializeObject(categoryFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ProcessCodeFile(string codesFileName, String SQLPathName)
        {
            _goCounter = 0;
            StreamWriter sw = null;
            int codeCounter = 0;
            try
            {
                sw = new StreamWriter(SQLPathName);
                if (_categoryBuilder != null)
                {
                    _categoryBuilder.SaveLookupListCategories(sw);
                }

                sw.WriteLine("--================================================================================================================================= ");
                sw.WriteLine("--CREATING LOOKUP LIST VALUES");
                sw.WriteLine("--================================================================================================================================= ");
                using (XmlTextReader reader = new XmlTextReader(codesFileName))
                {
                    
                    CODE_STRUCT codeData = new CODE_STRUCT();
                    
                    while (reader.ReadToFollowing("CODE_ENTRY"))
                    {
                        XmlReader codeReader = reader.ReadSubtree();
                        codeData.Categories = new List<string>();
                        if (codeReader.ReadToFollowing("LISTID"))
                        {
                            codeData.LISTID = (string)codeReader.ReadElementContentAs("string".GetType(), null);
                        }
                        if (codeReader.ReadToFollowing("CODE"))
                        {
                            codeData.CODE = (string)codeReader.ReadElementContentAs("string".GetType(), null);
                        }
                        if (codeReader.ReadToFollowing("CODE_WITH_DECIMAL"))
                        {
                            codeData.CODE_WITH_DECIMAL = (string)codeReader.ReadElementContentAs("string".GetType(), null);
                        }
                        if (codeReader.ReadToFollowing("DESCRIPTION"))
                        {
                            codeData.DESCRIPTION =(string)codeReader.ReadElementContentAs("string".GetType(), null);
                        }
                        while (codeReader.ReadToFollowing("CATEGORY"))
                        {
                            codeData.Categories.Add((string)codeReader.ReadElementContentAs("string".GetType(), null));
                        }

                        String ValidationMessage = codeData.ValidateData();
                        if (!string.IsNullOrEmpty(ValidationMessage))
                            throw (new Exception(ValidationMessage));
                        
                        SaveCode(codeData, sw);

                        codeReader.Close();
                        codeCounter++;
                    }
                    reader.Close();
                }

                sw.WriteLine("GO");
                sw.WriteLine(" ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception @ entry " + codeCounter);
                throw ex;
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }


        }

        private void SaveCode(CODE_STRUCT code, StreamWriter sw)
        {
            string c;
            string cd;
            string spanDiagnosisID = "10";
            string spanProcedureID = "11";

            //Delete the existing List Lookup Values for the given listid if not done already.
            if (!_listToUpdate.Contains(code.LISTID))
            {
                sw.WriteLine(" ");
                
                sw.WriteLine("DELETE FROM [LookupListValues] WHERE ListId = " + code.LISTID);
                if (code.LISTID == "4")// || code.LISTID == "7" || code.LISTID == "8")
                    sw.WriteLine("DELETE FROM [LookupListValues] WHERE ListId = " + spanDiagnosisID);
                else if (code.LISTID == "5")// || code.LISTID == "9")
                    sw.WriteLine("DELETE FROM [LookupListValues] WHERE ListId = " + spanProcedureID);

                sw.WriteLine("GO");
                sw.WriteLine(" ");
                _listToUpdate.Add(code.LISTID);
            }

            if (code.LISTID != "2" && code.LISTID != "3")
            {
                try
                {
                    c = (code.CODE != "UNDEFINED" ? code.CODE.PadRight(6) : "");
                    cd = (code.CODE_WITH_DECIMAL != "UNDEFINED" ? code.CODE_WITH_DECIMAL.PadRight(6) : "");
                }
                catch
                {
                    c = "UNDEFINED";
                    cd = "UNDEFINED";
                }
            }
            else
            {
                c = string.Empty;
                cd = string.Empty;
            }

            string desc;
            if (!String.IsNullOrEmpty(code.DESCRIPTION))
                desc = code.DESCRIPTION;
            else
                desc = "UNDEFINED";
            desc = desc.Replace("'", "''");
            string categoryLookupCode;
            if (code.LISTID == "1")
            {
                categoryLookupCode = cd;
                cd = desc;
                c = "";
            }
            else if (code.LISTID == "2")
            {
                categoryLookupCode = cd;
                cd = desc;
                c = "";
            }
            else if (code.LISTID == "6")
            {
                cd = c;
                categoryLookupCode = c;
            }
            else
            {
                categoryLookupCode = cd;
            }

            /*
                * Code can belong to multiple categories. One INSERT for each category.
                */
            foreach (String catName in code.Categories)
            {
                string categoryID = (_categoryBuilder != null ? _categoryBuilder.LookupCategory(code.LISTID, catName, categoryLookupCode != null ? categoryLookupCode : "UNDEFINED") : "0");
                string x = "INSERT INTO [dbo].[LookupListValues] ([ListId],[CategoryId],[ItemName],[ItemCode],[ItemCodeWithNoPeriod]) VALUES (" + code.LISTID + ", " + categoryID + ",'" + desc + "','" + cd.Replace("'", "''") + "','" + c.Replace("'", "''") + "')";
                sw.WriteLine(x);

                /*
                    * SPAN- Diagnostics(ListID=10) represents all diagnostic codes (ListID=4,7,8)
                    * SPAN- Procedure(ListID=11) represents all procedure codes (ListID=5,9)
                    * Thus whenever an entry for ListID=4,7,8 encountered, put an additional entry for ListID=10..Keep the Category ID same
                    * whenever an entry for ListID=5,9 encountered, put an additional entry for ListID=11..Keep the Category ID same
                    */
                string spanListID = string.Empty;
                if (code.LISTID == "4" || code.LISTID == "7" || code.LISTID == "8")
                    spanListID = "10";
                else if (code.LISTID == "5" || code.LISTID == "9")
                    spanListID = "11";
                if (!string.IsNullOrEmpty(spanListID))
                {
                    categoryID = (_categoryBuilder != null ? _categoryBuilder.LookupCategory(spanListID, catName, categoryLookupCode != null ? categoryLookupCode : "UNDEFINED") : "0");
                    x=" IF NOT EXISTS (SELECT 1 FROM [dbo].[LookupListValues] WHERE [ListID] = "  + spanListID + " and [CategoryId] = " + categoryID + " and [ItemName] = '" + desc + "' and [ItemCode] = '" + cd.Replace("'", "''") + "') " ;
                    x = x + " INSERT INTO [dbo].[LookupListValues] ([ListId],[CategoryId],[ItemName],[ItemCode],[ItemCodeWithNoPeriod]) VALUES (" + spanListID + ", " + categoryID + ",'" + desc + "','" + cd.Replace("'", "''") + "','" + c.Replace("'", "''") + "')";
                    sw.WriteLine(x);
                }

            }
            if (_goCounter++ > 100)
            {
                sw.WriteLine("GO");
                _goCounter = 0;
            }
        }
    }

    public struct CODE_STRUCT
    {
       public string LISTID ;
       public string CODE ;
       public string CODE_WITH_DECIMAL ;
       public string DESCRIPTION ;
       public List<String> Categories;

       public String ValidateData()
       {
           List<String> MissingEntries = new List<string>();
           String InvalidMessage = string.Empty;

           if (string.IsNullOrEmpty(LISTID))
               MissingEntries.Add("ListID");
           if (string.IsNullOrEmpty(DESCRIPTION))
               MissingEntries.Add("Description");

           if(MissingEntries.Count>0)
           {
               InvalidMessage = "Following entries are missing:";
               foreach (String en in MissingEntries)
                   InvalidMessage = InvalidMessage + en + ";";
           }

           return InvalidMessage;
       }
    }
}
