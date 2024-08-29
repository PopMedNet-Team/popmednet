package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;


@XmlRootElement(name="GetRequestsResponse")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNGetRequestsResponse extends MessageElement implements IPMNResponse
{
	@XmlElementWrapper(name="GetRequestsResult")
	@XmlElement(name="Request")
	private List<PMNRequest> requests;
	
	public List<PMNRequest> getRequests()
	{
		return requests;
	}
	
	public void setRequests(List<PMNRequest> requests)
	{
		this.requests = requests;
	}
	
//	public static IPMNResponse load(String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
//	{
// 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
//			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
//		JAXBContext context = JAXBContext.newInstance(PMNGetRequestsResponse.class);
//		Unmarshaller unmarshaller = context.createUnmarshaller();
//        return (IPMNResponse)unmarshaller.unmarshal(node);		
//	}

}
