package com.lincolnpeak.i2b2.utils.xml;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlValue;

@XmlRootElement(name = "request_type")
@XmlAccessorType(XmlAccessType.FIELD)
public class RequestType implements IXmlFragment 
{
	@XmlValue
	private String value;
	
	public String getValue()
	{
		return value;
	}
	
	public void setValue(String value)
	{
		this.value = value;
	}
}
