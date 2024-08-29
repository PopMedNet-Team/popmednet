package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.MissingResourceException;
import java.util.ResourceBundle;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.restlet.ext.xml.DomRepresentation;
import org.restlet.resource.ServerResource;
import org.w3c.dom.Document;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.IResponse;


public class PMNResource extends ServerResource 
{
	private final Log log = LogFactory.getLog(getClass());
	private String CRC_URL = "http://localhost:9090/i2b2/rest/QueryToolService/"; // Backup
	private String PMN_RESOURCE_BUNDLE = "com.lincolnpeak.i2b2.restlet.PMNAdapter";

	protected DomRepresentation buildRestResponse(Class<? extends IResponse> response) throws CannotBuildRestResponse
	{
		try
		{
			return buildRestResponse(response.newInstance());
		}
		catch (InstantiationException e) 
		{
			log.error(e);
			throw new CannotBuildRestResponse(e);
		} 
		catch (IllegalAccessException e) 
		{
			log.error(e);
			throw new CannotBuildRestResponse(e);
		}		
		
	}
	
	protected DomRepresentation buildRestResponse(IResponse response) throws CannotBuildRestResponse
	{
		try
		{
			DomRepresentation domRep = new DomRepresentation();
			JAXBContext context = JAXBContext.newInstance(response.getClass());
			Marshaller marshaller = context.createMarshaller();
			marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
			DocumentBuilder docBuilder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
			Document document = docBuilder.newDocument();
			marshaller.marshal(response, document);
			domRep.setDocument(document);
			return domRep;
		}
		catch (IOException e) 
		{
			log.error(e);
			throw new CannotBuildRestResponse(e);
		} 
		catch (JAXBException e) 
		{
			log.error(e);
			throw new CannotBuildRestResponse(e);
		} 
		catch (ParserConfigurationException e) 
		{
			log.error(e);
			throw new CannotBuildRestResponse(e);
		} 
	}

	protected String getResourceValue(String key, String defaultValue)
	{
		String value = defaultValue;
		try
		{
			ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
			value = resource.getString(key);
		}
		catch(MissingResourceException e)
		{
			log.error(e);
			value = defaultValue;
		}
		
		return value;
	}

	protected String getCRCUrl()
	{
		return getResourceValue("crcUrl", CRC_URL);
	}
	
	protected String getCRCProxyUser()
	{
		return getResourceValue("crcProxyUser", "demo");
	}
	
	protected String getCRCProxyGroup()
	{
		return getResourceValue("crcProxyGroup", "Demo");
	}
	
	protected String getCRCProxyDomain()
	{
		return getResourceValue("crcProxyDomain", "i2b2demo");
	}
	
	protected String getCRCProxyProject()
	{
		return getResourceValue("crcProxyProject", "Demo");
	}
	
	protected String getCRCProxyPassword()
	{
		return getResourceValue("crcProxyPassword", "demouser");
	}
	
	protected String getHqmfProxyUser()
	{
		return getResourceValue("hqmfProxyUser", "pmn");
	}
	
	protected String getHqmfProxyGroup()
	{
		return getResourceValue("hqmfProxyGroup", "Demo");
	}
	
	protected String getHqmfProxyDomain()
	{
		return getResourceValue("hqmfProxyDomain", "i2b2demo");
	}
	
	protected String getHqmfProxyProject()
	{
		return getResourceValue("hqmfProxyProject", "pmn");
	}
	
	protected String getHqmfProxyPassword()
	{
		return getResourceValue("hqmfProxyPassword", "demouser");
	}
	
	protected String getMessage(String messageFile)
	{
		BufferedReader reader = null;
		try
		{
			reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/com/lincolnpeak/i2b2/restlet/pmnresources/" + messageFile)));

			String i2b2Message = "";
			String line;
			while((line = reader.readLine()) != null)
				i2b2Message += line;
									
			return i2b2Message;
		} 
		catch (IOException e) 
		{
			log.error(e);
		}
		finally
		{
			if(reader != null)
				try 
				{
					reader.close();
				} 
				catch (IOException e) 
				{
					log.error(e);
				}
		}
		
		return null;
	}
	
	protected String getRunQueryInstanceFromQueryDefinitionMessage()
	{
		return getMessage("CRC_QRY_runQueryInstance_fromQueryDefinition.xml");
		
//		BufferedReader reader = null;
//		try
//		{
//			reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/com/lincolnpeak/i2b2/restlet/pmnresources/CRC_QRY_runQueryInstance_fromQueryDefinition.xml")));
//
//			String i2b2Message = "";
//			String line;
//			while((line = reader.readLine()) != null)
//				i2b2Message += line;
//									
//			return i2b2Message;
//		} 
//		catch (IOException e) 
//		{
//			log.error(e);
//		}
//		finally
//		{
//			if(reader != null)
//				try 
//				{
//					reader.close();
//				} 
//				catch (IOException e) 
//				{
//					log.error(e);
//				}
//		}
//		
//		return null;
	}
}
