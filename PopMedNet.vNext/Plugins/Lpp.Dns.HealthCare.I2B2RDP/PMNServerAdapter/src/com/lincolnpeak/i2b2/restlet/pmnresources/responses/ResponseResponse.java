package com.lincolnpeak.i2b2.restlet.pmnresources.responses;

import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "ResponseResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class ResponseResponse implements IResponse
{
	@XmlElementWrapper(name="ResponseResult")
	@XmlElement(name="Document")
	private List<ResponseDocument> responseDocuments;
		
	public void setResponseDocuments(List<ResponseDocument> responseDocuments)
	{
		this.responseDocuments = responseDocuments;
	}
	
	public List<ResponseDocument> getResponseDocument()
	{
		return responseDocuments;
	}
	
}
