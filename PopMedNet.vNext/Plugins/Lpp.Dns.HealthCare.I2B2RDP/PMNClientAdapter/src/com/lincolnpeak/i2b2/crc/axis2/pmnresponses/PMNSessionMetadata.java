package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;


@XmlRootElement(name="SessionMetadata")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNSessionMetadata extends MessageElement implements IPMNResponse
{
	@XmlElement(name="ModelId")
	private String modelId;
	
	@XmlElement(name="RequestTypeId")
	private String requestTypeId;
	
	@XmlElement(name="ReturnUrl")
	private String returnUrl;
	
	public String getModelId()
	{
		return modelId;
	}
	
	public void setModelId(String modelId)
	{
		this.modelId = modelId;
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
	
//	public static IPMNResponse load(String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
//	{
// 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
//			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
//		JAXBContext context = JAXBContext.newInstance(PMNSessionMetadata.class);
//		Unmarshaller unmarshaller = context.createUnmarshaller();
//        return (IPMNResponse)unmarshaller.unmarshal(node);		
//	}

}
