<?xml version="1.0"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes"/>

  <xsl:variable name="CODE">ICD-9 Diagnosis</xsl:variable>
  <xsl:variable name="OPTIONCOUNT">
    <xsl:value-of select="count(request_builder/response/report/options/option)"/>
  </xsl:variable>
  <xsl:variable name="request_type" select="/request_builder/header/request_type"/>

  <xsl:template match="/">
    &#10; SELECT
    <xsl:apply-templates select="request_builder/response/report/options" mode="outerselect"/>
    count("Patients") as "Patients"
    &#10; FROM (
    <xsl:apply-templates select="request_builder/request"/>
    &#10; ) i
    <xsl:if test="$OPTIONCOUNT > 0">
      GROUP BY
      <xsl:apply-templates select="request_builder/response/report/options" mode="group_by"/>
      ORDER BY
      <xsl:apply-templates select="request_builder/response/report/options" mode="group_by"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="request">
    SELECT DISTINCT
    <xsl:apply-templates select="/request_builder/response/report/options" mode="select"/>
    f.PATIENT_NUM AS "Patients"
    FROM OBSERVATION_FACT f
    JOIN PATIENT_DIMENSION p on f.PATIENT_NUM = p.PATIENT_NUM
    JOIN VISIT_DIMENSION v on f.ENCOUNTER_NUM = v.ENCOUNTER_NUM
    JOIN ICD9_CODE_LOOKUP l on f.CONCEPT_CD = l.CONCEPT_CD
    JOIN PMN_I2B2_RACE_CODE_LOOKUP pr on pr.RACE_CD = p.RACE_CD
    JOIN I2B2_PMN_RACE_CODE_LOOKUP rp on p.RACE_CD = rp.RACE_CD
    <xsl:if test="$OPTIONCOUNT > 0">
      <xsl:apply-templates select="/request_builder/response/report/options" mode="join"/>
    </xsl:if>
    WHERE
    <xsl:apply-templates select="request_builder/request"/>
    <xsl:apply-templates select="variables"/>
  </xsl:template>

  <xsl:template match="variables">
    <xsl:apply-templates select="variable">
      <xsl:with-param name="operator">And</xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="operation">
      <xsl:with-param name="operator">And</xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="options"/>
  </xsl:template>

  <xsl:template match="inclusion_criteria">
    <xsl:apply-templates select="variable">
      <xsl:with-param name="operator">And</xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="operation">
      <xsl:with-param name="operator">And</xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="options"/>
  </xsl:template>

  <xsl:template match="exclusion_criteria">
    <xsl:apply-templates select="variable">
      <xsl:with-param name="operator">And</xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="operation">
      <xsl:with-param name="operator">And</xsl:with-param>
    </xsl:apply-templates>
    <xsl:apply-templates select="options"/>
  </xsl:template>

  <xsl:template match="variable">
    <xsl:param name="operator"/>
    <xsl:choose>
      <xsl:when test="@name = 'Code'">
        f.concept_cd
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
        CONVERT(date, f.START_DATE)
      </xsl:when>
      <xsl:when test="@name = 'Age'">
        DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766
      </xsl:when>
      <xsl:when test="@name = 'Sex'">
        p.SEX_CD
      </xsl:when>
      <xsl:when test="@name = 'Race'">
        pr.RACE_CODE
      </xsl:when>
      <xsl:otherwise>
        p.<xsl:value-of select="@name"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:choose>
      <xsl:when test='@operator'>
        <xsl:value-of disable-output-escaping="yes" select="@operator"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="@name = 'Code'">
            like
          </xsl:when>
          <xsl:otherwise>
            =
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:choose>

      <xsl:when test="@name = 'Code'">
        upper('ICD9:<xsl:value-of select="@value"/>%')
      </xsl:when>

      <xsl:when test="@name = 'Observation_Period'">
        DATEADD(DAY,
        <xsl:value-of select="@value"/>
        , CONVERT(date, '1/1/1960'))
      </xsl:when>

      <xsl:when test="@name = 'Race'">
        <xsl:value-of select="@value"/>
        <xsl:text> </xsl:text>
      </xsl:when>
      <xsl:when test="@name = 'Sex'">
        <xsl:choose>
          <xsl:when test="@value = 'M'">
            <xsl:text> 'M' </xsl:text>
          </xsl:when>
          <xsl:when test="@value = 'F'">
            <xsl:text> 'F'  </xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text> 'M' </xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>

      <xsl:otherwise>
        '<xsl:value-of select="@value"/>'
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="following-sibling::variable or following-sibling::operation">
      <xsl:value-of select="$operator"/>
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>

  <xsl:template match="operation">
    <xsl:param name="operator"/>
    (
    <xsl:apply-templates select="variable">
      <xsl:with-param name="operator" select="@operator"/>
    </xsl:apply-templates>
    <xsl:apply-templates select="operation">
      <xsl:with-param name="operator" select="$operator"/>
    </xsl:apply-templates>
    )
    <xsl:if test="following-sibling::operation">
      <xsl:value-of disable-output-escaping="yes" select="$operator"/>
      <xsl:text> </xsl:text>
    </xsl:if>
  </xsl:template>

  <xsl:template match="options" mode="group_by">
    <xsl:apply-templates select="option" mode="group_by"/>
  </xsl:template>

  <xsl:template match="option" mode="group_by">
    <xsl:if test="position() > 1">
      <xsl:text>,</xsl:text>
    </xsl:if>
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        "Code", "Description"
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
        "Observation Period"
      </xsl:when>
      <xsl:when test="@name = 'Sex'">
        "Sex"
      </xsl:when>
      <xsl:when test="@name = 'Race'">
        "Race"
      </xsl:when>
      <xsl:when test="@name = 'Age'">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <!-- 5 year -->
            "5 Year Age Group"
          </xsl:when>
          <xsl:otherwise>
            <!-- 10 year -->
            "10 Year Age Group"
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'CenterId'">
        "Center"
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="options" mode="select">
    <xsl:apply-templates select="option" mode="select"/>
  </xsl:template>

  <xsl:template match="option" mode="outerselect">
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        "Code", "Description",
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
        "Observation Period",
      </xsl:when>
      <xsl:when test="@name = 'Sex'">
        "Sex",
      </xsl:when>
      <xsl:when test="@name = 'Race'">
        "Race",
      </xsl:when>
      <xsl:when test="@name = 'Age'">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <!-- 5 year -->
            "5 Year Age Group",
          </xsl:when>
          <xsl:otherwise>
            <!-- 10 year -->
            "10 Year Age Group",
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'CenterId'">
        "Center",
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="option" mode="select">
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        <xsl:choose>
          <xsl:when test="@value = '3'">
            <!-- 3 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                l.CODE_3DIG AS "Code", l.NAME_3DIG AS "Description",
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '4'">
            <!-- 4 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                l.CODE_4DIG AS "Code", l.NAME_4DIG AS "Description",
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '5'">
            <!-- 5 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                l.CODE_5DIG AS "Code", l.NAME_5DIG AS "Description",
              </xsl:when>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <!-- Month -->
            LEFT(CONVERT(VARCHAR, f.START_DATE, 102),4) + '-' + LEFT(CONVERT(VARCHAR, f.START_DATE, 101),2) AS "Observation Period",
          </xsl:when>
          <xsl:when test="@value = '2'">
            <!-- Year -->
            LEFT(CONVERT(VARCHAR, f.START_DATE, 102),4) AS "Observation Period",
          </xsl:when>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'Sex'">
        p.SEX_CD AS "Sex",
      </xsl:when>
      <xsl:when test="@name = 'Race'">
        rp.RACE_CHAR AS "Race",
      </xsl:when>
      <xsl:when test="@name = 'Age'">
        <xsl:choose>
          <xsl:when  test="@value = '1'">
            <!-- 5 year -->
            <xsl:text disable-output-escaping="yes">
            CASE
              WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 0 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766  &lt; 5 THEN '00-04'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 5 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766  &lt; 10 THEN '05-09'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 10 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 15 THEN '10-14'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 15 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 20 THEN '15-19'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 20 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 25 THEN '20-24'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 25 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 30 THEN '25-29'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 30 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 35 THEN '30-34'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 35 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 40 THEN '35-39'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 40 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 45 THEN '40-44'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 45 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 50 THEN '45-49'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 50 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 55 THEN '50-54'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 55 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 60 THEN '55-59'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 60 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 65 THEN '60-64'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 65 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 70 THEN '65-69'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 70 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 75 THEN '70-74'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 75 THEN '75+'
		        END as "5 Year Age Group",
          </xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <!-- 10 year -->
            <xsl:text disable-output-escaping="yes">
            CASE
              WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 0 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766  &lt; 10 THEN '00-09'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 10 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 20 THEN '10-19'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 20 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 30 THEN '20-29'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 30 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 40 THEN '30-39'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 40 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 50 THEN '40-49'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 50 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 60 THEN '50-59'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 60 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 70 THEN '60-69'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 70 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 79 THEN '70-79'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 80 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 89 THEN '80-89'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 90 AND DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &lt; 99 THEN '90-99'
			        WHEN DATEDIFF(hour, p.BIRTH_DATE,f.START_DATE)/8766 &gt;= 100 THEN '100+'
		        END as "10 Year Age Group",
          </xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'CenterId'">
        v.LOCATION_CD AS "Center",
      </xsl:when>
      <xsl:otherwise>
        f.<xsl:value-of select="@name"/>,
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="options" mode="join">
    <xsl:apply-templates select="option" mode="join"/>
  </xsl:template>

  <xsl:template match="option" mode="join">
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        <xsl:choose>
          <xsl:when test="@value = '3'">
            <!-- 3 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">

              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '4'">
            <!-- 4 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">

              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '5'">
            <!-- 5 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">

              </xsl:when>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="response">
    <xsl:apply-templates select="report"/>
  </xsl:template>

  <xsl:template match="report">
    <xsl:apply-templates select="options" mode="group_by"/>
  </xsl:template>
</xsl:stylesheet>