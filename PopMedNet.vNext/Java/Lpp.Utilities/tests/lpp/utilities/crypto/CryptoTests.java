package lpp.utilities.crypto;

import static org.junit.Assert.*;

import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;

import org.junit.Test;

public class CryptoTests
{
	@Test
	public void encryptDecrypt()
	{
		try
		{
			String testString = "Hello World";
			String cipherName = "AES";
			String algorithm = "PBKDF2WithHmacSHA1";
			String keyAlgorithm = "AES";
			String sharedSecret = "Password1!";
			String salt = "abcd1234";
			int iteration = 1000;
			int keySize = 128;
			
			CryptoTool crypto = CryptoTool.getCrypto();
			crypto.setCipher(cipherName);
			crypto.setIV(null);
			
        	PBEKeySpec keySpec = new PBEKeySpec(sharedSecret.toCharArray(), salt.getBytes(), iteration, keySize);
            SecretKeyFactory factory = SecretKeyFactory.getInstance(algorithm);            
			crypto.setKey(new SecretKeySpec(factory.generateSecret(keySpec).getEncoded(), keyAlgorithm));

			assertEquals(testString, crypto.decrypt(crypto.encrypt(testString)));
		}
		catch(Exception e)
		{
			fail(e.toString());
		}
		
	}
	
	@Test
	public void encryptDecryptAES()
	{
		try
		{
			String testString = "Hello World";
			String cipherName = "AES/CBC/PKCS5Padding";
			String algorithm = "PBKDF2WithHmacSHA1";
			String keyAlgorithm = "AES";
			String sharedSecret = "Password1!";
			String salt = "abcd1234";
			int iteration = 1000;
			int keySize = 256;
			
			CryptoTool crypto = CryptoTool.getCrypto();
			crypto.setCipher(cipherName);
			
        	PBEKeySpec keySpec = new PBEKeySpec(sharedSecret.toCharArray(), salt.getBytes(), iteration, keySize);
            SecretKeyFactory factory = SecretKeyFactory.getInstance(algorithm);            
			crypto.setKey(new SecretKeySpec(factory.generateSecret(keySpec).getEncoded(), keyAlgorithm));

			assertEquals(testString, crypto.decryptAES(crypto.encryptAES(testString)));
		}
		catch(Exception e)
		{
			fail(e.toString());
		}
		
	}

	@Test
	public void decryptAES()
	{
		try
		{
			String testString = "Hello World";
			String cipherText = "EAAAALbw7SVS58Uj04FK2dr+sJfB8TIHE8gH43vmHXWuWZZh";
			String cipherName = "AES/CBC/PKCS5Padding";
			String algorithm = "PBKDF2WithHmacSHA1";
			String keyAlgorithm = "AES";
			String sharedSecret = "Password1!";
			String salt = "abcd1234";
			int iteration = 1000;
			int keySize = 256;
			
			CryptoTool crypto = CryptoTool.getCrypto();
			crypto.setCipher(cipherName);
			
        	PBEKeySpec keySpec = new PBEKeySpec(sharedSecret.toCharArray(), salt.getBytes(), iteration, keySize);
            SecretKeyFactory factory = SecretKeyFactory.getInstance(algorithm);            
			crypto.setKey(new SecretKeySpec(factory.generateSecret(keySpec).getEncoded(), keyAlgorithm));

			assertEquals(testString, crypto.decryptAES(cipherText));
		}
		catch(Exception e)
		{
			fail(e.toString());
		}
		
	}
	
}
