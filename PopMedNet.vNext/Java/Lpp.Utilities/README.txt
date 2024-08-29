README

Contents
========

README.txt		- This file
crypto.jar		- Jar file containing the class library
src/			- Source code
doc/			- Javadoc
tests/			- JUnit tests that can be used as examples

Crypto.class is the entry point class. All other classes support it.


Java JDK Stronger Encryption
============================

By default, Java JDK is released with a 128 bit encryption strength. The code provided here assumes a 256 bit key strength,
so JDK has to be upgraded to support that. Note that there is a USA export restriction on stronger encryption JDK.

To enable strong encryption in your JDK, go to:

http://www.oracle.com/technetwork/java/javase/downloads/jce-7-download-432124.html

Agree to the terms and download and explode the zip file provided. Copy the content to:

$JRE_HOME/lib/security

