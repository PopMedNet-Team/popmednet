package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.util.Arrays;

import org.restlet.engine.util.Base64;
import org.restlet.representation.Representation;
import org.restlet.resource.Get;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseDocumentResponse;

public class GetResponseDocument extends PMNResource 
{
	@Get
	public Representation ResponseDocument() throws CannotBuildRestResponse
	{
		String requestId = (String) getRequestAttributes().get("requestId");
		String documentId = (String) getRequestAttributes().get("documentId");	
		int offset = Integer.parseInt((String) getRequestAttributes().get("offset"));
		I2B2Request i2b2Request = I2B2Requests.getInstance().get(requestId);
		
		try 
		{
			ResponseDocumentResponse responseDocumentData = new ResponseDocumentResponse();
			
			byte[] docDataBytes = i2b2Request.getResponseDocumentData(documentId).getBytes();
			byte[] copyBytes = Arrays.copyOfRange(docDataBytes, offset, offset + 3000 > docDataBytes.length ? docDataBytes.length : offset + 3000);
			responseDocumentData.setData(Base64.encode(copyBytes, false));
			responseDocumentData.setNumBytes(copyBytes.length);
			
			return buildRestResponse(responseDocumentData);

		} 
		catch(CannotBuildRestResponse e)
		{
			i2b2Request.setStatus(StatusCode.Error);
			i2b2Request.setStatusMessage(e.getMessage());
			throw e;
		}	
		
	}
}