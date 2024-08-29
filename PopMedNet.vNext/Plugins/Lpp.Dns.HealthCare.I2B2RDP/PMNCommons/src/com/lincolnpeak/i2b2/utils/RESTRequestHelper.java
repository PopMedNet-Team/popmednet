package com.lincolnpeak.i2b2.utils;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.security.KeyStore;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.HttpStatus;
import org.apache.http.HttpVersion;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpRequestBase;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.PlainSocketFactory;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.entity.ContentType;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.tsccm.ThreadSafeClientConnManager;
import org.apache.http.params.BasicHttpParams;
import org.apache.http.params.HttpParams;
import org.apache.http.params.HttpProtocolParams;
import org.apache.http.protocol.HTTP;

import com.lincolnpeak.i2b2.exceptions.CannotRunRequest;

public class RESTRequestHelper 
{
	protected final static Log log = LogFactory.getLog(RESTRequestHelper.class);

	public static String postRequest(String url, String requestContent) throws CannotRunRequest
	{
		HttpPost httpPost = new HttpPost(url);
		
		if(requestContent != null)
		{
			log.debug(">>>>>>>>>>>>>>>> Posting to url: " + url);
			log.debug(">>>>>>>>>>>>>>>> With the following content:");
			log.debug(requestContent);
			StringEntity requestEntity = new StringEntity(requestContent, ContentType.create("text/xml", "UTF-8"));
			httpPost.setEntity(requestEntity);
		}
		
		return runRequest(httpPost);
	}

	public static String postRequest(String url) throws CannotRunRequest
	{
		return postRequest(url, null);
	}
	
	public static String getRequest(String url) throws CannotRunRequest
	{
		HttpGet httpGet = new HttpGet(url);
		return runRequest(httpGet);
	}
	
	private static String runRequest(HttpRequestBase httpRequest) throws CannotRunRequest
	{
//		HttpClient httpClient = new DefaultHttpClient();
		HttpClient httpClient = SSLSocketFactoryConfigurator.getInstance().getHttpClient();
		
		String httpResponseMessage = "";		

		try 
		{			
			HttpResponse response = httpClient.execute(httpRequest);

			HttpEntity entity = response.getEntity();
			
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

			if(response.getStatusLine().getStatusCode() == HttpStatus.SC_OK)
			{
				log.debug(httpResponseMessage);
				return httpResponseMessage;
			}
			else
			{
				log.debug(response);
				throw new CannotRunRequest(httpResponseMessage);
			}
								
		}
		catch (ClientProtocolException e) 
		{
			log.debug(e);
			throw new CannotRunRequest(e);
		} 
		catch (IOException e) 
		{
			// In case of an IOException, the connection will be released
			// back to the connection manager automatically
			throw new CannotRunRequest(e);
		}
		catch (RuntimeException e) 
		{
			// In case of an unexpected exception you may want to abort
			// the HTTP request in order to shut down the underlying
			// connection and release it back to the connection manager.
			httpRequest.abort();
			throw new CannotRunRequest(e);
		} 
		finally
		{
			// When HttpClient instance is no longer needed,
			// shut down the connection manager to ensure
			// immediate deallocation of all system resources
			httpClient.getConnectionManager().shutdown();				
		}
				
	}
}
