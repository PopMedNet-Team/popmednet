SELECT DISTINCT 'INSERT INTO [ICD9_CODE_LOOKUP] ([CONCEPT_CD],[CODE_3DIG],[NAME_3DIG],[CODE_4DIG],[NAME_4DIG],[CODE_5DIG],[NAME_5DIG]) VALUES (''ICD9:' || esp_diagnosis.dx ||  ''',''' ||  uvt_dx_3dig.item_code || ''',''' ||  uvt_dx_3dig.item_text ||  ''',''' ||  uvt_dx_4dig.item_code_with_dec || ''',''' ||  uvt_dx_4dig.item_text || ''',''' ||  uvt_dx_5dig.item_code_with_dec || ''',''' ||  uvt_dx_5dig.item_text || ''')'
FROM 
  esp_mdphnet.esp_diagnosis, 
  esp_mdphnet.uvt_dx_3dig, 
  esp_mdphnet.uvt_dx_4dig, 
  esp_mdphnet.uvt_dx_5dig
WHERE 
  esp_diagnosis.dx_code_3dig = uvt_dx_3dig.item_code AND
  esp_diagnosis.dx_code_4dig = uvt_dx_4dig.item_code AND
  esp_diagnosis.dx_code_5dig = uvt_dx_5dig.item_code
