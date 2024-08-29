package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import java.util.Date;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "VersionResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class VersionResponse implements IResponse
{
	
	@XmlElement(name="Version")
	private String version;
	
	@XmlElement(name="BuildDate")
	private String buildDate;
	
	public void setVersion(String version)
	{
		this.version = version;
	}
	
	public String getVersion()
	{
		return version;
	}
	
	public void setBuildDate(String buildDate)
	{
		this.buildDate = buildDate;
	}
	
	public String getBuildDate()
	{
		return buildDate;
	}

}
