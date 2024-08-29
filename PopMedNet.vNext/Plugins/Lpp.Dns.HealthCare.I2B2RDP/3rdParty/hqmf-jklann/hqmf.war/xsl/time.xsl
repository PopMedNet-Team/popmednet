<?xml version='1.0' encoding='UTF-8' ?>
<xsl:stylesheet exclude-result-prefixes="java doc" version="1.0"
  xmlns="urn:hl7-org:v3" xmlns:java="http://xml.apache.org/xslt/java"
  xmlns:doc="urn:schemas-cagle-com:document"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <doc:summary>
    <doc:title>CDA Time Functions</doc:title>
    <doc:filename>time.xsl</doc:filename>
    <doc:version>1.0</doc:version>
    <doc:dateCreated>2008-01-30</doc:dateCreated>
    <doc:description> This module creates several variables which are used to
      get at the current time, and timezone. It makes use of Xalan capabilities
      to integrate with Java object. </doc:description>
  </doc:summary>

  <!-- definitions for handling some basic time information -->
  <xsl:variable name="nowDate" select="java:java.util.Date.new()"
    doc:public="yes"
    doc:description="Contains the time of invocation of the stylesheet as a Java Date object."/>
  <xsl:variable name="hl7DateFmt"
    select="java:java.text.SimpleDateFormat.new(&quot;yyyyMMddHHmmss.SSSZ&quot;)"
    doc:public="yes"
    doc:description="A java.text.SimpleDateFormat object which parses dates in HL7 V2/V3 Timestamp format
    (YYYYMMDDhhmmss.sss+/-ZONE), and formats java.util.Date objects into appropriate strings"/>
  <xsl:variable name="now" select="java:format($hl7DateFmt, $nowDate)"
    doc:public="yes"
    doc:description="The current time as a string formated in HL7 V2 format (YYYYMMDDhhmmss.sss+/-ZONE)."/>
  <xsl:param name="tzOffset" select="java:substring($now, 18)" doc:public="yes"
    doc:description="The offset from UTC for the current local, as a string"/>
  <xsl:variable name="oidFmt"
    select="java:java.text.SimpleDateFormat.new(&quot;yyyyMMdd.kmmssSSS&quot;)"
    doc:public="yes"
    doc:description="A java.text.SimpleDateFormat object which can convert a date (as a java.util.Date) into a 
                     suffix that can be appended to an OID to generate a new OID based on the time."/>
  <xsl:variable name="nowOID" select="java:format($oidFmt, $nowDate)"
    doc:public="yes"
    doc:description="A string generated from the current time that can used to 
                     create a new OID from an existing OID"/>

</xsl:stylesheet>
