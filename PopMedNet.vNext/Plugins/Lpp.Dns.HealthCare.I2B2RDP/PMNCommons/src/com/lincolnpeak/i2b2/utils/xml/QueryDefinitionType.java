package com.lincolnpeak.i2b2.utils.xml;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.StringWriter;
import java.util.List;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Marshaller;
import javax.xml.bind.Unmarshaller;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Element;
import org.xml.sax.SAXException;



@XmlRootElement(name="query_definition")
@XmlAccessorType(XmlAccessType.FIELD)
public class QueryDefinitionType implements IXmlFragment
{
	@XmlElement(name="query_name")
	private String queryName;
	
	@XmlElement(name="query_timing")
	private String queryTiming;
	
	@XmlElement(name="specificity_scale")
	private String specificityScale;
	
	@XmlElement(name="panel")
	private List<PanelType> panel;
	
	public String getQueryName()
	{
		return queryName;
	}
	
	public void setQueryName(String queryName)
	{
		this.queryName = queryName;
	}
	
	public String getQueryTiming()
	{
		return queryTiming;
	}
	
	public void setQueryTiming(String queryTiming)
	{
		this.queryTiming = queryTiming;
	}
	
	public List<PanelType> getPanel()
	{
		return panel;
	}
	
	public void setPanel(List<PanelType> panel)
	{
		this.panel = panel;
	}

	public static QueryDefinitionType load(String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
	{
 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
		JAXBContext context = JAXBContext.newInstance(QueryDefinitionType.class);
		Unmarshaller unmarshaller = context.createUnmarshaller();
        return (QueryDefinitionType)unmarshaller.unmarshal(node);		
	}
	
	public String toString()
	{
		StringWriter w = new StringWriter();
		
		try
		{
			JAXBContext context = JAXBContext.newInstance(this.getClass());
			Marshaller marshaller = context.createMarshaller();
			marshaller.setProperty(Marshaller.JAXB_FRAGMENT, Boolean.TRUE);
	        marshaller.marshal(this, w);
		}
		catch(JAXBException e)
		{
			e.printStackTrace();
		}
		
        return w.toString();
	}
}
