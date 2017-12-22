<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
    <xsl:output method="html" indent="yes"/>
    <xsl:template match="/">
      <html>
        <body style="font-family: Calibri; font-size: font-size: 16px;">
          <table style="border-style:solid; border-width: 2px; width: 620px; margin:4px; padding:4px">
            <tr>
              <td>
                <xsl:apply-templates select="request_builder/header"/>
              </td>
            </tr>
            <tr>
              <td>
                <hr/>
              </td>
            </tr>
            <tr>
              <td>
                <xsl:apply-templates select="request_builder/request"/>
              </td>
            </tr>
          </table>
        </body>
      </html>
    </xsl:template>
 
    <xsl:template match="header">
      <table style="width: 600px;  text-align:left" >
        <tr>
          <td>
            <b>Request Name: </b>
            <xsl:value-of select="request_name"/>
          </td>
          <td>
            <b>Request Type: </b>
            <xsl:value-of select="request_type"/>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <b>Request Submitter: </b>
            <xsl:value-of select="submitter_email"/>
          </td>
        </tr>
      </table>
    </xsl:template>
  
    <xsl:template match="request">
      <xsl:apply-templates select="Files"/>
    </xsl:template>

  <xsl:template match="Files">
    <b>Files</b>
    <table style="border-style:solid; border-width: 1px; border-color: Gray; width: 600px; text-align:left;">
      <thead style="background-color: #E3E6EB; font-weight: bold; ">
        <th>Name</th>
        <th width="100px">Mime Type</th>
        <th width="100px">Size</th>
      </thead>
      <xsl:apply-templates select="File"/>
    </table>
  </xsl:template>

  <xsl:template match="File">
    <tr>
      <td>
        <xsl:value-of select="Name"/>
      </td>
      <td width="100px">
        <xsl:value-of select="MimeType"/>
      </td>
      <td width="100px">
        <xsl:value-of select="Size"/>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
