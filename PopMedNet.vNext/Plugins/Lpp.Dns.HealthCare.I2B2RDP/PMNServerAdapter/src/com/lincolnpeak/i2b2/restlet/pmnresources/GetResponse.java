package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.util.List;

import org.restlet.representation.Representation;
import org.restlet.resource.Get;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseDocument;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseResponse;

public class GetResponse extends PMNResource 
{
	@Get
	public Representation Response() throws CannotBuildRestResponse 
	{
		String requestId = (String) getRequestAttributes().get("requestId");
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);

		try 
		{
			ResponseResponse response = new ResponseResponse();

			List<ResponseDocument> documents = I2B2Requests.getInstance().get(requestId).getResponseDocuments();
			response.setResponseDocuments(documents);
			
			return buildRestResponse(response);
			
		} 
		catch(CannotBuildRestResponse e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw e;
		}	
		
	}
}