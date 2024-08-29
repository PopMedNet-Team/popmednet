/*Modifier Migration script
Michael Buck, PhD
mbuck@health.nyc.gov
August 2, 2012
*/

/*Migrates CEDD into MODIFIER_DIMENSION
*Note: If I add modifiers from other ontologies they might not be purged with the SOURCESYSTEM_CD like 'QUERYHEALTH'.
Need to add this in later if needed.
*/

DELETE FROM nyc_i2b2data.dbo.MODIFIER_DIMENSION WHERE SOURCESYSTEM_CD like 'QUERYHEALTH'
;

INSERT INTO nyc_i2b2data.dbo.MODIFIER_DIMENSION
SELECT DISTINCT a.C_FULLNAME as MODIFIER_PATH, a.C_BASECODE as MODIFIER_CD, a.C_NAME as NAME_CHAR, NULL as MODIFIER_BLOB, GETDATE() as UPDATE_DATE, GETDATE() as DOWNLOAD_DATE, GETDATE() as IMPORT_DATE, 'QUERYHEALTH' as SOURCESYSTEM_CD, 1 as UPLOAD_ID 
FROM cedd_ont.dbo.CEDD a
WHERE a.M_APPLIED_PATH <> '@'
and a.C_SYNONYM_CD = 'N'  /*Don't add synonyms; the C_FULLNAME has to be unique in MODIFIER_DIMENSION*/
and a.C_FULLNAME not in (SELECT DISTINCT MODIFIER_PATH FROM nyc_i2b2data.dbo.MODIFIER_DIMENSION) /*Don't reinsert the same rows.*/
and a.C_BASECODE is not null
;

