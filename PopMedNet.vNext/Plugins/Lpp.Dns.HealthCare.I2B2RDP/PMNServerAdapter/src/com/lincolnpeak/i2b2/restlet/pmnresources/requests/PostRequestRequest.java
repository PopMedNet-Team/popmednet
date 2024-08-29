package com.lincolnpeak.i2b2.restlet.pmnresources.requests;

import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.restlet.pmnresources.requests.KeyValueOfstringanyType;
import com.lincolnpeak.i2b2.restlet.pmnresources.PMNDocument;

@XmlRootElement(name = "Request")
@XmlAccessorType(XmlAccessType.FIELD)
public class PostRequestRequest
{
	@XmlElementWrapper(name="RequestDocuments")
	@XmlElement(name="Document")
	private List<PMNDocument> requestDocuments;
	
	@XmlElement(name="RequestTypeId")
	private String requestTypeId;
	
	@XmlElementWrapper(name="Settings")
	@XmlElement(name="KeyValueOfstringanyType")
	private List<KeyValueOfstringanyType> settings;
	
	public void setRequestTypeId(String requestTypeId)
	{
		this.requestTypeId = requestTypeId;
	}
	
	public String getRequestTypeId()
	{
		return requestTypeId;
	}
	
	public void setRequestDocuments(List<PMNDocument> requestDocuments)
	{
		this.requestDocuments = requestDocuments;
	}
	
	public List<PMNDocument> getRequestDocuments()
	{
		return requestDocuments;
	}
	
	public void setSettings(List<KeyValueOfstringanyType> settings)
	{
		this.settings = settings;
	}
	
	public List<KeyValueOfstringanyType> getSettings()
	{
		return settings;
	}	
}
