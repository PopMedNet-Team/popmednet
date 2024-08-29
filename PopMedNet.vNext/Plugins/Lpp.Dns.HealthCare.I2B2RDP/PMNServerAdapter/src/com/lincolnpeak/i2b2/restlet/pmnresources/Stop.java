package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.io.IOException;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;

import org.restlet.ext.xml.DomRepresentation;
import org.restlet.representation.Representation;
import org.restlet.resource.Put;
import org.w3c.dom.Document;
import org.w3c.dom.Element;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotRetrieveRestRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.requests.StopRequest;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.StopResponse;

public class Stop extends PMNResource 
{
	@Put("xml")
	public Representation StopExecution(Representation rep) throws CannotBuildRestResponse, CannotRetrieveRestRequest 
	{
		String requestId = (String) getRequestAttributes().get("requestId");	
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);

		try 
		{
			try
			{
				DomRepresentation domRep = new DomRepresentation(rep);
				Document doc = domRep.getDocument();
	
				Element requestElement = doc.getDocumentElement();
				JAXBContext context = JAXBContext.newInstance(StopRequest.class);
				Unmarshaller unmarshaller = context.createUnmarshaller();
		        StopRequest requestPojoFromXml = (StopRequest)unmarshaller.unmarshal(requestElement);
			}
			catch (IOException e) 
			{
				throw new CannotRetrieveRestRequest(e);
			} 
			catch (JAXBException e) 
			{
				throw new CannotRetrieveRestRequest(e);
			} 

	        return buildRestResponse(StopResponse.class);
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
		
	}
}