package com.lincolnpeak.i2b2.crc.axis2.pmnrequests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="SubmitRequest")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNSubmitRequest extends MessageElement
{
	@XmlElement(name="Header")
	private PMNHeader header;
	
	public PMNSubmitRequest()
	{		
	}
	
	public PMNSubmitRequest(PMNHeader header)
	{
		this.header = header;
	}
	
	public void setHeader(PMNHeader header)
	{
		this.header = header;
	}
	
	public PMNHeader getHeader()
	{
		return header;
	}

}
