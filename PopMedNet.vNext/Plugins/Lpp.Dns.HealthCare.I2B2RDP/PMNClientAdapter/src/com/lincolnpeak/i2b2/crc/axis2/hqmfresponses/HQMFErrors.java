package com.lincolnpeak.i2b2.crc.axis2.hqmfresponses;

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

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="errors")
@XmlAccessorType(XmlAccessType.FIELD)
public class HQMFErrors extends MessageElement
{
	@XmlElement(name="warning")
	private String warning;

	@XmlElement(name="fatalerror")
	private String fatalError;
	
	public String getWarning()
	{
		return warning;
	}
	
	public void setWarning(String warning)
	{
		this.warning = warning;
	}

	public String getFatalError()
	{
		return fatalError;
	}
	
	public void setFatalError(String fatalError)
	{
		this.fatalError = fatalError;
	}

}