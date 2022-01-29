# FrontEnd for Art Gallery Template

This contains the code for various frontend which includes the admin panel for the art gallery and the interface to deploy your art gallery 

## Details about the folders 

hackathonmeta [Here](hackathonmeta) : Contains the Unity build (Compiled WASM code) and Admin panel <br />
script [Here](script) : Contains various scripts for supporting deployment on IPFS

## Brief info on scripts 

Ipfs-helper [Here](script/ipfs-helper.js) : Provides functions to upload all the files to the ipfs node <br />
nftstorage.js [Here](script/nftstorage.js) : Provides function to upload initial metadata to nft.storage

## How to run 

- Simply run an http server in the current folder 
```bash
python3 -m http.server
```

## Warnings 

- The current build in the hackathonmeta [Here](hackathonmeta) has hardcoded endpoints , so before running this 
  make sure to replace it with your build , instructions are give [Here](../ArtGallery%20Template)



