package jklann.hqmf;

import java.io.StringReader;
import java.io.StringWriter;
import java.util.logging.Level;
import java.util.logging.Logger;

import javax.xml.transform.ErrorListener;
import javax.xml.transform.Source;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.TransformerFactoryConfigurationError;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;

/** Jeff Klann - 7/2012
 * 
 * Error handler for Xalan transforms. Logs warnings and errors to the Java logger. When a fatal error 
 * or error is encountered, an exception is thrown with the text of that error and the warning just prior.
 * Keeps a global log of warnings and errors and the last fatal error, for return and display. TODO: this 
 * is probably silly and a memory hog.
 * 
 * @author jklann
 *
 */

public class ProcessorErrorHandler implements ErrorListener {

	static StringBuilder log;
	static Transformer identity;
	static String lastFatality = "";
	static int lastFatalCode = 500;
	static Logger logger;
	
	static {
		logger = Logger.getLogger("jklann.hqmf.ProcessorErrorHandler");
		log = new StringBuilder();
		log.append("<?xml version='1.0'?><errors>");
		try {
			identity = TransformerFactory.newInstance().newTransformer();
		} catch (TransformerConfigurationException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (TransformerFactoryConfigurationError e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	String lastwarning = "<warning></warning>";
	
	public static String getLog() {
		return webifyXML(log.toString()+"</errors>");
	}
	
	public static String webifyXML(String input) {
		System.out.println(input);
		Source xmlInput = new StreamSource(new StringReader(input));
		StreamResult xmlOutput = new StreamResult(new StringWriter());
		try {
			identity.transform(xmlInput, xmlOutput);
		} catch (TransformerException e) {
			// TODO Auto-generated catch block
			logger.log(Level.WARNING,"Error webifying log: "+e.getMessage());
		}
		return xmlOutput.getWriter().toString();		
	}
	
	public void error(TransformerException arg0) throws TransformerException {
		log.append("<error>"+arg0.getMessage()+"</error>");
		
		lastFatality = webifyXML("<errors>"+lastwarning+"<error>"+arg0.getMessageAndLocation()+"</error>"+"</errors>");
		//System.err.println(lastFatality);
		logger.log(Level.SEVERE,"Error during transform",lastFatality);
		throw arg0;

	}

	public void fatalError(TransformerException arg0)
			throws TransformerException {
		log.append("<fatalerror>"+arg0.getMessageAndLocation()+"</fatalerror>");
		
		lastFatality = webifyXML("<errors>"+lastwarning+"<fatalerror>"+arg0.getMessageAndLocation()+"</fatalerror>"+"</errors>");
		//System.err.println(lastFatality);
		logger.log(Level.SEVERE,"Fatal error during transform",lastFatality);
		throw arg0;
	}

	public void warning(TransformerException arg0) throws TransformerException {
		lastwarning="<warning>"+arg0.getMessage()+"</warning>";
		// Fatal warnings by convention begin with (xxx) http status
		try { 
			lastFatalCode = Integer.parseInt(arg0.getMessage().substring(1,4));
		} catch(Exception e) { lastFatalCode = 500; }
		log.append(lastwarning);
		//System.err.println(lastwarning);
		logger.log(Level.WARNING,"Warning during transform",lastwarning);
	}

}
