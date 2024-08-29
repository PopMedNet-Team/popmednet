<?xml version="1.0" encoding="UTF-8"?>
<!-- iHQMF to i2b2 Converter
    Changelog:
    Jeff Klann - 5/2012 - v0.1
        Supports basic transformations. Demographics->Age; Diagnosis; Lab Test Results
        Inversion (NOT operation)
        effectiveTime and measurementPeriod
        Panel-wide timeRelationships: all time relationships within a panel reference each other and occur overlappingly 
        Panel-wide filterRelationships: all criteria within a panel specify a minimum occurrence number that is the same
        (Appropriate errors thrown for time and filter relationships that are impossible in i2b2 XML.)
        SHRINE Demographics -> Age, Race, Marital Status, Gender, Language
        SHRINE Diagnoses, Medications
        i2b2 demo procedures
    Jeff Klann - 6/2012 - v0.2
    	First pass at integration is complete. The webservice URL to get term info is passed as as parameter "serviceurl", which defaults to localhost. Only the code request is sent, not the authentication info. The service URL's port is hardcoded to 8080 right now, which should be changed. The tree that is searched is determined by the rootkey parameter. Alternate 'SHRINE|' codes are concatenated into the result tree, at high computational cost. Also added a fullquery parameter that adds headers only if set to true, because the webclient can add these.
    	Fixed bug: multiple item-names showing up
    	Fixed bug: destination namespaces should be supressed
    	Fixed bug (but in a messy way): demographics values should be processed as codes
    Jeff Klann 7/10/2012 - v0.21
        Modified XSL to support varied ports and take either passwords or tokens.        
        Now passes authentication info to the ONT cell for terminology lookups.
    Jeff Klann 7/12/2012
        Weird typo prevented demographic code lookups.
        Warning messages are now also output as XML comments.
        \\SHRINE rootkey properly causes SHRINE| to be prepended
    Jeff Klann 7/16/2012
        Now emits better error messages as embedded XML comments for unauthenticated user or not found code (includes URL now).
        If an OID is found, it is now emitted as an I2B2 key to be consistent with the behavior of tohqmf.xsl. 
          Note that it doesn't look up information on the key, so the resulting XML might not work. - UNTESTED!
        Demographics criteria codes are now prepended with DEM| for all rootkeys that start with \\I2B2 or \\i2b2,
          to better support the demo ontology
    Jeff Klann 7/19/2012
       Ripped out the old age handling code that allowed ranges but relied on the local concepts file.
       New version prefers an IVL_PQ of 1 year but looks things up in the actual ontology cell. 
       A special hack generates an item_key for age ranges but presently does not use the ontology cell
         (it probably ought to use get_name or a partial key match) and cannot handle ranges that aren't 
         directly in i2b2
    Jeff Klann 8/4/2012
       Added a hack to support CEDD while it still has SHRINE| basecodes. 
    Jeff Klann 8/9/2012
       Added external configuration support for codeSystem->basecode conversion.
    Jeff Klann 10/1/2012
       Significant rewrite to support CEDD ontology for the NYC pilot. Now also uses the translatorMetaConfig extensively.
       Old code got cleaned up, new code needs to be refactored, but many new features are supported: anonymous text, anonymous codes, modifiers, result flags, providers, vitals, social history, and more!
       Also subkey-age no longer needs to be specified. All you need are rootkey, fullquery, and serviceurl.
       Now requires a mutt version of ihqmf as input, with the old version of ihqmf for everythng except dataCriteria, which should be copied verbatim but with a criteriaType added.
    Jeff Klann 10/2/2012
       Updated to come in line with bugixes in the HQMF transform, really just that value is in the node body.
       effectiveTime was causing an error to be thrown.
    Jeff Klann 10/21/2012
       Added support for value sets and person date of birth (actually all demographic criteria with type IVL_TS).
    Jeff Klann 11/8/2012
       Bugfix: was still referring to DemographicsCriteria in two places in the code. 
    Jeff Klann 12/13/12
       Rootkey of \\i2b2_ now signals DEM| and DIAG| basecode prefixes and \\i2b2_DEMO usage for age range queries
         AND lowercasing of codes (this works for the BIDMC demo but seems like a bit of a hack in the general case)
    
    Todo: 
      EncounterCriteria AgeAtVisit.
      Many special cases have a lot of special case code - could use an organizational refresh
      Finish timeRelationships to the measurePeriod (right now just copies the measurePeriod)
      Most modifiers not supported
      More details in spreadsheet and other materials.
      Misc todos
-->

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:xalan="http://xml.apache.org/xalan"
    xmlns:java="http://xml.apache.org/xslt/java"
    xmlns:hl7v3="urn:hl7-org:v3"
    xmlns:mc="urn:jklann:hqmf:metaConfig"
    xmlns:dyn="http://exslt.org/dynamic"
    exclude-result-prefixes="xsi xalan java hl7v3 mc"    
    version="1.0">
    <xsl:import href="time.xsl"/>
    
    <xsl:output method="xml" standalone="yes" omit-xml-declaration="no" indent="yes" xalan:indent-amount="2"/>
    
    <!-- Pretty the output -->
    <xsl:strip-space elements="*"/>
    <xsl:template match="text()"/>
    <xsl:template mode="item" match="text()"/>
    
    <!-- Global parameters that should get their value from the invoking code.
       I think this will all be determined by the server or by query health and doesn't need to be packaged in HQMF -->
    <xsl:param name="username">demo</xsl:param>
    <xsl:param name="sessiontoken"></xsl:param>
    <xsl:param name="userpassword">demouser</xsl:param>
    <xsl:param name="userproject">Demo</xsl:param> <!-- Also this is the user's group -->
    <xsl:param name="userdomain">i2b2demo</xsl:param> <!-- Public demoserver is HarvardDemo -->
    
    <!-- Other important options -->
    <!--<xsl:param name="serviceurl">http://ec2-23-20-41-242.compute-1.amazonaws.com:9090/jersey</xsl:param> -->
    <xsl:param name="serviceurl">http://localhost:8080</xsl:param> <!-- The base URL and port of the webservice. -->
    <xsl:param name="fullquery">false</xsl:param> <!-- Set to true to produce all headers for the query -->
    <xsl:param name="rootkey">\\CEDD</xsl:param> <!-- Set which ontology hierarchy to search for reverse translation. -->
      <!-- Put the root key here, e.g. \\I2B2, \\I2B2_DEMO, \\SHRINE, \\CEDD.
         The root key of \\ is special and forces use of the local concepts.xml instead of live terminology lookups. 
         \\SHRINE also causes SHRINE| to be prepended in getTermInfo. This is used for filtering except 
         when reconstructing ages. \\CEDD also turns on CEDD-specific features. -->
    
    <!-- Concepts. Now hooks into ontology cell, unless rootkey is \\. -->
    <xsl:variable name="concepts" select="document('concepts.xml')"/>
    
    <!-- Load the metaconfig. -->
    <xsl:variable name="metaconfig" select="document('translatorMetaConfig.xml')"/>
   
    <!-- Have references to dataCriteria readily available. -->
    <xsl:key name="dataCriteria" match="/hl7v3:ihqmf/*/hl7v3:id" use="@extension"/>
    
    <!-- Also have a reference to the measurePeriod available. -->
    <xsl:variable name="measurePeriod-rtf">
        <xsl:apply-templates select="/hl7v3:ihqmf/hl7v3:measurePeriod/hl7v3:value"/>
    </xsl:variable>
    <xsl:variable name="measurePeriod" select="xalan:nodeset($measurePeriod-rtf)"/>
    
    <!-- Output concepts codes for dataCriteria -->
    <xsl:template match="hl7v3:id[local-name(parent::node())!='qualifiedEntityParticipant']|hl7v3:criteriaType|hl7v3:localVariableName" mode="get-concept"></xsl:template>
    <xsl:template match="hl7v3:*" mode="get-concept">
      <xsl:variable name="criteriaType" select="ancestor::node()/hl7v3:criteriaType"></xsl:variable> 
      <xsl:variable name="code" select="current()"/>
      <xsl:variable name="parent" select="parent::node()"/>
            
      <!-- extract the criteria mapping from the metaconfig -->
      <!-- TODO: could return multiple criteria mappings in cases where I reused an extension (social history and healthcare provider). 
           Right now the workaround is that all possible modifier mappings are considered for reused extensions. -->
      <xsl:variable name="metacriteria"
        select="$metaconfig/mc:metaConfig/mc:criteriaMappings/mc:item[@extension=$criteriaType][@i2b2_rootkey=$rootkey][1]"/>      
      <xsl:if test="not($metacriteria)"><xsl:message terminate="yes">(501) Undefined criteria type <xsl:value-of select="$criteriaType"/></xsl:message></xsl:if> 
      
      <!-- Extract modifier criteria information from the metaconfig -->
      <xsl:variable name="metamodifier"
        select="$metaconfig/mc:metaConfig/mc:modifierMappings/mc:item[@i2b2_type=$metaconfig/mc:metaConfig/mc:criteriaMappings/mc:item[@extension=$criteriaType][@i2b2_rootkey=$rootkey]/@i2b2_type and 
          (@tag_name=local-name($code) or local-name($parent)=@special)]"/>

      <xsl:if test="local-name()!=$metacriteria/@tag_name and not($metamodifier/@special) and local-name()!=$metamodifier/@tag_name">
        <xsl:message terminate="yes">(501) Could not find an i2b2 parse for <xsl:value-of select="local-name()"/> in <xsl:value-of select="parent::node()"/></xsl:message>
      </xsl:if>

      <!-- If it's a demographic, we might need information on it -->
      <xsl:variable name="metademo">
        <xsl:if test="$metacriteria/@extension='Demographics'">
          <xsl:variable name="demo-code" select="parent::node()/hl7v3:code"></xsl:variable>
          <xsl:variable name="democode-rtf">
            <xsl:for-each select="$metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[$demo-code/@code=@code]">
              <xsl:choose>
                <xsl:when test="not(attribute::test-reverse)">
                  <xsl:copy-of select="current()"/>
                </xsl:when>            
                <xsl:when test="attribute::test-reverse">
                  <xsl:if test="dyn:evaluate(attribute::test-reverse)">
                    <xsl:copy-of select="current()"/>
                  </xsl:if>
                </xsl:when>
              </xsl:choose>
            </xsl:for-each>
          </xsl:variable>
          <xsl:variable name="democode" select="xalan:nodeset($democode-rtf)/mc:item"/>
          <xsl:copy-of select="$democode"/>
        </xsl:if>
      </xsl:variable>
      
      <!-- Extract information for rebuilding the key -->
      <xsl:variable name="subtype-rtf">
        <xsl:choose>
          <!--<xsl:when test="count(xalan:nodeset($metademo)/mc:item)=0"></xsl:when>-->
          <!-- CEDD ends its subtypes with _[nodename]
               TODO: this _[nodename] was intended to make Translation easier, but it currently ends up requiring a lot of special cases. I should rethink this with MBuck. -->
          <xsl:when test="$rootkey='\\CEDD'">
            <xsl:choose>
              <!-- Special case: subtypes labeled as noExtension in the config file. -->
              <xsl:when test="xalan:nodeset($metademo)/mc:item/@noExtension"><xsl:value-of select="xalan:nodeset($metademo)/mc:item/@i2b2_subtype"/></xsl:when>
              <!-- Special case: non-demographic, non-CD elements are _text -->
              <xsl:when test="(count(xalan:nodeset($metademo)/mc:item)=0) and not(@xsi:type='CD')"><xsl:value-of select="concat(xalan:nodeset($metademo)/mc:item/@i2b2_subtype,'_text')"/></xsl:when>
              <!-- Special case (this is a weird one) when the criteria definition's primary data type matches this element, it's _code -->
              <xsl:when test="$metacriteria/@tag_name=local-name() and (@xsi:type='CD' or @xsi:type='ST')"><xsl:value-of select="concat(xalan:nodeset($metademo)/mc:item/@i2b2_subtype,'_code')"/></xsl:when>
              <!-- If we're processing the code element of a demographic, we must be referring to it's _value element. (For non-CD types that have codes relevant to i2b2.) -->
              <xsl:when test="(count(xalan:nodeset($metademo)/mc:item)>0) and not(local-name='code')"><xsl:value-of select="concat(xalan:nodeset($metademo)/mc:item/@i2b2_subtype,'_value')"/></xsl:when>
              <xsl:otherwise><xsl:value-of select="concat(xalan:nodeset($metademo)/mc:item/@i2b2_subtype,'_',local-name())"/></xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="xalan:nodeset($metademo)/mc:item/@i2b2_subtype"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <xsl:variable name="subtype" select="xalan:nodeset($subtype-rtf)"/>
      <xsl:variable name="type" select="$metacriteria/@i2b2_type"/>
      
      <xsl:choose>
        <!-- We ignore the code element in demographics, it's only useful in rebuilding the I2B2 key. Exception is when I'm using the demographic template for not-quite-demographic things like social history, so we catch those by
             looking for situations where the demographic code is undefined in the config file. Also demographic values with a string or IVL_TS type (like postalcode or date of birth) require the code to be repeated.
              TODO: Technically should do this for IVL_PQ as well but I use not very portable translation code that requires it be ignored here. -->
        <xsl:when test="local-name()='code' and $metacriteria/@extension='Demographics' and not(count(xalan:nodeset($metademo)/mc:item)=0 or xalan:nodeset($metademo)/mc:item/@dataType='ST' or xalan:nodeset($metademo)/mc:item/@dataType='IVL_TS')"/>
        <xsl:when test="local-name()='participant'"/> <!-- We also ignore the top level participant element, it gets unpacked in a later template. (Note that this is strange organization.) -->
        <xsl:when test="local-name()='effectiveTime'"/>
        
        <!-- Todo:  cases where key needs to be rebuilt, cases where we want the demographic code reinserted TODO: bug social history modifiers stopped working!!-->
 
        <!-- Special case, provider participants -->
        <xsl:when test="ancestor::node()[local-name()='participant'][@typeCode='PRF']">
          <xsl:variable name="value-constraint">
            <constrain_by_value>
              <value_operator>LIKE[contains]</value_operator>
              <value_constraint><xsl:value-of select="concat(current()/@code,text(),hl7v3:name/text(),current()/@extension)"/></value_constraint> <!-- This is a bit of a hack too. -->
              <value_unit_of_measure></value_unit_of_measure>
              <value_type>TEXT</value_type>
            </constrain_by_value>
          </xsl:variable>
          <!-- Generate key name -->
          <xsl:variable name="key-name">
            <!-- Modifier vs. item. Note I could just change type/subtype above but would need to check both for existence of metamodifier and value/code elements in the event of a special tag. Seemed easier here. -->
            <xsl:choose>
              <xsl:when test="ancestor::node()/hl7v3:value or ancestor::node()/hl7v3:code">
                <xsl:value-of select="concat('%5C%5C',substring($rootkey,3),'%5C%25',$metamodifier/@i2b2_type,'%5C',$metamodifier/@i2b2_subtype,'%5C')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat('%5C%5C',substring($rootkey,3),'%5C%25', 'providerID','%5C')"/>   <!-- TODO: I got rid of $type here because there's not a 1:1 mapping in the config. -->
              </xsl:otherwise>
            </xsl:choose>
            <!-- Note that this choose block and the above hardcoded reference to providerID ought to be moved to metaconfig, but I'm waiting for a more thorough set of criteria to map before I make this change.
                 Also, TODO, ask Mike why the inconsistency between organizationName and providerOrganizationName. -->
            <xsl:choose>
              <xsl:when test="local-name()='id'"></xsl:when>
              <xsl:when test="local-name()='name'"><xsl:value-of select="'providerName%5C'"/></xsl:when>
              <xsl:when test="local-name()='qualifiedOrganization' and hl7v3:name and (ancestor::node()/value or ancestor::node()/code)"><xsl:value-of select="'providerOrganizationName%5C'"/></xsl:when> 
              <xsl:when test="local-name()='qualifiedOrganization' and hl7v3:name and not(ancestor::node()/value or ancestor::node()/code)"><xsl:value-of select="'organizationName%5C'"/></xsl:when>
              <xsl:otherwise><xsl:message terminate="yes">(501) This type of provider code is not implemented: <xsl:value-of select="local-name()"/></xsl:message></xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <!-- Call getTermInfo -->
          <xsl:variable name="terminfo-rtf">
            <xsl:call-template name="getTermInfo">
              <xsl:with-param name="key-name" select="$key-name"/>
            </xsl:call-template>
          </xsl:variable>
          <xsl:variable name="terminfo" select="xalan:nodeset($terminfo-rtf)/concept[1]"/>
          <!-- Output i2b2 XML (could probably reduce redundancy here) -->
          <xsl:choose>
            <xsl:when test="ancestor::node()/hl7v3:value or ancestor::node()/hl7v3:code">
              <constrain_by_modifier>
                <modifier_name><xsl:value-of select="$terminfo/name"/></modifier_name>
                <modifier_key><xsl:value-of select="$terminfo/key"/></modifier_key>
                <applied_path><xsl:value-of select="concat(substring($rootkey,2),'\',$type,'\%')"/></applied_path>
                <xsl:copy-of select="$value-constraint"/>
              </constrain_by_modifier>
            </xsl:when>
            <xsl:otherwise>
              <hlevel><xsl:value-of select="$terminfo/level"/></hlevel>
              <item_name><xsl:value-of select="$terminfo/name"/></item_name>
              <item_key><xsl:value-of select="$terminfo/key"/></item_key>
              <tooltip><xsl:value-of select="$terminfo/tooltip"/></tooltip>
              <class>ENC</class> <!-- TODO: I'm not sure what this means. -->
              <item_icon><xsl:value-of select="$terminfo/visualattributes"/></item_icon>
              <item_is_synonym><xsl:value-of select="$terminfo/synonym_cd"/></item_is_synonym> 
              <xsl:copy-of select="$value-constraint"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        
        <!-- Special case: Demographic age ranges. -->
        <xsl:when test="local-name()='value' and current()/@xsi:type='IVL_PQ' and $metademo and xalan:nodeset($metademo)/mc:item/@code='424144002'">
          <xsl:variable name="codeinfo-rtf"><xsl:call-template name="getAge">
            <xsl:with-param name="type" select="$type"/>
            <xsl:with-param name="subtype" select="$subtype"/>
          </xsl:call-template></xsl:variable>
          <xsl:variable name="codeinfo" select="xalan:nodeset($codeinfo-rtf)/concept"/>     
          <hlevel><xsl:value-of select="$codeinfo/level"/></hlevel>
          <item_name><xsl:value-of select="$codeinfo/name"/></item_name>
          <item_key><xsl:value-of select="$codeinfo/key"/></item_key>
          <tooltip><xsl:value-of select="$codeinfo/tooltip"/></tooltip>
          <class>ENC</class> <!-- TODO: I'm not sure what this means. -->
          <item_icon><xsl:value-of select="$codeinfo/visualattributes"/></item_icon>
          <item_is_synonym><xsl:value-of select="$codeinfo/synonym_cd"/></item_is_synonym>  
        </xsl:when>
        
        <!-- Handle PQ and IVL_PQ and flags in value -->
        <xsl:when test="local-name()='value' and (current()/@xsi:type='PQ' or current()/@xsi:type='IVL_PQ' or current()/@code='62482003' or current()/@code='75540009' or current()/@code='260394003')">
          <xsl:call-template name="contrain_by_value"/>
        </xsl:when> 
        
        <!-- Handle demographic CDs that aren't directly in the terminology
             TODO: Redundancy with freetext and provider templates. -->
        <xsl:when test="local-name()='code' and $metacriteria/@extension='Demographics' and (xalan:nodeset($metademo)/mc:item/@dataType='ST' or xalan:nodeset($metademo)/mc:item/@dataType='IVL_TS')">
          <xsl:variable name="key-name" select="concat('%5C%5C',substring($rootkey,3),'%5C%25', $type,'%5C%25', $subtype)"/>  
          <!-- Call getTermInfo -->
          <xsl:variable name="terminfo-rtf">
            <xsl:call-template name="getTermInfo">
              <xsl:with-param name="key-name" select="$key-name"/>
              <xsl:with-param name="output-opts" select="'nomods'"></xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <xsl:variable name="terminfo" select="xalan:nodeset($terminfo-rtf)/concept[1]"/>
          <!-- Output i2b2 XML (could probably reduce redundancy here) -->
          <hlevel><xsl:value-of select="$terminfo/level"/></hlevel>
          <item_name><xsl:value-of select="$terminfo/name"/></item_name>
          <item_key><xsl:value-of select="$terminfo/key"/></item_key>
          <tooltip><xsl:value-of select="$terminfo/tooltip"/></tooltip>
          <class>ENC</class> <!-- TODO: I'm not sure what this means. -->
          <item_icon><xsl:value-of select="$terminfo/visualattributes"/></item_icon>
          <item_is_synonym><xsl:value-of select="$terminfo/synonym_cd"/></item_is_synonym>              
        </xsl:when>
        
        <!-- Handle CDs with code systems or value sets  -->
        <xsl:when test="(current()/@xsi:type='CD' or local-name()='code') and (current()/@codeSystem or current()/@valueSet)">
          <xsl:variable name="codeinfo-rtf"><xsl:call-template name="getCodeInfo"><xsl:with-param name="type" select="$type"></xsl:with-param></xsl:call-template></xsl:variable>
          <xsl:variable name="codeinfo" select="xalan:nodeset($codeinfo-rtf)/concept"/>   
          <xsl:choose>
            <xsl:when test="$metamodifier">
              <constrain_by_modifier>
                <modifier_name><xsl:value-of select="$codeinfo/name"/></modifier_name>
                <modifier_key><xsl:value-of select="$codeinfo/key"/></modifier_key>
                <applied_path><xsl:value-of select="concat(substring($rootkey,2),'\',$type,'\%')"/></applied_path>
              </constrain_by_modifier>
            </xsl:when>
            <xsl:otherwise>
              <hlevel><xsl:value-of select="$codeinfo/level"/></hlevel>
              <item_name><xsl:value-of select="$codeinfo/name"/></item_name>
              <item_key><xsl:value-of select="$codeinfo/key"/></item_key>
              <tooltip><xsl:value-of select="$codeinfo/tooltip"/></tooltip>
              <class>ENC</class> <!-- TODO: I'm not sure what this means. -->
              <item_icon><xsl:value-of select="$codeinfo/visualattributes"/></item_icon>
              <item_is_synonym><xsl:value-of select="$codeinfo/synonym_cd"/></item_is_synonym>              
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
       
        <!-- Handle freetext entry -->
        <xsl:when test="current()/@xsi:type='ST' or ((current()/@xsi:type='CD' or local-name()='code') and not(current()/@codeSystem))">
          
          <!-- TODO: VERY similar to provider template. Should be merged, especially if we can make a healthcare provider extension in HQMF. -->
          <xsl:if test="current()/@xsi:type='CD' or count(xalan:nodeset($metademo)/mc:item)=0"> <!-- TODO: Would be better to check against and use originalText, requires changes on both sides of xform.-->
            <!-- Generate key name -->
            <xsl:variable name="key-name">
              <!-- Modifier vs. item. Note I could just change type/subtype above but would need to check both for existence of metamodifier and value/code elements in the event of a special tag. Seemed easier here. -->
              <xsl:choose>
                <xsl:when test="not(local-name()='value' or local-name()='code')">
                  <xsl:value-of select="concat('%5C%5C',substring($rootkey,3),'%5C%25',$metamodifier/@i2b2_type,'%5C',$metamodifier/@i2b2_subtype,'%5C')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat('%5C%5C',substring($rootkey,3),'%5C%25', $type,'%5C%25', $subtype)"/>  
                </xsl:otherwise>
              </xsl:choose>  
            </xsl:variable>
            <!-- Call getTermInfo -->
            <xsl:variable name="terminfo-rtf">
              <xsl:call-template name="getTermInfo">
                <xsl:with-param name="key-name" select="$key-name"/>
                <xsl:with-param name="output-opts" select="'nomods'"></xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="terminfo" select="xalan:nodeset($terminfo-rtf)/concept[1]"/>
            <!-- Output i2b2 XML (could probably reduce redundancy here) -->
            <xsl:choose>
              <xsl:when test="ancestor::node()/value or ancestor::node()/code">
                <constrain_by_modifier>
                  <modifier_name><xsl:value-of select="$terminfo/name"/></modifier_name>
                  <modifier_key><xsl:value-of select="$terminfo/key"/></modifier_key>
                  <applied_path><xsl:value-of select="concat(substring($rootkey,2),'\',$type,'\%')"/></applied_path>
                </constrain_by_modifier>
              </xsl:when>
              <xsl:otherwise>
                <hlevel><xsl:value-of select="$terminfo/level"/></hlevel>
                <item_name><xsl:value-of select="$terminfo/name"/></item_name>
                <item_key><xsl:value-of select="$terminfo/key"/></item_key>
                <tooltip><xsl:value-of select="$terminfo/tooltip"/></tooltip>
                <class>ENC</class> <!-- TODO: I'm not sure what this means. -->
                <item_icon><xsl:value-of select="$terminfo/visualattributes"/></item_icon>
                <item_is_synonym><xsl:value-of select="$terminfo/synonym_cd"/></item_is_synonym>              
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
          <constrain_by_value>
            <value_operator>LIKE[contains]</value_operator>
            <value_constraint><xsl:value-of select="concat(current()/text(),current()/@code)"/></value_constraint>
            <value_unit_of_measure></value_unit_of_measure>
            <value_type>TEXT</value_type>
          </constrain_by_value>
        </xsl:when>
        
        <xsl:otherwise>
          <xsl:message terminate="yes">(501) Unsupported variable (type <xsl:value-of select="current()/@xsi:type"/>, element <xsl:value-of select="local-name()"/>)</xsl:message>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:template>
  
  <!-- Use the getCodeInfo function from my Java service to communicate with ONT 
       Returns the first matching term (a concept element) -->
  <xsl:template name="getTermInfo">
    <xsl:param name="key-name"/>
    <xsl:param name="output-opts"/>
    
    <xsl:variable name="url">
      <xsl:if test="$sessiontoken=''"><xsl:value-of select="concat($serviceurl,'/hqmf/getTermInfo/',$userdomain,'/',$userproject,'/',$username,'/password/',$userpassword)"/></xsl:if>
      <xsl:if test="not($sessiontoken='')"><xsl:value-of select="concat($serviceurl,'/hqmf/getTermInfo/',$userdomain,'/',$userproject,'/',$username,'/token/SessionKey:',$sessiontoken)"/></xsl:if>
    </xsl:variable>
    <xsl:variable name="terminfo" select="document(concat($url,'?key=', $key-name))"/>         
    <xsl:if test="not($terminfo//status/@type='DONE')">
      <xsl:message terminate="yes">(400) <xsl:value-of select="$terminfo//status/@type"/>: <xsl:value-of select="$terminfo//status"/> with URL <xsl:value-of select="concat($url,' and code',$key-name)"/></xsl:message>
    </xsl:if>
    <!-- Terminate if code not found -->
    <xsl:if test="count($terminfo/descendant::concept[starts-with(descendant::key,$rootkey)])=0">
      <xsl:comment><xsl:value-of select="$key-name"/> term info not found. :(</xsl:comment>
      <xsl:comment>URL: <xsl:value-of select="$url"/> with rootkey <xsl:value-of select="$rootkey"/></xsl:comment>
      <xsl:message terminate="yes">(400) Term info <xsl:value-of select="$key-name"/> not found; URL: <xsl:value-of select="$url"/> with rootkey <xsl:value-of select="$rootkey"/></xsl:message>
    </xsl:if>
    
    <!-- Output either the first code or the first non-modifier -->
    <xsl:choose>
      <xsl:when test="$output-opts='nomods'">
        <xsl:copy-of select="$terminfo/descendant::concept[starts-with(descendant::key,$rootkey)][facttablecolumn!='modifier_cd'][1]"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="$terminfo/descendant::concept[starts-with(descendant::key,$rootkey)][1]"/> 
      </xsl:otherwise>
    </xsl:choose>
             
  </xsl:template>
  
    <!-- Use the getCodeInfo function from my Java service to communicate with ONT -->
    <xsl:template name="getCodeInfo">
      <xsl:param name="i2b2-precode" select="current()"/>
      <xsl:param name="type"/>
 
      <!-- Extract the code system from the config file -->
      <xsl:variable name="metacodesys-rtf">
        <xsl:for-each select="$metaconfig/mc:metaConfig/mc:codeSystems/mc:item[@codeSystem=$i2b2-precode/@codeSystem]">
          <xsl:choose>
            <xsl:when test="not(attribute::test-reverse)">
              <xsl:copy-of select="current()"/>
            </xsl:when>            
            <xsl:when test="attribute::test-reverse">
              <xsl:if test="dyn:evaluate(attribute::test-reverse)">
                <xsl:copy-of select="current()"/>
              </xsl:if>
            </xsl:when>
          </xsl:choose>
        </xsl:for-each>
      </xsl:variable>
      <xsl:variable name="metacodesys" select="xalan:nodeset($metacodesys-rtf)"/>
      <xsl:if test="not($metacodesys/mc:item) and not($i2b2-precode/@valueSet)"><xsl:message terminate="yes">(501) Undefined code system <xsl:value-of select="$i2b2-precode/@codeSystem"/>
      </xsl:message></xsl:if>
            
      <!-- Translate HQMF code to I2B2 basecode -->
      <!-- Some special casing for rootkeys \\I2B2 and \\i2b2_ -->
      <xsl:variable name="i2b2-code">
        <xsl:if test="(ancestor-or-self::hl7v3:observationCriteria/child::hl7v3:criteriaType='Demographics' or ancestor-or-self::hl7v3:encounterCriteria) and starts-with(translate($rootkey, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'\\I2B2')">DEM|</xsl:if>
        <xsl:if test="(ancestor-or-self::hl7v3:observationCriteria/child::hl7v3:criteriaType='Problems') and starts-with(translate($rootkey, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'\\I2B2_')">DIAG|</xsl:if>
        <xsl:if test="$i2b2-precode/@codeSystem and starts-with(translate($rootkey, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'\\I2B2_')"><xsl:value-of select="$metacodesys/mc:item/@basecode"/>:<xsl:value-of select="translate($i2b2-precode/@code, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/></xsl:if>
        <xsl:if test="$i2b2-precode/@codeSystem and not(starts-with(translate($rootkey, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'\\I2B2_'))"><xsl:value-of select="$metacodesys/mc:item/@basecode"/>:<xsl:value-of select="$i2b2-precode/@code"/></xsl:if>
        <xsl:if test="$i2b2-precode/@valueSet">OID:<xsl:value-of select="$i2b2-precode/@valueSet"/></xsl:if>
      </xsl:variable>                        

      <!-- Lookup the i2b2 code info from the ONT cell -->
      <xsl:variable name="urlservice">getCodeInfo</xsl:variable>
      <xsl:variable name="url">
        <xsl:if test="$sessiontoken=''"><xsl:value-of select="concat($serviceurl,'/hqmf/',$urlservice,'/',$userdomain,'/',$userproject,'/',$username,'/password/',$userpassword)"/></xsl:if>
        <xsl:if test="not($sessiontoken='')"><xsl:value-of select="concat($serviceurl,'/hqmf/',$urlservice,'/',$userdomain,'/',$userproject,'/',$username,'/token/SessionKey:',$sessiontoken)"/></xsl:if>
      </xsl:variable>
      <xsl:variable name="i2b2-concept-rtf"> <!-- This is an unfortunate but necessary way to put result-tree fragments backtogether. -->
        <xsl:choose>
          <xsl:when test="$rootkey='\\SHRINE'">
            <xsl:copy-of select="document(concat($url,'?key=',normalize-space(concat('SHRINE%7C',$i2b2-code))))"/>                                                                                                             
          </xsl:when> 
          <xsl:when test="$rootkey='\\'">                                      
            <xsl:copy-of select="$concepts//concept[basecode=normalize-space($i2b2-code)]"/>     
            <xsl:copy-of select="$concepts//concept[basecode=normalize-space(concat('SHRINE|',$i2b2-code))]"/>                                                                                                             
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="document(concat($url,'?key=',normalize-space($i2b2-code)))"/>                                       
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <!-- TODO: I hard code looking for the first matching concept in the \\I2B2 tree... -->
      <xsl:variable name="i2b2-concepts" select="xalan:nodeset($i2b2-concept-rtf)"/>
      <xsl:if test="not($i2b2-concepts//status/@type='DONE')">
        <xsl:message terminate="yes">(404) <xsl:value-of select="$i2b2-concepts//status/@type"/>: <xsl:value-of select="$i2b2-concepts//status"/> with URL <xsl:value-of select="concat($url,' and code',$i2b2-code)"/></xsl:message>
      </xsl:if>
      
      <!-- Terminate if code not found -->
      <xsl:if test="count($i2b2-concepts/descendant::concept[starts-with(descendant::key,$rootkey)])=0">
        <xsl:comment><xsl:value-of select="$i2b2-code"/> not found. :(</xsl:comment>
        <xsl:comment>URL: <xsl:value-of select="$url"/> with rootkey <xsl:value-of select="$rootkey"/></xsl:comment>
        <xsl:message terminate="yes">(400) <xsl:value-of select="$i2b2-code"/> not found; URL: <xsl:value-of select="$url"/> with rootkey <xsl:value-of select="$rootkey"/></xsl:message>
      </xsl:if>
      
      <!-- Note hack: Disable type searching if the result is no keys. Used for extensions that are used in multiple places. -->
      <xsl:choose>
        <xsl:when test="not($type) or count($i2b2-concepts/descendant::concept[starts-with(descendant::key,$rootkey)][contains(descendant::key,$type)])=0"><xsl:copy-of select="$i2b2-concepts/descendant::concept[starts-with(descendant::key,$rootkey)][1]"/></xsl:when>
        <xsl:otherwise><xsl:copy-of select="$i2b2-concepts/descendant::concept[starts-with(descendant::key,$rootkey)][contains(descendant::key,$type)]"/></xsl:otherwise>
      </xsl:choose>
      
    </xsl:template>
  
    <xsl:template name="getAge">
      <xsl:param name="subtype"/>
      <xsl:param name="type"/>

      <!-- Create the i2b2 code element for age ranges. Single years are converted to a code lookup. Ranges are converted to a range name for name lookup. -->
        <xsl:if test="not(current()/@xsi:type='IVL_PQ' or current()/hl7v3:low or current()/hl7v3:high or current()/hl7v3:low/@unit='a' or current()/hl7v3:high/@unit='a')">
          <xsl:message terminate="yes">(501) Age constraints of BETWEEN only supported!</xsl:message>
        </xsl:if>
        <xsl:variable name="i2b2-precode-rtf">
          <xsl:choose>
            <!-- One year range: code lookup -->
            <xsl:when test="current()/hl7v3:low/@value+1=current()/hl7v3:high/@value and current()/hl7v3:high/@unit='a' and current()/hl7v3:low/@unit='a'">
              <xsl:if test="not(current()/hl7v3:low/@inclusive='true') or not(current()/hl7v3:high/@inclusive='false')">
                <xsl:message terminate="yes">(501) Single year age ranges must be inclusive to non-inclusive!</xsl:message>
              </xsl:if>
              <hl7v3:code>
                <xsl:attribute name="code">
                  <xsl:value-of select="current()/hl7v3:low/@value"/>    
                </xsl:attribute>
                <xsl:attribute name="codeSystem">AGE</xsl:attribute>                                
              </hl7v3:code>                                       
            </xsl:when> 
            <!-- Multi-year ranges -->
            <xsl:otherwise>
              <xsl:variable name="agerange">
                <xsl:if test="current()/hl7v3:low/@inclusive='false'"><xsl:value-of select="current()/hl7v3:low/@value +1"/></xsl:if>
                <xsl:if test="current()/hl7v3:low/@inclusive='true'"><xsl:value-of select="current()/hl7v3:low/@value"/></xsl:if>
                <xsl:text>-</xsl:text>
                <xsl:if test="current()/hl7v3:high/@inclusive='false'"><xsl:value-of select="current()/hl7v3:high/@value -1"/></xsl:if>
                <xsl:if test="current()/hl7v3:high/@inclusive='true'"><xsl:value-of select="current()/hl7v3:high/@value"/></xsl:if>
              </xsl:variable>
              <hl7v3:code>
                <xsl:attribute name="code">
                  <xsl:value-of select="$agerange"/>    
                </xsl:attribute>
                <xsl:attribute name="codeSystem">AGERANGE</xsl:attribute>                                
              </hl7v3:code>                                          
            </xsl:otherwise>
          </xsl:choose> 
        </xsl:variable>
       <xsl:variable name="i2b2-precode" select="xalan:nodeset($i2b2-precode-rtf)/hl7v3:code"/>
      
      <!-- Either look up a single year age or do the specialized bucket handling -->
      <xsl:choose>
        <!-- Single age. -->
        <xsl:when test="$i2b2-precode/@codeSystem='AGE'">
          <xsl:call-template name="getCodeInfo">
            <xsl:with-param name="i2b2-precode" select="$i2b2-precode"/>
          </xsl:call-template>         
        </xsl:when>
        <xsl:otherwise>
          <!-- Age range -->
          <xsl:variable name="url">
            <xsl:if test="$sessiontoken=''"><xsl:value-of select="concat($serviceurl,'/hqmf/getTermInfo/',$userdomain,'/',$userproject,'/',$username,'/password/',$userpassword)"/></xsl:if>
            <xsl:if test="not($sessiontoken='')"><xsl:value-of select="concat($serviceurl,'/hqmf/getTermInfo/',$userdomain,'/',$userproject,'/',$username,'/token/SessionKey:',$sessiontoken)"/></xsl:if>
          </xsl:variable> 
          <!-- todo: Slight hack, add DEMO extension if rootkey is \\i2b2_ -->
          <xsl:variable name="rootext"><xsl:if test="substring($rootkey,3)='i2b2_'">DEMO</xsl:if></xsl:variable>
          <xsl:variable name="agerange" select="document(concat($url,'?key=%5C%5C',substring($rootkey,3),$rootext,'%5C%25',$subtype,'%5C',$i2b2-precode/@code,'%20years%20old%5C'))"/>         
          <xsl:if test="not($agerange//status/@type='DONE')">
            <xsl:message terminate="yes"><xsl:value-of select="$agerange//status/@type"/>: <xsl:value-of select="$agerange//status"/> with URL <xsl:value-of select="concat($url,' and code',$agerange)"/></xsl:message>
          </xsl:if>
          <!-- Terminate if code not found -->
          <xsl:if test="count($agerange/descendant::concept[starts-with(descendant::key,$rootkey)])=0">
            <xsl:comment><xsl:value-of select="$i2b2-precode/@code"/> age range not found. :(</xsl:comment>
            <xsl:comment>URL: <xsl:value-of select="$url"/> with rootkey <xsl:value-of select="$rootkey"/></xsl:comment>
            <xsl:message terminate="yes">(400) Age range <xsl:value-of select="$i2b2-precode/@code"/> not found; URL: <xsl:value-of select="$url"/> with rootkey <xsl:value-of select="$rootkey"/></xsl:message>
          </xsl:if>
          <xsl:copy-of select="$agerange/descendant::concept[starts-with(descendant::key,$rootkey)][1]"/>          
        </xsl:otherwise>
      </xsl:choose>
    </xsl:template>
 
    <!-- Convert IVL_PQ width values to a high and low 
        This is really a meager effort and a better normalizer should be written.
        The only option is a width in years with no high or low. -->
    <xsl:template match="hl7v3:value">
        <xsl:choose>
            <xsl:when test="./hl7v3:width">
                <xsl:if test="hl7v3:width/@unit!='a'">
                    <xsl:message terminate="yes">(501) Only timetamp widths in year multiples are supported.</xsl:message>
                </xsl:if>
                <hl7v3:low value="{number(substring($now,1,8))-(10000*hl7v3:width/@value)}"/> 
                <hl7v3:high value="{$now}"/>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="."/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <!-- Constrain_by_date 
        Assumes that effectiveTime has only a high/low element.
        Also assumes that if an effectiveTime exists, it completely overrides the measurePeriod. This might be wrong? -->
    <xsl:template name="constrain_by_date">
        <xsl:choose>
            <xsl:when test="hl7v3:effectiveTime">
                <constrain_by_date>
                    <date_from>
                        <xsl:call-template name="convert_date"><xsl:with-param name="input_date" select="hl7v3:effectiveTime/hl7v3:low/@value"/></xsl:call-template>           
                    </date_from>
                    <date_to>
                        <xsl:call-template name="convert_date"><xsl:with-param name="input_date" select="hl7v3:effectiveTime/hl7v3:high/@value"/></xsl:call-template>  
                    </date_to>
                </constrain_by_date>
            </xsl:when>
            <xsl:when test="count(hl7v3:timeRelationship/hl7v3:measurePeriodTimeReference)>0">
                <!-- TODO: Actually process the timeQuantity and timeRelationshipCode -->
                <constrain_by_date>
                    <date_from>
                        <xsl:call-template name="convert_date"><xsl:with-param name="input_date" select="$measurePeriod/hl7v3:low/@value"/></xsl:call-template>           
                    </date_from>
                    <date_to>
                        <xsl:call-template name="convert_date"><xsl:with-param name="input_date" select="$measurePeriod/hl7v3:high/@value"/></xsl:call-template>  
                    </date_to>
                </constrain_by_date>                
            </xsl:when>
          <xsl:when test="ancestor-or-self::hl7v3:observationCriteria/child::hl7v3:criteriaType='Demographics' and ancestor-or-self::hl7v3:observationCriteria/hl7v3:code/@code='424144002'">
                <xsl:message terminate="no">
                    Default date constraints on ages need to be handled by adjusting the base age
                    and are currently ignored.
                </xsl:message>
                <xsl:comment>Default date constraints on ages need to be handled by adjusting the base age
                    and are currently ignored.</xsl:comment>
            </xsl:when>
            <xsl:otherwise>
                <!-- Process measurePeriod -->
                <constrain_by_date>
                    <date_from>
                        <xsl:call-template name="convert_date"><xsl:with-param name="input_date" select="$measurePeriod/hl7v3:low/@value"/></xsl:call-template>           
                    </date_from>
                    <date_to>
                        <xsl:call-template name="convert_date"><xsl:with-param name="input_date" select="$measurePeriod/hl7v3:high/@value"/></xsl:call-template>  
                    </date_to>
                </constrain_by_date>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <!-- Helper function converts dates from ISO/HL7 format to XMLSchema dateTime format -->
    <xsl:template name="convert_date">
        <xsl:param name="input_date"/>
        <xsl:value-of select="concat(substring($input_date,1,4),'-',substring($input_date,5,2),'-',substring($input_date,7,2),'Z')"/>
    </xsl:template>

    
    <!-- Constrain_by_value on lab results. -->
    <xsl:template name="contrain_by_value">
        <constrain_by_value>
            <xsl:choose>
                <xsl:when test="current()/@xsi:type='CD'">
                  <value_operator>EQ</value_operator>
                  <value_constraint>
                    <xsl:choose>
                      <xsl:when test="current()/@code='62482003'">L</xsl:when>
                    </xsl:choose>
                    <xsl:choose>
                      <xsl:when test="current()/@code='75540009'">H</xsl:when>
                    </xsl:choose>
                    <xsl:choose>
                      <xsl:when test="current()/@code='260394003'">N</xsl:when>
                    </xsl:choose>             
                  </value_constraint>
                  <value_unit_of_measure>%</value_unit_of_measure>
                  <value_type>FLAG</value_type>
                </xsl:when>
                <xsl:when test="current()/@xsi:type='IVL_PQ'">
                    <value_type>NUMBER</value_type>
                    <xsl:choose>
                        <xsl:when test="current()/hl7v3:low and not(current()/hl7v3:high)"> <!-- GT and GE -->
                            <value_unit_of_measure><xsl:value-of select="current()/hl7v3:low/@unit"/></value_unit_of_measure>
                            <value_constraint><xsl:value-of select="current()/hl7v3:low/@value"/></value_constraint>
                            <value_operator> 
                                <xsl:if test="current()/hl7v3:low/@inclusive='false'">GT</xsl:if>
                                <xsl:if test="current()/hl7v3:low/@inclusive='true'">GE</xsl:if>
                            </value_operator>  
                        </xsl:when>
                        <xsl:when test="current()/hl7v3:high and not(current()/hl7v3:low)"> <!-- LT and LE -->
                            <value_unit_of_measure><xsl:value-of select="current()/hl7v3:high/@unit"/></value_unit_of_measure>
                            <value_constraint><xsl:value-of select="current()/hl7v3:high/@value"/></value_constraint>
                            <value_operator> 
                                <xsl:if test="current()/hl7v3:high/@inclusive='false'">LT</xsl:if>
                                <xsl:if test="current()/hl7v3:high/@inclusive='true'">LE</xsl:if>
                            </value_operator> 
                        </xsl:when>
                        <xsl:when test="current()/hl7v3:low and current()/hl7v3:high"> <!-- BETWEEN -->
                            <xsl:if test="current()/hl7v3:low/@unit != current()/hl7v3:high/@unit"><xsl:message terminate="yes">(400) Units do not agree in interval!</xsl:message></xsl:if>
                            <value_unit_of_measure><xsl:value-of select="current()/hl7v3:high/@unit"/></value_unit_of_measure>
                            <value_constraint><xsl:value-of select="current()/hl7v3:low/@value"/> and <xsl:value-of select="current()/hl7v3:high/@value"/></value_constraint>
                            <value_operator>BETWEEN</value_operator> <!-- TODO: Not checking inclusiveness pattern is supported -->
                        </xsl:when>
                        <xsl:otherwise> 
                            <xsl:message terminate="yes">(501) Unsupported constraint type!</xsl:message>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:when test="current()/@xsi:type='PQ'"> <!-- EQ -->
                    <constrain_by_value>
                        <value_type>NUMBER</value_type>
                        <value_unit_of_measure><xsl:value-of select="current()/@unit"/></value_unit_of_measure>
                        <value_operator>EQ</value_operator>
                        <value_constraint><xsl:value-of select="current()/@value"/></value_constraint>
                    </constrain_by_value>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:message terminate="yes">(501) Unclear why constrain by value was called for this unsupported type: <xsl:value-of select="current()/@xsi:type"/></xsl:message>
                </xsl:otherwise>
            </xsl:choose>
        </constrain_by_value>
    </xsl:template>
    
    <!-- Top-level output -->
    <xsl:template match="/hl7v3:ihqmf">
        <xsl:if test="$fullquery='true'">
	        <ns6:request xmlns:ns4="http://www.i2b2.org/xsd/cell/crc/psm/1.1/" xmlns:ns7="http://www.i2b2.org/xsd/cell/ont/1.1/" xmlns:ns3="http://www.i2b2.org/xsd/cell/crc/pdo/1.1/" xmlns:ns5="http://www.i2b2.org/xsd/hive/plugin/" xmlns:ns2="http://www.i2b2.org/xsd/hive/pdo/1.1/" xmlns:ns6="http://www.i2b2.org/xsd/hive/msg/1.1/" xmlns:ns8="http://www.i2b2.org/xsd/cell/crc/psm/querydefinition/1.1/">
	            <message_header>
	                <security> <!-- May use <ticket/> instead of <domain/>, <username/> & <password/> -->
	                    <domain><xsl:value-of select="$userdomain"/></domain>
	                    <username><xsl:value-of select="$username"/></username>
	                    <xsl:if test="$sessiontoken!=''">
	                        <password token_ms_timeout="1800000" is_token="true">SessionKey:<xsl:value-of select="$sessiontoken"/></password> 
	                    </xsl:if>
	                    <xsl:if test="$sessiontoken=''">
	                        <password token_ms_timeout="1800000" is_token="false"><xsl:value-of select="$userpassword"/></password> 
	                    </xsl:if>                    
	                </security>
	                <project_id><xsl:value-of select="$userproject"/></project_id>
	            </message_header>
	            <request_header>
	                <result_waittime_ms>180000</result_waittime_ms> <!-- Must send back 'DONE', 'PENDING' or 'ERROR' within this time -->
	            </request_header>
	            <message_body> 
	                <ns4:psmheader>
	                    <user>
	                        <xsl:attribute name="login"><xsl:value-of select="$username"/></xsl:attribute>
	                        <xsl:attribute name="group"><xsl:value-of select="$userproject"/></xsl:attribute>
	                        <xsl:value-of select="$username"/>
	                    </user>
	                    <patient_set_limit>0</patient_set_limit>
	                    <estimated_time>0</estimated_time>
	                    <query_mode>optimize_without_temp_table</query_mode>
	                    <request_type>CRC_QRY_runQueryInstance_fromQueryDefinition</request_type>
	                </ns4:psmheader>
	                    <ns4:request xsi:type="ns4:query_definition_requestType" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                        <query_definition>
                            <query_name><xsl:value-of select="hl7v3:title"/></query_name>
                            <query_timing>ANY</query_timing> <!-- Should work with measurePeriod -->
                            <specificity_scale>0</specificity_scale>
                            <xsl:apply-templates select="hl7v3:populationCriteria"/>
                        </query_definition>
                    </ns4:request>
                </message_body>
            </ns6:request>
          </xsl:if>
          <xsl:if test="not($fullquery='true')">
            <query_definition>
                <query_name><xsl:value-of select="hl7v3:title"/></query_name>
                <query_timing>ANY</query_timing> <!-- Should work with measurePeriod -->
                <specificity_scale>0</specificity_scale>
                <xsl:apply-templates select="hl7v3:populationCriteria"/>
            </query_definition>
          </xsl:if>
    </xsl:template>
    
    <xsl:template match="hl7v3:populationCriteria">
        <xsl:if  test="count(hl7v3:dataCriteriaCombiner) != 1">
            <xsl:message terminate="yes">(501) Non-i2b2 dataCriteriaCombiner structure - too many outer combiners (<xsl:value-of select="count(dataCriteriaCombiner)"/>)!</xsl:message>
        </xsl:if>
        <xsl:if  test="hl7v3:dataCriteriaCombiner[1]/hl7v3:criteriaOperation != 'AllTrue'">
            <xsl:message terminate="yes">(501) Non-i2b2 dataCriteriaCombiner structure - outer combiner must be AllTrue (not <xsl:value-of select="hl7v3:dataCriteriaCombiner[1]/hl7v3:criteriaOperation"/>)!</xsl:message>
        </xsl:if>
        <xsl:apply-templates select="hl7v3:dataCriteriaCombiner/hl7v3:dataCriteriaCombiner"/>            
    </xsl:template>

    <!-- Panel -->
    <xsl:template match="hl7v3:dataCriteriaCombiner/hl7v3:dataCriteriaCombiner">
        <xsl:if test="hl7v3:criteriaOperation!='AtLeastOneTrue' and hl7v3:criteriaOperation!='AllFalse'">
            <xsl:message terminate="yes">(501) Non-i2b2 dataCriteriaCombiner structure - inner combiners must be AtLeastOneTrue or AllFalse! (not <xsl:value-of select="hl7v3:criteraOperation"/>)</xsl:message>
        </xsl:if>
        
        <!-- Handle filter and global time criteria. -->
        <xsl:variable name="timestuff-rtf">
            <xsl:apply-templates mode="time" select="key('dataCriteria',hl7v3:dataCriteriaReference/@extension)/.."></xsl:apply-templates>
        </xsl:variable>
        <xsl:variable name="timestuff" select="xalan:nodeset($timestuff-rtf)"/>
        
        <!-- Panel-wide minimum count -->
        <xsl:variable name="mincount">
            <xsl:choose>
                <xsl:when test="count($timestuff/VALUE)>0">
                    <xsl:choose>
                        <xsl:when test="sum($timestuff/VALUE) = $timestuff/VALUE[1]*count($timestuff/VALUE)">
                            <xsl:value-of select="$timestuff/VALUE[1]"/>
                        </xsl:when>
                        <xsl:otherwise>
                            <xsl:message terminate="yes">
                                (501) i2b2 requires all filterCriteria within a panel to be the same!
                            </xsl:message>
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:otherwise>1</xsl:otherwise>
            </xsl:choose>
        </xsl:variable> 
        
        <!-- Check all temporal constraints are within one panel
              TODO: We do not check that each item also mentions all items in the panel... -->
        <xsl:for-each select="$timestuff/REF">
            <xsl:choose>    
                <xsl:when test="hl7v3:dataCriteriaReference[@extension=current()]">
                    <xsl:message>Temporal constraint ok!</xsl:message>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:message terminate="yes">
                        (501) Cannot process time constraint - referencing across panels!
                    </xsl:message>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:for-each>
        <xsl:variable name="panel-timing">
            <xsl:choose>
                <xsl:when test="count($timestuff/REF)>0">SAMEVISIT</xsl:when>
                <xsl:otherwise>ANY</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
        
        <panel>
            <panel_number><xsl:value-of select="1+count(preceding-sibling::hl7v3:dataCriteriaCombiner)"/></panel_number>
            <panel_accuracy_scale>100</panel_accuracy_scale>
            <invert>
                <xsl:choose>
                    <xsl:when test="hl7v3:criteriaOperation='AtLeastOneTrue'">0</xsl:when>
                    <xsl:when test="hl7v3:criteriaOperation='AllFalse'">1</xsl:when>
                </xsl:choose>
            </invert>
            <panel_timing><xsl:value-of select="$panel-timing"/></panel_timing> <!-- Todo: should at least play with measurePeriod -->
            <total_item_occurrences><xsl:value-of select="$mincount"/></total_item_occurrences>
            <xsl:apply-templates mode="item" select="key('dataCriteria',hl7v3:dataCriteriaReference/@extension)/.."></xsl:apply-templates>
        </panel>
    </xsl:template>

    <!-- Handle panel-wide temporal constraints -->
    <xsl:template mode="time" match="*">
        <xsl:if test="./hl7v3:timeRelationship/hl7v3:dataCriteriaTimeReference">
            <xsl:choose>
                <xsl:when test="count(./hl7v3:timeRelationship/hl7v3:timeQuantity)=0"> <!-- TODO: Also check the time relationship is one that means 'overlaps' -->
                    <REF><xsl:value-of select="./hl7v3:timeRelationship/hl7v3:dataCriteriaTimeReference/@extension"/></REF>                    
                </xsl:when>
                <xsl:otherwise>
                    <xsl:message terminate="yes">(501) Illegal panel-wide time relationship; only same encounter is supported.</xsl:message>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:if>
        <xsl:if test="./hl7v3:filterCriteria/hl7v3:filterCode">
            <xsl:choose>
                <xsl:when test="./hl7v3:filterCriteria/hl7v3:filterCode = 'MIN' and ./hl7v3:filterCriteria/hl7v3:repeatNumber">
                    <VALUE><xsl:value-of select="./hl7v3:filterCriteria/hl7v3:repeatNumber/@value"/></VALUE>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:message terminate="yes">
                        (501) The only filterCriteria supported are minimum repeat numbers!
                    </xsl:message>
                </xsl:otherwise>
            </xsl:choose>
        </xsl:if>
    </xsl:template>
 

    
    <!-- Overhauled version criteria template -->
    <xsl:template mode="item" match="hl7v3:*[hl7v3:criteriaType]">
      <!-- extract the criteria mapping from the metaconfig -->
      <xsl:variable name="metacriteria"
        select="$metaconfig/mc:metaConfig/mc:criteriaMappings/mc:item[@extension=current()/hl7v3:criteriaType][@i2b2_rootkey=$rootkey][1]"/>      
      <xsl:if test="not($metacriteria)"><xsl:message terminate="yes">
        Undefined criteria type <xsl:value-of select="hl7v3:criteriaType"/> in item <xsl:value-of select="hl7v3:localVariableName"/></xsl:message></xsl:if> 
      
      <item>
      
        <!-- Insert the concept elements -->
        <!-- Handle top-level codes -->
        <xsl:apply-templates select="current()/*" mode="get-concept"/>
        <xsl:choose>
          <!-- Handle medication codes -->
          <xsl:when test="$metacriteria/@extension='Medications'">
            <xsl:apply-templates select="hl7v3:participant[@typeCode='CSM']/hl7v3:roleParticipant[@classCode='THER']/*" mode="get-concept"/>
          </xsl:when>
          <!-- Handle provider codes -->
          <xsl:when test="hl7v3:participant[@typeCode='PRF']/hl7v3:qualifiedEntityParticipant[@classCode='PHYS']">
            <xsl:apply-templates select="hl7v3:participant[@typeCode='PRF']/hl7v3:qualifiedEntityParticipant[@classCode='PHYS']/*" mode="get-concept"/>
          </xsl:when>
        </xsl:choose>

        <xsl:call-template name="constrain_by_date"/>

      </item>    
    </xsl:template>
    
</xsl:stylesheet>