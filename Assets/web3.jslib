mergeInto(LibraryManager.library, {
    WalletAddress: function(){
        var returnStr;
        try{
            returnStr = web3.currentProvider.selectedAddress;
        }catch(e){
            returnStr = "";
        }

        var buffersize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(buffersize);
        stringToUTF8(returnStr, buffer, buffersize);
        return buffer;
    },
});