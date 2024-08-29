<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xmlns:xalan="http://xml.apache.org/xalan"
  xmlns:v3="urn:hl7-org:v3" xmlns="urn:hl7-org:v3"
  exclude-result-prefixes="xsi xalan">
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
        Jeff Klann - 8/8/2012 - bugfix, somehow wasn't carrying dataCriteriaCombiner operations over.
          The revised translator will supersede this bugfix.
        Jeff Klann - 9/27/12 - NOTE THIS IS NOT THE SAME AS THE DRAJER LLC VERSION. This version is being maintained for compatibility with
          existing transforms and the current ballot. Once r2 is approved, we should review to what degree the intermediate format
          provides benefit other than increasing readibility. This version is a HACK that keeps the header, measure period,
          population criteria, temporal relationship, and excerpt code from Keith's original version. Otherwise it just straight up copies
          the data criteria so it can be properly processed in the reverse transform. This was needed to quickly support all the neat new
          features in CEDD and mesh with the XML config file.
  -->
  <xsl:template match="/v3:QualityMeasureDocument">
    <ihqmf>
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
        select="v3:component/v3:dataCriteriaSection/v3:entry/v3:*[v3:definition]" mode="dataCriteria"/>

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

  <!-- The intermediate model has many problems. To reduce code rewriting, we reuse its population,header,controlVariable,excerpt,and timeRelationship
  simplifications and simply copy the criteria definitions. -->
  <xsl:template match="*" mode="dataCriteria" >
    <xsl:element name="{local-name(.)}">
      <xsl:copy-of select="../v3:localVariableName"/>
      <xsl:apply-templates mode="copy"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="v3:definition" mode="copy">
    <xsl:element name="criteriaType"><xsl:value-of select="current()/v3:*[1]/v3:id/@extension"/></xsl:element>
  </xsl:template>
  
  <xsl:template match="@*|node()" mode="copy">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" mode="copy"/>
    </xsl:copy>
  </xsl:template>

  <!-- JGK: Keeping Keith's code for excerpts, temporallyRelatedInformation, and IPP because I'd already coded it. -->

  <!-- Handle excerpts as filters -->
  <xsl:template match="v3:excerpt" mode="copy">
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
      <!-- Child criteria will not be allowed in the next release: <xsl:apply-templates select="*"/> -->
    </filterCriteria>
  </xsl:template>

  <!-- Map temporallyRelatedInformation to timeRelationship -->
  <xsl:template match="v3:temporallyRelatedInformation" mode="copy">
    <timeRelationship>
      <timeRelationshipCode>
        <xsl:value-of select="@typeCode"/>
      </timeRelationshipCode>
      <timeQuantity value="{v3:pauseQuantity/@value}"
        unit="{v3:pauseQuantity/@unit}"/>
      <measurePeriodTimeReference>
        <xsl:copy-of select="v3:*/v3:id"/>
        <!-- If there is no event reference, use measurePeriod ID 
          TBD: Discuss whether this works for implementers, and ensure
          that <measurePeriod> and other events have similar representation
          of time components with respect to time to facilitate 
          implementation.
        -->
        <xsl:if test="not(v3:*/v3:id)">
          <xsl:copy-of select="//v3:measurePeriod/v3:id"/>
        </xsl:if>
      </measurePeriodTimeReference>
    </timeRelationship>
  </xsl:template>
  
  <!-- TBD: Still need to deal with this -->
  <xsl:template
    match="v3:patientPopulationCriteria|v3:numeratorCriteria|v3:denominatorCriteria">
    <populationCriteria>
      <xsl:copy-of select="v3:id"/>
      <xsl:call-template name="dataCriteriaCombiner">
        <xsl:with-param name="operation">AllTrue</xsl:with-param>
      </xsl:call-template>
    </populationCriteria>
  </xsl:template>
  
  <xsl:template match="v3:denominatorExceptionCriteria"> 
    <populationCriteria>
      <xsl:copy-of select="v3:id"/>
      <xsl:call-template name="dataCriteriaCombiner">
        <xsl:with-param name="operation">AtLeastOneTrue</xsl:with-param>
      </xsl:call-template>
    </populationCriteria>
  </xsl:template>
  <xsl:template match="v3:stratifierCriteria">
    <stratifierCriteria>
      <xsl:copy-of select="v3:id"/>
      <xsl:call-template name="dataCriteriaCombiner">
        <xsl:with-param name="operation">OnlyOneTrue</xsl:with-param>
      </xsl:call-template>
    </stratifierCriteria>
  </xsl:template>

  <xsl:template match="v3:observationReference|v3:actReference|
    v3:substanceAdministrationReference|v3:supplyReference|
    v3:procedureReference|v3:encounterReference">
    <!-- TBD: Move @root/@extension up to dataCriteriaReference 
      (Change v3:id below to v3:id/@*)
    -->
    <dataCriteriaReference>
      <xsl:copy-of select="v3:id/@*"/>
    </dataCriteriaReference>
  </xsl:template>
  
  <xsl:template
    name="dataCriteriaCombiner"
    match="v3:allTrue|v3:allFalse|
       v3:atLeastOneTrue|v3:atLeastOneFalse|
       v3:onlyOneTrue|v3:onlyOneFalse">
    <!--<xsl:param name="operation"/>
    
    <xsl:param name="operation" select="concat(translate(substring(local-name(.),
      1,1),'abcdefghijklmnopqrstuvwxyz',
      'abcdefghijklmnopqrstuvwxyz'),substring(local-name(.),2,string-length(local-name(.))))"/> -->
   
    <xsl:param name="operation" select="concat(translate(substring(local-name(.),
      1,1),'abcdefghijklmnopqrstuvwxyz',
      'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),substring(local-name(.),2,string-length(local-name(.))))"/>

    <dataCriteriaCombiner>
      <!-- TBD: Change initial case of dataCriteriaOperationType to
        match schema
      -->
      <criteriaOperation>
        <xsl:value-of select="$operation"/>
      </criteriaOperation>
      <xsl:apply-templates 
        select="v3:precondition[v3:observationReference|v3:actReference|
        v3:substanceAdministrationReference|v3:supplyReference|
        v3:procedureReference|v3:encounterReference]/*"/>
      
      <xsl:apply-templates 
        select="v3:precondition[v3:allTrue|v3:allFalse|
        v3:atLeastOneTrue|v3:atLeastOneFalse|
        v3:onlyOneTrue|v3:onlyOneFalse]/*"/>
    </dataCriteriaCombiner>
  </xsl:template>
</xsl:stylesheet>
