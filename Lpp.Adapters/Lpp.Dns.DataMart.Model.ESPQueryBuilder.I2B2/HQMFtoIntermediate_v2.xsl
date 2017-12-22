<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns="urn:hl7-org:v3" 
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:xalan="http://xml.apache.org/xalan"
  xmlns:v3="urn:hl7-org:v3"
  xmlns:rvs="urn:ihe:iti:svs:2008">  <!-- applied to refer to unprefixed default namespace defined here ... <RetrieveValueSetResponse xmlns="urn:ihe:iti:svs:2008"> -db -->
  
  <xsl:output method="xml" standalone="yes" omit-xml-declaration="no" indent="yes" xalan:indent-amount="2"/>
  <!-- 
    Process an HQMF document and extract the essentials for
    translation to executable code 
    
    Changelog:
        Keith W. Boone - 5/2012 - initial version
        Jeff Klann - 6/2012 - fixes to mesh with my updated schema
          Note that this is not thoroughly tested - it has been tested on the i2b2 HQMF version of NQF59
           and on my larger composite query. 
           There are places this translator probably still does not produce valid ihqmf. Needs work.
        Dan Brown (Drajer LLC - Dragon's Company) - 7/2012 - Added 8 new templates and re-coded 7 existing templates to produce
          a valid output against valid input files (NQF59, ICD9, ReportableDiseaseQuery tested so far).
          Fixed all errors in output from populationCriteria, stratifierCriteria, and MedicationCriteria - including
          errors in children/attributes (measurePeriodTimeReference, dataCriteriaReference, medicationState, etc.)
        Dan Brown (Drajer LLC - Dragon's Company) - 7/2012 - Added code (3 new templates, adjusted 2 others)   
          to check for valueSet ids and match them within DemographicsCriteria and EncounterCriteria.
        Dan Brown (Drajer LLC - Dragon's Company) - 8/2012 - Expanded valueSet capture and display to include 
        SubstanceAdministrationCriteria (1x MedicationCriteria, 1x ImmunizationCriteria) and SupplyCriteria (1x MedicationCriteria).
        Solidified the valueSet collection process into one template/param that works for everything to increase performance.  
  -->
  <xsl:template match="/v3:QualityMeasureDocument">
    <ihqmf xmlns="urn:hl7-org:v3"  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xsi:schemaLocation="urn:hl7-org:v3 ../schemas/HQMFIntermediateTranslator.xsd">  
      <!-- Pull Title and Description from appropriate parts of the document -->
      <title>
        <xsl:value-of select="v3:title"/>
      </title>
      <description>
        <xsl:value-of
          select="v3:component/v3:measureDescriptionSection/v3:text"/>
      </description>

      <!-- Translate the measurePeriod -->
      <!-- TODO-JGK: This seems like a nice place to instantiate widths into high/low -->
      <measurePeriod>
        <xsl:copy-of select="v3:controlVariable/v3:measurePeriod/v3:id"/>
        <xsl:copy-of select="v3:controlVariable/v3:measurePeriod/v3:value"/>     
      </measurePeriod>

      <!-- Insert criteria -->
      <xsl:apply-templates
        select="v3:component/v3:dataCriteriaSection/v3:entry/v3:*[v3:definition]"/>

      <!-- Then pull population and stratifier criteria together -->
      <xsl:apply-templates
        select="//v3:patientPopulationCriteria|//v3:numeratorCriteria|
        //v3:denominatorCriteria|//v3:denominatorExceptionCriteria"
      />
      <!-- 
        TBD: This should be exactly like population criteria, 
        but include only stratifiers 
      -->
      <xsl:apply-templates
        select="//v3:stratifierCriteria"
      />
    </ihqmf>
  </xsl:template>

  <!-- Turn off default text matching rule -->
  <xsl:template match="text()"/>

  <!-- Perform the basic filling in for each data criteria
    Caller must pass the source of the primary code in the primaryCode
    parameter.
    -->
  <xsl:template name="dataCriteria">
    <xsl:param name="primaryCode"/>
    <!-- id and localVariableName are copies -->
    <xsl:copy-of select="v3:id"/>
    <xsl:copy-of select="../v3:localVariableName"/>
    
    <!-- 
      If there was a value set in primaryCode, copy it
      TBD: This should be in same sequence with codedValue 
      and freeTextValue
    -->
    <xsl:if test="$primaryCode/@valueSet">
      <code valueSet="{$primaryCode/@valueSet}"/>
    </xsl:if>

    <!-- effectiveTime is a copy -->
    <xsl:copy-of select="v3:effectiveTime"/>

    <!-- If there was a code set in primaryCode, copy it -->
    <xsl:if test="$primaryCode/@code">
      <code code="{$primaryCode/@code}"
        codeSystem="{$primaryCode/@codeSystem}"
        displayName="{$primaryCode/@displayName}"/> <!-- adds displayName attribute to <code> output -db -->
    </xsl:if>

    <!-- If there was a free text value, get it -->
    <xsl:if test="$primaryCode/v3:originalText/text()">
      <freeTextValue>
        <xsl:value-of select="$primaryCode/v3:orginalText/text()"/>
      </freeTextValue>
    </xsl:if>
    <xsl:apply-templates select="v3:excerpt"/>
  <!-- Moved to more specific locations for correct ordering required for validation -db -->    
  <!--  <xsl:apply-templates select="v3:temporallyRelatedInformation"/>  -->
  </xsl:template>

  <!-- Handle excerpts as filters -->
  <xsl:template match="v3:excerpt">
    <filterCriteria>
      <xsl:if test="v3:subsetCode">
        <filterCode>
          <xsl:value-of select="v3:subsetCode/@code"/>
        </filterCode>
      </xsl:if>
      <!-- if there was a repeatNumber, include it -->
      <xsl:copy-of select="v3:*/v3:repeatNumber"/>
      
      <!-- TBD: Need to discuss criteria inside excerpt in 
          intermediate schema
      -->
      <!-- This select is why the criteria template predicates
        use ancestor-or-self::*/v3:definition, because the definition
        isn't needed in excerpted child criteria (because the parent 
        provides it). TODO: Introduced bug to fix another - got rid of ancestor or self
        -->
      <xsl:apply-templates select="*"/>
    </filterCriteria>
  </xsl:template>

  <!-- Map temporallyRelatedInformation to timeRelationship -->  
  <xsl:template match="v3:temporallyRelatedInformation">
    <timeRelationship>
      <timeRelationshipCode>
        <xsl:value-of select="@typeCode"/>
      </timeRelationshipCode>
      <timeQuantity value="{v3:pauseQuantity/@value}"
        unit="{v3:pauseQuantity/@unit}"/>
      <measurePeriodTimeReference>
        <!-- <xsl:copy-of select="v3:*/v3:id"/> -->
        <!-- Changed to "*/v3:id/@*" instead of "v3:*/v3:id" as it was copying the ID element
        and elements are not allowed in measurePeriodTimeReference as per the schema  -db -->
        <xsl:copy-of select="*/v3:id/@*"/>  <!-- this works as well "v3:*/v3:id/@*" -db -->
        
<!--         If there is no event reference, use measurePeriod ID 
          TBD: Discuss whether this works for implementers, and ensure
          that <measurePeriod></measurePeriod> and other events have similar representation
          of time components with respect to time to facilitate 
          implementation.      -->
<!--         <xsl:if test="not(v3:*/v3:id)">
     <xsl:copy-of select="//v3:measurePeriod/v3:id"/>
        </xsl:if> -->
      </measurePeriodTimeReference>
    </timeRelationship>
  </xsl:template>

  <!-- Demographics -->
  <xsl:template
    match="v3:observationCriteria[
      self::*/v3:definition/v3:*/v3:id/@extension = 'Demographics']">

    <DemographicsCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:code"/>
      </xsl:call-template>
      
      <!-- If there's a valueSet (then there's a code) within observationCriteria, then we go forward
      Note: Only sections with valueSets must be processed   -db -->
      <xsl:if test="self::*/v3:value/@valueSet">              
        <xsl:message>code element with @valueSet for DEMOGRAPHICS</xsl:message>
        <!-- Call collectValues template implicitly through template match and
        use the cur_valueSetID parameter to capture id and pass it through -db -->
        <xsl:apply-templates select="//v3:valueSet" mode="allValues">
          <xsl:with-param name="cur_valueSetID" select="self::*/v3:value/@valueSet"/>
        </xsl:apply-templates>
      </xsl:if>
      
      <!-- Needs to occur below valueSet - if relevant, will be output within new <timeRelationship> -db -->
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/>       

      <!--  Needs to occur below valueSet, or below <timeRelationship> if there is one - 
      if there's a value, copy the element -db -->
      <xsl:if test="v3:value"> 
        <xsl:copy-of select="v3:value"/>
      </xsl:if>      
    </DemographicsCriteria>
    
  </xsl:template>

  <!-- Problems -->
  <xsl:template
    match="v3:observationCriteria[
      self::*/v3:definition/v3:*/v3:id/@extension = 'Problems']">
    <ProblemCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:value"/>
      </xsl:call-template>
     <xsl:apply-templates select="v3:temporallyRelatedInformation"/>       
    </ProblemCriteria>
  </xsl:template>

  <!-- Allergies -->
  <xsl:template
    match="v3:observationCriteria[ 
      self::*/v3:definition/v3:*/v3:id/@extension = 'Allergies']">
    <AllergyCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:value"/>
      </xsl:call-template>
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/>
    </AllergyCriteria>
  </xsl:template>

  <!-- Encounters -->
  <xsl:template
    match="v3:encounterCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension = 'Encounters']">
    
    <EncounterCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:code"/>
      </xsl:call-template>
      <xsl:if test="v3:value">
        <xsl:copy-of select="v3:value"/>
      </xsl:if>

      <!-- If there's a valueSet (then there's a code) within encounterCriteria, then we go forward
      Note: Only sections with valueSets must be processed   -db -->      
      <xsl:if test="self::*/v3:code/@valueSet">  <!-- IF THE INPUT FILE IS WRONG (supposed to be a value element) THEN THIS IS THE CORRECT CODE <xsl:if test="self::*/v3:value/@valueSet"> -db-->    
        <xsl:message>code element with @valueSet for ENCOUNTER</xsl:message>
        <!-- Call collectValues template implicitly through template match and
        use the cur_valueSetID parameter to capture id and pass it through -db -->
        <xsl:apply-templates select="//v3:valueSet" mode="allValues">
          <xsl:with-param name="cur_valueSetID" select="self::*/v3:code/@valueSet"/> <!-- IF THE INPUT FILE IS WRONG (supposed to be a value element) THEN THE FOLLOWING SELECT IS THE CORRECT CODE select="self::*/v3:value/@valueSet -db -->
        </xsl:apply-templates>
      </xsl:if>      

      <!-- Needs to occur below valueSet - if relevant, will be output within new <timeRelationship> -db -->
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/>
      
      <!-- Note, this fix may need to be applied for all elements within the dataCriteria template (it may have had wrong xPath originally) -db -->    
      <!-- If there's a text element (freeTextCode in output) copy it and surround it with <freeTextCode> -db -->    
      <xsl:if test="v3:text/@description">
        <freeTextCode>
          <xsl:value-of select="v3:text/@description"/>
        </freeTextCode>
      </xsl:if>             
    </EncounterCriteria>    
  </xsl:template>

  <!-- Performs multiple checks to ensure there is a match between the ValueSet @id 
  (carried by a parameter) and the current (definition) valueSet -db -->
  <xsl:template
    name="collectValues"
    match="v3:valueSet"
    mode="allValues">
    
    <!-- Define parameter used to keep track of current @valueSet # within <value> -db -->   
    <xsl:param name="cur_valueSetID"/>    
    <!-- <xsl:message>GOT valueSet</xsl:message> -->
    
    <!-- 1: If there's a root attribute within the id element AND
    the id is EQUAL TO the current id from the sent parameter... -db -->
    <xsl:if test="v3:id/@root = $cur_valueSetID"> 
      <!-- 2: Ensure there is a <reference> element WHICH starts with a web address AND contains the current id -db -->
      <xsl:if test="v3:text/v3:reference[starts-with(@value, 'https://') and contains(@value, $cur_valueSetID)]"> <!-- if this node value is irrelevant and all that is needed is the element itself apply v3:text/v3:reference instead -db -->
        <!-- 3:  Check if RetrieveValueSetResponse with a ValueSet @id 
        is EQUAL TO the current id from the sent parameter -db -->
        <xsl:if test="v3:text/rvs:RetrieveValueSetResponse/rvs:ValueSet/@id = $cur_valueSetID">
          <!-- Collects all Concept codes associated with the id -db -->
          <xsl:call-template name="collectValuesOperation"/>
        </xsl:if>
      </xsl:if>
    </xsl:if>    
  </xsl:template>
    
  <!-- Displays the current <id> and *Lists all of the Concept elements along with their code attributes -db -->  
  <xsl:template
    name="collectValuesOperation">
      <xsl:message>COLLECT VALUES</xsl:message>            
	    <valueSet>
	      <!--  Copy the id code - the current input id can be used because it was proven to be a match -db -->
          <xsl:copy-of select="v3:id"/>
	      <!-- Lists all of the available code attributes and surrounds them with the <concept> element -db -->		  		  
		  <xsl:for-each select="v3:text/rvs:RetrieveValueSetResponse/rvs:ValueSet/rvs:ConceptList/rvs:Concept">
		    <concept>
		      <xsl:copy-of select="@code"></xsl:copy-of>
		    </concept>
		  </xsl:for-each>	          
        </valueSet>            
  </xsl:template>
  
  <!-- Procedures -->
  <xsl:template
    match="v3:procedureCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension = 'Procedures']">
    <ProcedureCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:code"/>
      </xsl:call-template>
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/>
      <!-- TBD: Not sure what value to put here -->
      <xsl:if test="false()">
        <procedureStatus code="{@moodCode}"/>
      </xsl:if>
      <xsl:if test="v3:targetSiteCode/@code">
        <procedureBodySite code="{v3:targetSiteCode/@code}"/>
      </xsl:if>
    </ProcedureCriteria>
  </xsl:template>

  <!-- Medications -->
  <xsl:template
    match="v3:substanceAdministrationCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension = 
    'Medications']">
    <MedicationCriteria>
      <xsl:if
        test="not(v3:participant/v3:roleParticipant[@classCode='THER']/v3:code)">
        <!-- TBD: Does it make sense to have a medication criteria without
          a code? -->
        <xsl:message terminate="no"> No Mzedication code found for
          medication criteria //substanceAdministrationCriteria[
            <xsl:value-of
            select="1+ count(preceding::v3:substanceAdministrationCriteria)"
          /> ] </xsl:message>
      </xsl:if>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode"
          select="v3:participant/v3:roleParticipant[@classCode='THER']/v3:code"
        />
      </xsl:call-template>
      
      <!-- If there's a valueSet within substanceAdministrationCriteria, then we go forward
      Note: Only sections with valueSets must be processed   -db -->      
      <xsl:if test="self::*/v3:participant/v3:roleParticipant/v3:code/@valueSet"> 
        <xsl:message>code element with @valueSet for SUBSTANCE</xsl:message>
        <!-- Call collectSubstanceValues template implicitly through template match and
        use the cur_valueSetID_Subs parameter to capture id and pass it through -db -->
        <xsl:apply-templates select="//v3:valueSet" mode="allValues">
          <xsl:with-param name="cur_valueSetID" select="self::*/v3:participant/v3:roleParticipant/v3:code/@valueSet"/>
        </xsl:apply-templates>
      </xsl:if>      

      <!-- Needs to occur below valueSet - if relevant, will be output within new <timeRelationship> -db -->
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/> 
      
      <medicationState>
        <xsl:value-of select="@moodCode"/>
        <!-- deal with default if processed w/o schema -->
        <xsl:if test="string(@moodCode)=''">EVN</xsl:if>
      </medicationState>
      <xsl:if test="v3:routeCode/@code">
        <routeCode>
          <xsl:value-of select="v3:routeCode/@code"/>
        </routeCode>
      </xsl:if>

      <!-- TBD: These two would be easier if the names were simply
        rateQuantity and doseQuantity
        -->
      <xsl:if test="v3:rateQuantity">
        <medicationRateQuantity>
          <xsl:copy-of select="v3:rateQuantity/@*"/>
          <xsl:copy-of select="v3:rateQuantity/*"/>
        </medicationRateQuantity>
      </xsl:if>
      <xsl:if test="v3:doseQuantity">
        <medicationDoseQuantity>
          <xsl:copy-of select="v3:doseQuantity/@*"/>
          <xsl:copy-of select="v3:doseQuantity/*"/>
        </medicationDoseQuantity>
      </xsl:if>
    </MedicationCriteria>
  </xsl:template>
  
<!-- Original match commented out as it refers to an attribute (Medications) which does not exist 
for the two targeted localVariableNames (DiabetesMedSupplied and DiabetesMedOrdered -db )-->
<!-- match="v3:supplyCriteria[
     self::*/v3:definition/v3:*/v3:id/@extension = 
     'Medications']" -->
<!-- Replaced with @RX-->
  <!-- Medications (Supply) -->
  <!-- TBD: Consider merging this template with substanceAdministration -->
  <xsl:template
    match="v3:supplyCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension =
    'RX']">
    <MedicationCriteria>
      <xsl:if
        test="not(v3:participant/v3:roleParticipant[@classCode='THER']/v3:code)">
        <!-- TBD: Does it make sense to have a medication criteria without
          a code? -->
        <xsl:message terminate="no"> No Medication code found for
          medication criteria //supplyCriteria[
          <xsl:value-of
            select="1+ count(preceding::v3:supplyCriteria)"
          /> ] </xsl:message>
      </xsl:if>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode"
          select="v3:participant/v3:roleParticipant[@classCode='THER']/v3:code"
        />
      </xsl:call-template>
      
      <!-- If there's a valueSet within supplyCriteria, then we go forward
      Note: Only sections with valueSets must be processed   -db -->      
      <xsl:if test="self::*/v3:participant/v3:roleParticipant/v3:code/@valueSet"> 
        <xsl:message>code element with @valueSet for SUPPLY</xsl:message>
        <!-- Call collectValues template implicitly through template match and
        use the cur_valueSetID parameter to capture id and pass it through -db -->
        <xsl:apply-templates select="//v3:valueSet" mode="allValues">
          <xsl:with-param name="cur_valueSetID" select="self::*/v3:participant/v3:roleParticipant/v3:code/@valueSet"/>
        </xsl:apply-templates>
      </xsl:if>
      
      <!-- Needs to occur below valueSet - if relevant, will be output within new <timeRelationship> -db -->
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/>                 
     
      <medicationState>
        <!-- select between default @moodCode value or existing @moodCode value to be displayed  -db -->
        <xsl:choose>
          <!-- apply default value (EVN) if there is no moodCode attribute -db -->
          <xsl:when test="not(@moodCode)">
            <xsl:text>EVN</xsl:text>
          </xsl:when>
          <!-- otherwise apply the value of the existing attribute -db -->
          <xsl:otherwise>
            <xsl:value-of select="@moodCode"/>
          </xsl:otherwise>   
        </xsl:choose>           
      </medicationState>
      <!-- We will not know dose or rate if supply -->
    </MedicationCriteria>
  </xsl:template>
  
  <!-- Immunzations -->
  <!-- TBD: Do we need to deal with supply for immunization? -->
  <xsl:template
    match="v3:substanceAdministrationCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension = 'Immunizations']">
    <ImmunizationCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:value"/>
      </xsl:call-template>
      
      <!-- does immunization need a code here too? -db -->
      
      <!-- If there's a valueSet within ImmunizationCriteria, then we go forward
      Note: Only sections with valueSets must be processed -db -->    
      <xsl:if test="self::*/v3:participant/v3:roleParticipant/v3:code/@valueSet"> 
        <xsl:message>code element with @valueSet for IMMUNIZATION</xsl:message>
        <!-- Call collectValues template implicitly through template match and
        use the cur_valueSetID parameter to capture id and pass it through -db -->
        <xsl:apply-templates select="//v3:valueSet" mode="allValues">
          <xsl:with-param name="cur_valueSetID" select="self::*/v3:participant/v3:roleParticipant/v3:code/@valueSet"/>
        </xsl:apply-templates>
      </xsl:if>       
      
      <!-- Needs to occur below valueSet - if relevant, will be output within new <timeRelationship> -db -->
      <xsl:apply-templates select="v3:temporallyRelatedInformation"/>      
      
      <!-- TBD: Not sure what to put here -->
      <xsl:if test="false()">
        <reaction>st</reaction>
      </xsl:if>
      <xsl:if test="false()">
        <category>st</category>
      </xsl:if>
    </ImmunizationCriteria>
  </xsl:template>

  <xsl:template
    match="v3:observationCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension = 'Results']">
    <LabResultsCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:code"/>
      </xsl:call-template>
     
     <!--  Displays value if its within match specifications -db -->  <!-- original, does not collect from <excerpt> to match output -->
     <!-- <xsl:copy-of select="v3:value"/> -->
     <!-- Displays value outside of match (attribute specific) specifications -db --> <!-- 1st FIX, addition for specificity and to collect uncollected value -->
     <!-- <xsl:copy-of select="self::*/v3:excerpt/v3:*/v3:value"/> -->
     
     <!-- Dual fix: Should work for all situations - assuming we even want to collect outside of <excerpt> -db -->
     <xsl:copy-of select="self::*/v3:*/v3:*/v3:value"/>
 
      <!-- TBD: Not sure what to put here -->
      <xsl:if test="false()">
        <resultStatus>st</resultStatus>
      </xsl:if>
    </LabResultsCriteria>
  </xsl:template>

  <xsl:template
    match="v3:observationCriteria[
    self::*/v3:definition/v3:*/v3:id/@extension = 'Vitals']">
    <VitalSignCriteria>
      <xsl:call-template name="dataCriteria">
        <xsl:with-param name="primaryCode" select="v3:code"/>
      </xsl:call-template>

      <xsl:if test="false()">
        <vitalSignType>st</vitalSignType>
      </xsl:if>
      <xsl:if test="false()">
        <vitalSignValue>st</vitalSignValue>
      </xsl:if>
    </VitalSignCriteria>
  </xsl:template>
  
  <!--  Selects between default precondition value and matched precondition value. -db 
  The default for v3:patientPopulationCriteria and v3:numeratorCriteria and v3:denominatorCriteria is AllTrue.  
  Shown as  extension="IPP" (Initial Patient Population) and extension="DENOM" (Denominator) and extension="NUMER" (Numerator) in output -db -->
  <xsl:template
    match="v3:patientPopulationCriteria|v3:numeratorCriteria|v3:denominatorCriteria">
    
    <populationCriteria>             
      
      <xsl:copy-of select="v3:id"/>           
        
        <xsl:choose>           
          <!-- Checks if any current possible (default) precondition(s) exist or not.
          If it does NOT exist (as defined by this when statement), then a default value specific to the template match
          is applied to the curPreCon parameter and sent to the dataCriteriaCombiner template to be displayed. -db -->   
          <xsl:when test="not(v3:precondition//v3:allTrue) or not(v3:precondition//v3:atLeastOneTrue)">       
          <xsl:message>~~~~~No precondition found - applied default!</xsl:message>                       
	        <dataCriteriaCombiner>   
	          <criteriaOperation>  
	            <xsl:call-template name="dataCriteriaCombiner">
	              <!-- Apply default as there is no available precondition -db) -->  
	              <xsl:with-param name="curPreCon"><criteriaOperation>allTrue</criteriaOperation></xsl:with-param>
	      		</xsl:call-template>
	      	  </criteriaOperation>
	            <xsl:call-template name="displayDataRootExt"/>
	        </dataCriteriaCombiner>      
          </xsl:when>          
          <!-- Otherwise, defined precondition elements are found - 
          call the dataCriteriaCombiner template w/o a value to let the precondition specific template matches
          handle the curPreCon parameter assignment. -db  -->
          <xsl:otherwise>
            <xsl:message>applied template match!</xsl:message>  
<!--              <xsl:call-template name="dataCriteriaCombiner"/>   -->   
               <xsl:call-template name="displayDataRootExt"/>
          </xsl:otherwise>                 
        </xsl:choose>
    
    </populationCriteria>
    
  </xsl:template>
  
  <!-- Same as previous template (routes to default or matched) but requires a different default value per match -db
  The default for v3:denominatorExceptionCriteria is AtLeastOneTrue Shown as extension="DENEXCEP" (Denominator Exceptions) in output. -db -->
  <xsl:template match="v3:denominatorExceptionCriteria"> 

    <populationCriteria>             
      
      <xsl:copy-of select="v3:id"/>           
        
        <xsl:choose>           
          <xsl:when test="not(v3:precondition//v3:allTrue) or not(v3:precondition//v3:atLeastOneTrue)">         
          <xsl:message>~~~~~No precondition found - applied default!</xsl:message>                     
	        <dataCriteriaCombiner>   
	          <criteriaOperation>  
	            <xsl:call-template name="dataCriteriaCombiner">
	              <xsl:with-param name="curPreCon"><criteriaOperation>atLeastOneTrue</criteriaOperation></xsl:with-param>
	      		</xsl:call-template>
	      	  </criteriaOperation>
	            <xsl:call-template name="displayDataRootExt"/>
	        </dataCriteriaCombiner>      
          </xsl:when>          
          <xsl:otherwise>
            <xsl:message>applied template match!</xsl:message>  
<!--              <xsl:call-template name="dataCriteriaCombiner"/>    -->  
               <xsl:call-template name="displayDataRootExt"/>
          </xsl:otherwise>                 
        </xsl:choose>
    
    </populationCriteria>    
     
  </xsl:template>
  
  <xsl:template match="v3:stratifierCriteria">
    <stratifierCriteria>
      <xsl:copy-of select="v3:id"/>
      <xsl:call-template name="stratCombiner">
      </xsl:call-template>
    </stratifierCriteria>
  </xsl:template>

<!--  original template, displays dataCriteriaReference in wrong place -db -->
<!--   <xsl:template match="v3:observationReference|v3:actReference|
    v3:substanceAdministrationReference|v3:supplyReference|
    v3:procedureReference|v3:encounterReference">
-->
  
  <!-- removed v3:supplyReference, responsible for displaying dataCriteriaReference before <populationCriteria> section -->
  <xsl:template match="v3:observationReference|v3:actReference|
    v3:substanceAdministrationReference|v3:supplyReference|
    v3:procedureReference|v3:encounterReference">

    <dataCriteriaReference>
      <xsl:copy-of select="v3:id/@*"/>
    </dataCriteriaReference>
  </xsl:template>
  
  <!-- The following six templates apply the existing (found/matched) precondition by sending the associated 
  value with the curPreCon parameter to the dataCriteriaCombiner template. -db -->
  <xsl:template
    name="allTrueOperation"
    match="v3:allTrue">
      <dataCriteriaCombiner>
        <criteriaOperation> 
          <xsl:call-template name="dataCriteriaCombiner">
            <xsl:with-param name="curPreCon">allTrue</xsl:with-param>
          </xsl:call-template>
        </criteriaOperation>
          <xsl:call-template name="displayDataRootExt"/>
      </dataCriteriaCombiner>
  </xsl:template>  
  <xsl:template
    name="allFalseOperation"
    match="v3:allFalse">
      <dataCriteriaCombiner>
        <criteriaOperation> 
          <xsl:call-template name="dataCriteriaCombiner">
            <xsl:with-param name="curPreCon">allFalse</xsl:with-param>
          </xsl:call-template>
        </criteriaOperation>
          <xsl:call-template name="displayDataRootExt"/>
      </dataCriteriaCombiner>
  </xsl:template>  
  <xsl:template
    name="atLeastOneTrueOperation"
    match="v3:atLeastOneTrue">
      <dataCriteriaCombiner>
        <criteriaOperation>
          <xsl:call-template name="dataCriteriaCombiner">
            <xsl:with-param name="curPreCon">atLeastOneTrue</xsl:with-param>
          </xsl:call-template>
        </criteriaOperation>
          <xsl:call-template name="displayDataRootExt"/>
      </dataCriteriaCombiner>
  </xsl:template>   
  <xsl:template
    name="atLeastOneFalseOperation"
    match="v3:atLeastOneFalse">
      <dataCriteriaCombiner>
        <criteriaOperation>    
          <xsl:call-template name="dataCriteriaCombiner">
            <xsl:with-param name="curPreCon">atLeastOneFalse</xsl:with-param>
          </xsl:call-template>
        </criteriaOperation>
          <xsl:call-template name="displayDataRootExt"/>
      </dataCriteriaCombiner>                 
  </xsl:template>  
  <xsl:template
    name="onlyOneTrueOperation"
    match="v3:onlyOneTrue">
      <dataCriteriaCombiner>
        <criteriaOperation>     
          <xsl:call-template name="dataCriteriaCombiner">
            <xsl:with-param name="curPreCon">onlyOneTrue</xsl:with-param>
          </xsl:call-template>
        </criteriaOperation>
          <xsl:call-template name="displayDataRootExt"/>
      </dataCriteriaCombiner>                     
  </xsl:template>   
  <xsl:template
    name="onlyOneFalseOperation"
    match="v3:onlyOneFalse">
      <dataCriteriaCombiner>
        <criteriaOperation>    
          <xsl:call-template name="dataCriteriaCombiner">
            <xsl:with-param name="curPreCon">onlyOneFalse</xsl:with-param>
          </xsl:call-template>
        </criteriaOperation>
          <xsl:call-template name="displayDataRootExt"/>
      </dataCriteriaCombiner>               
  </xsl:template>
  
  <!-- Displays current sent precondition value.  
  The value changes based on whether its in the source or needs a default applied -db -->
  <xsl:template
    name="dataCriteriaCombiner">
    
    <!-- Define parameter used to keep track of current precondition -db -->   
    <xsl:param name="curPreCon"/> 
    
    <!-- Reports when fucntion is called to the console -db -->
    <xsl:message>***dataCriteriaCombiner***</xsl:message>     
      
    <!-- Display current sent precondition -db -->
    <xsl:value-of select="$curPreCon"/>
    <xsl:message>###printed criteria###</xsl:message>     
  </xsl:template>
  
  <!-- Allows the display of the current dataCriteriaReference element(s) along with its root and extension attributes -db -->
  <xsl:template
    name="displayDataRootExt">
    
      <xsl:apply-templates 
        select="v3:precondition[v3:observationReference|v3:actReference|
        v3:substanceAdministrationReference|v3:supplyReference|
        v3:procedureReference|v3:encounterReference]/*"/>
      
      <xsl:apply-templates 
        select="v3:precondition[v3:allTrue|v3:allFalse|
        v3:atLeastOneTrue|v3:atLeastOneFalse|
        v3:onlyOneTrue|v3:onlyOneFalse]/*"/>       
  </xsl:template>  
  
  <!--  <stratifierCriteria> should NOT contain precondition values hence the separate template -db -->
  <xsl:template
    name="stratCombiner">
          
      <!--  displays dataCriteriaReference and attributes (root and extension) within stratifierCriteria -db -->
      <xsl:apply-templates 
        select="v3:precondition[v3:observationReference|v3:actReference|
        v3:substanceAdministrationReference|v3:supplyReference|
        v3:procedureReference|v3:encounterReference]/*"/>       
  </xsl:template>
    
</xsl:stylesheet>