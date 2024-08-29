package com.lincolnpeak.i2b2.utils;

import java.security.KeyManagementException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.CertificateException;
import java.security.cert.X509Certificate;
import java.util.ResourceBundle;

import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLContext;
import javax.net.ssl.TrustManager;
import javax.net.ssl.X509TrustManager;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.apache.http.client.HttpClient;
import org.apache.http.conn.ClientConnectionManager;
import org.apache.http.conn.scheme.Scheme;
import org.apache.http.conn.scheme.SchemeRegistry;
import org.apache.http.conn.ssl.SSLSocketFactory;
import org.apache.http.impl.client.DefaultHttpClient;

public class SSLSocketFactoryConfigurator 
{
	protected final static Log log = LogFactory.getLog(SSLSocketFactoryConfigurator.class);
	private static String PMN_RESOURCE_BUNDLE = "com.lincolnpeak.i2b2.utils.PMNCommons";
	private static SSLSocketFactoryConfigurator instance = null;
	private boolean trustAll = false;
	private SSLContext sc;
	
	public static SSLSocketFactoryConfigurator getInstance()
	{
		try
		{
			if(instance == null)
			{
				ResourceBundle resource = ResourceBundle.getBundle(PMN_RESOURCE_BUNDLE);
				instance = new SSLSocketFactoryConfigurator(Boolean.parseBoolean(resource.getString("trustAll")));
			}
		}
		catch(Exception e)
		{
			log.error(e);
		}
		
		return instance;
	}
	
	private SSLSocketFactoryConfigurator(boolean trustAll) throws NoSuchAlgorithmException, KeyManagementException
	{
		log.debug("TrustAll = " + trustAll);
		this.trustAll = trustAll;
		
		if(!trustAll)
			return;
		
		sc = SSLContext.getInstance("TLS");
		
		TrustManager[] trustAllCerts = new TrustManager[] 
		{
				new X509TrustManager() 
				{
					public void checkClientTrusted(X509Certificate[] chain,	String authType) throws CertificateException 
					{
					}
					
					public void checkServerTrusted(X509Certificate[] chain, String authType) throws CertificateException 
					{
					}
					
					public X509Certificate[] getAcceptedIssuers() 
					{	
						return null; 
					}
				}
		};
		
		sc.init(null, trustAllCerts, null);

//		HttpsURLConnection.setDefaultSSLSocketFactory(sc.getSocketFactory());
	}
	
	public HttpClient getHttpClient()
	{
		HttpClient httpClient = new DefaultHttpClient();
		
		if(!trustAll)
			return httpClient;
		
		SSLSocketFactory ssf = new SSLSocketFactory(sc);
		ClientConnectionManager ccm = httpClient.getConnectionManager();
		SchemeRegistry sr = ccm.getSchemeRegistry();
		sr.register(new Scheme("https", ssf, 443));
		return new DefaultHttpClient(ccm, httpClient.getParams());
	}
}