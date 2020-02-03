<?xml version="1.0"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" indent="yes"/>

  <xsl:template match="/">
    SELECT
    Result.Disease,
    <xsl:apply-templates select="request_builder/response/report/options/option"/>
    , COUNT(Result.Patients) AS Patients
    FROM
    (
    SELECT diagnosis.CONCEPT_CD AS Disease,
    DATEDIFF(year, patient.BIRTH_DATE, encounter.START_DATE) AS Age,
    patient.SEX_CD AS Sex,
    patient.RACE_CD AS Race,
    encounter.LOCATION_CD AS CenterId,
    patient.ZIP_CD AS Zip,
    encounter.START_DATE AS Observation_Period,
    diagnosis.PATIENT_NUM AS Patients
    FROM OBSERVATION_FACT diagnosis
    JOIN VISIT_DIMENSION encounter ON diagnosis.ENCOUNTER_NUM = encounter.ENCOUNTER_NUM
    JOIN CONCEPT_DIMENSION concept ON diagnosis.CONCEPT_CD = concept.CONCEPT_CD
    JOIN PATIENT_DIMENSION patient ON diagnosis.PATIENT_NUM = patient.PATIENT_NUM
    WHERE
    <xsl:apply-templates select="request_builder/request/variables/operation/variable"/>
    <xsl:if test="request_builder/request/variables/operation/operation">
      AND <xsl:apply-templates select="request_builder/request/variables/operation/operation"/>
    </xsl:if>
    ) AS Result
    GROUP BY Result.Disease,
    <xsl:apply-templates select="request_builder/response/report/options/option" mode="group"/>
  </xsl:template>

  <xsl:template match="option" mode="group">
    Result.<xsl:value-of select="@name"/>
    <xsl:if test="position() != last()">,</xsl:if>
  </xsl:template>

  <xsl:template match="option">
    Result.<xsl:value-of select="@name"/>
    <xsl:if test="position() != last()">,</xsl:if>
  </xsl:template>

  <!-- 
  <xsl:template match="option[@name='Observation_Period']" mode='group'>
  </xsl:template>
  -->

  <xsl:template match="option[@name='Observation_Period']">
    <xsl:choose>
      <xsl:when test="@value='1'">
        LEFT(CONVERT(VARCHAR, Result.Observation_Period, 102),4) + '-' + LEFT(CONVERT(VARCHAR, Result.Observation_Period, 101),2) AS "Observation Period",
      </xsl:when>
      <xsl:otherwise>
        LEFT(CONVERT(VARCHAR, Result.Observation_Period, 102),4) AS "Observation Period",
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="option[@name='Age']" mode='group'>
    <xsl:choose>
      <xsl:when test="@value='1'">
        CASE
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 0 AND Result.<xsl:value-of select="@name"/> &lt; 5 THEN '00-04'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 5 AND Result.<xsl:value-of select="@name"/> &lt; 10 THEN '05-09'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 10 AND Result.<xsl:value-of select="@name"/> &lt; 15 THEN '10-14'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 15 AND Result.<xsl:value-of select="@name"/> &lt; 20 THEN '15-19'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 20 AND Result.<xsl:value-of select="@name"/> &lt; 25 THEN '20-24'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 25 AND Result.<xsl:value-of select="@name"/> &lt; 30 THEN '25-29'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 30 AND Result.<xsl:value-of select="@name"/> &lt; 35 THEN '30-34'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 35 AND Result.<xsl:value-of select="@name"/> &lt; 40 THEN '35-39'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 40 AND Result.<xsl:value-of select="@name"/> &lt; 45 THEN '40-44'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 45 AND Result.<xsl:value-of select="@name"/> &lt; 50 THEN '45-49'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 50 AND Result.<xsl:value-of select="@name"/> &lt; 55 THEN '50-54'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 55 AND Result.<xsl:value-of select="@name"/> &lt; 60 THEN '55-59'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 60 AND Result.<xsl:value-of select="@name"/> &lt; 65 THEN '60-64'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 65 AND Result.<xsl:value-of select="@name"/> &lt; 70 THEN '65-69'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 70 AND Result.<xsl:value-of select="@name"/> &lt; 75 THEN '70-74'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 75 THEN '75+'
        END
      </xsl:when>
      <xsl:otherwise>
        CASE
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 0 AND Result.<xsl:value-of select="@name"/> &lt; 10 THEN '00-09'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 10 AND Result.<xsl:value-of select="@name"/> &lt; 20 THEN '15-19'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 20 AND Result.<xsl:value-of select="@name"/> &lt; 30 THEN '20-29'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 30 AND Result.<xsl:value-of select="@name"/> &lt; 40 THEN '30-39'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 40 AND Result.<xsl:value-of select="@name"/> &lt; 50 THEN '40-49'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 50 AND Result.<xsl:value-of select="@name"/> &lt; 60 THEN '50-59'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 60 AND Result.<xsl:value-of select="@name"/> &lt; 70 THEN '60-69'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 70 THEN '70+'
        END
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != last()">,</xsl:if>
  </xsl:template>

  <xsl:template name="age" match="option[@name='Age']">
    <xsl:choose>
      <xsl:when test="@value='1'">
        CASE
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 0 AND Result.<xsl:value-of select="@name"/> &lt; 5 THEN '00-04'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 5 AND Result.<xsl:value-of select="@name"/> &lt; 10 THEN '05-09'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 10 AND Result.<xsl:value-of select="@name"/> &lt; 15 THEN '10-14'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 15 AND Result.<xsl:value-of select="@name"/> &lt; 20 THEN '15-19'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 20 AND Result.<xsl:value-of select="@name"/> &lt; 25 THEN '20-24'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 25 AND Result.<xsl:value-of select="@name"/> &lt; 30 THEN '25-29'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 30 AND Result.<xsl:value-of select="@name"/> &lt; 35 THEN '30-34'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 35 AND Result.<xsl:value-of select="@name"/> &lt; 40 THEN '35-39'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 40 AND Result.<xsl:value-of select="@name"/> &lt; 45 THEN '40-44'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 45 AND Result.<xsl:value-of select="@name"/> &lt; 50 THEN '45-49'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 50 AND Result.<xsl:value-of select="@name"/> &lt; 55 THEN '50-54'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 55 AND Result.<xsl:value-of select="@name"/> &lt; 60 THEN '55-59'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 60 AND Result.<xsl:value-of select="@name"/> &lt; 65 THEN '60-64'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 65 AND Result.<xsl:value-of select="@name"/> &lt; 70 THEN '65-69'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 70 AND Result.<xsl:value-of select="@name"/> &lt; 75 THEN '70-74'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 75 THEN '75+'
        END AS "5 Year Age Group"
      </xsl:when>
      <xsl:otherwise>
        CASE
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 0 AND Result.<xsl:value-of select="@name"/> &lt; 10 THEN '00-09'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 10 AND Result.<xsl:value-of select="@name"/> &lt; 20 THEN '15-19'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 20 AND Result.<xsl:value-of select="@name"/> &lt; 30 THEN '20-29'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 30 AND Result.<xsl:value-of select="@name"/> &lt; 40 THEN '30-39'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 40 AND Result.<xsl:value-of select="@name"/> &lt; 50 THEN '40-49'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 50 AND Result.<xsl:value-of select="@name"/> &lt; 60 THEN '50-59'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 60 AND Result.<xsl:value-of select="@name"/> &lt; 70 THEN '60-69'
        WHEN Result.<xsl:value-of select="@name"/> &gt;= 70 THEN '70+'
        END AS "10 Year Age Group"
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != last()">,</xsl:if>
  </xsl:template>

  <xsl:template match="variable[@operator]">
    <xsl:value-of select="@name"/>
    <xsl:value-of select="@operator"/>
    '<xsl:value-of select="@value"/>'
    <xsl:if test="position() != last()">AND </xsl:if>
  </xsl:template>

  <xsl:template match="variable[not(@operator)]">
    <xsl:value-of select="@name"/>_CD
    =
    '<xsl:value-of select="@value"/>'
    <xsl:if test="position() != last()">AND </xsl:if>
  </xsl:template>

  <xsl:template match="variable[@name='Age']">
    DATEDIFF(year, patient.BIRTH_DATE, encounter.START_DATE)
    <xsl:value-of select="@operator"/>
    '<xsl:value-of select="@value"/>'
    <xsl:if test="position() != last()">AND </xsl:if>
  </xsl:template>

  <xsl:template match="variable[@name='Observation_Period']">
    DATEDIFF(day, '1960-01-01', encounter.START_DATE)
    <xsl:value-of select="@operator"/>
    '<xsl:value-of select="@value"/>'
    <xsl:if test="position() != last()">AND </xsl:if>
  </xsl:template>

  <xsl:template match="variable[@name='Disease']">
    UPPER(concept.CONCEPT_PATH) like
    UPPER(CONCAT('\i2b2\Clinical\Diagnoses\Patient Reported\', REPLACE('<xsl:value-of select="@value"/>%', '_', ' ')))
    <xsl:if test="position() != last()">AND </xsl:if>
  </xsl:template>

  <xsl:template match="operation">
    (<xsl:apply-templates select="variable" mode="or"/>)
    <xsl:if test="position() != last()">AND </xsl:if>
  </xsl:template>

  <xsl:template match="variable" mode="or">
    <xsl:value-of select="@name"/>_CD
    =
    '<xsl:value-of select="@value"/>'
    <xsl:if test="position() != last()">OR </xsl:if>
  </xsl:template>

</xsl:stylesheet>
