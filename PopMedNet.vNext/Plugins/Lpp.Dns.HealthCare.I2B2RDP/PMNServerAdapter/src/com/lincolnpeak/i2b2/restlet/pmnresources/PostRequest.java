package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

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
import com.lincolnpeak.i2b2.restlet.pmnresources.requests.PostRequestRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.PostRequestResponse;

public class PostRequest extends PMNResource 
{
	@Post("xml")
    public Representation Request(Representation rep) throws CannotBuildRestResponse, CannotRetrieveRestRequest
	{
		String requestId = (String) getRequestAttributes().get("requestId");	
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);

		try 
		{
	        List<String> desiredDocIds = new ArrayList<String>();

	        try
			{
				DomRepresentation domRep = new DomRepresentation(rep);
				Document doc = domRep.getDocument();
				
				Element requestElement = doc.getDocumentElement();
				JAXBContext context = JAXBContext.newInstance(PostRequestRequest.class);
				Unmarshaller unmarshaller = context.createUnmarshaller();
		        PostRequestRequest requestPojoFromXml = (PostRequestRequest)unmarshaller.unmarshal(requestElement);
		        
		        I2B2Requests requests = I2B2Requests.getInstance();
		        I2B2Request request = new I2B2Request();
		        request.setDocuments(requestPojoFromXml.getRequestDocuments());
		        request.setStatus(StatusCode.InProgress);
		        requests.put(requestId, request);
	
		        List<PMNDocument> desiredDocs = requestPojoFromXml.getRequestDocuments();

		        for(PMNDocument desiredDoc : desiredDocs)
		        	desiredDocIds.add(desiredDoc.getDocumentId());
	
			}
			catch (IOException e) 
			{
				throw new CannotRetrieveRestRequest(e);
			} 
			catch (JAXBException e) 
			{
				throw new CannotRetrieveRestRequest(e);
			}			
	
	        PostRequestResponse response = new PostRequestResponse();
	        response.setDesiredDocuments(desiredDocIds);
	        response.setRequestToken(requestId);
	        
	        return buildRestResponse(response);
	        
//	        context = JAXBContext.newInstance(PostRequestResponse.class);
//	        Marshaller marshaller = context.createMarshaller();
//	        marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, true);
//	        DocumentBuilder docBuilder = DocumentBuilderFactory.newInstance().newDocumentBuilder();
//	        Document document = docBuilder.newDocument();
//	        marshaller.marshal(response, document);
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

		
//		catch (ParserConfigurationException e) 
//		{
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
//		catch (Exception e)
//		{
//			e.printStackTrace();
//		}
//		
//		
//		
//		return null;
            
    }
}
