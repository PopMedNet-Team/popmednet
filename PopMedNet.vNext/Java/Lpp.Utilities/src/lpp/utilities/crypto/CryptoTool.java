package lpp.utilities.crypto;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.security.GeneralSecurityException;
import java.security.Key;

import javax.crypto.Cipher;
import javax.crypto.spec.IvParameterSpec;
import javax.xml.bind.DatatypeConverter;

/**
 * Wraps JCE for easier usage.
 * 
 * @author Daniel Dee
 * @version 1.0
 */
public class CryptoTool
{
	private static CryptoTool crypto = null;

	private static Cipher cipher = null;

	private static Key _key = null;

	private static IvParameterSpec _iv = null;

	/**
	 * Private constructor. Crypto is a singleton. Use getCrypto() for an
	 * instance.
	 */
	private CryptoTool()
	{
	}

	/**
	 * Crypto is a singleton. This method calls the constructor to create one if
	 * this is the first usage, or returns an existing one.
	 * 
	 * @return a Crypto instance
	 * @throws InvalidCipherAlgorithmException
	 * @throws CipherAlgorithmNotSpecifiedException
	 */
	public static CryptoTool getCrypto() throws InvalidCipherAlgorithmException,
	        CipherAlgorithmNotSpecifiedException
	{
		if (crypto == null)
			crypto = new CryptoTool();
		return crypto;
	}

	/**
	 * Changes the cipher from the default set in Crypto.properties.
	 * 
	 * @param cipherName
	 * @throws InvalidCipherAlgorithmException
	 * @throws CipherAlgorithmNotSpecifiedException
	 */
	public void setCipher(String cipherName)
	        throws InvalidCipherAlgorithmException,
	        CipherAlgorithmNotSpecifiedException
	{
		try
		{
			cipher = Cipher.getInstance(cipherName);
		} catch (GeneralSecurityException e)
		{
			throw new InvalidCipherAlgorithmException(e);
		}
	}

	/**
	 * Gets the key used by the cipher.
	 * 
	 * @return Key
	 * @throws CannotLoadKeyFileException
	 */
	public Key getKey() throws CannotLoadKeyFileException
	{
		return _key;
	}

	/**
	 * Changes the key used by the cipher.
	 * 
	 * @param key
	 */
	public void setKey(Key key)
	{
		_key = key;
	}

	/**
	 * Gets the Initialization Vector (IV) if any used by this cipher.
	 * 
	 * @return IvParameterSpec
	 */
	public IvParameterSpec getIV()
	{
		return _iv;
	}

	/**
	 * Sets the Initialization Vector (IV) if desired or necessary.
	 * 
	 * @param iv
	 */
	public void setIV(byte[] iv)
	{
		if (iv == null)
		{
			_iv = null;
			return;
		}
		IvParameterSpec ivSpec = new IvParameterSpec(iv);
		_iv = ivSpec;
	}

	/**
	 * Encrypts a String. If an IV file is set, it will be used instead of any
	 * internal one. The key is loaded from the keyfile if one is not set.
	 * 
	 * @param string
	 * @return encrypted string
	 */
	public String encrypt(String string)
	{
		return new String(encrypt(string.getBytes()));
	}

	/**
	 * Encrypts a message byte array. If an IV file is set, it will be used
	 * instead of any internal one. The key is loaded from the keyfile if one is
	 * not set.
	 * 
	 * @param message
	 *            byte array
	 * @return an encrypted message byte array
	 */
	public byte[] encrypt(byte[] message)
	{
		try
		{
			if (_key == null)
				return null;
			if (_iv == null)
				cipher.init(Cipher.ENCRYPT_MODE, _key);
			else
				cipher.init(Cipher.ENCRYPT_MODE, _key, _iv);
			return cipher.doFinal(message);
		} catch (Exception e)
		{
			return null;
		}
	}

	/**
	 * Decrypts an encrypted String. If an IV file is set, it will be used
	 * instead of any internal one. The key is loaded from the keyfile if one is
	 * not set.
	 * 
	 * @param encryptedString
	 * @return decrypted string
	 */
	public String decrypt(String encryptedString)
	{
		return new String(decrypt(encryptedString.getBytes()));
	}

	/**
	 * Decrypts an encrypted message byte array. If an IV file is set, it will
	 * be used instead of any internal one. The key is loaded from the keyfile
	 * if one is not set.
	 * 
	 * @param encryptedMessage
	 * @return decrypted byte array
	 */
	public byte[] decrypt(byte[] encryptedMessage)
	{
		try
		{
			if (_key == null)
				return null;
			if (_iv == null)
				cipher.init(Cipher.DECRYPT_MODE, _key);
			else
				cipher.init(Cipher.DECRYPT_MODE, _key, _iv);
			return cipher.doFinal(encryptedMessage);
		} catch (Exception e)
		{
			System.err.println(e);
			return null;
		}
	}

	/**
	 * Encrypts a String prepended with IV and padded to 16 byte boundary.
	 * 
	 * @param plainText String to be encrypted
	 * @return an encrypted string
	 */
	public String encryptAES(String plainText)
	{
		// Encrypt the entire message and padding.
		byte[] encryptedBytes = encrypt(plainText).getBytes();
		int numEncryptedBytes = encryptedBytes.length;

		// Allocate sufficient space to store 4 bytes for the length of the IV,
		// the IV and the encrypted string.
		byte[] iv = cipher.getIV();
		int ivLength = iv.length;
		byte[] encryptedBytesWithIV = new byte[numEncryptedBytes + ivLength + 4];

		// Prepend the IV to the encrypted message+padding and save into the
		// newly allocated space.
		ByteBuffer dbuf = ByteBuffer.allocate(4);
		dbuf.order(ByteOrder.LITTLE_ENDIAN);
		dbuf.putInt(ivLength);
		System.arraycopy(dbuf.array(), 0, encryptedBytesWithIV, 0, 4);
		System.arraycopy(iv, 0, encryptedBytesWithIV, 4, ivLength);
		System.arraycopy(encryptedBytes, 0, encryptedBytesWithIV, 4 + ivLength,
		        numEncryptedBytes);

		// Base64 the encrypted string and return it.
		return DatatypeConverter.printBase64Binary(encryptedBytesWithIV);
	}

	/**
	 * Decrypts a hexed String prepended with the IV.
	 * 
	 * @param cipherText Encrypted string
	 * @return decrypted String
	 */
	public String decryptAES(String cipherText)
	{
		// De-base64 encrypted String first.
		byte[] encryptedBytesWithIV = DatatypeConverter
		        .parseBase64Binary(cipherText);

		// Read first 4 bytes. That stores the IV length.
		ByteBuffer wrapped = ByteBuffer.wrap(encryptedBytesWithIV);
		wrapped.order(ByteOrder.LITTLE_ENDIAN);
		int ivLength = wrapped.getInt(0); // reads 4 bytes starting at 0.

		// Allocate space to store the encrypted string, stripping the prepended
		// IV.
		int numEncryptedBytes = encryptedBytesWithIV.length - ivLength - 4; // includes
																			// count
																			// of
																			// 4
																			// bytes
		byte[] encryptedBytes = new byte[numEncryptedBytes];
		byte[] iv = new byte[ivLength];

		// Read the iv.
		System.arraycopy(encryptedBytesWithIV, 4, iv, 0, ivLength);
		System.arraycopy(encryptedBytesWithIV, ivLength + 4, encryptedBytes, 0,
		        numEncryptedBytes);

		// Tell Crypto what the IV is, so that it can decrypt this string
		// properly.
		crypto.setIV(iv);

		// Decrypt the String. Strip the padded bytes (the last byte determines
		// the number of padded bytes). Then return the decrypted String.
		byte[] decryptedBytes = crypto.decrypt(encryptedBytes);
		return new String(decryptedBytes);
	}

}
