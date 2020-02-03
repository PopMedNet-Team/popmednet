<?xml version="1.0"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:variable name="header" select="name(/NewDataSet/Table/*)"/>

<xsl:template match="/NewDataSet">
	<html>
		<body>
			<table>
				<thead>
				<tr>
				<xsl:apply-templates select="Table[1]/*" mode="header"/>
				</tr>
				</thead>
				<xsl:apply-templates select="Table" mode="body"/>
			</table>
		</body>
	</html>
</xsl:template>

<xsl:template match="Table/*" mode="header">
	<th>
		<xsl:value-of select="name(.)"/>
	</th>
</xsl:template>

<xsl:template match="Table" mode="body">
	<tr>
		<xsl:apply-templates select="node()"/>
	</tr>
</xsl:template>

<xsl:template match="*">
	<td><xsl:value-of select="."/></td>
</xsl:template>

</xsl:stylesheet>