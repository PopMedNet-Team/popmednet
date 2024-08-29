package com.lincolnpeak.i2b2.restlet.pmnresources;

import org.restlet.representation.Representation;
import org.restlet.resource.Get;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.StatusResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.StatusResult;

public class GetStatus extends PMNResource 
{
	@Get
	public Representation Status() throws CannotBuildRestResponse 
	{
		String requestId = (String) getRequestAttributes().get("requestId");
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);
		
		try 
		{
			StatusResponse status = new StatusResponse();
			StatusResult result = new StatusResult();
			
			result.setCode(i2b2Request.getStatus());
			result.setMessage(i2b2Request.getStatusMessage());
			status.setStatus(result);
			
			return buildRestResponse(status);
		} 
		catch(CannotBuildRestResponse e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw e;
		}

	}
}

