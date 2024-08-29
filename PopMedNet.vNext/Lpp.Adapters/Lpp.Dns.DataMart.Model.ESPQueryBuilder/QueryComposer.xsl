<?xml version="1.0"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes"/>

  <!-- Set up a node list variable and a count variable for all <option> nodes in the XML document. -->
  <xsl:variable name="ALL_OPTIONS" select="request_builder/response/report/options/option"/>
  <xsl:variable name="ALL_OPTIONS_COUNT">
    <xsl:value-of select="count($ALL_OPTIONS)"/>
  </xsl:variable>
  <!-- Set up a node list variable and a count variable for all <inclusion_criteria> and <exclusion_criteria> nodes in the XML document. -->
  <xsl:variable name="ALL_CRITERIA" select="request_builder/request/criteria/*[local-name() = 'inclusion_criteria' or local-name() = 'exclusion_criteria']"/>
  <xsl:variable name="ALL_CRITERIA_COUNT">
    <xsl:value-of select="count($ALL_CRITERIA)"/>
  </xsl:variable>

  <!-- Set up a node list variable and a count variable for all <inclusion_criteria> and <exclusion_criteria> nodes in the XML document. -->
  <xsl:variable name="ALL_INC_CRITERIA" select="request_builder/request/criteria/*[local-name() = 'inclusion_criteria']"/>
  <xsl:variable name="ALL_INC_CRITERIA_COUNT">
    <xsl:value-of select="count($ALL_INC_CRITERIA)"/>
  </xsl:variable>

  <!-- Set up a node list variable and a count variable for all <inclusion_criteria> and <exclusion_criteria> nodes in the XML document. -->
  <xsl:variable name="ALL_EXC_CRITERIA" select="request_builder/request/criteria/*[local-name() = 'exclusion_criteria']"/>
  <xsl:variable name="ALL_EXC_CRITERIA_COUNT">
    <xsl:value-of select="count($ALL_EXC_CRITERIA)"/>
  </xsl:variable>

  <!-- Set up variables that will let us check whether ICD9 will be used for stratification, and if so how many digits. -->
  <xsl:variable name="ICD9_OPTION" select="request_builder/response/report/options/option[@name = 'ICD9']"/>
  <xsl:variable name="ICD9_OPTION_DIGITS" select="request_builder/response/report/options/option[@name = 'ICD9']/@value"/>

  <xsl:variable name="INCLUDE_PROJECTION_ADDITIONS" select="request_builder/response/report/options/@projectable"/>
    

  <!--
    ##  Note that I'm using <xsl:text> nodes (or <xsl:value-of> and concat) whenever I
    ##  output text, and strategically employing &#10; (newline) in my text strings.
    ##  This keeps the resulting XML output both easier to read in the log file, and
    ##  quite a bit smaller.
    -->

  <!--
    ##  ENTRY POINT
    -->

  <xsl:template match="/">
    <xsl:call-template name="projectionPrepend"/>
    <xsl:call-template name="mainQuery"/>
    <xsl:call-template name="projectionPostpend"/>
  </xsl:template>

  <!-- Prepend special cross join select to capture all combination of projection support column values. -->
  <xsl:template name="projectionPrepend">
    <!-- Prepend only if options node is specified and all are projection stratification -->
    <xsl:if test="$ALL_OPTIONS and $INCLUDE_PROJECTION_ADDITIONS = 'true'">
      select distinct
      <xsl:if test="$ALL_OPTIONS/@name='Age' and $ALL_OPTIONS/@value='2'">
        ag.item_text as "Ten Year Age Group",
      </xsl:if>
      <xsl:if test="$ALL_OPTIONS/@name='Sex'">
        s.item_text as "Sex",
      </xsl:if>
      <xsl:if test="$ALL_OPTIONS/@name='Ethnicity'">
        re.item_text as "Ethnicity",
      </xsl:if>
      <xsl:if test="$ALL_OPTIONS/@name='Zip'">
        zipcodes.item_text as "Zip",
      </xsl:if>
        case when proj."Patients" is null then 0 else proj."Patients" end
        from esp_mdphnet.uvt_agegroup_10yr ag cross join esp_mdphnet.uvt_sex s cross join esp_mdphnet.uvt_race_ethnicity re
        <xsl:if test="$ALL_OPTIONS/@name='Zip'">cross join (select distinct * from (values <xsl:apply-templates select="//criteria/inclusion_criteria//variable[@name = 'ZipCode']" mode="zipcodes_for_projection_stratification"></xsl:apply-templates>) as zipcodes(item_code,item_text)) as zipcodes</xsl:if>
      left join (
    </xsl:if>
  </xsl:template>

  <!-- Postpend special order clause to capture all combination of projection support column values. -->
  <xsl:template name="projectionPostpend">
    <!-- Prepend only if options node is specified and all are projection stratification -->
    <xsl:if test="$ALL_OPTIONS and $INCLUDE_PROJECTION_ADDITIONS = 'true'">
      ) proj on
      <xsl:if test="$ALL_OPTIONS/@name='Age' and $ALL_OPTIONS/@value='2'">
        ag.item_text=proj."Ten Year Age Group"
        <xsl:if test="$ALL_OPTIONS/@name='Sex' or $ALL_OPTIONS/@name='Ethnicity' or $ALL_OPTIONS/@name='Zip'">
          and
        </xsl:if>
      </xsl:if>
      <xsl:if test="$ALL_OPTIONS/@name='Sex'">
        s.item_text=proj."Sex"
        <xsl:if test="$ALL_OPTIONS/@name='Ethnicity' or $ALL_OPTIONS/@name='Zip'">
          and
        </xsl:if>
      </xsl:if>
      <xsl:if test="$ALL_OPTIONS/@name='Ethnicity'">
          re.item_text=proj."Ethnicity"
      <xsl:if test="$ALL_OPTIONS/@name='Zip'">    and</xsl:if>
      </xsl:if>
        <xsl:if test="$ALL_OPTIONS/@name='Zip'">
            zipcodes.item_code = proj."Zip"
        </xsl:if>
      ORDER BY
      <xsl:apply-templates select="$ALL_OPTIONS" mode="fields_from_options"/>
    </xsl:if>
  </xsl:template>

  <!-- Main query clause. If no projection is needed, the select query starts here. -->
  <xsl:template name="mainQuery">
    <xsl:variable name="HAS_SMOKING">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Smoking']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- Generate the SELECT portion of the outer query. -->
    <xsl:text>SELECT </xsl:text>
    <xsl:apply-templates select="$ALL_OPTIONS" mode="fields_from_options"/>
    <xsl:variable name="count" select="count(//option)"/>
    <xsl:if test="$count > 0">
      <xsl:text>, </xsl:text>
    </xsl:if>
    <xsl:text>COUNT("Patients") as "Patients"</xsl:text>

    <!-- Generate the FROM portion of the outer query -->
    <xsl:text>&#10;FROM (</xsl:text>
    <xsl:apply-templates select="." mode="inner_query"/>
    <xsl:text>&#10;) cohort </xsl:text>
    <xsl:if test="$HAS_SMOKING = 'yes'">
      <xsl:text>&#10;WHERE cohort.smoking IN (</xsl:text>
      <xsl:apply-templates select=".//variable[@name = 'Smoking']" />)
    </xsl:if>

    <!-- Generate the GROUP BY and ORDER BY portions of the outer query. -->
    <xsl:if test="$ALL_OPTIONS_COUNT > 0">
      <xsl:text>&#10;GROUP BY </xsl:text>
      <xsl:apply-templates select="$ALL_OPTIONS" mode="fields_from_options"/>
      <!--<xsl:text>"Patients"</xsl:text>-->
      <xsl:text>&#10;ORDER BY </xsl:text>
      <xsl:apply-templates select="$ALL_OPTIONS" mode="fields_from_options"/>
      <!--<xsl:text>"Patients"</xsl:text>-->
    </xsl:if>
  </xsl:template>

  <xsl:template match="variable">
    <xsl:choose>
      <xsl:when test="@value = 1">
        'Current'
      </xsl:when>
      <xsl:when test="@value = 3">
        'Never'
      </xsl:when>
      <xsl:when test="@value = 5">
        'Not available'
      </xsl:when>
      <xsl:when test="@value = 4">
        'Passive'
      </xsl:when>
      <xsl:when test="@value = 2">
        'Former'
      </xsl:when>
    </xsl:choose>
    <xsl:if test="position() != last()">
      ,
    </xsl:if>
  </xsl:template>

  <!--
    ##  FIELDS FROM OPTIONS (multi-use utility template)
    -->

  <!-- This template is used to generate field lists for the SELECT, GROUP BY and ORDER BY portions of the outer query. -->
  <!-- The "isProjected" parameter when supplied suppresses return of any projection-supported columns even when present. -->
  <xsl:template match="option" mode="fields_from_options">
    <xsl:param name="isProjected"/>

    <xsl:variable name="HAS_ICD9_CODES">
      <xsl:choose>
        <xsl:when test="$ALL_INC_CRITERIA[.//variable/@name = 'Code']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_DISEASES">
      <xsl:choose>
        <xsl:when test="$ALL_INC_CRITERIA[.//variable/@name = 'Disease'] ">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Ignore observation date, age range, center, and zip for Visits since the results only include patients from the encounter table.  We would have to change this to return the visits between the observation range if and only if the count was greater than the min number specified -->
    <xsl:variable name="HAS_VISITS">
      <xsl:choose>
        <xsl:when test="$ALL_INC_CRITERIA[.//variable/@name = 'Visits'] ">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="position" select="position()"/>
    <xsl:variable name="count" select="count(//option)"/>

    <xsl:choose>
      <xsl:when test="@name = 'ICD9' and $HAS_ICD9_CODES = 'yes'">
        <xsl:text>"Code", "Code Description"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@name = 'Disease'and $HAS_DISEASES = 'yes'">
        <xsl:text>"Disease"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period' and ($HAS_DISEASES = 'yes' or $HAS_ICD9_CODES = 'yes') ">
        <xsl:text>"Observation Period"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>

      <xsl:when test="@name = 'Sex' and $isProjected != 'yes'">
        <xsl:text>"Sex"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@name = 'Race'">
        <xsl:text>"Race"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@name = 'Ethnicity' and $isProjected != 'yes'">
        <xsl:text>"Ethnicity"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@name = 'TobaccoUse'">
        <xsl:text>"Tobacco Use"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>
      <xsl:when test="@name = 'Age' and ($HAS_DISEASES = 'yes' or $HAS_ICD9_CODES = 'yes' or $HAS_VISITS = 'yes')">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <xsl:text>"Five Year Age Group" </xsl:text>
            <xsl:if test="$position != $count">
              <xsl:text>,</xsl:text>
            </xsl:if>
          </xsl:when>
          <xsl:when test="@value = '2' and $isProjected != 'yes'">
            <xsl:text>"Ten Year Age Group"</xsl:text>
            <xsl:if test="$position != $count">
              <xsl:text>, </xsl:text>
            </xsl:if>
          </xsl:when>
        </xsl:choose>
      </xsl:when>

      <xsl:when test="@name = 'CenterId' and ($HAS_DISEASES = 'yes' or $HAS_ICD9_CODES = 'yes')">
        <xsl:text>"Center"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>

      <xsl:when test="@name = 'Zip'">
        <xsl:text>"Zip"</xsl:text>
        <xsl:if test="$position != $count">
          <xsl:text>, </xsl:text>
        </xsl:if>
      </xsl:when>

    </xsl:choose>
  </xsl:template>


  <!--
    ##  INNER QUERY (a.k.a. cohort)
    -->

  <xsl:template match="/" mode="inner_query">
    <!-- Generate the SELECT portion of the inner query. -->
    <xsl:apply-templates select="." mode="inner_selectclause"/>
    <!-- Generate the FROM portion of the inner query. -->
    <xsl:apply-templates select="." mode="inner_fromclause"/>
    <!-- Generate the WHERE portion of the inner query. -->
    <xsl:apply-templates select="." mode="inner_whereclause"/>
  </xsl:template>


  <!--
    ##  INNER QUERY "SELECT" CLAUSE
    -->
  <xsl:template match="/" mode="inner_selectclause">
    <xsl:text>&#10;  SELECT DISTINCT d.patid as "Patients", d.smoking</xsl:text>
    <!-- Add columns to the SELECT clause of the inner query based on stratification options. -->
    <xsl:apply-templates select="$ALL_OPTIONS" mode="inner_selectclause_conditions_from_options"/>
  </xsl:template>

  <xsl:template match="option" mode="inner_selectclause_conditions_from_options">

    <xsl:variable name="HAS_ICD9_CODES">
      <xsl:choose>
        <xsl:when test="$ALL_INC_CRITERIA[.//variable/@name = 'Code']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_DISEASES">
      <xsl:choose>
        <xsl:when test="$ALL_INC_CRITERIA[.//variable/@name = 'Disease'] ">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Ignore observation date, age range, center, and zip for Visits since the results only include patients from the encounter table.  We would have to change this to return the visits between the observation range if and only if the count was greater than the min number specified -->
    <xsl:variable name="HAS_VISITS">
      <xsl:choose>
        <xsl:when test="$ALL_INC_CRITERIA[.//variable/@name = 'Visits'] ">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <!-- Note that ICD9 always references the "dx" table alias, which can be uvt_dx_3dig, uvt_dx_4dig, or uvt_dx_5dig. -->

      <xsl:when test="@name = 'ICD9'">
        <xsl:choose>
          <xsl:when test="$ICD9_OPTION_DIGITS = '3'">, dx.item_code </xsl:when>
          <xsl:otherwise>, dx.item_code_with_dec </xsl:otherwise>
        </xsl:choose>
        <xsl:text> AS "Code", dx.item_text AS "Code Description"</xsl:text>
      </xsl:when>

      <!-- Note that Disease always references the "s1" table alias, which is the esp_disease criteria in the FIRST group containing an ICD9 or Disease. -->
      <xsl:when test="@name = 'Disease'">
        <xsl:text>, disease_inc.condition as "Disease"</xsl:text>
      </xsl:when>

      <xsl:when test="@name = 'Observation_Period'">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <!-- Month -->
            <xsl:choose>
              <xsl:when test="$HAS_ICD9_CODES = 'yes'">
                , to_char((date '1960-01-01' + diagnosis_inc.a_date), 'YYYY-MM') AS "Observation Period"
              </xsl:when>
              <xsl:when test="$HAS_DISEASES = 'yes'">
                , to_char((date '1960-01-01' + disease_inc.date), 'YYYY-MM') AS "Observation Period"
              </xsl:when>
              <xsl:when test="$HAS_VISITS = 'yes'">
                <xsl:text>                
                , to_char((date '1960-01-01' + encounter_inc.date), 'YYYY-MM') AS "Observation Period"
                </xsl:text>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '2'">
            <!-- Year -->
            <xsl:choose>
              <xsl:when test="$HAS_ICD9_CODES = 'yes'">
                , date_part('year', date '1960-01-01' + diagnosis_inc.a_date) AS "Observation Period"
              </xsl:when>
              <xsl:when test="$HAS_DISEASES = 'yes'">
                , date_part('year', date '1960-01-01' + disease_inc.date) AS "Observation Period"
              </xsl:when>
              <xsl:when test="$HAS_VISITS = 'yes'">
                , date_part('year', date '1960-01-01' + encounter_inc.date) AS "Observation Period"
              </xsl:when>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
      </xsl:when>

      <xsl:when test="@name = 'Sex'">
        <xsl:text>, s.item_text as "Sex"</xsl:text>
      </xsl:when>

      <xsl:when test="@name = 'Ethnicity'">
        <xsl:text>, e.item_text AS "Ethnicity"
          </xsl:text>
      </xsl:when>

      <xsl:when test="@name = 'Race'">
        <xsl:text>, r.item_text as "Race"</xsl:text>
      </xsl:when>

      <xsl:when test="@name = 'Zip'">
        <xsl:text>, d.zip5 as "Zip"</xsl:text>
      </xsl:when>

      <xsl:when test="@name = 'TobaccoUse'">
        <xsl:text>, d.smoking as "Tobacco Use"</xsl:text>
      </xsl:when>


      <xsl:when test="@name = 'Age'">
        <!-- Find all occurences of Disease -->
        <xsl:variable name="Disease_positions">
          <xsl:for-each select="//variable[@name='Disease']">
            <xsl:value-of select="count(preceding::*) + 1"/>
            <xsl:text> , </xsl:text>
          </xsl:for-each>
        </xsl:variable>
        <!-- Just take the position of the first occurence -->
        <xsl:variable name="Disease_position">
          <xsl:value-of select="substring-before($Disease_positions, ', ')"/>
        </xsl:variable>
        <!--Find all occurences of Visit -->
        <xsl:variable name="Visits_positions">
          <xsl:for-each select="//variable[@name='Visits']">
            <xsl:value-of select="count(preceding::*) + 1"/>
            <xsl:text> , </xsl:text>
          </xsl:for-each>
        </xsl:variable>
        <!-- Just take the position of the first occurence of Visits -->
        <xsl:variable name="Visits_position">
          <xsl:value-of select="substring-before($Visits_positions, ', ')"/>
        </xsl:variable>
        <!--Find the all occurences of Code -->
        <xsl:variable name="ICD9_positions">
          <xsl:for-each select="//variable[@name='Code']">
            <xsl:value-of select="count(preceding::*) + 1"/>
            <xsl:text> , </xsl:text>
          </xsl:for-each>
        </xsl:variable>
        <!-- Just take the position of hte first occurence of Code -->
        <xsl:variable name="ICD9_position">
          <xsl:value-of select="substring-before($ICD9_positions, ', ')"/>
        </xsl:variable>

        <xsl:choose>
          <xsl:when test="$HAS_ICD9_CODES = 'yes' and $HAS_DISEASES = 'yes' and $HAS_VISITS = 'yes'">
            <xsl:choose>
              <xsl:when test="($ICD9_position &lt; $Disease_position) and ($ICD9_position &lt; $Visits_position)">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , diagnosis_inc.age_group_5yr AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , diagnosis_inc.age_group_10yr AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="($Disease_position &lt; $ICD9_position)and ($Disease_position &lt; $Visits_position)">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , disease_inc.age_group_5yr AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , disease_inc.age_group_10yr AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="($Visits_position &lt; $ICD9_position) and ($Visits_position &lt; $Disease_position)">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , "EncAgeGroup5" AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , "EncAgeGroup10" AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$HAS_ICD9_CODES = 'yes' and $HAS_DISEASES = 'yes' and $HAS_VISITS = 'no'">
            <xsl:choose>
              <xsl:when test="$ICD9_position &lt; $Disease_position">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , diagnosis_inc.age_group_5yr AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , diagnosis_inc.age_group_10yr AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , disease_inc.age_group_5yr AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , disease_inc.age_group_10yr AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$HAS_ICD9_CODES = 'yes' and $HAS_DISEASES = 'no' and $HAS_VISITS = 'yes'">
            <xsl:choose>
              <xsl:when test="$ICD9_position &lt; $Visits_position">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , diagnosis_inc.age_group_5yr AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , diagnosis_inc.age_group_10yr AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$Visits_position &lt; $ICD9_position">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , "EncAgeGroup5" AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , "EncAgeGroup10" AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$HAS_ICD9_CODES = 'no' and $HAS_DISEASES = 'yes' and $HAS_VISITS = 'yes'">
            <xsl:choose>
              <xsl:when test="$Disease_position &lt; $Visits_position">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , disease_inc.age_group_5yr AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , disease_inc.age_group_10yr AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$Visits_position &lt; $Disease_position">
                <xsl:choose>
                  <xsl:when test="@value = '1'">
                    <!-- 5 year -->
                    , "EncAgeGroup5" AS "Five Year Age Group"
                  </xsl:when>
                  <xsl:otherwise>
                    <!-- 10 year -->
                    , "EncAgeGroup10" AS "Ten Year Age Group"
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$HAS_ICD9_CODES = 'yes' and $HAS_DISEASES = 'no' and $HAS_VISITS = 'no'">
            <xsl:choose>
              <xsl:when test="@value = '1'">
                <!-- 5 year -->
                , diagnosis_inc.age_group_5yr AS "Five Year Age Group"
              </xsl:when>
              <xsl:otherwise>
                <!-- 10 year -->
                , diagnosis_inc.age_group_10yr AS "Ten Year Age Group"
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$HAS_ICD9_CODES = 'no' and $HAS_DISEASES = 'yes' and $HAS_VISITS = 'no'">
            <xsl:choose>
              <xsl:when test="@value = '1'">
                <!-- 5 year -->
                , disease_inc.age_group_5yr AS "Five Year Age Group"
              </xsl:when>
              <xsl:otherwise>
                <!-- 10 year -->
                , disease_inc.age_group_10yr AS "Ten Year Age Group"
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="$HAS_ICD9_CODES = 'no' and $HAS_DISEASES = 'no' and $HAS_VISITS = 'yes'">
            <xsl:choose>
              <xsl:when test="@value = '1'">
                <!-- 5 year -->
                , "EncAgeGroup5" AS "Five Year Age Group"
              </xsl:when>
              <xsl:otherwise>
                <!-- 10 year -->
                , "EncAgeGroup10" AS "Ten Year Age Group"
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
      </xsl:when>

      <xsl:when test="@name = 'CenterId'">
        <xsl:choose>
          <xsl:when test="$HAS_ICD9_CODES = 'yes'">
            , diagnosis_inc.centerid AS "Center"
          </xsl:when>
          <xsl:when test="$HAS_DISEASES = 'yes'">
            , disease_inc.centerid AS "Center"
          </xsl:when>
          <!--
          <xsl:when test="$HAS_VISITS = 'yes'">
            , encounter_inc.centerid AS "Center"
          </xsl:when>
          -->
        </xsl:choose>
      </xsl:when>

    </xsl:choose>
  </xsl:template>

  <!--
    ##  INNER QUERY "FROM" CLAUSE
    -->
  <xsl:template match="/" mode="inner_fromclause">
    <xsl:text>&#10;  FROM esp_mdphnet.esp_demographic d</xsl:text>
    <xsl:text>&#10;    JOIN esp_mdphnet.uvt_race r on (d.race = r.item_code)</xsl:text>
    <xsl:text>&#10;    JOIN esp_mdphnet.uvt_sex s on (d.sex = s.item_code)</xsl:text>
    <xsl:text>&#10;    JOIN esp_mdphnet.uvt_race_ethnicity e on (d.race_ethnicity = e.item_code)</xsl:text>
    <!-- Add LEFT JOINs to the FROM clause of the inner query for every criteria group containing an ICD9 or Disease. -->
    <xsl:apply-templates select="$ALL_INC_CRITERIA[.//variable/@name[.='Code' or .='Disease' or .='Sex' or .='Race' or .='Age' or .='ZipCode' or .='Visits']]" mode="inner_fromclause_from_criteria"/>
  </xsl:template>


  <xsl:template match="*" mode="inner_fromclause_from_criteria">
    <!-- Set up a variable that will let us check whether the current criteria group is an inclusion or exclusion. -->
    <xsl:variable name="CRITERIA_TYPE">
      <xsl:choose>
        <xsl:when test="local-name() = 'exclusion_criteria'">exclusion</xsl:when>
        <xsl:otherwise>inclusion</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- Set up a variable that will give us the position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
    <xsl:variable name="number" select="position()"/>

    <!-- Get the Observation Period start and end values, if present. -->
    <!-- Note that I'm completely ignoring the AND/OR <operator> nodes, here.  We ALWAYS "AND" observation dates, no matter what. -->
    <xsl:variable name="OBS_PERIOD_START" select=".//variable[@name = 'Observation_Period' and @operator = '&gt;='][1]/@value"/>
    <xsl:variable name="OBS_PERIOD_END" select=".//variable[@name = 'Observation_Period' and @operator = '&lt;='][1]/@value"/>

    <!-- Set up variables telling us whether the current criteria group contains an ICD9 or Disease. -->
    <xsl:variable name="HAS_ICD9_CODES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Code']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_DISEASES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Disease']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_SEXES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Sex']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_RACES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Race']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_ETHNICITY">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Ethnicity']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_VISITS">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Visits']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- Add an esp_diagnosis LEFT JOIN if the criteria group contains an ICD9. -->
    <!-- The table alias will be "g" followed by a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
    <!-- The LEFT JOIN's ON clause will limit the data returned by Observation Period (if specified), to reduce data in the join and speed the query. -->
    <xsl:if test="$HAS_ICD9_CODES = 'yes'">
      <xsl:apply-templates select="." mode="inner_fromclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Code</xsl:with-param>
        <xsl:with-param name="obsPeriodStart" select="$OBS_PERIOD_START"/>
        <xsl:with-param name="obsPeriodEnd" select="$OBS_PERIOD_END"/>
      </xsl:apply-templates>
    </xsl:if>
    <!-- Add an esp_disease LEFT JOIN if the criteria group contains an ICD9. -->
    <!-- The table alias will be "s" followed by a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
    <!-- The LEFT JOIN's ON clause will limit the data returned by Observation Period (if specified), to reduce data in the join and speed the query. -->
    <xsl:if test="$HAS_DISEASES = 'yes'">
      <xsl:apply-templates select="." mode="inner_fromclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Disease</xsl:with-param>
        <xsl:with-param name="obsPeriodStart" select="$OBS_PERIOD_START"/>
        <xsl:with-param name="obsPeriodEnd" select="$OBS_PERIOD_END"/>
      </xsl:apply-templates>
    </xsl:if>

    <!-- Add an nested query for patients that have the minimum visits specified based on the esp_encounter table. -->
    <xsl:if test="$HAS_VISITS = 'yes'">
      <xsl:apply-templates select="." mode="inner_fromclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Visits</xsl:with-param>
        <xsl:with-param name="obsPeriodStart" select="$OBS_PERIOD_START"/>
        <xsl:with-param name="obsPeriodEnd" select="$OBS_PERIOD_END"/>
      </xsl:apply-templates>
    </xsl:if>
  </xsl:template>

  <!-- This template generates a single LEFT JOIN on eithr esp_diagnosis or esp_disease, including its "ON (...)" clause. -->
  <xsl:template match="*" mode="inner_fromclause_joins_from_criteria">
    <xsl:param name="criteria_type"/>
    <xsl:param name="criteria_number"/>
    <xsl:param name="type"/>
    <xsl:param name="obsPeriodStart"/>
    <xsl:param name="obsPeriodEnd"/>

    <!-- Set up a node list variable all <option operator="And"> nodes in this group that contain <variable name="Age"> nodes. -->
    <!-- NOTE: Here, we're only matching Age criteria within a criteria group that also contains an ICD9 or Disease! -->
    <!--       Age criteria specified in groups that don't contain an ICD9 or Disease go in the WHERE clause! -->
    <xsl:variable name="AGE_GROUPS" select=".//operation[@operator = 'And' and variable/@name = 'Age']"/>

    <!-- The first diagnosis term in an inclusion criteria group is only one that can be used to stratify on, so we give it a special name.  All other diagnosis terms are given critera group name suffixes (e.g. diagnosis_cgname) -->
    <xsl:choose>


      <xsl:when test="$type = 'Code'">

        <!-- Determine diagnosis table name based on inc or exc critera groups -->
        <!-- The table alias will be "diagnosis" followed by "_inc" if its a inclusion ICD9, else a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
        <xsl:variable name="diagnosis_table_name">
          <xsl:choose>
            <xsl:when test="$criteria_type = 'inclusion' and $criteria_number = 1">
              <xsl:text>diagnosis_inc</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>diagnosis_</xsl:text>
              <xsl:value-of select="$criteria_number"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:text>&#10;    LEFT JOIN esp_mdphnet.esp_diagnosis </xsl:text>
        <xsl:value-of select="$diagnosis_table_name"/>
        <!-- The ON clause will limit the data returned by Observation Period (if specified), to reduce data in the join and speed the query. -->
        <xsl:text> ON (d.patid = </xsl:text>
        <xsl:value-of select="$diagnosis_table_name"/>
        <xsl:text>.patid</xsl:text>
        <xsl:if test="$obsPeriodStart">
          <xsl:text> and </xsl:text>
          <xsl:value-of select="$diagnosis_table_name"/>
          <xsl:text disable-output-escaping="yes">.a_date &gt;= </xsl:text>
          <xsl:value-of select="$obsPeriodStart"/>
        </xsl:if>
        <xsl:if test="$obsPeriodEnd">
          <xsl:text> and </xsl:text>
          <xsl:value-of select="$diagnosis_table_name"/>
          <xsl:text disable-output-escaping="yes">.a_date &lt;= </xsl:text>
          <xsl:value-of select="$obsPeriodEnd"/>
        </xsl:if>
        <!-- Add "Age at Encounter" checks, if needed. -->
        <xsl:if test="$AGE_GROUPS">
          <!-- We "AND" the specified age groups with the other criteria group conditions. -->
          <!-- If multiple age ranges were selected for the criteria group, we "OR" those ranges together. -->
          <xsl:text> and </xsl:text>
          <!-- "NOT" if it's an exclusion group, nothing if it's an inclusion.  
          <xsl:if test="$criteria_type = 'exclusion'">
            <xsl:text>NOT </xsl:text>
          </xsl:if>
          -->
          <xsl:text>(</xsl:text>
          <xsl:apply-templates select="$AGE_GROUPS" mode="inner_fromclause_join_age_andsets">
            <xsl:with-param name="table_name" select="$diagnosis_table_name"/>
            <xsl:with-param name="criteria_number" select="$criteria_number"/>
            <xsl:with-param name="type" select="$type"/>
          </xsl:apply-templates>
          <xsl:text>)</xsl:text>
        </xsl:if>
        <xsl:text>)</xsl:text>
        <!-- IF we're stratifying by ICD-9, then for the FIRST ICD-9 criteria group only,
             add the LEFT JOIN for the 3, 4, or 5 digit ICD-9 code lookup.  -->
        <xsl:if test="$ICD9_OPTION and $criteria_type = 'inclusion' and $criteria_number = 1">
          <xsl:text>&#10;    LEFT JOIN esp_mdphnet.uvt_dx_</xsl:text>
          <xsl:value-of select="$ICD9_OPTION_DIGITS"/>
          <xsl:text>dig dx ON (</xsl:text>
          <xsl:value-of select="$diagnosis_table_name"/>
          <xsl:text>.dx_code_</xsl:text>
          <xsl:value-of select="$ICD9_OPTION_DIGITS"/>
          <xsl:text>dig = dx.item_code)</xsl:text>
        </xsl:if>
      </xsl:when>

      <!-- DISEASE TABLE JOIN -->
      <xsl:when test="$type = 'Disease'">

        <!-- Determine disease table name based on inc or exc critera groups -->
        <!-- The table alias will be "disease" followed by "_inc" if its a inclusion ICD9, else a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
        <xsl:variable name="disease_table_name">
          <xsl:choose>
            <xsl:when test="$criteria_type = 'inclusion' and $criteria_number = 1">
              <xsl:text>disease_inc</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>disease_</xsl:text>
              <xsl:value-of select="$criteria_number"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>


        <xsl:text>&#10;    LEFT JOIN esp_mdphnet.esp_disease </xsl:text>
        <xsl:value-of select="$disease_table_name"/>
        <!-- The ON clause will limit the data returned by Observation Period (if specified), to reduce data in the join and speed the query. -->
        <xsl:text> ON (d.patid = </xsl:text>
        <xsl:value-of select="$disease_table_name"/>
        <xsl:text>.patid</xsl:text>
        <xsl:if test="$obsPeriodStart">
          <xsl:text> and </xsl:text>
          <xsl:value-of select="$disease_table_name"/>
          <xsl:text disable-output-escaping="yes">.date &gt;= </xsl:text>
          <xsl:value-of select="$obsPeriodStart"/>
        </xsl:if>
        <xsl:if test="$obsPeriodEnd">
          <xsl:text> and </xsl:text>
          <xsl:value-of select="$disease_table_name"/>
          <xsl:text disable-output-escaping="yes">.date &lt;= </xsl:text>
          <xsl:value-of select="$obsPeriodEnd"/>
        </xsl:if>

        <!-- Add "Age at Detect" checks, if needed. -->
        <xsl:if test="$AGE_GROUPS and $criteria_type != 'exclusion'">
          <!-- We "AND" the specified age groups with the other criteria group conditions. -->
          <!-- If multiple age ranges were selected for the criteria group, we "OR" those ranges together. -->
          <xsl:text> and </xsl:text>
          <!-- "NOT" if it's an exclusion group, nothing if it's an inclusion.  -->
          <xsl:if test="$criteria_type = 'exclusion'">
            <xsl:text>NOT </xsl:text>
          </xsl:if>
          <xsl:text>(</xsl:text>
          <xsl:apply-templates select="$AGE_GROUPS" mode="inner_fromclause_join_age_andsets">
            <xsl:with-param name="table_name" select="$disease_table_name"/>
            <xsl:with-param name="criteria_number" select="$criteria_number"/>
            <xsl:with-param name="type" select="$type"/>
          </xsl:apply-templates>
          <xsl:text>)</xsl:text>
        </xsl:if>
        <xsl:text>)</xsl:text>
      </xsl:when>


      <!-- ENCOUNTERS TABLE JOIN -->
      <xsl:when test="$type = 'Visits'">
        <xsl:variable name="visits" select=".//variable[@name = 'Visits']/@value"/>

        <!-- The table alias will be "encounter" followed by "_inc" if its an inclusion, else a number, representing position in the list of criteria groups. -->
        <xsl:variable name="encounter_table_name">
          <xsl:choose>
            <xsl:when test="$criteria_type = 'inclusion' and $criteria_number = 1">
              <xsl:text>encounter_inc</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>encounter_</xsl:text>
              <xsl:value-of select="$criteria_number"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:text disable-output-escaping="yes">&#10;    
            JOIN 
              ( SELECT "Patient", "EncAgeGroup5" , "EncAgeGroup10" 
	              FROM (
		              SELECT "Patient", Count("Encounters") as "Encounters", "EncAgeGroup5", "EncAgeGroup10"
		              FROM (
			              SELECT d.patid as "Patient", encounter.encounterid as "Encounters", encounter.age_group_5yr as "EncAgeGroup5", encounter.age_group_10yr as "EncAgeGroup10"
			              FROM esp_mdphnet.esp_demographic d
			              JOIN esp_mdphnet.esp_encounter encounter on (d.patid = encounter.patid</xsl:text>
        <xsl:if test="$obsPeriodStart">
          <xsl:text> and </xsl:text>
          <xsl:text disable-output-escaping="yes">encounter.a_date &gt;= </xsl:text>
          <xsl:value-of select="$obsPeriodStart"/>
        </xsl:if>
        <xsl:if test="$obsPeriodEnd">
          <xsl:text> and </xsl:text>
          <xsl:text disable-output-escaping="yes">encounter.a_date &lt;= </xsl:text>
          <xsl:value-of select="$obsPeriodEnd"/>
        </xsl:if>

        <xsl:if test="$AGE_GROUPS">
          <!-- We "AND" the specified age groups with the other criteria group conditions. -->
          <!-- If multiple age ranges were selected for the criteria group, we "OR" those ranges together. -->
          <!--<xsl:text> and </xsl:text>-->
          <!-- "NOT" if it's an exclusion group, nothing if it's an inclusion.  
          <xsl:if test="$criteria_type = 'exclusion'">
            <xsl:text>NOT </xsl:text>
          </xsl:if>
          -->
          <!-- Enable this once we figure age from birthdate in the demographics table-->
          <xsl:text> and (</xsl:text>
          <xsl:apply-templates select="$AGE_GROUPS" mode="inner_fromclause_join_age_andsets">
            <xsl:with-param name="table_name" select="'encounter'"/>
            <xsl:with-param name="criteria_number" select="$criteria_number"/>
            <xsl:with-param name="type" select="$type"/>
          </xsl:apply-templates>
          <xsl:text>)</xsl:text>

        </xsl:if>

        <xsl:text disable-output-escaping="yes">
			        )) encounters
		          GROUP BY "Patient", "EncAgeGroup5", "EncAgeGroup10"
		          ORDER BY "Patient", "EncAgeGroup5", "EncAgeGroup10"
	          ) encounter_cohort  
	          WHERE "Encounters" &gt;= </xsl:text>
        <xsl:value-of select="$visits"/>
        <xsl:text>) </xsl:text>
        <xsl:value-of select="$encounter_table_name"/>
        <!-- The ON clause will limit the data returned by Observation Period (if specified), to reduce data in the join and speed the query. -->
        <xsl:text> ON (d.patid = </xsl:text>
        <xsl:value-of select="$encounter_table_name"/>
        <xsl:text>."Patient")</xsl:text>
      </xsl:when>

    </xsl:choose>
  </xsl:template>

  <!-- This template represents a single "AND" block of age conditions (the start AND end of a single age range). -->
  <xsl:template match="operation" mode="inner_fromclause_join_age_andsets">
    <xsl:param name="table_name"/>
    <xsl:param name="criteria_number"/>
    <xsl:param name="type"/>

    <!-- We "AND" the specified age groups with the other criteria group conditions. -->
    <!-- If multiple age ranges were selected for the criteria group, we "OR" those ranges together. -->
    <xsl:if test="position() > 1">
      <xsl:text> or </xsl:text>
    </xsl:if>
    <xsl:text>(</xsl:text>
    <xsl:apply-templates select="./variable[@name = 'Age']" mode="inner_fromclause_join_age_match">
      <xsl:with-param name="table_name" select="$table_name"/>
      <xsl:with-param name="criteria_number" select="$criteria_number"/>
      <xsl:with-param name="type" select="$type"/>
    </xsl:apply-templates>
    <xsl:text>)</xsl:text>
  </xsl:template>

  <!-- This template represents a single age condition (the start OR end of a single age range). -->
  <xsl:template match="variable" mode="inner_fromclause_join_age_match">
    <xsl:param name="table_name"/>
    <xsl:param name="criteria_number"/>
    <xsl:param name="type"/>

    <!-- Set up a variable containing a single quote, to make the concat() statements below a bit more readable. -->
    <xsl:variable name="apos">'</xsl:variable>

    <xsl:if test="position() > 1">
      <xsl:text> and </xsl:text>
    </xsl:if>
    <xsl:choose>
      <xsl:when test="$type = 'Code'">
        <xsl:value-of select="concat($table_name, '.age_at_enc_year ')"/>
      </xsl:when>
      <xsl:when test="$type = 'Disease'">
        <xsl:value-of select="concat($table_name, '.age_at_detect_year ')"/>
      </xsl:when>
      <xsl:when test="$type = 'Visits'">
        <xsl:value-of select="concat($table_name, '.age_at_enc_year ')"/>
      </xsl:when>
    </xsl:choose>
    <xsl:value-of disable-output-escaping="yes" select="concat(@operator, ' ', @value)"/>
  </xsl:template>

  <!--
    ##  INNER QUERY "WHERE" CLAUSE
    -->

  <xsl:template match="/" mode="inner_whereclause">
    <!-- Add WHERE subclauses to the inner query for every criteria group containing an ICD9 or Disease. -->
    <xsl:apply-templates select="$ALL_INC_CRITERIA" mode="inner_whereclause_from_criteria">
      <xsl:with-param name="INC_CRITERIA" select="'yes'"></xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="$ALL_EXC_CRITERIA" mode="inner_whereclause_from_criteria">
      <xsl:with-param name="INC_CRITERIA" select="'no'"></xsl:with-param>
    </xsl:apply-templates>
  </xsl:template>


  <xsl:template match="*" mode="inner_whereclause_from_criteria">
    <xsl:param name="INC_CRITERIA"/>
    <xsl:choose>
      <xsl:when test="($INC_CRITERIA = 'yes' and position() > 1) or ($INC_CRITERIA = 'no' and ($ALL_INC_CRITERIA_COUNT > 0 or position() > 1))">
        <xsl:text>&#10;    AND (</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>&#10;  WHERE (</xsl:text>
      </xsl:otherwise>
    </xsl:choose>

    <!-- Set up a variable that will let us check whether the current criteria group is an inclusion or exclusion. -->
    <xsl:variable name="CRITERIA_TYPE">
      <xsl:choose>
        <xsl:when test="local-name() = 'exclusion_criteria'">exclusion</xsl:when>
        <xsl:otherwise>inclusion</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- Set up a variable that will give us the position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
    <xsl:variable name="number" select="position()"/>

    <!-- Set up variables telling us whether the current criteria group contains an ICD9 or Disease. -->
    <xsl:variable name="HAS_ICD9_CODES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Code']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="HAS_DISEASES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Disease']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Set up variables telling us whether the current criteria group contains a gender term. -->
    <xsl:variable name="HAS_SEXES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Sex']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Set up variables telling us whether the current criteria group contains a race term. -->
    <xsl:variable name="HAS_RACES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Race']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Set up variables telling us whether the current criteria group contains a race term. -->
    <xsl:variable name="HAS_ETHNICITY">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Ethnicity']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Set up variables telling us whether the current criteria group contains age terms. -->
    <xsl:variable name="HAS_AGES">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'Age']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Set up variables telling us whether the current criteria group contains age terms. -->
    <xsl:variable name="HAS_ZIPS">
      <xsl:choose>
        <xsl:when test=".//variable[@name = 'ZipCode']">yes</xsl:when>
        <xsl:otherwise>no</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- "NOT" if it's an exclusion group, nothing if it's an inclusion.  -->
    <xsl:if test="$CRITERIA_TYPE = 'exclusion'">
      <xsl:text>NOT EXISTS ( SELECT "ExcPatients" FROM
			(SELECT d.patid AS "ExcPatients" 
			 FROM esp_mdphnet.esp_demographic d </xsl:text>
      <!-- Add LEFT JOINs to the FROM clause of the inner query for every criteria group containing an ICD9 or Disease. -->
      <xsl:apply-templates select="current()" mode="inner_fromclause_from_criteria"/>
      <xsl:text> WHERE </xsl:text>
    </xsl:if>
    <!-- Start the clause for this criteria group. -->
    <!-- "if either ICD9 and Disease are specified in this criteria group, then we OR them together in an expression. -->
    <xsl:choose>
      <xsl:when test="$HAS_ICD9_CODES = 'yes' or $HAS_DISEASES = 'yes'">
        <xsl:text> ( </xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>1=1 </xsl:text>
      </xsl:otherwise>
    </xsl:choose>

    <!-- Add ICD9 value checks if needed. -->
    <xsl:if test="$HAS_ICD9_CODES = 'yes'">
      <xsl:text>(</xsl:text>
      <xsl:apply-templates select="." mode="inner_whereclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Code</xsl:with-param>
      </xsl:apply-templates>
      <xsl:text>)</xsl:text>
    </xsl:if>

    <!-- "OR" if both ICD9 and Disease are specified in this criteria group. -->
    <xsl:if test="$HAS_ICD9_CODES = 'yes' and $HAS_DISEASES = 'yes'">
      <xsl:text> OR </xsl:text>
    </xsl:if>

    <!-- Add Disease value checks if needed. -->
    <xsl:if test="$HAS_DISEASES = 'yes'">
      <xsl:apply-templates select="." mode="inner_whereclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Disease</xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>

    <!-- If either ICD9 and Disease are specified in this criteria group, close the subexpression. -->
    <xsl:if test="$HAS_ICD9_CODES = 'yes' or $HAS_DISEASES = 'yes'">
      <xsl:text>  ) </xsl:text>
    </xsl:if>

    <!-- Add Gender criteria -->
    <xsl:if test="$HAS_SEXES = 'yes'">
      <xsl:apply-templates select="." mode="inner_whereclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Sex</xsl:with-param>
      </xsl:apply-templates>
      <xsl:text>)</xsl:text>
    </xsl:if>

    <!-- Add Race criteria -->
    <xsl:if test="$HAS_RACES = 'yes'">
      <xsl:apply-templates select="." mode="inner_whereclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Race</xsl:with-param>
      </xsl:apply-templates>
      <xsl:text>)</xsl:text>
    </xsl:if>

    <!-- Add Ethnicity criteria -->
    <xsl:if test="$HAS_ETHNICITY = 'yes'">
      <xsl:apply-templates select="." mode="inner_whereclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">Ethnicity</xsl:with-param>
      </xsl:apply-templates>
      <xsl:text>)</xsl:text>
    </xsl:if>

    <!-- Add Zip criteria. -->
    <xsl:if test="$HAS_ZIPS = 'yes'">
      <xsl:apply-templates select="." mode="inner_whereclause_joins_from_criteria">
        <xsl:with-param name="criteria_type" select="$CRITERIA_TYPE"/>
        <xsl:with-param name="criteria_number" select="$number"/>
        <xsl:with-param name="type">ZipCode</xsl:with-param>
      </xsl:apply-templates>
    </xsl:if>

    <!-- End the clause for this criteria group. -->
    <xsl:text>)</xsl:text>
    <xsl:if test="$CRITERIA_TYPE = 'exclusion'">
      <xsl:text> XX WHERE "ExcPatients" = d.patid))</xsl:text>
    </xsl:if>
  </xsl:template>

  <!-- This template creates SQL that checks against one or more spefiied ICD9 or Disease values. -->
  <xsl:template match="*" mode="inner_whereclause_joins_from_criteria">
    <xsl:param name="criteria_type"/>
    <xsl:param name="criteria_number"/>
    <xsl:param name="type"/>

    <!-- Set up a variable containing a single quote, to make the concat() statements below a bit more readable. -->
    <xsl:variable name="apos">'</xsl:variable>

    <xsl:choose>

      <xsl:when test="$type = 'Code'">

        <!-- Determine disease table name based on inc or exc critera groups -->
        <!-- The table alias will be "disease" followed by "_inc" if its a inclusion ICD9, else a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
        <xsl:variable name="table_name">
          <xsl:choose>
            <xsl:when test="$criteria_type = 'inclusion' and $criteria_number = 1">
              <xsl:text>diagnosis_inc</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>diagnosis_</xsl:text>
              <xsl:value-of select="$criteria_number"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <!-- ICD9 checks are a series of "dx LIKE upper('...%')" expressions. -->
        <xsl:apply-templates select=".//variable[@name = 'Code']" mode="inner_whereclause_code_match">
          <xsl:with-param name="table_name" select="$table_name"/>
          <xsl:with-param name="criteria_type" select="$criteria_type"/>
          <xsl:with-param name="criteria_number" select="$criteria_number"/>
        </xsl:apply-templates>
      </xsl:when>

      <xsl:when test="$type = 'Disease'">

        <!-- Determine disease table name based on inc or exc critera groups -->
        <!-- The table alias will be "disease" followed by "_inc" if its a inclusion ICD9, else a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
        <xsl:variable name="table_name">
          <xsl:choose>
            <xsl:when test="$criteria_type = 'inclusion' and $criteria_number = 1">
              <xsl:text>disease_inc</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>disease_</xsl:text>
              <xsl:value-of select="$criteria_number"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <!-- Disease checks are a single "condition IN ('...','...','...')" expression. -->
        <xsl:choose>
          <xsl:when test="$criteria_type = 'exclusion'">
            <xsl:value-of select="concat('COALESCE(', $table_name, '.condition,', $apos, $apos, ')')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat($table_name, '.condition')"/>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:text> IN (</xsl:text>
        <xsl:apply-templates select=".//variable[@name = 'Disease']" mode="inner_whereclause_disease_match">
          <xsl:with-param name="table_name" select="$table_name"/>
          <xsl:with-param name="criteria_number" select="$criteria_number"/>
        </xsl:apply-templates>
        <xsl:text>)</xsl:text>
      </xsl:when>


      <xsl:when test="$type = 'ZipCode'">
        <xsl:variable name="ParentPosition" select="count(parent::*/preceding-sibling::*) + 1 "/>
        <xsl:choose>
          <xsl:when test="position() > 1">
            <xsl:text> OR </xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text> AND (</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
        <!-- Determine table name for ZipCode based on inc or exc critera groups -->
        <!-- The table alias will be "disease" followed by "_inc" if its a inclusion ICD9, else a number, representing position in the list of criteria groups CONTAINING AN ICD9 OR DISEASE. -->
        <xsl:variable name="table_name">
          <xsl:text>d</xsl:text>
        </xsl:variable>
        <!-- ZipCode checks are a single "condition IN ('...','...','...')" expression. -->
        <xsl:choose>
          <xsl:when test="$criteria_type = 'exclusion'">
            <xsl:value-of select="concat('COALESCE(', $table_name, '.condition,', $apos, $apos, ')')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat($table_name, '.zip5')"/>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:text> IN (</xsl:text>
        <xsl:apply-templates select=".//variable[@name = 'ZipCode']" mode="inner_whereclause_zipcode_match">
          <xsl:with-param name="table_name" select="$table_name"/>
          <xsl:with-param name="criteria_number" select="$criteria_number"/>
        </xsl:apply-templates>
        <xsl:text>)</xsl:text>
        <xsl:text>)</xsl:text>
      </xsl:when>


      <xsl:when test="$type = 'Sex'">
        <!-- Gender is always uses an AND condition among other terms in the criteria group -->
        <xsl:apply-templates select=".//variable[@name = 'Sex']" mode="inner_whereclause_sex_match">
          <xsl:with-param name="criteria_type" select="$criteria_type"/>
          <xsl:with-param name="criteria_number" select="$criteria_number"/>
        </xsl:apply-templates>
      </xsl:when>

      <xsl:when test="$type = 'Race'">
        <!-- Race is always uses an AND condition among other terms in the criteria group -->
        <xsl:apply-templates select=".//variable[@name = 'Race']" mode="inner_whereclause_race_match">
          <xsl:with-param name="criteria_type" select="$criteria_type"/>
          <xsl:with-param name="criteria_number" select="$criteria_number"/>
        </xsl:apply-templates>
      </xsl:when>

      <xsl:when test="$type = 'Ethnicity'">
        <!-- Race is always uses an AND condition among other terms in the criteria group -->
        <xsl:apply-templates select=".//variable[@name = 'Ethnicity']" mode="inner_whereclause_ethnicity_match">
          <xsl:with-param name="criteria_type" select="$criteria_type"/>
          <xsl:with-param name="criteria_number" select="$criteria_number"/>
        </xsl:apply-templates>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <!-- This template creates a single "dx LIKE upper('...%')" expression, preceded by "OR" if it's not the first one. -->
  <xsl:template match="variable" mode="inner_whereclause_code_match">
    <xsl:param name="table_name"/>
    <xsl:param name="criteria_type"/>
    <xsl:param name="criteria_number"/>

    <!-- Set up a variable containing a single quote, to make the concat() statements below a bit more readable. -->
    <xsl:variable name="apos">'</xsl:variable>

    <xsl:choose>
      <xsl:when test="position() > 1">
        <xsl:text> OR </xsl:text>
      </xsl:when>
    </xsl:choose>

    <xsl:choose>
      <xsl:when test="$criteria_type = 'exclusion'">
        <xsl:value-of select="concat('COALESCE(', $table_name, '.dx,', $apos, $apos, ')')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat($table_name, '.dx')"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text> LIKE upper('</xsl:text>
    <xsl:value-of select="@value"/>
    <xsl:text>%')</xsl:text>
  </xsl:template>


  <!-- This template just encloses a single condition value in single quotes, preceded by a comma if it's not the first one. -->
  <xsl:template match="variable" mode="inner_whereclause_disease_match">
    <xsl:param name="table_name"/>
    <xsl:param name="criteria_number"/>

    <xsl:if test="position() > 1">
      <xsl:text>,</xsl:text>
    </xsl:if>
    <xsl:text>'</xsl:text>
    <xsl:value-of select="@value"/>
    <xsl:text>'</xsl:text>
  </xsl:template>

  <!-- This template just encloses a single condition value in single quotes, preceded by a comma if it's not the first one. -->
  <xsl:template match="variable" mode="inner_whereclause_zipcode_match">
    <xsl:param name="table_name"/>
    <xsl:param name="criteria_number"/>

    <xsl:if test="position() > 1">
      <xsl:text>,</xsl:text>
    </xsl:if>
    <xsl:text>'</xsl:text>
    <xsl:value-of select="@value"/>
    <xsl:text>'</xsl:text>
  </xsl:template>

  <!-- This template is for joining zipcode values together for the dynamic table used in the outer projection stratification query-->
  <xsl:template match="variable" mode="zipcodes_for_projection_stratification">  
      <xsl:if test="position() > 1">
          <xsl:text>,</xsl:text>
      </xsl:if>
      <xsl:text>('</xsl:text>
      <xsl:value-of select="@value"/>
      <xsl:text>','</xsl:text>
      <xsl:value-of select="@value"/>
      <xsl:text>')</xsl:text>
  </xsl:template>



  <!-- This template creates a single "Demographic.Sex = 'M' or Demographic.Sex = 'F'" expression, preceded by "AND" if it's not the first one. -->
  <xsl:template match="variable" mode="inner_whereclause_sex_match">
    <xsl:param name="criteria_type"/>
    <xsl:param name="criteria_number"/>

    <!-- Set up a variable containing a single quote, to make the concat() statements below a bit more readable. -->
    <xsl:variable name="apos">'</xsl:variable>
    <!--
    <xsl:if test="count(parent::*/preceding-sibling::*) + 1  > 1 and position() = 1">
      <xsl:text> AND </xsl:text>
    </xsl:if>
    -->
    <xsl:choose>
      <xsl:when test="position() > 1">
        <xsl:text> OR </xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text> AND (</xsl:text>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:choose>
      <xsl:when test="$criteria_type = 'exclusion'">
        <xsl:value-of select="concat('COALESCE(d', '.sex,', $apos, $apos, ')')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('d', '.sex')"/>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:text> = </xsl:text>
    <xsl:text>'</xsl:text>
    <xsl:value-of select="@value"/>
    <xsl:text>'</xsl:text>
  </xsl:template>

  <xsl:template match="variable" mode="inner_whereclause_race_match">
    <xsl:param name="criteria_type"/>
    <xsl:param name="criteria_number"/>

    <xsl:variable name="ParentPosition" select="count(parent::*/preceding-sibling::*) + 1 "/>

    <xsl:choose>
      <xsl:when test="position() > 1">
        <xsl:text> OR </xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text> AND (</xsl:text>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:variable name="apos">'</xsl:variable>

    <xsl:choose>
      <xsl:when test="$criteria_type = 'exclusion'">
        <xsl:value-of select="concat('COALESCE(d', '.race,', $apos, $apos, ')')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('d', '.race')"/>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:text> = </xsl:text>
    <xsl:value-of select="@value"/>
  </xsl:template>

  <xsl:template match="variable" mode="inner_whereclause_ethnicity_match">
    <xsl:param name="criteria_type"/>
    <xsl:param name="criteria_number"/>

    <xsl:variable name="ParentPosition" select="count(parent::*/preceding-sibling::*) + 1 "/>

    <xsl:choose>
      <xsl:when test="position() > 1">
        <xsl:text> OR </xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text> AND (</xsl:text>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:variable name="apos">'</xsl:variable>

    <xsl:choose>
      <xsl:when test="$criteria_type = 'exclusion'">
        <xsl:value-of select="concat('COALESCE(d', '.race_ethnicity,', $apos, $apos, ')')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('d', '.race_ethnicity')"/>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:text> = </xsl:text>
    <xsl:value-of select="@value"/>
  </xsl:template>
</xsl:stylesheet>