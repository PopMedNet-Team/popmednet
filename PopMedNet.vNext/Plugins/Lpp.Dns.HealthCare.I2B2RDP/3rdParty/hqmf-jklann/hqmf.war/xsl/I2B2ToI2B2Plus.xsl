<?xml version="1.0" encoding="UTF-8"?>
<!-- Converts a full i2b2 query request (including headers) into an augmented query definition including
    basecodes, using my web service to call the ontology cell. This simplifies HQMF translation. The main feature
    is that it also recursively searches through the children of all keys without basecodes and adds those children
    with basecodes. It throws an error if it reaches a leaf with no basecode.
    
    Jeff Klann, PhD - 7/19/12
       Initial Version
    Jeff Klann, PhD - 7/27/12
       Bugfix: sanity check on item key, so things like previous query ids are not passed to getTermInfo.
    Jeff Klann, PhD - 7/30/12
       Now looks up basecodes on modifiers also (but does not recurse into children). Uses getTermInfo for 
      simplicity. This assumes a one-to-one mapping between modifier key and basecode and could break in a later
      version of i2b2.
    Jeff Klann, PhD - 9/27/12
       Eliminated the dependence on definining a subkey for age buckets. Now just grabs this info from the metaconfig.
       Fixed a bug where constraint information was not propagated on recursive expansion and basecodes were not looked up
         on modifiers in this situation.
    Jeff Klann, PhD - 10/21/12
       Small bugfix: now treats CEDD basecodes as if there were no basecode
    Jeff Klann, PhD - 10/22/12
       Small bugfix: containers are always unrolled, even if they have a basecode
    Jeff Klann, PhD - 10/22/12
       Small bugfix: CEDD basecodes with constrain_by_value (freetext searches) are no longer ignored.
       
    Todo: should not choose first matching concept for a key, should choose first matching concept with a
     basecode if any exist. However, the likelihood of duplicate keys makes this a minor issue.
     
    Todo: basecodes probably ought to be cached because they're looked up many times in the case of a modifier on a recursive expansion.
-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
    xmlns:xalan="http://xml.apache.org/xalan"
    xmlns:java="http://xml.apache.org/xslt/java"
    xmlns:ont="xalan://edu.harvard.i2b2.eclipse.plugins.ontology.ws.OntServiceDriver"
    xmlns:str="http://exslt.org/strings"
    xmlns:mc="urn:jklann:hqmf:metaConfig"
    extension-element-prefixes="ont"
    version="1.0"
    exclude-result-prefixes="xsi xalan java mc str">
    <xsl:import href="time.xsl"/>
    <xsl:import href="url-encode.xsl"/>
    <xsl:output indent="yes" xalan:indent-amount="2"/>
    <xsl:strip-space elements="*"/>
      
    <!-- The root URL for the webservice. If it is blank, runs locally on concepts.xml -->
    <!-- If not running locally, extracts security parameters from the i2b2 message. -->
    <!--<xsl:param name="serviceurl">http://ec2-23-20-41-242.compute-1.amazonaws.com:9090/jersey</xsl:param>-->
    <xsl:param name="serviceurl">http://localhost:8080</xsl:param>
    
    <!-- Load the metaconfig. -->
    <xsl:variable name="metaconfig" select="document('translatorMetaConfig.xml')"/>
      
    <!-- The security parameters for concept lookup -->
    <xsl:variable name="userdomain" select="//security/domain/text()"/>
    <xsl:variable name="userproject" select="descendant::message_body/descendant::user/@group"/>
    <xsl:variable name="username" select="//security/username/text()"/>
    <xsl:variable name="userpassword"><xsl:if test="not(//security/password/@is_token='true')"><xsl:value-of select="//security/password/text()"/></xsl:if></xsl:variable>
    <xsl:variable name="sessiontoken"><xsl:if test="//security/password/@is_token='true'"><xsl:value-of select="substring-after(//security/password/text(),'SessionKey:')"/></xsl:if></xsl:variable>
    
    <!-- Build the URLs -->
    <xsl:variable name="getTermInfoUrl">
        <xsl:if test="$sessiontoken=''"><xsl:value-of select="concat($serviceurl,'/hqmf/getTermInfo/',$userdomain,'/',$userproject,'/',$username,'/password/',$userpassword)"/></xsl:if>
        <xsl:if test="not($sessiontoken='')"><xsl:value-of select="concat($serviceurl,'/hqmf/getTermInfo/',$userdomain,'/',$userproject,'/',$username,'/token/SessionKey:',$sessiontoken)"/></xsl:if>
    </xsl:variable>
    <xsl:variable name="getChildrenUrl">
        <xsl:if test="$sessiontoken=''"><xsl:value-of select="concat($serviceurl,'/hqmf/getChildren/',$userdomain,'/',$userproject,'/',$username,'/password/',$userpassword)"/></xsl:if>
        <xsl:if test="not($sessiontoken='')"><xsl:value-of select="concat($serviceurl,'/hqmf/getChildren/',$userdomain,'/',$userproject,'/',$username,'/token/SessionKey:',$sessiontoken)"/></xsl:if>
    </xsl:variable>
    
    <!-- override XSL default rules for text() nodes -->
    <xsl:template match="text()"/>
    
    <!-- 
        create a variable to access local I2B2 Concepts for testing
    -->
    <xsl:variable name="concepts-test" select="document('concepts.xml')"/>
    
    <!-- These two templates copy the query_definition -->
    <xsl:template match="//query_definition">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()" mode="process"/>
        </xsl:copy>
    </xsl:template>
    
    <xsl:template match="@*|node()" mode="process">
        <xsl:copy>
            <xsl:apply-templates select="@*|node()" mode="process"/>
        </xsl:copy>
    </xsl:template>
    
    <!-- This template deals with modifiers -->
    <xsl:template match="constrain_by_modifier" mode="process">   
        <xsl:choose>
            <!-- Verify the item key is sane. Otherwise i2b2 seems to get stuck forever. -->
            <xsl:when test="not(starts-with(modifier_key,'\\'))">
                <xsl:message terminate="yes">(400) Key <xsl:value-of select="modifier_key"/> is not valid.</xsl:message>
            </xsl:when>
            
            <xsl:otherwise>
                <!-- Look up the basecode. We use the get-concept template rather than the more appropriate
                    modifier ontology calls. In the current version of I2B2 (flavors of 1.6) this works the 
                    same but doesn't filter on the applied path. If we assume there aren't modifiers with 
                    the same key in different paths (ok for CEDD), this works. -->
                <xsl:variable name="myconcept">
                    <xsl:call-template name="get-concept">
                        <xsl:with-param name="item_key" select="modifier_key"/>
                    </xsl:call-template> 
                </xsl:variable>
                
                <xsl:choose>
                    <!-- We must check for both a missing basecode and an empty code portion of the basecode, because e.g. SHRINE age folders have these empty. -->
                    <xsl:when test="xalan:nodeset($myconcept)/basecode and not(substring-after(xalan:nodeset($myconcept)/basecode,':')='')">
                        <constrain_by_modifier>
                            <xsl:apply-templates mode="process"/>
                            <xsl:copy-of select="$myconcept"/>
                        </constrain_by_modifier>               
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:message terminate="yes">(400) <xsl:value-of select="modifier_key"/> has no basecode, and modifier children unrolling are not supported!</xsl:message>
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
    
    <!-- This template modifies items -->
    <xsl:template match="item" mode="process">   
      <!-- Extract various parts of the ontology from item_key -->
      <xsl:variable name="split_item_key" select="str:split(substring-after(normalize-space(item_key),'\\'),'\')"/>    
      <xsl:variable name="subtype_split" select="str:split($split_item_key[4]/text(),'_')"/>
      <xsl:variable name="subtype" select="$subtype_split[1]/text()"/>
      
        <xsl:choose>
           <!-- Verify the item key is sane. Otherwise i2b2 seems to get stuck forever. -->
           <xsl:when test="not(starts-with(item_key,'\\'))">
               <xsl:message terminate="yes">(400) Key <xsl:value-of select="item_key"/> is not valid.</xsl:message>
           </xsl:when>
           
           <xsl:otherwise>
           <!-- Look up the basecode -->
           <xsl:variable name="myconcept">
               <xsl:call-template name="get-concept">
                   <xsl:with-param name="item_key" select="item_key"/>
               </xsl:call-template> 
           </xsl:variable>
           
           <xsl:choose>
               <!-- We must check for both a missing basecode and an empty code portion of the basecode, because e.g. SHRINE age folders have these empty. -->
               <!-- CEDD basecodes that aren't freetext searches are treated as no basecode. -->
               <xsl:when test="xalan:nodeset($myconcept)/basecode and not(substring-after(xalan:nodeset($myconcept)/basecode,':')='' or (starts-with(xalan:nodeset($myconcept)/basecode,'CEDD:') and not(constrain_by_value)))">
                   <item>
                       <xsl:apply-templates mode="process"/>
                       <xsl:copy-of select="$myconcept"/>
                   </item>               
               </xsl:when>
               <!-- Special handling for age ranges -->
             <xsl:when test="$metaconfig/mc:metaConfig/mc:demographicCodes/mc:item[@i2b2_subtype=$subtype][1]/@code='424144002' 
                 and (not(xalan:nodeset($myconcept)/basecode) or substring-after(xalan:nodeset($myconcept)/basecode,':')='')">
                   <xsl:comment>AGE RANGE DETECTED - LEAVING ALONE</xsl:comment>
                   <item>
                       <xsl:apply-templates mode="process"/>
                   </item>                 
               </xsl:when>
               <xsl:otherwise>
                   <xsl:comment>NO BASECODE - ADDING CHILDREN</xsl:comment> 
                   <xsl:call-template name="get-children">
                       <xsl:with-param name="item-orig" select="current()"/>
                       <xsl:with-param name="key-current" select="item_key"/>
                   </xsl:call-template>
               </xsl:otherwise>
           </xsl:choose>
           </xsl:otherwise>
        </xsl:choose>
    </xsl:template>
     
    <!-- 
        a template for I2B2 children lookup.
    -->
    <xsl:template name="get-children">
        <xsl:param name="item-orig"/>
        <xsl:param name="key-current"></xsl:param>
        
        <!-- URLencode the item key -->
        <xsl:variable name="item_key2">
            <xsl:call-template name="url-encode">
                <xsl:with-param name="str" select="normalize-space($key-current)"/>
            </xsl:call-template>
        </xsl:variable>
        
        <!-- Locate the concept in the i2b2 ontology, either with the web service, or
            the local file if in test mode. -->      
        <xsl:variable name="concept-rtf">
            <xsl:if test="not($serviceurl='')">
                <xsl:copy-of select="document(concat($getChildrenUrl,'?key=',normalize-space($item_key2)))"/> 
            </xsl:if>
            <xsl:if test="$serviceurl=''">
                <xsl:message terminate="yes">(500) Basecode not found when in local test mode; unsupported.</xsl:message>
            </xsl:if>
        </xsl:variable>
        <xsl:variable name="concepts" select="xalan:nodeset($concept-rtf)"/>
        <xsl:if test="not($concepts//status/@type='DONE')">
            <xsl:message terminate="yes">(400) <xsl:value-of select="$concepts//status/@type"/>: <xsl:value-of select="$concepts//status"/> for URL <xsl:value-of select="concat($getChildrenUrl,'?key=',normalize-space($item_key2))"/></xsl:message>
        </xsl:if>
        <xsl:if test="count($concepts//concept)=0">
            <xsl:message terminate="yes">(400) <xsl:value-of select="$key-current"/> has no children and no basecode!</xsl:message>
        </xsl:if>
            
        <!-- Insert the key, basecode and name into the query definition -->
        <xsl:for-each select="$concepts//concept">
            <xsl:choose>
                <xsl:when test="basecode and not(substring-after(basecode,':')='' or starts-with(basecode,'CEDD:') or starts-with(visualattributes,'CA'))">
                    <item>  
                        <basecode><xsl:value-of select="basecode"/></basecode>
                        <item_name><xsl:value-of select="name"/></item_name>
                        <item_key><xsl:value-of select="key"/></item_key>
                        <xsl:copy-of select="$item-orig/constrain_by_value"/>
                        <xsl:apply-templates select="$item-orig/constrain_by_modifier" mode="process"/>             
                    </item>
                </xsl:when>
                <xsl:otherwise>
                    <xsl:message terminate="no">Recursing on <xsl:value-of select="key"/></xsl:message>
                    <xsl:call-template name="get-children">
                        <xsl:with-param name="item-orig" select="$item-orig"></xsl:with-param> 
                        <xsl:with-param name="key-current" select="key"></xsl:with-param>
                    </xsl:call-template>
                </xsl:otherwise>
            </xsl:choose>           
        </xsl:for-each>
      </xsl:template>
     
    <!-- 
        a template for I2B2 concept lookup.
    -->
    <xsl:template name="get-concept">
        <xsl:param name="item_key"/>
        
        <!-- URLencode the item key -->
        <xsl:variable name="item_key2">
            <xsl:call-template name="url-encode">
                <xsl:with-param name="str" select="normalize-space($item_key)"/>
            </xsl:call-template>
        </xsl:variable>
        
        <!-- Locate the concept in the i2b2 ontology, either with the web service, or
            the local file if in test mode. -->
    
        <xsl:variable name="concept-rtf">
            <xsl:if test="not($serviceurl='')">
                <xsl:copy-of select="document(concat($getTermInfoUrl,'?key=',normalize-space($item_key2)))"/>
                
            </xsl:if>
            <xsl:if test="$serviceurl=''">
                <xsl:copy-of select="$concepts-test//concept[key = normalize-space($item_key)]"/>
            </xsl:if>
        </xsl:variable>
        <!-- Arbitrarily choose the first matching concept. -->
        <xsl:variable name="concepts" select="xalan:nodeset($concept-rtf)"/>
        <xsl:if test="not($concepts//status/@type='DONE')">
            <xsl:message terminate="yes">(400) <xsl:value-of select="$concepts//status/@type"/>: <xsl:value-of select="$concepts//status"/> for URL <xsl:value-of select="concat($getChildrenUrl,'?key=',normalize-space($item_key2))"/></xsl:message>
        </xsl:if>
        <xsl:variable name="concept" select="$concepts//concept[1][key = normalize-space($item_key)]"/>
        <xsl:if test="count($concept)=0">
            <xsl:message terminate="yes">(400) <xsl:value-of select="$item_key"/> not found!</xsl:message>
        </xsl:if>
        
        <!-- Insert the basecode and name into the query definition -->
        <xsl:if test="$concept/basecode">
            <basecode><xsl:value-of select="$concept/basecode"/></basecode>
        </xsl:if>
        <xsl:if test="$concept/name">
            <item_name><xsl:value-of select="$concept/name"/></item_name>
        </xsl:if>
    </xsl:template>
    
</xsl:stylesheet>
    
