<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                exclude-result-prefixes="msxsl"
                xmlns:lib="http://library.by/catalog">
    <xsl:output method="xml" indent="yes"/>

    <xsl:key name="group" match="/lib:catalog/lib:book" use="./lib:genre"/>

    <xsl:template match="/lib:catalog">
        <html>
            <head>
            </head>
            <body>
                <h1>
                    <xsl:value-of select="lib:GetCurrentDate()"/>
                </h1>
                <xsl:apply-templates select="./lib:book[generate-id(.) = generate-id(key('group', ./lib:genre))]"/>
                <h2>Total count of books: <xsl:value-of select="count(/lib:catalog/lib:book)"/></h2>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="/lib:catalog/lib:book">
        <h2>
            Genre : <xsl:value-of select="./lib:genre"/> - Book count: <xsl:value-of select="count(key('group', ./lib:genre))"/>
        </h2>
        <table border="1">
            <tr>
                <th>Author</th>
                <th>Title</th>
                <th>Publish Date</th>
                <th>Registration Date</th>
            </tr>
            <xsl:for-each select="key('group', ./lib:genre)">
                <tr>
                    <td>
                        <xsl:value-of select="./lib:author"/>
                    </td>
                    <td>
                        <xsl:value-of select="./lib:title"/>
                    </td>
                    <td>
                        <xsl:value-of select="./lib:publish_date"/>
                    </td>
                    <td>
                        <xsl:value-of select="./lib:registration_date"/>
                    </td>
                </tr>
            </xsl:for-each>
        </table>
    </xsl:template>

    <msxsl:script language="CSharp" implements-prefix="lib">
        public string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    </msxsl:script>
</xsl:stylesheet>