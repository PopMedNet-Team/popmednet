package net.shrine.adapter.translators;

import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.InputStreamReader;

import net.shrine.config.AdapterMappings;

import org.junit.Test;

import com.lincolnpeak.i2b2.utils.xml.QueryDefinitionType;
import com.lincolnpeak.i2b2.utils.xml.XmlHelper;

import junit.framework.Assert;


public class DefaultConceptTranslatorTests 
{
	@Test
	public void testTranslateQueryDefinition()
	{
		QueryDefinitionType queryDef = null;
		
		try
		{
			AdapterMappings mappings = AdapterMappings.loadfromStream(this.getClass().getResourceAsStream("/net/shrine/config/AdapterMappings.xml"));
			DefaultConceptTranslator translator = new DefaultConceptTranslator(mappings);
			BufferedReader reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/net/shrine/adapter/translators/SampleShrineRequest.xml")));
			String xml = "";
			String line;
			while((line = reader.readLine()) != null)
				xml += line;
			queryDef = (QueryDefinitionType) XmlHelper.getElementValue(xml, "query_definition", QueryDefinitionType.class);
			translator.translateQueryDefinition(queryDef);
		}
		catch(Exception e)
		{
			Assert.fail(e.getMessage());
		}
	}
}
