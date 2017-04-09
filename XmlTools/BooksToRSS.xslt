<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:lib="http://library.by/catalog"
>
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/">
        <xsl:element name="rss">
            <xsl:attribute name="version">2.0</xsl:attribute>
            <xsl:element name="channel">
                <xsl:apply-templates select="/lib:catalog/lib:book"/>
            </xsl:element>
        </xsl:element>
    </xsl:template>

    <xsl:template match="/lib:catalog/lib:book">
        <xsl:element name="item">
            <xsl:element name="title">
                <xsl:value-of select="./lib:title"/> by <xsl:value-of select="./lib:author"/>
            </xsl:element>
        </xsl:element>
        <xsl:element name="description">
            <xsl:value-of select="./lib:description"/>
        </xsl:element>
        <xsl:element name="link">
            <xsl:choose>
                <xsl:when test="./lib:isbn and ./lib:genre = 'Computer'">
                    http://my.safaribooksonline.com/<xsl:value-of select="./lib:isbn"/>/
                </xsl:when>
                <xsl:otherwise>
                    http://library.by/<xsl:value-of select="./@id"/>/
                </xsl:otherwise>
            </xsl:choose>
        </xsl:element>
        <xsl:element name="pubDate">
            <xsl:value-of select="./lib:registration_date"/>
        </xsl:element>
    </xsl:template>
</xsl:stylesheet>
