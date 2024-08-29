package com.lincolnpeak.i2b2.crc.axis2.i2b2responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlAttribute;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlValue;

@XmlRootElement(name="status")
@XmlAccessorType(XmlAccessType.FIELD)
public class Status
{
	@XmlAttribute(name="type")
	private String type;

	@XmlValue
	private String value;
	
	public Status()
	{		
	}
	
	public Status(String status)
	{
		type = status;
		value = status;
	}
	
	public String getType()
	{
		return type;
	}
	
	public void setType(String type)
	{
		this.type = type;
	}

	public String getValue()
	{
		return value;
	}
	
	public void setValue(String value)
	{
		this.value = value;
	}
}