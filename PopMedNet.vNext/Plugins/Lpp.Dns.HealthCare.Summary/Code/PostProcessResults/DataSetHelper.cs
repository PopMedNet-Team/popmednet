using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Lpp.Dns.HealthCare.Summary.Code
{
    public class DataSetHelper
    {
        private DataSet ds;
        public DataSetHelper(ref DataSet DataSet)
        {
            ds = DataSet;
        }
        public DataSetHelper()
        {
            ds = null;
        }
        private class FieldInfo
        {
            internal string RelationName;
            internal string FieldName;	//source table field name
            internal string FieldAlias;	//destination table field name
            internal string Aggregate;
        }
        private System.Collections.ArrayList m_FieldInfo; private string m_FieldList;
        private System.Collections.ArrayList GroupByFieldInfo; private string GroupByFieldList;
        private void ParseFieldList(string FieldList, bool AllowRelation)
        {
            /*
             * This code parses FieldList into FieldInfo objects  and then 
             * adds them to the m_FieldInfo private member
             * 
             * FieldList systax:  [relationname.]fieldname[ alias], ...
            */
            if (m_FieldList == FieldList) return;
            m_FieldInfo = new System.Collections.ArrayList();
            m_FieldList = FieldList;
            FieldInfo Field; string[] FieldParts; string[] Fields = FieldList.Split(',');
            int i;
            for (i = 0; i <= Fields.Length - 1; i++)
            {
                Field = new FieldInfo();
                //parse FieldAlias
                if (!Fields[i].Trim().StartsWith("'"))
                {
                    FieldParts = Fields[i].Trim().Split(' ');
                    switch (FieldParts.Length)
                    {
                        case 1:
                            //to be set at the end of the loop
                            break;
                        case 2:
                            Field.FieldAlias = FieldParts[1];
                            break;
                        default:
                            throw new Exception("Too many spaces in field definition: '" + Fields[i] + "'.");
                    }
                }
                else
                {
                    string nameStr = Fields[i].Trim();
                    int end = nameStr.Substring(1).IndexOf('\'');
                    string name = nameStr.Substring(0, end + 2);
                    string alias = string.Empty;
                    if (end + 3 < nameStr.Length)
                        alias = nameStr.Substring(end + 3);

                    if (alias.Length == 0)
                    {
                        FieldParts = new String[1];
                        FieldParts[0] = name;
                    }
                    else
                    {
                        FieldParts = new String[2];
                        FieldParts[0] = name;
                        FieldParts[1] = alias;
                    }
                }

                //parse FieldName and RelationName
                FieldParts = FieldParts[0].Split('.');
                switch (FieldParts.Length)
                {
                    case 1:
                        Field.FieldName = FieldParts[0];
                        break;
                    case 2:
                        if (AllowRelation == false)
                            throw new Exception("Relation specifiers not permitted in field list: '" + Fields[i] + "'.");
                        Field.RelationName = FieldParts[0].Trim();
                        Field.FieldName = FieldParts[1].Trim();
                        break;
                    default:
                        throw new Exception("Invalid field definition: " + Fields[i] + "'.");
                }
                if (Field.FieldAlias == null)
                    Field.FieldAlias = Field.FieldName;
                m_FieldInfo.Add(Field);
            }
        }
        private void ParseGroupByFieldList(string FieldList)
        {
            /*
            * Parses FieldList into FieldInfo objects and adds them to the GroupByFieldInfo private member
            * 
            * FieldList syntax: fieldname[ alias]|operatorname(fieldname)[ alias],...
            * 
            * Supported Operators: count,sum,max,min,first,last
            */
            if (GroupByFieldList == FieldList) return;
            GroupByFieldInfo = new System.Collections.ArrayList();
            FieldInfo Field; string[] FieldParts; string[] Fields = FieldList.Split(',');
            for (int i = 0; i <= Fields.Length - 1; i++)
            {
                Field = new FieldInfo();
                //Parse FieldAlias
                if (!Fields[i].Trim().StartsWith("'"))
                {
                    FieldParts = Fields[i].Trim().Split(' ');
                    switch (FieldParts.Length)
                    {
                        case 1:
                            //to be set at the end of the loop
                            break;
                        case 2:
                            Field.FieldAlias = FieldParts[1];
                            break;
                        default:
                            throw new ArgumentException("Too many spaces in field definition: '" + Fields[i] + "'.");
                    }
                }
                else
                {
                    string nameStr = Fields[i].Trim();
                    int end = nameStr.Substring(1).IndexOf('\'');
                    string name = nameStr.Substring(0, end + 2);
                    string alias = string.Empty;
                    if (end + 3 < nameStr.Length)
                        alias = nameStr.Substring(end + 3);

                    if (alias.Length == 0)
                    {
                        FieldParts = new String[1];
                        FieldParts[0] = name;
                    }
                    else
                    {
                        FieldParts = new String[2];
                        FieldParts[0] = name;
                        FieldParts[1] = alias;
                    }
                }
                //Parse FieldName and Aggregate
                FieldParts = FieldParts[0].Split('(');
                switch (FieldParts.Length)
                {
                    case 1:
                        Field.FieldName = FieldParts[0];
                        break;
                    case 2:
                        Field.Aggregate = FieldParts[0].Trim().ToLower();    //we're doing a case-sensitive comparison later
                        Field.FieldName = FieldParts[1].Trim(' ', ')');
                        break;
                    default:
                        throw new ArgumentException("Invalid field definition: '" + Fields[i] + "'.");
                }
                if (Field.FieldAlias == null)
                {
                    if (Field.Aggregate == null)
                        Field.FieldAlias = Field.FieldName;
                    else
                        Field.FieldAlias = Field.Aggregate + "of" + Field.FieldName;
                }
                GroupByFieldInfo.Add(Field);
            }
            GroupByFieldList = FieldList;
        }
        private DataTable CreateGroupByTable(string TableName, DataTable SourceTable, string FieldList)
        {
            /*
             * Creates a table based on aggregates of fields of another table
             * 
             * RowFilter affects rows before GroupBy operation. No "Having" support
             * though this can be emulated by subsequent filtering of the table that results
             * 
             *  FieldList syntax: fieldname[ alias]|aggregatefunction(fieldname)[ alias], ...
            */
            if (FieldList == null)
            {
                throw new ArgumentException("You must specify at least one field in the field list.");
                //return CreateTable(TableName, SourceTable);
            }
            else
            {
                DataTable dt = new DataTable(TableName);
                ParseGroupByFieldList(FieldList);
                foreach (FieldInfo Field in GroupByFieldInfo)
                {
                    DataColumn dc = SourceTable.Columns[Field.FieldName];
                    if (Field.Aggregate == null)
                        dt.Columns.Add(Field.FieldAlias, dc.DataType, dc.Expression);
                    else
                        dt.Columns.Add(Field.FieldAlias, dc.DataType);
                }
                if (ds != null)
                    ds.Tables.Add(dt);
                return dt;
            }
        }
        private void InsertGroupByInto(DataTable DestTable, DataTable SourceTable, string FieldList,
            string RowFilter, string GroupBy)
        {
            /*
             * Copies the selected rows and columns from SourceTable and inserts them into DestTable
             * FieldList has same format as CreateGroupByTable
            */
            if (FieldList == null)
                throw new ArgumentException("You must specify at least one field in the field list.");
            ParseGroupByFieldList(FieldList);	//parse field list
            ParseFieldList(GroupBy, false);			//parse field names to Group By into an arraylist
            DataRow[] Rows = SourceTable.Select(RowFilter, GroupBy);
            DataRow LastSourceRow = null, DestRow = null; bool SameRow; int RowCount = 0;
            foreach (DataRow SourceRow in Rows)
            {
                SameRow = false;
                if (LastSourceRow != null)
                {
                    SameRow = true;
                    foreach (FieldInfo Field in m_FieldInfo)
                    {
                        if (!ColumnEqual(LastSourceRow[Field.FieldName], SourceRow[Field.FieldName]))
                        {
                            SameRow = false;
                            break;
                        }
                    }
                    if (!SameRow)
                        DestTable.Rows.Add(DestRow);
                }
                if (!SameRow)
                {
                    DestRow = DestTable.NewRow();
                    RowCount = 0;
                }
                RowCount += 1;
                foreach (FieldInfo Field in GroupByFieldInfo)
                {
                    switch (Field.Aggregate)    //this test is case-sensitive
                    {
                        case null:        //implicit last
                        case "":        //implicit last
                        case "last":
                            DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
                            break;
                        case "first":
                            if (RowCount == 1)
                                DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
                            break;
                        case "count":
                            DestRow[Field.FieldAlias] = RowCount;
                            break;
                        case "sum":
                            DestRow[Field.FieldAlias] = Add(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
                            break;
                        case "max":
                            DestRow[Field.FieldAlias] = Max(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
                            break;
                        case "min":
                            if (RowCount == 1)
                                DestRow[Field.FieldAlias] = SourceRow[Field.FieldName];
                            else
                                DestRow[Field.FieldAlias] = Min(DestRow[Field.FieldAlias], SourceRow[Field.FieldName]);
                            break;
                        case "avg":
                            object o = Avg(Rows, SourceRow, Field.FieldName);
                            DestRow[Field.FieldAlias] = (null == o || string.Empty == o.ToString()) ? o : decimal.Parse(o.ToString());
                            break;

                    }
                }
                LastSourceRow = SourceRow;
            }
            if (DestRow != null)
                DestTable.Rows.Add(DestRow);
        }


        private FieldInfo LocateFieldInfoByName(System.Collections.ArrayList FieldList, string Name)
        {
            //Looks up a FieldInfo record based on FieldName
            foreach (FieldInfo Field in FieldList)
            {
                if (Field.FieldName == Name)
                    return Field;
            }
            return null;
        }

        private bool ColumnEqual(object a, object b)
        {
            /*
             * Compares two values to see if they are equal. Also compares DBNULL.Value.
             * 
             * Note: If your DataTable contains object fields, you must extend this
             * function to handle them in a meaningful way if you intend to group on them.
            */
            if ((a is DBNull) && (b is DBNull))
                return true;    //both are null
            if ((a is DBNull) || (b is DBNull))
                return false;    //only one is null
            //return (a == b);    //value type standard comparison
            return (a.ToString().CompareTo(b.ToString()) == 0);
        }

        private object Min(object a, object b)
        {
            //Returns MIN of two values - DBNull is less than all others
            if ((a is DBNull) || (b is DBNull))
                return DBNull.Value;
            if (((IComparable)a).CompareTo(b) == -1)
                return a;
            else
                return b;
        }

        private object Max(object a, object b)
        {
            //Returns Max of two values - DBNull is less than all others
            if (a is DBNull)
                return b;
            if (b is DBNull)
                return a;
            if (((IComparable)a).CompareTo(b) == 1)
                return a;
            else
                return b;
        }

        private object Add(object a, object b)
        {
            //Adds two values - if one is DBNull, then returns the other
            if (a is DBNull)
                return b;
            if (b is DBNull)
                return a;
            //return ((decimal)a + (decimal)b);
            return (Decimal.Parse(a.ToString()) + Decimal.Parse(b.ToString()));
        }
        public DataTable SelectGroupByInto(string TableName, DataTable SourceTable, string FieldList,
            string RowFilter, string GroupBy)
        {
            /*
             * Selects data from one DataTable to another and performs various aggregate functions
             * along the way. See InsertGroupByInto and ParseGroupByFieldList for supported aggregate functions.
             */
            DataTable dt = CreateGroupByTable(TableName, SourceTable, FieldList);
            InsertGroupByInto(dt, SourceTable, FieldList, RowFilter, GroupBy);
            return dt;
        }

        private object Avg(DataRow[] Rows, DataRow currentRow, string fieldName)
        {
            decimal dblAvg = 0; int RowCount = 0; bool SameRow = true;
            System.Collections.ArrayList Val = new System.Collections.ArrayList();

            //Count the number of equal rows
            foreach (DataRow SourceRow in Rows)
            {
                foreach (FieldInfo Field in m_FieldInfo)
                {
                    SameRow = true;
                    if (!ColumnEqual(SourceRow[Field.FieldName], currentRow[Field.FieldName]))
                    {
                        SameRow = false;
                        break;
                    }
                }
                if (SameRow)
                {
                    RowCount += 1;
                    Val.Add((SourceRow[fieldName] is DBNull || string.IsNullOrEmpty(SourceRow[fieldName].ToString())) ? 0 : SourceRow[fieldName]);
                }
            }

            //Calculate the average
            for (int i = 0; i < Val.Count; i++)
            {
                dblAvg += Convert.ToDecimal(Val[i]);
            }

            dblAvg /= Val.Count;
            if (dblAvg == 0)
                return DBNull.Value;
            else
                return Decimal.Round(dblAvg, 1);
        }
    }
}
