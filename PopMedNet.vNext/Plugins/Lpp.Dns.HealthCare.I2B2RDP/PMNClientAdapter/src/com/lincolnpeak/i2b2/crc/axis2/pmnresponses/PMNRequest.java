package com.lincolnpeak.i2b2.crc.axis2.pmnresponses;

import java.util.Date;
import java.util.List;

import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlElementWrapper;
import javax.xml.bind.annotation.XmlRootElement;

import com.lincolnpeak.i2b2.crc.axis2.MessageElement;

@XmlRootElement(name="Request")
@XmlAccessorType(XmlAccessType.FIELD)
public class PMNRequest extends MessageElement implements IPMNResponse 
{
	@XmlElement(name="Id")
	private String id;
	
	@XmlElement(name="Name")
	private String name;
	
	@XmlElement(name="Description")
	private String description;
	
	@XmlElementWrapper(name="DataMartResponses")
	@XmlElement(name="DataMartResponse")
	private List<DataMartResponse> dataMartResponses;
	
	@XmlElement(name="CreateDate")
	private Date createDate;
	
	@XmlElement(name="RequestTypeId")
	private String requestTypeId;
	
	@XmlElementWrapper(name="Documents")
	@XmlElement(name="Document")
	private List<Document> documents;
	
	public String getId()
	{
		return id;
	}
	
	public void setId(String id)
	{
		this.id = id;
	}
	
	public String getName()
	{
		return name;
	}
	
	public void setName(String name)
	{
		this.name = name;
	}
	
	public String getDescription()
	{
		return description;
	}
	
	public void setDescription(String description)
	{
		this.description = description;
	}

	public List<DataMartResponse> getDataMartResponses()
	{
		return dataMartResponses;
	}
	
	public void setDataMartResponses(List<DataMartResponse> dataMartResponses)
	{
		this.dataMartResponses = dataMartResponses;
	}

	public Date getCreateDate()
	{
		return createDate;
	}
	
	public void setCreateDate(Date createDate)
	{
		this.createDate = createDate;
	}

	public String getRequestTypeId()
	{
		return requestTypeId;
	}
	
	public void setRequestTypeId(String requestTypeId)
	{
		this.requestTypeId = requestTypeId;
	}

	public List<Document> getDocuments()
	{
		return documents;
	}
	
	public void setDocuments(List<Document> documents)
	{
		this.documents = documents;
	}
	
//	public static IPMNResponse load(String xml) throws SAXException, IOException, ParserConfigurationException, JAXBException
//	{
// 		Element node =  DocumentBuilderFactory.newInstance().newDocumentBuilder()
//			.parse(new ByteArrayInputStream(xml.getBytes())).getDocumentElement();		
//		JAXBContext context = JAXBContext.newInstance(PMNRequest.class);
//		Unmarshaller unmarshaller = context.createUnmarshaller();
//        return (IPMNResponse)unmarshaller.unmarshal(node);		
//	}
}
