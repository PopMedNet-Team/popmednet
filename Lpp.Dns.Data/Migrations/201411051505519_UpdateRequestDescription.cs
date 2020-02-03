namespace Lpp.Dns.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestDescription : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Requests", "IX_Description");
            DropIndex("dbo.Requests", "_dta_index_Requests_24_98099390__K20D_K24_3_6_9_11_13_16_18_19_22_23_26_27_28_29_30_33_34_35_36_37_38_39_40_41_42_43_46_47_48_");
            DropIndex("dbo.Requests", "_dta_index_Requests_6_210099789__K24_K30_K35_K39_K37_K40_K19_K48_K46_K33_K34_K9_K42_1_3_6_11_13_15_16_18_20_22_23_26_27_28_29_");
            DropIndex("dbo.Requests", "_dta_index_Requests_6_210099789__K20D_K24_1_3_6_9_11_13_15_16_18_19_22_23_26_27_28_29_30_33_34_35_36_37_38_39_40_41_42_45_46_");
            Sql(@"Declare @TABLENAME varchar(max), @COLUMN varchar(max)
SET @TABLENAME = 'dbo.Requests'
SET @COLUMN = 'Description'
Declare @CONSTRAINT varchar(max)
                    set @CONSTRAINT ='ALTER TABLE '+@TABLENAME+' DROP CONSTRAINT '
                    set @CONSTRAINT = @CONSTRAINT + (select SYS_OBJ.name as CONSTRAINT_NAME
                    from sysobjects SYS_OBJ
                    join syscomments SYS_COM on SYS_OBJ.id = SYS_COM.id
                    join sysobjects SYS_OBJx on SYS_OBJ.parent_obj = SYS_OBJx.id 
                    join sysconstraints SYS_CON on SYS_OBJ.id = SYS_CON.constid
                    join syscolumns SYS_COL on SYS_OBJx.id = SYS_COL.id
                    and SYS_CON.colid = SYS_COL.colid
                    where
                    SYS_OBJ.uid = user_id() and SYS_OBJ.xtype = 'D'
                    and SYS_OBJx.name=@TABLENAME and SYS_COL.name=@COLUMN)
                    exec(@CONSTRAINT)");
            AddColumn("dbo.Requests", "Description2", c => c.String(false, defaultValue: ""));
            Sql("UPDATE Requests SET Description2 = Description");
            DropColumn("dbo.Requests", "Description");
            RenameColumn("dbo.Requests", "Description2", "Description");
        }
        
        public override void Down()
        {
        }
    }
}
