alter table esp_mdphnet.esp_demographic add column race_ethnicity integer;
update esp_mdphnet.esp_demographic
set race_ethnicity=case 
                     when hispanic='Y' then 6 
                                     when race=5 then 5
                                     when race in (2,4) then 2
                                     when race=3 then 3
                                    when race=1 then 1
                                     else 0
                                   end;
create index esp_demog_race_eth_idx  on esp_mdphnet.esp_demographic (race_ethnicity);
create table esp_mdphnet.uvt_race_ethnicity as
     select distinct 
         pat.race_ethnicity item_code,
               case
                    when pat.race_ethnicity=5 then 'White'::varchar(50)
                    when pat.race_ethnicity=3 then 'Black'::varchar(50)
                    when pat.race_ethnicity=2 then 'Asian'::varchar(50)
                    when pat.race_ethnicity=6 then 'Hispanic'::varchar(50)
                    when pat.race_ethnicity=1 then 'Native American'::varchar(50)
                    when pat.race_ethnicity=0 then 'Other'::varchar(50)
                end item_text
     from esp_mdphnet.esp_demographic pat;
alter table esp_mdphnet.uvt_race_ethnicity add primary key (item_code);
