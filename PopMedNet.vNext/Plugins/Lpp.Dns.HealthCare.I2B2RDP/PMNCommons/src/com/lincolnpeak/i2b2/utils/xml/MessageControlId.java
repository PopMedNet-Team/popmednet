package com.lincolnpeak.i2b2.utils.xml;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

@XmlRootElement(name = "message_control_id")
@XmlAccessorType(XmlAccessType.FIELD)
public class MessageControlId implements IXmlFragment 
{
	@XmlElement(name="message_num")
	private String messageNum;

	@XmlElement(name="processing_mode")
	private String processingMode;

	
	public String getMessageNum()
	{
		return messageNum;
	}
	
	public void setMessageNum(String messageNum)
	{
		this.messageNum = messageNum;
	}
	
	public String getProcessingMode()
	{
		return processingMode;
	}
	
	public void setProcessingMode(String processingMode)
	{
		this.processingMode = processingMode;
	}
	
}
