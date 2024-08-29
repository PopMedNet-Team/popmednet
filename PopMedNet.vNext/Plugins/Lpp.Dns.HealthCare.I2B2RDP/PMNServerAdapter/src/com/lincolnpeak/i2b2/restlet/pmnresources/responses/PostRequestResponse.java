package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "RequestResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class PostRequestResponse implements IResponse
{
	@XmlElement(name="RequestResult")
	private String requestToken;
	
	@XmlElementWrapper(name="DesiredDocuments")
	@XmlElement(name="string", namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")
	private List<String> desiredDocumentIds;
		
	public void setRequestToken(String requestToken)
	{
		this.requestToken = requestToken;
	}
	
	public String getRequestToken()
	{
		return requestToken;
	}
	
	public void setDesiredDocuments(List<String> desiredDocumentIds)
	{
		this.desiredDocumentIds = desiredDocumentIds;
	}
	
	public List<String> getDesiredDocuments()
	{
		return desiredDocumentIds;
	}
}
