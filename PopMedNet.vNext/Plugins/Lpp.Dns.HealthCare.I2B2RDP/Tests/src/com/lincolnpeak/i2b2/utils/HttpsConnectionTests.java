package com.lincolnpeak.i2b2.utils;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URL;
import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;

import org.apache.http.Header;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.HttpStatus;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.impl.client.DefaultHttpClient;
import org.junit.Test;

import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;

import junit.framework.Assert;

/**
 * Unit Test class for testing connection status with specified urlString.
 * 
 * @author daniel
 *
 */
public class HttpsConnectionTests 
{
	@Test
	public void TestConnection()
	{
		try 
		{
			SSLSocketFactoryConfigurator config = SSLSocketFactoryConfigurator.getInstance();
//			String urlString = "https://dnsquerytool.lincolnpeak.com"; 
			String urlString = "https://popmednet.bidmc.harvard.edu/services/?/QueryToolService/"; 
			URL url = new URL(urlString);
			HttpPost httpPost = new HttpPost(url.toURI());
			HttpClient httpClient = config.getHttpClient();
			HttpResponse response = httpClient.execute(httpPost);

			if(response.getStatusLine().getStatusCode() == HttpStatus.SC_MOVED_TEMPORARILY)
			{
				Header[] headers = response.getHeaders("Location");
				url = new URL(urlString + headers[0].getValue());
				httpPost = new HttpPost(url.toURI());
				httpClient = config.getHttpClient();
				response = httpClient.execute(httpPost);
			}
			
			if(response.getStatusLine().getStatusCode() != HttpStatus.SC_OK)
			{
				Assert.fail(String.valueOf(response.getStatusLine().getStatusCode()));
			}
			

			HttpEntity entity = response.getEntity();
			String httpResponseMessage = "";
			
			if (entity != null) 
			{
				InputStream instream = entity.getContent();
				try 
				{
					BufferedReader reader = new BufferedReader(new InputStreamReader(instream));
			         
					String line;
					while((line = reader.readLine()) != null)
						httpResponseMessage += line;
					
				}
				finally 
				{
					// Closing the input stream will trigger connection release
					instream.close();
				}
			}

			if( httpResponseMessage.length() > 0)
				System.out.println(httpResponseMessage);
			

		} 
		catch (Exception e) 
		{
			Assert.fail(e.getMessage());
			e.printStackTrace();
		} 
	}
}
