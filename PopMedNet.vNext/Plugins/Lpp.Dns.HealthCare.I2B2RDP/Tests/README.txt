1. Get Server Certificates

   Hits server providing SSL certificates and list their certificates and contents. Host is hardcoded to BI.
   Useful for checking connection is ok and that we can get hold of the certificates before delving into whether
   the certificates are valid or not.
   
2. HttpsConnectionTests

   JUnit tests. Uses Apache's httpclient to check if connection can be made to the server.
   
3. Jersey Test

   Written using Jersey. Sends and receives i2b2 ontology or query messages. Allows sending of known valid 
   messages (in XML file).
   
   Syntax: JersyTest <message> [Ont|Qry]
   
   