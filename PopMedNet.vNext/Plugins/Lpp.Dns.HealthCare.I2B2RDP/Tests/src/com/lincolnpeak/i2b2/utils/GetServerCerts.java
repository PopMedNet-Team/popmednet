package com.lincolnpeak.i2b2.utils;

/**
 * @Author Kushal Paudyal
 * www.sanjaal.com/java
 * Last Modified On: 2009-10-05
 *
 * A class that reads SSL Certificate from a SSL Server
 * and then prints some basic details.
 */
import java.security.cert.Certificate;

import javax.net.ssl.HttpsURLConnection;
import javax.net.ssl.SSLSocket;
import javax.net.ssl.SSLSocketFactory;

public class GetServerCerts {

	public static void main(String[] argv) throws Exception {

		/**
		 * 443 is the network port number used by the SSL https: URi scheme.
		 */
		int port = 443;

		String hostname = "popmednet.bidmc.harvard.edu";

		SSLSocketFactory factory = HttpsURLConnection
				.getDefaultSSLSocketFactory();

		System.out.println("Creating a SSL Socket For " + hostname
				+ " on port " + port);

		SSLSocket socket = (SSLSocket) factory.createSocket(hostname, port);

		/**
		 * Starts an SSL handshake on this connection. Common reasons include a
		 * need to use new encryption keys, to change cipher suites, or to
		 * initiate a new session. To force complete reauthentication, the
		 * current session could be invalidated before starting this handshake.
		 * If data has already been sent on the connection, it continues to flow
		 * during this handshake. When the handshake completes, this will be
		 * signaled with an event. This method is synchronous for the initial
		 * handshake on a connection and returns when the negotiated handshake
		 * is complete. Some protocols may not support multiple handshakes on an
		 * existing socket and may throw an IOException.
		 */

		socket.startHandshake();
		System.out.println("Handshaking Complete");

		/**
		 * Retrieve the server's certificate chain
		 * 
		 * Returns the identity of the peer which was established as part of
		 * defining the session. Note: This method can be used only when using
		 * certificate-based cipher suites; using it with non-certificate-based
		 * cipher suites, such as Kerberos, will throw an
		 * SSLPeerUnverifiedException.
		 * 
		 * 
		 * Returns: an ordered array of peer certificates, with the peer's own
		 * certificate first followed by any certificate authorities.
		 */
		Certificate[] serverCerts = socket.getSession().getPeerCertificates();
		System.out.println("Retreived Server's Certificate Chain");

		System.out.println(serverCerts.length + "Certifcates Found\n\n\n");
		for (int i = 0; i < serverCerts.length; i++) {
			Certificate myCert = serverCerts[i];
			System.out.println("====Certificate:" + (i + 1) + "====");
			System.out.println("-Public Key-\n" + myCert.getPublicKey());
			System.out.println("-Certificate Type-\n " + myCert.getType());

			System.out.println();
		}

		socket.close();
	}

	/*
	 * SANJAAL CORPS MAKES NO REPRESENTATIONS OR WARRANTIES ABOUT THE
	 * SUITABILITY OF THE SOFTWARE, EITHER EXPRESS OR IMPLIED, INCLUDING BUT NOT
	 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY, FITNESS FOR A
	 * PARTICULAR PURPOSE, OR NON-INFRINGEMENT. SANJAAL CORPS SHALL NOT BE
	 * LIABLE FOR ANY DAMAGES SUFFERED BY LICENSEE AS A RESULT OF USING,
	 * MODIFYING OR DISTRIBUTING THIS SOFTWARE OR ITS DERIVATIVES.
	 * 
	 * THIS SOFTWARE IS NOT DESIGNED OR INTENDED FOR USE OR RESALE AS ON-LINE
	 * CONTROL EQUIPMENT IN HAZARDOUS ENVIRONMENTS REQUIRING FAIL-SAFE
	 * PERFORMANCE, SUCH AS IN THE OPERATION OF NUCLEAR FACILITIES, AIRCRAFT
	 * NAVIGATION OR COMMUNICATION SYSTEMS, AIR TRAFFIC CONTROL, DIRECT LIFE
	 * SUPPORT MACHINES, OR WEAPONS SYSTEMS, IN WHICH THE FAILURE OF THE
	 * SOFTWARE COULD LEAD DIRECTLY TO DEATH, PERSONAL INJURY, OR SEVERE
	 * PHYSICAL OR ENVIRONMENTAL DAMAGE ("HIGH RISK ACTIVITIES"). SANJAAL CORPS
	 * SPECIFICALLY DISCLAIMS ANY EXPRESS OR IMPLIED WARRANTY OF FITNESS FOR
	 * HIGH RISK ACTIVITIES.
	 */
}