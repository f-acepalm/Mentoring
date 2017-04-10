<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl lib"
                xmlns:lib="http://library.by/catalog">

    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="/">
        <rss version="2.0">
            <channel>
                <title>Новости library.by</title>
                <link>http://library.by</link>
                <description>Какие-то новости</description>
                <xsl:apply-templates select="/lib:catalog/lib:book"/>
            </channel>
        </rss>
    </xsl:template>

    <xsl:template match="/lib:catalog/lib:book">
        <item>
            <title>
                <xsl:value-of select="./lib:title"/> | author - <xsl:value-of select="./lib:author"/>
            </title>
            <description>
                <xsl:value-of select="./lib:description"/>
            </description>
            <link>
                <xsl:choose>
                    <xsl:when test="./lib:isbn and ./lib:genre = 'Computer'">
                        http://my.safaribooksonline.com/<xsl:value-of select="./lib:isbn"/>/
                    </xsl:when>
                    <xsl:otherwise>
                        http://library.by/<xsl:value-of select="./@id"/>/
                    </xsl:otherwise>
                </xsl:choose>
            </link>
            <pubDate>
                <xsl:value-of select="./lib:registration_date"/>
            </pubDate>
        </item>
    </xsl:template>
</xsl:stylesheet>
