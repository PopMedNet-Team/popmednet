package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;


@XmlRootElement(name="ResponseSessionMetadata")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNResponseSessionMetadata extends MessageElement implements IPMNResponse
{
	@XmlElementWrapper(name="DataMartResponses")
	@XmlElement(name="DataMartResponse")
	private List<DataMartResponse> datamartResponses;
	
	@XmlElement(name="Session")
	private Session session;
	
	public List<DataMartResponse> getDataMartResponses()
	{
		return datamartResponses;
	}
	
	public void setDataMartResponses(List<DataMartResponse> datamartResponses)
	{
		this.datamartResponses = datamartResponses;
	}
	
	public Session getSession()
	{
		return session;
	}
	
	public void setSession(Session session)
	{
		this.session = session;
	}
	
//	public static IPMNResponse load(String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
//	{
// 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
//			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
//		JAXBContext context = JAXBContext.newInstance(PMNResponseSessionMetadata.class);
//		Unmarshaller unmarshaller = context.createUnmarshaller();
//        return (IPMNResponse)unmarshaller.unmarshal(node);		
//	}

}
