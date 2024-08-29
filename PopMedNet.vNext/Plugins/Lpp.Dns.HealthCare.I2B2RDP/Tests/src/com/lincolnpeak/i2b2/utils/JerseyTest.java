package com.lincolnpeak.i2b2.utils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.MissingResourceException;
import java.util.ResourceBundle;

import javax.ws.rs.core.MediaType;
import com.sun.jersey.api.client.Client; import com.sun.jersey.api.client.ClientResponse;
import com.sun.jersey.api.client.WebResource;

/**
 * Sends and receives I2B2 test message and response.
 * 
 * JerseyTest <xml message> [Ont|Qry]
 * 
 * for ontology or query message.
 * 
 * @author daniel
 *
 */
public class JerseyTest {
	public static void main(String[] args) {
		Client httpClient = Client.create();
		WebResource testRequest;
		if(args[1].equals("Ont"))
			testRequest = httpClient.resource("https://popmednet.bidmc.harvard.edu/services/?/OntologyService/getCodeInfo");
		else
			testRequest = httpClient.resource("https://popmednet.bidmc.harvard.edu/services/?/QueryToolService/request");
//		String file = new JerseyTest().getMessage("TestOntologyMessage.xml");
		String file = new JerseyTest().getMessage(args[0]);
		System.out.println("Posting:\n" + file);
	   	ClientResponse response =
testRequest.accept(MediaType.APPLICATION_XML,MediaType.TEXT_XML).type(MediaType.TEXT_XML).post(ClientResponse.class, file);
//testRequest.accept(MediaType.APPLICATION_XML,MediaType.TEXT_XML).type(MediaType.TEXT_XML).get(ClientResponse.class);
    	String responseText = response.getEntity(String.class);   	
    	System.out.println(responseText);

	}

	protected String getMessage(String messageFile)
	{
		BufferedReader reader = null;
		try
		{
			reader = new BufferedReader(new InputStreamReader(this.getClass().getResourceAsStream("/com/lincolnpeak/i2b2/utils/" + messageFile)));

			String i2b2Message = "";
			String line;
			while((line = reader.readLine()) != null)
				i2b2Message += line;
									
			return i2b2Message;
		} 
		catch (IOException e) 
		{
			System.err.println(e);
		}
		finally
		{
			if(reader != null)
				try 
				{
					reader.close();
				} 
				catch (IOException e) 
				{
					System.err.println(e);
				}
		}
		
		return null;
	}
}