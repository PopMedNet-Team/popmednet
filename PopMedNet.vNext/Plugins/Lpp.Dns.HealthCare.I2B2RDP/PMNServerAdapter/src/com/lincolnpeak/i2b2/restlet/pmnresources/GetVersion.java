package com.lincolnpeak.i2b2.restlet.pmnresources;

import java.util.List;

import org.restlet.representation.Representation;
import org.restlet.resource.Get;

import com.lincolnpeak.i2b2.restlet.pmnresources.exceptions.CannotBuildRestResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseDocument;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.ResponseResponse;
import com.lincolnpeak.i2b2.restlet.pmnresources.responses.VersionResponse;

import com.lincolnpeak.i2b2.utils.Version;

public class GetVersion extends PMNResource 
{
	@Get
	public Representation Version() throws CannotBuildRestResponse 
	{
		try 
		{
			VersionResponse response = new VersionResponse();
			response.setVersion(Version.getVersion());
			response.setBuildDate(Version.getBuildDate());
			return buildRestResponse(response);
		} 
		catch(CannotBuildRestResponse e)
		{
			throw e;
		}	
		
	}
}