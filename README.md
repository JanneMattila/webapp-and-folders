# Web app and filesystem perf testing

## Build Status

[![Build Status](https://dev.azure.com/jannemattila/jannemattila/_apis/build/status/JanneMattila.326-webapp-and-folders?branchName=master)](https://dev.azure.com/jannemattila/jannemattila/_build/latest?definitionId=45&branchName=master)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## Download Blob via streaming

Generate large files and upload them to Blob.
You can use Azure Cloud Shell so you don't 
have to upload the files to the cloud.

```bash
mkdir blobtemp
cd blobtemp

head -c 500MB </dev/urandom > 500MB.bin
head -c 1000MB </dev/urandom > 1000MB.bin

sha256sum 500MB.bin > 500MB.bin.sha256
sha256sum 1000MB.bin > 1000MB.bin.sha256

cat 500MB.bin.sha256
cat 1000MB.bin.sha256

azcopy copy /home/user/blobtemp "https://<accountname>.blob.core.windows.net/demo1?<sastoken>" --recursive
```

Download the files using the API:

```powershell
# Localhost version
curl --request GET --url 'https://localhost:5001/api/blob?container=demo1&path=blobtemp/500MB.bin' --output 500MB.bin
curl --request GET --url 'https://localhost:5001/api/blob?container=demo1&path=blobtemp/500MB.bin.256' --output 500MB.bin.sha256

# App Service version
curl --request GET --url 'https://webappfileblob-abc123.northeurope-01.azurewebsites.net/api/blob?container=demo1&path=blobtemp/500MB.bin' --output 500MB.bin
curl --request GET --url 'https://webappfileblob-abc123.northeurope-01.azurewebsites.net/api/blob?container=demo1&path=blobtemp/500MB.bin.256' --output 500MB.bin.sha256

# Check the hash
cat 500MB.bin.sha256
Get-FileHash -Path 500MB.bin -Algorithm SHA256
```

## Upload file to blob using web page Blob via streaming

Web page chunks the file and sends it to the server.

```
https://localhost:5001/upload.html
```
