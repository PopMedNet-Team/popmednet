
--------------------------------------------------------
--  DDL for Table CEDD
--------------------------------------------------------

  CREATE TABLE I2B2METADATA.CEDD 
   (	"C_HLEVEL" INT			NOT NULL, 
	"C_FULLNAME" VARCHAR(700)	NOT NULL, 
	"C_NAME" VARCHAR(2000)		NOT NULL, 
	"C_SYNONYM_CD" CHAR(1)		NOT NULL, 
	"C_VISUALATTRIBUTES" CHAR(3)	NOT NULL, 
	"C_TOTALNUM" INT			NULL, 
	"C_BASECODE" VARCHAR(50)	NULL, 
	"C_METADATAXML" TEXT		NULL, 
	"C_FACTTABLECOLUMN" VARCHAR(50)	NOT NULL, 
	"C_TABLENAME" VARCHAR(50)	NOT NULL, 
	"C_COLUMNNAME" VARCHAR(50)	NOT NULL, 
	"C_COLUMNDATATYPE" VARCHAR(50)	NOT NULL, 
	"C_OPERATOR" VARCHAR(10)	NOT NULL, 
	"C_DIMCODE" VARCHAR(700)	NOT NULL, 
	"C_COMMENT" TEXT			NULL, 
	"C_TOOLTIP" VARCHAR(900)	NULL,
	"M_APPLIED_PATH" VARCHAR(700)	NOT NULL, 
	"UPDATE_DATE" DATETIME		NOT NULL, 
	"DOWNLOAD_DATE" DATETIME	NULL, 
	"IMPORT_DATE" DATETIME	NULL, 
	"SOURCESYSTEM_CD" VARCHAR(50)	NULL, 
	"VALUETYPE_CD" VARCHAR(50)	NULL,
	"M_EXCLUSION_CD"	VARCHAR(25) NULL,
	"C_PATH"	VARCHAR(700)   NULL,
	"C_SYMBOL"	VARCHAR(50)	NULL
   ) ;
   
CREATE NONCLUSTERED INDEX [META_FULLNAME_IDX] ON [CEDD]
(
      [C_FULLNAME] ASC,
      [C_HLEVEL] ASC,
      [C_SYNONYM_CD] ASC
)
INCLUDE ( [C_NAME],
[C_VISUALATTRIBUTES],
[C_TOTALNUM],
[C_BASECODE],
[C_TOOLTIP],
[VALUETYPE_CD])
;

CREATE NONCLUSTERED INDEX [META_APPLIED_PATH_IDX] ON [CEDD]
(
      [M_APPLIED_PATH] ASC,
      [C_HLEVEL] ASC,
      [M_EXCLUSION_CD] ASC
)
INCLUDE ( [C_FULLNAME])
;

INSERT INTO I2B2METADATA.TABLE_ACCESS(C_TABLE_CD, C_TABLE_NAME, C_PROTECTED_ACCESS, C_HLEVEL, C_FULLNAME, C_NAME, C_SYNONYM_CD, C_VISUALATTRIBUTES, C_TOTALNUM, C_BASECODE, C_METADATAXML, C_FACTTABLECOLUMN, C_DIMTABLENAME, C_COLUMNNAME, C_COLUMNDATATYPE, C_OPERATOR, C_DIMCODE, C_COMMENT, C_TOOLTIP, C_ENTRY_DATE, C_CHANGE_DATE, C_STATUS_CD, VALUETYPE_CD) 
    VALUES('CEDD', 'CEDD', 'N', 0, '\CEDD\', 'CEDD', 'N', 'CA', NULL, NULL, NULL, 'concept_cd', 'concept_dimension', 'concept_path', 'T', 'LIKE', '\CEDD\', NULL, 'CEDD', NULL, NULL, NULL, NULL)
;

INSERT INTO I2B2METADATA.SCHEMES(C_KEY, C_NAME, C_DESCRIPTION) 
    VALUES('CEDD:', 'CEDD', 'Query Health CEDD')
;
