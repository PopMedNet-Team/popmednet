package jklann.hqmf;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/** 
 * Jeff Klann - 7/2012
 * A very simple properties container which loads and holds hqmf.properties.
 * 
 * @author jklann
 *
 */
public class MyProps {
	private static MyProps instance = null;
	
	public static MyProps getInstance() { 
		if (instance==null) instance = new MyProps();
		return instance;
	}

	String xslLoc;
	String ontLoc;
	String baseURL;
	String fullI2B2;
	String rootKey;
	String subkeyAge;
	
	protected MyProps() {
		reload();
	}
	public void reload() {
		Properties myProps = new Properties();

		InputStream inStream = null;
		inStream = Thread.currentThread().getContextClassLoader()
    		.getResourceAsStream("hqmf.properties");

		try {
			myProps.load(inStream);
		} catch (IOException e1) {
			// TODO Auto-generated catch block
			System.err.println("Could not load hqmf.properties!");
		} catch (NullPointerException e2) {
			System.err.println("Could not find hqmf.properties!");
		}
		xslLoc = myProps.getProperty("xslloc"); 
		ontLoc = myProps.getProperty("ontloc");
		baseURL = myProps.getProperty("baseurl");
		fullI2B2 = myProps.getProperty("fulli2b2");
		rootKey = myProps.getProperty("rootkey");
		subkeyAge = myProps.getProperty("subkey_age");
		System.out.println("rootkey is:"+rootKey);
		System.out.println("fulli2b2 is:"+fullI2B2+".");
	}
}
