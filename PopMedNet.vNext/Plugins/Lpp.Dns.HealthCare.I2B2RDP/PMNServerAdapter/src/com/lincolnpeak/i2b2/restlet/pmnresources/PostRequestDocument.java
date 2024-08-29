package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.io.IOException;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;

import org.restlet.ext.xml.DomRepresentation;
import org.restlet.representation.Representation;
import org.restlet.resource.Post;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotRetrieveRestRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.requests.RequestDocumentRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.RequestDocumentResponse;

public class PostRequestDocument extends PMNResource 
{
	@Post("xml")
    public Representation RequestDocument(Representation rep) throws CannotBuildRestResponse, CannotRetrieveRestRequest
	{
		String requestId = (String) getRequestAttributes().get("requestId");	
		String documentId = (String) getRequestAttributes().get("documentId");	
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);
		
		try 
		{
			int offset = Integer.parseInt((String) getRequestAttributes().get("offset"));	

			try
			{
				DomRepresentation domRep = new DomRepresentation(rep);
				Document doc = domRep.getDocument();
					        
				Element requestElement = doc.getDocumentElement();
				JAXBContext context = JAXBContext.newInstance(RequestDocumentRequest.class);
				Unmarshaller unmarshaller = context.createUnmarshaller();
		        RequestDocumentRequest requestPojoFromXml = (RequestDocumentRequest)unmarshaller.unmarshal(requestElement);
		        i2b2Request.putDocumentData(documentId, new String(requestPojoFromXml.decodedData()), offset);	        		        
			}
			catch (IOException e) 
			{
				throw new CannotRetrieveRestRequest(e);
			} 
			catch (JAXBException e) 
			{
				throw new CannotRetrieveRestRequest(e);
			}
			
	        return buildRestResponse(RequestDocumentResponse.class);
	        
//	        context = JAXBContext.newInstance(RequestDocumentResponse.class);
//	        Marshaller marshaller = context.createMarshaller();
//	        marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
//	        DocumentBuilder docBuilder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
//	        Document document = docBuilder.newDocument();
//	        marshaller.marshal(new RequestDocumentResponse(), document);
//	        domRep.setDocument(document);
//	        return domRep;
		} 
		catch(CannotBuildRestResponse e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw e;
		}	
		catch(CannotRetrieveRestRequest e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw e;
		}
		catch(NumberFormatException e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw new CannotBuildRestResponse(e);
		}	
		
//		catch (IOException e) 
//		{
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		} 
//		catch (JAXBException e) 
//		{
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		} catch (ParserConfigurationException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
		
//		return null;
		
		
//    		Document[] requestDocuments, 
//    		String requestTypeId, 
//    		Map<String, Object> settings,
//            String[] desiredDocumentIds
            
    }
}

//@XmlRootElement(name = "RequestResponse")
//@XmlAccessorType(XmlAccessType.FIELD)
//class RequestResponse
//{
//	@XmlElementWrapper(name="DesiredDocuments")
//	@XmlElement(name="Document")
//	private List<PMNDocument> desiredDocuments;
//	
//	public void setDesiredDocuments(List<PMNDocument> desiredDocuments)
//	{
//		this.desiredDocuments = desiredDocuments;
//	}
//	
//	public List<PMNDocument> getDesiredDocuments()
//	{
//		return desiredDocuments;
//	}
//}

//@XmlRootElement(name = "Request")
//@XmlAccessorType(XmlAccessType.FIELD)
//class Request
//{
//	@XmlElementWrapper(name="RequestDocuments")
//	@XmlElement(name="Document")
//	private List<PMNDocument> requestDocuments;
//	
//	@XmlElement(name="RequestTypeId")
//	private String requestTypeId;
//	
//	@XmlElementWrapper(name="Settings")
//	@XmlElement(name="KeyValueOfstringanyType")
//	private List<KeyValueOfstringanyType> settings;
//	
//	public void setRequestTypeId(String requestTypeId)
//	{
//		this.requestTypeId = requestTypeId;
//	}
//	
//	public String getRequestTypeId()
//	{
//		return requestTypeId;
//	}
//	
//	public void setRequestDocuments(List<PMNDocument> requestDocuments)
//	{
//		this.requestDocuments = requestDocuments;
//	}
//	
//	public List<PMNDocument> getRequestDocuments()
//	{
//		return requestDocuments;
//	}
//	
//	public void setSettings(List<KeyValueOfstringanyType> settings)
//	{
//		this.settings = settings;
//	}
//	
//	public List<KeyValueOfstringanyType> getSettings()
//	{
//		return settings;
//	}	
//}

//class RequestDocuments
//{
//	private List<PMNDocument> requestDocuments = new ArrayList<PMNDocument>();
//	
//	public void setRequestDocuments(List<PMNDocument> requestDocuments)
//	{
//		this.requestDocuments = requestDocuments;
//	}
//	
//	public List<PMNDocument> getRequestDocuments()
//	{
//		return requestDocuments;
//	}
//}
//
//class Settings
//{
//	private List<KeyValueOfstringanyType> settings = new ArrayList<KeyValueOfstringanyType>();
//	
//	public void setSettings(List<KeyValueOfstringanyType> settings)
//	{
//		this.settings = settings;
//	}
//	
//	public List<KeyValueOfstringanyType> getSettings()
//	{
//		return settings;
//	}	
//}

//@XmlRootElement(name="KeyValueOfstringanyType")
//@XmlAccessorType(XmlAccessType.FIELD)
//class KeyValueOfstringanyType
//{
//	@XmlElement(name="Key")
//	private String key;
//	
//	@XmlElement(name="Value")
//	private String value;
//	
//	public void setKey(String key)
//	{
//		this.key = key;
//	}
//	
//	public String getKey()
//	{
//		return key;
//	}
//	
//	public void setValue(String value)
//	{
//		this.value = value;
//	}
//	
//	public String getValue()
//	{
//		return value;
//	}		
//}
	
//    void RequestDocument(
//            String requestId,
//            String documentId,
//            String offset,
//            int offset,
//            byte[] data);
//
//    void Start(String requestId);
//
//    void Stop(String requestId, 
//            StopReason reason);
//
//    RequestStatus Status(string requestId);
//
//    Document[] Response(string requestId);
//
//    int ResponseDocument(
//            String requestId,
//            String documentId,
//            String offset,
//            byte[] data
//            );
//
//    void Close(String requestId);
//
//    IDictionary<string,string> Properties();
//      
//    IDictionary<string,bool> Capabilities();

