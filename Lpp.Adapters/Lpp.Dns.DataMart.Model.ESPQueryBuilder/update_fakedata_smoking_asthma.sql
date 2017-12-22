ALTER TABLE esp_mdphnet.esp_demographic ADD COLUMN smoking character varying(20);

update esp_mdphnet.esp_demographic
set smoking =
case ascii(substring(patid from char_length(patid) -1)) %5
when 0 then 'Current'
when 1 then 'Former'
when 2 then 'Passive'
when 3 then 'Never'
else 'Not available'
END
