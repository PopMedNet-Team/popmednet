<?php

///*
// * Copyright Michiel Post
// * http://www.michielpost.nl
// * contact@michielpost.nl
// * */

//Get variables
$filename = isset($_GET["file"]) ? $_GET["file"] : "";
$parameters = isset($_GET["param"]) ? $_GET["param"] : "";
$lastChunk = isset($_GET["last"]) ? strtolower($_GET["last"]) == "true" ? true : false : true;
$firstChunk = isset($_GET["first"]) ? strtolower($_GET["first"]) == "true" ? true : false : false;
$offset = isset($_GET["offset"]) ? (int)$_GET["offset"] : 0;

//Set the file path
$filePath = "./Upload/" . $filename;

//Actions for first chunk
if($firstChunk)
{
	if(file_exists($filePath))
	{
		//Delete file if the file already exists
		unlink($filePath);		
	}	
}


//Write the file content (for all chunks)	
if($offset > 0 && file_exists($filePath))
{
  //Append to existing file
	$file = fopen($filePath,"a");
}
else
{
  //Write new file
	$file = fopen($filePath,"w");
}

//Get posted file data
$input = fopen("php://input", "r");

if($file != null && $input != null)
{
  //Write data to file
	while ($data = fread($input, 1024))
  		fwrite($file, $data);

	fclose($file);
	fclose($input);
}


//Finish up
if($lastChunk )
{	
	//You have access to the parameters in $parameters and the filename in $filename
}


?>
