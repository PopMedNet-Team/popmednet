package com.lincolnpeak.i2b2.utils;

import java.util.MissingResourceException;
import java.util.ResourceBundle;

import net.shrine.adapter.translators.DefaultConceptTranslator;
import net.shrine.config.AdapterMappings;

import com.lincolnpeak.i2b2.exceptions.CannotGetElementValue;
import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;
import com.lincolnpeak.i2b2.utils.xml.QueryDefinitionType;
import com.lincolnpeak.i2b2.utils.xml.XmlHelper;

public class ConceptTranslator 
{
	private static String PMN_RESOURCE_BUNDLE = "com.lincolnpeak.i2b2.utils.PMNCommons";
	private static ConceptTranslator instance = null;
	private DefaultConceptTranslator translator = null;

	public static ConceptTranslator getInstance()
	{
		if(instance == null)
			instance = new ConceptTranslator();
		
		return instance;
	}
	
	public ConceptTranslator()
	{
		if(getDoTranslate())
		{
			AdapterMappings mappings = AdapterMappings.loadfromStream(ConceptTranslator.class.getResourceAsStream("/net/shrine/config/AdapterMappings.xml"));
			translator = new DefaultConceptTranslator(mappings);
		}
	}
	
	public String toLocal(String i2b2) throws CannotRunRequest
	{
		if(!getDoTranslate())
			return i2b2;
		
		QueryDefinitionType queryDef = null;
		
		try
		{
			queryDef = (QueryDefinitionType) XmlHelper.getElementValue(i2b2, "query_definition", QueryDefinitionType.class);
			translator.translateQueryDefinition(queryDef);
			String translatedQueryDef = queryDef.toString();			
			return XmlHelper.replaceElementFragment(i2b2, "query_definition", translatedQueryDef);
		}
		catch (CannotGetElementValue e) 
		{
			throw new CannotRunRequest(e);
		}
		
	}
	
	private static boolean getDoTranslate()
	{
		boolean doConvert = false;
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			doConvert = Boolean.parseBoolean(resource.getString("translateConcepts"));
		}
		catch(MissingResourceException e)
		{
		}
		
		return doConvert;
	}
}
