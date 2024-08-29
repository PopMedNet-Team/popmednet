package jklann.hqmf;
import java.io.FileInputStream;
import java.util.Properties;
import java.util.logging.Level;
import java.util.logging.Logger;

import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.stream.StreamSource;

import com.sun.jersey.api.client.Client;
import com.sun.jersey.api.client.WebResource;

/** Jeff Klann - 7/2012
 * 
 * Does all the hard work of actually calling Xalan on the transforms, which it
 * loads from a directory in .properties. Also builds appropriate XML requests for
 * the ONT cell.
 * 
 * @author jklann
 *
 */

public class Processors {
	private static Processors instance = null;

	protected Processors() {
		MyProps myProps = MyProps.getInstance();
		
		// Load XSL
		//System.out.println("XSL will be loaded from: " + myProps.xslLoc);
		tFactory = javax.xml.transform.TransformerFactory.newInstance();
		tFactory.setErrorListener(new ProcessorErrorHandler());
		reload();
		
		// Set up web requests to ONT
		//System.out.println("ONT requests from "+myProps.ontLoc);
		httpClient = Client.create();
		getTermInfo = httpClient.resource(myProps.ontLoc+"/getTermInfo");
		getCodeInfo = httpClient.resource(myProps.ontLoc+"/getCodeInfo");
		getChildren = httpClient.resource(myProps.ontLoc+"/getChildren");
		header = new StringBuilder();
		
		header.append("<?xml version='1.0' encoding='UTF-8' standalone='yes'?>");
		header.append("<ns3:request xmlns:ns3='http://www.i2b2.org/xsd/hive/msg/1.1/' xmlns:ns4='http://www.i2b2.org/xsd/cell/ont/1.1/' xmlns:ns2='http://www.i2b2.org/xsd/hive/plugin/'>");
		header.append("<message_header><sending_application><application_name>i2b2 Ontology</application_name><application_version>1.6</application_version></sending_application><sending_facility><facility_name>i2b2 Hive</facility_name></sending_facility>");			
	}
	public static Processors getInstance() { 
		if (instance==null) instance = new Processors();
		return instance;
	}
	public void reload() {
		
		// i2b2->i2b2+ [i2b2 with basecodes]
		try {
			i2b2plus = tFactory.newTransformer(new StreamSource(MyProps.getInstance().xslLoc+"/I2B2ToI2B2Plus.xsl"));
			i2b2plus.setParameter("serviceurl", MyProps.getInstance().baseURL);
			i2b2plus.setParameter("subkey-age", MyProps.getInstance().subkeyAge);
			i2b2plus.setErrorListener(new ProcessorErrorHandler());
		} catch (TransformerConfigurationException e) {
			Logger.getLogger("jklann.hqmf.Processors").log(Level.SEVERE,"Could not load toi2b2plus.xsl! "+e.getMessageAndLocation());
		}
		
		// i2b2+->HQMF
		try {
			hqmf = tFactory.newTransformer(new StreamSource(MyProps.getInstance().xslLoc+"/I2B2PlusToHQMF.xsl"));
			hqmf.setErrorListener(new ProcessorErrorHandler());
		} catch (TransformerConfigurationException e) {
			Logger.getLogger("jklann.hqmf.Processors").log(Level.SEVERE,"Could not load tohqmf.xsl! "+e.getMessageAndLocation());
		}
			
		// HQMF->iHQMF
		try{
			ihqmf = tFactory.newTransformer(new StreamSource(MyProps.getInstance().xslLoc+"/HQMFtoIntermediate.xsl"));
			ihqmf.setErrorListener(new ProcessorErrorHandler());
		} catch (TransformerConfigurationException e) {
			Logger.getLogger("jklann.hqmf.Processors").log(Level.SEVERE,"Could not load toihqmf.xsl! "+e.getMessageAndLocation());
		}
		
		// iHQMF->i2b2
		try{
			i2b2 = tFactory.newTransformer(new StreamSource(MyProps.getInstance().xslLoc+"/IntermediateToI2B2.xsl"));
			i2b2.setParameter("serviceurl", MyProps.getInstance().baseURL);
			i2b2.setParameter("fullquery", MyProps.getInstance().fullI2B2);
			i2b2.setParameter("rootkey", MyProps.getInstance().rootKey);
			i2b2.setParameter("subkey-age", MyProps.getInstance().subkeyAge);
			i2b2.setErrorListener(new ProcessorErrorHandler());
		} catch (TransformerConfigurationException e) {
			Logger.getLogger("jklann.hqmf.Processors").log(Level.SEVERE,"Could not load toi2b2.xsl! "+e.getMessageAndLocation());
		}
	}
	
	public String generateRequest(String user, String project, String domain, String password, boolean isToken, String key, String requestType) {
		StringBuilder request = new StringBuilder(header);
		request.append("<security>");
		request.append("<domain>");
		request.append(domain);
		request.append("</domain><username>");
		request.append(user);
		request.append("</username><password token_ms_timeout='1800000' is_token=");
		if (isToken) request.append("'true'>"); else request.append("'false'>");
		request.append(password);
		request.append("</password></security><project_id>");
		request.append(project);
		request.append("</project_id></message_header>");
		if (requestType.equals("getTermInfo")) {
			request.append("<message_body><ns4:get_term_info blob='true' type='core' synonyms='true' hiddens='true'><self>");
			request.append(key);
			request.append("</self></ns4:get_term_info></message_body>");
		} else if (requestType.equals("getCodeInfo")) {
			request.append("<message_body><ns4:get_code_info hiddens='true' synonyms='true' type='core' blob='false'><match_str strategy='exact'>");
			request.append(key);
			request.append("</match_str></ns4:get_code_info></message_body>");
		} else if (requestType.equals("getChildren")) {
			request.append("<message_body><ns4:get_children hiddens='true' synonyms='true' type='core' blob='false'><parent>");
			request.append(key);
			request.append("</parent></ns4:get_children></message_body>");
		}
		request.append("</ns3:request>");
		//System.out.println(request.toString());
		return request.toString();
	}
	
	private StringBuilder header;
	
	javax.xml.transform.TransformerFactory tFactory;
	public Transformer hqmf, i2b2, ihqmf, i2b2plus;
	public Client httpClient = null;
	public WebResource getTermInfo = null, getCodeInfo = null, getChildren = null;

}
