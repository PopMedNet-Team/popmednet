package com.lincolnpeak.i2b2.utils.xml;

import java.io.ByteArrayInputStream;
import java.io.IOException;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Element;
import org.xml.sax.SAXException;

import com.lincolnpeak.i2b2.exceptions.CannotGetElementValue;

public class XmlHelper 
{
	public static IXmlFragment getElementValue(String xml, String name, Class<? extends IXmlFragment> fragmentClass) throws CannotGetElementValue
	{
		try
		{
			String fragmentXml = getElementFragment(xml, name);
	 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
	 							.parse(new ByteArrayInputStream(fragmentXml.getBytes())).getDocumentElement();		
			JAXBContext context = JAXBContext.newInstance(fragmentClass);
			Unmarshaller unmarshaller = context.createUnmarshaller();
	        return (IXmlFragment) unmarshaller.unmarshal(node);
		}
		catch(ParserConfigurationException e)
		{
			throw new CannotGetElementValue(name, e);
		}
		catch(IOException e)
		{
			throw new CannotGetElementValue(name, e);			
		}
		catch(SAXException e)
		{
			throw new CannotGetElementValue(name, e);			
		}
		catch(JAXBException e)
		{
			throw new CannotGetElementValue(name, e);			
		}

	}
	
	public static String replaceElementFragment(String xml, String name, String replace) throws CannotGetElementValue
	{
		try
		{
			int startTag = xml.indexOf("<" + name + ">");
			if(startTag == -1)
				startTag = xml.indexOf("<" + name + " ");
			
			int endTag = xml.substring(startTag).indexOf("</" + name + ">");
			int endTagLen = ("</" + name + ">").length();
			
			if(endTag == -1)
			{
				endTag = xml.substring(startTag).indexOf("/>");
				endTagLen = "/>".length();
			}
	
			endTag += startTag + endTagLen;
			
			return xml.substring(0, startTag) + replace + xml.substring(endTag);	
		}
		catch(Exception ex)
		{
			throw new CannotGetElementValue(name, ex);
		}
	}
	
	public static String getElementFragment(String xml, String name) throws CannotGetElementValue
	{
		try
		{
			int startTag = xml.indexOf("<" + name + ">");
			if(startTag == -1)
				startTag = xml.indexOf("<" + name + " ");
			
			int endTag = xml.substring(startTag).indexOf("</" + name + ">");
			int endTagLen = ("</" + name + ">").length();
			
			if(endTag == -1)
			{
				endTag = xml.substring(startTag).indexOf("/>");
				endTagLen = "/>".length();
			}
	
			endTag += startTag + endTagLen;
			return xml.substring(startTag, endTag);
		}
		catch(Exception ex)
		{
			throw new CannotGetElementValue(name, ex);
		}
	}
}
