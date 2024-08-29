package com.lincolnpeak.i2b2.utils.xml;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlValue;

@XmlRootElement(name = "_type")
@XmlAccessorType(XmlAccessType.FIELD)
public class QueryStatusType implements IXmlFragment 
{
	@XmlElement(name="status_type_id")
	private String statusTypeId;
	
	@XmlElement(name="name")
	private String name;

	@XmlElement(name="description")
	private String description;

	public String getStatusTypeId()
	{
		return statusTypeId;
	}
	
	public void setStatusTypeId(String statusTypeId)
	{
		this.statusTypeId = statusTypeId;
	}

	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}

	public String getDescription()
	{
		return description;
	}
	
	public void setDescription(String description)
	{
		this.description = description;
	}

}
