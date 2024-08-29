package com.lincolnpeak.i2b2.restlet.pmnresources.requests;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="KeyValueOfstringanyType")
@XmlAccessorType(XmlAccessType.FIELD)
public class KeyValueOfstringanyType
{
	@XmlElement(name="Key")
	private String key;
	
	@XmlElement(name="Value")
	private String value;
	
	public void setKey(String key)
	{
		this.key = key;
	}
	
	public String getKey()
	{
		return key;
	}
	
	public void setValue(String value)
	{
		this.value = value;
	}
	
	public String getValue()
	{
		return value;
	}		
}

