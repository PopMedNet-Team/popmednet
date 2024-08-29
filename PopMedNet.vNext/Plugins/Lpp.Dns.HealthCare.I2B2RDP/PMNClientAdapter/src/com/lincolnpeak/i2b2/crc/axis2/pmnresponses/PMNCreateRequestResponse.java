package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import java.io.ByteArrayInputStream;
import java.io.IOException;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;

import org.w3c.dom.Element;
import org.xml.sax.SAXException;


@XmlRootElement(name="CreateRequestResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNCreateRequestResponse implements IPMNResponse
{
	@XmlElement(name="CreateRequestResult")
	private String requestId;
	
	public String getCreateRequestResult()
	{
		return requestId;
	}
	
	public void setCreateRequestResult(String requestId)
	{
		this.requestId = requestId;
	}
	
	public static IPMNResponse load(String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
	{
 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
		JAXBContext context = JAXBContext.newInstance(PMNCreateRequestResponse.class);
		Unmarshaller unmarshaller = context.createUnmarshaller();
        return (IPMNResponse)unmarshaller.unmarshal(node);		
	}

}
