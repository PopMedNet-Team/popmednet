<?xml version="1.0"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" indent="yes"/>

	<xsl:variable name="DISEASE">Reportable Disease</xsl:variable>
	<xsl:variable name="CODE">ICD-9 Diagnosis</xsl:variable>
  <xsl:variable name="OPTIONCOUNT">
    <xsl:value-of select="count(request_builder/response/report/options/option)"/>
  </xsl:variable>
	<xsl:variable name="request_type" select="/request_builder/header/request_type"/>
	
	<xsl:template match="/">
    SELECT 
      <xsl:choose>
        <xsl:when test="$request_type = $CODE">
        </xsl:when>
        <xsl:otherwise>
          "Disease",
        </xsl:otherwise>
      </xsl:choose>
      <xsl:apply-templates select="request_builder/response/report/options" mode="outerselect"/>
      count("Patients") as "Patients"
    FROM (
    <xsl:apply-templates select="request_builder/request"/>
    ) d
    <xsl:if test="$OPTIONCOUNT > 0">
      <xsl:choose>
        <xsl:when test="$request_type = $CODE">
          GROUP BY
          <xsl:apply-templates select="request_builder/response/report/options" mode="group_by"/>
        </xsl:when>
        <xsl:otherwise>
          GROUP BY
            "Disease",
            <xsl:apply-templates select="request_builder/response/report/options" mode="group_by"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="$request_type = $CODE">
          ORDER BY
          <xsl:apply-templates select="request_builder/response/report/options" mode="group_by"/>
        </xsl:when>
        <xsl:otherwise>
          ORDER BY
            "Disease",
            <xsl:apply-templates select="request_builder/response/report/options" mode="group_by"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <xsl:template match="request">
    SELECT DISTINCT
    <xsl:choose>
          <xsl:when test="$request_type = $CODE">
          </xsl:when>
          <xsl:otherwise>
            a.condition AS "Disease",
          </xsl:otherwise>
      </xsl:choose>
      <xsl:apply-templates select="/request_builder/response/report/options" mode="select"/>
      a.patid AS "Patients"
    FROM
    <xsl:choose>
			  <xsl:when test="$request_type = $CODE">
          esp_mdphnet.esp_diagnosis a
        </xsl:when>
		    <xsl:when test="$request_type = $DISEASE">
          esp_mdphnet.esp_disease a
        </xsl:when> 
		  </xsl:choose>
    JOIN esp_mdphnet.esp_demographic b ON a.patid=b.patid
    JOIN esp_mdphnet.uvt_race c ON b.race=c.item_code
    <xsl:if test="$OPTIONCOUNT > 0">
      <xsl:apply-templates select="/request_builder/response/report/options" mode="join"/>
    </xsl:if>
    WHERE
    <xsl:apply-templates select="request_builder/request"/>

		<xsl:apply-templates select="variables"/>
	</xsl:template>

	<xsl:template match="variables">
		<xsl:apply-templates select="variable">
			<xsl:with-param name="operator">And</xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="operation">
			<xsl:with-param name="operator">And</xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="options"/>
	</xsl:template>
	
	<xsl:template match="inclusion_criteria">
		<xsl:apply-templates select="variable">
			<xsl:with-param name="operator">And</xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="operation">
			<xsl:with-param name="operator">And</xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="options"/>
	</xsl:template>
	
	<xsl:template match="exclusion_criteria">
		<xsl:apply-templates select="variable">
			<xsl:with-param name="operator">And</xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="operation">
			<xsl:with-param name="operator">And</xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="options"/>
	</xsl:template>
	
	<xsl:template match="variable">
		<xsl:param name="operator"/>
		<xsl:choose>
			<xsl:when test="@name = 'Code'">
				a.dx
			</xsl:when>
			<xsl:when test="@name = 'Disease'">
				lower(a.condition)
			</xsl:when>			
			<xsl:when test="@name = 'Observation_Period'">
				<xsl:choose>
					<xsl:when test="$request_type = $CODE">
        				a.a_date						
					</xsl:when>
					<xsl:when test="$request_type = $DISEASE">
						a.date
					</xsl:when>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="@name = 'Age'">
				<xsl:choose>
					<xsl:when test="$request_type = $CODE">
						a.age_at_enc_year
					</xsl:when>
					<xsl:when test="$request_type = $DISEASE">
						a.age_at_detect_year
					</xsl:when>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="@name = 'Sex' or @name='Race'">
				b.<xsl:value-of select="@name"/>
			</xsl:when>	
			<xsl:when test="@name = 'Smoking'">
				b.<xsl:value-of select="@name"/>
			</xsl:when>			
			<xsl:otherwise>
				a.<xsl:value-of select="@name"/>			
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
			<xsl:when test='@operator'>
				<xsl:value-of select="@operator"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
				    <xsl:when test="@name = 'Disease'">
				    	=
				    </xsl:when>
				    <xsl:when test="@name = 'Code'">
				    	like
				    </xsl:when>
				    <xsl:otherwise>
				        =
				    </xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
		    <xsl:when test="@name = 'Disease'">
		        lower('<xsl:value-of select="@value"/>')
		    </xsl:when>
		    <xsl:when test="@name = 'Code'">
		        upper('<xsl:value-of select="@value"/>%')
		    </xsl:when>
		    <xsl:when test="@name = 'Smoking'">
		        <xsl:choose>
		        	<xsl:when test="@value = 1">
		        		'Current'
		        	</xsl:when>
		        	<xsl:when test="@value = 2">
		        		'Never'
		        	</xsl:when>
		        	<xsl:when test="@value = 3">
		        		'Not available'
		        	</xsl:when>
		        	<xsl:when test="@value = 4">
		        		'Passive'
		        	</xsl:when>
		        	<xsl:when test="@value = 5">
		        		'Former'
		        	</xsl:when>
		        </xsl:choose>
		    </xsl:when>
		    <xsl:otherwise>
		        '<xsl:value-of select="@value"/>'
		    </xsl:otherwise>		    
		</xsl:choose>
		<xsl:if test="following-sibling::variable or following-sibling::operation">
			<xsl:value-of select="$operator"/><xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="operation">
		<xsl:param name="operator"/>
		(
		<xsl:apply-templates select="variable">
			<xsl:with-param name="operator" select="@operator"/>
		</xsl:apply-templates>		
		<xsl:apply-templates select="operation">
			<xsl:with-param name="operator" select="$operator"/>
		</xsl:apply-templates>
		)
		<xsl:if test="following-sibling::operation">
			<xsl:value-of select="$operator"/><xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="options" mode="group_by">
		<xsl:apply-templates select="option" mode="group_by"/>
	</xsl:template>
	
	<xsl:template match="option" mode="group_by">
    <xsl:if test="position() > 1">
      <xsl:text>,</xsl:text>
    </xsl:if>
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        "Code", "Description"
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
				"Observation_Period"
			</xsl:when>
			<xsl:when test="@name = 'Sex'">
				"Sex"
			</xsl:when>
			<xsl:when test="@name = 'Race'">
				"Race"
			</xsl:when>
      <xsl:when test="@name = 'Age'">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <!-- 5 year -->
            "5 Year Age Group"
          </xsl:when>
          <xsl:otherwise>
            <!-- 10 year -->
            "10 Year Age Group"
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'CenterId'">
        "Center"
      </xsl:when>
      <xsl:when test="@name = 'Zip'">
        "Zip"
      </xsl:when>
    </xsl:choose>
	</xsl:template>

	<xsl:template match="options" mode="select">
		<xsl:apply-templates select="option" mode="select"/>
	</xsl:template>

  <xsl:template match="option" mode="outerselect">
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        "Code", "Description",
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
        "Observation_Period",
      </xsl:when>
      <xsl:when test="@name = 'Sex'">
        "Sex",
      </xsl:when>
      <xsl:when test="@name = 'Race'">
        "Race",
      </xsl:when>
      <xsl:when test="@name = 'Age'">
        <xsl:choose>
          <xsl:when test="@value = '1'">
            <!-- 5 year -->
            "5 Year Age Group",
          </xsl:when>
          <xsl:otherwise>
            <!-- 10 year -->
            "10 Year Age Group",
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'CenterId'">
        "Center",
      </xsl:when>
      <xsl:when test="@name = 'Zip'">
        "Zip",
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
	<xsl:template match="option" mode="select">
     <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        <xsl:choose>
          <xsl:when test="@value = '3'">
            <!-- 3 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                dx.item_code AS "Code", dx.item_text AS "Description",
              </xsl:when>
              <xsl:when test="$request_type = $DISEASE">
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '4'">
            <!-- 4 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                dx.item_code_with_dec AS "Code", dx.item_text AS "Description",
              </xsl:when>
              <xsl:when test="$request_type = $DISEASE">
                
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '5'">
            <!-- 5 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                dx.item_code_with_dec AS "Code", dx.item_text AS "Description",
              </xsl:when>
              <xsl:when test="$request_type = $DISEASE">

              </xsl:when>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="@name = 'Observation_Period'">
				<xsl:choose>
					<xsl:when test="@value = '1'"> <!-- Month -->
						<xsl:choose>
							<xsl:when test="$request_type = $CODE">
								to_char((date '1960-01-01' + a.a_date), 'YYYY-MM') AS "Observation_Period",
							</xsl:when>
							<xsl:when test="$request_type = $DISEASE">
								to_char((date '1960-01-01' + a.date), 'YYYY-MM') AS "Observation_Period",
							</xsl:when>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="@value = '2'"> <!-- Year -->
						<xsl:choose>
							<xsl:when test="$request_type = $CODE">
								date_part('year', date '1960-01-01' + a.a_date) AS "Observation_Period",
							</xsl:when>
							<xsl:when test="$request_type = $DISEASE">
								date_part('year', date '1960-01-01' + a.date) AS "Observation_Period",
							</xsl:when>
						</xsl:choose>
					</xsl:when>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="@name = 'Sex'">
				b.sex AS "Sex",
			</xsl:when>
			<xsl:when test="@name = 'Race'">
				c.item_text AS "Race",
			</xsl:when>
			<xsl:when test="@name = 'Age'">
			    <xsl:choose>
			    	<xsl:when test="@value = '1'"> <!-- 5 year -->
				        a.age_group_5yr AS "5 Year Age Group",
				    </xsl:when>
				    <xsl:otherwise> <!-- 10 year -->
				        a.age_group_10yr AS "10 Year Age Group",
				    </xsl:otherwise>
			    </xsl:choose>				
			</xsl:when>
			<xsl:when test="@name = 'CenterId'">
				a.centerid AS "Center",
			</xsl:when>
       <xsl:when test="@name = 'Zip'">
         b.zip5 AS "Zip",
       </xsl:when>
       <xsl:otherwise>
				a.<xsl:value-of select="@name"/>,
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

  <xsl:template match="options" mode="join">
    <xsl:apply-templates select="option" mode="join"/>
  </xsl:template>
  
  <xsl:template match="option" mode="join">
    <xsl:choose>
      <xsl:when test="@name = 'ICD9'">
        <xsl:choose>
          <xsl:when test="@value = '3'">
            <!-- 3 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                JOIN esp_mdphnet.uvt_dx_3dig dx ON a.dx_code_3dig=dx.item_code
              </xsl:when>
              <xsl:when test="$request_type = $DISEASE">
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '4'">
            <!-- 4 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                JOIN esp_mdphnet.uvt_dx_4dig dx ON a.dx_code_4dig=dx.item_code
              </xsl:when>
              <xsl:when test="$request_type = $DISEASE">
              </xsl:when>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@value = '5'">
            <!-- 5 digit -->
            <xsl:choose>
              <xsl:when test="$request_type = $CODE">
                JOIN esp_mdphnet.uvt_dx_5dig dx ON a.dx_code_5dig=dx.item_code
              </xsl:when>
              <xsl:when test="$request_type = $DISEASE">
              </xsl:when>
            </xsl:choose>
          </xsl:when>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="response">
		<xsl:apply-templates select="report"/>
	</xsl:template>
	
	<xsl:template match="report">
		<xsl:apply-templates select="options" mode="group_by"/>
	</xsl:template>

  

</xsl:stylesheet>