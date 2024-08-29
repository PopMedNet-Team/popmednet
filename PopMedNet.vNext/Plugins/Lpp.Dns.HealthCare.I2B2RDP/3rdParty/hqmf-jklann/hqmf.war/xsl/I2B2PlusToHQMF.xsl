<?xml version="1.0" encoding="UTF-8"?>
<!-- i2b2 XML to HQMF Converter
    Changelog:
        Keith W. Boone - 3/2012 - initial version
          Supports basic transformations using the i2b2_demo ontology.
          Translates the following sections: Demographics->Age; Diagnosis; Lab Test Results
        Nageshwara Bashyam - 3/2012 - bugixes
        Jeff Klann, PhD - 03/28/2012
           Added support for sections in the SHRINE ontology: Demographics-> Age, Race, Marital Status, Gender, Language ;
             Medications ; Procedures ; Diagnosis
           Note that SHRINE lab results DO NOT work properly - this was left unfinished partly because there are only a few demo labs in standard
             SHRINE, so the demo ontology might be a better choice for labs?
        Jeff Klann, PhD - 04/2/2012, 4/3/2012
           Bugfixes. Added some boilerplate to link to the ONT cell through a Java webservice call.
        Jeff Klann, PhD - 6/20/2012
        	First pass at integration is complete. The webservice URL to get term info is passed as as parameter "serviceurl", which defaults to localhost. Only the key request is sent, not the authentication info. The service URL's port is hardcoded to 8080 right now, which should be changed. Age codes still rely on the local concept dictionary because my lookup mechanism doesn't use fields which are searchable through the API. Probably a good way to do it is to have a service refresh all age codes occasionally and store a local copy.  
        Jeff Klann, PhD - 7/9/2012
          Updates wrt integration. Now extracts security parameters from the i2b2 message and uses this to query the ONT cell.
          Still has the problem with age range lookups
          Now requires a full i2b2 query message (not just query definition) for security parameter lookup
        Jeff Klann, PhD 7/12/2012
          \\SHRINE rootkey properly causes SHRINE| to be prepended
        Jeff Klann, PhD 7/16/2012
          Errors are output as XML comments when either the concept can't be found or the user couldn't be authenticated.
        Jeff Klann, PhD 7/23/2012
           New preprocessing approach that expects a query_definition with basecodes so that folders can
          be recursively expanded until basecodes are found. Run toi2b2plus.xsl on your i2b2 before sending it
          here. The age handling was modified. Single-year ages now convert a basecode to an IVL_PQ. Ranges
          still use Keith's method of decomposing the item name - this assumes the same age range bucket
          exists on the destination system so might not be a permanent fix.
           Changed measure period to 5 years (should eventually be user configurable)
           Added NDC codes to the codesystem mapping.
        Jeff Klann, PhD 7/30/2012
          Added a stub for modifiers. Rejects all modifiers without a CEDD: basecode and generates a dummy element
         for those.
        Jeff Klann, PhD 8/8/2012
          Added scaffolding for CEDD section and demographic type detection. Really this logic should be refactored, though.
          Simplified key information extraction in the panel template. A precursor to the refactoring I want to do.
          Set the default measurePeriod to 200 years
        Jeff Klann, PhD 8/9/2012
          Added external configuration support for basecode->codeSystem conversion.
        Jeff Klann, PhD 9/26/2012
          A very significant rewrite that retools most of the data criteria sections to allow for easier updates and moves
         a lot into the external configuration file. Some modifiers are now supported as well as anonymous codes and text.
         The large overhaul now supports a large portion of basic CEDD. See below.
        Jeff Klann, PhD 10/2/2012
          SHRINE| basecodes were no longer, supported, it seems like for awhile.
          Bugfixes. Extra xsi:type appearing; Got rid of erroneous use of originalText; <value> now has its value in the node text 
          Remaining (pre-ballot) schema-validity issues: value constraints without a unit shouldn't have an empty unit, interpretationCode should be CE, code should always appear before value (doesn't with problem modifiers)
        Jeff Klann, PhD 10/21/2012
          Added support for OID -> valueSets. 
        
        The current supported ontologies:
         SHRINE:
           Demographics (Age, Race, Marital Status, Gender, Language), Medications (RxNorm ingredients), Procedures, Diagnoses (ICD-9)
         i2b2_demo:
           meds (NDC codes), procedures (ICD), diagnoses (ICD), labs (LOINC), some demographics (those supported by SHRINE)
         CEDD: 
          overall features (anonymous codes, text values), results (including result interpretation codes), vitals,
          patientInformation (date of birth, age, city/state/zip, ethncity, language, gender, race, religion, marital status),
          condition (including type code), results (including interpretation codes), medication, 
          encounter (not age at visit or length of stay but including encounter provider organization name),
          healthcare provider id, name, and organization name (but not location modifiers),
          medication (encoded as administration), social history (including type codes)
        
        
        Todo/concerns:
           - Finish CEDD support for NY pilot, especially date/time modifiers
           - Extensive testing and validation, including:
                * What is the preferred SNOMED code for Language?
           - Zipcodes are unraveled by i2b2plus.xsl but probably shouldn't be when a state or city can
              be specified instead. It is very slow.
           - Come in line with HQMFr2 when it passes ballot
           - Various sections marked TBD:        
-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:xalan="http://xml.apache.org/xalan"
  xmlns:str="http://exslt.org/strings"
  xmlns:dyn="http://exslt.org/dynamic"
  xmlns:java="http://xml.apache.org/xslt/java"
  xmlns:mc="urn:jklann:hqmf:metaConfig"
  xmlns:ont="xalan://edu.harvard.i2b2.eclipse.plugins.ontology.ws.OntServiceDriver"
  extension-element-prefixes="ont"
  exclude-result-prefixes="mc dyn java str xalan ont"
  xmlns="urn:hl7-org:v3" version="1.0">
  <xsl:import href="time.xsl"/>
  <xsl:import href="url-encode.xsl"/>
  <xsl:output indent="yes" xalan:indent-amount="2"/>
   
  <!-- Load the metaconfig. -->
  <xsl:variable name="metaconfig" select="document('translatorMetaConfig.xml')"/>
  
  <xsl:template name="replace-string">
    <xsl:param name="text"/>
    <xsl:param name="replace"/>
    <xsl:param name="with"/>
    <xsl:choose>
      <xsl:when test="contains($text,$replace)">
        <xsl:value-of select="substring-before($text,$replace)"/>
        <xsl:value-of select="$with"/>
        <xsl:call-template name="replace-string">
          <xsl:with-param name="text"
            select="substring-after($text,$replace)"/>
          <xsl:with-param name="replace" select="$replace"/>
          <xsl:with-param name="with" select="$with"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <!-- Create an OID for the document -->
  <xsl:variable name="OID"
    >2.16.840.1.113883.3.1619.5148.3</xsl:variable>
  <!-- Create a variable called docOID that will be used
    as the OID for the HQMF document being generated.  It is created
    from a root OID (see above), and nowOID.
    
    nowOID is a variable created by time.xsl that contains the
    current timestamp in OID format.  Adding it to the end of an
    existing OID creates a new OID -->
  <xsl:variable name="docOID">
    <xsl:value-of select="$OID"/>
    <xsl:text>.</xsl:text>
    <xsl:value-of select="$nowOID"/>
  </xsl:variable>

  <!-- This template processes the query_definition element
    found in an I2B2 query request 
  -->
  <xsl:template match="//query_definition">
    <xsl:if
      test="//result_output_list/result_output/@name != 'patient_count_xml'">
      <!-- if asking for something other than patient counts, terminate with
        an error -->
      <xsl:message terminate="yes">(501) Cannot map <xsl:value-of
          select="//result_output_list/result_output/@name"/> to HQMF - only patient count queries are supported.
      </xsl:message>
    </xsl:if>
    <!-- Create the quality measure document header with appropriate values -->
    <QualityMeasureDocument xmlns="urn:hl7-org:v3"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      classCode="DOC">
      <typeId root="2.16.840.1.113883.1.3" extension="POQM_HD00001"/>
      <id root="{$docOID}.1"/>
      <code code="57024-2" codeSystem="2.16.840.1.113883.6.1"/>
      <!-- take the title from the query_name field -->
      <title><xsl:value-of select='//query_name'/></title>
      <statusCode code="completed"/>
      <setId root="{$docOID}"/>
      <versionNumber value="1"/>
      <author typeCode="AUT" contextControlCode="OP">
        <assignedPerson classCode="ASSIGNED"/>
      </author>
      <custodian typeCode="CST">
        <assignedPerson classCode="ASSIGNED"/>
      </custodian>

      <!-- TBD: This needs to be able to be set from a query, to
        control start and end date for a measure.  But right now,
        the date range can only be set for a panel, not for the
        entire query.  Not sure how to handle this.
        -->
      <controlVariable>
        <localVariableName>MeasurePeriod</localVariableName>
        <measurePeriod>
          <id root="0" extension="StartDate"/>
          <value>
            <width unit="a" value="200"/>
          </value>
        </measurePeriod>
      </controlVariable>
      <component>
        <measureDescriptionSection>
          <title>Measure Description Section</title>
          <text><xsl:value-of select='//query_name'/></text>
        </measureDescriptionSection>
      </component>

      <component>
        <dataCriteriaSection>
          <code code="57025-9" codeSystem="2.16.840.1.113883.6.1"/>
          <title>Data Criteria Section</title>
          <text>This section describes the data criteria.</text>
          <!-- This chunk of definitions is fixed data that is used
            to tie query criteria into an implementation data model
          -->
          <definition>
            <observationDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Demographics"/>
            </observationDefinition>
          </definition>
          <definition>
            <encounterDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Encounters"/>
            </encounterDefinition>
          </definition>
          <definition>
            <observationDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Problems"/>
            </observationDefinition>
          </definition>
          <definition>
            <observationDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Allergies"/>
            </observationDefinition>
          </definition>
          <definition>
            <procedureDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Procedures"/>
            </procedureDefinition>
          </definition>
          <definition>
            <observationDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Results"/>
            </observationDefinition>
          </definition>
          <definition>
            <observationDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Vitals"/>
            </observationDefinition>
          </definition>
          <definition>
            <substanceAdministrationDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1"
                extension="Medications"/>
            </substanceAdministrationDefinition>
          </definition>
          <definition>
            <supplyDefinition>
              <id root="2.16.840.1.113883.3.1619.5148.1" extension="RX"
              />
            </supplyDefinition>
          </definition>
          
          <!-- Create items for each panel in the query 
          -->
          <xsl:apply-templates select="panel/item">
            <!-- ordered by panel number -->
            <xsl:sort select="../panel_number" data-type="number"
              order="ascending"/>
          </xsl:apply-templates>
        </dataCriteriaSection>
      </component>

      <component>
        <populationCriteriaSection>
          <code code="57026-7" codeSystem="2.16.840.1.113883.6.1"/>
          <title>Population Criteria Section</title>
          <text>This section describes the Initial Patient Population,
            Numerator, Denominator, Denominator Exceptions, and Measure
            Populations</text>
          <!-- an I2B2 Query defines only the initial patient population,
            there are no numerators or denominators 
          -->
          <entry>
            <patientPopulationCriteria>
              <id root="{$docOID}" extension="IPP"/>
              <!-- The population criteria is generated by processing
                each panel in order.
              -->
              <xsl:apply-templates select="panel">
                <xsl:sort select="panel_number" data-type="number"
                  order="ascending"/>
              </xsl:apply-templates>
            </patientPopulationCriteria>
          </entry>
        </populationCriteriaSection>
      </component>
    </QualityMeasureDocument>
  </xsl:template>

  <!-- override XSL default rules for text() nodes -->
  <xsl:template match="text()"/>
  <xsl:template match="text()" mode="bytype"/>

  <!-- create a local variable name from a text string in
    the I2B2 ontology. 
    Inserts leading _ if it begins with a number
    Translates >=, <=, >, < and = into GE, LE, GT, LT and EQ respectively
    Replaces other special characters with _
    Adds a value to create a unique name.
  -->
  <xsl:template name="get-localVariable-name">
    <xsl:param name="string" select="normalize-space(item_name)"/>
    <xsl:variable name="unique" select="generate-id(item_name)"/>
    <xsl:variable name="name">
      <!-- Insert leading _ if starts with a number -->
      <xsl:if test="contains('1234567890',substring($string,1,1))">
        <xsl:text>_</xsl:text>
      </xsl:if>
      <!-- Translates >=, <=, >, < and = into GE, LE, GT, LT and EQ respectively -->
      <xsl:choose>
        <xsl:when test="contains($string,'&gt;=')">
          <xsl:value-of select="substring-before($string,'&gt;=')"/>
          <xsl:text>GE</xsl:text>
          <xsl:value-of select="substring-after($string,'&gt;=')"/>
        </xsl:when>
        <xsl:when test="contains($string,'&lt;=')">
          <xsl:value-of select="substring-before($string,'&lt;=')"/>
          <xsl:text>LE</xsl:text>
          <xsl:value-of select="substring-after($string,'&lt;=')"/>
        </xsl:when>
        <xsl:when test="contains($string,'&gt;')">
          <xsl:value-of select="substring-before($string,'&gt;')"/>
          <xsl:text>GT</xsl:text>
          <xsl:value-of select="substring-after($string,'&gt;')"/>
        </xsl:when>
        <xsl:when test="contains($string,'&lt;')">
          <xsl:value-of select="substring-before($string,'&lt;')"/>
          <xsl:text>LT</xsl:text>
          <xsl:value-of select="substring-after($string,'&lt;')"/>
        </xsl:when>
        <xsl:when test="contains($string,'=')">
          <xsl:value-of select="substring-before($string,'=')"/>
          <xsl:text>EQ</xsl:text>
          <xsl:value-of select="substring-after($string,'=')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <!-- Replaces other special characters with _ -->
    <xsl:value-of
      select="translate(translate(normalize-space($name),':()%',''),' ','_')"/>
    <!-- Ensures uniqueness of the variable name -->
    <xsl:text>_</xsl:text>
    <xsl:value-of select="$unique"/>
  </xsl:template>

  <!-- creates entries for each panel item in the dataCriteriaSection
    Each entry appears in the format (an example is shown below):    
    <entry>
      <localVariableName>_18-34_years_old_N100A3</localVariableName>
      <[criteriatype]Criteria>
        <id extension="[localvariablename]" root="[docOID]"/>
      <[code/value] codeSystemName="[e.g., LOINC]" codeSystem="[e.g., 2.16.840.1.113883.6.1]"
        displayName="[e.g., HGB_A1C]" code="[e.g., 4548-4]"/>
      <effectiveTime>
        <low value="20110101"/>
        <high value="20111231"/>
      </effectiveTime>
      <value xsi:type="IVL_PQ">
        <low inclusive="true" unit="%" value="9"/>
      </value>
        <definition>
          <observationReference moodCode="DEF">
            <id extension="Results"
              root="2.16.840.1.113883.3.1619.5148.1"/>
          </observationReference>
        </definition>
      </[criteriatype]Criteria>
    </entry>
    
    In addition to key-based code/value, value constraints, and effectiveTime constraints, also supports
     the strange template for substance administration and demographic code lookup, among other things
     documented at the top.
  -->
  <xsl:template match="panel/item">
    <entry>
      <!-- create the variable name, which is used in two places -->
      <xsl:variable name="name">
        <xsl:call-template name="get-localVariable-name"/>
      </xsl:variable>
      <!-- In here it is used the localVariableName -->
      <localVariableName>
        <xsl:value-of select="$name"/>
      </localVariableName>
      <!-- Extract various parts of the ontology from item_key -->
      <xsl:variable name="split_item_key" select="str:split(substring-after(normalize-space(item_key),'\\'),'\')"/>    
      <xsl:variable name="key" select="substring-after(normalize-space(item_key),concat('\',$split_item_key[2],'\'))"/>
      <xsl:variable name="type" select="$split_item_key[3]/text()"/>
      <xsl:variable name="subtype_split" select="str:split($split_item_key[4]/text(),'_')"/>
      <xsl:variable name="subtype" select="$subtype_split[1]/text()"/>
      <xsl:variable name="subtype_type" select="$subtype_split[2]/text()"/>
 
      <!-- extract the criteria mapping from the metaconfig -->
      <xsl:variable name="metacriteria"
        select="$metaconfig/mc:metaConfig/mc:criteriaMappings/mc:item[@i2b2_type=$type][1]"/>      
      <xsl:if test="not($metacriteria)"><xsl:message terminate="yes">
        Undefined criteria type <xsl:value-of select="$type"/></xsl:message></xsl:if>    
       
      <!-- Create the criteria -->
      <xsl:element name="{$metacriteria/@criteria}">
        <id root="{$docOID}" extension="{$name}"/>
          <xsl:if test="../panel_date_from|../panel_date_to">
            <!-- Date ranges in i2b2 are ignored for ages -->
            <xsl:if test="$metacriteria/@extension!='Demographics' or $subtype!='Age'">
              <effectiveTime>
                <xsl:apply-templates
                  select="../panel_date_from|../panel_date_to"/>
              </effectiveTime> 
            </xsl:if>
          </xsl:if>
          <!-- If it's a demographic criteria, look up the appropriate code (SNOMED by default) and insert a code element -->
          <xsl:if test="$metacriteria/@extension='Demographics'">
            <xsl:variable name="democode"
              select="$metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]"/> 
             <xsl:if test="not($democode)"><xsl:message terminate="yes">(501) Undefined demographic code <xsl:value-of select="$subtype"/>
             </xsl:message></xsl:if>
            <xsl:choose>
              <xsl:when test="$democode/@ignore">
                <!-- Special code to suppress the code output -->
              </xsl:when>
              <xsl:when test="$democode/@codeSystem">
                <code code="{$democode/@code}" codeSystem="{$democode/@codeSystem}" codeSystemName="{$democode/@codeSystemName}" 
                  displayName="{$democode/@displayName}"/>                
              </xsl:when>
              <xsl:otherwise>
                <code code="{$democode/@code}" codeSystem="2.16.840.1.113883.6.96" codeSystemName="SNOMED CT" 
                  displayName="{$democode/@displayName}"/>                  
              </xsl:otherwise>
            </xsl:choose>
   
          </xsl:if>
        
         <!-- Insert i2b2-extracted code or value (from key) -->
         <xsl:choose>
           <!-- Special case formatting for substance administration -->
           <xsl:when test="$metacriteria/@extension='Medications'">
             <participant typeCode="CSM">
               <roleParticipant classCode="THER">
                 <xsl:call-template name="get-concept"/>
               </roleParticipant>
             </participant>
           </xsl:when>
           <!-- Special case formatting for healthcare provider (also, see above, relies on basecode to properly code provider name vs. code -->
           <xsl:when test="$metacriteria/@special='qualifiedEntityParticipant'">
             <participant typeCode="PRF">
               <qualifiedEntityParticipant classCode="PHYS">
                 <xsl:call-template name="get-concept"/>
               </qualifiedEntityParticipant>
             </participant>
           </xsl:when>
           <xsl:otherwise><xsl:call-template name="get-concept"/></xsl:otherwise>
         </xsl:choose>
 
          <!-- Value/modifier -->
          <xsl:apply-templates select="constrain_by_value"/>
          <xsl:apply-templates select="constrain_by_modifier"/>
          
        
          <definition>
            <xsl:element name="{$metacriteria/@ref}">
              <xsl:attribute name="moodCode">DEF</xsl:attribute>
              <xsl:element name="id">
                <xsl:attribute name="root">2.16.840.1.113883.3.1619.5148.1</xsl:attribute>
                <xsl:attribute name="extension"><xsl:value-of select="$metacriteria/@extension"/></xsl:attribute>
              </xsl:element>
            </xsl:element>
          </definition>
      </xsl:element>
      
    </entry>
  </xsl:template>

  <!-- 
    Generate the population criteria from the panels.
    All panels are ANDed together 
    If the panel is inverted each item in it must be false
    Otherwise, at least one item in it must be true.
  -->
  <xsl:template match="panel">
    <xsl:choose>
      <xsl:when test="invert = '1'">
        <!-- If the panel is inverted each item in it must be false -->
        <precondition>
          <allFalse>
            <xsl:apply-templates select="item" mode="criteria"/>
          </allFalse>
        </precondition>
      </xsl:when>
      <!-- If there is more than one item in the panel,
        Or them together using atLeastOneTrue 
      -->
      <xsl:when test="count(item) &gt; 1">
        <precondition>
          <atLeastOneTrue>
            <xsl:apply-templates select="item" mode="criteria"/>
          </atLeastOneTrue>
        </precondition>
      </xsl:when>
      <!-- If there is only one item in the panel, no need to 
        Or it with other items. (JGK: I stuck this back in for now for ease of reverse translation)
      -->
      <xsl:otherwise>
        <precondition>
          <atLeastOneTrue>
            <xsl:apply-templates select="item" mode="criteria"/>
          </atLeastOneTrue>
        </precondition>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <!-- Now generate items in the Patient Population 
    Each item produces a precondition in the following form:
    <precondition>
    <observationReference>
    <id extension="name" root="docOID"/>
    </observationReference>
    </precondition>
    
    The name of the reference element is set based on the ontology item_key
    The id of the reference is based upon it's name.
  -->
  <xsl:template match="item" mode="criteria">
    <xsl:variable name="name">
      <xsl:call-template name="get-localVariable-name"/>
    </xsl:variable>
    <!-- Extract the i2b2 key type -->
    <xsl:variable name="split_item_key" select="str:split(substring-after(normalize-space(item_key),'\\'),'\')"/>    
    <xsl:variable name="type" select="$split_item_key[3]/text()"/>
    <!-- The name of the reference element is set based on the ontology item_key -->
    <xsl:variable name="elem-name" select="$metaconfig/mc:metaConfig/mc:criteriaMappings/mc:item[@i2b2_type=$type]/@ref"/>
    
    <precondition>
      <xsl:element name="{$elem-name}">
        <id root="{$docOID}" extension="{$name}">
          <xsl:attribute name="extension">
            <xsl:call-template name="get-localVariable-name"/>
          </xsl:attribute>
        </id>
      </xsl:element>
    </precondition>
  </xsl:template>

  <!-- Do the hard work of building the concept element (e.g., code, value, etc) from basecode, text modifier, etc. -->
  <xsl:template name="get-concept">
    <!-- Extract various parts of the ontology from item_key -->
    <xsl:variable name="item_key" select="concat(item_key,modifier_key)"/>
    <xsl:variable name="split_item_key" select="str:split(substring-after(normalize-space($item_key),'\\'),'\')"/>    
    <xsl:variable name="key" select="substring-after(normalize-space(item_key),concat('\',$split_item_key[2],'\'))"/>
    <xsl:variable name="type" select="$split_item_key[3]/text()"/>
    <xsl:variable name="subtype_split" select="str:split($split_item_key[4]/text(),'_')"/>
    <xsl:variable name="subtype" select="$subtype_split[1]/text()"/>
    <xsl:variable name="subtype_type" select="$subtype_split[2]/text()"/>
    
    <!-- extract the code from the I2B2 concept XML -->
    <xsl:variable name="code" select="substring-after(basecode,':')"/>    
    <xsl:variable name="code-system" select="substring-before(concat(substring-after(basecode[contains(.,'|')],'|'),basecode[not(contains(.,'|'))]),':')"/>
    
    <!-- extract the criteria mapping from the metaconfig -->
    <xsl:variable name="metacriteria"
      select="$metaconfig/mc:metaConfig/mc:criteriaMappings/mc:item[@i2b2_type=$type][1]"/>      
    <xsl:if test="not($metacriteria)"><xsl:message terminate="yes">
      Undefined criteria type <xsl:value-of select="$type"/></xsl:message></xsl:if> 
    
    <!-- Extract modifier criteria information from the metaconfig -->
    <xsl:variable name="metamodifier"
      select="$metaconfig/mc:metaConfig/mc:modifierMappings/mc:item[@i2b2_type=$type][@i2b2_subtype=$subtype][1]"/>
    <xsl:if test="not($metamodifier) and modifier_key"><xsl:message terminate="yes">
      Undefined modifier mapping for <xsl:value-of select="modifier_key"/></xsl:message></xsl:if> 
    
    <!-- Choose the correct tag name from either modifier info or criteria info -->
    <xsl:variable name="tag_name">
      <xsl:choose>
        <xsl:when test="$metamodifier">
          <xsl:value-of select="$metamodifier/@tag_name"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$metacriteria/@tag_name"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <!-- Extract the code and/or value from i2b2 element into $data-element -->
    <xsl:variable name="data-element">
      <xsl:choose>
        <!-- Demographic value of non CD or ST type -->
        <!-- TODO: Support age at visit and other IVL_PQ types -->
        <xsl:when test="$metacriteria/@extension='Demographics' and 
          $metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]/@dataType!='CD' and
          $metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]/@dataType!='ST'"> 
          <xsl:variable name="data_type" 
            select="$metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]/@dataType"/>
          <xsl:choose>
            <!-- If it's an age, map values -->
            <xsl:when test="$data_type='IVL_PQ'">
              <xsl:choose>
                <!-- New: Handle Age = X by basecode detection -->
                <xsl:when test="basecode and substring-after(basecode,':')">
                  <xsl:variable name="age"
                    select="substring-after(basecode,':')"/>
                  <value xsi:type="IVL_PQ">
                    <low value="{normalize-space($age)}" inclusive="true"
                      unit="a"/>
                    <high value="{normalize-space($age)+1}"
                      inclusive="false" unit="a"/>
                  </value>
                </xsl:when>
                <!-- Handle Age >= X -->
                <xsl:when test="contains(item_name,'gt;=')">
                  <xsl:variable name="age"
                    select="substring-before(substring-after(item_name,'= '),' ')"/>
                  <value xsi:type="IVL_PQ">
                    <low value="{normalize-space($age)}" inclusive="true"
                      unit="a"/>
                  </value>
                </xsl:when>
                <!-- Handle Age = X -->
                <xsl:when test="contains(item_name,'=')">
                  <xsl:variable name="age"
                    select="substring-before(substring-after(item_name,'= '),' ')"/>
                  <value xsi:type="IVL_PQ">
                    <low value="{normalize-space($age)}" inclusive="true"
                      unit="a"/>
                    <high value="{normalize-space($age)+1}"
                      inclusive="false" unit="a"/>
                  </value>
                </xsl:when>
                <!-- process age ranges -->
                <xsl:when test="contains(item_name,'-')">
                  <xsl:variable name="agelow"
                    select="substring-before(item_name,'-')"/>
                  <xsl:variable name="agehigh"
                    select="substring-before(substring-after(item_name,'-'),' ')"/>
                  <value xsi:type="IVL_PQ">
                    <low value="{normalize-space($agelow)}" inclusive="true"
                      unit="a"/>
                    <high value="{normalize-space($agehigh)+1}"
                      inclusive="false" unit="a"/>
                  </value>
                </xsl:when>
                <xsl:otherwise>
                  <!-- don't recognize age format -->
                  <xsl:message terminate="yes">(400) Cannot Parse Age Range for
                    <xsl:value-of select="$item_key"/> to HQMF Demographics
                  </xsl:message>
                </xsl:otherwise>
              </xsl:choose>               
            </xsl:when>      
            <xsl:when test="$data_type='IVL_TS'">
              <!-- This will be handled by the standard constrain by value template.  -->
            </xsl:when>
            <!-- TODO: Zip Codes mapped to State, City or Postal Code  -->
            <xsl:when test="$data_type='AD'">
              <value xsi:type="AD">
                <city/>
                <state/>
                <zipCode/>
              </value>
            </xsl:when> 
            <xsl:otherwise>
              <xsl:message terminate="yes">(501) Unsupported demographic datatype: 
                <xsl:value-of select="$metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]/@dataType"/>
              </xsl:message>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <!-- Text element with subtype_type=text or demographic criteria with data type ST -->
        <xsl:when test="$subtype_type='text' or ($metacriteria/@extension='Demographics' and 
          $metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]/@dataType='ST')">
          <xsl:variable name="text_value" select="constrain_by_value[value_type='TEXT']"/>
          <xsl:if test="count($text_value)!=1"><xsl:message terminate="yes">(404) Wrong number of text value constraints
            <xsl:value-of select="count($text_value)"/></xsl:message></xsl:if>
          <xsl:element name="{$tag_name}">
            <xsl:attribute name="xsi:type">ST</xsl:attribute>
            <xsl:value-of select="$text_value[1]/value_constraint/text()"/>
            <!-- Illegal for ST types <xsl:attribute name="displayName"><xsl:value-of select="item_name"/></xsl:attribute> -->
          </xsl:element>
        </xsl:when>
        <!-- Coded element without a coding system when subtype_type=code or value and basecode=CEDD -->
        <xsl:when test="($subtype_type='code' or $subtype_type='value') and $code-system='CEDD'">
          <xsl:variable name="text_value" select="constrain_by_value[value_type='TEXT']"/>
          <xsl:if test="count($text_value)!=1"><xsl:message terminate="yes">(404) Wrong number of text value constraints
            <xsl:value-of select="count($text_value)"/></xsl:message></xsl:if>
          <xsl:element name="{$tag_name}">
            <xsl:attribute name="xsi:type">CD</xsl:attribute>
            <xsl:attribute name="code"><xsl:value-of select="$text_value[1]/value_constraint/text()"/></xsl:attribute>
            <xsl:attribute name="displayName"><xsl:value-of select="item_name"/></xsl:attribute>
            <xsl:attribute name="nullFlavor">UNK</xsl:attribute>
          </xsl:element>
        </xsl:when> 
        <!-- Coded element when subtype_type=code basecode=OID -->
        <xsl:when test="($subtype_type='code' or $subtype_type='value') and $code-system='OID'">
          <xsl:element name="{$tag_name}">
            <xsl:attribute name="xsi:type">CD</xsl:attribute>
            <xsl:attribute name="valueSet"><xsl:value-of select="$code"/></xsl:attribute>
            <xsl:attribute name="displayName"><xsl:value-of select="item_name"/></xsl:attribute>
          </xsl:element>
        </xsl:when>         
        <!-- SPECIAL CASE: provider id (basecode CEDD:HealthcareProviderID) -->
        <xsl:when test="basecode='CEDD:HealthcareProviderID'">
          <xsl:variable name="text_value" select="constrain_by_value[value_type='TEXT']"/>
          <xsl:if test="count($text_value)!=1"><xsl:message terminate="yes">(404) Wrong number of text value constraints
            <xsl:value-of select="count($text_value)"/></xsl:message></xsl:if>
          <xsl:element name="id">
            <xsl:attribute name="root">2.16.840.1.113883.19.5</xsl:attribute>
            <xsl:attribute name="extension"><xsl:value-of select="$text_value[1]/value_constraint/text()"/></xsl:attribute>
          </xsl:element>            
        </xsl:when>
        <!-- SPECIAL CASE: provider name (basecode CEDD:HealthcareProviderName) -->
        <xsl:when test="basecode='CEDD:HealthcareProviderName'">
          <xsl:variable name="text_value" select="constrain_by_value[value_type='TEXT']"/>
          <xsl:if test="count($text_value)!=1"><xsl:message terminate="yes">(404) Wrong number of text value constraints
            <xsl:value-of select="count($text_value)"/></xsl:message></xsl:if>
          <xsl:element name="name"><xsl:value-of select="$text_value[1]/value_constraint/text()"/></xsl:element>            
        </xsl:when>
        <!-- SPECIAL CASE: provider organization name (basecode CEDD:HealthcareProviderOrganizationName or CEDD:EncounterOrganizationName) -->
        <xsl:when test="basecode='CEDD:HealthcareProviderOrganizationName' or basecode='CEDD:EncounterOrganizationName'">
          <xsl:variable name="text_value" select="constrain_by_value[value_type='TEXT']"/>
          <xsl:if test="count($text_value)!=1"><xsl:message terminate="yes">(404) Wrong number of text value constraints
            <xsl:value-of select="count($text_value)"/></xsl:message></xsl:if>
          <xsl:element name="qualifiedOrganization">
            <xsl:element name="name"><xsl:value-of select="$text_value[1]/value_constraint/text()"/></xsl:element> 
          </xsl:element>                 
        </xsl:when>
        <!-- CD Coded element -->
        <xsl:when test="basecode">
          <!-- Code system -->
          <xsl:variable name="metacodesys-rtf">
            <xsl:for-each select="$metaconfig/mc:metaConfig/mc:codeSystems/mc:item[@basecode=$code-system]">
              <xsl:choose>
                <xsl:when test="not(attribute::test)">
                  <xsl:copy-of select="current()"/>
                </xsl:when>            
                <xsl:when test="attribute::test">
                  <xsl:if test="dyn:evaluate(attribute::test)">
                    <xsl:copy-of select="current()"/>
                  </xsl:if>
                </xsl:when>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <xsl:variable name="metacodesys" select="xalan:nodeset($metacodesys-rtf)"/>
          <xsl:if test="not($metacodesys/mc:item)"><xsl:message terminate="yes">
            Undefined code system <xsl:value-of select="$code-system"/>
          </xsl:message></xsl:if>   
          <xsl:element name="{$tag_name}">
            <xsl:attribute name="xsi:type">CD</xsl:attribute>
            <xsl:attribute name="code"><xsl:value-of select="$code"/></xsl:attribute>
            <xsl:attribute name="displayName"><xsl:value-of select="item_name"/></xsl:attribute>
            <xsl:attribute name="codeSystem"><xsl:value-of select="$metacodesys/mc:item/@codeSystem"/></xsl:attribute>
            <xsl:attribute name="codeSystemName"><xsl:value-of select="$metacodesys/mc:item/@codeSystemName"/></xsl:attribute>
          </xsl:element>
        </xsl:when>
        <!-- Keith's code: otherswise, there is no Root concept, so we change this (For now)
          to the I2B2 concept, and use the OID below to represent the I2B2 ontology
          Because the ontology paths in I2B2 contain spaces, we change them to + signs
          because HL7 doesn't like space characters in codes.
          Ideally, this would be changed into a valueSet that could be accessed
          from the I2B2 ontology cell.  I don't know if I2B2 ontology entries have
          a numeric identifier that could be used in the construction of value set
          identifiers, but if there was such a value, this could be done rather easily.
        -->
        <xsl:otherwise>
          <xsl:message terminate="yes">(404) <xsl:value-of select="$item_key"/> was not found or it had no basecode (<xsl:value-of select="basecode"/>).</xsl:message>
          <xsl:comment><xsl:value-of select="item_key"/> was not found or it had no basecode.</xsl:comment>
          <xsl:element name="{$tag_name}">
            <xsl:attribute name="xsi:type">CD</xsl:attribute>
            <xsl:attribute name="code"><xsl:value-of select="translate(normalize-space(item_key),' ','+')"/></xsl:attribute>
            <xsl:attribute name="codeSystem">2.16.840.1.113883.3.1619.5148.19.1</xsl:attribute>
          </xsl:element>
        </xsl:otherwise>
      </xsl:choose>  
    </xsl:variable>
    
    <xsl:copy-of select="$data-element"/>    
  </xsl:template>

  <!-- generate <low> and <high> elements in 
    <effectiveTime> based on panel_date values 
    -->
  <xsl:template match="panel_date_from|panel_date_to">
    <!-- set the element name to low if from panel_date_from, otherwise use <high> -->
    <xsl:variable name="name">
      <xsl:choose>
        <xsl:when test="self::panel_date_from">low</xsl:when>
        <xsl:otherwise>high</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:element name="{$name}">
      <xsl:attribute name="value">
        <!-- Put the date in ISO (HL7) format -->
        <xsl:value-of
          select="concat(substring(.,1,4),substring(.,6,2),substring(.,9,2))"
        />
      </xsl:attribute>
    </xsl:element>
  </xsl:template>

  <!-- deal with modifier constraints -->
  <xsl:template match="constrain_by_modifier">

    <!-- Extract various parts of the ontology from modifier_key -->
    <xsl:variable name="split_item_key" select="str:split(substring-after(normalize-space(modifier_key),'\\'),'\')"/>    
    <xsl:variable name="type" select="$split_item_key[3]/text()"/>
    <xsl:variable name="subtype_split" select="str:split($split_item_key[4]/text(),'_')"/>
    <xsl:variable name="subtype" select="$subtype_split[1]/text()"/>
    
    <!-- Extract modifier criteria information from the metaconfig -->
    <xsl:variable name="metamodifier"
      select="$metaconfig/mc:metaConfig/mc:modifierMappings/mc:item[@i2b2_type=$type][@i2b2_subtype=$subtype][1]"/>
    <xsl:if test="not($metamodifier) and modifier_key"><xsl:message terminate="yes">
      Undefined modifier mapping for <xsl:value-of select="modifier_key"/></xsl:message></xsl:if> 
    
    <xsl:choose>
      <!-- Special case formatting for healthcare provider (also, see above, relies on basecode to properly code provider name vs. code -->
      <xsl:when test="$metamodifier/@special='qualifiedEntityParticipant'">
        <participant typeCode="PRF">
          <qualifiedEntityParticipant classCode="PHYS">
            <xsl:call-template name="get-concept"/>
          </qualifiedEntityParticipant>
        </participant>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="get-concept"/>        
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>
  
  <!-- deal with constrain_by_value on lab results and in demographics for dates in CEDD -->
  <xsl:template
    match="constrain_by_value[value_type = 'NUMBER']">
    <xsl:variable name="unit" select="value_unit_of_measure"/>
    <!-- Use IVL_PQ for NUMBERs -->
    <value xsi:type="IVL_PQ">
      <xsl:choose>
        <xsl:when test="value_operator = 'EQ'">
          <xsl:attribute name="xsi:type">PQ</xsl:attribute>
          <xsl:if test="$unit!=''">
            <xsl:attribute name="unit">
              <xsl:value-of select="$unit"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:attribute name="value">
            <xsl:value-of select="value_constraint"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="value_operator = 'GT'">
          <low value="{value_constraint}" unit="{$unit}"
            inclusive="false"/>
        </xsl:when>
        <xsl:when test="value_operator = 'GE'">
          <low value="{value_constraint}" unit="{$unit}"
            inclusive="true"/>
        </xsl:when>
        <xsl:when test="value_operator = 'LT'">
          <high value="{value_constraint}" unit="{$unit}"
            inclusive="false"/>
        </xsl:when>
        <xsl:when test="value_operator = 'LE'">
          <high value="{value_constraint}" unit="{$unit}"
            inclusive="true"/>
        </xsl:when>
        <xsl:when test="value_operator = &quot;BETWEEN&quot;">
          <low
            value="{substring-before(value_constraint,' and ')}"
            unit="{$unit}"/>
          <high
            value="{substring-after(value_constraint,' and ')}"
            unit="{$unit}"/>
        </xsl:when>
      </xsl:choose>
    </value>
  </xsl:template>
  
  <!-- Handle FLAGs with a degree SNOMED code, not sure if this is CCDA preferred.
       Also, it is hard-coded and not in the metaconfig which is perhaps non-optimal, though it doesn't 
       change from i2b2 installation to installation. -->
  <xsl:template
    match="constrain_by_value[value_type = 'FLAG']">
    <value xsi:type="CD" codeSystem="2.16.840.1.113883.6.96" codeSystemName="SNOMED">
      <xsl:choose>
        <xsl:when test="value_constraint='L'">
          <xsl:attribute name="code">62482003</xsl:attribute>
          <xsl:attribute name="displayName">Low</xsl:attribute>
        </xsl:when>
        <xsl:when test="value_constraint='H'">
          <xsl:attribute name="code">75540009</xsl:attribute>
          <xsl:attribute name="displayName">High</xsl:attribute>
        </xsl:when>
        <xsl:when test="value_constraint='N'">
          <xsl:attribute name="code">260394003</xsl:attribute>
          <xsl:attribute name="displayName">Normal Limits</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:message terminate="yes">(400) Cannot Parse Flag type of <xsl:value-of
            select="value_constraint"/>  </xsl:message>          
        </xsl:otherwise>
      </xsl:choose>
    </value>

  </xsl:template>
  
  <!-- Deal with non-numeric value tests by reporting an error -->
  <xsl:template
    match="constrain_by_value[value_type != 'NUMBER' and value_type != 'TEXT' and value_type != 'FLAG']">
    <xsl:message terminate="yes">(400) Cannot Parse Value for <xsl:value-of
        select="value_type"/> types </xsl:message>
  </xsl:template>
  
</xsl:stylesheet>

<!-- _________________THE FOLLOWING ARE KEITH BOONE'S NOTES ON THE DIFFERENT TEMPLATE TYPES_______________________________ -->
<!-- This template handls items coming from the Demographics ontology in I2B2
  It generates an observationCriteria element, 
  gives the item the appropriate id generated by the OID unique to this document and the identifier name generated
  by the panel/item template,    
  Maps the ontology to the appropriate SNOMED Code (or barfs if it doesn't 
  recognized the demographic)
  Maps panel dates to effectiveTime, 
  and sets value to the appropriate type and content
  
  An example is given below:
  <observationCriteria>
  <id extension="_18-34_years_old_N100A3"
  root="2.16.840.1.113883.3.1619.5148.3.20120214.165226983"/>
  <code displayName="Current Chronological Age"
  codeSystem="2.16.840.1.113883.6.96" code="424144002"/>
  <effectiveTime>
  <low value="20110101"/>
  <high value="20111231"/>
  </effectiveTime>
  <value xsi:type="IVL_PQ">
  <low unit="a" inclusive="true" value="18"/>
  <high unit="a" inclusive="false" value="35"/>
  </value>
  <definition>
  <observationReference moodCode="DEF">
  <id extension="Demographics"
  root="2.16.840.1.113883.3.1619.5148.1"/>
  </observationReference>
  </definition>
  </observationCriteria>
-->
<!-- Handle diagnoses 
  This template handls items coming from the Diagnoses ontology in I2B2
  It generates an observationCriteria element, 
  gives the item the appropriate id generated by the OID unique to this document and the identifier name generated
  by the panel/item template,
  Maps panel dates to effectiveTime, 
  Locates the code from the ontology and puts it in value
  
  An example is given below:
  <observationCriteria>
  <id extension="Diabetes_mellitus_N1011B"
  root="2.16.840.1.113883.3.1619.5148.3.20120214.165226983"/>
  <effectiveTime>
  <low value="20100101"/>
  <high value="20111231"/>
  </effectiveTime>
  <value codeSystemName="ICD-9-CM"
  codeSystem="2.16.840.1.113883.6.103"
  displayName="Diabetes Melitus"
  code="250" xsi:type="CD"/>
  <definition>
  <observationReference moodCode="DEF">
  <id extension="Problems"
  root="2.16.840.1.113883.3.1619.5148.1"/>
  </observationReference>
  </definition>
  </observationCriteria>
  
-->
<!-- Handle Lab Results
  This template handls items coming from the Lab Results ontology in I2B2
  It generates an observationCriteria element, 
  gives the item the appropriate id generated by the OID unique to this document and the identifier name generated
  by the panel/item template,
  Locates the code from the ontology and puts it in code
  Maps panel dates to effectiveTime, 
  Generates the value test.
  
  An example is given below:
  
  <observationCriteria>
  <id extension="HGB_A1C_LOINC4548-4_GE_9_N1014B"
  root="2.16.840.1.113883.3.1619.5148.3.20120214.165226983"/>
  <code codeSystemName="LOINC"
  codeSystem="2.16.840.1.113883.6.1"
  displayName="HGB_A1C"
  code="4548-4"/>
  <effectiveTime>
  <low value="20110101"/>
  <high value="20111231"/>
  </effectiveTime>
  <value xsi:type="IVL_PQ">
  <low inclusive="true" unit="%" value="9"/>
  </value>
  <definition>
  <observationReference moodCode="DEF">
  <id extension="Results"
  root="2.16.840.1.113883.3.1619.5148.1"/>
  </observationReference>
  </definition>
  </observationCriteria>
  
-->
<!-- Handle medications
  This template handels items coming from the medication ontology in SHRINE or i2b2demo
  It generates an substanceAdministrationCriteria element, 
  gives the item the appropriate id generated by the OID unique to this document and the identifier name generated
  by the panel/item template,
  Maps panel dates to effectiveTime, 
  Locates the code from the ontology and puts it in the code of the participant (a consumable substance)
  Does not do anything with temporallyRelatedInformation, which might mean effectiveTime is probably not the way to do times?
  
  An example is given below from the repository (no temporally related information is included):
  <substanceAdministrationCriteria moodCode="INT">
  <id root="0" extension="DiabetesMedIntended"/>
  <participant typeCode="CSM">
  <roleParticipant classCode="THER">
  <code valueSet="2.16.840.1.113883.3.464.1.94"/>
  </roleParticipant>
  </participant>
  <definition>
  <substanceAdministrationReference moodCode="DEF">
  <id root="0" extension="Medication"/>
  </substanceAdministrationReference>
  </definition>
  <temporallyRelatedInformation typeCode="SAS">
  <pauseQuantity value="-1" unit="a"/>
  <observationReference>
  <id root="0" extension="MeasurePeriod"/>
  </observationReference>            
  </temporallyRelatedInformation>
  </substanceAdministrationCriteria>
  
-->
<!-- Handle procedures - very similar to diagnoses
  This template handls items coming from the Procedures ontology in I2B2
  It generates an observationCriteria element, 
  gives the item the appropriate id generated by the OID unique to this document and the identifier name generated
  by the panel/item template,
  Maps panel dates to effectiveTime, 
  Locates the code from the ontology and puts it in *code*.
  TBD: how to indicate procedure was performed?
  
  This example comes from a sample in the repository (this template does nothing with the excerpt element):
  <observationCriteria>
  <id root="0" extension="Eye Exam"/>
  <code valueSet="2.16.840.1.113883.3.464.0001.241"/>
  <definition>
  <observationReference moodCode="DEF">
  <id root="0" extension="Procedure"/>
  </observationReference>
  </definition>
  <excerpt>
  <subsetCode code="RECENT"/>
  <observationCriteria>
  <id extension="0" root="Procedure, Performed: eye exam"/>
  (Review what the best datatype would be to measure whether an exam has been performed)
  <value xsi:type="IVL_PQ">
  <high value="1" unit="a" inclusive="true"/>
  </value>
  </observationCriteria>
  </excerpt>
  </observationCriteria>
-->
<!-- TBD: handle Encounters
  None of these really work. Age at visit should not be the same snomed code as age. It probably should be an excerpt in a visit type?
  We need to define what OIDs we're using for visit type. There is no guidance on coding length of stay.
  Visit type should be an oid, either hedis or i2b2
-->