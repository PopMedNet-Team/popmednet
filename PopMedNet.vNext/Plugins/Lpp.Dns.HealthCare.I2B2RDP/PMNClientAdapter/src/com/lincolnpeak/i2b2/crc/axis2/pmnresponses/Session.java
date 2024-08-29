package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;


@XmlRootElement(name="Session")
@XmlAccessorType(XmlAccessType.FIELD)
public class Session implements IPMNResponse
{
	@XmlElement(name="ModelId")
	private String modelId;
	
	@XmlElement(name="ReturnUrl")
	private String returnUrl;

	@XmlElement(name="RequestId")
	private String requestId;
	
	@XmlElement(name="RequesetTypeId")
	private String requestTypeId;
	
	public String getModelId()
	{
		return modelId;
	}
	
	public void setModelId(String modelId)
	{
		this.modelId = modelId;
	}
	
	public String getRequestId()
	{
		return requestId;
	}
	
	public void setRequestId(String requestId)
	{
		this.requestId = requestId;
	}

	public String getRequestTypeId()
	{
		return requestTypeId;
	}
	
	public void setRequestTypeId(String requestTypeId)
	{
		this.requestTypeId = requestTypeId;
	}
	
	public String getReturnUrl()
	{
		return returnUrl;
	}
	
	public void setReturnUrl(String returnUrl)
	{
		this.returnUrl = returnUrl;
	}
	
}
