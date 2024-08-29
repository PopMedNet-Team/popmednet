package com.lincolnpeak.i2b2.restlet.pmnresources;

import org.restlet.representation.Representation;
import org.restlet.resource.Put;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.CloseResponse;

public class Close extends PMNResource 
{
	@Put("xml")
	public Representation CloseRequest(Representation rep) throws CannotBuildRestResponse 
	{
		String requestId = (String) getRequestAttributes().get("requestId");
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);
		
		try
		{
			return buildRestResponse(CloseResponse.class);		
		}
		catch(CannotBuildRestResponse e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw e;
		}
		
	}
}