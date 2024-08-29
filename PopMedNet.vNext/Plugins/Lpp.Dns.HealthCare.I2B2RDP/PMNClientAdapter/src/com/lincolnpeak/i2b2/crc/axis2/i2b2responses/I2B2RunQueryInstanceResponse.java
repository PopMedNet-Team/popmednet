package com.lincolnpeak.i2b2.crc.axis2.i2b2responses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name="response")
@XmlAccessorType(XmlAccessType.FIELD)
public class I2B2RunQueryInstanceResponse 
{
	@XmlElementWrapper(name="message_header")
	@XmlElement(name="sending_application")
	private Application sendingApplication;
	
	@XmlElementWrapper(name="response_header")
	@XmlElement(name="result_status")
	private ResultStatus resultStatus;
	
	@XmlElementWrapper(name="message_body")
	@XmlElement(name="response")
	private Response response;
	
	public I2B2RunQueryInstanceResponse()
	{		
	}
	
	public I2B2RunQueryInstanceResponse(String appName, String appVersion)
	{
		sendingApplication = new Application();
		sendingApplication.setName(appName);
		sendingApplication.setVersion(appVersion);
	}
	
	public void setStatus(String status, String returnUrl)
	{
		resultStatus = new ResultStatus();
		resultStatus.setReturnUrl(returnUrl);
		resultStatus.setStatus(new Status(status));
		
		response = new Response();
		response.setResponseStatus(new ResponseStatus(status));
	}
		
	public Application getSendingApplication()
	{
		return sendingApplication;
	}
	
	public void setSendingApplication(Application sendingApplication)
	{
		this.sendingApplication = sendingApplication;
	}
	
	public ResultStatus getResultStatus()
	{
		return resultStatus;
	}
	
	public void setResultStatus(ResultStatus resultStatus)
	{
		this.resultStatus = resultStatus;
	}

	public Response getResponse()
	{
		return response;
	}
	
	public void setResponse(Response response)
	{
		this.response = response;
	}
}

@XmlRootElement(name="sending_application")
@XmlAccessorType(XmlAccessType.FIELD)
class Application
{
	@XmlElement(name="application_name")
	private String name;

	@XmlElement(name="application_version")
	private String version;
	
	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}
	
	public String getVersion()
	{
		return version;
	}
	
	public void setVersion(String version)
	{
		this.version = version;
	}
}

@XmlRootElement(name="result_status")
@XmlAccessorType(XmlAccessType.FIELD)
class ResultStatus
{
	@XmlElement(name="status")
	private Status status;

	@XmlElement(name="returnUrl")
	private String returnUrl;
	
	public Status getStatus()
	{
		return status;
	}
	
	public void setStatus(Status status)
	{
		this.status = status;
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

@XmlRootElement(name="response")
@XmlAccessorType(XmlAccessType.FIELD)
class Response
{
	@XmlElement(name="status")
	private ResponseStatus responseStatus;

	public ResponseStatus getResponseStatus()
	{
		return responseStatus;
	}
	
	public void setResponseStatus(ResponseStatus responseStatus)
	{
		this.responseStatus = responseStatus;
	}
	
}

