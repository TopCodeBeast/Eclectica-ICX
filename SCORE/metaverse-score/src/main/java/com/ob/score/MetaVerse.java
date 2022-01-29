package com.ob.score;

import score.Address;
import score.Context;
import score.DictDB;
import score.VarDB;
import score.annotation.External;
import score.annotation.Payable;

import com.ob.score.IRC3Basic;

import java.math.BigInteger;

public class MetaVerse extends IRC3Basic {
    

    private final DictDB<BigInteger,String> tokenURIs;
    private final DictDB<BigInteger,BigInteger> tokenPrices;
    private final VarDB<String> metaUri;
    private final VarDB<BigInteger> tokenIndex;


    public MetaVerse(String _name, String _symbol, String _Uri) {
        super(_name, _symbol);

        this.metaUri = Context.newVarDB("MetaVerseURI", String.class);
        this.metaUri.set(_Uri);

        this.tokenIndex = Context.newVarDB("TokenIndex", BigInteger.class);

        this.tokenURIs = Context.newDictDB("TokenURIs", String.class);
        this.tokenPrices = Context.newDictDB("TokenPrices", BigInteger.class);
    }

    @External(readonly=true)
    public String getMetaverseInfo(){
        return this.metaUri.getOrDefault("");
    }

    @External(readonly=true)
    public BigInteger getTokenIndex(){
        return this.tokenIndex.getOrDefault(BigInteger.ZERO);
    }

    @External(readonly=true)
    public BigInteger isNFTSold(BigInteger _tokenId){
        Address owner = ownerOf(_tokenId);
        return owner.equals(Context.getOwner()) ? BigInteger.ZERO:BigInteger.ONE;
    }

    @External(readonly=true)
    public String getNFTUri(BigInteger _tokenId){
        return this.tokenURIs.getOrDefault(_tokenId,"");
    }

    @External(readonly=true)
    public BigInteger getNFTPrice(BigInteger _tokenId){
        return this.tokenPrices.getOrDefault(_tokenId,BigInteger.ZERO);
    }

    @External
    public void approveTransfer(BigInteger _tokenId) {
        Address owner = ownerOf(_tokenId);
        Context.require(Context.getCaller().equals(owner));
        this.approve(Context.getAddress(),_tokenId);
    }

    @External
    public void updateNFTPrice(BigInteger _tokenId, BigInteger price) {
        Address owner = ownerOf(_tokenId);
        Context.require(Context.getCaller().equals(owner));
        this.tokenPrices.set(_tokenId, price);
    }

    @External
    @Payable
    public void buyNft(BigInteger _tokenId) {
        Address owner = ownerOf(_tokenId);
        Context.require(!owner.equals(Context.getCaller()));

        BigInteger price = this.tokenPrices.get(_tokenId);
        Context.require(Context.getValue().compareTo(price)!=-1);
        

        Context.transfer(owner, Context.getValue());
        this.transferFrom(owner, Context.getCaller(), _tokenId);
    }

    @External
    public BigInteger mint(String _tokenUri, BigInteger price) {
        
        Context.require(Context.getCaller().equals(Context.getOwner()));

        BigInteger currentIndex = this.tokenIndex.getOrDefault(BigInteger.ZERO);
        super._mint(Context.getCaller(), currentIndex);
        this.tokenURIs.set(currentIndex,_tokenUri);
        this.tokenPrices.set(currentIndex, price);

        this.tokenIndex.set(currentIndex.add(BigInteger.ONE));
        
        return currentIndex;
    }

    @External
    public void burn(BigInteger _tokenId) {
        // simple access control - only the owner of token can burn it
        Address owner = ownerOf(_tokenId);
        Context.require(Context.getCaller().equals(owner));
        super._burn(_tokenId);
    }
}
