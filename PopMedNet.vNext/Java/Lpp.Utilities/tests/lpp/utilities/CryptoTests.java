package lpp.utilities;

import static org.junit.Assert.assertEquals;
import static org.junit.Assert.fail;

import org.junit.Test;

public class CryptoTests
{
	public void decryptCSharpEncryptedPassword()
	{
		try
		{
	        String plainText = "SystemAdministrator:Password1!";
	        String sharedSecret = "ytfnNpyEuXSmrN5x2HYIkkAAv0we27Ljnuw6oTq+6NI="; // oAuthHash
	        String salt = "altappsso"; // oAuthKey
	        String encrypted = "EAAAABOVMHo54ap9LhtyvIpBapFE13emmfe/o+k/u6OU3EQf9O6DG96xWpQQ8/iqPkw1Pw==";
//	        encrypted = Crypto.encryptStringAES(plainText, sharedSecret, salt);
	        String decrypted = Crypto.decryptStringAES(encrypted, sharedSecret, salt);
	        assertEquals(plainText, decrypted);
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
	        String plainText = "Hello World";
	        String sharedSecret = "Password1!";
	        String salt = "abcd1234";
	        String encrypted = Crypto.encryptStringAES(plainText, sharedSecret, salt);
	        String decrypted = Crypto.decryptStringAES(encrypted, sharedSecret, salt);
	        assertEquals(plainText, decrypted);
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
			String plainText = "Hello World";
	        String cipherText = "EAAAALbw7SVS58Uj04FK2dr+sJfB8TIHE8gH43vmHXWuWZZh";
	        String sharedSecret = "Password1!";
	        String salt = "abcd1234";
	        String decrypted = Crypto.decryptStringAES(cipherText, sharedSecret, salt);
	        assertEquals(plainText, decrypted);
		}
		catch(Exception e)
		{
			fail(e.toString());
		}
	}
	
	@Test
	public void decryptMany()
	{
        String[] plaintexts = { "Hello World", "Humpty Dumpty has a great fall", "Jack be nimble, Jack be quick", "To be or not to be, that is the question" };
		String[] tokens = { "EAAAAFSPjJ0a1R/8yKsbds2RnG0+pSjOeSwvzGZgRYR6Kihm", 
							"EAAAADfCDjIbsFdw9KllHq7q4tCv+FN6je8SForXW06MjGkz6O/LQhcFvt0i1pmIQ1+tLw==", 
							"EAAAAA0mZ1J2hpCzxnlMe2CAdE1952tK3UfTl5AkRNVQCtehIARxyn+d3hf1mlQIaZ7zaA==", 
							"EAAAABhHvi+KFHdYIn1KS2Ef/jzvMuiiJgjQ+Z0MZVkd5eu5n5qYPWwDaVyhp2opdk/Flz7/RWgu+bArfyhQKm20CqU=" };
		
		try
		{
	        String sharedSecret = "Password1!";
	        String salt = "abcd1234";
	        
	        for(int i=0; i < tokens.length; i++)
	        {
	        	String token = tokens[i];
	        	String plaintext = plaintexts[i];
	        	String decrypted = Crypto.decryptStringAES(token, sharedSecret, salt);
	        	assertEquals(plaintext, decrypted);
	        }
		}
		catch(Exception e)
		{
			fail(e.toString());
		}
	}
}
