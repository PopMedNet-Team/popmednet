package lpp.utilities;

import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;

import lpp.utilities.crypto.CryptoTool;

/**
 * Cryptographic utility to encrypt and decrypt a string using AES/CBC cipher with PKCS5 padding.
 * The key uses the AES PBKDF2WithHmacSHA1 algorithm with an iteration of 1000.
 * The key size is assumed to be 256 and is hardcoded.
 * 
 * @author Daniel
 *
 */
public class Crypto
{
	private static String cipherName = "AES/CBC/PKCS5Padding";
	private static String algorithm = "PBKDF2WithHmacSHA1";
	private static String keyAlgorithm = "AES";
	private static int iteration = 1000;
	private static int keySize = 256;
	
	/**
	 * Encrypt the given string using AES.  The string can be decrypted using
     * DecryptStringAES().  The sharedSecret and salt parameters must match.
	 * @param plainText The text to encrypt.
	 * @param sharedSecret A password used to generate a key for encryption.
	 * @param salt Random data to salt the sharedSecret.
	 * @return Encrypted text
	 * @throws ArgumentNullException 
	 */
    public static String encryptStringAES(String plainText, String sharedSecret, String salt) throws ArgumentNullException
    {
        if (plainText == null || plainText.length() == 0)
            throw new ArgumentNullException("plainText");
        if (sharedSecret == null || sharedSecret.length() == 0)
            throw new ArgumentNullException("sharedSecret");
        if (salt == null || salt.length() == 0)
            throw new ArgumentNullException("salt");

        byte[] _salt = salt.getBytes();

        String outStr = null;                 // Encrypted string to return
        CryptoTool aesAlg = null;        // RijndaelManaged object used to encrypt the data.

        try
        {         
            // Create a RijndaelManaged object
            aesAlg = CryptoTool.getCrypto();
            aesAlg.setCipher(cipherName);
            aesAlg.setIV(null);

            // generate the key from the shared secret and the salt
        	PBEKeySpec keySpec = new PBEKeySpec(sharedSecret.toCharArray(), _salt, iteration, keySize);
            SecretKeyFactory factory = SecretKeyFactory.getInstance(algorithm);
            aesAlg.setKey(new SecretKeySpec(factory.generateSecret(keySpec).getEncoded(), keyAlgorithm));
            
            // Create a decryptor to perform the stream transform.
            // Create the streams used for encryption.
            outStr = aesAlg.encryptAES(plainText);
        }
        catch(Exception e)
        {
            throw new SecurityException(e.toString());
        }

        // Return the encrypted bytes from the memory stream.
        return outStr;
    }
    
	/**
	 * Decrypt the given string.  Assumes the string was encrypted using
     * EncryptStringAES(), using an identical sharedSecret.
	 * @param cipherText The text to decrypt.
	 * @param sharedSecret A password used to generate a key for decryption.
	 * @param salt Random data to salt the sharedSecret.
	 * @return Decrypted text
	 * @throws ArgumentNullException 
	 */
	public static String decryptStringAES(String cipherText, String sharedSecret, String salt) throws ArgumentNullException
	{
        if (cipherText == null || cipherText.length() == 0)
            throw new ArgumentNullException("cipherText");
        if (sharedSecret == null || sharedSecret.length() == 0)
            throw new ArgumentNullException("sharedSecret");
        if (salt == null || salt.length() == 0)
            throw new ArgumentNullException("salt");

        // Declare the RijndaelManaged object
        // used to decrypt the data.
        CryptoTool aesAlg = null;
        
        // Declare the string used to hold
        // the decrypted text.
        String plaintext = null;

        try
        {
            // Create a RijndaelManaged object
            // with the specified key and IV.
            aesAlg = CryptoTool.getCrypto();
            aesAlg.setCipher(cipherName);
            
            // generate the key from the shared secret and the salt
            // PBEKeySpec == Rfc2898DeriveBytes
        	PBEKeySpec keySpec = new PBEKeySpec(sharedSecret.toCharArray(), salt.getBytes(), iteration, keySize);
            SecretKeyFactory factory = SecretKeyFactory.getInstance(algorithm);            
			aesAlg.setKey(new SecretKeySpec(factory.generateSecret(keySpec).getEncoded(), keyAlgorithm));
	        
	        plaintext = aesAlg.decryptAES(cipherText);
        }
        catch(Exception e)
        {
            throw new SecurityException(e.toString());
        }

        return plaintext;
	}

	
}
