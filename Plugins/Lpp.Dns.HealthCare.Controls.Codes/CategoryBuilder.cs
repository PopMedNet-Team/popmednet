using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Lpp.Dns.HealthCare.Controls.Codes
{
    class CategoryBuilder
    {
        private CATEGORIES _categoryCodes = null;
        private System.Collections.Specialized.StringDictionary _categories = new System.Collections.Specialized.StringDictionary();
        private Dictionary<string, Dictionary<string, int>> lstCategories = new Dictionary<string, Dictionary<string, int>>();

        private bool IsCategoryExists(String Category, string ListID)
        {
            bool IsExist = false;

            string category = Category.Replace("'", "''");
            string cateoryId = string.Empty;
            //string categoryLoookupKey = string.Empty;

            if (string.IsNullOrEmpty(category)) category = "UNDEFINED";

            if (lstCategories.ContainsKey(ListID) && lstCategories[ListID].ContainsKey(category))
                IsExist = true;

            return IsExist;
        }

        private string GetCategoryId(String Category,string ListID, bool CreateIfNotExists)
        {
            string category = Category.Replace("'", "''");
            string cateoryId = string.Empty;
            string categoryLoookupKey = string.Empty;

            if (string.IsNullOrEmpty(category)) category = "UNDEFINED";

            if (lstCategories.ContainsKey(ListID) && lstCategories[ListID].ContainsKey(category))
                return lstCategories[ListID][category].ToString();
            else if (CreateIfNotExists)
            {
                if (!lstCategories.ContainsKey(ListID)) lstCategories.Add(ListID, new Dictionary<string, int>());
                if (!lstCategories[ListID].ContainsKey(category))
                    lstCategories[ListID].Add(category, lstCategories[ListID].Count + 1);
                return lstCategories[ListID][category].ToString();
            }
            else
                return "0";
        }

        public string LookupCategory(String ListId, String CategoryName, String Code)
        {
            try
            {
                //if (_categories != null)
                //{
                //    foreach (CATEGORIES.CATEGORYRow category in _categoryCodes.CATEGORY)
                //    {
                //        if (ListId.CompareTo(category.LISTID) == 0 && CategoryName.CompareTo(category.CATEGORY_NAME) == 0)
                //        {
                //            return GetCategoryId(CategoryName,ListId,false);
                //        }
                //    }
                //}
                return GetCategoryId(CategoryName, ListId, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public void SaveLookupListCategories(StreamWriter sw)
        {
            try
            {
                int goCounter = 0;
                List<string> ListCategoriesToUpdate = new List<string>();

                sw.WriteLine("--================================================================================================================================= ");
                sw.WriteLine("--CREATING LOOKUP LIST CATEGORIES");
                sw.WriteLine("--================================================================================================================================= ");

                foreach (CATEGORIES.CATEGORYRow category in _categoryCodes.CATEGORY)
                {
                    if (!ListCategoriesToUpdate.Contains(category.LISTID))
                    {
                        sw.WriteLine(" ");
                        sw.WriteLine("DELETE FROM [LookupListCategories] WHERE ListId = " + category.LISTID);

                        if (category.LISTID == "4" || category.LISTID == "7" || category.LISTID == "8")
                        {
                            if (!ListCategoriesToUpdate.Contains("10"))
                            {
                                sw.WriteLine("DELETE FROM [LookupListCategories] WHERE ListId = 10");
                                ListCategoriesToUpdate.Add("10");
                            }
                        }
                        else if (category.LISTID == "5" || category.LISTID == "9")
                        {
                            if (!ListCategoriesToUpdate.Contains("11"))
                            {
                                sw.WriteLine("DELETE FROM [LookupListCategories] WHERE ListId = 11");
                                ListCategoriesToUpdate.Add("11");
                            }
                        }
                        sw.WriteLine("GO");
                        sw.WriteLine(" ");
                        ListCategoriesToUpdate.Add(category.LISTID);
                    }

                    string desc = string.Empty;
                    if (!String.IsNullOrEmpty(category.CATEGORY_NAME))
                        desc = category.CATEGORY_NAME.Replace("'", "''");
                    else
                        desc = "UNDEFINED";
                    if (!IsCategoryExists(desc, category.LISTID))
                    {
                        string categoryID = GetCategoryId(desc, category.LISTID,true);
                        sw.WriteLine("INSERT INTO [dbo].[LookupListCategories] ([ListId],[CategoryId],[CategoryName]) VALUES (" + category.LISTID + ", " + categoryID + ",'" + desc + "')");
                        /*
                            * SPAN- Diagnostics(ListID=10) represents all diagnostic codes (ListID=4,7,8)
                            * SPAN- Procedure(ListID=11) represents all procedure codes (ListID=5,9)
                            * Thus whenever an entry for ListID=4,7,8 encountered, put an additional entry for ListID=10..Keep the Category ID same
                            * whenever an entry for ListID=5,9 encountered, put an additional entry for ListID=11..Keep the Category ID same
                            */
                        int spanListID = 0;
                        string spanCategoryID = "0";
                        if (category.LISTID == "4" || category.LISTID == "7" || category.LISTID == "8")
                            spanListID = 10;
                        else if (category.LISTID == "5" || category.LISTID == "9")
                            spanListID = 11;
                        if ((spanListID == 10 || spanListID == 11) && !IsCategoryExists(desc, spanListID.ToString()))
                        {
                            spanCategoryID = GetCategoryId(desc, spanListID.ToString(),true);
                            //sw.WriteLine("DELETE FROM [LookupListCategories] WHERE ListId = "+ spanListID + " and CategoryId ="+ spanCategoryID);
                            sw.WriteLine("INSERT INTO [dbo].[LookupListCategories] ([ListId],[CategoryId],[CategoryName]) VALUES (" + spanListID + ", " + spanCategoryID + ",'" + desc + "')");
                        }

                        if (goCounter++ > 100)
                        {
                            sw.WriteLine("GO");
                            goCounter = 0;
                        }
                    }
                }
                sw.WriteLine("GO");
                sw.WriteLine(" ");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        private String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        public void DeserializeObject(string settingsFileName)
        {
            FileStream fs = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CATEGORIES));

                // A FileStream is needed to read the XML document.
                fs = new FileStream(settingsFileName, FileMode.Open);
                XmlReader reader = new XmlTextReader(fs);
                _categoryCodes = (CATEGORIES)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

    }
}
